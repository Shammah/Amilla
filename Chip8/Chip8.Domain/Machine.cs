using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amilla.Chip8.Domain.Interfaces;

namespace Amilla.Chip8.Domain
{
    /// <summary>
    /// An actual Chip8 machine emulator.
    /// Basically a face class; it contains all various machine components.
    /// </summary>
    public class Machine : IChip8Emulator
    {
        private bool isLoaded;

        public Machine()
        {
            this.Memory = new Memory();
            this.State = new State();
            this.Display = new Display();
            this.CPU = new CPU(
                this.Memory,
                this.State,
                this.Display);

            this.isLoaded = false;
        }

        public Memory Memory { get; }
        public State State { get; }
        public Display Display { get; }
        public CPU CPU { get; }

        public bool IsLoaded
        {
            get => this.isLoaded;
            private set => this.isLoaded = value;
        }
        
        private void Init()
        {
            this.State.PC = Memory.Program;

            // Copy over the installed font into memory. This is such that
            // the I register can have a pointer to font memory.
            this.Memory.LoadFont(Display.CreateFont());
        }

        public void Tick()
        {
            if (this.CPU.WaitingForKey)
                this.CPU.CheckForKeys();
            else
                this.CPU.Execute(this.CPU.Fetch());
        }

        /// <summary>
        /// Hard reset of the entire machine.
        /// </summary>
        public void Reset()
        {
            this.Memory.Reset();
            this.State.Reset();
            this.Display.Reset();
        }

        public void LoadFromMemory(byte[] rom)
        {
            this.Memory.LoadProgram(rom);
            this.isLoaded = true;

            Init();
        }

        public void LoadFromMemory(IReadOnlyCollection<byte> rom)
        {
            LoadFromMemory(rom.ToArray());
        }

        public void LoadFromFile(FileInfo file)
        {
            if (!file.Exists)
                throw new ArgumentException($"ROM ({file.FullName}) does not exist!");

            var rom = File.ReadAllBytes(file.FullName).ToArray();
            LoadFromMemory(rom);
        }
    }
}