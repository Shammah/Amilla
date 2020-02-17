using Xunit;

namespace Amilla.Chip8.Domain.Tests
{
    public class StateTests
    {
        [Fact]
        public void CollectionSizes()
        {
            var state = new State();

            Assert.Equal(State.NumRegisters, state.V.Length);
            Assert.Equal(State.NumKeys, state.Keys.Length);
            Assert.Equal(State.StackSize, state.Stack.Length);
        }

        [Fact]
        public void Reset()
        {
            var state = new State();
            Assert.Equal(new State(), state);

            state.I = 40;
            state.DelayTimer = 30;
            state.SoundTimer = 13;
            state.PC = 5;
            state.SP = 3;
            state.V[5] = 4;
            state.Stack[2] = 3;
            state.Keys[3] = true;

            Assert.NotEqual(new State(), state);

            state.Reset();
            Assert.Equal(new State(), state);
        }
    }
}
