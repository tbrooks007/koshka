using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitsBlog.Core.Structs
{
    public struct Author
    {
        #region Properties

        public int UserId;
        public string FirstName;
        public string MiddleName;
        public string LastName;
        public bool IsSelected;

        // if no display name entered use first, last and middle name
        public string DisplayName { get; private set; } 

        #endregion

        public Author(int userId, string firstName, string lastName, string middleName, string displayName) : this()
        {
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.MiddleName = middleName;
            this.DisplayName = displayName;
        }

        public Author(int userId, string firstName, string lastName, string middleName, string displayName, bool isSelected): this()
        {
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.MiddleName = middleName;
            this.DisplayName = displayName;
            this.IsSelected = isSelected;
        }
    }
}
