using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MapGen_test
{
    public class TilePicker
    {
        public class TilePickClass
        {
            public Texture2D texture;

            public Rectangle rectangle;

            public int tileValue;

            public string tileName;

            public string tilefileLocation;

            public TilePickClass(Texture2D newTexture, int newTileValue, string newtileName)
            {
                tileValue = newTileValue;
                texture = newTexture;
                tileName = newtileName;
                rectangle = new Rectangle(0, 0, 64, 64);
            }
        }

        public static List<TilePicker.TilePickClass> TileSet = new List<TilePicker.TilePickClass>();

        private Button_Menu overlay,highlighter;

        private int ScreenWidth;

        private int ScreenHeight;

        public static int tileCount = 0;

        public ContentManager content;

        private const float spacerVal = 2f;

        private const float iconSize = 0.7f;
        public void LoadAllTilesFromFile(GraphicsDevice graphics)
        {
            string fileLoc = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            fileLoc += "/MapGen_testContent/Tiles/";

            string[] filePaths = Directory.GetFiles(fileLoc);
            for(int i=0;i<filePaths.Count();i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePaths[i]);

                AddTileToMenuFromComputer(getTextureFromFile(filePaths[i], graphics),tileCount,fileName);
            }


            RectangleUpdate();

        }
        public void Load(ContentManager Content, int screenWidth, int screenHeight,GraphicsDevice graphics)
        {
            content = Content;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            AddTileToMenu("Tiles/Default_Tile", TilePicker.tileCount, Content, "Default_Tile");
            int num;
            for (int i = TilePicker.tileCount; i <= 3; i = num + 1)
            {
                AddTileToMenu("Tiles/GrassTile" + i, TilePicker.tileCount, Content, "GrassTile");
                num = i;
            }
            AddTileToMenu("Tiles/CollTile", TilePicker.tileCount, Content, "CollTile");
            AddTileToMenu("Tiles/File_Icon", -2, Content, "File_Icon");

            highlighter = new Button_Menu(CreateRectangle((int)(64*iconSize), (int)(64 * iconSize), Color.Red, graphics), (int)(64 * iconSize), (int)(64 * iconSize), new Vector2(-64,-64));
            overlay = new Button_Menu(Content.Load<Texture2D>("MenuOverlay/Menu"), screenWidth - 100, screenHeight - 100, new Vector2(50f, 50f));
            RectangleUpdate();
        }

        public void AddTileToMenu(string fileLocName, int tileNum, ContentManager Content, string fileName)
        {
            TileSet.Add(new TilePickClass(Content.Load<Texture2D>(fileLocName), tileNum, fileName));
            tileCount += 1;
        }

        public void AddTileToMenuFromComputer(Texture2D texture, int tileNum, string fileName)
        {
            TileSet.Add(new TilePickClass(texture, tileCount, fileName));
            tileCount += 1;
        }

        public void AddTile(GraphicsDevice graphics)//add tile from the folder manager
        {
            string fileLoc = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            fileLoc += "/MapGen_testContent/Tiles/";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                fileLoc += openFileDialog.SafeFileName;
                if (!File.Exists(fileLoc))
                {
                    File.Copy(fileName, fileLoc);
                }
                Texture2D texture;
                //using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
                //{
                //    texture = Texture2D.FromStream(graphics, fileStream);
                //}
                texture = getTextureFromFile(fileName, graphics);

                string safeFileName = openFileDialog.SafeFileName;

                this.AddTileToMenuFromComputer(texture, tileCount, safeFileName.Remove(safeFileName.Count<char>() - 4, 4));
                this.RectangleUpdate();
            }
        }
        public Texture2D getTextureFromFile(string fileName,GraphicsDevice graphics )
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                return Texture2D.FromStream(graphics, fileStream);
            }
        }
        public int TileSelector(Vector2 mousePos)
        {
            int result;
            foreach (TilePicker.TilePickClass current in TilePicker.TileSet)
            {
                bool flag = mousePos.X > (float)current.rectangle.Left;
                if (flag)
                {
                    bool flag2 = mousePos.X < (float)current.rectangle.Right;
                    if (flag2)
                    {
                        bool flag3 = mousePos.Y < (float)current.rectangle.Bottom;
                        if (flag3)
                        {
                            bool flag4 = mousePos.Y > (float)current.rectangle.Top;
                            if (flag4)
                            {
                                result = current.tileValue;
                                return result;
                            }
                        }
                    }
                }
            }
            result = -1;
            return result;
        }
        public void TileSelectorHighlighter(Vector2 mousePos)
        {
            foreach (TilePicker.TilePickClass current in TilePicker.TileSet)
            {
                bool flag = mousePos.X > (float)current.rectangle.Left;
                if (flag)
                {
                    bool flag2 = mousePos.X < (float)current.rectangle.Right;
                    if (flag2)
                    {
                        bool flag3 = mousePos.Y < (float)current.rectangle.Bottom;
                        if (flag3)
                        {
                            bool flag4 = mousePos.Y > (float)current.rectangle.Top;
                            if (flag4)
                            {
                                highlighter.SetPosition(new Vector2(current.rectangle.X, current.rectangle.Y));
                                break;
                            }
                        }
                    }
                }
                
                    //highlighter.SetPosition(Vector2.Zero);
            }

        }

        private void RectangleUpdate()
        {
            int num = 0;
            int num2 = 0;
            int num3;
            for (int i = 0; i < TilePicker.TileSet.Count<TilePicker.TilePickClass>(); i = num3 + 1)
            {
                bool flag = overlay.GetPosition.X + (float)(num + 1) * ((float)TileSet[i].texture.Width * 0.7f * 2f) > this.overlay.GetPosition.X + (float)this.overlay.sizeX;
                if (flag)
                {
                    //num2 += (int)((float)TilePicker.TileSet[i].texture.Height * 0.7f * 2f);
                    num2 += (int)((float)64 * 0.7f * 2f);
                    num = 0;
                }
                TilePicker.TileSet[i].rectangle = new Rectangle((int)(overlay.GetPosition.X * 2f + (float)num * 2f * ((float)TileSet[i].texture.Width * 0.7f)), (int)(this.overlay.GetPosition.Y * 2f) + num2, (int)((float)TilePicker.TileSet[i].texture.Width * 0.7f), (int)((float)TilePicker.TileSet[i].texture.Height * 0.7f));
                num3 = num;
                num = num3 + 1;
                num3 = i;
            }
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

            overlay.Draw(spriteBatch, .95f);

            spriteBatch.Begin();
            int num;
            for (int i = 0; i < TilePicker.TileSet.Count<TilePicker.TilePickClass>(); i = num + 1)
            {
                spriteBatch.Draw(TilePicker.TileSet[i].texture, TilePicker.TileSet[i].rectangle, Color.White);
                num = i;
            }
            spriteBatch.End();

            highlighter.Draw(spriteBatch, .4f);
        }
    }
}