using Xunit;

namespace Amilla.Chip8.Domain.Tests.Instructions
{
    public class Op8
    {
        private Machine chip8;

        public Op8()
        {
            this.chip8 = new Machine();
        }

        [Fact]
        public void Op8XY0()
        {
            var program = new byte[] { 0x80, 0x10 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x1] = 0xFF;

            // Run one instruction.
            this.chip8.Tick();

            Assert.Equal(0xFF, this.chip8.State.V[0x0]);
        }

        [Fact]
        public void Op8XY1()
        {
            var program = new byte[] { 0x80, 0x11 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0x0F;
            this.chip8.State.V[0x1] = 0xF0;

            // Run one instruction.
            this.chip8.Tick();

            Assert.Equal(0x0F | 0xF0, this.chip8.State.V[0x0]);
        }

        [Fact]
        public void Op8XY2()
        {
            var program = new byte[] { 0x80, 0x12 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0x0F;
            this.chip8.State.V[0x1] = 0xF0;

            // Run one instruction.
            this.chip8.Tick();

            Assert.Equal(0x0F & 0xF0, this.chip8.State.V[0x0]);
        }

        [Fact]
        public void Op8XY3()
        {
            var program = new byte[] { 0x80, 0x13 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0x0F;
            this.chip8.State.V[0x1] = 0xF0;

            // Run one instruction.
            this.chip8.Tick();

            Assert.Equal(0x0F ^ 0xF0, this.chip8.State.V[0x0]);
        }

        [Fact]
        public void Op8XY4NoCarry()
        {
            var program = new byte[] { 0x80, 0x14 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0x0F;
            this.chip8.State.V[0x1] = 0xF0;

            // Run one instruction.
            this.chip8.Tick();

            Assert.Equal(0x0F + 0xF0, this.chip8.State.V[0x0]);
            Assert.Equal(0x0, this.chip8.State.V[0xF]);
        }

        [Fact]
        public void Op8XY4Carry()
        {
            var program = new byte[] { 0x80, 0x14 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0xFF;
            this.chip8.State.V[0x1] = 0x01;

            // Run one instruction.
            this.chip8.Tick();

            Assert.Equal(0x0, this.chip8.State.V[0x0]);
            Assert.Equal(0x1, this.chip8.State.V[0xF]);
        }

        [Fact]
        public void Op8XY5NoBorrow()
        {
            var program = new byte[] { 0x80, 0x15 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0x0F;
            this.chip8.State.V[0x1] = 0x0A;
            this.chip8.State.V[0xF] = 0x1;

            // Run one instruction.
            this.chip8.Tick();

            Assert.Equal(0x0F - 0x0A, this.chip8.State.V[0x0]);
            Assert.Equal(0x0, this.chip8.State.V[0xF]);
        }

        [Fact]
        public void Op8XY5Borrow()
        {
            var program = new byte[] { 0x80, 0x15 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0x0A;
            this.chip8.State.V[0x1] = 0x0F;
            this.chip8.State.V[0xF] = 0x0;

            // Run one instruction.
            this.chip8.Tick();

            Assert.NotEqual(0x0F - 0x0A, this.chip8.State.V[0x0]);
            Assert.Equal(0x1, this.chip8.State.V[0xF]);
        }

        [Fact]
        public void Op8XY6LSB0()
        {
            var program = new byte[] { 0x80, 0x16 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0xAA;
            this.chip8.State.V[0xF] = 0x1;

            // Run one instruction.
            this.chip8.Tick();

            // VF should be 1 since LSB is 0.
            Assert.Equal(0xAA / 2, this.chip8.State.V[0x0]);
            Assert.Equal(0x0, this.chip8.State.V[0xF]);
        }

        [Fact]
        public void Op8XY6LSB1()
        {
            var program = new byte[] { 0x80, 0x16 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0xFF;
            this.chip8.State.V[0xF] = 0x0;

            // Run one instruction.
            this.chip8.Tick();

            // VF should be 1 since LSB is 1.
            Assert.Equal(0xFF / 2, this.chip8.State.V[0x0]);
            Assert.Equal(0x1, this.chip8.State.V[0xF]);
        }

        [Fact]
        public void Op8XY7NotBorrow()
        {
            var program = new byte[] { 0x80, 0x17 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0x0A;
            this.chip8.State.V[0x1] = 0x0B;
            this.chip8.State.V[0xF] = 0x0;

            // Run one instruction.
            this.chip8.Tick();

            Assert.Equal(0x0B - 0xA, this.chip8.State.V[0x0]);
            Assert.Equal(0x1, this.chip8.State.V[0xF]);
        }

        [Fact]
        public void Op8XY7Borrow()
        {
            var program = new byte[] { 0x80, 0x17 };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0x0B;
            this.chip8.State.V[0x1] = 0x0A;
            this.chip8.State.V[0xF] = 0x1;

            // Run one instruction.
            this.chip8.Tick();

            // Variable because constant -1 cannot be casted to byte.
            byte x = 0x0B;
            byte y = 0x0A;
            var expected = (byte)(y - x);

            // VF should be 1 since LSB is 1.
            Assert.Equal(expected, this.chip8.State.V[0x0]);
            Assert.Equal(0x0, this.chip8.State.V[0xF]);
        }

        [Fact]
        public void Op8XYEMSB0()
        {
            var program = new byte[] { 0x80, 0x1E };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0x7A;
            this.chip8.State.V[0xF] = 0x1;

            // Run one instruction.
            this.chip8.Tick();

            // VF should be 1 since LSB is 1.
            Assert.Equal(0x7A * 2, this.chip8.State.V[0x0]);
            Assert.Equal(0x0, this.chip8.State.V[0xF]);
        }

        [Fact]
        public void Op8XYMSB1()
        {
            var program = new byte[] { 0x80, 0x1E };
            this.chip8.LoadFromMemory(program);
            this.chip8.State.V[0x0] = 0xF0;
            this.chip8.State.V[0xF] = 0x0;

            // Run one instruction.
            this.chip8.Tick();

            // Variable because constant -1 cannot be casted to byte.
            byte x = 0xF0;
            var expected = (byte)(x << 1);

            // VF should be 1 since LSB is 1.
            Assert.Equal(expected, this.chip8.State.V[0x0]);
            Assert.Equal(0x1, this.chip8.State.V[0xF]);
        }
    }
}
