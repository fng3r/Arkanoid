using System;
using System.Collections.Generic;
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
            var gameModel = new GameModel();
            var form = new GameForm(gameModel);
            Application.EnableVisualStyles();
            Application.Run(form);
        }
    }
}
