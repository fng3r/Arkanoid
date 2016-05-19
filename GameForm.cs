using System;
using System.Media;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
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
        bool started;
        Button restartButton;

        public GameForm(GameModel game)
        {
            InitializeButtons();
            InitializeComponent();
            Size = game.WindowSize;
            this.game = game;
            DoubleBuffered = true;
            Text = "ARKANOID";
            timer = new Timer();
            timer.Interval = 5;
            MinimizeBox = false;
            WindowState = FormWindowState.Maximized;
            Focus();
            timer.Tick += TimerTick;
            game.LevelCompleted += () =>
            {
                timer.Stop();
                    CreateGraphics().DrawString("LEVEL COMPLETED", new Font("Arial", 40, FontStyle.Bold), Brushes.Red, (Width - 12 * 40)/2, (Height-40)/2);
                    Thread.Sleep(1500);
                    Invalidate();
                    Update();
                timer.Start();
            };
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Left) left = true;
            if (e.KeyCode == Keys.Right) right = true;
            if (e.KeyCode == Keys.Space) game.ReleaseBall();
            if (e.KeyCode == Keys.X) game.Shooting();
            if (e.KeyCode == Keys.Enter)
            {
                started = true;
                timer.Start();
            }

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
            if (game.GameOver)
            {
                
            }
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
            
            if (!started)
            {
                g.DrawString("Press ENTER to start a new game",
                    new Font("Arial", 30), Brushes.White, 350 , Height - 200);
                g.DrawImage(Image.FromFile(@"images\arkanoid_logo.png"), new Point(250, 200));
                return;
            }
            
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
                Controls.Add(restartButton);
            };
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            BackgroundImage = Image.FromFile(@"images\space.png");
            ClientSize = new Size(293, 272);
            Name = "GameForm";
            Load += new EventHandler(LoadMusic);
            ResumeLayout(false);
        }

        private void InitializeButtons()
        {
            restartButton = new Button();
            restartButton.Size = new Size(400, 130);
            restartButton.Location = new Point(500, 100);
            restartButton.Image = Image.FromFile(@"images\restart.jpg");
            restartButton.Click += (sender, args) =>
            {
                game = new GameModel(game.WindowSize);
                started = false;
                Controls.Remove(restartButton);
                Invalidate();
            };
        }

        private void LoadMusic(object sender, EventArgs e)
        {
            SoundPlayer Audio;
            Audio = new SoundPlayer("muzyka_kosmosa.wav");
            //Audio.Load();
            //Audio.PlayLooping();
        }
    }
}
