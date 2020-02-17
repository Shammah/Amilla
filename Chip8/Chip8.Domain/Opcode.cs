using System.Collections.Generic;
using Amilla.Chip8.Domain.SeedWork;

namespace Amilla.Chip8.Domain
{
    /// <summary>
    /// Operation code that the machine can read and interpet.
    /// 
    /// The original implementation of the Chip-8 language includes 36 different
    /// instructions, including math, graphics, and flow control functions.
    /// 
    /// All instructions are 2 bytes long and are stored most-significant-byte first.
    /// In memory, the first byte of each instruction should be located at an even addresses.
    /// If a program includes sprite data, it should be padded so any instructions following
    /// it will be properly situated in RAM.
    /// </summary>
    public class Opcode : ValueObject
    {
        public Opcode(ushort op)
        {
            NNNN = op;
        }

        /// <summary>
        /// The entire 16-bit opcode.
        /// </summary>
        public ushort NNNN { get; }

        /// <summary>
        /// The lowest 12 bits of the instruction.
        /// Sometimes referred to as 'addr'.
        /// </summary>
        public ushort NNN => (ushort)(NNNN & 0xFFF);

        /// <summary>
        /// The lowest 8 bits of the instruction.
        /// Sometimes referred to as 'byte'.
        /// </summary>
        public byte NN => (byte)(NNNN & 0xFF);

        /// <summary>
        /// The same as <see cref="NN"/>.
        /// </summary>
        public byte KK => NN;

        /// <summary>
        /// A 4-bit value, the upper 4 bits of the high byte of the instruction.
        /// </summary>
        public byte U => (byte)((NNNN >> 12) & 0xF);

        /// <summary>
        /// A 4-bit value, the lower 4 bits of the high byte of the instruction.
        /// </summary>
        public byte X => (byte)((NNNN >> 8) & 0xF);

        /// <summary>
        /// A 4-bit value, the upper 4 bits of the low byte of the instruction.
        /// </summary>
        public byte Y => (byte)((NNNN >> 4) & 0xF);

        /// <summary>
        /// A 4-bit value, the lower 4 bits of the low byte of the instruction.
        /// Sometimes referred to as 'nibble'.
        /// </summary>
        public byte N => (byte)(NNNN & 0xF);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return NNNN;
        }
    }
}