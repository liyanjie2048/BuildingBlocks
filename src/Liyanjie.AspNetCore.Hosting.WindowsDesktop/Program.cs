using System;
using System.Windows.Forms;

namespace Liyanjie.AspNetCore.Hosting.WindowsDesktop
{
    static class Program
    {
        internal static Form Form { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form = new Form();
            Application.Run(Form);
        }
    }
}
