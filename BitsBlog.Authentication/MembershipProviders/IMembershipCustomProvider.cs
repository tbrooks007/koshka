using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using BitsBlog.Core;
using BitsBlog.Core.Interfaces;

namespace BitsBlog.Authentication
{
    public interface IMembershipCustomProvider
    {
        string ApplicationName { get; set; }
        bool EnablePasswordReset { get; }
        bool EnablePasswordRetrieval { get; }
        int MaxInvalidPasswordAttempts { get; }
        int MinRequiredNonAlphanumericCharacters { get; }
        int PasswordAttemptWindow { get; }
        MembershipPasswordFormat PasswordFormat { get; }
        string PasswordStrengthRegularExpression { get; }
        bool RequiresQuestionAndAnswer { get; }
        bool RequiresUniqueEmail { get; }
        //User CreateUser(string username, string password, string email, string firstName, string lastName, string role, out MembershipCreateStatus status, string middleName = null, string displayName = null);
        //User GetUser(int providerUserKey, bool userIsOnline);
        //bool UpdateUser(User user);
        bool ValidateUser(string username, string password, bool createNewSession = false);
    }
}
