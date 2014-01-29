using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Core;

namespace BitsBlog.Data.Repositories.Interfaces
{
    public interface IBlogPostRepository : IRepository<BlogPost>, IDocumentRepository<BlogPost>
    {
    }
}
