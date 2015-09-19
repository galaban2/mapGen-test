using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapGen_test
{
    class Tiles
    {
        protected Texture2D texture;

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
        }

        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            protected set { rectangle = value; }
        }

        private bool isPassable = true;
        public bool IsPassable
        {
            get { return isPassable; }
            set { isPassable = value; }
        }
        public string tileName;
        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                if (IsPassable)
                    spriteBatch.Draw(texture, rectangle, Color.White);
                else if (!IsPassable)
                {
                    spriteBatch.Draw(texture, rectangle, new Color(150, 150, 150, 255));
                }
            }
        }
            public Tiles(Rectangle newRectangle,string newTileName,Texture2D newTexture )
        {
                //texture = Content.Load<Texture2D>(fileLocation);
            texture = newTexture;
                this.Rectangle = newRectangle;
                tileName = newTileName;

            }

        
    }
    //class DefaultTile : Tiles
    //{

    //    public DefaultTile(Rectangle newRectangle)
    //    {
    //        texture = Content.Load<Texture2D>("Tiles/Default_Tile");
    //        this.Rectangle = newRectangle;

    //    }

    //}


    //class GrassTiles : Tiles
    //{

    //    public GrassTiles(int i, Rectangle newRectangle)
    //    {
    //        texture = Content.Load<Texture2D>("Tiles/GrassTile"+i);
    //        this.Rectangle = newRectangle;

    //    }

    //}
}

