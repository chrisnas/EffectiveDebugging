using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;

namespace TestApplication
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, List<string>> _strings;
        private readonly List<Timer> _timers;

        public MainWindow()
        {
            InitializeComponent();

            Title = $"{Process.GetCurrentProcess().Id.ToString()} - {Title}";

            _timers = new List<Timer>();
        }

        private void btnStringDuplicates_Click(object sender, RoutedEventArgs e)
        {
            _strings = new Dictionary<string, List<string>>();
            _strings["0123456789"] = new List<string>(GetStringList(128));
            _strings["_123456789"] = new List<string>(GetStringList("_123456789", 128));
        }

        private string GetNewString()
        {
            var copy = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                copy.Append(i.ToString());
            }
            return copy.ToString();
        }

        private List<string> GetStringList(string s, int count)
        {
            var strings = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                strings.Add(s);
            }

            return strings;
        }
        private List<string> GetStringList(int count)
        {
            var strings = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                strings.Add(GetNewString());
            }

            return strings;
        }

        private void btnTimers_Click(object sender, RoutedEventArgs e)
        {
            StopTimers();

            if (MessageBox.Show("Click OK to start new timers", "", MessageBoxButton.OKCancel) ==
                MessageBoxResult.Cancel) return;

            AddDelegateTimers(_timers);
            AddMethodTimers(_timers);
        }

        private void StopTimers()
        {
            foreach (var timer in _timers)
            {
                timer.Dispose();
            }
        }

        private void AddDelegateTimers(List<Timer> timers)
        {
            for (int i = 0; i < 3; i++)
            {
                _timers.Add(new Timer((state) =>
                    {
                        Debug.WriteLine($"delegate #{state} ticked");
                    },
                    i.ToString(), 1000, 5000));
            }
        }

        private void AddMethodTimers(List<Timer> timers)
        {
            for (int i = 0; i < 5; i++)
            {
                _timers.Add(new Timer(OnTimer, i.ToString(), 1000, 5000));
            }
        }

        private void OnTimer(object state)
        {
            Debug.WriteLine($"method #{state} ticked");
        }



        private void FullGC_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
