using System;
using System.Threading;
using System.Threading.Tasks;

namespace StockMarket
{
    public class Clock
    {
        private readonly Action _callback;
        private ManualResetEventSlim _mre = new ManualResetEventSlim();

        public Clock(Action callback)
        {
            _callback = callback;
        }

        public void WaitForNextCycle()
        {
            _mre.Wait();
        }

        public void Start()
        {
            Task.Run(Timer);
        }

        private async Task Timer()
        {
            while (true)
            {
                await Task.Delay(1000);

                var old = Interlocked.Exchange(ref _mre, new ManualResetEventSlim());
                old.Set();

                _ = Task.Run(_callback);
            }
        }
    }
}