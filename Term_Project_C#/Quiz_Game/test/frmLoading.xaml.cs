using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QuizGame
{
    /// <summary>
    /// Interaction logic for frmLoading.xaml
    /// </summary>
    public partial class frmLoading : Window
    {
        private int progress = 0;
        private DispatcherTimer timerProgress;
        public frmLoading()
        {
            InitializeComponent();
            timerProgress = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20) 
            };
            timerProgress.Tick += TimerProgress_Tick;
            timerProgress.Start();
        }

        private void PgBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double newValue = e.NewValue;
            lbProgress.Content = $"{newValue}%";
        }

        private void TimerProgress_Tick(object sender, EventArgs e)
        {
            progress++;
            if (progress >= 100)
            {
                progress = 100;
                timerProgress.Stop();

                //MainWindow mainWindow = new MainWindow();
                //mainWindow.Show();
                frmMenu frmMenu = new frmMenu();
                frmMenu.ShowDialog();
                /*frmMain frmMain = new frmMain();
                frmMain.ShowDialog();*/
                this.Close();
            }

            PgBar.Value = progress;
        }
        
    }
}
