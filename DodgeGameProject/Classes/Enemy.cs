using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DodgeGameProject
{
    public class Enemy : Player
    {
        private bool isAlive = true;
        public Enemy(int left, int top) : base(left, top, 30, 30) { }


        public void Chase(User userPlayer, Canvas c)
        {
            /* The method for chasing after the user */
            double enemyLeft = Canvas.GetLeft(this.currentPlayer);
            double enemyTop = Canvas.GetTop(this.currentPlayer);
            double userLeft = Canvas.GetLeft(userPlayer.CurrentPlayer);
            double userTop = Canvas.GetTop(userPlayer.CurrentPlayer);
            if (enemyLeft < userLeft)
            {
                MoveRight(c);
                if (enemyTop < userTop)
                    this.currentPlayer.Fill = ImageByLocation("ms-appx:///Images/comet/downRight.png");
                else
                    this.currentPlayer.Fill = ImageByLocation("ms-appx:///Images/comet/upRight.png");
            }
            else if (enemyLeft > userLeft)
            {
                MoveLeft();
                if (enemyTop < userTop)
                    this.currentPlayer.Fill = ImageByLocation("ms-appx:///Images/comet/downLeft.png");
                else
                    this.currentPlayer.Fill = ImageByLocation("ms-appx:///Images/comet/upLeft.png");
            }
            if (enemyTop < userTop)
            {
                MoveDown(c);
                if (enemyLeft < userLeft)
                    this.currentPlayer.Fill = ImageByLocation("ms-appx:///Images/comet/downRight.png");
                else
                    this.currentPlayer.Fill = ImageByLocation("ms-appx:///Images/comet/downLeft.png");
            }
            else if (enemyTop > userTop)
            {
                MoveUp();
                if (enemyLeft < userLeft)
                    this.currentPlayer.Fill = ImageByLocation("ms-appx:///Images/comet/upRight.png");
                else
                    this.currentPlayer.Fill = ImageByLocation("ms-appx:///Images/comet/upLeft.png");
            }
        }

        public ImageBrush ImageByLocation(string path)
        {
            var img = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(path))
            };
            return img;
        }

        public bool IsAlive { get { return this.isAlive; } set { this.isAlive = value; } }
    }
}
