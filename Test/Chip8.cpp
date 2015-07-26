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
        GIVEN("An emulator instance")
        {
            REQUIRE(chip8->IsLoaded() == false);

            WHEN("Testing opcodes")
            {
                WHEN("6XNN")
                {
                    uint8_t code[] = { 0x63, 0x42 };
                    chip8->LoadFromMemory(code, 2);

                    auto& state = chip8->GetState();
                    REQUIRE(state.V[0x3] == 0);

                    chip8->Tick();
                    REQUIRE(state.V[0x3] == 0x42);
                }
            }
        }
    }
}
