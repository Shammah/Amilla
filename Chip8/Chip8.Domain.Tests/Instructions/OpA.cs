using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class OpA
    {
        private readonly Machine chip8;

        public OpA()
        {
            chip8 = new Machine();
        }

        [Fact]
        public void OpANNN()
        {
            var program = new byte[] { 0xAF, 0xFF };
            chip8.LoadFromMemory(program);

            chip8.Tick();

            Assert.Equal(0xFFF, chip8.State.I);
        }
    }
}
