using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DodgeGameProject
{
    internal class GameBoard
    {
        private readonly int width;
        private readonly int height;
        private const int ENEMY_SIZE = 10;

        private readonly Enemy[] enemies;
        private readonly User user;
        private readonly Random r;

        public GameBoard(int width, int height)
        {
            this.width = width;
            this.height = height;
            r = new Random();

            this.user = new User(this.width / 2 - 25, this.height / 2 - 25);

            List<int> leftPositions = EnemyPositions(user.Left, this.width);
            List<int> topPositions = EnemyPositions(user.Top, this.height);
            this.enemies = new Enemy[ENEMY_SIZE];
            for (int i = 0; i < this.enemies.Length; i++)
                this.enemies[i] = new Enemy(leftPositions[i], topPositions[i]);
        }

        public Enemy[] Enemies { get { return enemies; } }
        public User User { get { return user; } }

        public int Enemy_Size { get { return ENEMY_SIZE; } }

        public List<int> EnemyPositions(int location, int screenSizeDirection)
        {
            /*this method creates a list of random positions for the enemies.
             *If the position is too close to the user(that probably gonna cause for
             *a quick losing), the method would generates a new random position. 
             */
            Random rnd = new Random();
            int num;
            var positions = new List<int>();
            for (int i = 0; i < ENEMY_SIZE; i++)
            {
                do
                {
                    num = rnd.Next(30, screenSizeDirection - 30);
                }
                while (!(num < location - user.Width * 2 || num > location + user.Width * 2));
                positions.Add(num);
            }
            return positions;
        }


        public bool UserCollision(User userPlayer, Player[] others, Canvas c)
        {
            /* Collision between the user and one of the enemies */
            foreach (Player other in others)
            {
                if ((userPlayer.Top >= other.Top - userPlayer.Width && userPlayer.Top <= other.Top + other.Width) &&
                    (userPlayer.Left >= other.Left - userPlayer.Width && userPlayer.Left <= other.Left + other.Width))
                {
                    ShowContentDialog("ms-appx:///Images/sad.png", "You've got busted", "OK");
                    return true;
                }
            }
            return false;
        }
        public void EnemyCollision(Enemy[] others, Canvas c)
        {
            /* Collision between 2 enemies */
            for (int i = 0; i < others.Length; i++)
            {
                for (int j = 0; j < others.Length; j++)
                    if (i != j && others[j].IsAlive && others[i].Top > others[j].Top - others[i].Width && others[i].Top < others[j].Top + others[i].Width
                        && others[i].Left > others[j].Left - others[i].Width && others[i].Left < others[j].Left + others[i].Width)
                    {
                        c.Children.Remove(others[i].CurrentPlayer);
                        others[i].IsAlive = false;
                    }
            }
        }
        public bool Win()
        {
            int count = 0;
            foreach (Enemy currentEnemy in enemies)
            {
                if (!currentEnemy.IsAlive)
                    count++;
            }
            if (count == ENEMY_SIZE - 1)
            {
                ShowContentDialog("ms-appx:///Images/trophy.png", "You Win!", "OK");
                return true;
            }
            return false;
        }

        public async void ShowContentDialog(string path, string title, string btnText)
        {
            /* Method for showing a content dialog when winning/losing */
            var contentDialog = new ContentDialog();
            contentDialog.Title = title;
            contentDialog.PrimaryButtonText = btnText;
            var panel = new StackPanel();
            panel.Children.Add(new Image { Source = new BitmapImage(new Uri(path)), Width = 300, Height = 300 });
            contentDialog.Content = panel;
            await contentDialog.ShowAsync();
        }
    }
}
