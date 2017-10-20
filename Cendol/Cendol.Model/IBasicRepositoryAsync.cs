using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cendol.Model
{
    public interface IBasicRepositoryAsync<T> where T: EntityBase
    {
        Task<T> GetByIdAsync(long Id);
        IEnumerable<T> ListAll(); //included disabled or deleted entries
        IEnumerable<T> List();
        IEnumerable<T> List(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, string order = "", bool isDeleted = false);
        IEnumerable<T> List(Expression<Func<T, bool>> predicate, string order = "", bool isDeleted = false);

        void Insert(T entity);
        void Delete(long Id);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
        void Update(T entity, Expression<Func<T, object>>[] properties);
        Task SaveAsync();
        DbConnection GetConnection();
        DbContext GetDbContext();
        DbSet<T> GetDbSet();
    }
}
