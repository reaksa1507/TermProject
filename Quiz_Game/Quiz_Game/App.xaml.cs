using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Quiz_Game
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            /*var mainWindow = new MainWindow();
            mainWindow.Show();*/

            //var frmMenu = new frmMenu();
            //frmMenu.Show();

            var loadingfrm = new frmLoading();
            loadingfrm.Show();
        }
    }
}
