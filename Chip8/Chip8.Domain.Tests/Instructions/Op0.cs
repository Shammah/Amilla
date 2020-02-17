using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class Op0
    {
        private Machine chip8;

        public Op0()
        {
            this.chip8 = new Machine();
        }

        [Fact]
        public void Op00E0()
        {
            var program = new byte[] { 0x00, 0xE0 };
            this.chip8.LoadFromMemory(program);

            // Set all pixels on.
            for (var i = 0; i < Display.Width * Display.Height; i++)
                this.chip8.Display.Pixels[i] = true;

            // Run one instruction.
            this.chip8.Tick();

            // Screen should be empty.
            Assert.Equal(new Display(), this.chip8.Display);
        }

        [Fact]
        public void Op00EE()
        {
            var program = new byte[] { 0x00, 0xEE };
            this.chip8.LoadFromMemory(program);

            // Add address to stack.
            this.chip8.State.Stack[this.chip8.State.SP] = 0xBEEF;
            this.chip8.State.SP++;

            // Run one instruction.
            this.chip8.Tick();

            // PC should be new address on stack.
            Assert.Equal(0xBEEF, this.chip8.State.PC);
        }

        [Fact]
        public void Op0NNN()
        {
            var program = new byte[] { 0x01, 0x11 };
            this.chip8.LoadFromMemory(program);

            // Run one instruction.
            this.chip8.Tick();

            // PC should not have jumped, it should have just skipped it.
            Assert.Equal(Memory.Program + 2, this.chip8.State.PC);
        }
    }
}
