namespace Wpf3DAnimations.Common.Mvvm
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    public abstract class NotificationObject : INotifyPropertyChanged
    {
        private BackingFields _backingFields;

        protected NotificationObject()
        {
            PropertyChanged += OnPropertyChanged;
        }

        public BackingFields BackingFields
        {
            get { return PropertyHelper.CreateIfNeeded(ref _backingFields, () => new BackingFields(RaisePropertyChanged)); }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public T CreateIfNeeded<T>(Func<T> newInstanceCreateMethod, [CallerMemberName] string propertyName = null)
        {
            if (newInstanceCreateMethod == null)
            {
                return default(T);
            }
            if (Equals(BackingFields.GetValue<T>(propertyName, null), default(T)))
            {
                BackingFields.SetValue(propertyName, newInstanceCreateMethod.Invoke(), null);
            }
            return BackingFields.GetValue<T>(propertyName, null);
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = PropertyHelper.GetName(propertyExpression);
            RaisePropertyChanged(propertyName);
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var copy = PropertyChanged;
            if (copy != null)
            {
                copy.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
