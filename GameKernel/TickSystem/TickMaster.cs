using System.Collections.Concurrent;
using System.Diagnostics;

namespace GameKernel.TickSystem;

public class TickMaster : ITickMaster
{
  private readonly Thread tickThread;
  private volatile bool isRunning;
  
  private readonly HashSet<WeakReference<ITicking>> references = [];
  private readonly ConcurrentBag<ITicking> pendingRegistration = [];
  private readonly ConcurrentBag<ITicking> pendingRemoval = [];
  
  private readonly Lock registrationLock = new();
  private readonly Lock removalLock = new();

  private readonly Action<Exception>? ErrorHandler;

  public TickMaster(Action<Exception>? errorHandler = null)
  {
    ErrorHandler = errorHandler;
    tickThread = new Thread(TickLoop);
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
    tickThread.Join();
  }

  /// <summary>
  /// The ticking loop function.
  /// </summary>
  private void TickLoop()
  {
    var stopwatch = Stopwatch.StartNew();
    while (isRunning)
    {
      var deltaTime = stopwatch.Elapsed.TotalSeconds;
      stopwatch.Restart();

      TickInstances(deltaTime);
      AddPendingReferences();
      CleanupReferences();
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
        ErrorHandler?.Invoke(exception);
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
}