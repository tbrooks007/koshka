using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Core.Enums;
using System.Security;

namespace BitsBlog.Core.Interfaces
{
    public interface IUser
    {
        int Id { get; }
        Guid UUID { get;  }
        string Username { get; }
        string Password { get; }
        string AccessKey { get; } //IV
        string UniqueSecretKey { get; } //Key
        string Salt { get; }
        string EmailAddress { get; }
        Name Name { get; set; }
        string Role { get;}
        DateTime DateCreated { get;}
        DateTime DateUpdated { get; }
        DateTime DateLastLoggedIn { get; }
        bool IsActive { get; }

        //string ProfilePicturePath { get; set; }
    }
}
