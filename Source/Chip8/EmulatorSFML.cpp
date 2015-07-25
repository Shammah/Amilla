#include "EmulatorSFML.hpp"
using namespace Chip8;

EmulatorSFML::EmulatorSFML(std::shared_ptr<IEmulator> emulator)
    : _window(std::unique_ptr<sf::RenderWindow>(new sf::RenderWindow(sf::VideoMode(800, 600), "Shammah's Chip8 Emulator")))
{
    _emulator = emulator;
}

EmulatorSFML::~EmulatorSFML()
{

}

void EmulatorSFML::Start()
{
   while (_window->isOpen())
       Tick();
}

void EmulatorSFML::Tick()
{
    // Process events
    ProcessEvents();

    _emulator->Tick();

    Draw();
}

void EmulatorSFML::Stop()
{
    _window->close();
}

void EmulatorSFML::ProcessEvents()
{
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
                _emulator->GetState().Keys[key->second] = true;
            break;

        case sf::Event::KeyReleased:
            key = _keyMapping.find(event.key.code);
            if (key != _keyMapping.cend())
                _emulator->GetState().Keys[key->second] = false;
            break;
        }
    }
}

void EmulatorSFML::Draw()
{
    _window->clear();

    _window->display();
}