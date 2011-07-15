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
using ExEnSilver.Graphics;

// For Network
using System.Net;
using System.Xml;

using Newtonsoft.Json;



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

        // For debugging, since Silverlight doesn't seem to allow debugging within the IDE.
        public static bool DEBUG = true;
        public static string DEBUGMESSAGE = "Initial Debug Message";
        public static DebugMessageQueue debugMessageQueue;

        // Test
        SoshilandGame testGame;
        Player testPlayer = new Player("Test Player");

        KeyboardState prevKeyboardState = Keyboard.GetState();

        Rectangle mainFrame;

        // Text Variables
        SpriteFont spriteFont;


        // The background which is also the board.
        Texture2D background;

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

        // The position to display a magnified property card.
        Vector2 zoomPos;

        // An integer that determines which property card to show. 0 means no card is selected.
        Props drawId = Props.None;

        public Game1()
        {
            graphics = new GraphicsDeviceManager( this );
            base.Content.RootDirectory = "Content";
            Game1.Content = base.Content;
            IsMouseVisible = true;

            // Preferred window size is 640x640
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
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

            debugMessageQueue = new DebugMessageQueue();

            testGame = new SoshilandGame();

            // TEMPORARY creating user for JSON
            User testUser = new User();
            testUser.Name = "Mark";
            testUser.Money = 1500;
            testUser.BoardPosition = 0;


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
            background = base.Content.Load<Texture2D>("assets/main_screen_wide");
            mainFrame = new Rectangle( 0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height );
            zoomPos = new Vector2( (float) mainFrame.Width * (float) 0.84375, (float) mainFrame.Height * (float) 0.0875 );
            
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
            
            // Set drawId based on the mouse position when left-clicked. Commented out to develop new UI.
            if ( ms.Y <= 84 )
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

            testGame.PlayerInputUpdate();

            // Test for Grabbing Data (GET)
            if (kbInput.IsKeyDown(Keys.A) && prevKeyboardState.IsKeyUp(Keys.A))
            {
                string uriRequest = "http://daum.heroku.com/soshi";

                debugMessageQueue.addMessageToQueue("Attempting to send Request to " + uriRequest);
                HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(uriRequest));

                httpRequest.BeginGetResponse(new AsyncCallback(HttpResponseHandler), httpRequest);
            }

            // Test for Writing Data (POST)
            if (kbInput.IsKeyDown(Keys.S) && prevKeyboardState.IsKeyUp(Keys.S))
            {
                string uriRequest = "http://daum.heroku.com/soshi";

                debugMessageQueue.addMessageToQueue("Attempting to add a user to" + uriRequest);
                
                
                HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(uriRequest));

                httpRequest.Method = "POST";
                httpRequest.BeginGetRequestStream(new AsyncCallback(RequestReady), httpRequest);
            }

            prevKeyboardState = kbInput;
            
            base.Update( gameTime );
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class User
        {
            [JsonProperty]
            public string Name { get; set; }
            [JsonProperty]
            public int Money { get; set; }
            [JsonProperty]
            public int BoardPosition { get; set; }
        }

        void RequestReady(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            Stream stream = request.EndGetRequestStream(result);

            // Send the post variables  
            StreamWriter writer = new StreamWriter(stream);

            User testUser = new User();
            testUser.BoardPosition = 10;
            testUser.Money = 2000;
            testUser.Name = "John Smith";

            string testUserText = JsonConvert.SerializeObject(testUser);

            writer.WriteLine(testUserText);
            //writer.WriteLine("Name=John Smith");writer.WriteLine("Money=2000");writer.WriteLine("BoardPosition=10");

            debugMessageQueue.addMessageToQueue("Writing data: " + testUserText);

            writer.Flush();
            writer.Close();

            request.BeginGetResponse(new AsyncCallback(ResponseReady), request);
        }

        // Get the Result  
        void ResponseReady(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            // get the result text  
            string resultString = reader.ReadToEnd();

            debugMessageQueue.addMessageToQueue("Response: " + resultString);
            
        }  

        public void HttpResponseHandler(IAsyncResult result)
        {
            // acquire the result.
            HttpWebRequest httpRequest = (HttpWebRequest)result.AsyncState;

            // acquire the feed response.
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.EndGetResponse(result);

            // load the response into a stream reader
            var streamReader = new StreamReader(httpResponse.GetResponseStream());
            // Convert stream into string
            string text = streamReader.ReadToEnd();

            //User readData = JsonConvert.DeserializeObject<User>(text);

            debugMessageQueue.addMessageToQueue(text);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw( GameTime gameTime )
        {
            GraphicsDevice.Clear( Color.White );

            
            spriteBatch.Begin();

            
            spriteBatch.Draw( background, mainFrame, Color.White );

            
            // Draw a property card based on the current drawId
            switch ( drawId )
            {
                case Props.LaScala:
                    spriteBatch.Draw( propLaScala, zoomPos, Color.White );
                    break;
                case Props.Bali:
                    spriteBatch.Draw( propBali, zoomPos, Color.White );
                    break;
                case Props.Chance1:
                    spriteBatch.Draw( chance1, zoomPos, Color.White );
                    break;
                case Props.TempleMount:
                    spriteBatch.Draw( propTempMount, zoomPos, Color.White );
                    break;
                case Props.DamnoenMarket:
                    spriteBatch.Draw( propDamnoenMart, zoomPos, Color.White );
                    break;
                case Props.GreatWall:
                    spriteBatch.Draw( propGreatWall, zoomPos, Color.White );
                    break;
                case Props.TajMahal:
                    spriteBatch.Draw( propTajMahal, zoomPos, Color.White );
                    break;
                case Props.StatueLiberty:
                    spriteBatch.Draw( propStatLiberty, zoomPos, Color.White );
                    break;
                case Props.Forever9:
                    spriteBatch.Draw( forever9, zoomPos, Color.White );
                    break;
                case Props.EiffelTower:
                    spriteBatch.Draw( propEiffel, zoomPos, Color.White );
                    break;
                case Props.Parthenon:
                    spriteBatch.Draw( propParthenon, zoomPos, Color.White );
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
