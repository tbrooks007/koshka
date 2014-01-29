using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitsBlog.Core.Interfaces
{
    public interface IComment
    {
        //TODO Add properites
        int BlogId { get; set; }
        int PostId { get; set; }
        bool IsUser { get; set; }
        int UserId { get; set; }
        DateTime DateCreated { get; set; }
        bool IsApproved { get; set; } // unless author(s) have to approve this is true by default in DB
        string DisplayName { get; set; }  // must have display name to leave comments.
        string EmailAddress { get; set; }
        string CommentBody { get; set; }
    }
}
