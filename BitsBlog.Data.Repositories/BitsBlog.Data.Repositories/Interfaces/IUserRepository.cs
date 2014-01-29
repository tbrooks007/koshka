using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Core;

namespace BitsBlog.Data.Repositories.Interfaces
{
    internal interface IUserRepository : IRepository<User>
    {
        void GetUserInfoForValidationByUsername(string username, out int id, out string salt, out string roleName, out string hash);
        bool CheckUsernameExist(string username);
        bool UpdatePassword(int userId, string newPassword, string newSalt);
        bool UpdateAPIKeys(int userId, string newAccessKey, string newSecreteKey);
    }
}
