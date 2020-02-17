using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class Op6
    {
        private Machine chip8;

        public Op6()
        {
            this.chip8 = new Machine();
        }

        [Fact]
        public void Op6XNN()
        {
            var program = new byte[] { 0x60, 0xFF };
            this.chip8.LoadFromMemory(program);

            // Run one instruction.
            this.chip8.Tick();

            // Make sure the register value was set.
            Assert.Equal(0xFF, this.chip8.State.V[0x0]);
        }
    }
}
