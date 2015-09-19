using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;

namespace MapGen_test
{
    class Map
    {
        public int tilesPlaced
        {
            get { return tile.Count(); }
        }

        private List<Tiles> tile = new List<Tiles>();

        public List<Tiles> Tile
        {
            get { return tile; }
            set { tile = value; }
        }

        //private List<DefaultTile> defaultTile = new List<DefaultTile>();

        //public List<DefaultTile> DefaultTile
        //{
        //    get { return defaultTile; }
        //    set { defaultTile = value; }
        //}



        private int width, height;
        public int MapHeight
        {
            get { return height; }

        }
        public int MapWidth
        {
            get { return width; }
        }

        public Map()
        {

        }
        public static List<Texture2D> Grasstiles = new List<Texture2D>();

        private Texture2D highlightRect;

        List<TilePicker.TilePickClass> tileSet = new List<TilePicker.TilePickClass>();

        public void LoadTileTextures(GraphicsDevice graphic,int sizeX,int sizeY)
        {
            tileSet = TilePicker.TileSet;

            highlightRect = CreateRectangle(sizeX, sizeY, Color.ForestGreen ,graphic );
        }
 
        public bool isFirstGenerate=true;
        public void Generate(int[,] map, int sizex, int sizey,bool clearLists)
        {
            if(clearLists)
            {
                isFirstGenerate = true;
                tile.Clear();
            }


            for (int x = 0; x < map.GetLength(1); x++)
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int mapPoint = map[y, x];


                    if((x==xTileChange &&y==yTileChange)||isFirstGenerate)
                    {
                        Rectangle positionRect = new Rectangle(x * sizex, y * sizey, sizex, sizey);

                        if (mapPoint >=0)
                            Tile.Add(new Tiles (positionRect,tileSet[mapPoint].tileName,tileSet[mapPoint].texture));

                    }
                    width = (x + 1) * sizex;
                    height = (y + 1) * sizey;
                }


            isFirstGenerate = false;

        }
        public int yTileChange, xTileChange;
        public void CheckMap(int x, int y, ref int[,] map, int sizex, int sizey, int whichTile)
        {
            Rectangle mouseRect = new Rectangle(x, y, 1, 1);
            //check for mouse intersecting with the tiles when mouse is clicked


            for (int i = 0; i < Tile.Count(); i++)
                if (isTouching(mouseRect, Tile[i].Rectangle))
                {
                    if (tileSet[whichTile].tileName == "CollTile")
                    {
                        if (tileSet[whichTile].tileName != "Default_Tile")
                        {
                                Tile[i].IsPassable = false;
                        }
                    }
                    else
                    {
                        map[Tile[i].Rectangle.Y / sizey, Tile[i].Rectangle.X / sizex] = whichTile;
                        yTileChange = Tile[i].Rectangle.Y / sizey;
                        xTileChange = Tile[i].Rectangle.X / sizex;

                        Tile.RemoveAt(i);
                        Generate(map, sizex, sizey, false);
                    }

                }


          


        }

        public Vector2 highlightTile(int x,int y,int[,] map, int sizex, int sizey,SpriteBatch spriteBatch)
        {
            Rectangle mouseRect = new Rectangle(x, y, 1, 1);

          
            for (int i = 0; i < Tile.Count(); i++)
                if (isTouching(mouseRect, Tile[i].Rectangle))
                {
                    return new Vector2(Tile[i].Rectangle.X, Tile[i].Rectangle.Y);
                }


            return Vector2.Zero;
        }

        public void AddCollider(int x, int y)
        {
            Rectangle mouseRect = new Rectangle(x, y, 1, 1);

            for (int i = 0; i < Tile.Count(); i++)
                if (isTouching(mouseRect, Tile[i].Rectangle))
                {
                    Tile[i].IsPassable = false;
                }

        }

        public bool isTouching(Rectangle r1, Rectangle r2)
        {
            if (r1.X >= r2.Left)
                if (r1.X <= r2.Right)
                    if (r1.Y <= r2.Bottom)
                        if (r1.Y >= r2.Top)
                            return true;

            return false;
        }
        static private Texture2D CreateRectangle(int width, int height, Color colori, GraphicsDevice graphic)
        {
            Texture2D rectangleTexture = new Texture2D(graphic, width, height);// create the rectangle texture, ,but it will have no color! lets fix that
            Color[] color = new Color[width * height];//set the color to the amount of pixels in the textures
            for (int i = 0; i < color.Length; i++)//loop through all the colors setting them to whatever values we want
            {
                color[i] = colori;
            }
            rectangleTexture.SetData(color);//set the color data on the texture
            return rectangleTexture;//return the texture
        }

        public void Draw(SpriteBatch spriteBatch)
        {
          
            foreach (Tiles tileDraw in Tile)
                tileDraw.Draw(spriteBatch);


        }
        public void Draw (SpriteBatch spriteBatch,Vector2 drawPos)
        {
            spriteBatch.Draw(highlightRect,drawPos, Color.Red*.5f);
        }



    }
}
