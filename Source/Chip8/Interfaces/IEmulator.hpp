#pragma once

#include "Display.hpp"
#include "State.hpp"
#include "Storage.hpp"

namespace Chip8
{
    class IEmulator
    {
    public:
        virtual void Open(const std::string& rom) = 0;
        virtual void Tick() = 0;

        virtual const Storage& GetStorage() const = 0;
        virtual Storage& GetStorage() = 0;

        virtual const State& GetState() const = 0;
        virtual State& GetState() = 0;

        virtual const Display& GetDisplay() const = 0;
        virtual Display& GetDisplay() = 0;
    };
}