namespace Amilla.Chip8.Domain.Interfaces
{
    /// <summary>
    /// An interpreter shell is a means of communication for the emulator
    /// with the outside world; display, sound, input etc.
    /// </summary>
    public interface IChip8Shell
    {
        /// <summary>
        /// The emulator contained inside the shell.
        /// </summary>
        IChip8 Emulator { get; }

        void Start();
        void Stop();

        /// <summary>
        /// Whether the emulator is currently running or not.
        /// </summary>
        bool IsRunning { get; }
    }
}