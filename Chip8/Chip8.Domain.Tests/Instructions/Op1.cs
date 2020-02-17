using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class Op1
    {
        private readonly Machine chip8;

        public Op1()
        {
            chip8 = new Machine();
        }

        [Fact]
        public void Op1NNN()
        {
            var program = new byte[] { 0x1F, 0xED };
            chip8.LoadFromMemory(program);

            // Run one instruction.
            chip8.Tick();

            // PC should be set to FED.
            Assert.Equal(0xFED, chip8.State.PC);
        }
    }
}
