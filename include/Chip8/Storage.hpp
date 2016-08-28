#pragma once

#include <array>
#include "Amilla/IResettable.hpp"

namespace Chip8
{
    /**
     * A Chip8 machine consists of memory and a state.
     *
     * The first 512 KiB of the RAM region is reserved for the interpreter itself,
     * like its registers. Previously, I put the registers and other memory elements
     * into a union with the enitre memory itself. However, this made it difficult
     * to extend, as unions and non-POD members don't go that well together.
     */
    class Storage : public Amilla::IResettable
    {
    public:
        typedef uint8_t byte_t;
        static constexpr uint16_t RAM_SIZE = 4096;

    public:
        Storage();
        virtual ~Storage();

    public:
        /** The entire memory of the machine. */
        std::array<byte_t, RAM_SIZE> RAM;

    public:
        /** Resets the entire machine, inlcuding both memory and state. */
        void Reset() override;
    };
}
