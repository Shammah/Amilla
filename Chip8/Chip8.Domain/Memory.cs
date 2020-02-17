using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Amilla.Chip8.Domain.Interfaces;
using Amilla.Chip8.Domain.SeedWork;

namespace Amilla.Chip8.Domain
{
    /// <summary>
    /// The memory storage of the Chip8 machine.
    ///
    /// The first 512 KiB of the RAM region is reserved for the interpreter itself,
    /// like its registers. Therefore, the programs that are loaded into memory usually
    /// start at memory address 0x200.
    /// 
    /// However, we dont have memory limitations, and thuse store the <see cref="State"/>
    /// at a different location. Hence, when using <see cref="Reset"/>, the state wont be reset.
    /// 
    /// We do use the first 512 KiB for the <see cref="Display.Font"/> however. This is so that
    /// the machine's <see cref="State.I"/> register can point to the font memory.
    /// </summary>
    public class Memory : ValueObject, IResettable, IEnumerable<byte>
    {
        /// <summary>
        /// Total size of the memory in bytes.
        /// </summary>
        public const int RAMSize = 4096;

        /// <summary>
        /// Starting location of the loaded program in memory.
        /// </summary>
        public const int Program = 0x200;

        /// <summary>
        /// Starting location of the font, loaded in memory.
        /// </summary>
        public const int Font = 0x0;

        public Memory()
        {
            RAM = new byte[RAMSize];

            Reset();
        }

        /// <summary>
        /// The entire RAM memory of the machine.
        /// </summary>
        public byte[] RAM { get; }

        /// <summary>
        /// Indexer function for quick access to the underlying RAM array.
        /// </summary>
        /// <returns>The value of the memory at a specific address.</returns>
        public byte this[int i]
        {
            get => RAM[i];
            set => RAM[i] = value;
        }

        public IEnumerator<byte> GetEnumerator() => RAM.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Loads the program into memory.
        /// </summary>
        /// <param name="program">The program data to put into memory.</param>
        public void LoadProgram(byte[] program)
        {
            if (program.Length >= RAMSize - Program)
                throw new ArgumentException($"ROM size ({program.Length} bytes) does not fit in memory ({RAMSize - Program} bytes)");

            program.CopyTo(RAM, Program);
        }

        /// <summary>
        /// Loads the font into memory.
        /// </summary>
        /// <param name="font">The font data to put into memory.</param>
        public void LoadFont(byte[] font)
        {
            if (font.Length >= Program)
                throw new ArgumentException($"Font size ({font.Length} bytes) does not fit in memory portion for fonts ({Program} bytes)");

            font.CopyTo(RAM, Font);
        }

        /// <summary>
        /// Resets the entire memory of the machine.
        /// </summary>
        public void Reset()
        {
            Array.Clear(RAM, 0, RAMSize);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            foreach (var mem in RAM)
                yield return mem;
        }
    }
}