#pragma once

namespace Amilla
{
    class IResettable
    {
    public:
        virtual ~IResettable() {};
        virtual void Reset() = 0;
    };
}