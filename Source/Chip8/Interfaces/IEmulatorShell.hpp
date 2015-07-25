#pragma once

#include <memory>
#include <unordered_map>
#include "IEmulator.hpp"

namespace Chip8
{
    /**
     * An interpreter shell is a means of communication the Chip8 interpreter
     * with the outside world; display, sound, input etc.
     */
    template <class KeyType>
    class IEmulatorShell
    {
    public:
        virtual ~IEmulatorShell() {};

        virtual const std::shared_ptr<IEmulator> GetEmulator() const = 0;
        virtual const std::unordered_map<KeyType, uint8_t>& GetKeyMapping() = 0;

        virtual void Start() = 0;
        virtual void Tick() = 0;
        virtual void Stop() = 0;
    };
}