using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BitsBlog.CMS.Web.Models.ViewModels;
using System.Web.Security;
using BitsBlog.Authentication.MembershipProviders;
using BitsBlog.Authentication;
using BitsBlog.CMS.Web.MVCAuthorizeFilters;

namespace BitsBlog.CMS.Web.Controllers
{
    public class UserAccountController : BitsBaseController
    {
        public UserAccountController(): base()
        {
            //TODO should move this to the base
            //if (MembershipProvider == null)
               // MembershipProvider = (BitsBlogMembershipProvider)Membership.Providers["BitsBlogMembershipProvider"];
        }

        public ActionResult Logout()
        {
            //expire the form authent ticket and nuke current session
            BitsBlog.Authentication.Utils.AuthUtils.Logout();

            return RedirectToAction("index", "admin");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model)
        {
            ViewBag.ErrorMessage = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    //validate user
                    bool isValid = MembershipProvider.ValidateUser(model.Username, model.Password);

                    if (isValid && Session[AuthSessionTags.AUTH_USER_BLOB] != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Username, false);
                        return RedirectToAction("index", "dashboard");
                    }

                    ViewBag.ErrorMessage = "The username or password you entered is not correct. Please try again.";
                }
                catch (ArgumentNullException e)
                {
                    //TODO Log error
                    ViewBag.ErrorMessage = "There was a problem logging in. Please contact administrator.";
                }
            }

            return RedirectToAction("InvalidLogin", "admin");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CreateUser()
        {
            return View("Create",new UserViewModel());
        }

        [HttpPost]
        public ActionResult CreateUser(UserViewModel model)
        {
            PasswordValidationStatus validationStatus;
            if (!ValidatePassword(model.PasswordModel.CurrentPassword, model.PasswordModel.ComparePassword, out validationStatus))
            {
                ViewBag.ErrorMessage = GetInvaidStatusErrorMessage(validationStatus);
                return View("Create", model);
            }

            MembershipCreateStatus createStatus;
            BitsBlogMembershipUser membershipUser = null;
            ViewBag.SuccessMessage = string.Empty;
            ViewBag.ErrorMessage = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    //create new user
                    membershipUser = MembershipProvider.CreateUser(model.Username,
                        model.PasswordModel.CurrentPassword,
                        model.EmailAddress,
                        model.FirstName,
                        model.LastName,
                        model.Role,
                        out createStatus,
                        model.MiddleName,
                        model.DisplayName);

                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        //persist new user data, so that the data will persit to the display of the update view.
                        TempData["NewMembershipUser"] = membershipUser;

                        //redirect to the update view
                        return RedirectToAction("UpdateUser", "UserAccount", new { id = membershipUser.ProviderUserKey });
                    }
                    else if (createStatus == MembershipCreateStatus.DuplicateUserName)
                        ViewBag.ErrorMessage = "The username " + model.Username + " already exists. Please enter another username.";
                    else
                        ViewBag.ErrorMessage = "Ooops this user was not created. Please contact the site administrator.";
                }
                catch (ArgumentNullException e)
                {
                    //TODO log or handle exception
                    ViewBag.ErrorMessage = "Ooops this user was not created due to a system error. Please contact the site administrator.";
                }
            }

            return View("Create", model);
        }

        [HttpPost]
        public ActionResult UpdateUser(UserViewModel model)
        {
            ViewBag.SuccessMessage = string.Empty;
            ViewBag.ErrorMessage = string.Empty;

            //todo add need validation etc.
            if (ModelState.IsValid)
            {
                try
                {
                    BitsBlogMembershipUser mu = BuildMembershipUserFromMViewModel(model);
                    MembershipProvider.UpdateUser(mu);

                    ViewBag.SuccessMessage = "User successfully updated!";
                }
                catch (Exception e)
                {
                    //TODO log or handle exception
                    ViewBag.ErrorMessage = "User was not successfully updated. Please check form fields or contact administrator.";
                }
            }

            return View("Update", model);
        }

        [HttpGet]
        public ActionResult UpdateUser(int id)
        {
            var newUser = TempData["NewMembershipUser"] as BitsBlogMembershipUser;
            UserViewModel viewModel = null;

            if (newUser != null)
            {
                //if we are here then a new user was just created...persit new user data.
                viewModel = BuildViewModelFromMembershipUser(newUser);

                //add success messaage
                ViewBag.SuccessMessage = "User successfully created!";
            }
            else if (id > 0)
            {
                BitsBlogMembershipUser membershipUser = MembershipProvider.GetUser(id,true) as BitsBlogMembershipUser;
                viewModel = membershipUser != null ? BuildViewModelFromMembershipUser(membershipUser) : null;
            }

            if (viewModel == null)
            {
                ViewBag.ErrorMessage = "There was no user to update!";
                return RedirectToAction("CreateUser", "UserAccount");
            }
            else
            {
                return View("Update", viewModel);
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(PasswordViewModel model)
        {
            //validate passwords
            PasswordValidationStatus validationStatus;
            bool validationFailed = false;

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "You must enter your current password.";
                validationFailed = true;
            }
            else if (!ValidatePassword(model.NewPassowrd, model.ComparePassword, out validationStatus))
            {
                ViewBag.ErrorMessage = GetInvaidStatusErrorMessage(validationStatus);
                validationFailed = true;
            }

            try
            {
                //if validation was successful, perform the update
                bool isUpdated = false;
                
                if (validationFailed == false && Session[AuthSessionTags.AUTH_USER_BLOB] != null)
                {
                    AuthUser authUser = Session[AuthSessionTags.AUTH_USER_BLOB] as AuthUser;
                    isUpdated = authUser != null ? MembershipProvider.ChangePassword(authUser.username, model.CurrentPassword, model.NewPassowrd) : false;

                    if (isUpdated) 
                        ViewBag.SuccessMessage = "Password successfully changed!";
                    else
                        ViewBag.ErrorMessage = "Your password was not successfully updated. Please check form fields or contact administrator.";
                }
            }
            catch (Exception e)
            {
                //TODO log / handle exception
                ViewBag.ErrorMessage = "Your password was not successfully updated due to a system error. Please check form fields or contact administrator.";
            }
     
            return View("ChangePassword", model);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View("ChangePassword", new PasswordViewModel());
        }

        [HttpPost]
        public ActionResult RegenerateAPIKeys(APIKeyModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if validation was successful, perform the update
                    bool isUpdated = false;

                    if (Session[AuthSessionTags.AUTH_USER_BLOB] != null)
                    {
                        AuthUser authUser = Session[AuthSessionTags.AUTH_USER_BLOB] as AuthUser;
                        string accessKey = null;
                        string secreteKey = null;
                        isUpdated = authUser != null ? MembershipProvider.GenerateNewAPIKeys(authUser.username, model.CurrentPassword, out accessKey, out secreteKey) : false;

                        if (isUpdated)
                        {
                            ViewBag.SuccessMessage = "API keys successfully updated.";
                            model.SecretKey = secreteKey;
                            model.AccessKey = accessKey;
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Your keys were not successfully updated. Please check form fields or contact administrator.";
                        }
                    }
                }
                catch (Exception e)
                {
                    //TODO log / handle exception
                    ViewBag.ErrorMessage = "Your keys were not successfully updated due to a system error. Please check form fields or contact administrator.";
                }
            }

            return View("RegenerateKeys", model);
        }

        [HttpGet]
        public ActionResult RegenerateAPIKeys()
        {
            return View("RegenerateKeys", new APIKeyModel());
        }

        //TODO should maybe move this to the view model as a static helper?
        private UserViewModel BuildViewModelFromMembershipUser(BitsBlogMembershipUser membershipUser)
        {
            UserViewModel viewModel = new UserViewModel();
            viewModel.Username = membershipUser.UserName;
            viewModel.FirstName = membershipUser.Name.FirstName;
            viewModel.LastName = membershipUser.Name.LastName;
            viewModel.MiddleName = membershipUser.Name.MiddleName;
            viewModel.Role = membershipUser.Role;
            viewModel.PasswordModel.CurrentPassword = string.Empty;//do not display or return real password... .net will not display this anyway because of the view model password field decoration
            viewModel.Id = Convert.ToInt32(membershipUser.ProviderUserKey);
            viewModel.EmailAddress = membershipUser.Email;
            viewModel.UniqueKey = membershipUser.SecreteKey;
            viewModel.AccessKey = membershipUser.AccessKey;

            return viewModel;
        }

        //TODO should maybe move this to the view model as a static helper?
        private BitsBlogMembershipUser BuildMembershipUserFromMViewModel(UserViewModel model)
        {
            Core.Name fullName = new Core.Name(model.FirstName, model.LastName, model.MiddleName, model.DisplayName);
            BitsBlogMembershipUser mu = new BitsBlogMembershipUser(MembershipProviderName, model.Username, model.Id, model.PasswordModel.CurrentPassword, model.EmailAddress, true, DateTime.Now, fullName, model.Role, model.AccessKey, model.UniqueKey);

            return mu;
        }

        private bool ValidatePassword(string password, string comparePassword, out PasswordValidationStatus validationStatus)
        {
            bool isValid = false;

            if (BitsBlogMembershipProvider.ValidatePasswords(password, comparePassword, out validationStatus))
                isValid = true;

            return isValid;
        }

        private string GetInvaidStatusErrorMessage(PasswordValidationStatus validationStatus)
        {
            string message = string.Empty;

            if (validationStatus == PasswordValidationStatus.PasswordsDoNotMatch)
                message = "Passowrds do not match. Please enter them again.";
            else if (validationStatus == PasswordValidationStatus.PasswordLengthIsInvalid)
                message = "Passowrd(s) are not the proper length, they must be 10 characters long.";
            else if (validationStatus == PasswordValidationStatus.PasswordFailedRulesCheck)
                message = "Passowrd(s) must contain at least one capital letter, at least one special character and at least one number.";

            return message;
        }
    }
}
