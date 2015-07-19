#include <fstream>
#include "Interpreter.hpp"
using namespace std;
using namespace Chip8;

Interpreter::Interpreter()
{
    _mainOpcodeTable =
    {
        &Interpreter::Special,
        &Interpreter::Jump,
        &Interpreter::Call2,
        &Interpreter::SkipEqualsNN,
        &Interpreter::SkipNotEqualsNN,
        &Interpreter::SkipEqualsReg,
        &Interpreter::StoreNN,
        &Interpreter::AddNN,
        &Interpreter::Arithmetic,
        &Interpreter::SkipNotEqualsReg,
        &Interpreter::StoreI,
        &Interpreter::JumpV0,
        &Interpreter::Rand,
        &Interpreter::Draw,
        &Interpreter::Key,
        &Interpreter::Advanced
    };

    _arithOpcodeTable =
    {
        &Interpreter::Store,
        &Interpreter::Or,
        &Interpreter::And,
        &Interpreter::Xor,
        &Interpreter::Add,
        &Interpreter::Sub,
        &Interpreter::Shr,
        &Interpreter::Sub2,
        &Interpreter::Shl
    };
}

Interpreter::~Interpreter()
{
}

void Interpreter::Tick()
{
    _opcode = Fetch();
    (this->*_mainOpcodeTable[_opcode.u])();
}

void Interpreter::Reset()
{
    _storage.Reset();
    _state.Reset();
    _display.Reset();
}

void Interpreter::Open(const string& rom)
{
    ifstream file(rom, std::ios::binary);
    int i = START;

    while (file.good() && i < _storage.RAM_SIZE)
        _storage.RAM[i++] = file.get();

    _state.PC = START;
}

Interpreter::Opcode Interpreter::Fetch()
{
    // Fetch operation code. Remember that they are two bytes big.
    uint16_t& PC    = _state.PC;
    uint16_t op     = (_storage.RAM[PC] << 8) | _storage.RAM[PC + 1];
    PC             += 2;
    assert(PC % 2 == 0);

    // Extract components of opcode.
    Opcode opcode;
    opcode.u    = (op >> 12) & 0xF;
    opcode.n    = op & 0xF;
    opcode.nn   = op & 0xFF;
    opcode.nnn  = op & 0xFFF;
    opcode.x    = (op >> 4) & 0xF;
    opcode.y    = (op >> 8) & 0xF;

    return opcode;
}

// OPERATIONS
void Interpreter::Nop()
{

}

inline void Interpreter::Skip()
{
    _state.PC += 2;
}

void Interpreter::Special()
{
    switch (_opcode.nnn)
    {
    case 0x0E0: Clear(); break;
    case 0x0EE: Return(); break;
    default: Call();
    }
}

void Interpreter::Call()
{
    throw exception("Not implemented yet!");
}

void Interpreter::Clear()
{
    throw exception("Not implemented yet!");
}

void Interpreter::Return()
{
    throw exception("Not implemented yet!");
}

void Interpreter::Jump()
{
    _state.PC = _opcode.nnn;
}

void Interpreter::Call2()
{
    throw exception("Not implemented yet!");
}

void Interpreter::SkipEqualsNN()
{
    GET_Vx();
    if (Vx == _opcode.nn)
        Skip();
}

void Interpreter::SkipNotEqualsNN()
{
    GET_Vx();
    if (Vx != _opcode.nn)
        Skip();
}

void Interpreter::SkipEqualsReg()
{
    GET_VxVy();
    if (Vx == Vy)
        Skip();
}

void Interpreter::StoreNN()
{
    GET_Vx();
    Vx = _opcode.nn;
}

void Interpreter::AddNN()
{
    // This may overflow!!! This is by design!
    GET_Vx();
    Vx += _opcode.nn;
}

void Interpreter::Arithmetic()
{
    auto& n = _opcode.n;

    if (n == 0xE)
        (this->*_arithOpcodeTable.back())();
    else
        (this->*_arithOpcodeTable[n])();
}

void Interpreter::Store()
{
    GET_VxVy();
    Vx = Vy;
}

void Interpreter::Or()
{
    GET_VxVy();
    Vx |= Vy;
}

void Interpreter::And()
{
    GET_VxVy();
    Vx &= Vy;
}

void Interpreter::Xor()
{
    GET_VxVy();
    Vx ^= Vy;
}

void Interpreter::Add()
{
    GET_VxVy();
    GET_VF();

    uint16_t res = Vx + Vy;
    Vx = (State::reg_t)res;
    VF = res >> 8;
}

void Interpreter::Sub()
{
    GET_VxVy();
    GET_VF();

    VF = Vy > Vx;
    Vx -= Vy;
}

void Interpreter::Shr()
{
    GET_VxVy();
    GET_VF();

    VF = Vx & 0x1;
    Vx = Vy >> 1;
}

void Interpreter::Sub2()
{
    GET_VxVy();
    GET_VF();

    VF = Vx > Vy;
    Vx = Vy - Vx;
}

void Interpreter::Shl()
{
    GET_VxVy();
    GET_VF();

    VF = Vy >> 7;
    Vx = Vy << 1;
}

void Interpreter::SkipNotEqualsReg()
{
    GET_VxVy();
    if (Vx != Vy)
        Skip();
}

void Interpreter::StoreI()
{
    _state.I = _opcode.nnn;
}

void Interpreter::JumpV0()
{
    auto& PC = _state.PC;
    PC = (_state.V[0] + _opcode.nnn) & 0xFFF;
}

void Interpreter::Rand()
{
    GET_VxVy();
    Vx = _state.Random() & _opcode.nn;
}

void Interpreter::Draw()
{
    throw exception("Not implemented yet!");
}

void Interpreter::Key()
{
    if (_opcode.nn == 0x9E)
        SkipKeyPressed();
    else if (_opcode.nn == 0xA1)
        SkipKeyNotPressed();
    else
        throw exception("Unknown opcode!");
}

void Interpreter::SkipKeyPressed()
{
    GET_Vx();
    if (_state.Keys[Vx & 0xF])
        Skip();
}

void Interpreter::SkipKeyNotPressed()
{
    GET_Vx();
    if (!_state.Keys[Vx & 0xF])
        Skip();
}

void Interpreter::Advanced()
{
    switch (_opcode.nn)
    {
    case 0x07: LoadDelayTimer(); break;
    case 0x0A: WaitKey(); break;
    case 0x15: SetDelayTimer(); break;
    case 0x18: SetSoundTimer(); break;
    case 0x1E: AddI(); break;
    case 0x29: FX29(); break;
    case 0x33: FX33(); break;
    case 0x55: FX55(); break;
    case 0x65: FX65(); break;
    default: throw exception("Unknown opcode!");
    }
}

void Interpreter::LoadDelayTimer()
{
    GET_Vx();
    Vx = _state.DelayTimer;
}

void Interpreter::WaitKey()
{
    throw exception("Not implemented yet!");
}

void Interpreter::SetDelayTimer()
{
    GET_Vx();
    _state.DelayTimer = Vx;
}

void Interpreter::SetSoundTimer()
{
    GET_Vx();
    _state.SoundTimer = Vx;
}

void Interpreter::AddI()
{
    GET_Vx();
    _state.I = Vx;
}

void Interpreter::FX29()
{
    throw exception("Not implemented yet!");
}

void Interpreter::FX33()
{
    throw exception("Not implemented yet!");
}

void Interpreter::FX55()
{
    throw exception("Not implemented yet!");
}

void Interpreter::FX65()
{
    throw exception("Not implemented yet!");
}