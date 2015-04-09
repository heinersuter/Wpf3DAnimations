namespace Wpf3DAnimations.Common.Mvvm
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    public class DelegateCommand : ICommand
    {
        private readonly Action _executeAction;
        private readonly Func<bool> _canExecute;

        public DelegateCommand(Action execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            _executeAction = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            _executeAction.Invoke();
        }

        public void RaiseCanExecuteChanged()
        {
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.DataBind,
                new Action(CommandManager.InvalidateRequerySuggested));
        }
    }

    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _executeAction;
        private readonly Func<T, bool> _canExecute;

        public DelegateCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _executeAction = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute.Invoke((T)parameter);
        }

        public void Execute(object parameter)
        {
            _executeAction.Invoke((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.DataBind,
                new Action(CommandManager.InvalidateRequerySuggested));
        }
    }
}
