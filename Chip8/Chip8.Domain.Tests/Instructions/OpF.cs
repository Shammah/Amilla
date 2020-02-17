using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class OpF
    {
        private Machine chip8;

        public OpF()
        {
            this.chip8 = new Machine();
        }

        [Fact]
        public void OpFX07()
        {
            var program = new byte[] { 0xF0, 0x07 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.DelayTimer = 0xFF;

            this.chip8.Tick();

            Assert.Equal(this.chip8.State.V[0x0], this.chip8.State.DelayTimer);
        }

        [Fact]
        public void OpFX15()
        {
            var program = new byte[] { 0xF0, 0x15 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0xFF;

            this.chip8.Tick();

            Assert.Equal(this.chip8.State.V[0x0], this.chip8.State.DelayTimer);
        }

        [Fact]
        public void OpFX18()
        {
            var program = new byte[] { 0xF0, 0x18 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0xFF;

            this.chip8.Tick();

            Assert.Equal(this.chip8.State.V[0x0], this.chip8.State.SoundTimer);
        }

        [Fact]
        public void OpFX1E()
        {
            var program = new byte[] { 0xF0, 0x1E };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0x0A;
            this.chip8.State.I = 0x0A;

            this.chip8.Tick();

            Assert.Equal(0x0A + 0x0A, this.chip8.State.I);
        }

        [Fact]
        public void OpFX29()
        {
            var program = new byte[] { 0xF5, 0x29 }; // Hex character in V5
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x5] = 0xC;

            this.chip8.Tick();

            Assert.Equal(Memory.Font + 0xC * Display.BytesPerSprite, this.chip8.State.I);
        }

        [Fact]
        public void OpFX33()
        {
            ushort startMemoryAddress = 0x300;
            byte bcdTest = 251;

            var program = new byte[] { 0xF0, 0x33 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.I = startMemoryAddress;
            this.chip8.State.V[0x0] = bcdTest;

            this.chip8.Tick();

            // Hundreds
            Assert.Equal(2, this.chip8.Memory[startMemoryAddress + 0]);

            // Tens
            Assert.Equal(5, this.chip8.Memory[startMemoryAddress + 1]);

            // Ones
            Assert.Equal(1, this.chip8.Memory[startMemoryAddress + 2]);
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
            this.chip8.LoadFromMemory(program);

            for (var i = 0; i < values.Length; i++)
                this.chip8.State.V[i] = values[i];

            this.chip8.State.I = startMemoryAddress;

            this.chip8.Tick();

            for (var i = 0; i < values.Length; i++)
                Assert.Equal(this.chip8.State.V[i], this.chip8.Memory[startMemoryAddress + i]);

            Assert.Equal(startMemoryAddress + 0xB + 1, this.chip8.State.I);
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
            this.chip8.LoadFromMemory(program);

            for (var i = 0; i < values.Length; i++)
                this.chip8.Memory[startMemoryAddress + i] = values[i];

            this.chip8.State.I = startMemoryAddress;

            this.chip8.Tick();

            for (var i = 0; i < values.Length; i++)
                Assert.Equal(this.chip8.State.V[i], this.chip8.Memory[startMemoryAddress + i]);

            Assert.Equal(startMemoryAddress + 0xB + 1, this.chip8.State.I);
        }
    }
}
