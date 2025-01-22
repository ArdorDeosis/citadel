using System.ComponentModel;

namespace GameKernel.TickSystem;

public interface ITicking
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  protected internal void Tick(double deltaTime);
}