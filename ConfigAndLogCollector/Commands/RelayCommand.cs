using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConfigAndLogCollector.Commands
{
    class RelayCommand :ICommand
    {
        private readonly Action _singleAction;
        private readonly Action<object> _paramAction;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action act, Func<object, bool> canexec = null )
        {
            _singleAction = act;
            _canExecute = canexec;
        }

        public RelayCommand(Action<object> act, Func<object, bool> canexec = null)
        {
            _paramAction = act;
            _canExecute = canexec;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _singleAction?.Invoke();
            _paramAction?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
