using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Core.Structs;

namespace BitsBlog.Core.Interfaces
{
    public interface IDocument
    {
        #region Properties

        int Id { get; set; }
        string Title { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        string Body { get; set; }
        Author Author { get; set; }

        #endregion
    }
}
