using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class GameElement
    {

    }

    public class Ship : GameElement
    {
        public Rectangle Frame { get; set; }
        public double Velocity { get; set; }

        public Ship(int x, int y, int width, int height, int velocity)
        {
            Frame = new Rectangle(x, y, width, height);
            Velocity = velocity;
        }
    }

    public class Ball : GameElement
    {
        public Rectangle Frame { get; set; }
        public double Velocity { get; set; }
        public double Direction { get; set; }

        public Ball(int x, int y, int radius, double velocity, double direction)
        {
            Frame = new Rectangle(x, y, radius, radius);
            Velocity = velocity;
            Direction = direction;
        }
    }

    public class Brick : GameElement
    {
        public Rectangle Block { get; set; }
        public Color Color { get; set; }

        public Brick(int x, int y, int width, int height, Color color)
        {
            Block = new Rectangle(x, y, width, height);
            Color = color;
        }
    }

    public class Bonus : GameElement
    {
        public string Effect { get; private set; }
        public double Duration { get; private set; }
        public GameElement Target { get; private set; }

        public Bonus(string effect, double duration, GameElement target)
        {
            Effect = effect;
            Duration = duration;
            Target = target;
        }
    }
}
