namespace GameKernel.TickSystem;

public interface ITicking
{
  protected internal void Tick(double deltaTime);
}