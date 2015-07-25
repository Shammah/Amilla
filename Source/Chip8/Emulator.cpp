#include <fstream>
#include "Emulator.hpp"
using namespace std;
using namespace Chip8;

Emulator::Emulator()
{
    _mainOpcodeTable =
    {
        &Emulator::Special,
        &Emulator::Jump,
        &Emulator::Call2,
        &Emulator::SkipEqualsNN,
        &Emulator::SkipNotEqualsNN,
        &Emulator::SkipEqualsReg,
        &Emulator::StoreNN,
        &Emulator::AddNN,
        &Emulator::Arithmetic,
        &Emulator::SkipNotEqualsReg,
        &Emulator::StoreI,
        &Emulator::JumpV0,
        &Emulator::Rand,
        &Emulator::Draw,
        &Emulator::Key,
        &Emulator::Advanced
    };

    _arithOpcodeTable =
    {
        &Emulator::Store,
        &Emulator::Or,
        &Emulator::And,
        &Emulator::Xor,
        &Emulator::Add,
        &Emulator::Sub,
        &Emulator::Shr,
        &Emulator::Sub2,
        &Emulator::Shl
    };
}

Emulator::~Emulator()
{
}

void Emulator::Tick()
{
    _opcode = Fetch();
    (this->*_mainOpcodeTable[_opcode.u])();
}

void Emulator::Reset()
{
    _storage.Reset();
    _state.Reset();
    _display.Reset();
}

void Emulator::Open(const string& rom)
{
    ifstream file(rom, std::ios::binary);
    int i = START;

    while (file.good() && i < _storage.RAM_SIZE)
        _storage.RAM[i++] = file.get();

    _state.PC = START;

    // Copy over the installed font into memory. This is such that
    // the I register can have a pointer to font memory.
    auto fontBase = _storage.RAM.begin();
    std::advance(fontBase, FONT_BASE);
    std::copy_n(_display.Font.begin(), _display.FONT_MEMORY_SIZE, fontBase);
}

Emulator::Opcode Emulator::Fetch()
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
void Emulator::Nop()
{

}

inline void Emulator::Skip()
{
    _state.PC += 2;
}

void Emulator::Special()
{
    switch (_opcode.nnn)
    {
    case 0x0E0: Clear(); break;
    case 0x0EE: Return(); break;
    default: Call();
    }
}

void Emulator::Call()
{
    throw exception("Not implemented yet!");
}

void Emulator::Clear()
{
    for (auto& pixel : _display.Pixels)
        pixel = 0;
}

void Emulator::Return()
{
    _state.PC = _state.Stack[--_state.SP];
}

void Emulator::Jump()
{
    _state.PC = _opcode.nnn;
}

void Emulator::Call2()
{
    _state.Stack[_state.SP++] = _state.PC;
    _state.PC = _opcode.nnn;
}

void Emulator::SkipEqualsNN()
{
    GET_Vx();
    if (Vx == _opcode.nn)
        Skip();
}

void Emulator::SkipNotEqualsNN()
{
    GET_Vx();
    if (Vx != _opcode.nn)
        Skip();
}

void Emulator::SkipEqualsReg()
{
    GET_VxVy();
    if (Vx == Vy)
        Skip();
}

void Emulator::StoreNN()
{
    GET_Vx();
    Vx = _opcode.nn;
}

void Emulator::AddNN()
{
    // This may overflow!!! This is by design!
    GET_Vx();
    Vx += _opcode.nn;
}

void Emulator::Arithmetic()
{
    auto& n = _opcode.n;

    if (n == 0xE)
        (this->*_arithOpcodeTable.back())();
    else
        (this->*_arithOpcodeTable[n])();
}

void Emulator::Store()
{
    GET_VxVy();
    Vx = Vy;
}

void Emulator::Or()
{
    GET_VxVy();
    Vx |= Vy;
}

void Emulator::And()
{
    GET_VxVy();
    Vx &= Vy;
}

void Emulator::Xor()
{
    GET_VxVy();
    Vx ^= Vy;
}

void Emulator::Add()
{
    GET_VxVy();
    GET_VF();

    uint16_t res = Vx + Vy;
    Vx = (State::reg_t)res;
    VF = res >> 8;
}

void Emulator::Sub()
{
    GET_VxVy();
    GET_VF();

    VF = Vy > Vx;
    Vx -= Vy;
}

void Emulator::Shr()
{
    GET_VxVy();
    GET_VF();

    VF = Vx & 0x1;
    Vx = Vy >> 1;
}

void Emulator::Sub2()
{
    GET_VxVy();
    GET_VF();

    VF = Vx > Vy;
    Vx = Vy - Vx;
}

void Emulator::Shl()
{
    GET_VxVy();
    GET_VF();

    VF = Vy >> 7;
    Vx = Vy << 1;
}

void Emulator::SkipNotEqualsReg()
{
    GET_VxVy();
    if (Vx != Vy)
        Skip();
}

void Emulator::StoreI()
{
    _state.I = _opcode.nnn;
}

void Emulator::JumpV0()
{
    auto& PC = _state.PC;
    PC = (_state.V[0] + _opcode.nnn) & 0xFFF;
}

void Emulator::Rand()
{
    GET_VxVy();
    Vx = _state.Random() & _opcode.nn;
}

void Emulator::Draw()
{
    throw exception("Not implemented yet!");
}

void Emulator::Key()
{
    if (_opcode.nn == 0x9E)
        SkipKeyPressed();
    else if (_opcode.nn == 0xA1)
        SkipKeyNotPressed();
    else
        throw exception("Unknown opcode!");
}

void Emulator::SkipKeyPressed()
{
    GET_Vx();
    if (_state.Keys[Vx & 0xF])
        Skip();
}

void Emulator::SkipKeyNotPressed()
{
    GET_Vx();
    if (!_state.Keys[Vx & 0xF])
        Skip();
}

void Emulator::Advanced()
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

void Emulator::LoadDelayTimer()
{
    GET_Vx();
    Vx = _state.DelayTimer;
}

void Emulator::WaitKey()
{
    throw exception("Not implemented yet!");
}

void Emulator::SetDelayTimer()
{
    GET_Vx();
    _state.DelayTimer = Vx;
}

void Emulator::SetSoundTimer()
{
    GET_Vx();
    _state.SoundTimer = Vx;
}

void Emulator::AddI()
{
    GET_Vx();
    _state.I = Vx;
}

void Emulator::FX29()
{
    GET_Vx();
    _state.I = FONT_BASE + (Vx & 0xf) * _display.BYTES_PER_SPRITE;
}

void Emulator::FX33()
{
    throw exception("Not implemented yet!");
}

void Emulator::FX55()
{
    throw exception("Not implemented yet!");
}

void Emulator::FX65()
{
    throw exception("Not implemented yet!");
}