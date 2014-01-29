using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using BitsBlog.Authentication.Structs;
using System.Web.Security;
using System.Web;

namespace BitsBlog.Authentication.Utils
{
    public class AuthUtils
    {
        public static string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        /// <summary>
        /// Generates a symetric key based on the salt, and password
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="password"></param>
        /// <param name="interations"></param>
        public static SymmetricKey? GenerateSymetricKey(string saltBase64, string password, int interations = 1000)
        {
            //TODO add param validation

            SymmetricKey? key = null;

            try
            {
                byte[] saltBytes = Convert.FromBase64String(saltBase64);

                Rfc2898DeriveBytes rfcDerived = new Rfc2898DeriveBytes(password, saltBytes, interations);
                string ivString = Convert.ToBase64String(rfcDerived.GetBytes(16)); //128 bits
                string keyString = Convert.ToBase64String(rfcDerived.GetBytes(32)); //256 bits

                //key(secrete key) and IV(access)
                key = new SymmetricKey(ivString, keyString);
            }
            catch (Exception e)
            {
                //todo log error
            }

            return key;
        }

        public static string GenerateSHA1HashedString(string strToBeHashed, string salt)
        {
            //TODO add param validation

            string saltAndString = String.Concat(strToBeHashed, salt);
            string hashedString = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndString, "sha1");

            return hashedString;
        }

        public static string GenerateSHA256HashedString(string strToBeHashed, string salt)
        {
            string saltAndString = String.Concat(strToBeHashed, salt);
            byte[] saltStringBytes = UTF8Encoding.UTF8.GetBytes(saltAndString);
            
            SHA256 shaM = new SHA256Managed();
            string hashedString = BitConverter.ToString(shaM.ComputeHash(saltStringBytes)); //Convert.ToBase64String() - use this

            return hashedString;
        }

        public static bool ValidateHash(string stringToValidate, string salt, string hashToCompare)
        {
            string hashedString = GenerateSHA1HashedString(stringToValidate, salt);//GenerateSHA256HashedString(stringToValidate, salt);//GenerateSHA1HashedString(stringToValidate, salt);//GenerateSHA256HashedString(stringToValidate, salt);
            bool isValid = false;

            if (hashedString.Equals(hashToCompare, StringComparison.InvariantCultureIgnoreCase))
                isValid = true;

            return isValid;
        }

        public static string GetUserIpAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!String.IsNullOrEmpty(ipAddress))
                return ipAddress;
            else
                return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public static void Logout()
        {
            /*
             * due to a bug in forms authentication where the ticket can still be used
             * as it isn't expired when you call FormsAuthentication.SignOut() we need to expire the cookie...
             * otherwise the ticket could be stolen and misused...
             */

            if (System.Web.HttpContext.Current != null && 
                System.Web.HttpContext.Current.Request.IsAuthenticated && 
                System.Web.HttpContext.Current.Session[AuthSessionTags.AUTH_USER_BLOB] != null)
            {
                System.Web.HttpContext currContext = System.Web.HttpContext.Current;
                AuthUser currUser = System.Web.HttpContext.Current.Session[AuthSessionTags.AUTH_USER_BLOB] as AuthUser;
                HttpCookie authenCookie = FormsAuthentication.GetAuthCookie(currUser.username, false);
                authenCookie.Expires = new DateTime(1999, 10, 12);
                currContext.Response.Cookies.Add(authenCookie);

                //clear session keys
                System.Web.HttpContext.Current.Session.Clear();

                //cancel the session
                System.Web.HttpContext.Current.Session.Abandon();
            }
        }
    }
}
