using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitsBlog.Core
{
    public class Name
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }

        public Name(string firstName, string lastName, string middleName, string displayName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.MiddleName = middleName;
            this.DisplayName = displayName;
        }
    }
}
