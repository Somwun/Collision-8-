using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading;

namespace Collision__8_
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int speed, points, currentXCoord, currentYCoord, prevXCoord, prevYCoord;
        private bool coinVisible = true;
        private Texture2D _pacRight, _pacLeft, _pacUp, _pacDown, _pacCurrent, _coin, _exit, _wall;
        List<Rectangle> coins, walls;
        Rectangle pacRect, exitRect;
        KeyboardState keyboardState;
        MouseState mouseState;
        SpriteFont pointCount;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.PreferredBackBufferWidth = 900;
            _graphics.ApplyChanges();
            pacRect = new Rectangle(10, 10, 80, 80);
            exitRect = new Rectangle(810, 510, 80, 80);
            walls = new List<Rectangle>();
            walls.Add(new Rectangle(0, 250, 350, 75));
            walls.Add(new Rectangle(550, 250, 350, 75));
            speed = 4;
            points = 0;
            coins = new List<Rectangle>();
            coins.Add(new Rectangle(400, 50, 50, 50));
            coins.Add(new Rectangle(475, 50, 50, 50));
            coins.Add(new Rectangle(200, 350, 50, 50));
            coins.Add(new Rectangle(400, 350, 50, 50));
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pacUp = Content.Load<Texture2D>("pac_up");
            _pacDown = Content.Load<Texture2D>("pac_down");
            _pacLeft = Content.Load<Texture2D>("pac_left");
            _pacRight = Content.Load<Texture2D>("pac_right");
            _pacCurrent = Content.Load<Texture2D>("pac_right");
            _coin = Content.Load<Texture2D>("coin");
            _exit = Content.Load<Texture2D>("hobbit_door");
            _wall = Content.Load<Texture2D>("rock_barrier");
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            prevXCoord = currentXCoord;
            prevYCoord = currentYCoord;
            //Movement
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                pacRect.X -= speed;
                _pacCurrent = _pacLeft;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                pacRect.X += speed;
                _pacCurrent = _pacRight;
            }
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                pacRect.Y -= speed;
                _pacCurrent = _pacUp;
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                pacRect.Y += speed;
                _pacCurrent = _pacDown;
            }
            //Bounderies
            if (pacRect.Right >= _graphics.PreferredBackBufferWidth)
                pacRect.X -= 3;
            if (pacRect.Left <= 0)
                pacRect.X += 3;
            if (pacRect.Bottom >= _graphics.PreferredBackBufferHeight)
                pacRect.Y -= 3;
            if (pacRect.Top <= 0)
                pacRect.Y += 3;
            for (int i = 0; i < walls.Count; i++)
            {
                if (pacRect.Intersects(walls[i]))
                {
                    pacRect.Y = prevYCoord;
                    pacRect.X = prevXCoord;
                }
            }
            //Collisions
            for (int i = 0; i < coins.Count; i++)
            {
                if (pacRect.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    points++;
                    i--;
                }
            }
            if (mouseState.LeftButton == ButtonState.Pressed & exitRect.Contains(mouseState.X, mouseState.Y))
                Exit();
            currentXCoord = pacRect.X;
            currentYCoord = pacRect.Y;
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            foreach (Rectangle coin in coins)
                _spriteBatch.Draw(_coin, coin, Color.White);
            foreach (Rectangle wall in walls)
                _spriteBatch.Draw(_wall, wall, Color.White);
            _spriteBatch.Draw(_pacCurrent, pacRect, Color.White);
            _spriteBatch.Draw(_exit, exitRect, Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}