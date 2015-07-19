#include "Interpreter.hpp"
using namespace Chip8;

int main()
{
    Interpreter chip8;
    chip8.Open("Roms/Chip-8 Demos/Maze [David Winter, 199x].ch8");

    while (true)
    {
        chip8.Tick();
    }

    return 0;
}