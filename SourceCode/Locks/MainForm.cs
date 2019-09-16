using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Locks
{
    public partial class MainForm : Form
    {
        private readonly ManualResetEvent _continueDeadlock;
        private readonly object _lock1 = new Object();
        private readonly object _lock2 = new Object();
        private int _resource1 = 0;
        private int _resource2 = 0;
        private bool _lockStarted = false;

        public MainForm()
        {
            InitializeComponent();

            _continueDeadlock = new ManualResetEvent(false);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = $"pid = {Process.GetCurrentProcess().Id}";

            ThreadPool.QueueUserWorkItem(OnAccessResource, new object[] { 1, _lock1, _lock2 });
            ThreadPool.QueueUserWorkItem(OnAccessResource, new object[] { 2, _lock2, _lock1 });
        }

        private void btnDeadlock_Click(object sender, EventArgs e)
        {
            if (_lockStarted)
            {
                _continueDeadlock.Reset();
                _lockStarted = false;
                btnDeadlock.Text = "Start dead(good)lock";
            }
            else
            {
                _continueDeadlock.Set();
                _lockStarted = true;
                btnDeadlock.Text = "Stop dead(good)lock";
            }
        }

        private void OnAccessResource(object state)
        {
            Random r = new Random();

            while (true)
            {
                _continueDeadlock.WaitOne();

                object[] parameters = (object[])state;
                int id = (int)parameters[0];

                lock (parameters[1])
                {
                    _resource1++;
                    lock (parameters[2])
                    {
                        _resource2++;

                        this.BeginInvoke(
                            (Action)(() =>
                            {
                                tbResult.Text = $"[{id}] - {_resource1} | {_resource2}";
                            })
                        );
                        Thread.Sleep((int)(500 + r.NextDouble() * 1000));
                    }
                }
            }
        }

    }
}
