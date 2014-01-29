using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Collections.Specialized;
using BitsBlog.Core;
using BitsBlog.Data.Repositories.Factories;
using BitsBlog.Authentication.Utils;
using BitsBlog.Security.Utilities;
using BitsBlog.Authentication.Structs;
using System.Web.Configuration;
using System.Configuration.Provider;

namespace BitsBlog.Authentication.MembershipProviders
{
    public sealed class BitsBlogMembershipProvider : CustomMembershipProviderBase, IMembershipCustomProvider
    {
        #region Fields

        private string _ApplicationName;
        private bool _EnablePasswordReset;
        //private bool _EnablePasswordRetrieval = false;
        //private bool _RequiresQuestionAndAnswer = false;
        //private bool _RequiresUniqueEmail = true;
        private int _MaxInvalidPasswordAttempts;
        private int _PasswordAttemptWindow;
        private int _MinRequiredPasswordLength;
        private int _MinRequiredNonalphanumericCharacters;
        private string _PasswordStrengthRegularExpression;
        //private MembershipPasswordFormat _PasswordFormat = MembershipPasswordFormat.Hashed;
        private RepsitoryFactory RepositoryFactory;
        private const int REQUIRED_PASSWORD_LENGTH = 10;  //TODO make length configrable in app rules when you finally created them :)
        #endregion

        #region Properties

        //public  string ApplicationName
        //{
        //    get { return _ApplicationName; }
        //    set { _ApplicationName = value; }
        //}

        //public  bool EnablePasswordReset
        //{
        //    get { return _EnablePasswordReset; }
        //}

        //public  bool EnablePasswordRetrieval
        //{
        //    get { return _EnablePasswordRetrieval; }
        //}

        //public  int MaxInvalidPasswordAttempts
        //{
        //    get { return _MaxInvalidPasswordAttempts; }
        //}

        //public  int MinRequiredNonAlphanumericCharacters
        //{
        //    get { return _MinRequiredNonalphanumericCharacters; }
        //}

        //public  int MinRequiredPasswordLength
        //{
        //    get { return _MinRequiredPasswordLength; }
        //}

        //public  int PasswordAttemptWindow
        //{
        //    get { return _PasswordAttemptWindow; }
        //}

        //public  MembershipPasswordFormat PasswordFormat
        //{
        //    get { return _PasswordFormat; }
        //}

        //public  string PasswordStrengthRegularExpression
        //{
        //    get { return _PasswordStrengthRegularExpression; }
        //}

        //public  bool RequiresQuestionAndAnswer
        //{
        //    get { return _RequiresQuestionAndAnswer; }
        //}

        //public  bool RequiresUniqueEmail
        //{
        //    get { return _RequiresUniqueEmail; }
        //}

        #endregion

        public override void Initialize(string name, NameValueCollection config)
        {
            // instanciate new instance of repository
            RepositoryFactory = new RepsitoryFactory();

            //set properties to configured values for the membership provider
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "BitsBlogMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", @"Stores and retrieves membership data from the local database. This is a generic provider as it 
                            uses the User Respository to do actual CRUD operations. The data access agents are injected into the repositories 
                            and the type of data access agent used is determined then, hence the generic provider.");
            }

            base.Initialize(name, config);

