using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BitsBlog.CMS.Web.Models.ViewModels
{
    public class PasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        //[Required]
        [DataType(DataType.Password)]
        public string NewPassowrd { get; set; }

        //[Required(ErrorMessage="Passwords do not match.")]
        [DataType(DataType.Password)]
        //[Compare("Password")]
        public string ComparePassword { get; set; }

    }
}