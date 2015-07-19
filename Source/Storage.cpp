#include "Storage.hpp"
using namespace Chip8;

Storage::Storage()
{
    Reset();
}

Storage::~Storage()
{
}

void Storage::Reset()
{
    RAM.fill(0);
}