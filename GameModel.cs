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
        public List<Brick> Bricks { get; set; }

        public GameModel()
        {
            Bricks = new List<Brick>();

            Ship = new Ship(148, 125, 5);
            Ship.Location = new Point(535, 600);

            Ball = new Ball(32);
            Ball.Location = new Point(535, 600);
            var w = 200;
            var h = 150;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {

                    var color = Color.Green;
                    var block = new Brick(w + 70 * i, h + 30 * j, 70, 30, color);
                    Bricks.Add(block);
                }
        }

        public void Move(Rectangle spaceRect, Turn turnRate)
        {
            var newX = (int)(Ship.Location.X + (int)turnRate * Ship.Velocity );
            Ship.Location = new Point(Math.Min(Math.Max(newX, 0), spaceRect.Width-Ship.Width/2), Ship.Location.Y);
           
        }
    }
}
