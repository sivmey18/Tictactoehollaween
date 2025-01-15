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
    public partial class ResultWindow : Window
    {
        private MainWindow mainWindow;

        public ResultWindow(string result, MainWindow mainWindowRef)
        {
            InitializeComponent();
            mainWindow = mainWindowRef;

            // Set result text based on the game outcome
            if (result == "Draw")
            {
                ResultText.Text = "🎃 It's a Draw! 🎃";
            }
            else
            {
                ResultText.Text = $" 🦇Player : {result} 🎉🎉";
            }
        }

        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();  // Hide the result window
            mainWindow.ResetGame();  // Call ResetGame in MainWindow
            mainWindow.Show();  // Show the main game window again
            this.Close();  // Close the result window
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            // Create a new BoardSizeSelection window
            BoardSizeSelection boardSizeSelection = new BoardSizeSelection();

            // Show the BoardSizeSelection window
            boardSizeSelection.Show();

            // Close the result window
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Shut down the application
            Application.Current.Shutdown();
        }

        public static void ShowResult(string result, MainWindow mainWindow)
        {
            // Create and show the result window
            ResultWindow resultWindow = new ResultWindow(result, mainWindow);
            resultWindow.ShowDialog();  // Display the result window as a modal dialog
        }
    }
}