using System;
using System.Linq;
using Amilla.Chip8.Domain.Exceptions;

namespace Amilla.Chip8.Domain
{
    /// <summary>
    /// The one and only class that reads executes operations.
    /// </summary>
    public class CPU
    {
        public const int NumMainOpcodes = 0x10;
        public const int NumArithOpcodes = 0x9;

        private Action[] mainOpcodeTable;
        private Action[] arithOpcodeTable;

        private Opcode opcode;

        public CPU(Memory memory,
            State state,
            Display display)
        {
            this.Memory = memory;
            this.State = state;
            this.Display = display;

            this.WaitingForKey = false;

            this.mainOpcodeTable = new Action[]
            {
                Special,
                Jump,
                Call2,
                SkipEqualsNN,
                SkipNotEqualsNN,
                SkipEqualsReg,
                StoreNN,
                AddNN,
                Arithmetic,
                SkipNotEqualsReg,
                StoreI,
                JumpV0,
                Rand,
                Draw,
                Key,
                Advanced
            };

            this.arithOpcodeTable = new Action[]
            {
                Store,
                Or,
                And,
                Xor,
                Add,
                Sub,
                Shr,
                Sub2,
                Shl
            };
        }

        public Memory Memory { get; }
        public State State { get; }
        public Display Display { get; }

        /// <summary>
        /// When this value is true, all opcode execution does nothing, unless set back to false;
        /// </summary>
        public bool WaitingForKey { get; set; }

        /// <summary>
        /// Fetches the next <see cref="Opcode"/> from the <see cref="this.Memory"/>.
        /// This increments <see cref="this.State.PC"/> by 2.
        /// </summary>
        /// <returns>The newly fetched <see cref="Opcode"/> to execute.</returns>
        public Opcode Fetch()
        {
            var hi = this.Memory[this.State.PC];
            var lo = this.Memory[this.State.PC + 1];
            var op = (hi << 8) | lo;

            this.State.PC += 2;

            return new Opcode((ushort)op);
        }

        public void Execute(Opcode op)
        {
            this.opcode = op;
            this.mainOpcodeTable[op.U]();
        }

        private byte Vx
        {
            get => this.State.V[this.opcode.X];
            set => this.State.V[this.opcode.X] = value;
        }

        private byte Vy
        {
            get => this.State.V[this.opcode.Y];
            set => this.State.V[this.opcode.Y] = value;
        }

        private byte VF
        {
            get => this.State.V[0xF];
            set => this.State.V[0xF] = value;
        }

        private void Nop() { }

        /// <summary>
        /// Skips the state to the next instruction location.
        /// </summary>
        private void Skip()
        {
            this.State.PC += 2;
        }

        /// <summary>
        /// Checks if a key was pressed, and if pressed puts the key value
        /// into <see cref="Vx"/> and sets <see cref="WaitingForKey"/> to false.
        /// </summary>
        public void CheckForKeys()
        {
            for (byte k = 0; k < State.NumKeys; k++)
            {
                if (this.State.Keys[k])
                {
                    this.Vx = k;
                    this.WaitingForKey = false;
                }
            }
        }

        #region Opcodes

        /// <summary>
        /// 0
        /// Call any of the special functions. 
        /// </summary>
        private void Special()
        {
            switch (this.opcode.NNN)
            {
                case 0x0E0: Clear(); break;
                case 0x0EE: Return(); break;
                default: Call(); break;
            }
        }

        /// <summary>
        /// 0NNN
        /// Execute machine language subroutine at address NNN.
        /// </summary>
        /// <remarks>
        /// This instruction is only used on the old computers on which Chip-8 was originally implemented. It is ignored by modern interpreters.
        /// </remarks>
        private void Call() { }

        /// <summary>
        /// 00E0
        /// Clear the screen.
        /// </summary>
        private void Clear()
        {
            this.Display.Reset();
        }

        /// <summary>
        /// 00EE
        /// Return from a subroutine.
        /// </summary>
        private void Return()
        {
            this.State.PC = this.State.Stack[--this.State.SP];
        }


        /// <summary>
        /// 1NNN
        /// Jump to address.
        /// </summary>
        private void Jump()
        {
            this.State.PC = this.opcode.NNN;
        }


        /// <summary>
        /// 2NNN
        /// Execute machine language subroutine at address NNN.
        /// </summary>
        void Call2()
        {
            this.State.Stack[this.State.SP++] = this.State.PC;
            this.State.PC = this.opcode.NNN;
        }

        /// <summary>
        /// 3XNN
        /// Skip the following instruction if the value of register VX equals NN.
        /// </summary
        private void SkipEqualsNN()
        {
            if (this.Vx == this.opcode.NN)
                Skip();
        }


