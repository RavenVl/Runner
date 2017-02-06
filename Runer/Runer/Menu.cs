using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Runer
{
    class Menu
    {
        
        public List<Item> Items { get; set; }
        SpriteFont font;
        int CurentItem;
        KeyboardState oldState;

        public Menu()
        {
            Items = new List<Item>();
        }

        public void Update()
        {

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Enter))
                Items[CurentItem].OnClick();

            int delta = 0;

            if (state.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                delta = -1;
            }
            if (state.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                delta = 1;
            }
            CurentItem += delta;
            bool ok = false;

            while (!ok)
            {
                if (CurentItem < 0)
                    CurentItem = Items.Count - 1;
                else if (CurentItem > Items.Count - 1)
                    CurentItem = 0;
                else if (Items[CurentItem].activ == false)
                    CurentItem += delta;
                else ok = true;
            }

            oldState = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();
           int y = 100;
           
            foreach (Item  item in Items)
	{

                Color color = Color.White;
                if (item.activ == false)
                {
                    color = Color.Gray;
                }
                if (item == Items[CurentItem])
                {
                    color = Color.Red;
                }
                spriteBatch.DrawString(font, item.text, new Vector2(100, y), color);
        
                y += 40;
	}
            
            spriteBatch.End();
        }

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("font_menu");
        }
    }
}
