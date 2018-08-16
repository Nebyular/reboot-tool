using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace reboot_tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public MainWindow()
        {
            InitializeComponent();

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;
            dt.Start();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            if (deincrement == 0)
            {
                e.Cancel = false;
            }
        }

        private int deincrement = 305; //0.5 hours in seconds
        private void dtTicker(object sender, EventArgs e)
        {
            deincrement--;
            //timerLabel.Content = deincrement.ToString()+" Minutes";
            string timeVal = deincrement.ToString();
            double dtimeVal = Convert.ToDouble(timeVal);
            TimeSpan.FromMinutes(dtimeVal);
            TimeSpan time = TimeSpan.FromSeconds(dtimeVal);
            string str = time.ToString(@"hh\:mm\:ss");
            timerLabel.Content = str;
            if (deincrement == 300) //5 minutes in seconds
            {
                this.WindowState = WindowState.Normal;
                Activate();
            }
            if (deincrement <= 0)
            {
                string strCmdText = " ping google.com";
                System.Diagnostics.Process.Start("CMD.exe", "/C ipconfig");
                Close();

            }
        }
        int delayVal = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

            if (delayVal >= 1)
            {
                deferBtn.IsEnabled = false;
            }
            delayVal++;
            deincrement += 3600; //1 hour in seconds
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            deincrement = 1;
        }
    }
}
