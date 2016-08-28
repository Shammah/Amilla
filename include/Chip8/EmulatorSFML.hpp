#pragma once

#include <SFML/Graphics.hpp>
#include <SFML/Window.hpp>
#include "IChip8EmulatorShell.hpp"

namespace Chip8
{
    class EmulatorSFML : public IChip8EmulatorShell<sf::Keyboard::Key>
    {
    private:
        std::shared_ptr<IChip8Emulator> _emulator;
        std::unique_ptr<sf::RenderWindow> _window;

        sf::Sprite _display;
        sf::Texture _displayTex;

        std::unordered_map<sf::Keyboard::Key, uint8_t> _keyMapping =
        {
            { sf::Keyboard::Num0, 0x0 },
            { sf::Keyboard::Num1, 0x1 },
            { sf::Keyboard::Num2, 0x2 },
            { sf::Keyboard::Num3, 0x3 },
            { sf::Keyboard::Num4, 0x4 },
            { sf::Keyboard::Num5, 0x5 },
            { sf::Keyboard::Num6, 0x6 },
            { sf::Keyboard::Num7, 0x7 },
            { sf::Keyboard::Num8, 0x8 },
            { sf::Keyboard::Num9, 0x9 },
            { sf::Keyboard::A, 0xA },
            { sf::Keyboard::B, 0xB },
            { sf::Keyboard::C, 0xC },
            { sf::Keyboard::D, 0xD },
            { sf::Keyboard::E, 0xE },
            { sf::Keyboard::F, 0xF }
        };

    public:
        EmulatorSFML(std::shared_ptr<IChip8Emulator> emulator);
        virtual ~EmulatorSFML();

    protected:
        void ProcessEvents();
        void Draw();
        void RenderDisplay();
        virtual void Tick() override;

    public:
        virtual const std::unordered_map<sf::Keyboard::Key, uint8_t>& GetKeyMapping() override { return _keyMapping; }
        virtual const std::shared_ptr<IChip8Emulator> GetEmulator() const override { return _emulator; };

        virtual void Start() override;
        virtual void Stop() override;
    };
}
