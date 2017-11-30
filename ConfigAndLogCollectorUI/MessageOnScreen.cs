using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigAndLogCollectorUI
{

    public enum MessageType
    {
        Info,
        Error
    }


    public class MessageOnScreen
    {
        public MessageType Type { get; set; }
        public string Message { get; set; }


        public MessageOnScreen(MessageType typ, string message)
        {
            Message = message;
            Type = typ;
        }

    }
}
