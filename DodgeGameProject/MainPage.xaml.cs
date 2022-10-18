using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace DodgeGameProject
{
    public sealed partial class MainPage : Page
    {
        GameBoard gb;
        User userPiece;
        Enemy[] enemies;
        readonly int width = (int)ApplicationView.GetForCurrentView().VisibleBounds.Width; //boundaries of the window
        readonly int height = (int)ApplicationView.GetForCurrentView().VisibleBounds.Height;
        readonly DispatcherTimer timer;

        bool isFirstGame = true; //checks if the game is the first one so the pause button wouldn't make an error
        bool isPaused = false; //checks if the game is paused or not
        CommandBar cmb;
        AppBarButton btnStartGame;
        AppBarButton btnPauseGame;

        public MainPage()
        {
            this.InitializeComponent();

            CreateNewBoard();
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 200)
            };
            timer.Tick += Timer_Tick;
            CreateCommandBar();
        }

        public void CreateNewBoard()
        {
            gb = new GameBoard(width, height);
            userPiece = new User(gb.User.Left, gb.User.Top);

            enemies = new Enemy[gb.Enemy_Size];
            for (int i = 0; i < gb.Enemy_Size; i++)
                enemies[i] = gb.Enemies[i];
        }

        private void Timer_Tick(object sender, object e)
        {
            /* This function occures every certain amount of time(which defined in the interval) */
            if (gb.UserCollision(userPiece, enemies, canvasBoard))
            {
                timer.Stop();
                btnPauseGame.IsEnabled = false;
                Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown; //stops the user from moving
            }
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Chase(userPiece, canvasBoard);
                gb.EnemyCollision(enemies, canvasBoard);
            }
            if (gb.Win())
            {
                timer.Stop();
                btnPauseGame.IsEnabled = false;
                Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown; //stops the user from moving
            }
        }

        public void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs e)
        {
            switch (e.VirtualKey)
            {
                case VirtualKey.Down:
                    userPiece.MoveDown(canvasBoard);
                    break;

                case VirtualKey.Up:
                    userPiece.MoveUp();
                    break;

                case VirtualKey.Right:
                    userPiece.MoveRight(canvasBoard);
                    break;

                case VirtualKey.Left:
                    userPiece.MoveLeft();
                    break;
            }
        }

        public Rectangle CreateRectangle(Player p)
        {
            p.CurrentPlayer = new Rectangle
            {
                Width = p.Width,
                Height = p.Height
            };
            if (p is User)
                p.CurrentPlayer.Fill = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("ms-appx:///Images/earth.png"))
                };
            Canvas.SetLeft(p.CurrentPlayer, p.Left);
            Canvas.SetTop(p.CurrentPlayer, p.Top);
            canvasBoard.Children.Add(p.CurrentPlayer);
            return p.CurrentPlayer;
        }

        private void CreateCommandBar()
        {
            cmb = new CommandBar
            {
                Width = 200,
                Height = 70
            };
            gridBoard.Children.Add(cmb);
            cmb.Margin = new Thickness(0, height - 40, 0, 0);
            CreateAppBarButtons();
        }

        private void CreateAppBarButtons()
        {
            /*Creates the appBarButtons, naming and labeling them, putting an image on them(with symbol icon),
             *adding tooltips, adding the buttons to the command bar and applying their click's methods.
             */
            btnStartGame = new AppBarButton
            {
                Name = "btnStartGame",
                Label = "Start New Game",
                Icon = new SymbolIcon(Symbol.GoToStart)
            };
            ToolTip ttStart = new ToolTip
            {
                Content = "Start new game"
            };
            ToolTipService.SetToolTip(btnStartGame, ttStart);

            btnPauseGame = new AppBarButton
            {
                Name = "btnPauseGame",
                Label = "Pause",
                Icon = new SymbolIcon(Symbol.Pause)
            };
            ToolTip ttPause = new ToolTip
            {
                Content = "Pause"
            };
            ToolTipService.SetToolTip(btnPauseGame, ttPause);

            cmb.PrimaryCommands.Add(btnStartGame);
            cmb.PrimaryCommands.Add(btnPauseGame);

            btnStartGame.Click += btnStartGame_Click;
            btnPauseGame.Click += btnPauseGame_Click;
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            isFirstGame = false;
            isPaused = false;
            btnPauseGame.IsEnabled = true;

            canvasBoard.Children.Clear();
            CreateNewBoard();
            timer.Start();
            userPiece.CurrentPlayer = CreateRectangle(userPiece);
            userPiece.CurrentPlayer.RadiusX = 50; // round the corners of the user's rectangle
            userPiece.CurrentPlayer.RadiusY = 50;

            for (int i = 0; i < gb.Enemy_Size; i++)
                enemies[i].CurrentPlayer = CreateRectangle(enemies[i]);

            Timer_Tick(sender, e);
            btnPauseGame.Icon = new SymbolIcon(Symbol.Pause);

            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown; /*every time the start button 
            *is clicked, the speed of the user is being increased by the step-move field(which is 
            *determined in the Player class) due to the '+='. To solve it for now, I wrote this code line
            *to decrease the speed and then bring it back to the original.
            */
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown; //releases the moving
        }

        private void btnPauseGame_Click(object sender, RoutedEventArgs e)
        {
            ToolTip ttPause = new ToolTip();
            if (isPaused)
            {
                timer.Start();
                isPaused = false;
                Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown; //releases the moving
                btnPauseGame.Icon = new SymbolIcon(Symbol.Pause);
                btnPauseGame.Label = "Pause";
                ttPause.Content = "Pause";
            }
            else
            {
                if (!isFirstGame)
                {
                    timer.Stop();
                    isPaused = true;
                    Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown; //stops the user from moving
                    btnPauseGame.Icon = new SymbolIcon(Symbol.Play);
                    btnPauseGame.Label = "Resume";
                    ttPause.Content = "Resume";
                }
            }
            ToolTipService.SetToolTip(btnPauseGame, ttPause);
        }
    }
}
