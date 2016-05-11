using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class GameForm : Form
    {
        GameModel game;
        Image ship;
        Image ball;
        Image brick;
        bool left;
        bool right;
        Timer timer;


        public GameForm(GameModel game)
        {
            this.game = game;
            DoubleBuffered = true;
            Text = "ARKANOID";
            ship = Image.FromFile(@"images\ship.png");
            ball = Image.FromFile(@"images\ball.png");
            brick = Image.FromFile(@"images\brick.png");
            timer = new Timer();
            timer.Interval = 10;
            WindowState = FormWindowState.Maximized;
            Focus();
            timer.Tick += TimerTick;
            timer.Start();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Left) left = true;
            if (e.KeyCode == Keys.Right) right = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Left) left = false;
            if (e.KeyCode == Keys.Right) right = false;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (game == null) return;
            Turn control = left ? Turn.Left : right ? Turn.Right : Turn.None;
            game.Move(ClientRectangle, control);
            if (game.Bricks.Count == 0)
                timer.Stop();
            Invalidate();
            Update();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(Brushes.Wheat, ClientRectangle);

            if (game == null) return;
            if (timer.Enabled)
            {
                var matrix = g.Transform;
                g.DrawImage(ball, new Point(game.Ball.Location.X, game.Ball.Location.Y));
                g.Transform = matrix;
                g.DrawImage(ship, new Point(game.Ship.Location.X, game.Ship.Location.Y));

                foreach (var b in game.Bricks)
                {
                    g.DrawImage(brick, new Point(b.Block.X, b.Block.Y));
                }
            }
        }
    }
}
