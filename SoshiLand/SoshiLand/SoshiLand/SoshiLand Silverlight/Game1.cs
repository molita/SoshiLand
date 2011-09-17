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

            // Load property cards.
            propLaScala = Content.Load<Texture2D>( "assets\\prop_la_scala" );
            propBali = Content.Load<Texture2D>( "assets\\prop_bali" );
            propTempMount = Content.Load<Texture2D>( "assets\\prop_temp_mount" );
            propDamnoenMart = Content.Load<Texture2D>( "assets\\prop_damnoen_mart" );
            propGreatWall = Content.Load<Texture2D>( "assets\\prop_great_wall" );
            propTajMahal = Content.Load<Texture2D>( "assets\\prop_taj_mahal" );
            propStatLiberty = Content.Load<Texture2D>( "assets\\prop_stat_liberty" );
            propEiffel = Content.Load<Texture2D>( "assets\\prop_eiffel" );
            propParthenon = Content.Load<Texture2D>( "assets\\prop_parthenon" );
            chance1 = Content.Load<Texture2D>( "assets\\chance1" );
            forever9 = Content.Load<Texture2D>( "assets\\forever9" );
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

            // Set drawId based on the mouse position when left-clicked. Commented out to develop new UI.
            /*if ( ms.Y <= 84 )
            {
                if ( ms.X >= 324 )
                {
                    if ( ms.X <= 375 )
                        drawId = Props.LaScala;
                    else if ( ms.X <= 425 )
                        drawId = Props.Bali;
                    else if ( ms.X <= 474 )
                        drawId = Props.Chance1;
                    else if ( ms.X <= 525 )
                        drawId = Props.TempleMount;
                    else if ( ms.X <= 575 )
                        drawId = Props.DamnoenMarket;
                    else if ( ms.X <= 626 )
                        drawId = Props.GreatWall;
                    else if ( ms.X <= 677 )
                        drawId = Props.TajMahal;
                    else if ( ms.X <= 727 )
                        drawId = Props.StatueLiberty;
                    else if ( ms.X <= 778 )
                        drawId = Props.Forever9;
                    else if ( ms.X <= 827 )
                        drawId = Props.EiffelTower;
                    else if ( ms.X <= 876 )
                        drawId = Props.Parthenon;
                    else drawId = Props.None;
                }
                else drawId = Props.None;
            }
            else drawId = Props.None;
            */
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
                case Props.LaScala:
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 1, spriteBatch);
                    break;
                case Props.Bali:
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 2, spriteBatch);
                    break;
                case Props.Chance1:
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 3, spriteBatch);
                    break;
                case Props.TempleMount:
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 4, spriteBatch);
                    break;
                case Props.DamnoenMarket:
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 5, spriteBatch);
                    break;
                case Props.GreatWall:
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 6, spriteBatch);
                    break;
                case Props.TajMahal:
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 7, spriteBatch);
                    break;
                case Props.StatueLiberty:
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 8, spriteBatch);
                    break;
                case Props.Forever9:
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 9, spriteBatch);
                    break;
                case Props.EiffelTower:
                    SoshiLandUIFunctions.DrawZoomInSideBoxes(drawId, 10, spriteBatch);
                    break;
                case Props.Parthenon:
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
