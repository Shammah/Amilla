#pragma once

#include <assert.h>
#include <string>
#include "Display.hpp"
#include "IResettable.hpp"
#include "Storage.hpp"
#include "State.hpp"

#define GET_Vx() \
    auto& Vx = _state.V[_opcode.x]; \

#define GET_Vy() \
    auto& Vy = _state.V[_opcode.y]; \

#define GET_VxVy() \
    GET_Vx(); \
    GET_Vy();

#define GET_VF() \
    auto& VF = _state.V[0xF]; \

namespace Chip8
{
    /**
     * A Chip8 interpreter is able to load a program
     * into a machine, and execute it's state machine.
     * This class is essentially the 'CPU'.
     */
    class Interpreter : IResettable
    {
    public:
        /** Starting location in memory of loaded programs. */
        static constexpr State::addr_t START = 0x200;
        static constexpr uint8_t NUM_MAIN_OPCODES = 0x10;
        static constexpr uint8_t NUM_ARITH_OPCODES = 0x9;

        /** An operation is just a function. */
        typedef void(Interpreter::*Operation)();

        /** A destructured operation code. */
        struct Opcode
        {
            uint16_t nnn;
            uint8_t nn, n;
            uint8_t u, x, y;

            std::string ToString() const
            {
                return "Not implemented yet!";
            }
        };

    public:
        Interpreter();
        virtual ~Interpreter();

    private:
        /** The machine used by the interpreter. */
        Storage _storage;

        /** The current state of the CPU. */
        State _state;

        /** The screen display. */
        Display _display;

        /** Current opcode to be executed. */
        Opcode _opcode;
        std::array<Operation, NUM_MAIN_OPCODES> _mainOpcodeTable;
        std::array<Operation, NUM_ARITH_OPCODES> _arithOpcodeTable;

    private:
        /**
         * Fetch next opcode and increment PC.
         *
         * @return The fetched opcode to be executed next.
         */
        Opcode Fetch();

    public:
        void Open(const std::string& rom);
        void Tick();
        void Reset();

        inline const Storage& GetStorage() const { return _storage; }
        inline const State& GetState() const { return _state; }
        inline const Display& GetDisplay() const { return _display; }

    private:
        /** OPERATIONS */
        void Nop();
        inline void Skip();

        /* 0    */ void Special();              /* Call any of the special functions. */
        /* 0NNN */ void Call();                 /* Execute machine language subroutine at address NNN. */
        /* 00E0 */ void Clear();                /* Clear the screen. */
        /* 00EE */ void Return();               /* Return from a subroutine. */

        /* 1NNN */ void Jump();                 /* Jump to address NNN. */
        /* 2NNN */ void Call2();                /* Execute subroutine starting at address NNN. */
        /* 3XNN */ void SkipEqualsNN();         /* Skip the following instruction if the value of register VX equals NN. */
        /* 4XNN	*/ void SkipNotEqualsNN();      /* Skip the following instruction if the value of register VX is not equal to NN. */
        /* 5XY0	*/ void SkipEqualsReg();        /* Skip the following instruction if the value of register VX is equal to the value of register VY. */
        /* 6XNN	*/ void StoreNN();              /* Store number NN in register VX. */
        /* 7XNN	*/ void AddNN();                /* Add the value NN to register VX. */
        
        /* 8    */ void Arithmetic();           /* Call any of the arithmetic functions. */
        /* 8XY0 */ void Store();                /* Store the value of register VY in register VX. */
        /* 8XY1 */ void Or();                   /* Set VX to VX OR VY. */
        /* 8XY2	*/ void And();                  /* Set VX to VX AND VY. */
        /* 8XY3 */ void Xor();                  /* Set VX to VX XOR VY. */
        /* 8XY4 */ void Add();                  /* Add the value of register VY to register VX.
                                                 * Set VF to 01 if a carry occurs.
                                                 * Set VF to 00 if a carry does not occur. */
        /* 8XY5 */ void Sub();                  /* Subtract the value of register VY from register VX.
                                                 * Set VF to 00 if a borrow occurs.
                                                 * Set VF to 01 if a borrow does not occur. */
        /* 8XY6	*/ void Shr();                  /* Store the value of register VY shifted right one bit in register VX.
                                                 * Set register VF to the least significant bit prior to the shift. */
        /* 8XY7 */ void Sub2();                 /* Set register VX to the value of VY minus VX
                                                 * Set VF to 00 if a borrow occurs.
                                                 * Set VF to 01 if a borrow does not occur. */
        /* 8XYE */ void Shl();                  /* Store the value of register VY shifted left one bit in register VX.
                                                 * Set register VF to the most significant bit prior to the shift. */

        /* 9XY0 */ void SkipNotEqualsReg();     /* Skip the following instruction if the value of register VX is not equal to the value of register VY. */
        /* ANNN */ void StoreI();               /* Store memory address NNN in register I. */
        /* BNNN */ void JumpV0();               /* Jump to address NNN + V0. */
        /* CXNN */ void Rand();                 /* Set VX to a random number with a mask of NN. */
        /* DXYN */ void Draw();                 /* Draw a sprite at position VX, VY with N bytes of sprite data starting at the address stored in I.
                                                 * Set VF to 01 if any set pixels are changed to unset, and 00 otherwise. */

        /* E    */ void Key();                  /* Call any of the keypress functions. */
        /* EX9E */ void SkipKeyPressed();       /* Skip the following instruction if the key corresponding to the hex value currently stored in register VX is pressed. */
        /* EXA1 */ void SkipKeyNotPressed();    /* Skip the following instruction if the key corresponding to the hex value currently stored in register VX is not pressed. */

        /* F    */ void Advanced();             /* Call any of the advanced functions. */
        /* FX07 */ void LoadDelayTimer();       /* Store the current value of the delay timer in register VX. */
        /* FX0A */ void WaitKey();              /* Wait for a keypress and store the result in register VX. */
        /* FX15 */ void SetDelayTimer();        /* Set the delay timer to the value of register VX. */
        /* FX18 */ void SetSoundTimer();        /* Set the sound timer to the value of register VX. */
        /* FX1E */ void AddI();                 /* Add the value stored in register VX to register I. */

        /* FX29 */ void FX29(); /* Set I to the memory address of the sprite data corresponding to the hexadecimal digit stored in register VX. */
        /* FX33 */ void FX33(); /* Store the binary - coded decimal equivalent of the value stored in register VX at addresses I, I + 1, and I + 2. */
        /* FX55 */ void FX55(); /* Store the values of registers V0 to VX inclusive in memory starting at address I. I is set to I + X + 1 after operation.. */
        /* FX65 */ void FX65(); /* Fill registers V0 to VX inclusive with the values stored in memory starting at address I. I is set to I + X + 1 after operation.. */
    };
}