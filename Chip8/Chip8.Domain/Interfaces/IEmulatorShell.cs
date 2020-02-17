namespace Amilla.Chip8.Domain.Interfaces
{
    /// <summary>
    /// An interpreter shell is a means of communication for the emulator
    /// with the outside world; display, sound, input etc.
    /// </summary>
    public interface IEmulatorShell<E>
        where E : IEmulator
    {
        /// <summary>
        /// The emulator contained inside the shell.
        /// </summary>
        E Emulator { get; }
    }
}