using System;

namespace Discord
{
    public class MessageEventArgs : EventArgs
    {
        public Message Message { get; private set; }

        internal MessageEventArgs(Message msg)
        {
            Message = msg;
        }


        public override string ToString()
        {
            return Message.ToString();
        }
    }
}