namespace Wpf3DAnimations.Views
{
    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;
    using System.Windows.Threading;
    using Wpf3DAnimations.Common.Mvvm;
    using Wpf3DAnimations.Simulator;

    public class MainWindowViewModel : ViewModel, IDisposable
    {
        private readonly Dispatcher _dispatcher;
        private readonly TimeSpan _pollingTime = TimeSpan.FromSeconds(0.1);
        private readonly Timer _timer;

        private readonly AxisAngleRotation3D _innerAxisRotation = new AxisAngleRotation3D(new Vector3D(0.0, 0.0, 1.0), 0.0);
        private readonly AxisAngleRotation3D _outerAxisRotation = new AxisAngleRotation3D(new Vector3D(1.0, 0.0, 0.0), 0.0);

        private readonly AxisSimulator _innerAxisSimulator = new AxisSimulator();
        private readonly AxisSimulator _outerAxisSimulator = new AxisSimulator();

        public MainWindowViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            TwoAxesViewModel = new TwoAxesViewModel(_innerAxisRotation, _outerAxisRotation);
            InnerAxisViewModel = new AxisViewModel(_innerAxisSimulator) { Title = "Inner Axis" };
            OuterAxisViewModel = new AxisViewModel(_outerAxisSimulator) { Title = "Outer Axis" };
            ChartViewModel = new ChartViewModel();

            _timer = new Timer(state => Poll(), null, TimeSpan.Zero, _pollingTime);
        }

        public TwoAxesViewModel TwoAxesViewModel
        {
            get { return BackingFields.GetValue<TwoAxesViewModel>(); }
            private set { BackingFields.SetValue(value); }
        }

        public ChartViewModel ChartViewModel
        {
            get { return BackingFields.GetValue<ChartViewModel>(); }
            private set { BackingFields.SetValue(value); }
        }

        public AxisViewModel InnerAxisViewModel
        {
            get { return BackingFields.GetValue<AxisViewModel>(); }
            private set { BackingFields.SetValue(value); }
        }

        public AxisViewModel OuterAxisViewModel
        {
            get { return BackingFields.GetValue<AxisViewModel>(); }
            private set { BackingFields.SetValue(value); }
        }

        private void Poll()
        {
            _dispatcher.Invoke(() =>
            {
                InnerAxisViewModel.Position = _innerAxisSimulator.Position;
                OuterAxisViewModel.Position = _outerAxisSimulator.Position;

                ChartViewModel.AddInnerAxisPositionPoint(_innerAxisSimulator.Position);
                ChartViewModel.AddOuterAxisPositionPoint(_outerAxisSimulator.Position);
                ChartViewModel.AddInnerAxisRatePoint(_innerAxisSimulator.Rate);
                ChartViewModel.AddOuterAxisRatePoint(_outerAxisSimulator.Rate);

                Animate(_innerAxisSimulator.Position, _innerAxisSimulator.Rate, _innerAxisRotation, _pollingTime);
                Animate(_outerAxisSimulator.Position, _outerAxisSimulator.Rate, _outerAxisRotation, _pollingTime);
            });
        }

        private static void Animate(double position, double rate, AxisAngleRotation3D axisAngleRotation3D, TimeSpan animationDuration)
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

        public void Dispose()
        {
            _innerAxisSimulator.Dispose();
            _outerAxisSimulator.Dispose();
            _timer.Dispose();
        }
    }
}
