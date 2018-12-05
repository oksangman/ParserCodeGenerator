using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ParserCodeGenerator.Base
{
    public class Command : ICommand
    {
        #region private fields
        private readonly Action execute;
        private readonly Func<bool> canExecute;
        #endregion

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public Command()
        { }

        /// <summary>
        /// Initializes a new instance of the Command class
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public Command(Action execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Command class
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public Command(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            this.execute();
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute();
        }
    }

    public class Command<T> : ICommand
    {
        #region private fields
        private readonly Action<T> execute;
        private readonly Predicate<T> canExecute;
        #endregion

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public Command(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public Command(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is T)
            {
                return this.canExecute == null ? true : this.canExecute((T)parameter);
            }

            return this.canExecute == null ? true : this.canExecute(default(T));
        }
    }

}
