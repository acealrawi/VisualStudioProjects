using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;

namespace ChatServerConsole {
    class Program {
        static void Main(string[] args) {
            ChatClientMessage ccm = new ChatClientMessage("ali", "check");
            byte[] x = ccm.Serialize();

            int UserNameLength = BitConverter.ToInt32(x,0);
            string UserName =   Encoding.Unicode.GetString(x,sizeof(int),UserNameLength);
            int []xc = { 1, 3, 4, 5 };

            Debug.WriteLine(UserNameLength);
            Debug.WriteLine(UserName);
            Debug.WriteLine(sizeof(int));
            


            //IPAddress ip = IPAddress.Parse("233");
            // TcpListener server = new TcpListener(ip,3333);

            // server.Start();

            // TcpClient client = server.AcceptTcpClient();
            //
             NetworkStream stream = client.GetStream();

            stream.Read()

        }
    }
}
