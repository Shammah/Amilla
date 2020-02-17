using System;
using System.IO;
using Amilla.Chip8.Domain;

namespace Amilla.Chip8.MonoGame
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please put a ROM in the arguments.");
                return 1;
            }

            var chip8 = new Machine();
            var shell = new Chip8(chip8);

            shell.Emulator.LoadFromFile(new FileInfo(args[0]));
            shell.Start();

            return 0;
        }
    }
}
