using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class Op2
    {
        private Machine chip8;

        public Op2()
        {
            chip8 = new Machine();
        }

        [Fact]
        public void Op2NNN()
        {
            var program = new byte[] { 0x2A, 0xAA };
            chip8.LoadFromMemory(program);

            // Run one instruction.
            chip8.Tick();

            Assert.Equal(0xAAA, chip8.State.PC);

            // Make sure the return address is correct.
            Assert.Equal(Memory.Program + 2, chip8.State.Stack[chip8.State.SP - 1]);
        }
    }
}
