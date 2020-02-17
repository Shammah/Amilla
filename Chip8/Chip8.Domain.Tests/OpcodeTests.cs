using Xunit;

namespace Amilla.Chip8.Domain.Tests
{
    public class OpcodeTests
    {
        [Theory]
        [InlineData(ushort.MaxValue)]
        public void NNNN(ushort op) => Assert.Equal(0b11111111_11111111, new Opcode(op).NNNN);

        [Theory]
        [InlineData(ushort.MaxValue)]
        public void NNN(ushort op) => Assert.Equal(0b11111111_1111, new Opcode(op).NNN);

        [Theory]
        [InlineData(ushort.MaxValue)]
        public void NN(ushort op) => Assert.Equal(0b11111111, new Opcode(op).NN);

        [Theory]
        [InlineData(ushort.MaxValue)]
        public void N(ushort op) => Assert.Equal(0b1111, new Opcode(op).N);

        [Theory]
        [InlineData(ushort.MaxValue)]
        public void KK(ushort op) => Assert.Equal(0b11111111, new Opcode(op).KK);

        [Theory]
        [InlineData(ushort.MaxValue)]
        public void U(ushort op) => Assert.Equal(0b1111, new Opcode(op).U);

        [Theory]
        [InlineData(ushort.MaxValue)]
        public void X(ushort op) => Assert.Equal(0b1111, new Opcode(op).X);

        [Theory]
        [InlineData(ushort.MaxValue)]
        public void Y(ushort op) => Assert.Equal(0b1111, new Opcode(op).Y);
    }
}
