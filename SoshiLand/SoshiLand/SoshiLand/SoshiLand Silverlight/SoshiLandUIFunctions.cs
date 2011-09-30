using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SoshiLandSilverlight
{
    public static class SoshiLandUIFunctions
    {
        // For storing the center (Vector 2) of each box on the board.
        public static Vector2[] centerBoardPositions;
        // For storing the preset locations on the zoom box for the board pieces
        public static Vector2[] zoomBoxPiecesPositions;
        // To store the number of pieces on a certain location
        public static int[] numberOfPiecesInPosition;

        // Ratios based on the 7050x7050 board
        private static float ratioBlackBorder = 20f / 7050f;
        private static float ratioCornerBoxes = 800f / 7050f;
        private static float ratioSideBoxes = 470f / 7050f;
        private static float ratioSideHeight = 840f / 7050f;

        // Numbers for UI
        private static float zoomRatio = Game1.preferredWindowWidth * 0.0003125f;

        private static int zoomWidth = (int)(470 * zoomRatio);
        private static int zoomHeight = (int)(800 * zoomRatio);

        // Relative to the window size
        public static int window_leftSideOfBoard = ((Game1.preferredWindowWidth - (Game1.preferredWindowHeight)) / 2);
        public static int window_rightSideOfBoard = window_leftSideOfBoard + Game1.preferredWindowHeight;
        public static int window_oneSideBox = (int)(Game1.preferredWindowHeight * (ratioBlackBorder + ratioSideBoxes));
        public static int window_oneSideBoxIncludingBorder = (int)(window_oneSideBox + (ratioBlackBorder * 2));
        public static int window_cornerBox = (int)(Game1.preferredWindowHeight * (ratioCornerBoxes));
        public static int window_cornerBoxIncludingBorder = (int)(window_cornerBox + (ratioBlackBorder * 2));
        public static int window_blackBorder = (int)(Game1.preferredWindowHeight * ratioBlackBorder);

        public static int window_sideSizeWidth = window_leftSideOfBoard;    // NOTE: This is assuming both sides are equal sizes

        // Relative to the original size
        private static int orig_startOfSideRow = (int)(Game1.backgroundHeight * (ratioBlackBorder + ratioCornerBoxes));
        private static int orig_oneSideBox = (int)(Game1.backgroundHeight * (ratioBlackBorder + ratioSideBoxes));
        private static int orig_oneSideBoxIncludingBorder = (int)(Game1.backgroundHeight * ((ratioBlackBorder * 2) + ratioSideBoxes));
        private static int orig_cornerBoxIncludingBorder = (int)(Game1.backgroundHeight * ((ratioBlackBorder * 2) + ratioCornerBoxes));

        public static void InitializeUIFunctions()
        {
            zoomRatio = Game1.preferredWindowWidth * 0.0003125f;

            zoomWidth = (int)(470 * zoomRatio);
            zoomHeight = (int)(800 * zoomRatio);

            // Initialize the numberOfPiecesInPosition Variable
            numberOfPiecesInPosition = new int[48];
            for (int i = 0; i <numberOfPiecesInPosition.Length; i++)
                numberOfPiecesInPosition[0] = 0;

            /*
            // Relative to the window size
            window_leftSideOfBoard = ((Game1.preferredWindowWidth - (Game1.preferredWindowHeight)) / 2);
            window_rightSideOfBoard = window_leftSideOfBoard + Game1.preferredWindowHeight;
            window_oneSideBox = (int)(Game1.preferredWindowHeight * (ratioBlackBorder + ratioSideBoxes));
            window_oneSideBoxIncludingBorder = (int)(window_oneSideBox + (ratioBlackBorder * 2));
            window_cornerBox = (int)(Game1.preferredWindowHeight * (ratioCornerBoxes));
            window_cornerBoxIncludingBorder = (int)(window_cornerBox + (ratioBlackBorder * 2));
            window_blackBorder = (int)(Game1.preferredWindowHeight * ratioBlackBorder);
            */

            
            // Relative to the original size
            orig_startOfSideRow = (int)(Game1.backgroundHeight * (ratioBlackBorder + ratioCornerBoxes));
            orig_oneSideBox = (int)(Game1.backgroundHeight * (ratioBlackBorder + ratioSideBoxes));
            orig_oneSideBoxIncludingBorder = (int)(Game1.backgroundHeight * ((ratioBlackBorder * 2) + ratioSideBoxes));
            orig_cornerBoxIncludingBorder = (int)(Game1.backgroundHeight * ((ratioBlackBorder * 2) + ratioCornerBoxes));

            
            // These values are for the static 1000x600 size
            window_leftSideOfBoard = 200;
            window_rightSideOfBoard = 800;
            
            window_oneSideBox = 41;
            window_oneSideBoxIncludingBorder = 42;
            window_cornerBox = 69;
            window_cornerBoxIncludingBorder = 70;
            window_blackBorder = 1;

            window_sideSizeWidth = window_leftSideOfBoard;

            // Hardcoding these since the board is static now
            zoomBoxPiecesPositions = new Vector2[9];
            int middleX = ((Game1.preferredWindowWidth - window_rightSideOfBoard) / 2) + window_rightSideOfBoard;
            int bottomYRow = zoomHeight - 50;
            int topYRow = zoomHeight - 100;
            /*
            zoomBoxPiecesPositions[0] = new Vector2(middleX + 50, bottomYRow);
            zoomBoxPiecesPositions[1] = new Vector2(middleX + 20, bottomYRow);
            zoomBoxPiecesPositions[2] = new Vector2(middleX - 20, bottomYRow);
            zoomBoxPiecesPositions[3] = new Vector2(middleX - 50, bottomYRow);
            zoomBoxPiecesPositions[4] = new Vector2(middleX + 60, topYRow);
            zoomBoxPiecesPositions[5] = new Vector2(middleX + 30, topYRow);
            zoomBoxPiecesPositions[6] = new Vector2(middleX + 0, topYRow);
            zoomBoxPiecesPositions[7] = new Vector2(middleX - 30, topYRow);
            zoomBoxPiecesPositions[8] = new Vector2(middleX - 60, topYRow);
            */
            // Going to instead stack the pieces on the right side. 
            // The above code was attempting to put the pieces on the tile itself, but it may cover some text
            int XrightSide = Game1.preferredWindowWidth - 10;
            for (int a = 0; a < zoomBoxPiecesPositions.Length; a++)
                zoomBoxPiecesPositions[a] = new Vector2(XrightSide, (50 * a) + 20);

        }

        public static Props MouseCursorHoverForZoom(MouseState mouseState)
        {
            // Check if the mouse is within bounds of the top row
            if (mouseState.Y <= (ratioSideHeight * Game1.preferredWindowHeight) && mouseState.Y >= 0)
            {
                if (mouseState.X <= window_leftSideOfBoard)
                    return Props.None;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder)
                    return Props.FanMeeting;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 1))
                    return Props.LaScala;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 2))
                    return Props.Bali;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 3))
                    return Props.Chance1;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 4))
                    return Props.TempleMount;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 5))
                    return Props.DamnoenMarket;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 6))
                    return Props.GreatWall;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 7))
                    return Props.TajMahal;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 8) - 1)
                    return Props.StatueLiberty;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 9) - 1)
                    return Props.Forever9;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 10) - 1)
                    return Props.EiffelTower;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 11) - 1)
                    return Props.Parthenon;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 11) + window_cornerBoxIncludingBorder - 1)
                    return Props.GoBabysit;
            }
            
            // Check if the mouse is within bounds of the left column
            if (mouseState.X <= (ratioSideHeight * Game1.preferredWindowHeight) + window_leftSideOfBoard && mouseState.X >= window_leftSideOfBoard)
            {
                if (mouseState.Y <= 0)
                    return Props.None;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 1)
                    return Props.GizaPyramid;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 2)
                    return Props.BigBen;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 3)
                    return Props.Pisa;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 4)
                    return Props.CommChest1;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 5)
                    return Props.BarrierReef;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 6)
                    return Props.WencelsasSquare;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 7)
                    return Props.BarcelonaAirport;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 8)
                    return Props.MalibuBeach;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 9)
                    return Props.SoshiBond;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 10)
                    return Props.GoldenGateBridge;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 11)
                    return Props.SydneyOpera;

            }


            // Check if the mouse is within bounds of the right column
            if (mouseState.X >= window_rightSideOfBoard - (ratioSideHeight * Game1.preferredWindowHeight) && mouseState.X <= window_rightSideOfBoard)
            {
                if (mouseState.Y <= 0)
                    return Props.None;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 1)
                    return Props.WhiteHouse;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 2)
                    return Props.GyeongBokGoong;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 3)
                    return Props.MountEverest;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 4)
                    return Props.CommChest2;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 5)
                    return Props.GrandCanal;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 6)
                    return Props.VenetianResort;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 7)
                    return Props.ChateauDeChillon;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 8)
                    return Props.TokyoDome;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 9)
                    return Props.ShoppingSpree;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 10)
                    return Props.Colosseum;
                else if (mouseState.Y <= window_cornerBoxIncludingBorder + window_oneSideBoxIncludingBorder * 11)
                    return Props.BlueHouse;
            }

            // Check if the mouse is within bounds of the bottom row
            if (mouseState.Y >= Game1.preferredWindowHeight - (ratioSideHeight * Game1.preferredWindowHeight) && mouseState.Y <= Game1.preferredWindowHeight)
            {
                if (mouseState.X <= window_leftSideOfBoard)
                    return Props.None;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder)
                    return Props.Babysit;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 1))
                    return Props.UNHQ;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 2))
                    return Props.HongKong;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 3))
                    return Props.AmazonRainforest;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 4))
                    return Props.Chance2;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 5))
                    return Props.Disneyland;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 6))
                    return Props.AngkorWat;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 7))
                    return Props.LuxuryTax;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 8))
                    return Props.WalkOfFame;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 9))
                    return Props.KuwaitMuseum;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 10))
                    return Props.CNTower;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 11))
                    return Props.BukitTimah;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 11) + window_cornerBoxIncludingBorder)
                    return Props.Go;
            }

            return Props.None;

        }

        public static void DrawZoomInSideBoxes(Props prop, int positionFromLeftToRightOrTopToBottomExcludingCorner, SpriteBatch spriteBatch)
        {
            
            switch (prop)
            {
                // Top row items
                case Props.LaScala:
                case Props.Bali:
                case Props.Chance1:
                case Props.TempleMount:
                case Props.DamnoenMarket:
                case Props.GreatWall:
                case Props.TajMahal:
                case Props.StatueLiberty:
                case Props.Forever9:
                case Props.EiffelTower:
                case Props.Parthenon:
                    spriteBatch.Draw(Game1.background,
                        new Rectangle(window_rightSideOfBoard + (zoomWidth) + ((((Game1.preferredWindowWidth - Game1.preferredWindowHeight) / 2) - zoomWidth) / 2), zoomHeight, zoomWidth, zoomHeight),
                        new Rectangle(orig_startOfSideRow + (positionFromLeftToRightOrTopToBottomExcludingCorner - 1) * orig_oneSideBox, 0, (int)(Game1.backgroundWidth * (ratioSideBoxes + ratioBlackBorder * 2)), (int)(Game1.backgroundHeight * ratioSideHeight)),
                        Color.White, MathHelper.ToRadians(180), Vector2.Zero, SpriteEffects.None, 0f);
                    break;

                    // Bottom Row items
                case Props.UNHQ:
                case Props.HongKong:
                case Props.AmazonRainforest:
                case Props.Chance2:
                case Props.Disneyland:
                case Props.AngkorWat:
                case Props.LuxuryTax:
                case Props.WalkOfFame:
                case Props.KuwaitMuseum:
                case Props.CNTower:
                case Props.BukitTimah:
                    spriteBatch.Draw(Game1.background,
                        new Rectangle(window_rightSideOfBoard + ((((Game1.preferredWindowWidth - Game1.preferredWindowHeight) / 2) - zoomWidth) / 2), 0, zoomWidth, zoomHeight),
                        new Rectangle(orig_startOfSideRow + (positionFromLeftToRightOrTopToBottomExcludingCorner - 1) * orig_oneSideBox, Game1.backgroundHeight - (orig_cornerBoxIncludingBorder), (int)(Game1.backgroundWidth * (ratioSideBoxes + ratioBlackBorder * 2)), (int)(Game1.backgroundHeight * ratioSideHeight)),
                        Color.White, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0f);
                    break;
                    // Left Column Items
                case Props.GizaPyramid:
                case Props.BigBen:
                case Props.Pisa:
                case Props.CommChest1:
                case Props.BarrierReef:
                case Props.WencelsasSquare:
                case Props.BarcelonaAirport:
                case Props.MalibuBeach:
                case Props.SoshiBond:
                case Props.GoldenGateBridge:
                case Props.SydneyOpera:
                    spriteBatch.Draw(Game1.background,
                        new Rectangle(window_rightSideOfBoard + ((((Game1.preferredWindowWidth - Game1.preferredWindowHeight) / 2) - zoomWidth) / 2), zoomHeight, zoomHeight, zoomWidth),
                        new Rectangle(0, orig_startOfSideRow + (positionFromLeftToRightOrTopToBottomExcludingCorner - 1) * orig_oneSideBox, (int)(Game1.backgroundHeight * ratioSideHeight), (int)(Game1.backgroundWidth * (ratioSideBoxes + ratioBlackBorder * 2))),
                        Color.White, MathHelper.ToRadians(270), Vector2.Zero, SpriteEffects.None, 0f);
                    break;

                    // Right Column Items
                case Props.WhiteHouse:
                case Props.GyeongBokGoong:
                case Props.MountEverest:
                case Props.CommChest2:
                case Props.GrandCanal:
                case Props.VenetianResort:
                case Props.ChateauDeChillon:
                case Props.TokyoDome:
                case Props.ShoppingSpree:
                case Props.Colosseum:
                case Props.BlueHouse:
                    spriteBatch.Draw(Game1.background,
                        new Rectangle(window_rightSideOfBoard + zoomWidth + ((((Game1.preferredWindowWidth - Game1.preferredWindowHeight) / 2) - zoomWidth) / 2), 0, zoomHeight, zoomWidth),
                        new Rectangle(Game1.backgroundWidth - orig_cornerBoxIncludingBorder, orig_startOfSideRow + (positionFromLeftToRightOrTopToBottomExcludingCorner - 1) * orig_oneSideBox, (int)(Game1.backgroundHeight * ratioSideHeight), (int)(Game1.backgroundWidth * (ratioSideBoxes + ratioBlackBorder * 2))),
                        Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0f);
                    break;
                
            }
        }

        public static void DrawZoomInCornerBoxes(Props prop, SpriteBatch spriteBatch)
        {
            switch (prop)
            {
                case Props.FanMeeting:
                    spriteBatch.Draw(Game1.background,
                        new Rectangle(window_rightSideOfBoard + (zoomWidth) + ((((Game1.preferredWindowWidth - Game1.preferredWindowHeight) / 2) - zoomWidth) / 2), zoomWidth + ((zoomHeight - zoomWidth) / 2), zoomWidth, zoomWidth),
                        new Rectangle(0, 0, (int)(Game1.backgroundWidth * (ratioCornerBoxes + ratioBlackBorder * 2)), (int)(Game1.backgroundHeight * ratioSideHeight)),
                        Color.White, MathHelper.ToRadians(180), Vector2.Zero, SpriteEffects.None, 0f);
                    break;
                case Props.GoBabysit:
                    spriteBatch.Draw(Game1.background,
                        new Rectangle(window_rightSideOfBoard + (zoomWidth) + ((((Game1.preferredWindowWidth - Game1.preferredWindowHeight) / 2) - zoomWidth) / 2), zoomWidth + ((zoomHeight - zoomWidth) / 2), zoomWidth, zoomWidth),
                        new Rectangle(Game1.backgroundWidth - (int)(Game1.backgroundWidth * (ratioCornerBoxes + ratioBlackBorder * 2)), 0, (int)(Game1.backgroundWidth * (ratioCornerBoxes + ratioBlackBorder * 2)), (int)(Game1.backgroundHeight * ratioSideHeight)),
                        Color.White, MathHelper.ToRadians(180), Vector2.Zero, SpriteEffects.None, 0f);
                    break;
                case Props.Go:
                    spriteBatch.Draw(Game1.background,
                        new Rectangle(window_rightSideOfBoard + ((((Game1.preferredWindowWidth - Game1.preferredWindowHeight) / 2) - zoomWidth) / 2), ((zoomHeight - zoomWidth) / 2), zoomWidth, zoomWidth),
                        new Rectangle(Game1.backgroundWidth - (int)(Game1.backgroundWidth * (ratioCornerBoxes + ratioBlackBorder * 2)), Game1.backgroundWidth - (int)(Game1.backgroundWidth * (ratioCornerBoxes + ratioBlackBorder * 2)), (int)(Game1.backgroundWidth * (ratioCornerBoxes + ratioBlackBorder * 2)), (int)(Game1.backgroundHeight * ratioSideHeight)),
                        Color.White, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0f);
                    break;
                case Props.Babysit:
                    spriteBatch.Draw(Game1.background,
                        new Rectangle(window_rightSideOfBoard + ((((Game1.preferredWindowWidth - Game1.preferredWindowHeight) / 2) - zoomWidth) / 2), ((zoomHeight - zoomWidth) / 2), zoomWidth, zoomWidth),
                        new Rectangle(0, Game1.backgroundWidth - (int)(Game1.backgroundWidth * (ratioCornerBoxes + ratioBlackBorder * 2)), (int)(Game1.backgroundWidth * (ratioCornerBoxes + ratioBlackBorder * 2)), (int)(Game1.backgroundHeight * ratioSideHeight)),
                        Color.White, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0f);
                    break;
            }
        }

        public static void CalculateBoardBoxCenterPositions()
        {
            centerBoardPositions = new Vector2[48];

            // Switching these to static based on 1000x600
            // Going to hardcode these because it's always slightly off when calculating based on ratios
            #region Hardcoded center of tiles code
            int middleOfBottomRowY = 565;
            int middleOfLeftColumnX = 235;
            int middleOfRightColumnX = 765;
            int middleOfTopRowY = 35;

            // Bottom Row
            centerBoardPositions[1] = new Vector2(708, middleOfBottomRowY);
            centerBoardPositions[2] = new Vector2(667, middleOfBottomRowY);
            centerBoardPositions[3] = new Vector2(624, middleOfBottomRowY);
            centerBoardPositions[4] = new Vector2(583, middleOfBottomRowY);
            centerBoardPositions[5] = new Vector2(542, middleOfBottomRowY);
            centerBoardPositions[6] = new Vector2(500, middleOfBottomRowY);
            centerBoardPositions[7] = new Vector2(459, middleOfBottomRowY);
            centerBoardPositions[8] = new Vector2(417, middleOfBottomRowY);
            centerBoardPositions[9] = new Vector2(375, middleOfBottomRowY);
            centerBoardPositions[10] = new Vector2(333, middleOfBottomRowY);
            centerBoardPositions[11] = new Vector2(291, middleOfBottomRowY);

            // Left Column
            centerBoardPositions[13] = new Vector2(middleOfLeftColumnX, 508);
            centerBoardPositions[14] = new Vector2(middleOfLeftColumnX, 467);
            centerBoardPositions[15] = new Vector2(middleOfLeftColumnX, 424);
            centerBoardPositions[16] = new Vector2(middleOfLeftColumnX, 383);
            centerBoardPositions[17] = new Vector2(middleOfLeftColumnX, 342);
            centerBoardPositions[18] = new Vector2(middleOfLeftColumnX, 300);
            centerBoardPositions[19] = new Vector2(middleOfLeftColumnX, 259);
            centerBoardPositions[20] = new Vector2(middleOfLeftColumnX, 217);
            centerBoardPositions[21] = new Vector2(middleOfLeftColumnX, 175);
            centerBoardPositions[22] = new Vector2(middleOfLeftColumnX, 133);
            centerBoardPositions[23] = new Vector2(middleOfLeftColumnX, 91);

            // Top Row
            centerBoardPositions[35] = new Vector2(708, middleOfTopRowY);
            centerBoardPositions[34] = new Vector2(667, middleOfTopRowY);
            centerBoardPositions[33] = new Vector2(624, middleOfTopRowY);
            centerBoardPositions[32] = new Vector2(583, middleOfTopRowY);
            centerBoardPositions[31] = new Vector2(542, middleOfTopRowY);
            centerBoardPositions[30] = new Vector2(500, middleOfTopRowY);
            centerBoardPositions[29] = new Vector2(459, middleOfTopRowY);
            centerBoardPositions[28] = new Vector2(417, middleOfTopRowY);
            centerBoardPositions[27] = new Vector2(375, middleOfTopRowY);
            centerBoardPositions[26] = new Vector2(333, middleOfTopRowY);
            centerBoardPositions[25] = new Vector2(291, middleOfTopRowY);

            // Left Column
            centerBoardPositions[47] = new Vector2(middleOfRightColumnX, 508);
            centerBoardPositions[46] = new Vector2(middleOfRightColumnX, 467);
            centerBoardPositions[45] = new Vector2(middleOfRightColumnX, 424);
            centerBoardPositions[44] = new Vector2(middleOfRightColumnX, 383);
            centerBoardPositions[43] = new Vector2(middleOfRightColumnX, 342);
            centerBoardPositions[42] = new Vector2(middleOfRightColumnX, 300);
            centerBoardPositions[41] = new Vector2(middleOfRightColumnX, 259);
            centerBoardPositions[40] = new Vector2(middleOfRightColumnX, 217);
            centerBoardPositions[39] = new Vector2(middleOfRightColumnX, 175);
            centerBoardPositions[38] = new Vector2(middleOfRightColumnX, 133);
            centerBoardPositions[37] = new Vector2(middleOfRightColumnX, 91);

            // Corner Boxes
            centerBoardPositions[0] = new Vector2(middleOfRightColumnX, middleOfBottomRowY);//new Vector2(window_rightSideOfBoard - (window_cornerBoxIncludingBorder / 2), Game1.preferredWindowHeight - (window_cornerBoxIncludingBorder / 2));
            centerBoardPositions[12] = new Vector2(middleOfLeftColumnX, middleOfBottomRowY);//new Vector2(window_leftSideOfBoard + (window_cornerBoxIncludingBorder / 2), Game1.preferredWindowHeight - (window_cornerBoxIncludingBorder / 2));
            centerBoardPositions[24] = new Vector2(middleOfLeftColumnX, middleOfTopRowY);//new Vector2(window_leftSideOfBoard + (window_cornerBoxIncludingBorder / 2), (window_cornerBoxIncludingBorder / 2));
            centerBoardPositions[36] = new Vector2(middleOfRightColumnX, middleOfTopRowY);//new Vector2(window_rightSideOfBoard - (window_cornerBoxIncludingBorder / 2), (window_cornerBoxIncludingBorder / 2));
            #endregion

            #region commented out the Dynamic Sizing code
            for (int i = 0; i < centerBoardPositions.Length; i++)
            {
                // Locations excluding Corners

                
                /*
                // Bottom Row
                if (i > 0 && i < 12)
                {
                    int temp = i - 1;
                    centerBoardPositions[i] = new Vector2(
                        (window_rightSideOfBoard - (window_cornerBox + window_blackBorder)) // The very left of Go
                        - ((window_oneSideBoxIncludingBorder) * temp) - (window_oneSideBoxIncludingBorder / 2),   // The middle of the box, lengthwise
                        Game1.preferredWindowHeight - (window_cornerBoxIncludingBorder / 2)   // The middle of the bottom row, heightwise
                        );

                    if (Game1.preferredWindowHeight < 720)
                        centerBoardPositions[i].X  -= (temp * (0.9f * (1 - (Game1.preferredWindowHeight / 720))));
                }

                // Left Column
                if (i > 12 && i < 24)
                {
                    int temp = i - 13;
                    centerBoardPositions[i] = new Vector2(
                        (window_leftSideOfBoard + (window_cornerBoxIncludingBorder / 2)), // The middle of the box, heightwise
                        Game1.preferredWindowHeight - (window_cornerBox + window_blackBorder) - (temp * window_oneSideBoxIncludingBorder) - (window_oneSideBoxIncludingBorder / 2)); // Middle of the box, widthwise
                }

                // Top Row
                if (i > 24 && i < 36)
                {
                    int temp = i - 24;
                    centerBoardPositions[i] = new Vector2(
                        (window_leftSideOfBoard + (window_cornerBox + window_blackBorder)) // The very right of Fan Meeting
                        + ((window_oneSideBoxIncludingBorder) * temp) - (window_oneSideBoxIncludingBorder / 2),   // The middle of the box, lengthwise
                        (window_cornerBoxIncludingBorder / 2)   // The middle of the bottom row, heightwise
                        );
                }

                // Right Column
                if (i > 36)
                {
                    int temp = i - 36;
                    centerBoardPositions[i] = new Vector2(
                        (window_rightSideOfBoard - (window_cornerBoxIncludingBorder / 2)), // The middle of the box, heightwise
                        (window_cornerBox + window_blackBorder) + (temp * window_oneSideBoxIncludingBorder) - (window_oneSideBoxIncludingBorder / 2)); // Middle of the box, widthwise
                }
                */



            }
            #endregion
        }

        public static void RearrangePiecesOnTile(int tileNumber)
        {
            int counter = 3;
            if (true)
            //if (numberOfPiecesInPosition[tileNumber] > 1)
            {
                // Left or Right Column
                if ((tileNumber > 12 && tileNumber < 24) || (tileNumber > 36 && tileNumber < 48))
                {
                    foreach (Player p in SoshilandGame.ListOfPlayers)
                    {
                        if (p.CurrentBoardPosition == tileNumber)
                        {
                            // First set the pieces back to the center
                            p.SetBoardPieceRectangleLocation((int)SoshiLandUIFunctions.centerBoardPositions[tileNumber].X, (int)SoshiLandUIFunctions.centerBoardPositions[tileNumber].Y);
                            // Offset piece
                            p.SetBoardPieceRectangleLocation(p.getBoardPieceRectangle.X + (counter * 6), p.getBoardPieceRectangle.Y);
                            // Decrement the counter
                            counter--;
                        }
                    }
                }
                else
                {
                    
                    // Check if it's the jail tile
                    if (tileNumber == 12)
                    {
                        foreach (Player p in SoshilandGame.ListOfPlayers)
                        {
                            if (p.CurrentBoardPosition == 12)
                            {
                                // Check if the player is in jail
                                if (!p.inJail)
                                {
                                    if (counter > 0)
                                    {
                                        // Set the pieces to the left bottom corner
                                        p.SetBoardPieceRectangleLocation((int)SoshiLandUIFunctions.centerBoardPositions[tileNumber].X - 18, (int)SoshiLandUIFunctions.centerBoardPositions[tileNumber].Y + 18);
                                        // Offset piece
                                        p.SetBoardPieceRectangleLocation(p.getBoardPieceRectangle.X + (counter * 12), p.getBoardPieceRectangle.Y);
                                        // Decrement the counter
                                        counter--;
                                    }
                                    else
                                    {
                                        // Set the pieces to the left bottom corner
                                        p.SetBoardPieceRectangleLocation((int)SoshiLandUIFunctions.centerBoardPositions[tileNumber].X - 18, (int)SoshiLandUIFunctions.centerBoardPositions[tileNumber].Y + 18);
                                        // Offset piece
                                        p.SetBoardPieceRectangleLocation(p.getBoardPieceRectangle.X, p.getBoardPieceRectangle.Y + (counter * 6));
                                        // Decrement the counter
                                        counter--;
                                    }

                                }
                                else
                                {
                                    // Player is in jail, so arrange pieces as they would be in jail
                                    // Set the pieces to the right top corner
                                    p.SetBoardPieceRectangleLocation((int)SoshiLandUIFunctions.centerBoardPositions[tileNumber].X + 20, (int)SoshiLandUIFunctions.centerBoardPositions[tileNumber].Y - 20);
                                    // Offset piece
                                    p.SetBoardPieceRectangleLocation(p.getBoardPieceRectangle.X - Math.Abs((counter * 3)), p.getBoardPieceRectangle.Y - Math.Abs((counter * 3)));
                                    // Decrement the counter
                                    counter--;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Player p in SoshilandGame.ListOfPlayers)
                        {
                            if (p.CurrentBoardPosition == tileNumber)
                            {
                                // First set the pieces back to the center
                                p.SetBoardPieceRectangleLocation((int)SoshiLandUIFunctions.centerBoardPositions[tileNumber].X, (int)SoshiLandUIFunctions.centerBoardPositions[tileNumber].Y);
                                // Offset piece
                                p.SetBoardPieceRectangleLocation(p.getBoardPieceRectangle.X, p.getBoardPieceRectangle.Y + (counter * 6));
                                // Decrement the counter
                                counter--;
                            }
                        }
                    }
                }
            }
        }

        public static int GetTileNumber(Props p)
        {
            switch (p)
            {
                case Props.Go:
                    return 0;
                case Props.BukitTimah:
                    return 1;
                case Props.CNTower:
                    return 2;
                case Props.KuwaitMuseum:
                    return 3;
                case Props.WalkOfFame:
                    return 4;
                case Props.LuxuryTax:
                    return 5;
                case Props.AngkorWat:
                    return 6;
                case Props.Disneyland:
                    return 7;
                case Props.Chance1:
                    return 8;
                case Props.AmazonRainforest:
                    return 9;
                case Props.HongKong:
                    return 10;
                case Props.UNHQ:
                    return 11;
                case Props.Babysit:
                    return 12;
                case Props.SydneyOpera:
                    return 13;
                case Props.GoldenGateBridge:
                    return 14;
                case Props.SoshiBond:
                    return 15;
                case Props.MalibuBeach:
                    return 16;
                case Props.BarcelonaAirport:
                    return 17;
                case Props.WencelsasSquare:
                    return 18;
                case Props.BarrierReef:
                    return 19;
                case Props.CommChest1:
                    return 20;
                case Props.Pisa:
                    return 21;
                case Props.BigBen:
                    return 22;
                case Props.GizaPyramid:
                    return 23;
                case Props.FanMeeting:
                    return 24;
                case Props.LaScala:
                    return 25;
                case Props.Bali:
                    return 26;
                case Props.Chance2:
                    return 27;
                case Props.TempleMount:
                    return 28;
                case Props.DamnoenMarket:
                    return 29;
                case Props.GreatWall:
                    return 30;
                case Props.TajMahal:
                    return 31;
                case Props.StatueLiberty:
                    return 32;
                case Props.Forever9:
                    return 33;
                case Props.EiffelTower:
                    return 34;
                case Props.Parthenon:
                    return 35;
                case Props.GoBabysit:
                    return 36;
                case Props.WhiteHouse:
                    return 37;
                case Props.GyeongBokGoong:
                    return 38;
                case Props.MountEverest:
                    return 39;
                case Props.CommChest2:
                    return 40;
                case Props.GrandCanal:
                    return 41;
                case Props.VenetianResort:
                    return 42;
                case Props.ChateauDeChillon:
                    return 43;
                case Props.TokyoDome:
                    return 44;
                case Props.ShoppingSpree:
                    return 45;
                case Props.Colosseum:
                    return 46;
                case Props.BlueHouse:
                    return 47;

            }

            // This would be an error
            return -1;
        }

        public static void DrawPiecesOnZoomBox(int tileNumber, SpriteBatch spriteBatch)
        {
            int counter = 0;
            foreach (Player p in SoshilandGame.ListOfPlayers)
            {
                if (p.CurrentBoardPosition == tileNumber)
                {
                    p.SetZoomPieceRectangleLocation((int)zoomBoxPiecesPositions[counter].X, (int)zoomBoxPiecesPositions[counter].Y);
                    spriteBatch.Draw(p.getBoardPiece, p.getZoomPieceRectangle, new Rectangle(0, 0, p.getBoardPiece.Width, p.getBoardPiece.Height), Color.White, 0, new Vector2(p.getBoardPiece.Width / 2, p.getBoardPiece.Height / 2), SpriteEffects.None, 0);
                    counter++;
                }
            }
        }
    }
}
