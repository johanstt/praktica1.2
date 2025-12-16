using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQLitePCL;

namespace praktica
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Инициализация нативной библиотеки SQLite (для System.Data.SQLite 2.x)
            Batteries_V2.Init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Инициализация базы данных
            global::praktica.Database.Initialize();

            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK && loginForm.LoggedInUser != null)
                {
                    Application.Run(new MainForm(loginForm.LoggedInUser));
                }
            }
        }
    }
}
