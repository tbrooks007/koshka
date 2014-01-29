using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitsBlog.Core
{
    public class SystemSettings
    {
        public int NumberOfFailedLoginsAllowed {get; set;}
        public bool RequireCAPTCHAWhenLockedOut { get; set; }
        public bool RequireCAPTCHAOnLogin { get; set; }
        public string LogoImagePath { get; set; }
    }
}
