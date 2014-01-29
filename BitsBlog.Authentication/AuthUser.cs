using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitsBlog.Authentication
{
    public class AuthUser
    {
        public string role;
        public int Id;
        public string ipAddress;
        public string username;

        private AuthUser() { }

        public AuthUser(string role, int id, string ipAddress, string username)
        {
            this.role = role;
            this.Id = id;
            this.ipAddress = ipAddress;
            this.username = username;
        }
    }
}
