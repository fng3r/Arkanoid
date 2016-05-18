using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Size window = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            var gameModel = new GameModel(new Size(window.Width,window.Height-75));
            var form = new GameForm(gameModel);
            Application.EnableVisualStyles();
            Application.Run(form);

        }
    }
}
