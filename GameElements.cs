using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game
{
    public class GameElement
    {
        public Rectangle Frame { get; set; }
        public double Velocity { get; set; }
        public double Direction { get; set; }

        public Point Location => Frame.Location;

        public Image Img { get; set; }

        public GameElement(int x, int y, int width, int height, double velocity, double direction)
        {
            Frame = new Rectangle(x, y, width, height);
            Velocity = velocity;
            Direction = direction;
            if (!(this is Ship))                                                 
                Img = Image.FromFile(string.Format(@"images\{0}.png", GetType().Name));
        }

        public void Move()
        {
            
            var newX = (int)(Frame.X + Math.Cos(Direction) * Velocity);
            var newY = (int)(Frame.Y + Math.Sin(Direction) * Velocity);
            Frame = new Rectangle(newX, newY, Frame.Width, Frame.Height);
        }


    }

    public class Ship : GameElement
    {
        public int Bullets { get; set; }

        public Ship(int x, int y) 
            : base(x, y, Settings.ShipSize.Width, Settings.ShipSize.Height, 7, 0)
        {
            Img = Image.FromFile(@"images\ship.gif");
        }

        public void Move(Rectangle window, Turn turnRate)
        {
            var newX = (int)(Frame.X + (int)turnRate * Velocity);
            Frame = new Rectangle(Math.Min(Math.Max(newX, 0), window.Width - Frame.Width), Frame.Y,
                Frame.Width, Frame.Height);
        }

        public void ResizeImg()
        {
            Img = new Bitmap(Img, Frame.Width, Frame.Height);
        }

    }

    public class Ball : GameElement
    {
        public BallState State { get; set; }

        public Ball(int x, int y) 
            : base(x, y, Settings.BallSize.Width, Settings.BallSize.Height, 6, -Math.PI/4)
        {
            State = BallState.Caught;
        }

        public void StickWith(int dx)
        {
            Frame = new Rectangle(Frame.X + dx, Frame.Y, Frame.Width, Frame.Height);
        }
    }

    public class Brick : GameElement
    {
        public string Color { get; set; }

        public Brick(int x, int y) : base(x, y, Settings.BrickSize.Width, Settings.BrickSize.Height, 0, 0)
        {

        }

    }

    public class Bullet : GameElement
    {
        public Bullet(int x, int y) : base(x, y, 2, 4, 7, -Math.PI/2)
        {

        }
    }

    public class Level
    {
        public int Lvl { get; private set; }
        public HashSet<Brick> Blocks { get; private set; }

        public Level(int level, HashSet<Brick> blocks)
        {
            Lvl = level;
            Blocks = blocks;
        }
    }
}
