
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * RelayCommandWithParameter file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 5/29/2017 2:44:23 PM
* ******************************************************************************/

using System;
using System.Windows.Input;


namespace VollomeStudio.Helpers
{
    public class RelayCommandWithParameter : ICommand
    {
        #region Fields

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        public RelayCommandWithParameter(Action<object> execute)
            : this(execute, null)
        {

        }

        public RelayCommandWithParameter(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        #endregion
    } 
}