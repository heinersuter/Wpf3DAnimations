namespace Wpf3DAnimations.Views
{
    using Wpf3DAnimations.Common.Mvvm;
    using Wpf3DAnimations.Simulator;

    public class AxisViewModel : ViewModel
    {
        private readonly AxisSimulator _axisSimulator;

        public AxisViewModel(AxisSimulator axisSimulator)
        {
            _axisSimulator = axisSimulator;
        }

        public string Title
        {
            get { return BackingFields.GetValue<string>(); }
            set { BackingFields.SetValue(value); }
        }

        public double Position
        {
            get { return BackingFields.GetValue<double>(); }
            set { BackingFields.SetValue(value, x => _axisSimulator.Position = value); }
        }

        public double Rate
        {
            get { return BackingFields.GetValue<double>(); }
            set { BackingFields.SetValue(value, x => _axisSimulator.Rate = value); }
        }
    }
}
