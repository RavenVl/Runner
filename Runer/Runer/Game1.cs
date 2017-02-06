using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Runer
{
    /// <summary>
    /// Это главный тип игры
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Sprite run;
        Sprite idle;
        Sprite jump;
        Vector2 myPosition = new Vector2(10, 350);
        Vector2 oldPosition = Vector2.Zero;
        Sprite curentSprite;
        int Height;
        int Width;
        KeyboardState key;
        KeyboardState old_key;
        SpriteEffects curSE;
        Vector2 Speed = new Vector2(0, 0);
        int speedX = 3;
        bool isJumping = false;
        int Vn = 6;//начальная скорость прыжка
        float Tj = 0; //время прыжка
        Level curentLevel;
        int Dx = 0;//смещение экрана для прорисовки длинного уровня
        int Score = 0;
        SpriteFont font;
        Song musika;
        SoundEffect sound, sound1;
        Menu menu;
        enum GameState
        {
            Game,
            Menu
        }

        GameState gameState = GameState.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            run = new Sprite(Content);
            idle = new Sprite(Content);
            jump = new Sprite(Content);
            Width = graphics.PreferredBackBufferWidth = 500;
            Height = graphics.PreferredBackBufferHeight = 500;



        }

        /// <summary>
        /// Позволяет игре выполнить инициализацию, необходимую перед запуском.
        /// Здесь можно запросить нужные службы и загрузить неграфический
        /// контент.  Вызов base.Initialize приведет к перебору всех компонентов и
        /// их инициализации.
        /// </summary>
        protected override void Initialize()
        {
            // ЗАДАЧА: добавьте здесь логику инициализации
            menu = new Menu();
            Item startGame = new Item("New Game");
            Item resumeGame = new Item("Resume");
            Item exitGame = new Item("Exit");
            resumeGame.activ = false;

            startGame.Click += new EventHandler(startGame_Click);
            resumeGame.Click += new EventHandler(resumeGame_Click);
            exitGame.Click += new EventHandler(exitGame_Click);

            menu.Items.Add(startGame);
            menu.Items.Add(resumeGame);
            menu.Items.Add(exitGame);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent будет вызываться в игре один раз; здесь загружается
        /// весь контент.
        /// </summary>
        protected override void LoadContent()
        {
            // Создайте новый SpriteBatch, который можно использовать для отрисовки текстур.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            myPosition = new Vector2(Width / 2, Height - 65);
            run.LoadTexture("run", myPosition, new Vector2(148, 38), new Vector2(6, 1), 10);
            idle.LoadTexture("idle", myPosition, new Vector2(99, 38), new Vector2(4, 1), 10);
            jump.LoadTexture("jump", myPosition, new Vector2(208, 38), new Vector2(6, 1), 6);
            idle.Animation = true;
            run.Animation = true;
            jump.Animation = true;
            curSE = SpriteEffects.None;
            Speed.Y = (float)0.8;
            font = Content.Load<SpriteFont>("gameFont");
            //myPosition = new Vector2(Width/2, Height-2*idle.Size.Y);
            curentSprite = idle;
            menu.LoadContent(Content);
            curentLevel = new Level(1, Content);
            curentLevel.LoadLevel();
            curentLevel.BuildLevel();
            musika = Content.Load<Song>("musika");
            sound = Content.Load<SoundEffect>("sound");
            sound1 = Content.Load<SoundEffect>("sound1");
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.Play(musika);


            // ЗАДАЧА: используйте здесь this.Content для загрузки контента игры
        }

        /// <summary>
        /// UnloadContent будет вызываться в игре один раз; здесь выгружается
        /// весь контент.
        /// </summary>
        protected override void UnloadContent()
        {
            // ЗАДАЧА: выгрузите здесь весь контент, не относящийся к ContentManager
        }

        /// <summary>
        /// Позволяет игре запускать логику обновления мира,
        /// проверки столкновений, получения ввода и воспроизведения звуков.
        /// </summary>
        /// <param name="gameTime">Предоставляет моментальный снимок значений времени.</param>
        protected override void Update(GameTime gameTime)
        {

            if (gameState == GameState.Game)
                UpdateGameLogic(gameTime);
            else menu.Update();

            base.Update(gameTime);
        }

        private void UpdateGameLogic(GameTime gameTime)
{
            old_key = key;
            key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Escape))
            {
                gameState = GameState.Menu;
            }

            if (!isJumping)
            {
                curentSprite = idle;
                Speed.X = 0;

            }
            Vector2 newPosition = Vector2.Zero;
            if (key.IsKeyDown(Keys.Right) && !isJumping)
            {
                curentSprite = run;
                curSE = SpriteEffects.None;
                Speed.X = speedX;




            }

            if (key.IsKeyDown(Keys.Left) && !isJumping)
            {
                curentSprite = run;
                curSE = SpriteEffects.FlipHorizontally;
                Speed.X = -speedX;


            }


            //Проверка х 
            newPosition.X = myPosition.X + Speed.X;

            if (newPosition.X + curentSprite.FrameSize.X >= curentLevel.razmerX * 50)
            {
                newPosition.X = curentLevel.razmerX * 50 - curentSprite.FrameSize.X;
            }

            if (newPosition.X <= 0)
            {
                newPosition.X = 0;
            }

            oldPosition.X = myPosition.X;
            myPosition.X = newPosition.X;
            curentSprite.Position.X = myPosition.X;
            curentSprite.BB.Min = new Vector3(myPosition, 0);
            curentSprite.BB.Max = new Vector3(myPosition.X + 25, myPosition.Y + 38, 0);
            if (TestCollision(curentSprite.BB))
            {
                myPosition.X = oldPosition.X;

            }
            curentSprite.Position.X = myPosition.X;
            //Прооверка у
            // Speed.Y = Speed.Y + 2 * (float)0.1;
            if (key.IsKeyDown(Keys.Space) && !old_key.IsKeyDown(Keys.Space))
            {
                isJumping = true;

            }
            Speed.Y = 2 * Tj;
            if (isJumping)
            {
                curentSprite = jump;
                //Speed.X = 0;
                Speed.Y = -Vn + 2 * Tj;

            }

            Tj = Tj + (float)0.1;

            oldPosition.Y = myPosition.Y;
            newPosition.Y = myPosition.Y + Speed.Y;
            if (newPosition.Y + curentSprite.FrameSize.Y > Height)
            {
                newPosition.Y = Height - curentSprite.FrameSize.Y;
                Tj = 0;
                isJumping = false;

                Speed.Y = 0;
                jump.FrameCurent = new Vector2(0, 0);
            }


            myPosition.Y = newPosition.Y;
            curentSprite.Position.Y = myPosition.Y;
            curentSprite.BB.Min = new Vector3(myPosition, 0);
            curentSprite.BB.Max = new Vector3(myPosition.X + 25, myPosition.Y + 38, 0);
            if (TestCollision(curentSprite.BB))
            {
                myPosition.Y = oldPosition.Y;
                Tj = 0;
                isJumping = false;
                jump.FrameCurent = new Vector2(0, 0);
                jump.Position = myPosition;


            }
            curentSprite.Position.Y = myPosition.Y;




            int i = 0;
            while (i < curentLevel.diamonds.Count)
            {
                if (curentSprite.BB.Intersects(curentLevel.diamonds[i].box))
                {
                    Score += 10;
                    sound.Play();
                    curentLevel.diamonds.Remove(curentLevel.diamonds[i]);
                }
                i++;
            }

            int dxOld = Dx;
            Dx += (int)(curentSprite.Position.X) - Dx - Width / 2;
            if (Math.Abs(Dx - dxOld) > 50)
            {
                Dx = dxOld;
            }
            //               передвижение монстров
            Vector3 n_min;
            Vector3 n_max;
            foreach (Sprite s in curentLevel.monstrs)
            {



                float old_pos = s.Position.X;
                if (s.SE == SpriteEffects.None)
                {
                    s.Position.X += 1;
                }
                else
                {
                    s.Position.X -= 1;
                }

                n_min = new Vector3(s.Position.X, s.Position.Y, 0);
                n_max = new Vector3(s.Position.X + 50, s.Position.Y, 0);

                BoundingBox tb = new BoundingBox(n_min, n_max);

                if (TestCollision(tb))
                {
                    s.Position.X = old_pos;
                    if (s.SE == SpriteEffects.None)
                    {
                        s.SE = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        s.SE = SpriteEffects.None;
                    }
                }

                //             Проверка сваливания с платформы

                if (s.SE == SpriteEffects.None)
                {
                    n_min = new Vector3(s.Position.X + 50, s.Position.Y + 50, 0);
                    n_max = new Vector3(s.Position.X + 50, s.Position.Y + 100, 0);
                }
                else
                {
                    n_min = new Vector3(s.Position.X, s.Position.Y + 50, 0);
                    n_max = new Vector3(s.Position.X, s.Position.Y + 51, 0);
                }


                tb = new BoundingBox(n_min, n_max);

                if (!TestCollision(tb))
                {
                    s.Position.X = old_pos;
                    if (s.SE == SpriteEffects.None)
                    {
                        s.SE = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        s.SE = SpriteEffects.None;
                    }
                }



            }
            // 
            n_min = new Vector3(curentSprite.Position.X, curentSprite.Position.Y, 0);
            n_max = new Vector3(curentSprite.Position.X + 25, curentSprite.Position.Y + 38, 0);
            BoundingBox tb1 = new BoundingBox(n_min, n_max);

            foreach (Sprite m in curentLevel.monstrs)
            {
                n_min = new Vector3(m.Position.X, m.Position.Y, 0);
                n_max = new Vector3(m.Position.X + 50, m.Position.Y + 50, 0);

                BoundingBox tb3 = new BoundingBox(n_min, n_max);
                if (tb3.Intersects(tb1))
                {
                    sound1.Play();
                    Score = 0;
                    curentSprite = idle;
                    Speed.X = 0;
                    isJumping = false;
                    curentSprite.Position = new Vector2(Width / 2, Height - 65);
                    myPosition = curentSprite.Position;
                    Dx = 0;

                    MediaPlayer.Stop();
                    MediaPlayer.Play(musika);
                    curentLevel.Reset();
                    break;
                }
            }
}
        /// Вызывается, когда игра отрисовывается.
        /// </summary>
        /// <param name="gameTime">Предоставляет моментальный снимок значений времени.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // ЗАДАЧА: добавьте здесь код отрисовки

            if (gameState == GameState.Game)
                DrawGame(gameTime);
            else menu.Draw(spriteBatch);

            base.Draw(gameTime);


        }

        private void DrawGame(GameTime gameTime)
        {
            curentLevel.DrawLevel(spriteBatch, Dx, gameTime);
            spriteBatch.Begin();
            curentSprite.Draw(spriteBatch, gameTime, curSE, Dx);
            spriteBatch.DrawString(font, "Score : " + Score, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            foreach (Sprite s in curentLevel.monstrs)
            {
                s.Draw(spriteBatch, gameTime, s.SE, Dx);
            }

            spriteBatch.End();
        }

        public bool TestCollision(BoundingBox pos)
        {

            foreach (Level.Block b in curentLevel.bloks)
            {


                if (pos.Intersects(b.box) || pos.Contains(b.box) == ContainmentType.Contains)
                {
                    return true;
                }
            }
            return false;


        }

        public Rectangle RecDx(Rectangle r)
        {
            Rectangle r1 = r;
            r1.Offset(r.X - Dx, r.Y);
            return r1;


        }

        void startGame_Click(object sender, EventArgs E)
        {
            menu.Items[1].activ = true;
            gameState = GameState.Game;
            Score = 0;
            curentSprite = idle;
            Speed.X = 0;
            isJumping = false;
            curentSprite.Position = new Vector2(Width / 2, Height - 65);
            myPosition = curentSprite.Position;
            Dx = 0;
            curentLevel.Reset();

        }

        void resumeGame_Click(object sender, EventArgs E)
        {

            gameState = GameState.Game;

        }

        void exitGame_Click(object sender, EventArgs E)
        {

            this.Exit();
        }
    }
}
