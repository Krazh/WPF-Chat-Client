using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SecureTcpWpfClient.Model
{
    public class MyCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteFunc(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public Action<Object> ExecuteFunc
        {
            get; set;
        }

        public Predicate<object> CanExecuteFunc
        {
            get; set;
        }
    }
}
