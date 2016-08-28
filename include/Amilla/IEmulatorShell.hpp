#pragma once

#include <memory>
#include <type_traits>
#include "IEmulator.hpp"

namespace Amilla
{
    /**
     * An interpreter shell is a means of communication for the emulator
     * with the outside world; display, sound, input etc.
     *
     * Emulator must be of type IEmulator.
     */
    template <class Emulator>
    class IEmulatorShell
    {
    public:
        IEmulatorShell()
        {
            static_assert(std::is_base_of<IEmulator, Emulator>::value, "IEmulatorShell requires an Emulator of type IEmulator");
        }

        virtual ~IEmulatorShell() {};

        virtual const std::shared_ptr<Emulator> GetEmulator() const = 0;
    };
}