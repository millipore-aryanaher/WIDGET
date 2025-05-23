using System;
using System.Windows;

namespace WPFWidget
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string filenameArg = e.Args.Length > 0 ? e.Args[0] : null;

            var mainWindow = new MainWindow();

            if (!string.IsNullOrEmpty(filenameArg))
            {
                mainWindow.Tag = filenameArg;
            }

            mainWindow.Show();
        }
    }
}
