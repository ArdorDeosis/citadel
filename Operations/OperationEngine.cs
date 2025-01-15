using System.Reflection;
using Ember.DependencyInjection;

namespace Operations;

public class OperationEngine
{
  private readonly IActivator activator;
  private readonly Queue<IOperation> operations = new();
  
  public OperationEngine(IActivator activator)
  {
    this.activator = activator;
  }

  public void Execute<TOperation>() where TOperation : Operation => 
    operations.Enqueue(activator.CreateInstance<TOperation>());

  public void Execute<TOperation, TPayload>(TPayload payload) where TOperation : Operation<TPayload>
  {
    var operation = activator.CreateInstance<TOperation>();

    typeof(Operation<TPayload>).GetProperty(
      nameof(Operation<TPayload>.Payload),
      BindingFlags.Instance | BindingFlags.NonPublic
    )!.SetValue(operation, payload);
    
    operations.Enqueue(operation);
  }

  private bool TryExecute(Operation operation) => operation.Perform();
}