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
using System.Windows.Media.Animation;

namespace Tic_Tac_Toe
{
    public partial class BoardSizeSelection : Window
    {
        public BoardSizeSelection()
        {
            InitializeComponent();
        }

        private void BoardSize3x3_Click(object sender, RoutedEventArgs e)
        {
            // Start the game with a 3x3 board
            StartGame(3);
        }

        private void BoardSize4x4_Click(object sender, RoutedEventArgs e)
        {
            // Start the game with a 4x4 board
            StartGame(4);
        }

        private void BoardSize5x5_Click(object sender, RoutedEventArgs e)
        {
            // Start the game with a 5x5 board
            StartGame(5);
        }

        private void StartGame(int boardSize)
        {
            // Open the TitleScreen window with the selected board size
            TitleScreen titleScreen = new TitleScreen(boardSize);
            titleScreen.Show();
            this.Close();
        }
    }
}