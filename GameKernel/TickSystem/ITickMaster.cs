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
  
  /// <summary>
  /// Waits for the start of the next tick.
  /// </summary>
  /// <exception cref="InvalidOperationException">Thrown when this method is called on the tick thread.</exception>
  /// <remarks>
  /// This method is blocking execution on the current thread until the associated signal from the ITickMaster.
  /// </remarks>
  void WaitForTickStart();

  
  /// <summary>
  /// Waits for the end of the current tick.
  /// </summary>
  /// <exception cref="InvalidOperationException">Thrown when this method is called on the tick thread.</exception>
  /// <remarks>
  /// This method is blocking execution on the current thread until the associated signal from the ITickMaster.
  /// </remarks>
  void WaitForTickEnd();

  /// <summary>
  /// Waits for the end of the next (not started yet) tick.
  /// </summary>
  /// <exception cref="InvalidOperationException">Thrown when this method is called on the tick thread.</exception>
  /// <remarks>
  /// This method is blocking execution on the current thread until the associated signal from the ITickMaster.
  /// </remarks>
  void WaitForFullTick();
}