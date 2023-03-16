using System;
using System.Threading;

namespace MyCountDownEvent
{
    public class CMyCountdownEvent : IDisposable
    {
        private volatile Int32 _count;
        private readonly ManualResetEvent _event;
        private readonly Object _sync = new Object();

        
        public CMyCountdownEvent(Int32 initialCount)
        {
            if (initialCount < 0)
                throw new ArgumentException("Initial count can't be less than 0");

            _count = initialCount;
            _event = new ManualResetEvent(_count == 0 ? true : false);
        }

        public void Signal()
        {
            if (_count == 0)
                throw new InvalidOperationException("CountDownEvent is already signaled");
            lock (_sync)
                _count--;
            SignalEventIfNeeded();
        }

        public void Signal(Int32 signalCount)
        {
            if (_count - signalCount < 0)
                throw new InvalidOperationException(String.Format("Counter can't be decreased on value greater than the counter itself"));
            lock (_sync)
                _count -= signalCount;
            SignalEventIfNeeded();
        }

        public Boolean Wait(TimeSpan timeout)
        {
            if (_event.WaitOne(timeout))
                return true;

            return false;
        }

        public Boolean Wait()
        {
            if (_event.WaitOne())
                return true;

            return false;
        }
        private void SignalEventIfNeeded()
        {
            if (_count == 0)
                _event.Set();
        }

        public void Dispose()
        {
            _event?.Dispose();
        }

    }
}
