using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Runer
{
    class Sprite
    {
        public bool Visible; //видимость объекта
        public Vector2 Position;//координаты левого верхнего угла
        public Vector2 Size;//размеры
        public BoundingBox BB;//объект для просчета столкновений
        private Texture2D Picture; //картинка графически представляющая объект
        public Color Color = Color.White; //цвет которым рисуется объект
        private Vector2 ScaleFactor = new Vector2(1,1); //коэфициент сжатия растяжения
        private ContentManager Content; //для упрощения эквивалент менеджера в Game
        public Vector2 FrameCount = Vector2.One;//количество кадров по осям
        public Vector2 FrameSize;//размер одного кадра
        public Vector2 FrameCurent = Vector2.Zero;//текущий кадр на пересечении строки и столбца
        public bool Animation = false;//показывать анимацию
        public int FramesPerSecond;// количество кадров в секунду
        private double TimeFromLastFrame;//время с предыдущего кадра анимации
        private double TimeToNextFrame;//время до следующего кадра анимации
        public SpriteEffects SE = SpriteEffects.None;

        public Sprite(ContentManager content)
        {
            Content = content;
        }

        public void LoadTexture(string TextureName)
        {
            LoadTexture(TextureName, Vector2.Zero);

        }

        public void LoadTexture(string TextureName, Vector2 position)
        {
            LoadTexture(TextureName, position, new Vector2(64, 64)); 

        }

        public void LoadTexture(string TextureName, Vector2 position, Vector2 size)
        {
            LoadTexture(TextureName, position, size, Vector2.One);

        }

        public void LoadTexture(string TextureName, Vector2 position, Vector2 size, Vector2 frameCount)
        {

            LoadTexture(TextureName, position, size, frameCount, 24);
        }

        public void LoadTexture(string TextureName, Vector2 position, Vector2 size, Vector2 frameCount, int fps)
        {
            Picture = Content.Load<Texture2D>(TextureName);
            Position = position;
            Size = size;
            ScaleFactor.X = Size.X / Picture.Width;
            ScaleFactor.Y = Size.Y / Picture.Height;


            BB = new BoundingBox(new Vector3(Position, 0), new Vector3(Position.X + (Size.X * ScaleFactor.X), Position.Y + (Size.Y * ScaleFactor.Y), 0));


            FrameCount = frameCount;
            FrameSize = Size / FrameCount;
           
            int t = 1000 / fps;
            FramesPerSecond = fps;
            TimeFromLastFrame = 0;
            TimeToNextFrame = t;

            Visible = true;
            Animation = false;
        
        }

        public void Draw(SpriteBatch spriteBath, GameTime time, SpriteEffects sp, int dx)
        {
            if (Visible)
            {
                int frameSizeX = (int)(Picture.Width / FrameCount.X);
                int frameSizeY = (int)(Picture.Height / FrameCount.Y);

                int frameX = (int)(FrameCurent.X * frameSizeX);
                int frameY = (int)(FrameCurent.Y * frameSizeY);

                Rectangle rectangle = new Rectangle(frameX, frameY, frameSizeX, frameSizeY);

                
                //BB = new BoundingBox(new Vector3(Position, 0), new Vector3(Position.X + (Size.X * ScaleFactor.X), Position.Y + (Size.Y * ScaleFactor.Y), 0));
                Vector2 p1 = new Vector2(Position.X - dx, Position.Y); 
                //Position.X -= dx; 

                spriteBath.Draw(Picture, p1 , rectangle, Color, 0, Vector2.Zero, ScaleFactor, sp, 0);

                if (Animation)
                {
                    TimeFromLastFrame += time.ElapsedGameTime.Milliseconds;
                    if (TimeFromLastFrame >= TimeToNextFrame)
                    {
                        FrameCurent.X++;
                        TimeFromLastFrame = 0;
                    }
                    if (FrameCurent.X > FrameCount.X-1)
                    {
                        FrameCurent.X = 0;
                    }
                    
                }
 
            }
        
        }

       
    }
}
