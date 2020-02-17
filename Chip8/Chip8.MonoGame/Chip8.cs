using System.Collections.Generic;
using System.Linq;
using Amilla.Chip8.Domain.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Amilla.Chip8.MonoGame
{
    public class Chip8 : Game, IChip8EmulatorShell
    {
        public const int Width = 640;
        public const int Height = 480;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private IDictionary<Keys, int> keyMapping;

        private Texture2D chip8Texture;

        private bool isRunning;

        public Chip8(Domain.Machine chip8)
        {
            this.Emulator = chip8;

            this.graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Width,
                PreferredBackBufferHeight = Height
            };
            this.graphics.ApplyChanges();

            this.keyMapping = new Dictionary<Keys, int>()
            {
                { Keys.D0, 0x0 },
                { Keys.D1, 0x1 },
                { Keys.D2, 0x2 },
                { Keys.D3, 0x3 },
                { Keys.D4, 0x4 },
                { Keys.D5, 0x5 },
                { Keys.D6, 0x6 },
                { Keys.D7, 0x7 },
                { Keys.D8, 0x8 },
                { Keys.D9, 0x9 },
                { Keys.A, 0xA },
                { Keys.B, 0xB },
                { Keys.C, 0xC },
                { Keys.D, 0xD },
                { Keys.E, 0xE },
                { Keys.F, 0xF }
            };
        }

        public IChip8Emulator Emulator { get; }
        public Domain.Machine Machine => (Domain.Machine)this.Emulator;

        public bool IsRunning
        {
            get => this.isRunning;
            private set => this.isRunning = value;
        }

        private void RenderChip8()
        {
            var pixels = this.Machine
                .Display
                .Select(p => p
                    ? Color.White
                    : Color.Black)
                .ToArray();

            this.chip8Texture.SetData(pixels);
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.chip8Texture = new Texture2D(this.GraphicsDevice,
                Domain.Display.Width,
                Domain.Display.Height);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // Update keys.
            var kb = Keyboard.GetState();
            foreach (var button in this.keyMapping)
                this.Machine.State.Keys[button.Value] = kb.IsKeyDown(button.Key);

            if (this.Emulator.IsLoaded)
                this.Emulator.Tick();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            RenderChip8();

            this.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            this.spriteBatch.Draw(this.chip8Texture,
                new Rectangle(0, 0, Width, Height),
                Color.White);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Start()
        {
            if (this.IsRunning)
                return;

            this.IsRunning = true;
            Run();
        }

        public void Stop()
        {
            this.IsRunning = false;
            Exit();
        }
    }
}
