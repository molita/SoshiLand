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

        // Ratios based on the 7050x7050 board
        private static float ratioBlackBorder = 20f / 7050f;
        private static float ratioCornerBoxes = 800f / 7050f;
        private static float ratioSideBoxes = 470f / 7050f;
        private static float ratioSideHeight = 840f / 7050f;

        // Numbers for UI
        private static float zoomRatio = 0.4f;

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

        // Relative to the original size
        private static int orig_startOfSideRow = (int)(Game1.backgroundHeight * (ratioBlackBorder + ratioCornerBoxes));
        private static int orig_oneSideBox = (int)(Game1.backgroundHeight * (ratioBlackBorder + ratioSideBoxes));
        private static int orig_oneSideBoxIncludingBorder = (int)(Game1.backgroundHeight * ((ratioBlackBorder * 2) + ratioSideBoxes));
        private static int orig_cornerBoxIncludingBorder = (int)(Game1.backgroundHeight * ((ratioBlackBorder * 2) + ratioCornerBoxes));

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
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 8))
                    return Props.StatueLiberty;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 9))
                    return Props.Forever9;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 10))
                    return Props.EiffelTower;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 11))
                    return Props.Parthenon;
                else if (mouseState.X <= window_leftSideOfBoard + window_cornerBoxIncludingBorder + (window_oneSideBoxIncludingBorder * 11) + window_cornerBoxIncludingBorder)
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
                    return Props.CommChest;
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
                    return Props.CommChest;
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
                case Props.CommChest:
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

            for (int i = 0; i < centerBoardPositions.Length; i++)
            {
                // Locations excluding Corners

             
                // Bottom Row
                if (i > 0 && i < 12)
                {
                    int temp = i - 1;
                    centerBoardPositions[i] = new Vector2(
                        (window_rightSideOfBoard - (window_cornerBox + window_blackBorder)) // The very left of Go
                        - ((window_oneSideBoxIncludingBorder) * temp) - (window_oneSideBoxIncludingBorder / 2),   // The middle of the box, lengthwise
                        Game1.preferredWindowHeight - (window_cornerBoxIncludingBorder / 2)   // The middle of the bottom row, heightwise
                        );  
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

                // Corner Boxes

                // Go
                if (i == 0)
                    centerBoardPositions[i] = new Vector2(window_rightSideOfBoard - (window_cornerBoxIncludingBorder / 2), Game1.preferredWindowHeight - (window_cornerBoxIncludingBorder / 2));
                if (i == 12)
                    centerBoardPositions[i] = new Vector2(window_leftSideOfBoard + (window_cornerBoxIncludingBorder / 2), Game1.preferredWindowHeight - (window_cornerBoxIncludingBorder / 2));
                if (i == 24)
                    centerBoardPositions[i] = new Vector2(window_leftSideOfBoard + (window_cornerBoxIncludingBorder / 2), (window_cornerBoxIncludingBorder / 2));
                if (i == 36)
                    centerBoardPositions[i] = new Vector2(window_rightSideOfBoard - (window_cornerBoxIncludingBorder / 2), (window_cornerBoxIncludingBorder / 2));
            }

            Console.WriteLine("test");
        }
    }
}
