using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class Bonus : GameElement
    {
        public Bonus(int x, int y, int width, int height) : base(x, y, width, height, 6, Math.PI/2)
        {

        }

        public abstract void Act(GameModel el);
    }

    public class DecreaseBonus : Bonus
    {
        public DecreaseBonus(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }

        public override void Act(GameModel model)
        {
            Ship ship = model.Ship;
            if(ship.Frame.Width >57)
            ship.Frame = new Rectangle(ship.Frame.X + ship.Frame.Width / 2, ship.Frame.Y, (int)(ship.Frame.Width / 1.5), ship.Frame.Height);
            ship.ResizeImg();
        }
    }

    public class ExpandBonus : Bonus

    {
        public ExpandBonus(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }

        public override void Act(GameModel model)
        {
            Ship ship = model.Ship;
            if (ship.Frame.Width <= 630)
                ship.Frame = new Rectangle(ship.Frame.X - ship.Frame.Width / 2, ship.Frame.Y, (int)(ship.Frame.Width * 1.5), ship.Frame.Height);
                ship.ResizeImg();
        }
    }

    public class BulletBonus : Bonus
    {
        public BulletBonus(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }

        public override void Act(GameModel model)
        {
            Ship ship = model.Ship;
            ship.Bullets += 8;
        }
    }

    public class LifePlus : Bonus
    {
        public LifePlus(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }

        public override void Act(GameModel model)
        {
            model.Lifes++;
        }
    }

    public class LifeMinus : Bonus
    {
        public LifeMinus(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }

        public override void Act(GameModel model)
        {
            model.SetDefault();
            model.Lifes--;
        }
    }

    public class FireBallBonus : Bonus
    {
        public FireBallBonus(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }

        public override void Act(GameModel model)
        {
            model.Ball.State = BallState.Flaming;
            model.Ball.Img = Image.FromFile("images\\fireballbonus.png");
        }
    }

    public class FastBallBonus : Bonus
    {
        public FastBallBonus(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }

        public override void Act(GameModel model)
        {
            model.Ball.Velocity = 14;          
        }
    }

}
