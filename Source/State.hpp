#pragma once

#include <array>
#include <random>
#include "IResettable.hpp"

namespace Chip8
{
    /**
     * The state of Chip8 machine. like register values and screen output.
     */
    class State : public IResettable
    {
    public:
        /** Type of a normal register. */
        typedef uint8_t reg_t;

        /** Type of an adress container. */
        typedef uint16_t addr_t;

        static constexpr int NUM_REGISTERS = 16;
        static constexpr int NUM_KEYS = 16;
        static constexpr int STACK_SIZE = 12;

    public:
        State();
        virtual ~State();

    private:
        /** Random number regeneration. */
        std::random_device _rd;
        std::mt19937 _gen;
        std::uniform_int_distribution<> _RNG;

    public:
        /**
         * Chip - 8 has 16 general purpose 8 - bit registers, usually referred to as Vx,
         * where x is a hexadecimal digit(0 through F). There is also a 16 - bit register
         * called I. This register is generally used to store memory addresses, so only
         * the lowest (rightmost) 12 bits are usually used.
         *
         * The VF register should not be used by any program, as it is used as a flag by
         * some instructions.
         */
        std::array<reg_t, NUM_REGISTERS> V;
        addr_t I;

        /**
         * Chip - 8 also has two special purpose 8 - bit registers, for the delay and
         * sound timers. When these registers are non - zero, they are automatically
         * decremented at a rate of 60Hz.
         */
        reg_t DelayTimer, SoundTimer;

        /** Program counter & Stack pointer are 16 bit. */
        addr_t PC, SP;

        /** States of all keys being pressed. */
        std::array<bool, NUM_KEYS> Keys;

        /**
         * The stack is an array of 12 16-bit values, used to store the
         * address that the interpreter should return to when finished with
         * a subroutine. Chip-8 allows for up to 12 levels of nested subroutines.
         */
        std::array<addr_t, STACK_SIZE> Stack;

    public:
        /** Reset the entire state. */
        void Reset() override;

        /**
         * Generates a random number.
         *
         * @return A newly generated random number.
         */
        reg_t Random();
    };
}