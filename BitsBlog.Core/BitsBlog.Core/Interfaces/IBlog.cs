using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Core.Structs;

namespace BitsBlog.Core.Interfaces
{
    public interface IBlog
    {
        int Id { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        string UserUUID { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }

        // by default the creator of the blog is automatically
        // an author...for blogs with mulipule authors, they will be sent an invite
        // to sign up, then they will appear in list of possible authors for that blog
        // must create blog first before inviting multipule authors.
        IList<Author> Authors { get; set; }

        //generated when blog is created, used by read only apps to ID the blog
        Guid BlogUUID { get; set; }

        // general settings: blog user comments
        bool AllowReaderComments { get; set; }
        bool MustApproveReaderComments { get; set; }
        bool MustBeUserToMakeComments { get; set; } // would need to have an account, and logged in.
        bool RequireNameField { get; set; }
        bool RequireEmailField { get; set; }
        bool RequireCAPTCHA { get; set; }
    }
}
