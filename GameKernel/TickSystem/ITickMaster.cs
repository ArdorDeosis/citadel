namespace GameKernel.TickSystem;

/// <summary>
/// Manager for ticking objects.
/// </summary>
public interface ITickMaster
{
  /// <summary>
  /// Registers the specified <see cref="ITicking"/> instance.
  /// </summary>
  void Register(ITicking instance);
  
  /// <summary>
  /// Unregisters the specified <see cref="ITicking"/> instance.
  /// </summary>
  void Unregister(ITicking instance);
  
  /// <summary>
  /// Starts the ticking.
  /// </summary>
  void StartTicking();
  
  /// <summary>
  /// Ends the ticking.
  /// </summary>
  void Stop();
}