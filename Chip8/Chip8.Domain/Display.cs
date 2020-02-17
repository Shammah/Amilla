using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Amilla.Chip8.Domain.Interfaces;

namespace Amilla.Chip8.Domain
{
    /// <summary>
    /// The graphics output section of the machine.
    /// 
    /// Each 'pixel' in the Chip8 machine is a byte,
    /// which actualy holds 8 pixels for the real screen.
    /// Thus, every bit on (1) must be converted into a
    /// white 32 bit pixel, and black if it's off (0).
    /// </summary>
    public class Display : IResettable, IEnumerable<bool>
    {
        public const int Width = 64;
        public const int Height = 32;
        public const int BytesPerSprite = 5;

        public Display()
        {
            this.Pixels = new bool[Width * Height];

            Reset();
        }

        /// <summary>
        /// The actual screen containing the pixels to render.
        /// </summary>
        public bool[] Pixels { get; }

        public bool this[int i]
        {
            get => this.Pixels[i];
            set => this.Pixels[i] = value;
        }

        public IEnumerator<bool> GetEnumerator() => this.Pixels.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Resets the entire screen to render nothing.
        /// </summary>
        public void Reset()
        {
            Array.Clear(this.Pixels, 0, Width * Height);
        }

        /// <summary>
        /// Each font character (0 .. F) are 5 * 8bit pixels.
        /// Each font character thus has a height of 5 rows.
        /// </summary>
        public static byte[] CreateFont()
        {
            return new byte[]
            {
                0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
                0x20, 0x60, 0x20, 0x20, 0x70, // 1
                0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
                0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
                0x90, 0x90, 0xF0, 0x10, 0x10, // 4
                0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
                0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
                0xF0, 0x10, 0x20, 0x40, 0x40, // 7
                0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
                0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
                0xF0, 0x90, 0xF0, 0x90, 0x90, // A
                0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
                0xF0, 0x80, 0x80, 0x80, 0xF0, // C
                0xE0, 0x90, 0x90, 0x90, 0xE0, // D
                0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
                0xF0, 0x80, 0xF0, 0x80, 0x80  // F
            };
        }
    }
}