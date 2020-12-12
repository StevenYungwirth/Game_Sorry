//-----------------------------------------------------------------------
// <copyright file="Card7Results.cs" company="ColeSeanStevenSueCompany">
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

    public sealed class Card7Results
    {
        /// <summary>
        /// The first pawn's move value.
        /// </summary>
        public int pawn1MoveValue;

        /// <summary>
        /// The second pawn's move value.
        /// </summary>
        public int pawn2MoveValue;

        /// <summary>
        /// Create a new Lazy.
        /// </summary>
        private static readonly Lazy<Card7Results>
            lazy = new Lazy<Card7Results>(() => new Card7Results());

        /// <summary>
        /// Returns the Lazy instance.
        /// </summary>
        public static Card7Results Instance
        {
            get { return lazy.Value; }
        }

        /// <summary>
        /// The constructor
        /// </summary>
        private Card7Results()
        {

        }
    }
}
