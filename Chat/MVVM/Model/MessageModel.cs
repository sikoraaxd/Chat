using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.MVVM.Model
{
    internal class MessageModel
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}