        /// <summary>
        /// 4XNN
        /// Skip the following instruction if the value of register VX is not equal to NN.
        /// </summary
        private void SkipNotEqualsNN()
        {
            if (this.Vx != this.opcode.NN)
                Skip();
        }


        /// <summary>
        /// 5XY0
        /// Skip the following instruction if the value of register VX is equal to the value of register VY.
        /// </summary
        private void SkipEqualsReg()
        {
            if (this.Vx == this.Vy)
                Skip();
        }


        /// <summary>
        /// 6XNN
        /// Store number NN in register VX.
        /// </summary
        private void StoreNN()
        {
            this.Vx = this.opcode.NN;
        }


        /// <summary>
        /// 7XNN
        /// Add the value NN to register VX.
        /// This may overflow VX, which is by design.
        /// </summary
        private void AddNN()
        {
            this.Vx += this.opcode.NN;
        }


        /// <summary>
        /// 8   
        /// Call any of the arithmetic functions.
        /// </summary
        private void Arithmetic()
        {
            var n = this.opcode.N;

            if (n == 0xE)
                this.arithOpcodeTable.Last()();
            else
                this.arithOpcodeTable[n]();
        }

        /// <summary>
        /// 8XY0
        /// Store the value of register VY in register VX.
        /// </summary
        private void Store()
        {
            this.Vx = this.Vy;
        }

        /// <summary>
        /// 8XY1
        /// Set VX to VX OR VY.
        /// </summary
        private void Or()
        {
            this.Vx |= this.Vy;
        }

        /// <summary>
        /// 8XY2
        /// Set VX to VX AND VY.
        /// </summary
        private void And()
        {
            this.Vx &= this.Vy;
        }

        /// <summary>
        /// 8XY3
        /// Set VX to VX XOR VY.
        /// </summary
        private void Xor()
        {
            this.Vx ^= this.Vy;
        }

        /// <summary>
        /// 8XY4
        /// Add the value of register VY to register VX.
        /// Set VF to 01 if a carry occurs.
        /// Set VF to 00 if a carry does not occur.
        /// </summary
        private void Add()
        {
            var res = this.Vx + this.Vy;

            this.Vx = (byte)res;
            this.VF = (byte)((res >> 8) > 0 ? 1 : 0);
        }

        /// <summary>
        /// 8XY5
        /// Subtract the value of register VY from register VX.
        /// Set VF to 00 if a borrow occurs.
        /// Set VF to 01 if a borrow does not occur.
        /// </summary
        private void Sub()
        {
            this.VF = (byte)((this.Vy > this.Vx) ? 1 : 0);
            this.Vx -= this.Vy;
        }

        /// <summary>
        /// 8XY6
        /// Store the value of register VX shifted right one bit in register VX.
        /// Set register VF to the least significant bit prior to the shift.
        /// </summary
        private void Shr()
        {
            this.VF = (byte)(this.Vx & 0x1);
            this.Vx >>= 1;
        }

        /// <summary>
        /// 8XY7
        /// Set register VX to the value of VY minus VX
        /// Set VF to 00 if a borrow occurs.
        /// Set VF to 01 if a borrow does not occur.
        /// </summary
        private void Sub2()
        {
            this.VF = (byte)((this.Vy > this.Vx) ? 1 : 0);
            this.Vx = (byte)(this.Vy - this.Vx);
        }

        /// <summary>
        /// 8XYE
        /// Store the value of register VX shifted left one bit in register VX.
        /// Set register VF to the most significant bit prior to the shift.
        /// </summary
        private void Shl()
        {
            this.VF = (byte)((this.Vx >> 7) & 0x1);
            this.Vx <<= 1;
        }


        /// <summary>
        /// 9XY0
        /// Skip the following instruction if the value of register VX is not equal to the value of register VY.
        /// </summary
        private void SkipNotEqualsReg()
        {
            if (this.Vx != this.Vy)
                Skip();
        }


        /// <summary>
        /// ANNN
        /// Store memory address NNN in register I.
        /// </summary
        private void StoreI()
        {
            this.State.I = this.opcode.NNN;
        }


        /// <summary>
        /// BNNN
        /// Jump to address NNN + V0.
        /// </summary
        private void JumpV0()
        {
            this.State.PC = (ushort)(this.State.V[0] + this.opcode.NNN);
        }


        /// <summary>
        /// CXNN
        /// Set VX to a random number with a mask of NN.
        /// </summary
        private void Rand()
        {
            this.Vx = (byte)(this.State.Random() & this.opcode.NN);
        }


