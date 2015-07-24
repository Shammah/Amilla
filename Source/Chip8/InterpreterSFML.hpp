#pragma once

#include "SFML/Graphics.hpp"
#include "SFML/Window.hpp"
#include "IInterpreterShell.hpp"

namespace Chip8
{
    class InterpreterSFML : public IInterpreterShell<sf::Keyboard::Key>
    {
    private:
        std::shared_ptr<IInterpreter> _interpreter;
        std::unique_ptr<sf::RenderWindow> _window;

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
        InterpreterSFML(std::shared_ptr<IInterpreter> interpreter);
        virtual ~InterpreterSFML();

        virtual const std::unordered_map<sf::Keyboard::Key, uint8_t>& GetKeyMapping() { return _keyMapping; }
        virtual const std::shared_ptr<IInterpreter> GetInterpreter() const { return _interpreter; };

        virtual void Start() override;
        virtual void Tick();
        virtual void Stop() override;
    };
}