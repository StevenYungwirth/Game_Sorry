//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="ColeSeanStevenSueCompany">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Threading;

namespace Game_Sorry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Contains interaction logic for MainWindow.xaml.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not yet taught.")]
    public partial class MainWindow : Window
    {
        private Board sorryBoard;

        // Number of cards in the deck.
        const int DECKSIZE = 45;

        /// <summary>
        /// Number of Main spaces on the board.
        /// </summary>
        const int SPACECOUNT = 112;
        const int MAINSPACECOUNT = 60;

        /// <summary>
        /// Number of total players on the board.
        /// </summary>
        private const int NUMBEROFPLAYERS = 4;

        /// <summary>
        /// Number of pawns per player.
        /// </summary>
        private const int NUMBEROFPAWNS = 4;

        /// <summary>
        /// Player names.
        /// </summary>
        private readonly string[] PLAYERNAMES = new string[NUMBEROFPLAYERS]
        {
            "Red",
            "Blue",
            "Green",
            "Yellow"
        };

        /// <summary>
        /// Player colors.
        /// </summary>
        private readonly SolidColorBrush[] PLAYERCOLORS = new SolidColorBrush[NUMBEROFPLAYERS]
        {
            new SolidColorBrush(Colors.Red),
            new SolidColorBrush(Colors.Blue),
            new SolidColorBrush(Colors.Green),
            new SolidColorBrush(Colors.Yellow)
        };

        /// <summary>
        /// Start Spaces.
        /// </summary>
        private readonly int[,] STARTSPACES = new int[NUMBEROFPLAYERS, NUMBEROFPAWNS]
        {
            { 96, 97, 98, 99 },
            { 100, 101, 102, 103 },
            { 104, 105, 106, 107 },
            { 108, 109, 110, 111 }
        };

        /// <summary>
        /// Home Spaces.
        /// </summary>
        private readonly int[,] HOMESPACES = new int[NUMBEROFPLAYERS, NUMBEROFPAWNS]
        {
            { 80, 81, 82, 83 },
            { 84, 85, 86, 87 },
            { 88, 89, 90, 91 },
            { 92, 93, 94, 95 }
        };

        /// <summary>
        /// The largest move value.
        /// </summary>
        private const int LARGESTCARD = 12;

        /// <summary>
        /// Home stretch spaces.
        /// </summary>
        private readonly int[,] HOMESTRETCHSPACES = new int[NUMBEROFPLAYERS, LARGESTCARD]
        {
            { 51, 52, 53, 54, 55, 56, 57, 58, 59, 0, 1, 2 },
            { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 },
            { 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 },
            { 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47 }
        };

        /// <summary>
        /// Safety spaces.
        /// </summary>
        private readonly Dictionary<int, int>[] SAFETYSPACES = new Dictionary<int, int>[NUMBEROFPLAYERS]
        {
            new Dictionary<int, int>()
            {
                {1, 60},
                {2, 61},
                {3, 62},
                {4, 63},
                {5, 64}
            },
            new Dictionary<int, int>
            {
                {1, 65},
                {2, 66},
                {3, 67},
                {4, 68},
                {5, 69}
            },
            new Dictionary<int, int>
            {
                {1, 70},
                {2, 71},
                {3, 72},
                {4, 73},
                {5, 74}
            },
            new Dictionary<int, int>
            {
                {1, 75},
                {2, 76},
                {3, 77},
                {4, 78},
                {5, 79}
            }
        };

        /// <summary>
        /// First Spaces.
        /// </summary>
        private readonly int[] FIRSTSPACES = new int[NUMBEROFPLAYERS]
        {
            4,
            19,
            34,
            49
        };

        /// <summary>
        /// Last Spaces.
        /// </summary>
        private readonly int[] LASTSPACES = new int[NUMBEROFPLAYERS]
        {
            2,
            17,
            32,
            47
        };

        /// <summary>
        /// Is the click event disabled.
        /// </summary>
        private bool IsClickDisabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Displays information about the application.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileInfo fileInfo = new FileInfo(assembly.Location);
            MessageBox.Show(Assembly.GetExecutingAssembly().GetName().Name + "\n By: " + versionInfo.LegalCopyright + "\n Version: " + versionInfo.FileVersion + "\n" + fileInfo.LastWriteTime);

        }

        /// <summary>
        /// Quits the game.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void QuitGameButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Restarts the game.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void RestartGameButton_Click(object sender, RoutedEventArgs e)
        {
            messageCenterTextBlock.Text = "";
            StartGameButton_Click(null, null);
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments for the event.</param>
        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            messageCenterTextBlock.Text = "Take a turn.";

            // Create an instance of the Board class.
            this.sorryBoard = new Board();

            // Create an instance of the Deck class.
            this.sorryBoard.CardDeck = new Deck();
            this.sorryBoard.CardDeck.deckSize = DECKSIZE;
            this.sorryBoard.CardDeck.Shuffle();

            // Create an instance of the Space class
            this.sorryBoard.Space = new Space[SPACECOUNT];
            for (int i = 0; i < SPACECOUNT; i++)
            {
                // Set the Spaces
                Space tempSpace = new Space();
                tempSpace.SpaceBorder = (Border)this.FindName("spaceBorder" + i);
                tempSpace.SpaceBorder.Background = null;
                tempSpace.SpaceBorder.MouseLeftButtonUp += new MouseButtonEventHandler(SelectPawn);

                // 0 to 59 are the main board spaces
                if (i < 60 && ((i - 1) % 15 == 0 || (i - 9) % 15 == 0))
                {
                    tempSpace.Slide = new Slide();
                    if (i <= 15)
                    {
                        tempSpace.Slide.Color = new SolidColorBrush(Colors.Red);
                    }
                    else if (i <= 30)
                    {
                        tempSpace.Slide.Color = new SolidColorBrush(Colors.Blue);
                    }
                    else if (i <= 45)
                    {
                        tempSpace.Slide.Color = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        tempSpace.Slide.Color = new SolidColorBrush(Colors.Yellow);
                    }

                    if ((i - 1) % 15 == 0)
                    {
                        // Short slide, spaces 
                        tempSpace.Slide.SlideStart = i;
                        tempSpace.Slide.SlideEnd = i + 3;
                    }
                    else
                    {
                        // Long slide
                        tempSpace.Slide.SlideStart = i;
                        tempSpace.Slide.SlideEnd = i + 4;
                    }
                }

                // Add the space to the array
                this.sorryBoard.Space[i] = tempSpace;
            }

            // Initialize Players
            this.sorryBoard.Players = new Player[NUMBEROFPLAYERS];

            // Initialize each player
            for (int player = 0; player < NUMBEROFPLAYERS; player++)
            {
                Player tempPlayer = new Player();
                tempPlayer.Name = PLAYERNAMES[player];
                tempPlayer.Color = PLAYERCOLORS[player];
                tempPlayer.CardChoice = null;
                tempPlayer.AdditionalTurn = false;
                tempPlayer.FirstSpace = FIRSTSPACES[player];
                tempPlayer.LastSpace = LASTSPACES[player];
                tempPlayer.StartSpaces = new List<int>();
                tempPlayer.HomeSpaces = new List<int>();
                for (int space = 0; space < NUMBEROFPAWNS; space++)
                {
                    tempPlayer.StartSpaces.Add(STARTSPACES[player, space]);
                    tempPlayer.HomeSpaces.Add(HOMESPACES[player, space]);
                }

                tempPlayer.HomeStretch = new List<int>();
                for (int space = 0; space < LARGESTCARD; space++)
                {
                    tempPlayer.HomeStretch.Add(HOMESTRETCHSPACES[player, space]);
                }
                tempPlayer.SafetySpaces = SAFETYSPACES[player];

                // Initialize player's Pawns
                tempPlayer.Pawns = new List<Pawn>();
                for (int i = 0; i < NUMBEROFPAWNS; i++)
                {
                    Pawn pawn = new Pawn();
                    pawn.CurrentSpace = tempPlayer.StartSpaces[i];
                    tempPlayer.Pawns.Add(pawn);
                    this.sorryBoard.Space[tempPlayer.Pawns[i].CurrentSpace].SpaceBorder.Background = tempPlayer.Color;
                    pawn.IsInStart = true;
                    pawn.IsInHome = false;
                }

                // Add tempPlayer to players
                this.sorryBoard.Players[player] = tempPlayer;
            }

            // Create an instance of the current player class.
            this.sorryBoard.CurrentPlayer = new Player();

            // Set the current player to the red player
            this.sorryBoard.CurrentPlayer = sorryBoard.Players.FirstOrDefault(x => x.Name == "Red");

            // Set the board controls
            startGameButton.IsEnabled = false;
            restartGameButton.IsEnabled = true;
            drawCardButton.IsEnabled = true;
            playerTurnLabel.Content = "Red's Turn";

            List<Space> slideSpaces = new List<Space>();
            foreach (Space space in this.sorryBoard.Space)
            {
                if (space.Slide != null)
                {
                    slideSpaces.Add(space);
                }
            }
        }

        private void SetPawnFlags(Pawn pawn)
        {
            if (this.sorryBoard.CurrentPlayer.StartSpaces.Contains(pawn.CurrentSpace))
            {
                pawn.IsInStart = true;
                pawn.IsInHome = false;
                pawn.IsInSafety = false;
            }

            if (this.sorryBoard.CurrentPlayer.HomeSpaces.Contains(pawn.CurrentSpace))
            {
                pawn.IsInStart = false;
                pawn.IsInHome = true;
                pawn.IsInSafety = false;
            }

            foreach (KeyValuePair<int, int> i in this.sorryBoard.CurrentPlayer.SafetySpaces)
            {
                // Find which number safety space the pawn is on
                if (i.Value == pawn.CurrentSpace)
                {
                    pawn.IsInStart = false;
                    pawn.IsInHome = false;
                    pawn.IsInSafety = true;
                }
            }

            if (pawn.CurrentSpace >= 0 && pawn.CurrentSpace < MAINSPACECOUNT)
            {
                pawn.IsInStart = false;
                pawn.IsInHome = false;
                pawn.IsInSafety = false;
            }
        }

        private void SelectPawn(object sender, MouseButtonEventArgs e)
        {
            if (IsClickDisabled)
            {
                e.Handled = true;
                return;
            }

            Card currentCard = this.sorryBoard.CardDeck.Cards[this.sorryBoard.CurrentPlayer.CurrentCardNumber];

            if (sender != null && this.sorryBoard.CurrentPlayer.HasDrawnCard && !this.sorryBoard.CurrentPlayer.HasSelectedValidPawn)
            {
                Border space = sender as Border;

                string spaceName = space.Name;
                int spaceNumber = int.Parse(spaceName.Substring(11, spaceName.Length - 11));

                foreach (Pawn pawn in this.sorryBoard.CurrentPlayer.Pawns)
                {
                    if (pawn.CurrentSpace == spaceNumber && CanBeMoved(pawn) && space.Background == this.sorryBoard.CurrentPlayer.Color)
                    {
                        this.sorryBoard.CurrentPlayer.HasSelectedValidPawn = true;
                        this.sorryBoard.CurrentPlayer.SelectedPawn = pawn;
                    }
                }

                if (this.sorryBoard.CurrentPlayer.HasSelectedValidPawn && currentCard.MoveValue != 0)
                {
                    if (currentCard.MoveValue == 11)
                    {
                        if (this.sorryBoard.CurrentPlayer.CardChoice != null)
                        {
                            if (this.sorryBoard.CurrentPlayer.CardChoice == false)
                            {
                                MovePawn(this.sorryBoard.CurrentPlayer.SelectedPawn, this.sorryBoard.CardDeck.Cards[this.sorryBoard.CurrentPlayer.CurrentCardNumber].MoveValue);
                                messageCenterTextBlock.Text = "Draw a card.";
                                EndTurn();
                            }
                            else
                            {
                                messageCenterTextBlock.Text = "Select an opponent's pawn you want to swap with";
                            }
                        }
                    }
                    else if (currentCard.MoveValue == 10)
                    {
                        if (this.sorryBoard.CurrentPlayer.CardChoice != null)
                        {
                            MovePawn(this.sorryBoard.CurrentPlayer.SelectedPawn, currentCard.MoveValue);
                            messageCenterTextBlock.Text = "Draw a card.";
                            EndTurn();
                        }
                    }
                    else if (currentCard.MoveValue == 7)
                    {
                        if (this.sorryBoard.CurrentPlayer.Pawn1Split == 0)
                        {
                            if (this.sorryBoard.CurrentPlayer.Pawn2Split != 0)
                            {
                                MovePawn(this.sorryBoard.CurrentPlayer.SelectedPawn, this.sorryBoard.CurrentPlayer.Pawn2Split);
                                this.sorryBoard.CurrentPlayer.Pawn2Split = 0;
                                messageCenterTextBlock.Text = "Draw a card.";
                                EndTurn();
                            }
                            else
                            {
                                MovePawn(this.sorryBoard.CurrentPlayer.SelectedPawn, currentCard.MoveValue);
                                messageCenterTextBlock.Text = "Draw a card.";
                                EndTurn();
                            }
                        }
                        else
                        {
                            MovePawn(this.sorryBoard.CurrentPlayer.SelectedPawn, this.sorryBoard.CurrentPlayer.Pawn1Split);
                            this.sorryBoard.CurrentPlayer.Pawn1Split = 0;
                            this.sorryBoard.CurrentPlayer.HasSelectedValidPawn = false;
                            messageCenterTextBlock.Text =
                                "Select your second pawn to move " +
                                this.sorryBoard.CurrentPlayer.Pawn2Split.ToString() + " spaces.";
                        }
                    }
                    else
                    {
                        if (this.sorryBoard.CurrentPlayer.SelectedPawn.IsInStart)
                        {
                            this.sorryBoard.CardDeck.Cards[this.sorryBoard.CurrentPlayer.CurrentCardNumber].MoveValue = 1;
                        }
                        MovePawn(this.sorryBoard.CurrentPlayer.SelectedPawn, this.sorryBoard.CardDeck.Cards[this.sorryBoard.CurrentPlayer.CurrentCardNumber].MoveValue);
                        EndTurn();
                    }
                }
                else if (this.sorryBoard.CurrentPlayer.HasSelectedValidPawn && currentCard.MoveValue == 0)
                {
                    messageCenterTextBlock.Text = "Select an opponent's pawn you want to Sorry";
                }
                else
                {
                    MessageBox.Show("Select a different pawn.");
                }
            }
            else if (sender != null && this.sorryBoard.CurrentPlayer.HasDrawnCard && this.sorryBoard.CurrentPlayer.HasSelectedValidPawn)
            {
                Pawn selectedPawn = new Pawn();
                Border space = sender as Border;
                string spaceName = space.Name;
                int spaceNumber = int.Parse(spaceName.Substring(11, spaceName.Length - 11));

                foreach (Player player in this.sorryBoard.Players)
                {
                    foreach (Pawn pawn in player.Pawns)
                    {
                        if (pawn.CurrentSpace == spaceNumber)
                        {
                            selectedPawn = pawn;
                        }
                    }
                }

                if (currentCard.MoveValue == 0)
                {
                    if (CanBeSorry(selectedPawn))
                    {
                        BumpPawn(selectedPawn.CurrentSpace);

                        // Reset the space the Pawn is in currently in
                        Border currentSpace = this.sorryBoard.Space[this.sorryBoard.CurrentPlayer.SelectedPawn.CurrentSpace].SpaceBorder;
                        currentSpace.Background = null;

                        // Move the Pawn to the new space
                        this.sorryBoard.CurrentPlayer.SelectedPawn.CurrentSpace = spaceNumber;
                        space.Background = this.sorryBoard.CurrentPlayer.Color;
                        this.sorryBoard.CurrentPlayer.SelectedPawn.IsInStart = false;
                        messageCenterTextBlock.Text = "Draw a card.";
                        EndTurn();
                    }
                    else
                    {
                        MessageBox.Show("Select a different pawn.");
                    }
                }
                else if (currentCard.MoveValue == 11 && this.sorryBoard.CurrentPlayer.CardChoice == true)
                {
                    if (CanBeSorry(selectedPawn))
                    {
                        int originalSpaceNumber = this.sorryBoard.CurrentPlayer.SelectedPawn.CurrentSpace;
                        int opponentSpaceNumber = selectedPawn.CurrentSpace;
                        Border originalSpace = this.sorryBoard.Space[originalSpaceNumber].SpaceBorder;
                        Border opponentSpace = this.sorryBoard.Space[opponentSpaceNumber].SpaceBorder;

                        originalSpace.Background = opponentSpace.Background;
                        this.sorryBoard.CurrentPlayer.SelectedPawn.CurrentSpace = opponentSpaceNumber;
                        opponentSpace.Background = this.sorryBoard.CurrentPlayer.Color;
                        selectedPawn.CurrentSpace = originalSpaceNumber;
                        messageCenterTextBlock.Text = "Draw a card.";
                        EndTurn();
                    }
                    else
                    {
                        MessageBox.Show("Select a different pawn");
                    }
                }
            }
            e.Handled = true;
        }

        private void DrawCardButton_Click(object sender, RoutedEventArgs e)
        {
            // Draw a card from the deck
            this.sorryBoard.CurrentPlayer.AdditionalTurn = false;
            int playerCard = this.sorryBoard.CurrentPlayer.DrawCard(this.sorryBoard.CardDeck);
            this.sorryBoard.CurrentPlayer.CurrentCardNumber = playerCard;
            this.sorryBoard.CurrentPlayer.HasDrawnCard = true;

            // The 2 card gives the player another turn whether that player has a valid move or not
            if (this.sorryBoard.CardDeck.Cards[playerCard].MoveValue == 2)
            {
                this.sorryBoard.CurrentPlayer.AdditionalTurn = true;
            }

            // Determine whether any pawn can be moved
            bool hasMoveablePawn = false;
            foreach (Pawn pawn in this.sorryBoard.CurrentPlayer.Pawns)
            {
                if (CanBeMoved(pawn))
                {
                    hasMoveablePawn = true;
                }
            }

            if (!hasMoveablePawn)
            {
                // The player can't move any of their pawns. Show which card was drawn and end the turn
                messageCenterTextBlock.Text = this.sorryBoard.CurrentPlayer.Name +
                                              this.sorryBoard.CardDeck.Cards[playerCard].CannotMove();
                EndTurn();
            }
            else
            {
                this.sorryBoard.CurrentPlayer.HasDrawnCard = true;
                // Show which card was drawn
                messageCenterTextBlock.Text = this.sorryBoard.CurrentPlayer.Name +
                                              this.sorryBoard.CardDeck.Cards[playerCard].CanMove();

                if (11 == sorryBoard.CardDeck.Cards[playerCard].MoveValue)
                {
                    // eleven properties
                    Window card11Window = new Card11Prompt();
                    if (!IsOpponentOnBoard())
                    {
                        Button swapButton = (Button)card11Window.FindName("swapButton");
                        swapButton.IsEnabled = false;
                    }
                    this.sorryBoard.CurrentPlayer.CardChoice = card11Window.ShowDialog();

                    if (this.sorryBoard.CurrentPlayer.CardChoice == false)
                    {
                        messageCenterTextBlock.Text = this.sorryBoard.CurrentPlayer.Name +
                                                      ", select a pawn to move eleven spaces";
                    }
                    else
                    {
                        messageCenterTextBlock.Text = this.sorryBoard.CurrentPlayer.Name +
                                                      ", select your pawn that you want to swap";
                    }
                }
                else if (10 == sorryBoard.CardDeck.Cards[playerCard].MoveValue)
                {
                    // ten properties
                    Window card10Window = new Card10Prompt();
                    this.sorryBoard.CurrentPlayer.CardChoice = card10Window.ShowDialog();

                    if (this.sorryBoard.CurrentPlayer.CardChoice == false)
                    {
                        messageCenterTextBlock.Text = this.sorryBoard.CurrentPlayer.Name +
                                                      ", select a pawn to move ten spaces";
                    }
                    else
                    {
                        messageCenterTextBlock.Text = this.sorryBoard.CurrentPlayer.Name +
                                                      ", select a pawn to move back one space";
                    }
                }
                else if (7 == sorryBoard.CardDeck.Cards[playerCard].MoveValue)
                {
                    // seven properties
                    Window card7Window = new Card7Prompt();
                    this.sorryBoard.CurrentPlayer.CardChoice = null;
                    if (HasOnePawn())
                    {
                        Button splitButton = (Button)card7Window.FindName("splitButton");
                        splitButton.IsEnabled = false;
                    }
                    this.sorryBoard.CurrentPlayer.CardChoice = card7Window.ShowDialog();

                    if (this.sorryBoard.CurrentPlayer.CardChoice == false)
                    {
                        messageCenterTextBlock.Text = this.sorryBoard.CurrentPlayer.Name +
                                                      ", select a pawn to move seven spaces";
                    }
                    else
                    {
                        bool? isChoiceMade;
                        do
                        {
                            Window card7Choices = new Card7Choices();
                            isChoiceMade = card7Choices.ShowDialog();
                        } while (isChoiceMade == false);
                        Card7Results card7Results = Card7Results.Instance;
                        this.sorryBoard.CurrentPlayer.Pawn1Split = card7Results.pawn1MoveValue;
                        this.sorryBoard.CurrentPlayer.Pawn2Split = card7Results.pawn2MoveValue;

                        messageCenterTextBlock.Text = this.sorryBoard.CurrentPlayer.Name +
                                                      ", select your first pawn to move " + this.sorryBoard.CurrentPlayer.Pawn1Split + " spaces";
                    }
                }

                // Disable the draw card button until the turn is over
                drawCardButton.IsEnabled = false;
            }
        }

        public bool HasOnePawn()
        {
            int count = 0;
            foreach (Pawn pawn in this.sorryBoard.CurrentPlayer.Pawns)
            {
                if (!pawn.IsInHome && !pawn.IsInStart)
                {
                    count++;
                }
            }

            return count == 1;
        }

        public void MovePawn(Pawn pawn, int moveValue, bool isSliding = false)
        {
            quitGameButton.IsEnabled = false;
            restartGameButton.IsEnabled = false;
            drawCardButton.IsEnabled = false;
            aboutButton.IsEnabled = false;
            IsClickDisabled = true;

            int moveValueRemaining;
            // check if moving backwards one
            if (this.sorryBoard.CardDeck.Cards[this.sorryBoard.CurrentPlayer.CurrentCardNumber].MoveValue == 10 && this.sorryBoard.CurrentPlayer.CardChoice == true && !isSliding)
            {
                moveValueRemaining = -1;
            }
            else
            {
                moveValueRemaining = moveValue;
            }
            int nextSpace;
            Brush nextSpaceColor = null;

            do
            {
                if (moveValueRemaining > 0)
                {
                    nextSpace = GetNextSpaceNumber(pawn, false);
                    moveValueRemaining--;
                }
                else
                {
                    nextSpace = GetNextSpaceNumber(pawn, true);
                    moveValueRemaining++;
                }

                if (nextSpaceColor == null)
                {
                    this.sorryBoard.Space[pawn.CurrentSpace].SpaceBorder.Background = null;
                }
                else
                {
                    this.sorryBoard.Space[pawn.CurrentSpace].SpaceBorder.Background = nextSpaceColor;
                }

                if (moveValueRemaining == 0)
                {
                    // The pawn's future space is occupied by an opponent. Bump the opponent's pawn back to start
                    BumpPawn(nextSpace);
                }

                nextSpaceColor = this.sorryBoard.Space[nextSpace].SpaceBorder.Background;
                pawn.CurrentSpace = nextSpace;
                this.sorryBoard.Space[nextSpace].SpaceBorder.Background = this.sorryBoard.CurrentPlayer.Color;
                SetPawnFlags(pawn);

                if (pawn.IsInHome)
                {
                    moveValueRemaining = 0;
                }

                Task task = Task.Factory.StartNew(() => Thread.Sleep(150));
                Task.WaitAll(new[] { task });
                DoEvents();
            } while (moveValueRemaining != 0);

            // Check if the pawn ended its movement on the start of a slide space
            Slide slide = this.sorryBoard.Space[pawn.CurrentSpace].Slide;
            if (slide != null)
            {
                // The pawn has landed on the start of a slide

                SolidColorBrush playerColor = this.sorryBoard.CurrentPlayer.Color;
                SolidColorBrush slideColor = slide.Color;
                string playerStringColor = this.sorryBoard.CurrentPlayer.Color.ToString();
                string slideStringColor = slide.Color.ToString();

                if (playerStringColor != slideStringColor)
                {
                    // Pawn has landed on the start of a different colored slide
                    for (int space = slide.SlideStart; space < slide.SlideEnd; space++)
                    {
                        if (this.sorryBoard.Space[space].SpaceBorder.Background != this.sorryBoard.CurrentPlayer.Color)
                        {
                            // Any opponent pawns in the slide get bumped back to start
                            BumpPawn(space);
                        }
                    }

                    // Move the pawn the distance of the slide
                    MovePawn(pawn, slide.SlideEnd - slide.SlideStart, true);
                }
            }

            pawn.IsInStart = false;
            quitGameButton.IsEnabled = true;
            restartGameButton.IsEnabled = true;
            aboutButton.IsEnabled = true;
            IsClickDisabled = false;
        }

        public void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));
        }

        public int GetNextSpaceNumber(Pawn pawn, bool isMovingBackwards)
        {
            if (!isMovingBackwards)
            {
                if (!pawn.IsInStart)
                {
                    if (pawn.IsInSafety)
                    {
                        if (pawn.CurrentSpace == this.sorryBoard.CurrentPlayer.SafetySpaces[5])
                        {
                            // The pawn is moving into home. Find the first empty home space
                            foreach (int homeSpace in this.sorryBoard.CurrentPlayer.HomeSpaces)
                            {
                                Border checkSpace = this.sorryBoard.Space[homeSpace].SpaceBorder;
                                if (checkSpace.Background == null)
                                {
                                    return homeSpace;
                                }
                            }
                        }
                        else
                        {
                            return pawn.CurrentSpace + 1;
                        }
                    }

                    if (pawn.CurrentSpace + 1 > MAINSPACECOUNT - 1)
                    {
                        // Pawn is moving to the first space
                        return 0;
                    }

                    if (pawn.CurrentSpace == this.sorryBoard.CurrentPlayer.LastSpace)
                    {
                        return this.sorryBoard.CurrentPlayer.SafetySpaces[1];
                    }

                    return pawn.CurrentSpace + 1;
                }
                else
                {
                    return this.sorryBoard.CurrentPlayer.FirstSpace;
                }
            }
            else
            {
                if (pawn.IsInSafety)
                {
                    if (pawn.CurrentSpace == this.sorryBoard.CurrentPlayer.SafetySpaces[1])
                    {
                        return this.sorryBoard.CurrentPlayer.LastSpace;
                    }
                    else
                    {
                        return pawn.CurrentSpace - 1;
                    }
                }

                if (pawn.CurrentSpace - 1 < 0)
                {
                    // Pawn has gone backwards to the last space
                    return MAINSPACECOUNT - 1;
                }

                return pawn.CurrentSpace - 1;
            }
        }

        public int ProjectedSpaceNumber(Pawn pawn)
        {
            // Determine where the player needs to move
            int projectedSpace = 0;
            int playerCard = this.sorryBoard.CurrentPlayer.CurrentCardNumber;
            projectedSpace = pawn.CurrentSpace + this.sorryBoard.CardDeck.Cards[playerCard].MoveValue;
            if (!pawn.IsInStart)
            {
                if (pawn.IsInSafety)
                {
                    foreach (KeyValuePair<int, int> space in this.sorryBoard.CurrentPlayer.SafetySpaces)
                    {
                        // Find which number safety space the pawn is on
                        if (space.Value == pawn.CurrentSpace)
                        {
                            if (projectedSpace > this.sorryBoard.CurrentPlayer.SafetySpaces[5])
                            {
                                // The pawn has moved enough to be put in home. Find the first empty home space
                                foreach (int homeSpace in this.sorryBoard.CurrentPlayer.HomeSpaces)
                                {
                                    Border checkSpace = this.sorryBoard.Space[homeSpace].SpaceBorder;
                                    if (checkSpace.Background == null)
                                    {
                                        return homeSpace;
                                    }
                                }
                            }

                            return projectedSpace;
                        }
                    }
                }
                // Calculate the space the player's Pawn should move to, given the Pawn's current space and the card drawn
                if (projectedSpace > MAINSPACECOUNT - 1)
                {
                    // Pawn has moved past the start of the board
                    projectedSpace = projectedSpace - MAINSPACECOUNT;
                }
                else if (projectedSpace < 0)
                {
                    // Pawn has gone backwards past the start of the board
                    projectedSpace = projectedSpace + MAINSPACECOUNT;
                }

                if (this.sorryBoard.CurrentPlayer.HomeStretch.Contains(pawn.CurrentSpace))
                {
                    // The pawn is in the home stretch (at least one card can move the pawn into safety or home)
                    if (projectedSpace > this.sorryBoard.CurrentPlayer.LastSpace)
                    {
                        // The pawn would move past their entry space into safety. Find how many spaces are left for it to move and move it that many spaces into safety
                        int moveRemaining = projectedSpace - this.sorryBoard.CurrentPlayer.LastSpace;
                        if (moveRemaining > 5)
                        {
                            // The pawn has moved enough to be put into home. Find the first empty home space
                            foreach (int homeSpace in this.sorryBoard.CurrentPlayer.HomeSpaces)
                            {
                                Border checkSpace = this.sorryBoard.Space[homeSpace].SpaceBorder;
                                if (checkSpace.Background == null)
                                {
                                    return homeSpace;
                                }
                            }
                        }
                        else
                        {
                            // The pawn is in the safety zone
                            return this.sorryBoard.CurrentPlayer.SafetySpaces[moveRemaining];
                        }
                    }
                }
            }
            else if (this.sorryBoard.CardDeck.Cards[playerCard].MoveValue == 1 || this.sorryBoard.CardDeck.Cards[playerCard].MoveValue == 2)
            {
                // Move the pawn out of start
                projectedSpace = this.sorryBoard.CurrentPlayer.FirstSpace;
            }

            // The space the pawn will be placed
            return projectedSpace;
        }

        private bool CanBeMoved(Pawn pawn)
        {
            if (pawn.IsInHome)
            {
                // A pawn in home can't be moved
                return false;
            }

            if (!pawn.IsInStart)
            {
                // Check if the space the pawn would be moved onto is a space with a friendly pawn
                Border projectedSpace = this.sorryBoard.Space[ProjectedSpaceNumber(pawn)].SpaceBorder;
                if (projectedSpace.Background == this.sorryBoard.CurrentPlayer.Color)
                {
                    // A pawn can't be moved to the same space as a friendly pawn
                    return false;
                }

                // A pawn can move on the main track
                return true;
            }
            else if (pawn.IsInStart &&
                     (this.sorryBoard.CardDeck.Cards[this.sorryBoard.CurrentPlayer.CurrentCardNumber].MoveValue == 1 ||
                      this.sorryBoard.CardDeck.Cards[this.sorryBoard.CurrentPlayer.CurrentCardNumber].MoveValue == 2) &&
                     this.sorryBoard.Space[this.sorryBoard.CurrentPlayer.FirstSpace].SpaceBorder.Background != this.sorryBoard.CurrentPlayer.Color)
            {
                // A pawn can only be moved out of start by the player drawing a 1 or 2, but not if the first space is occupied by a friendly pawn
                return true;
            }

            // A pawn can be moved out of start with a Sorry card
            if (this.sorryBoard.CardDeck.Cards[this.sorryBoard.CurrentPlayer.CurrentCardNumber].MoveValue == 0)
            {
                foreach (Pawn playerPawn in this.sorryBoard.CurrentPlayer.Pawns)
                {
                    if (playerPawn.IsInStart)
                    {
                        // But only if there's an opponent's pawn on the main track to swap it with
                        return IsOpponentOnBoard();
                    }
                }
            }

            // The pawn can't be moved
            return false;
        }

        private bool IsOpponentOnBoard()
        {
            foreach (Player player in this.sorryBoard.Players)
            {
                if (player.Color != this.sorryBoard.CurrentPlayer.Color)
                {
                    // You can only sorry an opponent's pawn
                    foreach (Pawn pawn in player.Pawns)
                    {
                        if (!pawn.IsInStart && !pawn.IsInHome && !pawn.IsInSafety)
                        {
                            // The opponent's pawn has to be on the main board track
                            return true;
                        }
                    }
                }
            }

            // Each opponent's pawns are all in start, home, or safe
            return false;
        }

        private bool CanBeSorry(Pawn pawn)
        {
            Border pawnSpace = this.sorryBoard.Space[pawn.CurrentSpace].SpaceBorder;
            return !pawn.IsInStart && !pawn.IsInHome && !pawn.IsInSafety && this.sorryBoard.CurrentPlayer.Color != pawnSpace.Background;
        }

        private bool CheckForWin()
        {
            // If all 4 of a player's pawns are home, they are the winner
            int homeCount = 0;
            foreach (Pawn pawn in this.sorryBoard.CurrentPlayer.Pawns)
            {
                if (pawn.IsInHome)
                {
                    homeCount++;
                }
            }
            return homeCount == 4;
        }

        private void EndTurn()
        {
            if (CheckForWin())
            {
                playerTurnLabel.Content = this.sorryBoard.CurrentPlayer.Name + " wins!";
                messageCenterTextBlock.Text = "";
                drawCardButton.IsEnabled = false;
                restartGameButton.IsEnabled = false;
                startGameButton.IsEnabled = true;
                MessageBox.Show(this.sorryBoard.CurrentPlayer.Name + " wins!");
                return;
            }

            // Set the next player as the current player
            this.sorryBoard.CurrentPlayer.HasDrawnCard = false;
            this.sorryBoard.CurrentPlayer.HasSelectedValidPawn = false;
            this.sorryBoard.CurrentPlayer.SelectedPawn = null;
            this.sorryBoard.CurrentPlayer.CardChoice = null;
            if (!sorryBoard.CurrentPlayer.AdditionalTurn)
            {
                switch (this.sorryBoard.CurrentPlayer.Name)
                {
                    case "Red":
                        this.sorryBoard.CurrentPlayer = this.sorryBoard.Players.FirstOrDefault(x => x.Name == "Blue");
                        break;
                    case "Blue":
                        this.sorryBoard.CurrentPlayer = this.sorryBoard.Players.FirstOrDefault(x => x.Name == "Green");
                        break;
                    case "Green":
                        this.sorryBoard.CurrentPlayer = this.sorryBoard.Players.FirstOrDefault(x => x.Name == "Yellow");
                        break;
                    case "Yellow":
                        this.sorryBoard.CurrentPlayer = this.sorryBoard.Players.FirstOrDefault(x => x.Name == "Red");
                        break;
                }
            }

            playerTurnLabel.Content = this.sorryBoard.CurrentPlayer.Name + "'s Turn";
            drawCardButton.IsEnabled = true;
        }

        private void BumpPawn(int space)
        {
            foreach (Player player in this.sorryBoard.Players)
            {
                if (player.Color == this.sorryBoard.Space[space].SpaceBorder.Background)
                {
                    foreach (Pawn pawn in player.Pawns)
                    {
                        if (pawn.CurrentSpace == space)
                        {
                            pawn.IsInStart = true;
                            foreach (int startSpace in player.StartSpaces)
                            {
                                Border checkSpace = this.sorryBoard.Space[startSpace].SpaceBorder;
                                if (checkSpace.Background == null)
                                {
                                    this.sorryBoard.Space[pawn.CurrentSpace].SpaceBorder.Background = null;
                                    pawn.CurrentSpace = startSpace;
                                    checkSpace.Background = player.Color;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            StartGameButton_Click(null, null);

            this.sorryBoard.Space[this.sorryBoard.Players[0].Pawns[0].CurrentSpace].SpaceBorder.Background = null;
            this.sorryBoard.Players[0].Pawns[0].CurrentSpace = 81;
            this.sorryBoard.Space[this.sorryBoard.Players[0].Pawns[0].CurrentSpace].SpaceBorder.Background = this.sorryBoard.Players[0].Color;
            this.sorryBoard.CurrentPlayer = this.sorryBoard.Players[0];
            SetPawnFlags(this.sorryBoard.Players[0].Pawns[0]);

            this.sorryBoard.Space[this.sorryBoard.Players[0].Pawns[1].CurrentSpace].SpaceBorder.Background = null;
            this.sorryBoard.Players[0].Pawns[1].CurrentSpace = 82;
            this.sorryBoard.Space[this.sorryBoard.Players[0].Pawns[1].CurrentSpace].SpaceBorder.Background = this.sorryBoard.Players[0].Color;
            this.sorryBoard.CurrentPlayer = this.sorryBoard.Players[0];
            SetPawnFlags(this.sorryBoard.Players[0].Pawns[1]);

            this.sorryBoard.Space[this.sorryBoard.Players[0].Pawns[2].CurrentSpace].SpaceBorder.Background = null;
            this.sorryBoard.Players[0].Pawns[2].CurrentSpace = 83;
            this.sorryBoard.Space[this.sorryBoard.Players[0].Pawns[2].CurrentSpace].SpaceBorder.Background = this.sorryBoard.Players[0].Color;
            this.sorryBoard.CurrentPlayer = this.sorryBoard.Players[0];
            SetPawnFlags(this.sorryBoard.Players[0].Pawns[2]);

            this.sorryBoard.Space[this.sorryBoard.Players[0].Pawns[3].CurrentSpace].SpaceBorder.Background = null;
            this.sorryBoard.Players[0].Pawns[3].CurrentSpace = 64;
            this.sorryBoard.Space[this.sorryBoard.Players[0].Pawns[3].CurrentSpace].SpaceBorder.Background = this.sorryBoard.Players[0].Color;
            this.sorryBoard.CurrentPlayer = this.sorryBoard.Players[0];
            SetPawnFlags(this.sorryBoard.Players[0].Pawns[3]);

            //this.sorryBoard.Space[this.sorryBoard.Players[0].Pawns[1].CurrentSpace].SpaceBorder.Background = null;
            //this.sorryBoard.Players[0].Pawns[1].CurrentSpace = 17;
            //this.sorryBoard.Space[17].SpaceBorder.Background = this.sorryBoard.Players[0].Color;
            //SetPawnFlags(this.sorryBoard.Players[0].Pawns[1]);
        }
    }
}
