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
        Player testPlayer = new Player("testPlayer");


        int testCounter = 100;
        Texture2D testTexture;

        // Buttons
        public static Button button_RollDice;
        public static Button button_EndTurn;
        public static Button button_Auction;
        public static Button button_Buy;
        public static Button button_Trade;
        public static Button button_Mortgage;

        KeyboardState prevKeyboardState = Keyboard.GetState();

        Rectangle mainFrame;

        // Text Variables
        SpriteFont spriteFont;


        // The background which is also the board.
        public static Texture2D background;

        // An integer that determines which property card to show. 0 means no card is selected.
        Props drawId = Props.None;

        public Game1(string[] players)
        {
            graphics = new GraphicsDeviceManager( this );
            base.Content.RootDirectory = "Content";
            Game1.Content = base.Content;
            IsMouseVisible = true;

            // This is the resolution of the player's monitor
            windowWidth = Convert.ToInt16(HtmlPage.Window.Eval("screen.availWidth").ToString());
            windowHeight = Convert.ToInt16(HtmlPage.Window.Eval("screen.availHeight").ToString());

            // Shrinking the window to be a ratio of the monitor resolution
            /*
            preferredWindowWidth = (int)(windowWidth / 1.5);
            preferredWindowHeight = (int)(windowHeight / 1.5);
            graphics.PreferredBackBufferHeight = preferredWindowHeight;
            graphics.PreferredBackBufferWidth = preferredWindowWidth;
            */

            // Setting the size to be static
            preferredWindowWidth = 1000;
            preferredWindowHeight = 600;
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
                // Load the background which is also the board.
                background = base.Content.Load<Texture2D>("assets/Board2000x2000");
                backgroundHeight = background.Height;
                backgroundWidth = background.Width;

                SoshiLandUIFunctions.InitializeUIFunctions();
                SoshiLandUIFunctions.CalculateBoardBoxCenterPositions();

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

            // Calculate size of rectangle based on browser height and width
            mainFrame = new Rectangle(
                (preferredWindowWidth / 2) - (preferredWindowHeight / 2), 0, 
                (int)(preferredWindowHeight*1.0f), (int)(preferredWindowHeight*1.0f));
            
            // Temporary to test board pieces
            SoshilandGame.ListOfPlayers[0].PlayerChoosesBoardPiece(BoardPiece.ITNW_Tiffany);
            SoshilandGame.ListOfPlayers[1].PlayerChoosesBoardPiece(BoardPiece.ITNW_Taeyeon);
            SoshilandGame.ListOfPlayers[2].PlayerChoosesBoardPiece(BoardPiece.ITNW_Jessica);
            SoshilandGame.ListOfPlayers[3].PlayerChoosesBoardPiece(BoardPiece.ITNW_Seohyun);
            SoshilandGame.ListOfPlayers[4].PlayerChoosesBoardPiece(BoardPiece.ITNW_Sunny);
            SoshilandGame.ListOfPlayers[5].PlayerChoosesBoardPiece(BoardPiece.ITNW_Yuri);
            SoshilandGame.ListOfPlayers[6].PlayerChoosesBoardPiece(BoardPiece.ITNW_Sooyoung);
            SoshilandGame.ListOfPlayers[0].SetBoardPieceRectangleLocation((int)SoshiLandUIFunctions.centerBoardPositions[0].X, (int)SoshiLandUIFunctions.centerBoardPositions[0].Y - 15);
            SoshilandGame.ListOfPlayers[1].SetBoardPieceRectangleLocation((int)SoshiLandUIFunctions.centerBoardPositions[0].X, (int)SoshiLandUIFunctions.centerBoardPositions[0].Y - 10);
            SoshilandGame.ListOfPlayers[2].SetBoardPieceRectangleLocation((int)SoshiLandUIFunctions.centerBoardPositions[0].X, (int)SoshiLandUIFunctions.centerBoardPositions[0].Y - 5);
            SoshilandGame.ListOfPlayers[3].SetBoardPieceRectangleLocation((int)SoshiLandUIFunctions.centerBoardPositions[0].X, (int)SoshiLandUIFunctions.centerBoardPositions[0].Y);
            SoshilandGame.ListOfPlayers[4].SetBoardPieceRectangleLocation((int)SoshiLandUIFunctions.centerBoardPositions[0].X, (int)SoshiLandUIFunctions.centerBoardPositions[0].Y + 5);
            SoshilandGame.ListOfPlayers[5].SetBoardPieceRectangleLocation((int)SoshiLandUIFunctions.centerBoardPositions[0].X, (int)SoshiLandUIFunctions.centerBoardPositions[0].Y + 10);
            SoshilandGame.ListOfPlayers[6].SetBoardPieceRectangleLocation((int)SoshiLandUIFunctions.centerBoardPositions[0].X, (int)SoshiLandUIFunctions.centerBoardPositions[0].Y + 15);

            

            // Roll Dice Button
            Texture2D rollPressed = Content.Load<Texture2D>("Buttons/Button_RollDicePressed");
            Texture2D rollUnPressed = Content.Load<Texture2D>("Buttons/Button_RollDiceUnPressed");

            // This is assuming that all the buttons are same width
            int leftAlignment = (SoshiLandUIFunctions.window_sideSizeWidth - (rollPressed.Width / 2)) / 2;
            int buttonWidth = rollPressed.Width / 2;
            int buttonHeight = rollPressed.Height / 2;


            Rectangle rollRectangle = new Rectangle(leftAlignment, 240, buttonWidth, buttonHeight); 
            button_RollDice = new Button("Roll", rollPressed, rollUnPressed, rollRectangle);

            Texture2D buyPressed = Content.Load<Texture2D>("Buttons/Button_BuyPressed");
            Texture2D buyUnPressed = Content.Load<Texture2D>("Buttons/Button_BuyUnPressed");
            Rectangle buyRectangle = new Rectangle(leftAlignment, 300, buttonWidth, buttonHeight);
            button_Buy = new Button("Buy", buyPressed, buyUnPressed, buyRectangle);

            Texture2D auctionPressed = Content.Load<Texture2D>("Buttons/Button_AuctionPressed");
            Texture2D auctionUnPressed = Content.Load<Texture2D>("Buttons/Button_AuctionUnPressed");
            Rectangle auctionRectangle = new Rectangle(leftAlignment, 360, buttonWidth, buttonHeight);
            button_Auction = new Button("Auction", auctionPressed, auctionUnPressed, auctionRectangle);

            Texture2D tradePressed = Content.Load<Texture2D>("Buttons/Button_TradePressed");
            Texture2D tradeUnPressed = Content.Load<Texture2D>("Buttons/Button_TradeUnPressed");
            Rectangle tradeRectangle = new Rectangle(leftAlignment, 420, buttonWidth, buttonHeight);
            button_Trade = new Button("Trade", tradePressed, tradeUnPressed, tradeRectangle);

            Texture2D mortgagePressed = Content.Load<Texture2D>("Buttons/Button_MortgagePressed");
            Texture2D mortgageUnPressed = Content.Load<Texture2D>("Buttons/Button_MortgageUnPressed");
            Rectangle mortgageRectangle = new Rectangle(leftAlignment, 480, buttonWidth, buttonHeight);
            button_Mortgage = new Button("Mortgage", mortgagePressed, mortgageUnPressed, mortgageRectangle);

            Texture2D endTurnPressed = Content.Load<Texture2D>("Buttons/Button_EndTurnPressed");
            Texture2D endTurnUnPressed = Content.Load<Texture2D>("Buttons/Button_EndTurnUnPressed");
            Rectangle endTurnRectangle = new Rectangle(leftAlignment, 540, buttonWidth, buttonHeight);
            button_EndTurn = new Button("EndTurn", endTurnPressed, endTurnUnPressed, endTurnRectangle);

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

            MouseState ms = Mouse.GetState();

            // For detecting the mouseover for the tiles zoom in
            drawId = SoshiLandUIFunctions.MouseCursorHoverForZoom(ms);

            // Buttons
            button_RollDice.ButtonClickUpdate(ms, gameTime);
            button_EndTurn.ButtonClickUpdate(ms, gameTime);
            button_Auction.ButtonClickUpdate(ms, gameTime);
            button_Buy.ButtonClickUpdate(ms, gameTime);
            button_Mortgage.ButtonClickUpdate(ms, gameTime);
            button_Trade.ButtonClickUpdate(ms, gameTime);

            if (soshiLandGame != null)
                soshiLandGame.PlayerInputUpdate();

            if (SoshiLandGameFunctions.animatingBoardPieceMovement && SoshiLandGameFunctions.doneMoveAnimation)
            {
                SoshiLandGameFunctions.doneMoveAnimation = false;

                if (SoshiLandGameFunctions.firstMovement)
                {
                    if (SoshilandGame.currentTurnsPlayers.PreviousBoardPosition == 48)
                        SoshilandGame.currentTurnsPlayers.PreviousBoardPosition = 0;
                    else
                        SoshilandGame.currentTurnsPlayers.PreviousBoardPosition++;

                    SoshiLandGameFunctions.currentPlayerAnimationMovementLocation = SoshilandGame.currentTurnsPlayers.PreviousBoardPosition;
                }
                else
                    SoshiLandGameFunctions.firstMovement = true;
            }

            if (!SoshiLandGameFunctions.doneMoveAnimation)
            {
                if (SoshilandGame.currentTurnsPlayers.PreviousBoardPosition == 48)
                    SoshilandGame.currentTurnsPlayers.PreviousBoardPosition = 0;

                if (SoshilandGame.currentTurnsPlayers.PreviousBoardPosition == 47)
                    SoshiLandGameFunctions.AnimateJumpNextBox(SoshilandGame.currentTurnsPlayers, gameTime, SoshiLandUIFunctions.centerBoardPositions[47], SoshiLandUIFunctions.centerBoardPositions[0]);
                else
                    SoshiLandGameFunctions.AnimateJumpNextBox(SoshilandGame.currentTurnsPlayers, gameTime, SoshiLandUIFunctions.centerBoardPositions[SoshilandGame.currentTurnsPlayers.PreviousBoardPosition], SoshiLandUIFunctions.centerBoardPositions[SoshilandGame.currentTurnsPlayers.PreviousBoardPosition + 1]);
            }

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
            
            // Draw Background
            spriteBatch.Draw( background, mainFrame, Color.White );

            // Draw Buttons
            button_RollDice.Draw(spriteBatch);
            button_EndTurn.Draw(spriteBatch);
            button_Auction.Draw(spriteBatch);
            button_Buy.Draw(spriteBatch);
            button_Trade.Draw(spriteBatch);
            button_Mortgage.Draw(spriteBatch);
            // Draw Board Pieces
            if (SoshilandGame.ListOfPlayers != null)
            {
                foreach (Player p in SoshilandGame.ListOfPlayers)
                {
                    spriteBatch.Draw(p.getBoardPiece, p.getBoardPieceRectangle, new Rectangle(0, 0, p.getBoardPiece.Width, p.getBoardPiece.Height), Color.White, 0, new Vector2(p.getBoardPiece.Width / 2, p.getBoardPiece.Height / 2), SpriteEffects.None, 0);
                }
            }
            
            // Draw a property card based on the current drawId
            #region Draw Zoom Box
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
                case Props.CommChest2: 
                case Props.CommChest1:
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
            #endregion

            SoshiLandUIFunctions.DrawPiecesOnZoomBox(SoshiLandUIFunctions.GetTileNumber(drawId), spriteBatch);

            if (DEBUG)
            {
                debugMessageQueue.PrintMessages(gameTime, spriteBatch);
                Game1.debugMessageQueue.PrintLeaderboard(SoshilandGame.ListOfPlayers, spriteBatch);
                // Post player standings on right side
            }

            
            spriteBatch.End();

            base.Draw( gameTime );
            
        }

    }
}
