using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class Op3
    {
        private readonly Machine chip8;

        public Op3()
        {
            chip8 = new Machine();
        }

        [Fact]
        public void Op3XNN()
        {
            var program = new byte[]
            {
                0x30, 0xFF,
                0x1F, 0xFF, // Jump, should be skipped.
                0x00, 0xE0, // Clear screen.
            };
            chip8.LoadFromMemory(program);
            chip8.State.V[0x0] = 0xFF;

            chip8.Tick();
            chip8.Tick();

            // Instruction should have skipped.
            Assert.NotEqual(0xFFF, chip8.State.PC);
        }
    }
}
