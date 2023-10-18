using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Collision__8_
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int speed, points = 0, currentXCoord, currentYCoord, prevXCoord, prevYCoord, random;
        private bool coinVisible = true;
        private bool left, right, up, down, prevLeft, prevRight, prevUp, prevDown, remove;
        Random generator = new Random();
        private Texture2D _pacRight, _pacLeft, _pacUp, _pacDown, _pacCurrent, _coin, _exit, _wall;
        List<Rectangle> coins, walls, exits;
        Rectangle pacRect;
        KeyboardState keyboardState;
        MouseState mouseState;
        SpriteFont _pointCount;
        SoundEffect _collect;
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
            pacRect = new Rectangle(10, 10, 70, 70);
            exits = new List<Rectangle>();
            exits.Add(new Rectangle(800, 500, 90, 90));
            exits.Add(new Rectangle(10, 500, 90, 90));
            walls = new List<Rectangle>();
            walls.Add(new Rectangle(0, 250, 340, 75));
            walls.Add(new Rectangle(560, 250, 340, 75));
            walls.Add(new Rectangle(415, 400, 75, 200));
            speed = 4;
            points = 0;
            coins = new List<Rectangle>();
            random = generator.Next(7, 16);
            for (int i = 0; i < random; i++)
            {
                remove = false;
                coins.Add(new Rectangle(generator.Next(1, 839), generator.Next(1, 539), 40, 40));
                foreach (Rectangle wall in walls)
                {
                    if (coins[i].Intersects(wall))
                        remove = true;        
                }
                int number = coins.Count - 1;
                for (int a = 0; a < number; a++)
                {
                    if (coins[i].Intersects(coins[a]))
                        remove = true;
                }
                foreach (Rectangle exit in exits)
                {
                    if (coins[i].Intersects(exit))
                        remove = true;
                }
                if (coins[i].Intersects(pacRect))
                    remove = true;
                if (remove)
                {
                    coins.Remove(coins[i]);
                    i--;
                }
            }
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
            _collect = Content.Load<SoundEffect>("CoinCollectSound");
            _pointCount = Content.Load<SpriteFont>("Points");
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
            prevLeft = left;
            prevRight = right;
            prevUp = up;
            prevDown = down;
            left = false;
            right = false;
            up = false;
            down = false;
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                pacRect.X -= speed;
                _pacCurrent = _pacLeft;
                foreach (Rectangle wall in walls)
                {
                    if (pacRect.Intersects(wall))
                    {
                        pacRect.X += speed;
                    }
                }
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                pacRect.X += speed;
                _pacCurrent = _pacRight;
                foreach (Rectangle wall in walls)
                {
                    if (pacRect.Intersects(wall))
                    {
                        pacRect.X -= speed;
                    }
                }
                
            }
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                pacRect.Y -= speed;
                _pacCurrent = _pacUp;
                foreach (Rectangle wall in walls)
                {
                    if (pacRect.Intersects(wall))
                    {
                        pacRect.Y += speed;
                    }
                }
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                pacRect.Y += speed;
                _pacCurrent = _pacDown;
                foreach (Rectangle wall in walls)
                {
                    if (pacRect.Intersects(wall))
                    {
                        pacRect.Y -= speed;
                    }
                }
            }
            //Bounderies
            if (pacRect.Right >= _graphics.PreferredBackBufferWidth)
                pacRect.X -= speed;
            if (pacRect.Left <= 0)
                pacRect.X += speed;
            if (pacRect.Bottom >= _graphics.PreferredBackBufferHeight)
                pacRect.Y -= speed;
            if (pacRect.Top <= 0)
                pacRect.Y += speed;
            //Coins
            for (int i = 0; i < coins.Count; i++)
            {
                if (pacRect.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    points++;
                    _collect.Play();
                    i--;
                }
            }
            foreach (Rectangle exit in exits)
            {
                if (mouseState.LeftButton == ButtonState.Pressed & exit.Contains(mouseState.X, mouseState.Y) || exit.Contains(pacRect))
                    Exit();
            }


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
            foreach (Rectangle exit in exits)
                _spriteBatch.Draw(_exit, exit, Color.White);
            _spriteBatch.Draw(_pacCurrent, pacRect, Color.White);
            _spriteBatch.DrawString(_pointCount, points.ToString(), new Vector2(10, 10), Color.Black);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}