using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class Op5
    {
        private Machine chip8;

        public Op5()
        {
            this.chip8 = new Machine();
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
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0xEE;
            this.chip8.State.V[0x1] = 0xEE;

            this.chip8.Tick();
            this.chip8.Tick();

            // Instruction should have skipped.
            Assert.NotEqual(0xFFF, this.chip8.State.PC);
        }
    }
}
