using System;
using System.Windows.Input;

namespace Pravadnichok.FileExplorer
{
    /// <summary>
    /// A basic command that runs an Action
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action mAction;

        // The event thats fired when the <see cref="CanExecute(object)"/> value has changed
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action action)
        {
            mAction = action;
        }


        #region Command Methods

        // A relay command can always execute
        public bool CanExecute(object parameter)
        {
            return true;
        }

        // Executes the commands Action
        public void Execute(object parameter)
        {
            mAction();
        }

        #endregion
    }
}
