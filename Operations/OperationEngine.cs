using System.Collections.Concurrent;
using Fyremoss.DependencyInjection;
using GameKernel;
using GameKernel.TickSystem;

namespace Operations;

/// <summary>
/// Executes operations thread-safe.
/// </summary>
public class OperationEngine : ITicking
{
  private readonly IInjector injector;
  private readonly ErrorHandler errorHandler;
  private readonly ConcurrentQueue<IOperation> operations = new();
  
  public OperationEngine(IInjector injector, ErrorHandler errorHandler)
  {
    this.injector = injector;
    this.errorHandler = errorHandler;
  }

  /// <summary>
  /// Executes the specified <paramref name="operation"/>.
  /// </summary>
  /// <param name="operation">The operation to execute.</param>
  public void Execute(IOperation operation)
  {
    injector.InjectProperties(operation);
    operations.Enqueue(operation);
  }

  /// <inheritdoc />
  public void Tick(double deltaTime)
  {
    while (operations.TryDequeue(out var operation))
    {
      try
      {
        operation.Perform();
      }
      catch (Exception exception)
      {
        errorHandler(exception);
      }
    }
  }
}