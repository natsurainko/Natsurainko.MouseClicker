using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MouseClicker
{
    public class ViewModel : INotifyPropertyChanged
    {
        private int _MOUSE_KEY;
        public int MOUSE_KEY { get => _MOUSE_KEY; set { _MOUSE_KEY = value; OnPropertyChanged("MOUSE_KEY"); } }

        private int _MOUSE_MODE;
        public int MOUSE_MODE { get => _MOUSE_MODE; set { _MOUSE_MODE = value; TIME_Visibility = value == 0 ? Visibility.Visible : Visibility.Collapsed; OnPropertyChanged("MOUSE_MODE"); } }

        private Visibility _TIME_Visibility = Visibility.Visible;
        public Visibility TIME_Visibility { get => _TIME_Visibility; set { _TIME_Visibility = value; OnPropertyChanged("TIME_Visibility"); } }

        private string _TIME_SPAN = "10";
        public string TIME_SPAN { get => _TIME_SPAN; set { _TIME_SPAN = value; OnPropertyChanged("TIME_SPAN"); } }

        private string _KEYBOARD_KEY = "F4";
        public string KEYBOARD_KEY { get => _KEYBOARD_KEY; set { _KEYBOARD_KEY = value; OnPropertyChanged("KEYBOARD_KEY"); } }

        private bool _ENABLE = true;
        public bool ENABLE { get => _ENABLE; set { _ENABLE = value; OnPropertyChanged("ENABLE"); } }

        public int KEY => (int)GetKey(KEYBOARD_KEY);

        private bool _RUNNING;
        public bool RUNNING { get => _RUNNING; set { _RUNNING = value; ENABLE = !value; OnPropertyChanged("RUNNING"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public Keys GetKey(string name)
        {
            switch (name)
            {
                case "F4":
                    return Keys.F4;
                case "F6":
                    return Keys.F6;
                case "F7":
                    return Keys.F7;
                case "F8":
                    return Keys.F8;
                case "F9":
                    return Keys.F9;
                case "F10":
                    return Keys.F10;
                case "F12":
                    return Keys.F12;
                default:
                    return Keys.F4;
            }
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
