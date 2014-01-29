using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BitsBlog.CMS.Web.Models.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            PasswordModel = new PasswordViewModel();
        }

        [Required]
        public string Username { get; set; }

        public PasswordViewModel PasswordModel { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Role { get; set; }

        [Required(ErrorMessage="Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\S?([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$")]
        public string EmailAddress { get; set; }

        public string DisplayName { get; set; }

        public string AccessKey { get; set; }
        public string UniqueKey { get; set; }
        public int Id { get; set; }
    }
}