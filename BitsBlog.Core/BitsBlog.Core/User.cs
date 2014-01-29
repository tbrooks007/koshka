using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Core.Interfaces;
using System.Security;

namespace BitsBlog.Core
{
    public sealed class User : IUser
    {
        #region Properties

        public int Id { get; set; }
        public Guid UUID { get; set; }
        /// <summary>
        /// Symetric key's initialization vector (base 64 string)
        /// </summary>
        public string AccessKey { get; set; }
        /// <summary>
        /// Symetric key's key (base 64 string)
        /// </summary>
        public string UniqueSecretKey { get; set; }
        public string Username{get;set;}
        public string Password { get; set; }
        /// <summary>
        /// Unique salt (base 64 string)
        /// </summary>
        public string Salt { get; internal set; }
        public string EmailAddress { get; set; }
        public Name Name { get; set; }
        public string Role { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateLastLoggedIn { get; set; }
        public string ProfilePicturePath { get; set; } //TODO remove this should be a system configured default path! (add to sys configs)
        public string Application { get; set; }
        public bool IsActive { get; set; }
        #endregion
    }
}
