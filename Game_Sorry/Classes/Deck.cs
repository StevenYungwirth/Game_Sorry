//-----------------------------------------------------------------------
// <copyright file="Deck.cs" company="ColeSeanStevenSueCompany">
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
    /// The class used to represent the deck.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not taught.")]
    public class Deck
    {
        /// <summary>
        /// The cards in the deck.
        /// </summary>
        public Card[] Cards;

        /// <summary>
        /// The size of the deck.
        /// </summary>
        public int deckSize;

        /// <summary>
        /// The number of cards left in the deck.
        /// </summary>
        public int cardsLeft;

        /// <summary>
        /// Initialize the list of cards that are in the deck.
        /// </summary>
        public void Shuffle()
        {
            // Set the number of cards for each move value we need in the deck.
            int[] cardCount = new int[13] { 4, 5, 4, 4, 4, 4, 0, 4, 4, 0, 4, 4, 4 };
            //int[] cardCount = new int[13] { 0, deckSize, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


            Cards = new Card[deckSize];
            int cardIndex = 0;
            for (int i = 0; i < cardCount.Length; i++)
            {
                for (int j = 0; j < cardCount[i]; j++)
                {
                    Cards[cardIndex] = new Card();
                    Cards[cardIndex].ID = cardIndex;
                    // Checks if card has move value of 4 if so change it to a negative 4
                    if (i == 4)
                    {
                        Cards[cardIndex].MoveValue = -4;
                    }
                    else
                    {
                        Cards[cardIndex].MoveValue = i;
                    }
                    Cards[cardIndex].isInDeck = true;
                    cardIndex++;
                }
            }

            cardsLeft = deckSize;
        }
    }
}
