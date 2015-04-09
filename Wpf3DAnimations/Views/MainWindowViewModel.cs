namespace Wpf3DAnimations.Views
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;
    using System.Windows.Threading;
    using Wpf3DAnimations.Common.Mvvm;
    using Wpf3DAnimations.Simulator;

    public class MainWindowViewModel : ViewModel
    {
        private readonly Dispatcher _dispatcher;
        private readonly TimeSpan _pollingTime = TimeSpan.FromSeconds(0.2);

        private readonly AxisAngleRotation3D _innerAxisRotation = new AxisAngleRotation3D(new Vector3D(0.0, 0.0, 1.0), 0.0);
        private readonly AxisAngleRotation3D _outerAxisRotation = new AxisAngleRotation3D(new Vector3D(1.0, 0.0, 0.0), 0.0);

        private readonly AxisSimulator _innerAxisSimulator = new AxisSimulator();
        private readonly AxisSimulator _outerAxisSimulator = new AxisSimulator();

        public MainWindowViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            var worker = new BackgroundWorker();
            worker.DoWork += Poll;
            worker.RunWorkerAsync();

            InnerAxisTransform = new RotateTransform3D(_innerAxisRotation);
            OuterAxisTransform = new RotateTransform3D(_outerAxisRotation);
        }

        public RotateTransform3D InnerAxisTransform
        {
            get { return BackingFields.GetValue<RotateTransform3D>(); }
            private set { BackingFields.SetValue(value); }
        }

        public RotateTransform3D OuterAxisTransform
        {
            get { return BackingFields.GetValue<RotateTransform3D>(); }
            private set { BackingFields.SetValue(value); }
        }

        public double InnerAxisPosition
        {
            get { return BackingFields.GetValue<double>(); }
            set { BackingFields.SetValue(value, x => _innerAxisSimulator.Position = value); }
        }

        public double InnerAxisRate
        {
            get { return BackingFields.GetValue<double>(); }
            set { BackingFields.SetValue(value, x => _innerAxisSimulator.Rate = value); }
        }

        public double OuterAxisPosition
        {
            get { return BackingFields.GetValue<double>(); }
            set { BackingFields.SetValue(value, x => _outerAxisSimulator.Position = value); }
        }

        public double OuterAxisRate
        {
            get { return BackingFields.GetValue<double>(); }
            set { BackingFields.SetValue(value, x => _outerAxisSimulator.Rate = value); }
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
                _dispatcher.Invoke(() =>
                {
                    InnerAxisPosition = _innerAxisSimulator.Position;
                    OuterAxisPosition = _outerAxisSimulator.Position;
                    Animate(_innerAxisSimulator.Position, _innerAxisSimulator.Rate, _innerAxisRotation, _pollingTime);
                    Animate(_outerAxisSimulator.Position, _outerAxisSimulator.Rate, _outerAxisRotation, _pollingTime);
                });
                Thread.Sleep(_pollingTime);
            }
        }
    }
}