            _ApplicationName = AuthUtils.GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            _MaxInvalidPasswordAttempts = Convert.ToInt32(AuthUtils.GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            _PasswordAttemptWindow = Convert.ToInt32(AuthUtils.GetConfigValue(config["passwordAttemptWindow"], "10"));
            _MinRequiredNonalphanumericCharacters = Convert.ToInt32(AuthUtils.GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            _MinRequiredPasswordLength = Convert.ToInt32(AuthUtils.GetConfigValue(config["minRequiredPasswordLength"], "6"));
            _EnablePasswordReset = Convert.ToBoolean(AuthUtils.GetConfigValue(config["enablePasswordReset"], "true"));
            _PasswordStrengthRegularExpression = Convert.ToString(AuthUtils.GetConfigValue(config["passwordStrengthRegularExpression"], ""));
        }

        /// <summary>
        /// Updates a limited set up user information.
        /// </summary>
        /// <param name="user">user to be updated</param>
        /// <exception cref="ArgumentNullException">When parameters are in valid.</exception>
        /// <exception cref="ProviderException">When the given user could not be updated.</exception>
        public override void UpdateUser(MembershipUser user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var userRepository = RepositoryFactory.CreateRepository<BitsBlog.Data.Repositories.Interfaces.IUserRepository>();
            var membershipUser = user as BitsBlogMembershipUser;
            int userId = 0;
            User userToUpdate = new User();

            try
            {
                //set the id
                userToUpdate.Id = Convert.ToInt32(membershipUser.ProviderUserKey);
            }
            catch(Exception e)
            {
                //TODO  log error / handle error
                //TODO make this a constant value (hard coding bad)
                userToUpdate.Id = -1; 
            }

            //get user's email address
            userToUpdate.EmailAddress = membershipUser.Email;

            //get user's name
            userToUpdate.Name = membershipUser.Name;

            //set users role
            userToUpdate.Role = membershipUser.Role; //TODO based on role, we should have a check to see if they can actually change their role (some work with view needed for this)

            userToUpdate.IsActive = true; //TOOD update this when we have a means to deactiveate users in the UI

            //save new user
            userId = userToUpdate.Id > 0 ? userRepository.Save(userToUpdate) : 0;

            if (userId == -1)
                throw new ProviderException("Unable to update user.");
        }

        /// <summary>
        /// Generates new API keys for existing, valid user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="newAccessKey"></param>
        /// <param name="newSecreteKey"></param>
        public bool GenerateNewAPIKeys(string username, string password, out string newAccessKey, out string newSecreteKey)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException("username", "The username parameter can not be null or empty.");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password", "The password parameter can not be null or empty.");

            newAccessKey = string.Empty;
            newSecreteKey = string.Empty;
            bool generatedKeysSuccessfully = false;

            if (ValidateUser(username, password, false))
            {
                AuthUser authUser = System.Web.HttpContext.Current.Session[AuthSessionTags.AUTH_USER_BLOB] as AuthUser;
                var userRepository = RepositoryFactory.CreateRepository<BitsBlog.Data.Repositories.Interfaces.IUserRepository>();

                string salt = Randomizer.GenerateRandomSalt(64); //generate new salt, no need to generate new hash as the password isn't new
                SymmetricKey? key = AuthUtils.GenerateSymetricKey(salt, password);
                newAccessKey = key.Value.AccessKey;
                newSecreteKey = key.Value.SecreteKey;

                //dave new API keys
                generatedKeysSuccessfully = userRepository.UpdateAPIKeys(authUser.Id, newAccessKey, newSecreteKey);
            }

            return generatedKeysSuccessfully;
        }

        public static bool ValidatePasswords(string password, string comparePassword, out PasswordValidationStatus validationStatus)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password", "The password parameter can not be null or empty.");
            if (string.IsNullOrWhiteSpace(comparePassword)) throw new ArgumentNullException("comparePassword", "The comparePassword parameter can not be null or empty.");

            bool isValid = false;
            validationStatus = PasswordValidationStatus.PasswordValid;

            //TODO 1) check to see if the passwords are equale
            //     2) check if the pattern fits the rules of the app...these rules should be stored in the db or web.config _passwordStrengthRegularExpression
            //     3) check if the password is 10 characters long
            // ** rule: Password must be 10 characters long, must contain at least one capital letter, must contain letters and numbers and at least one special character.

            //check if these two passwords are equale
            if (password.Equals(comparePassword, StringComparison.InvariantCultureIgnoreCase) &&
                IsPasswordsRequiredLength(password) &&
                IsPasswordRequiredStrength(password))
            {
                isValid = true;
            }
            else
            {
                if (!password.Equals(comparePassword, StringComparison.InvariantCultureIgnoreCase)) validationStatus = PasswordValidationStatus.PasswordsDoNotMatch;
                if (!IsPasswordsRequiredLength(password)) validationStatus = PasswordValidationStatus.PasswordLengthIsInvalid;
                if (!IsPasswordRequiredStrength(password)) validationStatus = PasswordValidationStatus.PasswordFailedRulesCheck;
            }
          
