using System.Collections.Concurrent;
using System.Diagnostics;

namespace GameKernel.TickSystem;

internal class TickMaster : ITickMaster
{
  private readonly Thread tickThread;
  private volatile bool isRunning;

  private readonly HashSet<WeakReference<ITicking>> references = [];
  private readonly ConcurrentBag<ITicking> pendingRegistration = [];
  private readonly ConcurrentBag<ITicking> pendingRemoval = [];

  private readonly Lock registrationLock = new();
  private readonly Lock removalLock = new();

  private readonly ErrorHandler? errorHandler;
  
  private readonly ManualResetEvent tickStartEvent = new(false);
  private readonly ManualResetEvent tickEndEvent = new(false);

  public TickMaster(ErrorHandler? errorHandler = null)
  {
    this.errorHandler = errorHandler;
    tickThread = new Thread(TickLoop) { Name = "Tick Thread" };
  }

  /// <inheritdoc />
  public void Register(ITicking instance)
  {
    using (registrationLock.EnterScope())
    {
      pendingRegistration.Add(instance);
    }
  }

  /// <inheritdoc />
  public void Unregister(ITicking instance)
  {
    using (removalLock.EnterScope())
    {
      pendingRemoval.Add(instance);
    }
  }

  /// <inheritdoc />
  public void StartTicking()
  {
    isRunning = true;
    tickThread.Start();
  }

  /// <inheritdoc />
  public void Stop()
  {
    isRunning = false;
    tickStartEvent.Set();
    tickEndEvent.Set();
    tickThread.Join();
  }

  /// <inheritdoc />
  public void WaitForTickStart()
  {
    ThrowIfOnTickThread();
    tickStartEvent.WaitOne();
  }

  /// <inheritdoc />  
  public void WaitForTickEnd()
  {
    ThrowIfOnTickThread();
    tickEndEvent.WaitOne();
  }

  /// <inheritdoc />
  public void WaitForFullTick()
  {
    ThrowIfOnTickThread();
    tickStartEvent.WaitOne();
    tickEndEvent.WaitOne();
  }

  /// <summary>
  /// The ticking loop function.
  /// </summary>
  private void TickLoop()
  {
    try
    {
      var stopwatch = Stopwatch.StartNew();
      while (isRunning)
      {
        var deltaTime = stopwatch.Elapsed.TotalSeconds;
        stopwatch.Restart();

        tickStartEvent.Pulse();

        TickInstances(deltaTime);
        AddPendingReferences();
        CleanupReferences();
        
        tickEndEvent.Pulse();
      }
    }
    catch (Exception exception)
    {
      Console.WriteLine(exception);
      throw;
    }
  }

  /// <summary>
  /// Calls the <see cref="ITicking.Tick"/> method on all registered <see cref="ITicking"/> instances.
  /// </summary>
  /// <param name="deltaTime"></param>
  private void TickInstances(double deltaTime)
  {
    foreach (var reference in references)
    {
      try
      {
        if (reference.TryGetTarget(out var instance))
          instance.Tick(deltaTime);
      }
      catch (Exception exception)
      {
        errorHandler?.Invoke(exception);
      }
    }
  }

  /// <summary>
  /// Adds all newly registered <see cref="ITicking"/> instances.
  /// </summary>
  private void AddPendingReferences()
  {
    using (registrationLock.EnterScope())
    {
      references.UnionWith(pendingRegistration.Select(instance => new WeakReference<ITicking>(instance)));
      pendingRegistration.Clear();
    }
  }

  /// <summary>
  /// Removes all dead references and all unregistered <see cref="ITicking"/> instances.
  /// </summary>
  private void CleanupReferences()
  {
    using (removalLock.EnterScope())
    {
      references.RemoveWhere(
        reference => !reference.TryGetTarget(out var instance) || pendingRemoval.Contains(instance));
      pendingRemoval.Clear();
    }
  }

  private void ThrowIfOnTickThread()
  {
    if (Thread.CurrentThread == tickThread)
      throw new InvalidOperationException("Can not wait for tick events on the tick thread.");
  }
}

file static class Extensions
{
  public static void Pulse(this ManualResetEvent resetEvent)
  {
    resetEvent.Set();
    resetEvent.Reset();
  }
}