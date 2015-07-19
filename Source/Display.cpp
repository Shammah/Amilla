#include "Display.hpp"
using namespace Chip8;

Display::Display()
{
    Reset();
}

Display::~Display()
{
}

void Display::Reset()
{
    InstallFont();
    Pixels.fill(0);
}

void Display::InstallFont()
{
    // Copied from Bisqwit.
    for (unsigned n : {
        0xF999F, 0x26227, 0xF1F8F, 0xF1F1F, 0x99F11,
        0xF8F1F, 0xF8F9F, 0xF1244, 0xF9F9F, 0xF9F1F,
        0xF9F99, 0xE9E9E, 0xF888F, 0xE999E, 0xF8F8F, 0xF8F88
    })
    {
        auto* p = Font.data();
        for (int a = 16; a >= 0; a -= 4)
            *p++ = (n >> a) & 0xF;
    }
}