using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class Op1
    {
        private Machine chip8;

        public Op1()
        {
            this.chip8 = new Machine();
        }

        [Fact]
        public void Op1NNN()
        {
            var program = new byte[] { 0x1F, 0xED };
            this.chip8.LoadFromMemory(program);

            // Run one instruction.
            this.chip8.Tick();

            // PC should be set to FED.
            Assert.Equal(0xFED, this.chip8.State.PC);
        }
    }
}
