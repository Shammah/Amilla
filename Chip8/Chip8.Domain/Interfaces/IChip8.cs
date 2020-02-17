using System.Collections.Generic;
using System.IO;

namespace Amilla.Chip8.Domain.Interfaces
{
    public interface IChip8 : IResettable
    {
        /// <summary>
        /// Loads a ROM directly from memory.
        /// </summary>
        /// <param name="rom">The ROM in memory as a collection of bytes.</param>
        void LoadFromMemory(IReadOnlyCollection<byte> rom);

        /// <summary>
        /// Loads the ROM from a file on the operating system.
        /// </summary>
        /// <param name="file">
        /// The location of the ROM on the operating system,
        /// relative to the executable location.
        /// </param>
        void LoadFromFile(FileInfo file);

        /// <summary>
        /// Advances the emulator by one CPU cycle.
        /// </summary>
        void Tick();

        /// <returns>
        /// Whether a ROM is currently loaded.
        /// </returns>
        bool IsLoaded { get; }
    }
}