using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class GameModel
    {
        public Ship Ship { get; private set; }
        public Ball Ball { get; private set; }
        public int Scores { get; private set; }
        public int Lifes { get; private set; } = 3;
        public Level Level { get; private set; }
        Random rnd;
        public List<Bonus> Bonuses {get; private set;}
        public List<Bullet> Bullets { get; private set; }

        int lvl = 1;
        Dictionary<int, Level> levels;

        public event Action LevelCompleted;
        public bool GameOver;

        public GameModel()
        {
            SetDefault();
            levels = GetLevels();
            Level = levels[lvl];
            rnd = new Random();
            Bonuses = new List<Bonus>();
            Bullets = new List<Bullet>();
        }

        public void ReleaseBall()
        {
            Ball.State = BallState.Free;
        }

        public void Shooting()
        {
            if (Ship.Bullets > 0)
            {
                Ship.Bullets -= 2;
                var b1 = new Bullet(Ship.Frame.Left, Ship.Frame.Y);
                var b2 = new Bullet(Ship.Frame.Right, Ship.Frame.Y);
                Bullets.Add(b1);
                Bullets.Add(b2);
            } 
        }

        public void Move(Rectangle window, Turn turnRate)
        {
            var oldX = Ship.Frame.X;
            Ship.Move(window, turnRate);
            if (Ball.State != BallState.Caught)
                Ball.Move();
            else
            {
                Ball.StickWith(Ship.Frame.X - oldX);
            }

            if (Ball.Frame.Bottom - Ball.Frame.Height / 2 >= window.Bottom)
            {
                Lifes--;
                SetDefault();
            }

            if (Lifes == 0) GameOver = true;

            if (Level.Blocks.Count == 0)
            {
                Scores += 1000 * lvl;
                LevelCompleted();
                GetNextLevel();
            }

            var toRemove = Level.Blocks.Where(x => x.Frame.IntersectsWith(Ball.Frame)).ToList();
            if (toRemove.Count != 0)
            {
                Ball.Direction += Ball.Direction > 0 ? Math.PI / 2 : -Math.PI / 2;
                Level.Blocks.ExceptWith(toRemove);
                var chance = rnd.NextDouble();
                var rect = toRemove.First().Frame;
                if (chance > 0.7)
                    Bonuses.Add(new BulletBonus(rect.X, rect.Y, rect.Width, rect.Height));
                Scores += 30 * toRemove.Count;
            }

            var bonusesRemove = new List<Bonus>();
            foreach (var bonus in Bonuses)
            {
                bonus.Move();
                if (bonus.Frame.IntersectsWith(Ship.Frame))
                {
                    bonus.Act(Ship);
                    bonusesRemove.Add(bonus);
                }
            }
            Bonuses = Bonuses.Except(bonusesRemove).ToList();

            var bulletsRemove = new List<Bullet>();
            foreach (var bullet in Bullets)
            {
                bullet.Move();
                foreach (var block in Level.Blocks)
                    if (bullet.Frame.IntersectsWith(block.Frame))
                    {
                        bulletsRemove.Add(bullet);
                        toRemove.Add(block);
                    }
            }
            Level.Blocks.ExceptWith(toRemove);
            Bullets = Bullets.Except(bulletsRemove).ToList();

            if (Ball.Frame.IntersectsWith(Ship.Frame))
                Ball.Direction += Ball.Direction > 0 ? Math.PI / 2 : -Math.PI / 2;

            if (Ball.Frame.Top <= window.Top || Ball.Frame.Right >= window.Right
                || Ball.Frame.Left <= window.Left)
                Ball.Direction += Ball.Direction > 0 ? Math.PI / 2 : -Math.PI / 2;
        }

        void GetNextLevel()
        {
            lvl++;
            Level = levels[lvl];
            SetDefault();
        }

        void SetDefault()
        { 
            Ship = new Ship(535, 670, 152, 56, 10);
            var ballX = Ship.Frame.X + (Ship.Frame.Width - 32) / 2;
            var ballY = Ship.Frame.Y - 32;
            Ball = new Ball(ballX, ballY, 32, 4, -Math.PI / 4);
            Bullets = new List<Bullet>();
        }

        Dictionary<int, Level> GetLevels()
        {
            var dict = new Dictionary<int, Level>();

            var level = 1;
            var width1 = 185;
            var height1 = 50;
            HashSet<Brick> blocks1 = new HashSet<Brick>();
            for (int i = 0; i < 11; i++)
                for (int j = 0; j < 10; j++)
                {
                    if (i % 2 == 1 && j != 0 && j != 9)
                        continue;
                    var color = "blue";
                    var blockX = width1 + 100 * j;
                    var blockY = height1 + 30 * i;
                    var block = new Brick(blockX, blockY, 100, 30, color);
                    blocks1.Add(block);
                }
            dict[level] = new Level(level, blocks1);

            level = 2;
            var width2 = 135;
            var height2 = 50;
            var blocks2 = new HashSet<Brick>();
            for (int i = 0; i < 11; i++)
                for (int j = i % 2; j < 11; j += 2)
                {
                    var color = "blue";
                    var block = new Brick(width2 + 100 * j, height2 + 30 * i, 100, 30, color);
                    blocks2.Add(block);
                }
            dict[level] = new Level(level, blocks2);

            level = 3;
            var width3 = 185;
            var height3 = 100;
            var blocks3 = new HashSet<Brick>();
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    var color = "blue";
                    var block = new Brick(width3 + 100 * j, height3 + 30 * i, 100, 30, color);
                    blocks3.Add(block);
                }
            dict[level] = new Level(level, blocks3);

            return dict;
        }

        public IEnumerator<GameElement> GetEnumerator()
        {
            yield return Ship;
            yield return Ball;
            foreach (var block in Level.Blocks)
                yield return block;
            foreach (var bullet in Bullets)
                yield return bullet;
            foreach (var bonus in Bonuses)
                yield return bonus;
        }

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}
    }
}
