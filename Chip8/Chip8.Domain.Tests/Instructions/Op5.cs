﻿using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class Op5
    {
        private readonly Machine chip8;

        public Op5()
        {
            chip8 = new Machine();
        }

        [Fact]
        public void Op5XY0()
        {
            var program = new byte[]
            {
                0x50, 0x10,
                0x1F, 0xFF, // Jump, should be skipped.
                0x00, 0xE0, // Clear screen.
            };
            chip8.LoadFromMemory(program);
            chip8.State.V[0x0] = 0xEE;
            chip8.State.V[0x1] = 0xEE;

            chip8.Tick();
            chip8.Tick();

            // Instruction should have skipped.
            Assert.NotEqual(0xFFF, chip8.State.PC);
        }
    }
}
