#pragma once

class IResettable
{
public:
    virtual ~IResettable() {};
    virtual void Reset() = 0;
};