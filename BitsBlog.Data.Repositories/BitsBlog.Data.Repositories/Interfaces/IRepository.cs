using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitsBlog.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        int Save(T entity);
        bool Delete(T entity);
        //bool Update(string UUID);
        //bool Delete(string UUID);
        T GetById(int Id);
        T GetByUUID(string UUID);
        IList<T> GetAll();
        IList<T> Get(int numberToRetrieve);
    }
}
