using Fyremoss.DependencyInjection;

namespace GameKernel.TickSystem;

public static class Setup
{
  public static void SetupTickSystem(this IInjectorConfiguration configuration)
  {
    configuration.Bind<ITickMaster>().To<TickMaster>().AsSingleton();
    configuration.AddCreationHook((injector, instance) =>
    {
      if (instance is ITicking ticking) 
        injector.Resolve<ITickMaster>().Register(ticking);
    });
  }
}