using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using SecureTcpWpfClient.Annotations;
using SecureTcpWpfClient.Model;
using SecureTcpWpfClient.Model.Handlers;

namespace SecureTcpWpfClient.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _serverCertName = "SBBruce";
        private ObservableCollection<Message> _messageCollection;
        private string _outgoingMessage;
        private string _address;
        private string _port;

        public MyCommand SendCommand { get; set; }
        public MyCommand ConnectCommand { get; set; }
        public MyCommand DisConnectFromServer { get; set; }
        private Connection _connection;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged(_address);
            }
        }
        Thread thrd;
        public string Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged(_port);
            }
        }

        public ObservableCollection<User> Users { get; set; }
        public MessageHandler MHandler { get; set; }

        public string OutgoingMessage
        {
            get { return _outgoingMessage;}
            set
            {
                _outgoingMessage = value;
                OnPropertyChanged(_outgoingMessage);
            }
        }

        public ObservableCollection<Message> MessageCollection
        {
            get { return _messageCollection; }
            set
            {
                _messageCollection = value;
            }
        }

        public Connection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }
            
        public MainViewModel()
        {
            _messageCollection = new ObservableCollection<Message>();
            Users = new ObservableCollection<User>();
            SetupCommands();
            Address = "localhost";
            Port = "6789";
            MHandler = new MessageHandler(this);
        }

        public void AddMessageToCollection(string message)
        {
            MessageCollection.Add(new Message(message));
        }

        private void SetupCommands()
        {
            SendCommand = new MyCommand();
            SendCommand.CanExecuteFunc = obj => true;
            SendCommand.ExecuteFunc = Send;
            ConnectCommand = new MyCommand();
            ConnectCommand.CanExecuteFunc = obj => true;
            ConnectCommand.ExecuteFunc = Connect;
            DisConnectFromServer = new MyCommand();
            DisConnectFromServer.CanExecuteFunc = obj => true;
            DisConnectFromServer.ExecuteFunc = Disconnect;
        }

        public void Disconnect(object parameter)
        {
            if (_connection.Connected)
            {
                _connection.SendMessage("quit");
            }
        }

        public void Connect(object parameter)
        {
            ConnectToServer(Address, Convert.ToInt32(Port));
        }

        public void Send(object parameter)
        {
            _connection.SendMessage(OutgoingMessage);
            OutgoingMessage = "";
        }

        public void ConnectToServer(string address, int port)
        {
            _connection = new Connection(address, port, _serverCertName, this);
            
            thrd = new Thread(Connection.Listen);
            thrd.Start();
            _connection.SendMessage("getallusers");
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
