using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class GameModel
    {
        public Ship Ship { get; set; }
        public Ball Ball { get; set; }
        public HashSet<Brick> Bricks { get; set; }

        public GameModel()
        {
            Bricks = new HashSet<Brick>();

            Ship = new Ship(535, 670, 152, 56, 10);

            Ball = new Ball(613, 700 - 32 - Ship.Frame.Height, 32, 2, -Math.PI / 4);
            var w = 185;
            var h = 50;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {

                    var color = Color.Green;
                    var block = new Brick(w + 100 * i, h + 30 * j, 100, 30, color);
                    Bricks.Add(block);
                }
        }

        public void Move(Rectangle spaceRect, Turn turnRate)
        {
            var newX = (int)(Ship.Frame.X + (int)turnRate * Ship.Velocity);
            Ship.Frame = new Rectangle(KeepInForm(newX, spaceRect.Width - Ship.Frame.Width), Ship.Frame.Y,
                Ship.Frame.Width, Ship.Frame.Height);
            var ballX = (int)(Ball.Frame.X + Math.Cos(Ball.Direction) * Ball.Velocity);
            var ballY = (int)(Ball.Frame.Y + Math.Sin(Ball.Direction) * Ball.Velocity);
            Ball.Frame = new Rectangle(ballX, ballY, Ball.Frame.Width, Ball.Frame.Height);
            var toRemove = Bricks.Where(x => x.Block.IntersectsWith(Ball.Frame)).ToList();
            if (toRemove.Count != 0)
            {
                Ball.Direction += Math.PI / 2;
                Bricks.ExceptWith(toRemove);
            }
            if (Ball.Frame.IntersectsWith(Ship.Frame))
                Ball.Direction += Math.PI/2;
            if (Ball.Frame.Top <= spaceRect.Top || Ball.Frame.Right >= spaceRect.Right
                || Ball.Frame.Left <= spaceRect.Left || Ball.Frame.Bottom >= spaceRect.Bottom)
                Ball.Direction += Math.PI/2;
        }

        public static int KeepInForm(int value, int edge)
        {
            return Math.Min(Math.Max(value, 0), edge);
        }
    }
}
