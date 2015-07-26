#include <memory>
#include "catch.hpp"
#include "boost/di.hpp"
#include "../Source/Chip8/Emulator.hpp"
#include "../Source/Chip8/Interfaces/IChip8Emulator.hpp"
using namespace Chip8;
namespace di = boost::di;

SCENARIO("Testing various Chip8 implementations")
{
    for (auto& chip8 : {
        di::make_injector(
            di::bind<IChip8Emulator, Emulator>()
        ).create<std::unique_ptr<IChip8Emulator>>()
    })
    {
        REQUIRE(chip8->IsLoaded() == false);
    }
}
