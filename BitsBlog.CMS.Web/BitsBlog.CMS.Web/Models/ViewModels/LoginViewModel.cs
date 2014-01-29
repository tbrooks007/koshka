using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BitsBlog.CMS.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        //TODO Add other field requriements like field character count validation (max sizes)e
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //TODO add attempted log in counter...need a way to authenticate this counter against the system rules...could load this at app start and update when user updates this
    }
}