using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class OpE
    {
        private Machine chip8;

        public OpE()
        {
            this.chip8 = new Machine();
        }

        [Fact]
        public void OpEX9E()
        {
            var program = new byte[]
            {
                0xE0, 0x9E,
                0x1F, 0xFF, // Jump, should be skipped.
                0x00, 0xE0, // Clear screen.
            };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0xF;
            this.chip8.State.Keys[0xF] = true;

            this.chip8.Tick();
            this.chip8.Tick();

            Assert.NotEqual(0xFFF, this.chip8.State.PC);
        }

        [Fact]
        public void OPEXA1()
        {
            var program = new byte[]
            {
                0xE0, 0xA1,
                0x1F, 0xFF, // Jump, should be skipped.
                0x00, 0xE0, // Clear screen.
            };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0xF;
            this.chip8.State.Keys[0xF] = true;
            this.chip8.State.Keys[0xF] = false;

            this.chip8.Tick();
            this.chip8.Tick();

            Assert.NotEqual(0xFFF, this.chip8.State.PC);
        }
    }
}
