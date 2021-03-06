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

using World_Of_Rectangle.Game;
using World_Of_Rectangle.Gamestates;

namespace World_Of_Rectangle
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        EGameStates gameState;
        IGamestateElement gameStateElement;
        IGamestateElement tmpGameStateElement;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //graphics.IsFullScreen = true;
            Global.SCREENSIZE = new Vector2(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();
            World.Initialize(Content);
            sat.Shape.Shape2D.Initialize(GraphicsDevice);
            sat.Etc.Helper.Initialize(GraphicsDevice);
            Input.Initialize();
            IsMouseVisible = true;
            Global.scaleValues = new Vector3(GraphicsDevice.DisplayMode.Width / Global.REFERENCE_SCREENSIZE.X,  GraphicsDevice.DisplayMode.Height / Global.REFERENCE_SCREENSIZE.Y, 1.0f);
            GameState = EGameStates.Menu;


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //gameStateElement.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            Input.Update();
            GameState = gameStateElement.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here



            base.Draw(gameTime);
            //spriteBatch.Begin();
            //spriteBatch.Draw(Content.Load<Texture2D>("button_name"),Input.mousePositionV(),Color.White);
            //spriteBatch.End();
            gameStateElement.Draw(gameTime, spriteBatch);
        }

        internal EGameStates GameState
        {
            get { return gameState; }
            set
            {
                // just change if the actual gamestate is being switched
                if (gameState != value)
                {
                    // changes value of gameState and changes type/object of gameStateElement
                    switch (value)
                    {
                        case EGameStates.Menu:
                            gameStateElement = new Menu();
                            tmpGameStateElement = null;
                            gameStateElement.LoadContent(Content);
                            break;
                        case EGameStates.Intro:
                            gameStateElement = new Intro();
                            tmpGameStateElement = null;
                            gameStateElement.LoadContent(Content);
                            break;
                        case EGameStates.Game:
                            // if gemstate was EGameStates.Inventory then load paused game, otherwise create a new one
                            if (gameState == EGameStates.Inventory)
                            {
                                gameStateElement = tmpGameStateElement;
                                tmpGameStateElement = null;
                            }
                            else
                            {
                                gameStateElement = new ActualGame();
                                gameStateElement.LoadContent(Content);
                            }
                            tmpGameStateElement = null;
                            break;
                        case EGameStates.Inventory:
                            if (gameState == EGameStates.Game)
                            {
                                tmpGameStateElement = gameStateElement;
                                gameStateElement = new Inventory(gameStateElement);
                                gameStateElement.LoadContent(Content);
                            }
                            break;
                        case EGameStates.Close:
                            Exit();
                            break;
                        case EGameStates.Credits:
                            gameStateElement = new Credits();
                            gameStateElement.LoadContent(Content);
                            break;
                    }
                    gameState = value;
                }
            }
        }
    }
}
