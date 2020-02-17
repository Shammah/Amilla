using System;

namespace Amilla.Chip8.Domain
{
    public class UnknownOpcodeException : Exception
    {
        public readonly Opcode Opcode;

        public UnknownOpcodeException(Opcode op)
        {
            this.Opcode = op;
        }
    }
}