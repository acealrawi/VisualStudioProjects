using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace UDP_server {


    public struct ChatClientMessage {
        public string UserName;
        public string Message;

        public ChatClientMessage(string UserName, string Message) {
            this.UserName = UserName;
            this.Message = Message;
        }

        public byte[] Serialize() {
            List<byte> Buffer = new List<byte>();
            
            
            byte[] NameBuffer = Encoding.Unicode.GetBytes(UserName);

            Buffer.AddRange(BitConverter.GetBytes(NameBuffer.Length));
            Buffer.AddRange(NameBuffer);

            byte[] MessageBuffer = Encoding.Unicode.GetBytes(Message);

            Buffer.AddRange(BitConverter.GetBytes(MessageBuffer.Length));
            Buffer.AddRange(MessageBuffer);

            return Buffer.ToArray();
        }

        public static ChatClientMessage Deserialize(byte[] Buffer) {
            int UserNameLength = BitConverter.ToInt32(Buffer,0);
            string UserName = Encoding.Unicode.GetString(Buffer, sizeof(int), UserNameLength);

            int MessageLength = BitConverter.ToInt32(Buffer, sizeof(int) + UserNameLength);
            string Message = Encoding.Unicode.GetString(Buffer, sizeof(int) * 2 + UserNameLength, MessageLength);

            return new ChatClientMessage(UserName, Message);
        }
    }


    /**********************************************************************************************************************/

    public struct ChatServerMessage {
        public string Message;

        public ChatServerMessage(string Message) {
            this.Message = Message;
        }

        public byte[] Serialize() {
            List<byte> Buffer = new List<byte>();

            byte[] MessageBuffer = Encoding.Unicode.GetBytes(Message);

            Buffer.AddRange(BitConverter.GetBytes(MessageBuffer.Length));
            Buffer.AddRange(MessageBuffer);

            return Buffer.ToArray();
        }

        public static ChatServerMessage Deserialize(byte[] Buffer) {
            int MessageLength = BitConverter.ToInt32(Buffer, 0);
            string Message = Encoding.Unicode.GetString(Buffer, sizeof(int), MessageLength);

            return new ChatServerMessage(Message);
        }
    }

  
    
}
