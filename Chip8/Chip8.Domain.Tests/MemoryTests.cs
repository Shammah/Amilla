using System;
using System.Linq;
using Xunit;

namespace Amilla.Chip8.Domain.Tests
{
    public class MemoryTests
    {
        [Fact]
        public void Reset()
        {
            var memory = new Memory();

            Assert.Equal(new Memory(), memory);

            memory.LoadFont(new byte[] { 1 });
            Assert.NotEqual(new Memory(), memory);

            memory.Reset();
            Assert.Equal(new Memory(), memory);

            memory.LoadProgram(new byte[] { 1 });
            Assert.NotEqual(new Memory(), memory);

            memory.Reset();
            Assert.Equal(new Memory(), memory);
        }

        [Fact]
        public void LoadFont()
        {
            var size = 0x1FF;
            var rand = new Random();
            var font = new byte[size];
            rand.NextBytes(font);

            var memory = new Memory();
            memory.LoadFont(font);

            Assert.True(memory
                .Take(size)
                .SequenceEqual(font));
        }

        [Fact]
        public void FontTooBig()
        {
            var size = 0x200;
            var rand = new Random();
            var font = new byte[size];
            rand.NextBytes(font);

            var memory = new Memory();
            Assert.Throws<ArgumentException>(() => memory.LoadFont(font));
        }

        [Fact]
        public void LoadProgram()
        {
            var size = 0x1FF;
            var rand = new Random();
            var program = new byte[size];
            rand.NextBytes(program);

            var memory = new Memory();
            memory.LoadProgram(program);

            Assert.True(memory
                .Skip(Memory.Program)
                .Take(size)
                .SequenceEqual(program));
        }

        [Fact]
        public void ProgramTooBig()
        {
            var size = Memory.RAMSize - Memory.Program;
            var rand = new Random();
            var program = new byte[size];
            rand.NextBytes(program);

            var memory = new Memory();
            Assert.Throws<ArgumentException>(() => memory.LoadProgram(program));
        }
    }
}
