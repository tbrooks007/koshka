using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitsBlog.Authentication
{
    public enum PasswordValidationStatus
    {
        PasswordsDoNotMatch, PasswordLengthIsInvalid, PasswordFailedRulesCheck, PasswordValid
    }
}
