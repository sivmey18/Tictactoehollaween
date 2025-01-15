using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Threading;

namespace Tic_Tac_Toe
{
    public partial class MainWindow : Window
    {
        private string currentPlayer = "🎃";  // Current player
        private string[] gameBoard;           // Game board
        private bool isAiEnabled;             // AI enabled/disabled
        private string aiPlayer = "👻";       // AI symbol
        private Random random = new Random(); // Random for AI moves
        private int boardSize;                // Board size (3, 4, or 5)
        private DispatcherTimer resultTimer;  // Timer for showing the result

        // Images for players
        private BitmapImage pumpkinImage = new BitmapImage(new Uri("Image/pumpkin.png", UriKind.RelativeOrAbsolute));
        private BitmapImage monsterImage = new BitmapImage(new Uri("Image/monster.png", UriKind.RelativeOrAbsolute));

        public MainWindow(int boardSize, bool isAiEnabled)
        {
            InitializeComponent();
            this.boardSize = boardSize;
            this.isAiEnabled = isAiEnabled;
            InitializeGameBoard();
            ResetGame();

            // Initialize the timer
            resultTimer = new DispatcherTimer();
            resultTimer.Interval = TimeSpan.FromSeconds(1); // 3 seconds delay
            resultTimer.Tick += ResultTimer_Tick;
        }

        private void InitializeGameBoard()
        {
            // Create the game board based on size
            gameBoard = new string[boardSize * boardSize];

            // Create the Grid for the game
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();
            GameGrid.Children.Clear();

            for (int i = 0; i < boardSize; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    Button button = new Button
                    {
                        Name = $"btn{row}{col}",
                        FontSize = 20,
                        Width = 100,
                        Height = 100,
                        Margin = new Thickness(5),
                        Background = Brushes.Orange,
                        RenderTransform = new TranslateTransform() // Add RenderTransform for animation
                    };
                    button.Click += Button_Click;
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    GameGrid.Children.Add(button);
                }
            }
        }

