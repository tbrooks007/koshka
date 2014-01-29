using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitsBlog.Core.Interfaces;

namespace BitsBlog.Data.Repositories.Interfaces
{
    public interface IDocumentRepository<T> : IRepository<T> where T : class
    {
        List<T> GetAllByAuthor(int Id);
        List<T> GetByYear(int year);
        List<T> GetByYearAndMonth(int month, int year);
    }
}
