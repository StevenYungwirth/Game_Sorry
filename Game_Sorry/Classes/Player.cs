//-----------------------------------------------------------------------
// <copyright file="Player.cs" company="Sean Beyer">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Game_Sorry
{
    /// <summary>
    /// The class used to represent a player.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not taught.")]
    public class Player
    {
        /// <summary>
        /// The players name.
        /// </summary>
        public string Name;

        /// <summary>
        /// The player's color.
        /// </summary>
        public SolidColorBrush Color;

        /// <summary>
        /// The player's Pawn.
        /// </summary>
        public List<Pawn> Pawns;

        /// <summary>
        /// The player's first space.
        /// </summary>
        public int FirstSpace;

        /// <summary>
        /// The player's last space before they go into the safety zone.
        /// </summary>
        public int LastSpace;

        /// <summary>
        /// The player's start space.
        /// </summary>
        public List<int> StartSpaces;

        /// <summary>
        /// The player's last spaces before the safety zone.
        /// </summary>
        public List<int> HomeStretch;

        /// <summary>
        /// The player's safety zone spaces.
        /// </summary>
        public Dictionary<int, int> SafetySpaces;

        /// <summary>
        /// The player's home spaces.
        /// </summary>
        public List<int> HomeSpaces;

        /// <summary>
        /// Whether the player gets an additional turn.
        /// </summary>
        public bool AdditionalTurn;

        /// <summary>
        /// Whether the player has drawn a card.
        /// </summary>
        public bool HasDrawnCard;

        /// <summary>
        /// The current card that the player drew.
        /// </summary>
        public int CurrentCardNumber;

        /// <summary>
        /// Whether the player selected a valid pawn.
        /// </summary>
        public bool HasSelectedValidPawn;

        /// <summary>
        /// The pawn selected by the player.
        /// </summary>
        public Pawn SelectedPawn;

        /// <summary>
        /// The player's choice.
        /// </summary>
        public bool? CardChoice;

        /// <summary>
        /// The first pawn's movement when a 7 is drawn.
        /// </summary>
        public int Pawn1Split;

        /// <summary>
        /// The second pawn's movement when a 7 is drawn.
        /// </summary>
        public int Pawn2Split;

        /// <summary>
        /// Draw a card for the player.
        /// </summary>
        public int DrawCard(Deck cardDeck)
        {
            // Take a card from the deck
            if (cardDeck.cardsLeft == 0)
            {
                // The deck is empty. Reshuffle all the cards back into the deck
                cardDeck.Shuffle();
            }

            // Get a card from the deck
            Random rand = new Random();
            int deckSize = cardDeck.deckSize;
            int cardNumber = rand.Next(deckSize);

            while (!cardDeck.Cards[cardNumber].isInDeck)
            {
                // The card is not in the deck, draw another card
                cardNumber = rand.Next(deckSize);
            }

            // Take the card out of the deck and return its number
            cardDeck.Cards[cardNumber].isInDeck = false;
            cardDeck.cardsLeft--;
            return cardNumber;
        }
    }
}
