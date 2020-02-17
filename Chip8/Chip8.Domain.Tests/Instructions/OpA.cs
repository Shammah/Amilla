using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class OpA
    {
        private Machine chip8;

        public OpA()
        {
            this.chip8 = new Machine();
        }

        [Fact]
        public void OpANNN()
        {
            var program = new byte[] { 0xAF, 0xFF };
            this.chip8.LoadFromMemory(program);

            this.chip8.Tick();

            Assert.Equal(0xFFF, this.chip8.State.I);
        }
    }
}
