using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Runer
{
    class Level
    {

       
        Texture2D texture1;
        Texture2D texture2;
        Texture2D textureZ;
        Texture2D textureM;

        string[] strings;
        public int razmerX;
        public List<Sprite> monstrs = new List<Sprite>();
        ContentManager content;
       
        public class Block
	{
		//Rectangle rect;//куда выводить блок
            //Texture2D textureBlock;//техтура блока
            public Rectangle r;
            public Texture2D texture1;
            public BoundingBox box;

            public Block(Rectangle r, Texture2D texture1)
            {
                // TODO: Complete member initialization
                this.r = r;
                this.texture1 = texture1;
                this.box = new BoundingBox(new Vector3(r.X, r.Y, 0), new Vector3(r.X + 25, r.Y + 25, 0));
            }
            

	}

        public class Diamond
        {
            //Rectangle rect;//куда выводить блок
            //Texture2D textureBlock;//техтура блока
            public Rectangle r;
            public Texture2D textureZ;
            public BoundingBox box;

            public Diamond(Rectangle r, Texture2D texture1)
            {
                // TODO: Complete member initialization
                this.r = r;
                this.textureZ = texture1;
                this.box = new BoundingBox(new Vector3(r.X, r.Y, 0), new Vector3(r.X + 25, r.Y + 25, 0));
            }


        }



        int CurentLevel;
        public List<Block> bloks = new List<Block>();
        public List<Diamond> diamonds = new List<Diamond>();
        //private int p;
        //private ContentManager Content;

        

        public  Level(int current_level, ContentManager Content)
        {
            content = Content;
            CurentLevel = current_level;
            texture2 = Content.Load<Texture2D>("earth_00001") ;
            texture1 = Content.Load<Texture2D>("kirpich_00001");
            textureZ = Content.Load<Texture2D>("zvezda_00001");
            textureM = Content.Load<Texture2D>("monstr_run");
        }

        public void BuildLevel()
        {
            
            int x = 0;
            int y = 0;

            foreach (string s  in strings)
            {
                razmerX = strings[0].Length;
                foreach (char c in s)
                {
                    if (c=='x')
                    {
                        Rectangle r= new Rectangle(x,y,25,25);
                        Block tempBlock = new Block(r, texture1);
                        bloks.Add(tempBlock);
                         
                    }

                    if (c == 'y')
                    {
                        Rectangle r = new Rectangle(x, y, 25, 25);
                        Block tempBlock = new Block(r, texture2);
                        bloks.Add(tempBlock);
                        
                    }

                    if (c == 'z')
                    {
                        Rectangle r = new Rectangle(x, y, 25, 25);
                        Diamond d = new Diamond(r,textureZ);
                        diamonds.Add(d);

                    }

                    if (c == 'm')
                    {
                        Vector2 r = new Vector2(x,y-25);
                        Sprite m = new Sprite(content);
                        m.LoadTexture("monstr_run", r, new Vector2(300, 50), new Vector2(5,1), 10);
                        m.Animation = true;
                        monstrs.Add(m);
                     }

                    x += 25;
                }
                x = 0;
                y += 25;
            }

        }

        public void LoadLevel()
        {
           strings = File.ReadAllLines ("content/level"+CurentLevel+".txt");
        
        }

        public void DrawLevel(SpriteBatch spriteBatch, int dx, GameTime gameTime)
        {
            int dy;
            spriteBatch.Begin();

            foreach (Block b in bloks)
            {
                Rectangle r1 = b.r;
                r1.Offset(-dx, 0); 
                spriteBatch.Draw(b.texture1, r1, Color.White);
            }

            foreach (Diamond d in diamonds)
            {
                Rectangle r1 = d.r;
                r1.Offset(-dx, 0);
                float t = (float)gameTime.TotalGameTime.TotalSeconds*3 + d.r.X;
                dy = (int)(Math.Sin(t) * 10);
                Rectangle r2 = new Rectangle(r1.X, r1.Y + dy, r1.Width, r1.Height);

                spriteBatch.Draw(d.textureZ, r2, Color.White);
            }

            

            spriteBatch.End();
        }

        public void Reset()
        {

            bloks.Clear();
            diamonds.Clear();
            monstrs.Clear();
            this.BuildLevel();
            
        }

    }
}
