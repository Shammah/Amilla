#pragma once

#include <string>
#include "IResettable.hpp"

namespace Amilla
{
    class IEmulator : public IResettable
    {
    public:
        virtual ~IEmulator() {};

        /**
        * Loads a ROM directly from memory.
        *
        * @param code Pointer to the ROM in memory.
        * @param size The size of the ROM in bytes.
        */
        virtual void LoadFromMemory(uint8_t const * const code, const size_t size) = 0;

        /**
        * Loads a ROM from a file on the operating system.
        *
        * @param rom The location of the ROM on the operating system,
        *            relative to the executable location.
        */
        virtual void LoadFromFile(const std::string& rom) = 0;

        /** @return Whether a ROM is currently loaded. */
        virtual bool IsLoaded() const = 0;

        /** Advances the emulator by one CPU cycle. */
        virtual void Tick() = 0;
    };
}