using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class Op7
    {
        private Machine chip8;

        public Op7()
        {
            chip8 = new Machine();
        }

        [Fact]
        public void Op7XNN()
        {
            var program = new byte[] { 0x70, 0x03 };
            chip8.LoadFromMemory(program);
            chip8.State.V[0x0] = 0x02;

            // Run one instruction.
            chip8.Tick();

            // Make sure the register value was set.
            Assert.Equal(0x02 + 0x03, chip8.State.V[0x0]);
        }
    }
}
