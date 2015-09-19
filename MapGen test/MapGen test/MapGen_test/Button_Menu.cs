

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapGen_test
{
    class Button_Menu
    {
        Texture2D texture;
        private  Vector2 position;
        public Vector2 GetPosition
        {
            get { return position; }
        }
        Rectangle rectangle;

        public int sizeX;

        public Button_Menu(Texture2D newTexture,int xLength,int yLength,Vector2 startVec)
        {
            texture = newTexture;
            rectangle = new Rectangle((int)startVec.X,(int)startVec.Y, xLength, yLength);
            position = startVec;
            sizeX = xLength;

        }     
        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch,float transparency)
        {
            //draw menu items above other begin functions 
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            spriteBatch.Draw(texture, position,rectangle, Color.White * transparency);


            spriteBatch.End();
        }


    }
}
