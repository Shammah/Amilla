#include <memory>
#include "boost/di.hpp"
#include "../Chip8/Emulator.hpp"
#include "../Chip8/EmulatorSFML.hpp"
using namespace Chip8;
namespace di = boost::di;

int main()
{
    auto injector = di::make_injector(
        di::bind<IChip8Emulator, Emulator>(),
        di::bind<IChip8EmulatorShell<sf::Keyboard::Key>, EmulatorSFML>()
    );

    auto shell = injector.create<std::unique_ptr<IChip8EmulatorShell<sf::Keyboard::Key>>>();
    shell->GetEmulator()->LoadFromFile("Roms/Chip-8 Demos/Maze [David Winter, 199x].ch8");
    shell->Start();

    return 0;
}