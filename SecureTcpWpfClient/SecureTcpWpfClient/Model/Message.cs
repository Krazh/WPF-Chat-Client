using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureTcpWpfClient.Model
{
    public class Message
    {
        private string _content;

        public string Content
        {
            get {return _content;}
            set { _content = value; }
        }

        public Message(string content)
        {
            Content = content;
        }
    }
}
