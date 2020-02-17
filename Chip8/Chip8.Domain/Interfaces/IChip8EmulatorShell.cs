namespace Amilla.Chip8.Domain.Interfaces
{
    public interface IChip8EmulatorShell : IEmulatorShell<IChip8Emulator>
    {
        void Start();
        void Stop();

        /// <summary>
        /// Whether the emulator is currently running or not.
        /// </summary>
        bool IsRunning { get; }
    }
}