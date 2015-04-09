namespace Wpf3DAnimations.Views
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using Wpf3DAnimations.Common.Mvvm;

    public class ChartViewModel : ViewModel
    {
        private readonly Stopwatch _stopwatch;
        private readonly TimeSpanAxis _timeAxis;
        private TimeSpan _visibleTime;
        private const string TimeAxisKey = "TimeAxis";
        private const string PositionAxisKey = "PositionAxis";
        private const string RateAxisKey = "RateAxis";

        public ChartViewModel()
        {
            _stopwatch = new Stopwatch();

            PlotModel = new PlotModel();

            _timeAxis = new TimeSpanAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Time",
                Key = TimeAxisKey,
                IsPanEnabled = false,
                IsZoomEnabled = false
            };
            PlotModel.Axes.Add(_timeAxis);
            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Degrees",
                Key = PositionAxisKey,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Minimum = 0,
                Maximum = 360
            });
            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Right,
                Title = "Degrees/s",
                Key = RateAxisKey,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Minimum = -360,
                Maximum = 360
            });
        }

        public PlotModel PlotModel
        {
            get { return BackingFields.GetValue<PlotModel>(); }
            private set { BackingFields.SetValue(value); }
        }

        public void AddPositionPoint(double position, string name)
        {
            AddPoint(position, name, PositionAxisKey);
        }

        public void AddRatePoint(double rate, string name)
        {
            AddPoint(rate, name, RateAxisKey);
        }

        private void AddPoint(double value, string name, string yAxisKey)
        {
            if (!_stopwatch.IsRunning)
            {
                _stopwatch.Start();
            }

            var serie = PlotModel.Series.OfType<LineSeries>().SingleOrDefault(series => series.Title == name);
            if (serie == null)
            {
                serie = new LineSeries
                {
                    Title = name,
                    XAxisKey = TimeAxisKey,
                    YAxisKey = yAxisKey,
                    TrackerFormatString = "{0}\n{1}: {2:mm\\:ss\\:ms}\n{3}: {4:0.###}"
                };
                PlotModel.Series.Add(serie);
            }

            serie.Points.RemoveAll(point => point.X < TimeSpanAxis.ToDouble(_stopwatch.Elapsed - _visibleTime));
            serie.Points.Add(new DataPoint(TimeSpanAxis.ToDouble(_stopwatch.Elapsed), value));

            _visibleTime = TimeSpan.FromSeconds(10);
            _timeAxis.Minimum = Math.Max(0, TimeSpanAxis.ToDouble(_stopwatch.Elapsed - _visibleTime));
            PlotModel.InvalidatePlot(true);
        }
    }
}
