namespace Wpf3DAnimations.Views
{
    using System.Windows.Media.Media3D;
    using Wpf3DAnimations.Common.Mvvm;

    public class TwoAxesViewModel : ViewModel
    {
        public TwoAxesViewModel(Rotation3D innerAxisRotation, Rotation3D outerAxisRotation)
        {
            InnerAxisTransform = new RotateTransform3D(innerAxisRotation);
            OuterAxisTransform = new RotateTransform3D(outerAxisRotation);
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
    }
}
