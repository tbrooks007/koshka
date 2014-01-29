using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Core.Interfaces;
using BitsBlog.Data.Repositories.Interfaces;
using BitsBlog.Data.Interfaces;
using BitsBlog.Core;

namespace BitsBlog.Data.Repositories.Bases
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected IDatabaseAccessAgent Agent { get; private set; }

        public RepositoryBase(IDatabaseAccessAgent dbAccessAgent) 
        {
            this.Agent = dbAccessAgent;
        }

        public abstract T GetById(int Id);
        public abstract T GetByUUID(string UUID);
        protected abstract bool Update(T entity);
        protected abstract int Create(T entity);
        public abstract bool Delete(T entity);
        public abstract IList<T> GetAll();
        public abstract IList<T> Get(int numberToRetrieve);
        public abstract int Save(T entity);
        protected abstract Dictionary<string,object> GetStoredProcedureParametersForEntity(T entity);
        protected abstract Dictionary<string, object> GetStoredProcedureParametersForEntityUpdate(T entity);
    }
}
