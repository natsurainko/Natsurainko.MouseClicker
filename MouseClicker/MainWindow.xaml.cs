using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MouseClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel Model { get; set; }

        KeyboardHook KeyboardHook = new KeyboardHook();

        NotifyIcon NotifyIcon = new NotifyIcon()
        {
            Text = "MouseClicker 鼠标连点器\r\n这个玩意只是告诉你你的连点是否在执行的",
            Icon = Resource1.aero_arrow,
            Visible = true
        };

        Timer Timer { get; set; } = new Timer();

        public MainWindow()
        {
            InitializeComponent();
            Model = (ViewModel)this.DataContext;
            App.Current.Exit += Current_Exit;

            KeyboardHook.KeyDownEvent += KeyboardHook_KeyDownEvent; ;//钩住键按下
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            if (Doing)
                Do();
            if (Model.RUNNING)
                Button_Click(this, null);
        }

        private void KeyboardHook_KeyDownEvent(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyValue == Model.KEY)
                Do();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Model.RUNNING)
            {
                KeyboardHook.Start();
                Button.Content = "禁用";
            }
            else
            {
                KeyboardHook.Stop(); 
                Button.Content = "启用";
            }
            Model.RUNNING = !Model.RUNNING;
        }

        public bool Doing = false;
        public void Do()
        {
            string message = string.Empty;

            if (Model.MOUSE_MODE == 0)
            {
                Timer.Interval = Convert.ToInt32(Model.TIME_SPAN);
                void Tick(object sender, EventArgs e)
                {
                    mouse_event(Model.MOUSE_KEY == 0 ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_RIGHTDOWN, 1, 0, 0, 0);
                    mouse_event(Model.MOUSE_KEY == 0 ? MOUSEEVENTF_LEFTUP : MOUSEEVENTF_RIGHTUP, 1, 0, 0, 0);
                }

                if (!Timer.Enabled)
                {
                    Timer.Tick += Tick;
                    Timer.Start();
                    Doing = true;
                }
                else
                {
                    Timer.Stop();
                    Timer.Tick -= Tick;
                    Doing = false;
                }
            }
            else
            {
                if (!Doing)
                {
                    mouse_event(Model.MOUSE_KEY == 0 ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_RIGHTDOWN, 1, 0, 0, 0);
                    Doing = true;
                }
                else
                {
                    mouse_event(Model.MOUSE_KEY == 0 ? MOUSEEVENTF_LEFTUP : MOUSEEVENTF_RIGHTUP, 1, 0, 0, 0);
                    Doing = false;
                }
            }

            if (Doing)
                message = "开始";
            else message = "关闭";

            Task.Run(delegate
            {
                NotifyIcon.ShowBalloonTip(2000, "MouseClicker 鼠标连点器", $"鼠标连点操作已{message}执行!", ToolTipIcon.None);
            });
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _ = System.Windows.MessageBox.Show(
                "MouseClicker 一个鼠标连点器\r\n" +
                "\"natsurainko的小工具\"\r\n" +
                "Copyright © 2022 Natsurainko 版权所有", 
                "关于", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) => App.Current.Shutdown(0);
    }
}
