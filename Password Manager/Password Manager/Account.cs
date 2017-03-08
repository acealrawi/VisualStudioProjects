using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager {


    class Account {

        private string email;
        private string password;
        private string username;

        public Account(string email, string username, string password) {
            this.email = email;
            this.username = username;
            this.password = password;
        }

        public Account(string email, string password) {
            this.email = email;
            this.password = password;
        }





        static void Main(string[] args) {


        }
    }
}
