namespace Wpf3DAnimations
{
    using System.ComponentModel;
    using System.Threading;

    public class AxisSimulator
    {
        public AxisSimulator()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += UpdateValues;
            worker.RunWorkerAsync();
        }

        public double Rate { get; set; }

        public double Position { get; set; }

        private void UpdateValues(object sender, DoWorkEventArgs args)
        {
            const int framerate = 100; // per second
            while (true)
            {
                var newPosition = (Position + (Rate / framerate));
                newPosition = newPosition % 360.0;
                if (newPosition < 0)
                {
                    newPosition = 360.0 + newPosition;
                }
                Position = newPosition;
                Thread.Sleep(1000 / framerate);
            }
        }
    }
}
