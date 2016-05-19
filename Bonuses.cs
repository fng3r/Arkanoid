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

        public abstract void Activate(GameModel el);

        public static Bonus GetRandomBonus(int x, int y, Random rnd)
        {
            var listOfBonuses = FindSubClassesOf<Bonus>().ToList();
            var bonusClass = listOfBonuses[rnd.Next(listOfBonuses.Count)];
            var bonus = (Bonus)bonusClass.GetConstructor(new Type[] { typeof(int), typeof(int) }).Invoke(new object[] { x, y});
            return bonus;
        }

        static IEnumerable<Type> FindSubClassesOf<TBaseType>()
        {
            var baseType = typeof(TBaseType);

            return baseType.Assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
        }
    }

    public class DecreaseBonus : Bonus
    {
        public DecreaseBonus(int x, int y) : base(x, y, Settings.BonusSize.Width, Settings.BonusSize.Height)
        {

        }

        public override void Activate(GameModel model)
        {
            Ship ship = model.Ship;
            if(ship.Frame.Width > 55)
            ship.Frame = new Rectangle(ship.Frame.X + ship.Frame.Width / 2, ship.Frame.Y, (int)(ship.Frame.Width / 1.5), ship.Frame.Height);
            ship.ResizeImg();
        }
    }

    public class ExpandBonus : Bonus

    {
        public ExpandBonus(int x, int y) : base(x, y, Settings.BonusSize.Width, Settings.BonusSize.Height)
        {

        }

        public override void Activate(GameModel model)
        {
            Ship ship = model.Ship;
            if (ship.Frame.Width <= 630)
                ship.Frame = new Rectangle(ship.Frame.X - ship.Frame.Width / 2, ship.Frame.Y, (int)(ship.Frame.Width * 1.5), ship.Frame.Height);
                ship.ResizeImg();
        }
    }

    public class BulletBonus : Bonus
    {
        public BulletBonus(int x, int y) : base(x, y, Settings.BonusSize.Width, Settings.BonusSize.Height)
        {

        }

        public override void Activate(GameModel model)
        {
            Ship ship = model.Ship;
            ship.Bullets += 10;
        }
    }

    public class LifeBonus : Bonus
    {
        public LifeBonus(int x, int y) : base(x, y, Settings.BonusSize.Width, Settings.BonusSize.Height)
        {

        }

        public override void Activate(GameModel model)
        {
            model.Lifes++;
        }
    }

    public class DeathBonus : Bonus
    {
        public DeathBonus(int x, int y) : base(x, y, Settings.BonusSize.Width, Settings.BonusSize.Height)
        {

        }

        public override void Activate(GameModel model)
        {
            model.SetDefault();
            model.Lifes--;
        }
    }

    public class FireBallBonus : Bonus
    {
        public FireBallBonus(int x, int y) : base(x, y, Settings.BonusSize.Width, Settings.BonusSize.Height)
        {

        }

        public override void Activate(GameModel model)
        {
            model.Ball.State = BallState.Flaming;
            model.Ball.Img = Image.FromFile("images\\fireballbonus.png");
        }
    }

    public class FastBallBonus : Bonus
    {
        public FastBallBonus(int x, int y) : base(x, y, Settings.BonusSize.Width, Settings.BonusSize.Height)
        {

        }

        public override void Activate(GameModel model)
        {
            model.Ball.Velocity = 12;          
        }
    }

}
