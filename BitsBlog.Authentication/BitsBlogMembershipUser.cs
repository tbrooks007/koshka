using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using BitsBlog.Core;
using BitsBlog.Core.Interfaces;

namespace BitsBlog.Authentication
{
    public class BitsBlogMembershipUser : MembershipUser
    {
        public Name Name { get; set; }
        public string Role { get; set; }
        public string AccessKey { get; set; }
        public string SecreteKey { get; set; }
        public string Password { get; set; }

        private BitsBlogMembershipUser() { }

        public BitsBlogMembershipUser(string providerName, string username, int providerUserKey, string emailAddress, bool isActive, DateTime lastLoggedInDate) 
            : base(providerName,username,providerUserKey,emailAddress,null,null,isActive,false,DateTime.MinValue,lastLoggedInDate,DateTime.MinValue,DateTime.MinValue,DateTime.MinValue)
        {

        }

        public BitsBlogMembershipUser(string providerName, string username, int providerUserKey, string emailAddress, bool isActive, DateTime lastLoggedInDate, Name name, string role, string accessKey, string secretKey)
            : base(providerName, username, providerUserKey, emailAddress, null, null, isActive, false, DateTime.MinValue, lastLoggedInDate, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue)
        {
            this.Name = name;
            this.Role = role;
            this.AccessKey = accessKey;
            this.SecreteKey = secretKey;
        }

        public BitsBlogMembershipUser(string providerName, string username, int providerUserKey, string password, string emailAddress, bool isActive, DateTime lastLoggedInDate, Name name, string role, string accessKey, string secretKey)
            : base(providerName, username, providerUserKey, emailAddress, null, null, isActive, false, DateTime.MinValue, lastLoggedInDate, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue)
        {
            this.Name = name;
            this.Role = role;
            this.AccessKey = accessKey;
            this.SecreteKey = secretKey;
            this.Password = password;
        }
    }
}
