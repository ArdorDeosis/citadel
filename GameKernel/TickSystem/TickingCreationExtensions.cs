using Ember.DependencyInjection;

namespace GameKernel.TickSystem;

public static class TickingCreationExtensions
{
  /// <summary>
  /// Creates an instance of type <typeparamref name="T"/> and registers it at the resolved <see cref="ITickMaster"/>.
  /// </summary>
  /// <param name="activator">The activator used to create the instance and resolve the <see cref="ITickMaster"/></param>
  public static T CreateTickingInstance<T>(this IActivator activator) where T : ITicking
  {
    var manager = activator.Resolve<ITickMaster>();
    var instance = activator.CreateInstance<T>();
    manager.Register(instance);
    return instance;
  }
}