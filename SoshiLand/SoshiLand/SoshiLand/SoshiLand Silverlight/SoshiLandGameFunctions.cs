using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SoshiLandSilverlight
{
    public static class SoshiLandGameFunctions
    {
        private static double animationInterval = 16;
        private static double animationGameTime;
        private static float amountToJump = Game1.preferredWindowHeight * 0.03f;
        private static float amountToJumpInterval = amountToJump * 0.05f;

        private static float amountToMove = SoshiLandUIFunctions.window_oneSideBoxIncludingBorder;
        private static float amountToMoveInterval = amountToMove * 0.05f;

        private static bool amountToMoveSet = false;

        private static float animationTotalFrames = 10;
        private static float animationCounter = 10;

        private static int currentDesiredPlayerLocation;
        
        public static bool doneMoveAnimation = true;
        public static bool animatingBoardPieceMovement = false;
        public static bool firstMovement = false;

        private static RowLocation rowLocation;

        public static void startNextPlayerTurn(List<Player> ListOfPlayers)
        {
            if (SoshilandGame.currentTurnsPlayers != null)
                Game1.debugMessageQueue.addMessageToQueue("Player " + "\"" + SoshilandGame.currentTurnsPlayers.getName + "\"'s " + " turn ends");

            int previousPlayersTurn = ListOfPlayers.IndexOf(SoshilandGame.currentTurnsPlayers);
            int nextPlayersTurn;

            // Checks if the player is at the end of the list
            if (previousPlayersTurn == ListOfPlayers.Count - 1)
                nextPlayersTurn = 0;
            else
                nextPlayersTurn = previousPlayersTurn + 1;

            SoshilandGame.currentTurnsPlayers = ListOfPlayers.ElementAt(nextPlayersTurn);

            Game1.debugMessageQueue.addMessageToQueue("Player " + "\"" + SoshilandGame.currentTurnsPlayers.getName + "\"'s " + " turn begins");

            // Set phase to Pre Roll Phase
            SoshilandGame.turnPhase = 0;
        }

        public static void MovePlayerToJail(Player p)
        {
            Game1.debugMessageQueue.addMessageToQueue("Player " + "\"" + p.getName + "\"" + " goes to jail!");
            // Set jail flag for player
            p.inJail = true;
            MovePlayer(p, 12);

            // Set phase to Post Roll Phase
            SoshilandGame.turnPhase = 2;
        }

        public static void MovePlayer(Player p, int position)
        {
            // Update the player's current position to the new position
            p.CurrentBoardPosition = position;
            Game1.debugMessageQueue.addMessageToQueue("Player " + "\"" + p.getName + "\"" + " moves to Tile \"" + SoshilandGame.Tiles[position].getName + "\"");
        }

        public static void MovePlayerDiceRoll(Player p, int roll)
        {
            int currentPosition = p.CurrentBoardPosition;
            p.PreviousBoardPosition = currentPosition;
            int newPosition = currentPosition + roll;

            currentDesiredPlayerLocation = newPosition;

            // If player passes or lands on Go
            if (newPosition > 47)
            {
                newPosition = Math.Abs(newPosition - 48);           // Get absolute value of the difference and move player to that new Tile
                p.BankPaysPlayer(200);                              // Pay player $200 for passing Go
            }
            // Move player to the new position
            SoshiLandGameFunctions.MovePlayer(p, newPosition);
            animatingBoardPieceMovement = true;
        }

        public static void RollDice(Player p)
        {
            SoshilandGame.DoublesRolled = false;
            int dice1Int = SoshilandGame.die.Next(1, 6);
            int dice2Int = SoshilandGame.die.Next(1, 6);

            int total = dice1Int + dice2Int;

            SoshilandGame.currentDiceRoll = total;                // Set the global dice roll variable

            if (dice1Int == dice2Int && SoshilandGame.gameInitialized)
            {
                SoshilandGame.DoublesRolled = true;
                // Check if it's the third consecutive double roll
                if (SoshilandGame.numberOfDoubles == 2)
                    // Move player to jail
                    SoshiLandGameFunctions.MovePlayerToJail(p);
                else
                    // Increment number of doubles
                    SoshilandGame.numberOfDoubles++;
            }

            Game1.debugMessageQueue.addMessageToQueue("Player " + "\"" + p.getName + "\"" + " rolls dice: " + dice1Int + " and " + dice2Int + ". Total: " + total);
            if (SoshilandGame.DoublesRolled)
                Game1.debugMessageQueue.addMessageToQueue("Player " + "\"" + p.getName + "\"" + " rolled doubles!");

            // Only move if the player is not in jail
            if ((!p.inJail) && SoshilandGame.gameInitialized)
                SoshiLandGameFunctions.MovePlayerDiceRoll(p, total);
        }

        public static bool PayTenPercentWorthToBank(Player player)
        {
            int tenPercent = (int)Math.Round(player.getNetWorth * 0.10);  // Calculate 10% of Player's money

            if (player.getMoney >= tenPercent)              // Check if player has enough money to pay 10%
            {
                SoshilandGame.currentTurnsPlayers.PlayerPaysBank(tenPercent);                 // Player pays bank 10%
                Game1.debugMessageQueue.addMessageToQueue(
                    "Player " + "\"" + SoshilandGame.currentTurnsPlayers.getName + "\"" + " pays $" + tenPercent + " in taxes");
                return true;
            }
            else
            {
                Game1.debugMessageQueue.addMessageToQueue(
                    "Player " + "\"" + SoshilandGame.currentTurnsPlayers.getName + "\"" + " needs to pay $" + tenPercent + " but does not have enough money");
                return false;
            }
        }

        public static void FollowCardInstructions(Card card, Player player, List<Player> listOfPlayers)
        {
            if (card.getMoneyModifier != 0)             // Check if we need to do anything money related
            {
                bool negative = false;                  // Temporary bool to check if the money modifier is negative
                if (card.getMoneyModifier < 0)
                    negative = true;                    // Set negative flag

                switch (card.getPerPlayer)              // Check if the card affects all players
                {
                    case true:                          // Case when card affects all players
                        switch (negative)               // Check if we're removing money
                        {
                            case true:                  // Case when we're removing money from current player (current player pays all other players)
                                foreach (Player p in listOfPlayers)
                                    if (player != p)    // Player cannot pay him/herself
                                        player.CurrentPlayerPaysPlayer(p, Math.Abs(card.getMoneyModifier));     // Pay each player the amount
                                break;
                            case false:                 // Case when player pays the bank
                                foreach (Player p in listOfPlayers)
                                    if (player != p)    // Player cannot be paid by him/herself
                                        p.CurrentPlayerPaysPlayer(player, Math.Abs(card.getMoneyModifier));     // Each player pays the current player the amount
                                break;
                        }
                        break;
                    case false:                         // Case when card does not affect all players
                        switch (negative)               // Check if we're removing money
                        {
                            case true:                  // Case when we're removing money
                                player.PlayerPaysBank(Math.Abs(card.getMoneyModifier));         // Player pays bank
                                break;
                            case false:                 // Case when we're adding money
                                player.BankPaysPlayer(Math.Abs(card.getMoneyModifier));         // Bank pays player
                                break;
                        }
                        break;
                }
            }

            if (card.getMoveModifier != 0)              // Check if we need to do a move modification
            {
                // Note: since there are no cards that do this yet, going to skip this for now
            }

            if (card.getMovePosition != 0)              // Check if we need to do a position movement
            {
                if (card.getSpecialCardType == SpecialCardType.CanPassGo)   // Check if the card actually moves around the board
                {
                    if (player.CurrentBoardPosition > card.getMovePosition && player.CurrentBoardPosition <= 47)    // Check if the player will pass Go from his or her current location
                        player.BankPaysPlayer(200);         // Pay 200 since player will pass Go


                }
                MovePlayer(player, card.getMovePosition);
            }

            if (card.getSpecialCardType == SpecialCardType.GetOutOfJailFreeCard)
            {
                player.FreeJailCards += 1;          // Give player a get out of jail free card
                Game1.debugMessageQueue.addMessageToQueue("Player \"" + player.getName + "\" gets a Get Out of Jail Free Card");
            }

        }

        public static void MortgageProperty(PropertyTile property)
        {
            // Check if property can be mortgaged first - 0 houses and not already mortgaged
            if (property.getNumberOfHouses == 0 && !property.MortgageStatus)
            {
                property.MortgageStatus = true;                             // Set mortgage status to True
                property.Owner.BankPaysPlayer(property.getMortgageValue);   // Pay the owner of the property the mortgage value of the property
            }
            else
                Game1.debugMessageQueue.addMessageToQueue("Warning: Property cannot be mortgaged");
        }
        public static void MortgageUtility(UtilityTile utlity)
        {
            // Check if property can be mortgaged first - No houses and not already mortgaged
            if (!utlity.MortgageStatus)
            {
                utlity.MortgageStatus = true;                           // Set mortgage status to True
                utlity.Owner.BankPaysPlayer(utlity.getMortgageValue);   // Pay the owner of the property the mortgage value of the property
            }
            else
                Game1.debugMessageQueue.addMessageToQueue("Warning: Utility cannot be mortgaged");
        }
        public static void UnmortgageProperty(PropertyTile property)
        {
            // Check if property is mortgaged
            if (property.MortgageStatus)
            {
                int payment = (int)(property.getMortgageValue * 1.1f);      // Calculate mortgage + 10% interest
                property.Owner.PlayerPaysBank(payment);                     // Pay bank amount
            }
            else
                Game1.debugMessageQueue.addMessageToQueue("Warning: Property cannot be unmortgaged");
        }

        public static void UnmortgageUtility(UtilityTile utility)
        {
            // Check if property is mortgaged
            if (utility.MortgageStatus)
            {
                int payment = (int)(utility.getMortgageValue * 1.1f);      // Calculate mortgage + 10% interest
                utility.Owner.PlayerPaysBank(payment);                     // Pay bank amount
            }
            else
                Game1.debugMessageQueue.addMessageToQueue("Warning: Utility cannot be unmortgaged");
        }

        public static void AnimateJumpNextBox(Player p, GameTime gameTime, Vector2 fromPosition, Vector2 toPosition)
        {
            
            // Set the amount to move only once
            if (!amountToMoveSet)
            {
                // Check if the X values are equal (this means the position is on the left or right columns)
                if (fromPosition.X == toPosition.X)
                {
                    amountToMove = Math.Abs(fromPosition.Y - toPosition.Y);
                    amountToMoveInterval = amountToMove * 0.1f;

                    amountToJump = Game1.preferredWindowHeight * 0.03f;
                    amountToJumpInterval = amountToJump * 0.05f;

                    amountToJumpInterval = 0;

                    // Set the top or bottom row
                    if (fromPosition.X < Game1.preferredWindowWidth / 2)
                        rowLocation = RowLocation.LeftColumn;   // Left Column
                    else
                        rowLocation = RowLocation.RightColumn;  // Right Column
                }
                    // Otherwise, the position is on the bottom or top rows
                else
                {
                    amountToMove = Math.Abs(fromPosition.X - toPosition.X);
                    amountToMoveInterval = amountToMove * 0.1f;

                    amountToJump = Game1.preferredWindowHeight * 0.03f;
                    amountToJumpInterval = amountToJump * 0.05f;

                    // Set the left or right column
                    if (fromPosition.Y < Game1.preferredWindowHeight / 2)
                        rowLocation = RowLocation.TopRow;   // Left Column
                    else
                        rowLocation = RowLocation.BottomRow;  // Right Column
                }

                amountToMoveSet = true;

            }

            animationGameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (animationGameTime > animationInterval && animationCounter != 0 && !doneMoveAnimation)
            {
                // Reset the gametime
                animationGameTime = 0;  
                // Move the location by an interval
                /*
                if (animationCounter >= animationTotalFrames / 2)
                    p.SetBoardPieceRectangleLocation((int)(toPosition.X + ((toPosition.X - fromPosition.X) / animationCounter)), (int)(toPosition.Y + ((toPosition.Y - fromPosition.Y) / animationCounter)) + (int)amountToJumpInterval);
                else
                    p.SetBoardPieceRectangleLocation((int)(toPosition.X + ((toPosition.X - fromPosition.X) / animationCounter)), (int)(toPosition.Y + ((toPosition.Y - fromPosition.Y) / animationCounter)) - (int)amountToJumpInterval);
                */
                switch (rowLocation)
                {
                    case RowLocation.BottomRow:
                        if (animationCounter > (animationTotalFrames * 0.5f) + 1)
                            p.SetBoardPieceRectangleLocation((int)(p.getBoardPieceRectangle.X - amountToMoveInterval), (int)(p.getBoardPieceRectangle.Y - amountToJumpInterval));
                        else
                            p.SetBoardPieceRectangleLocation((int)(p.getBoardPieceRectangle.X - amountToMoveInterval), (int)(p.getBoardPieceRectangle.Y + amountToJumpInterval));
                        break;
                    case RowLocation.LeftColumn:
                        if (animationCounter > (animationTotalFrames * 0.5f) + 1)
                            p.SetBoardPieceRectangleLocation((int)(p.getBoardPieceRectangle.X + amountToJumpInterval), (int)(p.getBoardPieceRectangle.Y - amountToMoveInterval));
                        else
                            p.SetBoardPieceRectangleLocation((int)(p.getBoardPieceRectangle.X - amountToJumpInterval), (int)(p.getBoardPieceRectangle.Y - amountToMoveInterval));
                        break;
                    case RowLocation.TopRow:
                        if (animationCounter > (animationTotalFrames * 0.5f) + 1)
                            p.SetBoardPieceRectangleLocation((int)(p.getBoardPieceRectangle.X + amountToMoveInterval), (int)(p.getBoardPieceRectangle.Y - amountToJumpInterval));
                        else
                            p.SetBoardPieceRectangleLocation((int)(p.getBoardPieceRectangle.X + amountToMoveInterval), (int)(p.getBoardPieceRectangle.Y + amountToJumpInterval));
                        break;
                    case RowLocation.RightColumn:
                        if (animationCounter > (animationTotalFrames * 0.5f) + 1)
                            p.SetBoardPieceRectangleLocation((int)(p.getBoardPieceRectangle.X - amountToJumpInterval), (int)(p.getBoardPieceRectangle.Y + amountToMoveInterval));
                        else
                            p.SetBoardPieceRectangleLocation((int)(p.getBoardPieceRectangle.X + amountToJumpInterval), (int)(p.getBoardPieceRectangle.Y + amountToMoveInterval));
                        break;
                }
                
                animationCounter--;
            }
            // Check if animation is done
            if (animationCounter == 0)
            {
                amountToMoveSet = false;
                doneMoveAnimation = true;
                animationCounter = 10;
                p.SetBoardPieceRectangleLocation((int)toPosition.X, (int)toPosition.Y);
            }

            // Check when animation has reached the desired location
            CheckIfPlayerReachesDesiredLocation(p, toPosition);
        }

        private static void CheckIfPlayerReachesDesiredLocation(Player p, Vector2 toPosition)
        {
            if (p.getBoardPieceRectangle.X == SoshiLandUIFunctions.centerBoardPositions[p.CurrentBoardPosition].X &&
                p.getBoardPieceRectangle.Y == SoshiLandUIFunctions.centerBoardPositions[p.CurrentBoardPosition].Y)
            {
                animatingBoardPieceMovement = false;
                firstMovement = false;
            }
        }
    }
}
