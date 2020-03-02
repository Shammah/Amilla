using System.Collections.Generic;
using System.Linq;
using Amilla.Chip8.Domain.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Amilla.Chip8.MonoGame
{
    public class Chip8 : Game, IChip8Shell
    {
        public const int Width = 640;
        public const int Height = 480;

        private readonly GraphicsDeviceManager graphics;
        private readonly IDictionary<Keys, int> keyMapping;

        private SpriteBatch spriteBatch;
        private Texture2D chip8Texture;

        public Chip8(Domain.Machine chip8)
        {
            Emulator = chip8;

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Width,
                PreferredBackBufferHeight = Height
            };
            graphics.ApplyChanges();

            keyMapping = new Dictionary<Keys, int>()
            {
                { Keys.X, 0x0 },
                { Keys.D1, 0x1 },
                { Keys.D2, 0x2 },
                { Keys.D3, 0x3 },
                { Keys.Q, 0x4 },
                { Keys.W, 0x5 },
                { Keys.E, 0x6 },
                { Keys.A, 0x7 },
                { Keys.S, 0x8 },
                { Keys.D, 0x9 },
                { Keys.Z, 0xA },
                { Keys.C, 0xB },
                { Keys.D4, 0xC },
                { Keys.R, 0xD },
                { Keys.F, 0xE },
                { Keys.V, 0xF }
            };
        }

        public IChip8 Emulator { get; }
        public Domain.Machine Machine => (Domain.Machine)Emulator;

        public bool IsRunning { get; private set; }

        private void RenderChip8()
        {
            var pixels = Machine
                .Display
                .Select(p => p
                    ? Color.White
                    : Color.Black)
                .ToArray();

            chip8Texture.SetData(pixels);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            chip8Texture = new Texture2D(GraphicsDevice,
                Domain.Display.Width,
                Domain.Display.Height);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // Update keys.
            var kb = Keyboard.GetState();
            foreach (var button in keyMapping)
                Machine.State.Keys[button.Value] = kb.IsKeyDown(button.Key);

            if (Emulator.IsLoaded)
                Emulator.Tick();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            RenderChip8();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(chip8Texture,
                new Rectangle(0, 0, Width, Height),
                Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Start()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            Run();
        }

        public void Stop()
        {
            IsRunning = false;
            Exit();
        }
    }
}
