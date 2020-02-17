using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class Op2
    {
        private Machine chip8;

        public Op2()
        {
            this.chip8 = new Machine();
        }

        [Fact]
        public void Op2NNN()
        {
            var program = new byte[] { 0x2A, 0xAA };
            this.chip8.LoadFromMemory(program);

            // Run one instruction.
            this.chip8.Tick();

            Assert.Equal(0xAAA, this.chip8.State.PC);

            // Make sure the return address is correct.
            Assert.Equal(Memory.Program + 2, this.chip8.State.Stack[this.chip8.State.SP - 1]);
        }
    }
}
