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
using System.Xml;

namespace MapGen_test
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        GetFile getFile;
        Camera camera;
        TilePicker tilePicker;
        const int tileSize = 64;
        public int sizeX, sizeY;
        public int[,] test;



        SpriteFont gameFont;
        int _fpsCount,_totalFrames;
        float _elapsed_time;

        public static int screenHeight, screenWidth;


        //key entry array
        public Keys[] keys = new Keys[10] { Keys.D0,Keys.D1, Keys.D2, Keys.D3, Keys.D4,
                        Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };

        enum MapInputUpdateState
        {
            noUpdate,
            updateMap,
            isUpdating

        }
        MapInputUpdateState mapInputUpdatestate;
        enum MenuState
        {
            noMenu,
            tilePickerMenu,
            gameMenu

                
        }
        MenuState menuState;


        public void FillMapArray(int xTileWidth, int yTileWidth)
        {
            test = new int[yTileWidth, xTileWidth]; //y then x, down then up
            for (int x = 0; x < test.GetLength(1); x++)
                for (int y = 0; y < test.GetLength(0); y++)
                    test[y, x] = 0;

            sizeX = 64;//screenWidth / test.GetLength(1);
            sizeY = 64;//screenHeight / test.GetLength(0);

        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            
        }

        

        protected override void Initialize()
        {
            
            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;

            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();

            map = new Map();
            tilePicker = new TilePicker();
            getFile = new GetFile();
            IsMouseVisible = true;
            FillMapArray(30, 30);

            camera = new Camera(GraphicsDevice.Viewport);


            base.Initialize();
        }
       


        protected override void LoadContent()
        { 
            gameFont = Content.Load<SpriteFont>("gameFont");


            map.LoadTileTextures(GraphicsDevice,sizeX,sizeY);

            tilePicker.Load(Content, screenWidth, screenHeight,GraphicsDevice);
            tilePicker.LoadAllTilesFromFile(GraphicsDevice);
            Tiles.Content = Content;

            map.Generate(test, sizeX, sizeY,false);

            spriteBatch = new SpriteBatch(GraphicsDevice);


        }
        protected override void UnloadContent()
        {

        }



        public void MapArrayInputUpdate(ref string inputIndex, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Back) && keyboard != oldKeyboardState)
                if (inputIndex != null && inputIndex != "" && inputIndex.Length - 1 != -1)
                    inputIndex = inputIndex.Remove(inputIndex.Length - 1);

            for (int i = 0; i < keys.Length; i++)
            {
                if (keyboard.IsKeyDown(keys[i]) && keyboard != oldKeyboardState)
                    inputIndex += i;
            }

        }
        public string MapArrayPlayerInputSizeX = "", MapArrayPlayerInputSizeY = "";
        public bool isXInput = true;
        KeyboardState oldKeyboardState;
        public void KeyboardInput()
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.R) && oldKeyboardState != keyboard)
                getFile.OpenFile();

            if (keyboard.IsKeyDown(Keys.W))
                camera.Zoom += .01f;
            else if (keyboard.IsKeyDown(Keys.S))
                camera.Zoom -= .01f;

            if (mapInputUpdatestate == MapInputUpdateState.noUpdate)
                if (keyboard.IsKeyDown(Keys.Space))
                {
                    MapArrayPlayerInputSizeX = "";
                    MapArrayPlayerInputSizeY = "";
                    mapInputUpdatestate = MapInputUpdateState.isUpdating;
                }
            if (keyboard.IsKeyDown(Keys.Tab) && keyboard != oldKeyboardState && menuState != MenuState.tilePickerMenu)
                menuState = MenuState.tilePickerMenu;
            else if (keyboard.IsKeyDown(Keys.Tab) && keyboard != oldKeyboardState)
                menuState = MenuState.noMenu;


            if (mapInputUpdatestate == MapInputUpdateState.isUpdating)//validate no tiles over 200;
            {
                if (isXInput)
                    MapArrayInputUpdate(ref MapArrayPlayerInputSizeX, keyboard);
                else if (!isXInput)
                    MapArrayInputUpdate(ref MapArrayPlayerInputSizeY, keyboard);

                if (keyboard.IsKeyDown(Keys.Enter) && keyboard != oldKeyboardState)
                {


                    if (isXInput)
                    {
                        if (int.Parse(MapArrayPlayerInputSizeX) > 200)
                        {
                            isXInput = true;
                            MapArrayPlayerInputSizeX = "";
                        }
                        else
                        {
                                isXInput = false;
                            
                        }
                    }
                    else if(!isXInput)
                    {
                        if (int.Parse(MapArrayPlayerInputSizeY) > 200)
                        {
                            isXInput = true;
                            MapArrayPlayerInputSizeY = "";
                        }
                        else
                        {

                            mapInputUpdatestate = MapInputUpdateState.updateMap;
                            isXInput = true;

                        }


                    }







                }
            }





            oldKeyboardState = keyboard;


        }

        public int tileVal;
        public void zoomTest()
        {
            float hold = camera.Zoom;

            if (map.MapWidth < screenWidth / camera.Zoom || map.MapHeight < screenHeight / camera.Zoom)
                camera.Zoom +=.01f;
        }
        public int mousePosX, mousePosY;
        public Vector2 highLightTile;
        protected override void Update(GameTime gameTime)
        {
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
           if(_elapsed_time >1000)
            {
                _elapsed_time = 0;
                _fpsCount = _totalFrames;
                _totalFrames = 0;
            }


            MouseState mouse = Mouse.GetState();
            camera.Update((int)mouse.X, (int)mouse.Y, map.MapWidth, map.MapHeight);

            mousePosX = (int)Math.Round(((decimal)(mouse.X / camera.Zoom) + (camera.xZoomOffset / (decimal)camera.Zoom)));
            mousePosY = (int)Math.Round(((decimal)(mouse.Y / camera.Zoom) + (camera.yZoomOffSet / (decimal)camera.Zoom)));


           // zoomTest();
            KeyboardInput();

            if (menuState == MenuState.noMenu)
            {
                highLightTile = map.highlightTile(mousePosX, mousePosY, test, sizeX, sizeY, spriteBatch);

                if (mouse.LeftButton == ButtonState.Pressed)
                { 
                   // if(tileVal !=)
                    map.CheckMap(mousePosX, 
                        mousePosY, ref test, sizeX, sizeY, tileVal);//GrassTile: 1-3,StoneTile: 4
                    
                }
                else
                {

                }


                if (mapInputUpdatestate == MapInputUpdateState.updateMap)
                {
                    FillMapArray(int.Parse(MapArrayPlayerInputSizeX), int.Parse(MapArrayPlayerInputSizeY));
                    map.Generate(test, sizeX, sizeY,true);

                    mapInputUpdatestate = MapInputUpdateState.noUpdate;

                }
            }

            if (menuState == MenuState.tilePickerMenu)
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    int hold;
                    hold = tilePicker.TileSelector(new Vector2(mouse.X, mouse.Y));//screenWidth, screenHeight

                    if (hold >= 0)
                        tileVal = hold;
                    else if (hold == -2)
                        tilePicker.AddTile(GraphicsDevice);
                }
                else
                    tilePicker.TileSelectorHighlighter(new Vector2(mouse.X, mouse.Y));


            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            _totalFrames++;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            map.Draw(spriteBatch);
            if (highLightTile != null)
                map.Draw(spriteBatch,highLightTile);

            if (mapInputUpdatestate == MapInputUpdateState.isUpdating)
            {
                if (isXInput)
                {
                    spriteBatch.DrawString(gameFont, "Enter value for X length", Vector2.Zero, Color.Black);
                    if (MapArrayPlayerInputSizeX != null)
                        spriteBatch.DrawString(gameFont, MapArrayPlayerInputSizeX, new Vector2(0, 20), Color.Black);
                }
                else if (!isXInput)
                {
                    spriteBatch.DrawString(gameFont, "Enter value for Y length", Vector2.Zero, Color.Black);
                    if (MapArrayPlayerInputSizeY != null)
                        spriteBatch.DrawString(gameFont, MapArrayPlayerInputSizeY, new Vector2(0, 20), Color.Black);
                }

            }
            else
            {
                MouseState mouse = Mouse.GetState();

                spriteBatch.DrawString(gameFont, "Map Height " + test.GetLength(0) , new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(gameFont, "Map Width " + test.GetLength(1), new Vector2(0, 20), Color.Black);
                spriteBatch.DrawString(gameFont, map.tilesPlaced.ToString(), new Vector2(0, 40), Color.Black);
                spriteBatch.DrawString(gameFont, _fpsCount.ToString(), new Vector2(0, 60), Color.Black);
                spriteBatch.DrawString(gameFont, camera.Zoom.ToString(), new Vector2(0, 80), Color.Black);
                spriteBatch.DrawString(gameFont, mouse.X.ToString()+" " + mouse.Y.ToString(), new Vector2(mouse.X/camera.Zoom,mouse.Y / camera.Zoom), Color.Black);

                
            }



            spriteBatch.End();

            if (menuState == MenuState.tilePickerMenu)
                tilePicker.Draw(spriteBatch);

            base.Draw(gameTime);
        }

    }

}
