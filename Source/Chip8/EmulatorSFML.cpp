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
    RenderDisplay();
    _window->draw(_display);
    _window->display();
}

void EmulatorSFML::RenderDisplay()
{
    /**
     * Each 'pixel' in the Chip8 emulator is a byte,
     * which actualy holds 8 pixels for the real screen.
     * Thus, every bit on (1) must be converted into a
     * white 32 bit pixel, and black if it's off (0).
     *
     * Pixels in SFML are RGBA, so each pixel needs 4 bytes.
     */
    std::array<uint8_t, Display::WIDTH * Display::HEIGHT * 4> buffer;
    auto buffer_it = buffer.begin();

    for (auto& pixels : _emulator->GetDisplay().Pixels)
    {
        // Each Chip8 pixel consists of 8 bits, which are the actual pixels.
        uint8_t mask = 0b10000000;
        for (int i = 0; i < 8; i++)
        {
            if ((pixels & mask) == mask)
                for (int j = 0; j < 4; j++)
                    *(buffer_it++) = 255;
            else
                for (int j = 0; j < 4; j++)
                    *(buffer_it++) = 0;

            mask >>= 1;
        }
    }

    _displayTex.update(buffer.data());
}