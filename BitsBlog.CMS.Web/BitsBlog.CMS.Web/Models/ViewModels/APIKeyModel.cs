using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace BitsBlog.CMS.Web.Models.ViewModels
{
    public class APIKeyModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}