        /// <summary>
        /// DXYN
        /// Draw a sprite at position VX, VY with N bytes of sprite data starting at the address stored in I.
        /// Set VF to 01 if any set pixels are changed to unset, and 00 otherwise.
        /// </summary
        private void Draw()
        {
            /**
             * Draws a sprite at coordinate (VX, VY) that has a width of 8 pixels
             * and a height of N pixels. Each row of 8 pixels is read as bit-coded
             * (with the most significant bit of each byte displayed on the left)
             * starting from memory location I; I value doesn't change after the
             * execution of this instruction. As described above, VF is set to 1 if
             * any screen pixels are flipped from set to unset when the sprite is drawn,
             * and to 0 if that doesn't happen.
             */
            for (byte b = 0; b < this.opcode.N; b++)
            {
                var pixel = this.Memory[Memory.Font + this.State.I + b];
                for (var bit = 0; bit < 8; bit++) // Iter through each bit
                {
                    if ((pixel & (0b10000000 >> bit)) > 0)
                    {
                        var displayAddr = this.Vx + bit + (this.Vy + b) * Display.Width;
                        if (this.Display[displayAddr] == true)
                            this.VF = 1;

                        this.Display[displayAddr] ^= true;
                    }
                }
            }
        }


        /// <summary>
        /// E
        /// Call any of the keypress functions.
        /// </summary
        private void Key()
        {
            var nn = this.opcode.NN;

            if (nn == 0x9E)
                SkipKeyPressed();
            else if (nn == 0xA1)
                SkipKeyNotPressed();
            else
                throw new UnknownOpcodeException(this.opcode);
        }

        /// <summary>
        /// EX9E
        /// Skip the following instruction if the key corresponding to the hex value currently stored in register VX is pressed.
        /// </summary
        private void SkipKeyPressed()
        {
            if (this.State.Keys[this.Vx])
                Skip();
        }

        /// <summary>
        /// EXA1
        /// Skip the following instruction if the key corresponding to the hex value currently stored in register VX is not pressed.
        /// </summary
        private void SkipKeyNotPressed()
        {
            if (!this.State.Keys[this.Vx])
                Skip();
        }


        /// <summary>
        /// F   
        /// Call any of the advanced functions.
        /// </summary
        private void Advanced()
        {
            switch (this.opcode.NN)
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
                default: throw new UnknownOpcodeException(this.opcode);
            }
        }

        /// <summary>
        /// FX07
        /// Store the current value of the delay timer in register VX.
        /// </summary
        private void LoadDelayTimer()
        {
            this.Vx = this.State.DelayTimer;
        }

        /// <summary>
        /// FX0A
        /// Wait for a keypress and store the result in register VX.
        /// </summary
        private void WaitKey()
        {
            this.WaitingForKey = true;
        }

        /// <summary>
        /// FX15
        /// Set the delay timer to the value of register VX.
        /// </summary
        private void SetDelayTimer()
        {
            this.State.DelayTimer = this.Vx;
        }

        /// <summary>
        /// FX18
        /// Set the sound timer to the value of register VX.
        /// </summary
        private void SetSoundTimer()
        {
            this.State.SoundTimer = this.Vx;
        }

        /// <summary>
        /// FX1E
        /// Add the value stored in register VX to register I.
        /// </summary
        private void AddI()
        {
            this.State.I += this.Vx;
        }


        /// <summary>
        /// FX29
        /// Set I to the memory address of the sprite data corresponding to the hexadecimal digit stored in register VX.
        /// </summary
        private void FX29()
        {
            this.State.I = (ushort)(Memory.Font + this.Vx * Display.BytesPerSprite);
        }

        /// <summary>
        /// FX33
        /// Store the binary-coded decimal equivalent of the value stored in register VX at addresses I, I + 1, and I + 2.
        /// </summary
        private void FX33()
        {
            this.Memory[this.State.I + 0] = (byte)((this.Vx / 100) % 10);
            this.Memory[this.State.I + 1] = (byte)((this.Vx / 10) % 10);
            this.Memory[this.State.I + 2] = (byte)((this.Vx / 1) % 10);
        }

        /// <summary>
        /// FX55
        /// Store the values of registers V0 to VX inclusive in memory starting at address I. I is set to I + X + 1 after operation..
        /// </summary
        private void FX55()
        {
            Array.Copy(this.State.V, 0, this.Memory.RAM, this.State.I, this.opcode.X + 1);
            this.State.I += this.opcode.X;
            this.State.I++;
        }

        /// <summary>
        /// FX65
        /// Fill registers V0 to VX inclusive with the values stored in memory starting at address I. I is set to I + X + 1 after operation..
        /// </summary
        private void FX65()
        {
            Array.Copy(this.Memory.RAM, this.State.I, this.State.V, 0, this.opcode.X + 1);
            this.State.I += this.opcode.X;
            this.State.I++;
        }

        #endregion
    }
}