        public void ResetGame()
        {
            // Reset the game board
            gameBoard = new string[boardSize * boardSize];
            foreach (Button btn in GameGrid.Children.OfType<Button>())
            {
                btn.Content = "";
            }

            currentPlayer = "🎃";
            UpdateStatusText();

            if (isAiEnabled && currentPlayer == aiPlayer)
            {
                AiMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button buttonClicked = (Button)sender;
            int buttonIndex = GetButtonIndex(buttonClicked.Name);

            if (gameBoard[buttonIndex] != null)
                return;

            if (currentPlayer == "🎃")
            {
                SetButtonImage(buttonClicked, pumpkinImage);
            }
            else
            {
                SetButtonImage(buttonClicked, monsterImage);
            }
            gameBoard[buttonIndex] = currentPlayer;

            int[] winningButtons = GetWinningButtons(currentPlayer);
            if (winningButtons != null)
            {
                AnimateWinningButtons(winningButtons); // Animate only the winning buttons
                resultTimer.Start(); // Start the timer
                return;
            }

            if (Array.Exists(gameBoard, element => element == null) == false)
            {
                ResultWindow.ShowResult("Draw", this);
                return;
            }

            SwitchPlayer();

            if (isAiEnabled && currentPlayer == aiPlayer)
            {
                AiMove();
            }
        }

        private int[] GetWinningButtons(string player)
        {
            // Check rows
            for (int row = 0; row < boardSize; row++)
            {
                bool isWinner = true;
                for (int col = 0; col < boardSize; col++)
                {
                    if (gameBoard[row * boardSize + col] != player)
                    {
                        isWinner = false;
                        break;
                    }
                }
                if (isWinner)
                {
                    int[] winningButtons = new int[boardSize];
                    for (int col = 0; col < boardSize; col++)
                    {
                        winningButtons[col] = row * boardSize + col;
                    }
                    return winningButtons;
                }
            }

            // Check columns
            for (int col = 0; col < boardSize; col++)
            {
                bool isWinner = true;
                for (int row = 0; row < boardSize; row++)
                {
                    if (gameBoard[row * boardSize + col] != player)
                    {
                        isWinner = false;
                        break;
                    }
                }
                if (isWinner)
                {
                    int[] winningButtons = new int[boardSize];
                    for (int row = 0; row < boardSize; row++)
                    {
                        winningButtons[row] = row * boardSize + col;
                    }
                    return winningButtons;
                }
            }

            // Check diagonal (top-left to bottom-right)
            bool isDiagonalWinner = true;
            for (int i = 0; i < boardSize; i++)
            {
                if (gameBoard[i * boardSize + i] != player)
                {
                    isDiagonalWinner = false;
                    break;
                }
            }
            if (isDiagonalWinner)
            {
                int[] winningButtons = new int[boardSize];
                for (int i = 0; i < boardSize; i++)
                {
                    winningButtons[i] = i * boardSize + i;
                }
                return winningButtons;
            }

            // Check diagonal (top-right to bottom-left)
            isDiagonalWinner = true;
            for (int i = 0; i < boardSize; i++)
            {
                if (gameBoard[i * boardSize + (boardSize - 1 - i)] != player)
                {
                    isDiagonalWinner = false;
                    break;
                }
            }
            if (isDiagonalWinner)
            {
                int[] winningButtons = new int[boardSize];
                for (int i = 0; i < boardSize; i++)
                {
                    winningButtons[i] = i * boardSize + (boardSize - 1 - i);
                }
                return winningButtons;
            }

            return null; // No winner
        }

        private void AnimateWinningButtons(int[] winningButtons)
        {
            if (winningButtons == null)
                return;

            foreach (int index in winningButtons)
            {
                Button button = GameGrid.Children
                    .OfType<Button>()
                    .FirstOrDefault(btn => GetButtonIndex(btn.Name) == index);

                if (button != null)
                {
                    // Apply the jump animation
                    Storyboard jumpAnimation = (Storyboard)FindResource("JumpAnimation");
                    jumpAnimation.Begin(button);
                }
            }
        }

        private void SwitchPlayer()
        {
            currentPlayer = (currentPlayer == "🎃") ? "👻" : "🎃";
            UpdateStatusText();
        }

        private void UpdateStatusText()
        {
            StatusText.Text = $"{currentPlayer}'s Turn";
        }

        private void AiMove()
        {
            var availableMoves = gameBoard
                .Select((value, index) => new { value, index })
                .Where(x => x.value == null)
                .Select(x => x.index)
                .ToList();
            int randomMove = availableMoves[random.Next(availableMoves.Count)];
            gameBoard[randomMove] = aiPlayer;
            UpdateButton(randomMove, aiPlayer);

            int[] winningButtons = GetWinningButtons(aiPlayer);
            if (winningButtons != null)
            {
                AnimateWinningButtons(winningButtons); // Animate only the winning buttons
                resultTimer.Start(); // Start the timer
                return;
            }

            if (Array.Exists(gameBoard, element => element == null) == false)
            {
                ResultWindow.ShowResult("Draw", this);
                return;
            }

            SwitchPlayer();
        }

        private void SetButtonImage(Button button, BitmapImage image)
        {
            Image img = new Image { Source = image, Width = 70, Height = 70 };
            button.Content = img;
        }

        private void UpdateButton(int index, string player)
        {
            Button button = GameGrid.Children
                .OfType<Button>()
                .FirstOrDefault(btn => GetButtonIndex(btn.Name) == index);

            if (button != null)
            {
                SetButtonImage(button, player == "🎃" ? pumpkinImage : monsterImage);
            }
        }

        private int GetButtonIndex(string buttonName)
        {
            int row = int.Parse(buttonName[3].ToString());
            int col = int.Parse(buttonName[4].ToString());
            return row * boardSize + col;
        }

        private void ResultTimer_Tick(object sender, EventArgs e)
        {
            resultTimer.Stop(); // Stop the timer
            ResultWindow.ShowResult(currentPlayer, this); // Show the result window
            ResetGame(); // Reset the game after showing the result
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            BoardSizeSelection boardSizeSelection = new BoardSizeSelection();
            boardSizeSelection.Show();
            this.Close();
        }

        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                // បង្កើតផ្ទាំង TitleScreen ថ្មី
                TitleScreen titleScreen = new TitleScreen();

                // បើកផ្ទាំង TitleScreen
                titleScreen.Show();

                // លាក់ផ្ទាំងបច្ចុប្បន្ន (ជំនួសអោយបិទ)
                this.Hide();
            }
            catch (Exception ex)
            {
                // បង្ហាញសារកំហុស បើមានបញ្ហា
                MessageBox.Show("មានបញ្ហាក្នុងការបើកផ្ទាំង៖ " + ex.Message, "កំហុស", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}