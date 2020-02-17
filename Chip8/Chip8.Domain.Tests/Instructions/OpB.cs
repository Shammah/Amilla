using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class OpB
    {
        private Machine chip8;

        public OpB()
        {
            this.chip8 = new Machine();
        }

        [Fact]
        public void OpBNNN()
        {
            var program = new byte[] { 0xBA, 0xAA };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0xBB;

            this.chip8.Tick();

            Assert.Equal(0xBB + 0xAAA, this.chip8.State.PC);
        }
    }
}
