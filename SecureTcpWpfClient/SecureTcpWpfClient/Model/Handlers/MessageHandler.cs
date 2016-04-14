using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SecureTcpWpfClient.Annotations;
using SecureTcpWpfClient.ViewModel;

namespace SecureTcpWpfClient.Model.Handlers
{
    public class MessageHandler : INotifyPropertyChanged
    {
        private MainViewModel _vm;

        public MessageHandler(MainViewModel vm)
        {
            _vm = vm;
        }
        public void HandleIncomingMessage(string message)
        {
            string[] splitStrings = message.Split(';');
            switch (splitStrings[0].ToLower())
            {
                case "quit":
                    _vm.Connection.Dispose();
                    break;
                case "allusers":
                    _vm.Users.Clear();
                    for (int i = 1; i < splitStrings.Length - 1; i++)
                    {
                        _vm.Users.Add(new User(splitStrings[i]));
                    }
                    break;
                case "userconnected":
                    _vm.MessageCollection.Add(new Message("User " + splitStrings[1] + " joined the server"));
                    _vm.Users.Add(new User(splitStrings[1]));
                    break;
                case "userdisconnected":
                    _vm.MessageCollection.Add(new Message(splitStrings[1] + " left the server"));
                    _vm.Users.RemoveAt(GetIndexOfUserByUsername(splitStrings[1]));
                    break;
                case "userchangedname":
                    int index = GetIndexOfUserByUsername(splitStrings[1]);
                    _vm.Users[index].Username = splitStrings[2];
                    _vm.MessageCollection.Add(new Message(splitStrings[1] + " changed name to " + splitStrings[2]));
                    break;
                default:
                    _vm.AddMessageToCollection(message);
                    break;
            }
        }

        private int GetIndexOfUserByUsername(string username)
        {
            var found = _vm.Users.FirstOrDefault(x => x.Username == username);
            return _vm.Users.IndexOf(found);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
