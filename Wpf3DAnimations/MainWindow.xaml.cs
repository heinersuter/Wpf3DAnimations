namespace Wpf3DAnimations
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;

    public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly TimeSpan _pollingTime = TimeSpan.FromSeconds(0.2);

        private readonly AxisAngleRotation3D _innerAxisRotation = new AxisAngleRotation3D(new Vector3D(0.0, 0.0, 1.0), 0.0);
        private readonly AxisAngleRotation3D _outerAxisRotation = new AxisAngleRotation3D(new Vector3D(1.0, 0.0, 0.0), 0.0);

        private readonly AxisSimulator _innerAxisSimulator = new AxisSimulator();
        private readonly AxisSimulator _outerAxisSimulator = new AxisSimulator();

        private double _innerAxisRate;
        private double _innerAxisPosition;
        private double _outerAxisRate;
        private double _outerAxisPosition;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            var worker = new BackgroundWorker();
            worker.DoWork += Poll;
            worker.RunWorkerAsync();

            InnerAxis.Transform = new RotateTransform3D(_innerAxisRotation);
            OuterAxis.Transform = new RotateTransform3D(_outerAxisRotation);
        }

        public double InnerAxisRate
        {
            get { return _innerAxisRate; }
            set
            {
                _innerAxisRate = value;
                _innerAxisSimulator.Rate = value;
                OnPropertyChanged();
            }
        }

        public double InnerAxisPosition
        {
            get { return _innerAxisPosition; }
            set
            {
                _innerAxisPosition = value;
                _innerAxisSimulator.Position = value;
                OnPropertyChanged();
            }
        }

        public double OuterAxisRate
        {
            get { return _outerAxisRate; }
            set
            {
                _outerAxisRate = value;
                _outerAxisSimulator.Rate = value;
                OnPropertyChanged();
            }
        }

        public double OuterAxisPosition
        {
            get { return _outerAxisPosition; }
            set
            {
                _outerAxisPosition = value;
                _outerAxisSimulator.Position = value;
                OnPropertyChanged();
            }
        }

        private void Animate(double position, double rate, AxisAngleRotation3D axisAngleRotation3D, TimeSpan animationDuration)
        {
            if (Math.Abs(rate) > 0.1 * animationDuration.TotalSeconds)
            {
                // Simulate future movement with given rate
                var newPosition = position + rate * animationDuration.TotalSeconds;
                axisAngleRotation3D.BeginAnimation(AxisAngleRotation3D.AngleProperty,
                    new DoubleAnimation(position, newPosition, new Duration(animationDuration)));
            }
            else
            {
                // Jump to current position
                axisAngleRotation3D.BeginAnimation(AxisAngleRotation3D.AngleProperty,
                    new DoubleAnimation(position, new Duration(TimeSpan.Zero)));
            }
        }

        private void Poll(object sender, DoWorkEventArgs args)
        {
            while (true)
            {
                Dispatcher.Invoke(() =>
                {
                    InnerAxisPosition = _innerAxisSimulator.Position;
                    OuterAxisPosition = _outerAxisSimulator.Position;
                    Animate(_innerAxisSimulator.Position, _innerAxisSimulator.Rate, _innerAxisRotation, _pollingTime);
                    Animate(_outerAxisSimulator.Position, _outerAxisSimulator.Rate, _outerAxisRotation, _pollingTime);
                });
                Thread.Sleep(_pollingTime);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
