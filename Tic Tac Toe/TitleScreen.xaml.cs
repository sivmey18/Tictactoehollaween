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
    public partial class TitleScreen : Window
    {
        private int boardSize;

        public TitleScreen(int boardSize)
        {
            InitializeComponent();
            this.boardSize = boardSize;
        }

        public TitleScreen()
        {
            InitializeComponent();
            this.boardSize = 3; // កំណត់តម្លៃដើម 3x3
        }

        private void PersonVsPerson_Click(object sender, RoutedEventArgs e)
        {
            MainWindow gameWindow = new MainWindow(boardSize, false);  // មនុស្ស vs មនុស្ស
            gameWindow.Show();
            this.Close();
        }

        private void PersonVsAI_Click(object sender, RoutedEventArgs e)
        {
            MainWindow gameWindow = new MainWindow(boardSize, true);  // មនុស្ស vs AI
            gameWindow.Show();
            this.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            BoardSizeSelection boardSizeSelection = new BoardSizeSelection();
            boardSizeSelection.Show();
            this.Close();
        }
    }
}