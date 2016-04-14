using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using SecureTcpWpfClient.Annotations;
using SecureTcpWpfClient.ViewModel;

namespace SecureTcpWpfClient.Model
{
    public class Connection : INotifyPropertyChanged
    {
        private TcpClient _client;
        private Stream _stream;
        private SslStream _sslStream;
        private StreamReader _sr;
        private StreamWriter _sw;
        private MainViewModel _vm;
        private volatile bool _connected;

        public bool Connected
        {
            get { return _connected; } 
            set { _connected = value; }
        }

        public StreamReader Sr
        {
            get { return _sr; }
            set { _sr = value; }
        }

        public StreamWriter Sw
        {
            get { return _sw; }
            set { _sw = value; }
        }

        public Connection(string address, int port, string serverCertName, MainViewModel vm)
        {
            _client = new TcpClient(address, port);
            Connected = true;
            _vm = vm;
            _stream = _client.GetStream();
            Authenticate(serverCertName, address);
            Sr = new StreamReader(_sslStream);
            Sw = new StreamWriter(_sslStream);
        }

        public void Authenticate(string serverCertName, string address)
        {
            try
            {
                _sslStream = new SslStream(_stream, false);
                _sslStream.AuthenticateAsClient(serverCertName);
            }
            catch (Exception)
            {
                Dispose();
            }
        }

        public void Listen()
        {
            try
            {
                while (_connected)
                {
                    string message = Sr.ReadLine();
                    App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                    {
                        _vm.MHandler.HandleIncomingMessage(message);
                    });
//                    if (message == "quit")
//                    {
//                        Connected = false;
//                    }
                }
            }
            catch (Exception e)
            {
                Dispose();
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                Sw.WriteLine(message);
                Sw.Flush();
            }
            catch (Exception)
            {
                Dispose();
            }

        }

        public void Dispose()
        {
            if (_stream.CanWrite)
                SendMessage("quit");
            _vm.Users.Clear();
            _vm.MessageCollection.Add(new Message("Quitting Server"));
            Connected = false;
            _stream.Close();
            _client.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private bool InstallCert()
        //{
        //    X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);

        //    try
        //    {
        //        store.Open(OpenFlags.ReadWrite);
        //        X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, "")
        //    }
        //}
    }
}
