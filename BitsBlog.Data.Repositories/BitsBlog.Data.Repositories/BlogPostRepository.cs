using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Data.Repositories.Bases;
using BitsBlog.Core.Interfaces;
using BitsBlog.Data.Interfaces;
using BitsBlog.Data.Repositories.Interfaces;
using BitsBlog.Core;

namespace BitsBlog.Data.Repositories
{
    public sealed class BlogPostRepository : RepositoryBase<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(IDatabaseAccessAgent dbAccessAgent) : base(dbAccessAgent) { }

        public override BlogPost GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public override BlogPost GetByUUID(string UUID)
        {
            throw new NotImplementedException();
        }

        protected override bool Update(BlogPost entity)
        {
            throw new NotImplementedException();
        }

        protected override int Create(BlogPost entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(BlogPost entity)
        {
            throw new NotImplementedException();
        }

        public override IList<BlogPost> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IList<BlogPost> Get(int numberToRetrieve)
        {
            throw new NotImplementedException();
        }

        public override int Save(BlogPost entity)
        {
            throw new NotImplementedException();
        }

        public List<BlogPost> GetAllByAuthor(int Id)
        {
            throw new NotImplementedException();
        }

        public List<BlogPost> GetByYear(int year)
        {
            throw new NotImplementedException();
        }

        public List<BlogPost> GetByYearAndMonth(int month, int year)
        {
            throw new NotImplementedException();
        }

        protected override Dictionary<string, object> GetStoredProcedureParametersForEntity(BlogPost entity)
        {
            return null;
        }

        protected override Dictionary<string, object> GetStoredProcedureParametersForEntityUpdate(BlogPost entity)
        {
            return null;
        }
    }
}
