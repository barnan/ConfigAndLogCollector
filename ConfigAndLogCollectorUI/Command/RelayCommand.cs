using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ConfigAndLogCollectorUI.Command
{
    class RelayCommand : ICommand, INotifyPropertyChanged
    {

        private Action _singleAction;
        private Action<object> _paramAction;
        private Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        public RelayCommand(Action act, Func<object, bool> canEx = null)
        {
            _singleAction = act;
            _canExecute = canEx;
        }

        public RelayCommand(Action<object> act, Func<object, bool> canEx = null)
        {
            _paramAction = act;
            _canExecute = canEx;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? false : _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _singleAction?.Invoke();

            _paramAction?.Invoke(parameter);
        }


        #region INotifyPropertyChanged

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
