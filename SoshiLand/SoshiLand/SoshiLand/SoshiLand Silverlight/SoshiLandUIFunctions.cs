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
        private static int window_leftSideOfBoard = ((Game1.preferredWindowWidth - (Game1.preferredWindowHeight)) / 2);
        private static int window_rightSideOfBoard = window_leftSideOfBoard + Game1.preferredWindowHeight;
        private static int window_oneSideBox = (int)(Game1.preferredWindowHeight * (ratioBlackBorder + ratioSideBoxes));
        private static int window_oneSideBoxIncludingBorder = (int)(window_oneSideBox + (ratioBlackBorder * 2));
        private static int window_cornerBox = (int)(Game1.preferredWindowHeight * (ratioCornerBoxes));
        private static int window_cornerBoxIncludingBorder = (int)(window_cornerBox + (ratioBlackBorder * 2));

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
                    //return Props.FanMeeting;
                    return Props.None;
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
                        new Rectangle(window_rightSideOfBoard, (zoomHeight / 2), zoomWidth, zoomHeight),
                        new Rectangle(orig_startOfSideRow + (positionFromLeftToRightOrTopToBottomExcludingCorner - 1) * orig_oneSideBox, 0, (int)(Game1.backgroundWidth * (ratioSideBoxes + ratioBlackBorder * 2)), (int)(Game1.backgroundHeight * ratioSideHeight)),
                        Color.White, MathHelper.ToRadians(180), new Vector2(zoomWidth / 2, zoomHeight / 2), SpriteEffects.None, 0f);
                    break;

                    // Bottom Row items
                    
                    // Left Column Items

                    // Right Column Items
                    //case Props.FanMeeting
                    //spriteBatch.d
            }
        }

    }
}
