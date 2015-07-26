#pragma once

#include <memory>
#include <unordered_map>
#include "IChip8Emulator.hpp"
#include "Interfaces/IEmulatorShell.hpp"

namespace Chip8
{
    /**
     * An interpreter shell is a means of communication the Chip8 interpreter
     * with the outside world; display, sound, input etc.
     */
    template <class KeyType>
    class IChip8EmulatorShell : public Amilla::IEmulatorShell<IChip8Emulator>
    {
    public:
        virtual ~IChip8EmulatorShell() {};

    protected:
        virtual void Tick() = 0;

    public:
        virtual const std::shared_ptr<IChip8Emulator> GetEmulator() const = 0;
        virtual const std::unordered_map<KeyType, uint8_t>& GetKeyMapping() = 0;

        virtual void Start() = 0;
        virtual void Stop() = 0;
    };
}