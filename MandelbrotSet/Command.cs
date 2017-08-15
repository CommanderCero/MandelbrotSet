using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MandelbrotSet
{
    public class Command<T> : ICommand where T : class 
    {
        private readonly Predicate<T> canExecute;
        private readonly Action<T> execute;

        public Command(Predicate<T> canExecute, Action<T> execute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested += value;
        }

        public bool CanExecute(object parameter)
        {
            return parameter == null ? canExecute(null) : canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }
}
