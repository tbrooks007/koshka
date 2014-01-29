using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Core.Interfaces;

namespace BitsBlog.Core
{
    public sealed class BlogPost : IDocument
    {
        #region Properties

        public int BlogId { get; set; }

        public string  BlogName { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Body { get; set; }

        public Structs.Author Author { get; set; }

        #endregion
    }
}
