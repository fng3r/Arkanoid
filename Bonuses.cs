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

        public abstract void Act(GameElement el);
    }

    public class ExpandBonus : Bonus

    {
        public ExpandBonus(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }

        public override void Act(GameElement _ship)
        {
            Ship ship = _ship as Ship;
            ship.Frame = new Rectangle(ship.Frame.X - ship.Frame.Width / 2, ship.Frame.Y, ship.Frame.Width * 2, ship.Frame.Height);
            Img = ship.ResizeImg();
        }
    }

    public class BulletBonus : Bonus
    {
        public BulletBonus(int x, int y, int width, int height) : base(x, y, width, height)
        {

        }

        public override void Act(GameElement _ship)
        {
            Ship ship = _ship as Ship;
            ship.Bullets += 8;
        }
    }

}
