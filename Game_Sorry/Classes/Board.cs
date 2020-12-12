//-----------------------------------------------------------------------
// <copyright file="Board.cs" company="ColeSeanStevenSueCompany">
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
    /// The class used to represent the board.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not taught.")]
    public class Board
    {
        /// <summary>
        /// The card deck.
        /// </summary>
        public Deck CardDeck;

        /// <summary>
        /// The current player.
        /// </summary>
        public Player CurrentPlayer;

        /// <summary>
        /// The players.
        /// </summary>
        public Player[] Players;

        /// <summary>
        /// The spaces.
        /// </summary>
        public Space[] Space;
    }
}