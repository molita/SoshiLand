using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Windows.Browser;
using ExEnSilver.Graphics;

// For Network
using System.Net;
using System.Xml;

using Newtonsoft.Json;
using SoshiLandSilverlight.GameData.JSON;


namespace SoshiLandSilverlight
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static ContentManager Content;

        // Browser window dimensions
        int windowHeight;
        int windowWidth;
        public static int preferredWindowWidth;
        public static int preferredWindowHeight;

        // Board Dimensions
        public static int backgroundHeight;
        public static int backgroundWidth;

        // For debugging, since Silverlight doesn't seem to allow debugging within the IDE.
        public static bool DEBUG = false;
        public static string DEBUGMESSAGE = "Initial Debug Message";
        public static DebugMessageQueue debugMessageQueue;

        // Test
        SoshilandGame soshiLandGame;
        string[] playerStringArray;

        KeyboardState prevKeyboardState = Keyboard.GetState();

        Rectangle mainFrame;

        // Text Variables
        SpriteFont spriteFont;


        // The background which is also the board.
        public static Texture2D background;

        // Sprites of property cards.
        Texture2D propLaScala;
        Texture2D propBali;
        Texture2D propTempMount;
        Texture2D propDamnoenMart;
        Texture2D propGreatWall;
        Texture2D propTajMahal;
        Texture2D propStatLiberty;
        Texture2D propEiffel;
        Texture2D propParthenon;
        Texture2D chance1;
        Texture2D forever9;

        // An integer that determines which property card to show. 0 means no card is selected.
        Props drawId = Props.None;

        public Game1(string[] players)
        {
            graphics = new GraphicsDeviceManager( this );
            base.Content.RootDirectory = "Content";
            Game1.Content = base.Content;
            IsMouseVisible = true;

            // Preferred window size is 640x640
            //graphics.PreferredBackBufferHeight = 640;
            //graphics.PreferredBackBufferWidth = 640;

            //graphics.PreferredBackBufferHeight = 480;
            //graphics.PreferredBackBufferWidth = 640;

            // This is the resolution of the player's monitor
            windowWidth = Convert.ToInt16(HtmlPage.Window.Eval("screen.availWidth").ToString());
            windowHeight = Convert.ToInt16(HtmlPage.Window.Eval("screen.availHeight").ToString());

            // Shrinking the window to be a ratio of the monitor resolution
            preferredWindowWidth = (int)(windowWidth / 1.5);
            preferredWindowHeight = (int)(windowHeight / 1.5);
            graphics.PreferredBackBufferHeight = preferredWindowHeight;
            graphics.PreferredBackBufferWidth = preferredWindowWidth;

            playerStringArray = players;
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
            if (playerStringArray != null)
            {
                debugMessageQueue = new DebugMessageQueue();
                soshiLandGame = new SoshilandGame(playerStringArray);
            }
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch( GraphicsDevice );
            
            // Load the background which is also the board.
            background = base.Content.Load<Texture2D>("assets/Board2000x2000");
            backgroundHeight = background.Height;
            backgroundWidth = background.Width;

            // Calculate size of rectangle based on browser height and width
            mainFrame = new Rectangle(
                (preferredWindowWidth / 2) - (preferredWindowHeight / 2), 0, 
                (int)(preferredWindowHeight*1.0f), (int)(preferredWindowHeight*1.0f));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update( GameTime gameTime )
        {
            // Allows the game to exit
            if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed )
                this.Exit();

            KeyboardState kbInput = Keyboard.GetState();

            // Full screen function. Currently disabled.
            //if ( kbInput.IsKeyDown( Keys.F11 ) && prevKeyboardState.IsKeyUp( Keys.F11 ) )
            //{
            //    graphics.ToggleFullScreen();
            //    mainFrame.Height = GraphicsDevice.Viewport.Height;
            //    mainFrame.Width = mainFrame.Height;
            //    mainFrame.X = (GraphicsDevice.Viewport.Width - mainFrame.Width) / 2;
            //}

            MouseState ms = Mouse.GetState();

            drawId = SoshiLandUIFunctions.MouseCursorHoverForZoom(ms);

            if (soshiLandGame != null)
                soshiLandGame.PlayerInputUpdate();

            prevKeyboardState = kbInput;
            
            base.Update( gameTime );
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw( GameTime gameTime )
        {
            GraphicsDevice.Clear( Color.Pink);
            
            spriteBatch.Begin();
            
            spriteBatch.Draw( background, mainFrame, Color.White );

            // Draw a property card based on the current drawId
            switch ( drawId )
            {
                    // Corner Items
                case Props.FanMeeting:  SoshiLandUIFunctions.DrawZoomInCornerBoxes(Props.FanMeeting, spriteBatch); break;
                case Props.GoBabysit:   SoshiLandUIFunctions.DrawZoomInCornerBoxes(Props.GoBabysit, spriteBatch); break;
                case Props.Babysit:     SoshiLandUIFunctions.DrawZoomInCornerBoxes(Props.Babysit, spriteBatch); break;
                case Props.Go:          SoshiLandUIFunctions.DrawZoomInCornerBoxes(Props.Go, spriteBatch); break;

                    // 1st Items
                case Props.LaScala:
                case Props.GizaPyramid:
                case Props.WhiteHouse:
                case Props.UNHQ: 
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 1, spriteBatch); 
                    break;

                    // 2nd Items
                case Props.Bali: 
                case Props.BigBen:
                case Props.GyeongBokGoong: 
                case Props.HongKong: 
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 2, spriteBatch); 
                    break;

                    // 3rd Items
                case Props.Chance1: 
                case Props.Pisa: 
                case Props.MountEverest:
                case Props.AmazonRainforest: 
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 3, spriteBatch); 
                    break;

                    // 4th Items
                case Props.TempleMount: 
                case Props.CommChest: 
                case Props.Chance2: 
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 4, spriteBatch); 
                    break;

                    // 5th Items
                case Props.GrandCanal: 
                case Props.DamnoenMarket: 
                case Props.BarrierReef: 
                case Props.Disneyland: 
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 5, spriteBatch); 
                    break;

                    // 6th Items
                case Props.GreatWall: 
                case Props.WencelsasSquare: 
                case Props.VenetianResort: 
                case Props.AngkorWat: 
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 6, spriteBatch); 
                    break;

                    // 7th Items
                case Props.TajMahal: 
                case Props.BarcelonaAirport:
                case Props.ChateauDeChillon: 
                case Props.LuxuryTax: 
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 7, spriteBatch); 
                    break;

                    // 8th Items
                case Props.StatueLiberty: 
                case Props.MalibuBeach: 
                case Props.TokyoDome: 
                case Props.WalkOfFame: 
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 8, spriteBatch);
                    break;

                    // 9th Items
                case Props.Forever9: 
                case Props.SoshiBond: 
                case Props.ShoppingSpree: 
                case Props.KuwaitMuseum:
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 9, spriteBatch); 
                    break;

                    // 10th Items
                case Props.EiffelTower: 
                case Props.GoldenGateBridge:
                case Props.Colosseum: 
                case Props.CNTower: 
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 10, spriteBatch); 
                    break;

                    // 11th Items
                case Props.Parthenon:       
                case Props.SydneyOpera:     
                case Props.BlueHouse:       
                case Props.BukitTimah:      
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 11, spriteBatch); 
                    break;
                default:
                    break;
            }

            if (DEBUG)
            {
                debugMessageQueue.PrintMessages(gameTime, spriteBatch);
                Game1.debugMessageQueue.PrintLeaderboard(SoshilandGame.ListOfPlayers, spriteBatch);
                // Post player standings on right side
            }


            spriteBatch.End();

            base.Draw( gameTime );
            
        }

        // Return a Vector2 indicating the position to draw a property card (or a sprite in general).
        private Vector2 makeTexturePos( Texture2D tex )
        {
            Vector2 v = new Vector2( mainFrame.Width / 2 - tex.Width / 2, mainFrame.Height / 2 - tex.Height / 2 );
            return v;
        }
    }
}
