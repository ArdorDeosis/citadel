namespace Operations;

/// <summary>
/// An operation to be executed by the <see cref="OperationEngine"/>.
/// </summary>
// TODO: this probably needs some form of feedback mechanism at some point
public interface IOperation
{
  /// <summary>
  /// Performs the operation.
  /// </summary>
  /// <returns></returns>
  internal void Perform();
}