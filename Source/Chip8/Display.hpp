#pragma once

#include <array>
#include <stdint.h>
#include "Interfaces/IResettable.hpp"

namespace Chip8
{
    class Display : public Amilla::IResettable
    {
    public:
        static constexpr uint8_t WIDTH = 64;
        static constexpr uint8_t HEIGHT = 32;
        static constexpr uint8_t BYTES_PER_SPRITE = 5;
        static constexpr uint16_t FONT_MEMORY_SIZE = 16 * BYTES_PER_SPRITE;

    public:
        Display();
        virtual ~Display();

    public:
        /**
         * Each font character (0 .. F) is 5 * 8bit pixels.
         * So its height is 5 rows of pixels.
         */
        std::array<uint8_t, FONT_MEMORY_SIZE> Font;

        /** The actual screen containing the pixels to render. */
        std::array<uint8_t, WIDTH * HEIGHT / 8> Pixels;

    public:
        void Reset() override;
        void InstallFont();
    };
}