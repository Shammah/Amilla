using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class OpF
    {
        private readonly Machine chip8;

        public OpF()
        {
            chip8 = new Machine();
        }

        [Fact]
        public void OpFX07()
        {
            var program = new byte[] { 0xF0, 0x07 };
            chip8.LoadFromMemory(program);
            chip8.State.DelayTimer = 0xFF;

            chip8.Tick();

            Assert.Equal(chip8.State.V[0x0], chip8.State.DelayTimer);
        }

        [Fact]
        public void OpFX15()
        {
            var program = new byte[] { 0xF0, 0x15 };
            chip8.LoadFromMemory(program);
            chip8.State.V[0x0] = 0xFF;

            chip8.Tick();

            Assert.Equal(chip8.State.V[0x0], chip8.State.DelayTimer);
        }

        [Fact]
        public void OpFX18()
        {
            var program = new byte[] { 0xF0, 0x18 };
            chip8.LoadFromMemory(program);
            chip8.State.V[0x0] = 0xFF;

            chip8.Tick();

            Assert.Equal(chip8.State.V[0x0], chip8.State.SoundTimer);
        }

        [Fact]
        public void OpFX1E()
        {
            var program = new byte[] { 0xF0, 0x1E };
            chip8.LoadFromMemory(program);
            chip8.State.V[0x0] = 0x0A;
            chip8.State.I = 0x0A;

            chip8.Tick();

            Assert.Equal(0x0A + 0x0A, chip8.State.I);
        }

        [Fact]
        public void OpFX29()
        {
            var program = new byte[] { 0xF5, 0x29 }; // Hex character in V5
            chip8.LoadFromMemory(program);
            chip8.State.V[0x5] = 0xC;

            chip8.Tick();

            Assert.Equal(Memory.Font + 0xC * Display.BytesPerSprite, chip8.State.I);
        }

        [Fact]
        public void OpFX33()
        {
            ushort startMemoryAddress = 0x300;
            byte bcdTest = 251;

            var program = new byte[] { 0xF0, 0x33 };
            chip8.LoadFromMemory(program);
            chip8.State.I = startMemoryAddress;
            chip8.State.V[0x0] = bcdTest;

            chip8.Tick();

            // Hundreds
            Assert.Equal(2, chip8.Memory[startMemoryAddress + 0]);

            // Tens
            Assert.Equal(5, chip8.Memory[startMemoryAddress + 1]);

            // Ones
            Assert.Equal(1, chip8.Memory[startMemoryAddress + 2]);
        }

        [Fact]
        public void OpFX55()
        {
            ushort startMemoryAddress = 0x300;
            var values = new byte[]
            {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05,
                0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B,
            };

            var program = new byte[] { 0xFB, 0x55 };
            chip8.LoadFromMemory(program);

            for (var i = 0; i < values.Length; i++)
                chip8.State.V[i] = values[i];

            chip8.State.I = startMemoryAddress;

            chip8.Tick();

            for (var i = 0; i < values.Length; i++)
                Assert.Equal(chip8.State.V[i], chip8.Memory[startMemoryAddress + i]);

            Assert.Equal(startMemoryAddress + 0xB + 1, chip8.State.I);
        }

        [Fact]
        public void OpFX65()
        {
            ushort startMemoryAddress = 0x300;
            var values = new byte[]
            {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05,
                0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B,
            };

            var program = new byte[] { 0xFB, 0x65 };
            chip8.LoadFromMemory(program);

            for (var i = 0; i < values.Length; i++)
                chip8.Memory[startMemoryAddress + i] = values[i];

            chip8.State.I = startMemoryAddress;

            chip8.Tick();

            for (var i = 0; i < values.Length; i++)
                Assert.Equal(chip8.State.V[i], chip8.Memory[startMemoryAddress + i]);

            Assert.Equal(startMemoryAddress + 0xB + 1, chip8.State.I);
        }
    }
}
