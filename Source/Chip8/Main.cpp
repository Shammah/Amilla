#include <memory>
#include "Emulator.hpp"
#include "EmulatorSFML.hpp"
using namespace Chip8;

int main()
{
    auto chip8 = std::make_shared<Emulator>();
    chip8->LoadFromFile("Roms/Chip-8 Demos/Maze [David Winter, 199x].ch8");

    EmulatorSFML shell(std::static_pointer_cast<IChip8Emulator>(chip8));
    shell.Start();

    return 0;
}