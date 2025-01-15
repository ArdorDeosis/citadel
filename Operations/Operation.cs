namespace Operations;

public interface IOperation
{
  internal bool Perform();
}

public abstract class Operation : IOperation
{
  public abstract bool Perform();
}

public abstract class Operation<TPayload> : IOperation
{
  protected internal TPayload Payload { get; protected init; } = default!;
  public abstract bool Perform();
}