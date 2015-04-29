namespace Wpf3DAnimations.Views
{
    using System;
    using System.Diagnostics;
    using Alsolos.Commons.Wpf.Mvvm;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public class ChartViewModel : ViewModel
    {
        private const string TimeAxisKey = "TimeAxis";
        private const string PositionAxisKey = "PositionAxis";
        private const string AmplitudeAxisKey = "AmplitudeAxis";
        private const string RateAxisKey = "RateAxis";

        private readonly Stopwatch _stopwatch;
        private readonly TimeSpan _visibleTime = TimeSpan.FromSeconds(10);

        private readonly TimeSpanAxis _timeAxis;

        private readonly LineSeries _innerAxisPositionSeries;
        private readonly LineSeries _innerAxisRateSeries;
        private readonly LineSeries _innerAxisAmplitudeSeries;
        private readonly LineSeries _outerAxisPositionSeries;
        private readonly LineSeries _outerAxisRateSeries;
        private readonly LineSeries _outerAxisAmplitudeSeries;

        public ChartViewModel()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

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
                Maximum = 360,
                PositionTier = 1
            });
            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Amplitude",
                Key = AmplitudeAxisKey,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Minimum = -1,
                Maximum = 1
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

            _innerAxisPositionSeries = new LineSeries
            {
                Title = "Inner Axis Position",
                XAxisKey = TimeAxisKey,
                YAxisKey = PositionAxisKey,
                Color = OxyColors.DarkBlue,
                IsVisible = false,
                TrackerFormatString = "{0}\n{1}: {2:mm\\:ss\\:ms}\n{3}: {4:0.###}"
            };
            PlotModel.Series.Add(_innerAxisPositionSeries);
            _innerAxisRateSeries = new LineSeries
            {
                Title = "Inner Axis Rate",
                XAxisKey = TimeAxisKey,
                YAxisKey = RateAxisKey,
                Color = OxyColors.DarkRed,
                IsVisible = false,
                TrackerFormatString = "{0}\n{1}: {2:mm\\:ss\\:ms}\n{3}: {4:0.###}"
            };
            PlotModel.Series.Add(_innerAxisRateSeries);
            _innerAxisAmplitudeSeries = new LineSeries
            {
                Title = "Inner Axis Amplitude",
                XAxisKey = TimeAxisKey,
                YAxisKey = AmplitudeAxisKey,
                Color = OxyColors.DarkGreen,
                IsVisible = false,
                TrackerFormatString = "{0}\n{1}: {2:mm\\:ss\\:ms}\n{3}: {4:0.###}"
            };
            PlotModel.Series.Add(_innerAxisAmplitudeSeries);
            _outerAxisPositionSeries = new LineSeries
            {
                Title = "Outer Axis Position",
                XAxisKey = TimeAxisKey,
                YAxisKey = PositionAxisKey,
                Color = OxyColors.LightBlue,
                IsVisible = false,
                TrackerFormatString = "{0}\n{1}: {2:mm\\:ss\\:ms}\n{3}: {4:0.###}"
            };
            PlotModel.Series.Add(_outerAxisPositionSeries);
            _outerAxisRateSeries = new LineSeries
            {
                Title = "Outer Axis Rate",
                XAxisKey = TimeAxisKey,
                YAxisKey = RateAxisKey,
                Color = OxyColors.LightSalmon,
                IsVisible = false,
                TrackerFormatString = "{0}\n{1}: {2:mm\\:ss\\:ms}\n{3}: {4:0.###}"
            };
            PlotModel.Series.Add(_outerAxisRateSeries);
            _outerAxisAmplitudeSeries = new LineSeries
            {
                Title = "Outer Axis Amplitude",
                XAxisKey = TimeAxisKey,
                YAxisKey = AmplitudeAxisKey,
                Color = OxyColors.LightGreen,
                IsVisible = false,
                TrackerFormatString = "{0}\n{1}: {2:mm\\:ss\\:ms}\n{3}: {4:0.###}"
            };
            PlotModel.Series.Add(_outerAxisAmplitudeSeries);
            IsPositionVisible = false;
            IsRateVisible = false;
            IsAmplitudeVisible = true;
        }

        public PlotModel PlotModel
        {
            get { return BackingFields.GetValue<PlotModel>(); }
            private set { BackingFields.SetValue(value); }
        }

        public bool IsPositionVisible
        {
            get { return BackingFields.GetValue<bool>(); }
            set
            {
                BackingFields.SetValue(
                    value,
                    x =>
                    {
                        _innerAxisPositionSeries.IsVisible = x;
                        _outerAxisPositionSeries.IsVisible = x;
                    });
            }
        }

        public bool IsRateVisible
        {
            get { return BackingFields.GetValue<bool>(); }
            set
            {
                BackingFields.SetValue(
                    value,
                    x =>
                    {
                        _innerAxisRateSeries.IsVisible = x;
                        _outerAxisRateSeries.IsVisible = x;
                    });
            }
        }

        public bool IsAmplitudeVisible
        {
            get { return BackingFields.GetValue<bool>(); }
            set
            {
                BackingFields.SetValue(
                    value,
                    x =>
                    {
                        _innerAxisAmplitudeSeries.IsVisible = x;
                        _outerAxisAmplitudeSeries.IsVisible = x;
                    });
            }
        }

        public void AddInnerAxisPositionPoint(double position)
        {
            AddPoint(position, _innerAxisPositionSeries);
            AddPoint(Math.Cos(position * Math.PI / 180), _innerAxisAmplitudeSeries);
        }

        public void AddInnerAxisRatePoint(double rate)
        {
            AddPoint(rate, _innerAxisRateSeries);
        }

        public void AddOuterAxisPositionPoint(double position)
        {
            AddPoint(position, _outerAxisPositionSeries);
            AddPoint(Math.Sin(position * Math.PI / 180), _outerAxisAmplitudeSeries);
        }

        public void AddOuterAxisRatePoint(double rate)
        {
            AddPoint(rate, _outerAxisRateSeries);
        }

        private void AddPoint(double value, LineSeries serie)
        {
            serie.Points.RemoveAll(point => point.X < TimeSpanAxis.ToDouble(_stopwatch.Elapsed - _visibleTime));
            serie.Points.Add(new DataPoint(TimeSpanAxis.ToDouble(_stopwatch.Elapsed), value));

            _timeAxis.Minimum = TimeSpanAxis.ToDouble(_stopwatch.Elapsed - _visibleTime);
            PlotModel.InvalidatePlot(true);
        }
    }
}
