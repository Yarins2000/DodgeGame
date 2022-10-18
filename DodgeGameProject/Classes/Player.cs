using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace DodgeGameProject
{
    public class Player
    {
        protected int left, top, width, height;
        protected int stepMove = 10;
        protected Rectangle currentPlayer;

        public Player(int left, int top, int width, int height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

        public int Left { get { return left; } }
        public int Top { get { return top; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public Rectangle CurrentPlayer { get { return currentPlayer; } set { currentPlayer = value; } }

        //The methods for moving the players
        public void MoveLeft()
        {
            if (Canvas.GetLeft(currentPlayer) <= stepMove)
                return;
            Canvas.SetLeft(currentPlayer, Canvas.GetLeft(currentPlayer) - stepMove);
            Canvas.SetTop(currentPlayer, Canvas.GetTop(currentPlayer));
            this.left = (int)Canvas.GetLeft(CurrentPlayer);
        }
        public void MoveRight(Canvas c)
        {
            if (Canvas.GetLeft(currentPlayer) + stepMove + currentPlayer.Width > c.ActualWidth)
                return;
            Canvas.SetLeft(currentPlayer, Canvas.GetLeft(currentPlayer) + stepMove);
            Canvas.SetTop(currentPlayer, Canvas.GetTop(currentPlayer));
            this.left = (int)Canvas.GetLeft(CurrentPlayer);
        }
        public void MoveUp()
        {
            if (Canvas.GetTop(currentPlayer) <= stepMove)
                return;
            Canvas.SetLeft(currentPlayer, Canvas.GetLeft(currentPlayer));
            Canvas.SetTop(currentPlayer, Canvas.GetTop(currentPlayer) - stepMove);
            this.top = (int)Canvas.GetTop(CurrentPlayer);
        }
        public void MoveDown(Canvas c)
        {
            if (Canvas.GetTop(currentPlayer) + stepMove + currentPlayer.Width > c.ActualHeight)
                return;
            Canvas.SetLeft(currentPlayer, Canvas.GetLeft(currentPlayer));
            Canvas.SetTop(currentPlayer, Canvas.GetTop(currentPlayer) + stepMove);
            this.top = (int)Canvas.GetTop(CurrentPlayer);
        }
    }
}
