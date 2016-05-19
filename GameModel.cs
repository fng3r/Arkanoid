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
        public int Lifes { get; set; }
        public Level Level { get; private set; }
        Random rnd;
        public HashSet<Bonus> Bonuses {get; private set;}
        public HashSet<Bullet> Bullets { get; private set; }
        public Size WindowSize { get; private set; }

        int lvl = 1;
        Dictionary<int, Level> levels;

        public event Action LevelCompleted;
        public bool GameOver;

        public GameModel(Size size)
        {
            WindowSize = size;
            GameOver = false;
            Lifes = 3;
            SetDefault();
            levels = GetLevels();
            Level = levels[lvl];
            rnd = new Random();
            Bonuses = new HashSet<Bonus>();
            Bullets = new HashSet<Bullet>();
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

            var blocksRemove = Level.Blocks.Where(x => x.Frame.IntersectsWith(Ball.Frame)).ToList();
            if (blocksRemove.Count != 0)
            {
                if (Ball.State != BallState.Flaming )
                {
                    var block = blocksRemove.First();
                    if (!(Ball.Frame.Right <= block.Frame.Right && Ball.Frame.Left >= block.Frame.Left))
                        Ball.Direction = Math.Sign(Ball.Direction) * Math.PI - Ball.Direction;
                    else
                    {

                        if (Ball.Frame.Top < block.Frame.Bottom && Ball.Frame.Bottom > block.Frame.Bottom)
                            Ball.Direction = -Ball.Frame.Top.CompareTo(block.Frame.Top) * Ball.Direction;
                        if (Ball.Frame.Bottom > block.Frame.Top && Ball.Frame.Bottom < block.Frame.Bottom)
                            Ball.Direction = Ball.Frame.Bottom.CompareTo(block.Frame.Bottom) * Ball.Direction;
                    }
                }
                Level.Blocks.ExceptWith(blocksRemove);
                var chance = rnd.NextDouble();
                var rect = blocksRemove.First().Frame;
                if (chance > 0.3)
                {
                    var bonus = Bonus.GetRandomBonus(rect.X, rect.Y, rnd);
                    Bonuses.Add(bonus);
                }
                Scores += 30 * blocksRemove.Count;
            }

            var bonusesRemove = new List<Bonus>();
            foreach (var bonus in Bonuses)
            {
                bonus.Move();
                if (bonus.Frame.IntersectsWith(Ship.Frame))
                {
                    bonus.Activate(this);
                    bonusesRemove.Add(bonus);
                }
            }
            Bonuses.ExceptWith(bonusesRemove);

            var bulletsRemove = new List<Bullet>();
            foreach (var bullet in Bullets)
            {
                bullet.Move();
                foreach (var block in Level.Blocks)
                    if (bullet.Frame.IntersectsWith(block.Frame))
                    {
                        blocksRemove.Add(block);
                        bulletsRemove.Add(bullet);
                    }
            }
            Level.Blocks.ExceptWith(blocksRemove);
            Bullets.ExceptWith(bulletsRemove);

            if (Ball.Frame.IntersectsWith(Ship.Frame))
            {
                var mid = Ship.Frame.Right - Ship.Frame.Width / 2;
                Ball.Direction = -Math.PI / 2 + (Math.PI / 2 * (Ball.Frame.X - mid) / (Ship.Frame.Width / 2 + 20));
            }

            if (Ball.Frame.Top <= window.Top) Ball.Direction = -Ball.Direction;
            else if (Ball.Frame.Right >= window.Right || Ball.Frame.Left <= window.Left)
                Ball.Direction = Math.Sign(Ball.Direction) * Math.PI - Ball.Direction;
        }

        void GetNextLevel()
        {
            lvl++;
            if (lvl < levels.Count)
            {
                Level = levels[lvl];
                SetDefault();
            }
        }

        public void SetDefault()
        {
            Bonuses = new HashSet<Bonus>();
            Bullets = new HashSet<Bullet>();
            Ship = new Ship((WindowSize.Width - Settings.ShipSize.Width)/2, WindowSize.Height);
            var ballX = Ship.Frame.X + (Ship.Frame.Width - Settings.BallSize.Width) / 2;
            var ballY = Ship.Frame.Y - Settings.BallSize.Height;
            Ball = new Ball(ballX, ballY);
        }

        Dictionary<int, Level> GetLevels()
        {
            var dict = new Dictionary<int, Level>();
            var wnum1 = 10;
            var hnum1= 11;
            var level = 1;
            var width1 = (WindowSize.Width - wnum1 * Settings.BrickSize.Width) / 2;
            var height1 = 50;

            HashSet<Brick> blocks1 = new HashSet<Brick>();
            for (int i = 0; i < hnum1; i++)
                for (int j = 0; j < wnum1; j++)
                {
                    if (i % 2 == 1 && j != 0 && j != 9)
                        continue;
                    var blockX = width1 + 100 * j;
                    var blockY = height1 + 30 * i;
                    var block = new Brick(blockX, blockY);
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
                    var block = new Brick(width2 + 100 * j, height2 + 30 * i);
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
                    var block = new Brick(width3 + 100 * j, height3 + 30 * i);
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
    }
}
