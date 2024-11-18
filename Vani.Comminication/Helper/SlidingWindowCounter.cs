namespace Vani.Comminication.Helper
{
    public class SlidingWindowCounter(int limit)
    {
        private int _count;
        private DateTime _windowStart = DateTime.UtcNow;

        public DateTime LastAccessed { get; internal set; }

        public bool Increment()
        {
            var now = DateTime.UtcNow;
            if (now > _windowStart.AddSeconds(1))
            {
                _windowStart = now;
                _count = 1;
                return true;
            }
            if (_count < limit)
            {
                _count++;
                return true;
            }
            return false;
        }
    }

}
