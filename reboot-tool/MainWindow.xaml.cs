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
using System.Configuration;

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
            dt.Tick += DtTicker;
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
        public class Values
        {

        }

        //config variables import
        public static string CommandVal()
        {
            string result = string.Empty;
            result = ConfigurationManager.AppSettings["command"];
            return result;

        }
        public static string SwitchesVal()
        {
            string result = string.Empty;
            result = ConfigurationManager.AppSettings["switches"];
            return result;

        }
        public static int InitialTimeVal()
        {
            int result = 0;
            result = Convert.ToInt32(ConfigurationManager.AppSettings["initialTime"]);
            return result;

        }
        public static int WarningTimeVal()
        {
            int result = 0;
            result = Convert.ToInt32(ConfigurationManager.AppSettings["warningTime"]);
            return result;

        }
        public static int DeferTimeVal()
        {
            int result = 0;
            result = Convert.ToInt32(ConfigurationManager.AppSettings["deferTime"]);
            return result;

        }
        //String commandVal = ConfigurationManager.AppSettings["command"];
        //String switchesVal = ConfigurationManager.AppSettings["switches"];
        //int initialTimeVal = Convert.ToInt32(ConfigurationManager.AppSettings["initialTime"]);
        //int warningTimeVal = Convert.ToInt32(ConfigurationManager.AppSettings["warningTime"]);
        //int deferTimeVal = Convert.ToInt32(ConfigurationManager.AppSettings["deferTime"]);
        public int deincrement = InitialTimeVal(); //0.5 hours in seconds
        private void DtTicker(object sender, EventArgs e)
        {
            deincrement--;
            //timerLabel.Content = deincrement.ToString()+" Minutes";
            string timeVal = deincrement.ToString();
            double dtimeVal = Convert.ToDouble(timeVal);
            TimeSpan.FromMinutes(dtimeVal);
            TimeSpan time = TimeSpan.FromSeconds(dtimeVal);
            string str = time.ToString(@"hh\:mm\:ss");
            timerLabel.Content = str;
            if (deincrement == WarningTimeVal()) //5 minutes in seconds
            {
                this.WindowState = WindowState.Normal;
                Activate();
            }
            if (deincrement <= 0)
            {
                try
                {
                    System.Diagnostics.Process.Start(CommandVal(), SwitchesVal());
                    Close();
                }
                catch (Exception appErr)
                {
                    MessageBoxResult result = MessageBox.Show("Configuration is pointed to an application that cannot be found. Please check the path within the 'command' key is correct, or check your file permissions." + "\n \n" + "End Users should raise a problem via MyIT." + "\n" +  "Specify the following Error Code: RESTOOL ERR-0001",
                                          "Error",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                    if (result == MessageBoxResult.OK)
                    {
                        Application.Current.Shutdown();
                    }


                }

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
            deincrement += DeferTimeVal(); //1 hour in seconds
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            deincrement = 1;
        }
    }
}
