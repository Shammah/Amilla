#pragma once

#include <array>
#include <stdint.h>
#include "IResettable.hpp"

namespace Chip8
{
    class Display : public IResettable
    {
    public:
        static constexpr uint8_t WIDTH = 64;
        static constexpr uint8_t HEIGHT = 32;

    public:
        Display();
        virtual ~Display();

    public:
        /**
         * Each font character (0 .. F) is 5 * 8bit pixels.
         * So its height is 5 rows of pixels.
         */
        std::array<uint8_t, 16 * 5> Font;

        /** The actual screen containing the pixels to render. */
        std::array<uint8_t, WIDTH * HEIGHT / 8> Pixels;

    public:
        void Reset() override;
        void InstallFont();
    };
}