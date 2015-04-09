namespace Wpf3DAnimations.Simulator
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    public class AxisSimulator : IDisposable
    {
        private readonly Stopwatch _stopWatch;
        private readonly Timer _timer;

        public AxisSimulator()
        {
            _stopWatch = new Stopwatch();
            _timer = new Timer(state => UpdateValues(), null, TimeSpan.Zero, TimeSpan.FromMilliseconds(10));
        }

        public double Rate { get; set; }

        public double Position { get; set; }

        private void UpdateValues()
        {
            if (!_stopWatch.IsRunning)
            {
                _stopWatch.Start();
            }
            var timeSinceLastUpdate = _stopWatch.Elapsed;
            _stopWatch.Restart();
            var framesPerSecond = 10000000.0 / timeSinceLastUpdate.Ticks;

            var newPosition = (Position + (Rate / framesPerSecond));
            newPosition = newPosition % 360.0;
            if (newPosition < 0)
            {
                newPosition = 360.0 + newPosition;
            }
            Position = newPosition;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