            return isValid;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassowrd)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException("username", "The username parameter can not be null or empty.");
            if (string.IsNullOrWhiteSpace(oldPassword)) throw new ArgumentNullException("oldPassword", "The oldPassword parameter can not be null or empty.");
            if (string.IsNullOrWhiteSpace(newPassowrd)) throw new ArgumentNullException("newPassowrd", "The newPassword parameter can not be null or empty.");

            bool isUpdated = false;

            //validate user before updating the password
            if (ValidateUser(username, oldPassword, false))
            {
                AuthUser authUser = System.Web.HttpContext.Current.Session[AuthSessionTags.AUTH_USER_BLOB] as AuthUser;
                var userRepository = RepositoryFactory.CreateRepository<BitsBlog.Data.Repositories.Interfaces.IUserRepository>();

                if (userRepository != null)
                {
                    //generate new salt for the new password
                    string salt = Randomizer.GenerateRandomSalt(64);

                    //generate salted & hashed version of password
                    string password = AuthUtils.GenerateSHA1HashedString(newPassowrd, salt);

                    //password is to be updated, keep the same keys based on the old password unless the user requests the keys to be regenerated...
                    isUpdated = userRepository.UpdatePassword(authUser.Id, password, salt);
                }
            }

            return isUpdated;
        }
            
        /// <summary>
        /// Validates a given user's credientals.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>true if the user is valid</returns>
        /// <exception cref="ArgumentNullException">When parameters are in valid.</exception>
        new public bool ValidateUser(string username, string password, bool createNewSession = true)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException("username", "The username parameter can not be null or empty.");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password", "The password parameter can not be null or empty.");

            bool validUserFound = false;
            var userRepository = RepositoryFactory.CreateRepository<BitsBlog.Data.Repositories.Interfaces.IUserRepository>();
            int id = 0;
            string salt = null;
            string hash = null;
            string roleName = null;

            if (userRepository != null)
            {
                userRepository.GetUserInfoForValidationByUsername(username, out id, out salt, out roleName, out hash);

                if (id != 0 && salt != null && roleName != null && hash != null)
                {
                    if (AuthUtils.ValidateHash(password, salt, hash))
                    {
                        validUserFound = true;

                        //save auth user blob to session
                        if (createNewSession)
                        {
                            //TODO when logging is implemented make sure to log this session info or create session log table to record who logged in and logged out when.
                            string currAuthUserIpAddress = AuthUtils.GetUserIpAddress();
                            AuthUser authUserBlob = new AuthUser(roleName, id, currAuthUserIpAddress, username);
                            System.Web.HttpContext.Current.Session[AuthSessionTags.AUTH_USER_BLOB] = authUserBlob;  //TODO if you add anything more senstive to this may want encrypt
                        }
                    }
                }
            }

            return validUserFound;
        }

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="role"></param>
        /// <param name="status"></param>
        /// <param name="middleName"></param>
        /// <param name="displayName"></param>
        /// <returns>BitsBlogMembershipUser if user was successfully created, otherwise returns null</returns>
        /// <exception cref="ArgumentNullException">When parameters are in valid.</exception>
        public BitsBlogMembershipUser CreateUser(string username, string password, string email, string firstName, string lastName, string role, out MembershipCreateStatus status, string middleName = null, string displayName = null)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException("username", "The username parameter can not be null or empty.");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password", "The password parameter can not be null or empty.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("email", "The email parameter can not be null or empty.");
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException("firstName", "The firstName parameter can not be null or empty.");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException("lastName", "The lastName parameter can not be null or empty.");
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentNullException("role", "The role parameter can not be null or empty.");

            var userRepository = RepositoryFactory.CreateRepository<BitsBlog.Data.Repositories.Interfaces.IUserRepository>();

            //check if username exists
            if (userRepository.CheckUsernameExist(username))
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            //if the username doesn't exist, create new user
            int userId = 0;
            BitsBlogMembershipUser membershipUser = null;
            status = MembershipCreateStatus.UserRejected;

            if (userRepository != null)
            {
                //create new User object
                User newUser = new User();

                //set username and email address
                newUser.Username = username;
                newUser.EmailAddress = email;

                //get user's name
                newUser.Name = new Name(firstName, lastName, middleName, displayName);

                //set users role
                newUser.Role = role;

                //set application name
                newUser.Application = _ApplicationName; // todo eventually replace this with an application UUID?

                //generate salt for user
                newUser.Salt = Randomizer.GenerateRandomSalt(64);

                //generate salted & hashed version of password
                newUser.Password = AuthUtils.GenerateSHA1HashedString(password, newUser.Salt);

                //generate crypto GUID
                newUser.UUID = Randomizer.GenerateCryptoUUID();

                //generate key(secrete key) and IV(access), FYI these are stored in an encrypted colum in the database
                SymmetricKey? key = AuthUtils.GenerateSymetricKey(newUser.Salt, password); //TODO  we may want to use something like the GUID that won't change to generate these?
                newUser.AccessKey = key.Value.AccessKey;
                newUser.UniqueSecretKey = key.Value.SecreteKey;

                //new users are active by default (may want to add more validations for new users later ie email validation)
                newUser.IsActive = true;

                //save new user
                userId = userRepository.Save(newUser);

                //then get and return MembershipUser if we saved the user successful. 
                if (userId > 0)
                {
                    status = MembershipCreateStatus.Success;
                    membershipUser = new BitsBlogMembershipUser(this.Name, username, userId, email, true, DateTime.Now, newUser.Name, newUser.Role, newUser.AccessKey, newUser.UniqueSecretKey);
                }
            }
            else
            {
                //TODO log to DB that we couldn't get a valid user repository
                status = MembershipCreateStatus.ProviderError;
            }

            return membershipUser;
        }

        /// <summary>
        /// Get user by provider user key.
        /// </summary>
        /// <param name="providerUserKey"></param>
        /// <param name="userIsOnline"></param>
        /// <returns>MembershipUser, otherwise returns null if unable to retrieve user by provider user key.</returns>
        /// <exception cref="ArgumentNullException">When parameters are in valid.</exception>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (providerUserKey == null) throw new ArgumentNullException("providerUserKey", "The providerUserKey parameter can not be null");

            BitsBlogMembershipUser membershipUser = null;

            try
            {    
                int userId = Convert.ToInt32(providerUserKey);

                if (userId > 0 && userIsOnline)
                {
                    var userRepository = RepositoryFactory.CreateRepository<BitsBlog.Data.Repositories.Interfaces.IUserRepository>();
                    User user = userRepository.GetById(userId);

                    if (user != null) 
                        membershipUser = new BitsBlogMembershipUser(this.Name, user.Username, userId, user.EmailAddress, true, DateTime.Now, user.Name, user.Role, user.AccessKey, user.UniqueSecretKey);
                }
            }
            catch (Exception e)
            {
                //TODO log error
                Console.WriteLine(e.Message);
            }

            return membershipUser;
        }

        #region helper methods

        private static bool IsPasswordsRequiredLength(string password)
        {
            bool isValidLength = false;

            if (!string.IsNullOrWhiteSpace(password))
            {
                if (password.Length == REQUIRED_PASSWORD_LENGTH)
                    isValidLength = true;
            }

            return isValidLength;
        }

        /// <summary>
        /// TODO implement this method!
        /// check if the pattern fits the rules of the app...these rules should be stored in the db or web.config _passwordStrengthRegularExpression
        /// </summary>
        /// <returns></returns>
        private static bool IsPasswordRequiredStrength(string password)
        {
            bool isValidStrength = false;

            if (!string.IsNullOrWhiteSpace(password))
            {
                isValidStrength = true;
            }

            return isValidStrength;
        }

        #endregion
    }
}
