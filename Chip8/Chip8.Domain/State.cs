using System;
using System.Collections.Generic;
using Amilla.Chip8.Domain.Interfaces;
using Amilla.Chip8.Domain.SeedWork;

namespace Amilla.Chip8.Domain
{
    /// <summary>
    /// Chip8 has 16 general purpose 8-bit registers, usually referred to as Vx,
    /// where x is a hexadecimal digit (0 through F). There is also a 16-bit register
    /// called I. This register is generally used to store memory addresses, so only
    /// the lowest (rightmost) 12 bits are usually used.
    ///
    /// The VF register should not be used by any program, as it is used as a flag by
    /// some instructions.
    /// 
    /// Chip8 also has two special purpose 8-bit registers, for the delay and
    /// sound timers. When these registers are non-zero, they are automatically
    /// decremented at a rate of 60Hz.
    /// </summary
    public class State : ValueObject, IResettable
    {
        public const int NumRegisters = 16;
        public const int NumKeys = 16;
        public const int StackSize = 12;

        private Random random;

        public State()
        {
            V = new byte[NumRegisters];
            Keys = new bool[NumKeys];
            Stack = new ushort[StackSize];

            Reset();
        }

        /// <summary>
        /// 16 general purpose 8-bit registers Vx, where x is a heximal digit.
        /// </summary>
        public byte[] V { get; }

        /// <summary>
        /// 16-bit instruction pointer.
        /// </summary>
        public ushort I { get; set; }

        public byte DelayTimer { get; set; }

        public byte SoundTimer { get; set; }

        /// <summary>
        /// The 16-bit program counter, used to store the currently executing address.
        /// </summary>
        public ushort PC { get; set; }

        /// <summary>
        /// The 16-bit stack pointer, used to point to the topmost level of the stack.
        /// </summary>
        public ushort SP { get; set; }

        /// <summary>
        /// The state of all keys being pressed.
        /// </summary>
        public bool[] Keys { get; }

        /// <summary>
        /// The stack is an array of 12 16-bit values, used to store the
        /// address that the interpreter should return to when finished with
        /// a subroutine. Chip8 allows for up to 12 levels of nested subroutines.
        /// </summary>
        public ushort[] Stack { get; }

        public void Reset()
        {
            random = new Random();

            I = 0;
            DelayTimer = 0;
            SoundTimer = 0;
            PC = 0;
            SP = 0;

            Array.Clear(V, 0, NumRegisters);
            Array.Clear(Keys, 0, NumKeys);
            Array.Clear(Stack, 0, StackSize);
        }

        /// <returns>
        /// A random register value.
        /// </returns>
        public byte Random()
        {
            var randomByte = new byte[1];
            random.NextBytes(randomByte);

            return randomByte[0];
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return I;
            yield return DelayTimer;
            yield return SoundTimer;
            yield return PC;
            yield return SP;

            foreach (var v in V)
                yield return v;

            foreach (var mem in Stack)
                yield return mem;

            foreach (var key in Keys)
                yield return key;
        }
    }
}