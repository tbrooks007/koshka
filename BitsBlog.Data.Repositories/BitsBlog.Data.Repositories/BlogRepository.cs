using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Data.Repositories.Bases;
using BitsBlog.Core.Interfaces;
using BitsBlog.Data.Interfaces;
using BitsBlog.Core;
using BitsBlog.Data.Repositories.Interfaces;

namespace BitsBlog.Data.Repositories
{
    public sealed class BlogRepository : RepositoryBase<Blog>, IBlogRepository
    {
        public BlogRepository(IDatabaseAccessAgent dbAccessAgent) : base(dbAccessAgent) { }

        public override Blog GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public override Blog GetByUUID(string UUID)
        {
            throw new NotImplementedException();
        }

        protected override bool Update(Blog entity)
        {
            throw new NotImplementedException();
        }

        protected override int Create(Blog entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Blog entity)
        {
            throw new NotImplementedException();
        }

        protected override Dictionary<string, object> GetStoredProcedureParametersForEntity(Blog entity)
        {
            return null;
        }

        protected override Dictionary<string, object> GetStoredProcedureParametersForEntityUpdate(Blog entity)
        {
            return null;
        }

        public override IList<Blog> GetAll()
        {
            throw new NotImplementedException();
        }

        public override IList<Blog> Get(int numberToRetrieve)
        {
            throw new NotImplementedException();
        }

        public override int Save(Blog entity)
        {
            throw new NotImplementedException();
        }
    }
}
