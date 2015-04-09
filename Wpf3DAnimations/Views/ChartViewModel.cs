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

        public ChartViewModel()
        {
            _stopwatch = new Stopwatch();

            PlotModel = new PlotModel();

            _timeAxis = new TimeSpanAxis { Position = AxisPosition.Bottom, Title = "Time", };
            PlotModel.Axes.Add(_timeAxis);
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Degrees", Minimum = 0, Maximum = 360 });
        }

        public PlotModel PlotModel
        {
            get { return BackingFields.GetValue<PlotModel>(); }
            private set { BackingFields.SetValue(value); }
        }

        public void AddPositionPoint(double position, string name)
        {
            if (!_stopwatch.IsRunning)
            {
                _stopwatch.Start();
            }

            var serie = PlotModel.Series.OfType<LineSeries>().SingleOrDefault(series => series.Title == name);
            if (serie == null)
            {
                serie = new LineSeries { Title = name, TrackerFormatString = "{0}\n{1}: {2:mm\\:ss\\:ms}\n{3}: {4:0.###}" };
                PlotModel.Series.Add(serie);
            }

            serie.Points.Add(new DataPoint(TimeSpanAxis.ToDouble(_stopwatch.Elapsed), position));

            _timeAxis.Minimum = Math.Max(0, TimeSpanAxis.ToDouble(_stopwatch.Elapsed - TimeSpan.FromSeconds(10)));
            PlotModel.InvalidatePlot(true);
        }
    }
}
