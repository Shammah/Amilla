using System;

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
    public struct Opcode : IEquatable<Opcode>, IEquatable<ushort>
    {
        public Opcode(ushort op)
        {
            this.NNNN = op;
        }

        /// <summary>
        /// The entire 16-bit opcode.
        /// </summary>
        public ushort NNNN { get; }

        /// <summary>
        /// The lowest 12 bits of the instruction.
        /// Sometimes referred to as 'addr'.
        /// </summary>
        public ushort NNN => (ushort)(this.NNNN & 0xFFF);

        /// <summary>
        /// The lowest 8 bits of the instruction.
        /// Sometimes referred to as 'byte'.
        /// </summary>
        public byte NN => (byte)(this.NNNN & 0xFF);

        /// <summary>
        /// The same as <see cref="NN"/>.
        /// </summary>
        public byte KK => this.NN;

        /// <summary>
        /// A 4-bit value, the upper 4 bits of the high byte of the instruction.
        /// </summary>
        public byte U => (byte)((this.NNNN >> 12) & 0xF);

        /// <summary>
        /// A 4-bit value, the lower 4 bits of the high byte of the instruction.
        /// </summary>
        public byte X => (byte)((this.NNNN >> 8) & 0xF);

        /// <summary>
        /// A 4-bit value, the upper 4 bits of the low byte of the instruction.
        /// </summary>
        public byte Y => (byte)((this.NNNN >> 4) & 0xF);

        /// <summary>
        /// A 4-bit value, the lower 4 bits of the low byte of the instruction.
        /// Sometimes referred to as 'nibble'.
        /// </summary>
        public byte N => (byte)(this.NNNN & 0xF);

        #region Equality

        public override int GetHashCode() => this.NNNN.GetHashCode();
        public override bool Equals(object obj) => Equals((Opcode)obj);
        public bool Equals(Opcode other) => this.NNNN == other.NNNN;
        public bool Equals(ushort other) => this.NNNN == other;

        public static bool operator ==(Opcode lhs, Opcode rhs) => lhs.Equals(rhs);
        public static bool operator !=(Opcode lhs, Opcode rhs) => !(lhs == rhs);

        #endregion
    }
}