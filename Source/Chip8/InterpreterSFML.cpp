#include "InterpreterSFML.hpp"
using namespace Chip8;

InterpreterSFML::InterpreterSFML(std::shared_ptr<IInterpreter> interpreter)
    : _window(std::unique_ptr<sf::RenderWindow>(new sf::RenderWindow(sf::VideoMode(800, 600), "Shammah's Chip8 Emulator")))
{
    _interpreter = interpreter;
}

InterpreterSFML::~InterpreterSFML()
{

}

void InterpreterSFML::Start()
{
   while (_window->isOpen())
       Tick();
}

void InterpreterSFML::Tick()
{
    // Process events
    sf::Event event;
    while (_window->pollEvent(event))
    {
        // Close window: exit
        if (event.type == sf::Event::Closed)
            _window->close();

        auto key = _keyMapping.cend();

        switch (event.type)
        {
        case sf::Event::Closed:
            Stop();
            break;

        case sf::Event::KeyPressed:
            key = _keyMapping.find(event.key.code);
            if (key != _keyMapping.cend())
                _interpreter->GetState().Keys[key->second] = true;
            break;

        case sf::Event::KeyReleased:
            key = _keyMapping.find(event.key.code);
            if (key != _keyMapping.cend())
                _interpreter->GetState().Keys[key->second] = false;
            break;
        }
    }

    _interpreter->Tick();

    // Clear screen
    _window->clear();

    // Update the window
    _window->display();
}

void InterpreterSFML::Stop()
{
    _window->close();
}