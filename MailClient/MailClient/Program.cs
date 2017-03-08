using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace MailClient {
    class MailClient {

        SslStream sslStream;
        MailClient(SslStream sslStream) {
            this.sslStream = sslStream;
        }

        public string sslRead() {

            byte[] buffer = new byte[1024];
            string data = null;
            while (sslStream.Read(buffer, 0, buffer.Length) != 0) {
                data = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                if (data.IndexOf("<EOF>") != 0) {
                    break;
                }
            }
            return data;
        }

        public void sslWrite(string msg, bool auth = false) {
            if (auth == false) {
                byte[] command = Encoding.ASCII.GetBytes(msg);
                sslStream.Write(command);
            }
            else {
                byte[] stringBytes = Encoding.ASCII.GetBytes(msg);
                string base64 = Convert.ToBase64String(stringBytes);
                string authentiction = "AUTH PLAIN " + base64 + "\r\n";
                byte[] base64Bytes = Encoding.ASCII.GetBytes(authentiction);
                sslStream.Write(base64Bytes);
            }
        }

        /*public static string toBase64Bytes(string msg) {
            byte[] stringBytes = Encoding.UTF8.GetBytes(msg);
            string base64 = Convert.ToBase64String(stringBytes);
            byte[] base64Bytes = Encoding.ASCII.GetBytes(base64);
            return base64 ;
        }*/


        static void Main(string[] args) {
            TcpClient mail = new TcpClient();
            
            string pass = "alialrawi96@gmail.com";
            string passs = "";
            

             mail.Connect("smtp.gmail.com", 465);
             SslStream sslStream = new SslStream(mail.GetStream());
             sslStream.AuthenticateAsClient("smtp.gmail.com");
             MailClient sslMail = new MailClient(sslStream);
             Console.WriteLine(sslMail.sslRead());
             sslMail.sslWrite("EHLO ali \n");
             Console.WriteLine(sslMail.sslRead());

             //sslMail.sslWrite("AUTH PLAIN ");
             
            string b64 = "\x00" + pass + "\x00" + passs;
            sslMail.sslWrite(b64,true);
            Console.WriteLine(sslMail.sslRead());


            /*sslMail.sslWrite("MAIL FROM: <alialrawi96@gmail.com>\r\n");
             Console.WriteLine(sslMail.sslRead());
             sslMail.sslWrite("RCPT TO: <alialrawip@gmail.com> \r\n");
             Console.WriteLine(sslMail.sslRead());
             sslMail.sslWrite("DATA\r\n");
             Console.WriteLine(sslMail.sslRead());*/


        }

    }   
}
