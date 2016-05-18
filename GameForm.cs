using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Game
{
    public partial class GameForm : Form
    {
        GameModel game;
        bool left;
        bool right;
        Timer timer;
        bool paused;

        public GameForm(GameModel game)
        {
            Size = game.SizeOfWindow;
            this.game = game;
            DoubleBuffered = true;
            Text = "ARKANOID";
            timer = new Timer();
            timer.Interval = 10;
            MinimizeBox = false;
            WindowState = FormWindowState.Maximized;
            Focus();
            timer.Tick += TimerTick;
            game.LevelCompleted += () =>
            {
                timer.Stop();
                for (int i = 1; i < 400; i += 2)
                {
                    CreateGraphics().DrawString("LEVEL COMPLETED", new Font("Arial", i / 5 + 1, FontStyle.Bold), Brushes.Red, (Width - 12 * i/5)/2, (Height-i/5)/2);
                    Invalidate();
                    Update();
                    Thread.Sleep(5);
                }
                timer.Start();
            };
            timer.Start();
            InitializeComponent();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Left) left = true;
            if (e.KeyCode == Keys.Right) right = true;
            if (e.KeyCode == Keys.Space) game.ReleaseBall();
            if (e.KeyCode == Keys.X) game.Shooting();

            if (e.KeyCode == Keys.Escape)
            {
                paused = !paused;
                if (paused)
                    timer.Stop();
                else
                    timer.Start();
                Invalidate();
            }
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
            Invalidate();
            Update();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
           

            if (game != null)
            {
                foreach (var item in game)
                    g.DrawImage(item.Img, item.Location);
            }
            g.DrawString(string.Format("Lifes: {0}", game.Lifes), new Font("Arial", 15), Brushes.Gold, Width-100, 0);
            g.DrawString(game.Scores.ToString(), new Font("Arial", 20), Brushes.Gold, 0, 0);

            if (paused)
            {
                g.DrawString("Paused", new Font("Arial", 70), Brushes.DarkRed, 500, 300);
            }

            if (game.GameOver)
            {
                g.DrawString("GAME OVER", new Font("Arial", 50), Brushes.Red, (Width - 400) / 2, (Height - 50) / 2);
                timer.Stop();
            };
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GameForm
            // 
            this.BackgroundImage = Image.FromFile("images\\space.png");
            this.ClientSize = new System.Drawing.Size(293, 272);
            this.Name = "GameForm";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.ResumeLayout(false);

        }   

        private void GameForm_Load(object sender, EventArgs e)
        {
            System.Media.SoundPlayer Audio;
            Audio = new System.Media.SoundPlayer("muzyka_kosmosa.wav");
            Audio.Load();
            Audio.PlayLooping();
        }
    }
}
