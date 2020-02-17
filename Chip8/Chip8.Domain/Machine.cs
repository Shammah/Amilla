using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amilla.Chip8.Domain.Interfaces;
using Amilla.Chip8.Domain.SeedWork;

namespace Amilla.Chip8.Domain
{
    /// <summary>
    /// An actual Chip8 machine emulator.
    /// Basically a face class; it contains all various machine components.
    /// </summary>
    public class Machine : ValueObject, IChip8Emulator
    {
        public Machine()
        {
            Memory = new Memory();
            State = new State();
            Display = new Display();
            CPU = new CPU(
                Memory,
                State,
                Display);

            IsLoaded = false;
        }

        public Memory Memory { get; }
        public State State { get; }
        public Display Display { get; }
        public CPU CPU { get; }

        public bool IsLoaded { get; private set; }
        
        private void Init()
        {
            State.PC = Memory.Program;

            // Copy over the installed font into memory. This is such that
            // the I register can have a pointer to font memory.
            Memory.LoadFont(Display.CreateFont());
        }

        public void Tick()
        {
            if (CPU.WaitingForKey)
                CPU.CheckForKeys();
            else
                CPU.Execute(CPU.Fetch());
        }

        /// <summary>
        /// Hard reset of the entire machine.
        /// </summary>
        public void Reset()
        {
            Memory.Reset();
            State.Reset();
            Display.Reset();
        }

        public void LoadFromMemory(byte[] rom)
        {
            Memory.LoadProgram(rom);
            IsLoaded = true;

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

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return State;
            yield return CPU;
            yield return Display;
            yield return Memory;
            yield return IsLoaded;
        }
    }
}