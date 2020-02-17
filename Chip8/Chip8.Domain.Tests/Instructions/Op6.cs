using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class Op6
    {
        private readonly Machine chip8;

        public Op6()
        {
            chip8 = new Machine();
        }

        [Fact]
        public void Op6XNN()
        {
            var program = new byte[] { 0x60, 0xFF };
            chip8.LoadFromMemory(program);

            // Run one instruction.
            chip8.Tick();

            // Make sure the register value was set.
            Assert.Equal(0xFF, chip8.State.V[0x0]);
        }
    }
}
