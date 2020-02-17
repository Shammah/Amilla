using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class OpB
    {
        private readonly Machine chip8;

        public OpB()
        {
            chip8 = new Machine();
        }

        [Fact]
        public void OpBNNN()
        {
            var program = new byte[] { 0xBA, 0xAA };
            chip8.LoadFromMemory(program);
            chip8.State.V[0x0] = 0xBB;

            chip8.Tick();

            Assert.Equal(0xBB + 0xAAA, chip8.State.PC);
        }
    }
}
