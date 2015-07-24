#include "State.hpp"
using namespace Chip8;

State::State()
    : _RNG(0, 255)
{
    Reset();
}

State::~State()
{

}

void State::Reset()
{
    _gen.seed(_rd());

    I = 0;
    DelayTimer = 0;
    SoundTimer = 0;
    PC = 0;
    SP = 0;

    V.fill(0);
    Keys.fill(0);
    Stack.fill(0);
}

State::reg_t State::Random()
{
    return (reg_t)_RNG(_gen);
}