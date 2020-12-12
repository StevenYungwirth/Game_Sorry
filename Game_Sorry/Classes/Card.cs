//-----------------------------------------------------------------------
// <copyright file="Card.cs" company="ColeSeanStevenSueCompany">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace Game_Sorry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The class used to represent the card.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not taught.")]
    public class Card
    {
        /// <summary>
        /// The card's ID.
        /// </summary>
        public int ID;

        /// <summary>
        /// The Move Value.
        /// </summary>
        public int MoveValue;

        /// <summary>
        /// Whether the card is in the deck.
        /// </summary>
        public bool isInDeck;

        /// <summary>
        /// Message to display when the player can move with the drawn card.
        /// </summary>
        public string CanMove()
        {
            if (this.MoveValue == 0)
            {
                return ", you drew a Sorry! Select a pawn from start and swap it with an enemy pawn!";
            }
            else if (this.MoveValue == 1)
            {
                return ", you drew a 1. Select a pawn to move out of start, or move one of your other pawns 1 space!";
            }
            else if (this.MoveValue == 2)
            {
                return ", you drew a 2. Select a pawn to move out of start or move one of your other pawns 2 spaces. Also take another turn!";
            }
            else if (this.MoveValue == 3 || this.MoveValue == 5 || this.MoveValue == 12)
            {
                return ", you drew a " + this.MoveValue + ". Select a pawn to move " + this.MoveValue + " spaces!";
            }
            else if (this.MoveValue == -4)
            {
                return ", you drew a 4. Select a pawn to move backwards 4 spaces!";
            }
            else if (this.MoveValue == 7)
            {
                return ", you drew a 7. Select a pawn to move forwards 7 spaces, or split this movement between two of your pawns.";
            }
            else if (this.MoveValue == 8)
            {
                return ", you drew an 8. Select a pawn to move 8 spaces!";
            }
            else if (this.MoveValue == 10)
            {
                return ", you drew a 10. Select a pawn to move forwards 10 spaces, or backwards 1 space.";
            }
            else
            {
                return ", you drew an 11. Select a pawn to move forwards 11 spaces or swap places with an opponent's pawn.";
            }
        }

        /// <summary>
        /// Message to display when the player cannot move with the drawn card.
        /// </summary>
        public string CannotMove()
        {
            if (this.MoveValue == 0)
            {
                return ", you drew a Sorry! You aren't able to select a pawn from start and swap it with an enemy pawn. It is the next player's turn";
            }
            else if (this.MoveValue != -4 && this.MoveValue != 8 && this.MoveValue != 11)
            {
                return ", you drew a " + this.MoveValue + ". You do not have any valid moves. It is the next player's turn.";
            }
            else if (this.MoveValue == -4)
            {
                return ", you drew a 4. You do not have any valid moves. It is the next player's turn.";
            }
            else
            {
                return ", you drew an " + this.MoveValue + ". You do not have any valid moves. It is the next player's turn.";
            }
        }
    }
}
