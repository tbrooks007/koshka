using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Data.Repositories.Bases;
using BitsBlog.Core.Interfaces;
using BitsBlog.Data.Interfaces;
using BitsBlog.Data.Repositories.Interfaces;
using BitsBlog.Core;
using System.Data;

namespace BitsBlog.Data.Repositories
{
    internal sealed class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDatabaseAccessAgent dbAccessAgent) : base(dbAccessAgent) { }

        /// <summary>
        /// Gets a user by a given UserId.
        /// </summary>
        /// <param name="Id">userId</param>
        /// <returns>User, returns null if the user could not be found.</returns>
        public override User GetById(int Id)
        {
            User returnUser = null;
            Dictionary<string, object> parameters = null;
            Dictionary<string, object> resultSet = null;

            if (Id > 0)
            {
                parameters = new Dictionary<string, object>();
                parameters.Add("@userId", Id);
                resultSet = Agent.ExecuteReader("spGetUserById", parameters);

                if (resultSet != null)
                    GetUserFromResultSet(resultSet, out returnUser);
            }

            return returnUser;
        }

        /// <summary>
        /// Updates a given user.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>bool - true if user was updated successfully, false otherwise</returns>
        /// <exception cref="ArgumentNullException">When parameters are in valid.</exception>
        protected override bool Update(User entity)
        {
            if (entity == null) throw new ArgumentNullException("entity", "The entity parameter can not be null.");
            
            Dictionary<string, object> parameters = null;
            bool isUpdateSuccess = false;

            try
            {
                parameters = GetStoredProcedureParametersForEntityUpdate(entity);
                isUpdateSuccess = Agent.ExecuteNonQuery("spUpdateUser", parameters);
            }
            catch (Exception e)
            {
                //todo handel / log error
            }

            return isUpdateSuccess;
        }

        /// <summary>
        /// Creates a new User.
        /// </summary>
        /// <param name="entity">User</param>
        /// <returns>int - newly created user's Id</returns>
        /// <exception cref="ArgumentNullException">When parameters are in valid.</exception>
        protected override int Create(User entity)
        {
            if (entity == null) throw new ArgumentNullException("entity","The entity parameter can not be null.");

            Dictionary<string, object> parameters = null;
            Dictionary<string, object> resultSet = null;
            int returnId = 0;

            parameters = this.GetStoredProcedureParametersForEntity(entity);
            resultSet = Agent.ExecuteReader("spCreateUser", parameters); //TODO  eventually need a mapping class or config file so that we have one truth for this stuff.

            if (resultSet != null)
            {
                foreach (KeyValuePair<string, object> param in resultSet)
                {
                    returnId = Convert.ToInt32(param.Value);
                }
            }

            return returnId;
        }
        
        /// <summary>
        /// Save given User.
        /// </summary>
        /// <param name="entity">User</param>
        /// <returns>int - userId</returns>
        /// <exception cref="ArgumentNullException">When parameters are in valid.</exception>
        public override int Save(User entity)
        {
            if (entity == null) throw new ArgumentNullException("entity", "The entity parameter can not be null.");

            int userId = 0;

            if (entity.Id == 0)
            {
                userId = Create(entity);
            }
            else
            {
                if(Update(entity))
                    userId = entity.Id;
            }

            return userId;
        }

        /// <summary>
        /// Validates the user by username, returns the
        /// </summary>
        /// <param name="username"></param>
        /// <param name="id"></param>
        /// <param name="salt"></param>
        /// <param name="roleName"></param>
        /// <param name="hash"></param>
        /// <exception cref="ArgumentNullException">When parameters are in valid.</exception>
        public void GetUserInfoForValidationByUsername(string username, out int id, out string salt, out string roleName, out string hash)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException("username", "The username parameter can not be null or empty.");

            id = 0;
            salt = null;
            roleName = null;
            hash = null;
            Dictionary<string, object> parameters = null;
            Dictionary<string, object> resultSet = null;

            parameters = new Dictionary<string, object>();
            parameters.Add("@username", username);
            resultSet = Agent.ExecuteReader("spGetAuthUserInfoByUsername", parameters);

            if (resultSet != null)
            {
                try
                {
                    foreach (KeyValuePair<string, object> param in resultSet)
                    {
                        string key = param.Key;
                        string value = param.Value.ToString();

                        if (key.Equals("Id", StringComparison.InvariantCultureIgnoreCase)) id = Convert.ToInt32(value);
                        if (key.Equals("Salt", StringComparison.InvariantCultureIgnoreCase)) salt = value;
                        if (key.Equals("RoleName", StringComparison.InvariantCultureIgnoreCase)) roleName = value;
                        if (key.Equals("Hash", StringComparison.InvariantCultureIgnoreCase)) hash = value;
                    }
                }
                catch (Exception e)
                {
                    //TODO log error
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Check if username exist in the database.
        /// </summary>
        /// <param name="username">string</param>
        /// <returns>true if username exists already</returns>
        /// <exception cref="ArgumentNullException">When parameters are in valid.</exception>
        public bool CheckUsernameExist(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException("username", "The username parameter can not be null or empty.");

            bool doesExist = false;
            Dictionary<string, object> resultSet = null;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@username", username);
            resultSet = Agent.ExecuteReader("spCheckActiveUsersForUsername", parameters);

            if (resultSet != null)
            {
                try
                {
                    foreach (KeyValuePair<string, object> param in resultSet)
                    {
                        string key = param.Key;
                        string value = param.Value.ToString();

                        if (key.Equals("doesexist", StringComparison.InvariantCultureIgnoreCase)) doesExist = Convert.ToBoolean(value);
                    }
                }
                catch (Exception e)
                {
                    //TODO log error
                    Console.WriteLine(e.Message);
                }
            }

            return doesExist;
        }

        /// <summary>
        /// Update user password.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <param name="newSalt"></param>
        /// <returns>true, if password was updated successfully.</returns>
        /// <exception cref="ArgumentNullException">When parameters are in valid.</exception>
        public bool UpdatePassword(int userId, string newPassword, string newSalt)
        {
            if (string.IsNullOrWhiteSpace(newPassword)) throw new ArgumentNullException("newPassword", "The username parameter can not be null or empty.");
            if (string.IsNullOrWhiteSpace(newSalt)) throw new ArgumentNullException("newSalt", "The username parameter can not be null or empty.");
            if (userId <= 0) throw new ArgumentNullException("userId", "The userId parameter can not zero or negative.");

            bool isUpdateSuccess = false;

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@UserId", userId);
                parameters.Add("@Password", newPassword);
                parameters.Add("@Salt", newSalt);

                isUpdateSuccess = Agent.ExecuteNonQuery("spChangeUserPassword", parameters);
            }
            catch (Exception e)
            {
                //todo handel / log error
            }

            return isUpdateSuccess;
        }

        public bool UpdateAPIKeys(int userId, string newAccessKey, string newSecreteKey)
        {
            if (string.IsNullOrWhiteSpace(newAccessKey)) throw new ArgumentNullException("newAccessKey", "The newAccessKey parameter can not be null or empty.");
            if (string.IsNullOrWhiteSpace(newSecreteKey)) throw new ArgumentNullException("newSecreteKey", "The newSecreteKey parameter can not be null or empty.");
            if (userId <= 0) throw new ArgumentNullException("userId", "The userId parameter can not zero or negative.");

            bool isUpdateSuccess = false;

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@UserId", userId);
                parameters.Add("@AccessKey", newAccessKey);
                parameters.Add("@SecretKey", newSecreteKey);

                isUpdateSuccess = Agent.ExecuteNonQuery("spUpdateUserAPIKeys", parameters);
            }
            catch (Exception e)
            {
                //todo handel / log error
            }

            return isUpdateSuccess;

        }

        #region Helper Methods

        protected override Dictionary<string, object> GetStoredProcedureParametersForEntity(User entity)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UUID", entity.UUID.ToString());
            parameters.Add("@Username", entity.Username);
            parameters.Add("@Password", entity.Password);
            parameters.Add("@Salt", entity.Salt);
            parameters.Add("@Email", entity.EmailAddress);
            parameters.Add("@FirstName", entity.Name.FirstName);
            parameters.Add("@LastName", entity.Name.LastName);
            parameters.Add("@MiddleName", entity.Name.MiddleName);
            parameters.Add("@DisplayName", entity.Name.DisplayName);
            parameters.Add("@ApplicationName", entity.Application);
            parameters.Add("@RoleName", entity.Role);
            parameters.Add("@AccessKey", entity.AccessKey);
            parameters.Add("@SecretKey", entity.UniqueSecretKey);
            parameters.Add("@IsActive", entity.IsActive);

            return parameters;
        }

        protected override Dictionary<string, object> GetStoredProcedureParametersForEntityUpdate(User entity)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", entity.Id);
            parameters.Add("@Email", entity.EmailAddress);
            parameters.Add("@FirstName", entity.Name.FirstName);
            parameters.Add("@LastName", entity.Name.LastName);
            parameters.Add("@RoleName", entity.Role);
            parameters.Add("@IsActive", entity.IsActive);

            return parameters;
        }

        private void GetUserFromResultSet(Dictionary<string, object> resultSet, out User user)
        {
            user = null;

            if (resultSet != null && resultSet.Count > 0)
            {
                user = new User();
                user.Name = new Name(null,null,null,null);

                foreach (KeyValuePair<string, object> param in resultSet)
                {
                    string key = param.Key;

                    if (key.Equals("FirstName", StringComparison.InvariantCultureIgnoreCase)) user.Name.FirstName = param.Value as string;
                    else if (key.Equals("LastName", StringComparison.InvariantCultureIgnoreCase)) user.Name.LastName = param.Value as string;
                    else if (key.Equals("MiddleName", StringComparison.InvariantCultureIgnoreCase)) user.Name.MiddleName = param.Value as string;
                    else if (key.Equals("DisplayName", StringComparison.InvariantCultureIgnoreCase)) user.Name.DisplayName = param.Value as string;
                    else if (key.Equals("Username", StringComparison.InvariantCultureIgnoreCase)) user.Username = param.Value as string;
                    else if (key.Equals("Email", StringComparison.InvariantCultureIgnoreCase)) user.EmailAddress = param.Value as string;
                    else if (key.Equals("Salt", StringComparison.InvariantCultureIgnoreCase)) user.Salt = param.Value as string;
                    else if (key.Equals("RoleName", StringComparison.InvariantCultureIgnoreCase)) user.Role = param.Value as string;
                    else if (key.Equals("AccessKey", StringComparison.InvariantCultureIgnoreCase)) user.AccessKey = param.Value as string;
                    else if (key.Equals("SecretKey", StringComparison.InvariantCultureIgnoreCase)) user.UniqueSecretKey = param.Value as string;
                }
            }
        }

        #endregion

        #region Unimplemented Methods

        public override User GetByUUID(string UUID)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public override IList<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IList<User> Get(int numberToRetrieve)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
