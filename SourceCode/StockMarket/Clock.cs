using System.Threading;

namespace StockMarket
{
    public class Clock
    {
        private ManualResetEventSlim _mre = new ManualResetEventSlim();

        public void Tick()
        {
            var old = Interlocked.Exchange(ref _mre, new ManualResetEventSlim());
            old.Set();
        }

        public void WaitForNextCycle()
        {
            _mre.Wait();
        }
    }
}