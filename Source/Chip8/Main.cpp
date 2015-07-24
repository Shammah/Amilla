#include <memory>
#include "Interpreter.hpp"
#include "InterpreterSFML.hpp"
using namespace Chip8;

int main()
{
    auto chip8 = std::make_shared<Interpreter>();
    chip8->Open("Roms/Chip-8 Demos/Maze [David Winter, 199x].ch8");

    InterpreterSFML shell(std::static_pointer_cast<IInterpreter>(chip8));
    shell.Start();

    return 0;
}