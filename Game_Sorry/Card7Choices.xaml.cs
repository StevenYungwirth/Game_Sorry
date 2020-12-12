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
using System.Windows.Shapes;
using Window = System.Windows.Window;

namespace Game_Sorry
{
    /// <summary>
    /// Interaction logic for Card7Choices.xaml
    /// </summary>
    public partial class Card7Choices : Window
    {
        public int Pawn1MoveValue
        {
            get { return pawn1MoveValue; }
        }

        public int Pawn2MoveValue
        {
            get { return pawn2MoveValue; }
        }

        private int pawn1MoveValue = 6;
        private int pawn2MoveValue = 1;

        public Card7Choices()
        {
            InitializeComponent();
        }

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            pawn1MoveValue++;
            pawn2MoveValue--;
            pawn1Label.Content = Pawn1MoveValue.ToString();
            pawn2Label.Content = Pawn2MoveValue.ToString();

            buttonUp.IsEnabled = Pawn1MoveValue != 6;
            buttonDown.IsEnabled = true;
        }

        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            pawn1MoveValue--;
            pawn2MoveValue++;
            pawn1Label.Content = Pawn1MoveValue.ToString();
            pawn2Label.Content = Pawn2MoveValue.ToString();

            buttonDown.IsEnabled = Pawn1MoveValue != 1;
            buttonUp.IsEnabled = true;
        }

        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Card7Results card7Results = Card7Results.Instance;
            card7Results.pawn1MoveValue = pawn1MoveValue;
            card7Results.pawn2MoveValue = pawn2MoveValue;
            this.Close();
        }
    }
}
