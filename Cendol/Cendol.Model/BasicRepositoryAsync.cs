using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cendol.Model
{
    public class BasicRepositoryAsync<T> : IBasicRepositoryAsync<T> where T: EntityBase 
    {
        private CendolDbContext _dbContext;

        public long CompanyID { get; set; }
        public string UserName { get; set; }

        public BasicRepositoryAsync(CendolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(long Id)
        {
            //throw new Exception("Exception Test from GetByID");
            return await _dbContext.Set<T>().AsNoTracking<T>().Where(c => c.Id == Id).SingleOrDefaultAsync();
            /* NTY [2016.7.29] Added .AsNoTracking in order for Web API to be able to update entity */
        }

        public virtual DbContext GetDbContext()
        {
            return _dbContext;
        }

        public virtual DbSet<T> GetDbSet()
        {
            return _dbContext.Set<T>();
        }

        public virtual IEnumerable<T> ListAll()
        {
            return _dbContext.Set<T>()
                .AsNoTracking();
        }

        public virtual IEnumerable<T> List()
        {
            return _dbContext.Set<T>()
                .AsNoTracking<T>();
        }

        public virtual IEnumerable<T> List(Expression<Func<T, bool>> predicate, string order = "", bool isDeleted = false)
        {
            if (order.Trim() != "")
            {
                return _dbContext.Set<T>()
                    .AsNoTracking()
                    .Where(predicate)
                    .OrderBy(order);
            }
            else
            {
                return _dbContext.Set<T>()
                    .AsNoTracking()
                    .Where(predicate);
            }
        }

        public IEnumerable<T> List(Expression<Func<T, bool>> predicate, int pageNumber = 1, int pageSize = 15, string order = "", bool isDeleted = false)
        {
            var skip = (pageNumber - 1) * pageSize;

            if (order.Trim() == "")
            {
                return _dbContext.Set<T>()
                        .AsNoTracking()
                        .Where(predicate)
                        .Skip(skip)
                        .Take(pageSize);
            }
            else
            {
                return _dbContext.Set<T>()
                        .AsNoTracking()
                        .Where(predicate)
                        .OrderBy(order)
                        .Skip(skip)
                        .Take(pageSize);
            }
        }

        public void Insert(T entity)
        {
            //throw new Exception("Exception Test at Insert");
            _dbContext.Set<T>().Add(entity);

            /* It is better call SaveChanges separately with Save() method in Service layer */
            //_dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            /* It is better call SaveChanges separately with Save() method in Service layer */
            //_dbContext.SaveChanges();
        }

        public void Update(T entity, Expression<Func<T, Object>>[] properties)
        {
            //throw new Exception("Exception Test at Update");
            _dbContext.Set<T>().Attach(entity);

            foreach (var property in properties)
            {
                if (property.Body is MemberExpression)
                    _dbContext.Entry<T>(entity)
                    .Property(((MemberExpression)property.Body).Member.Name)
                    .IsModified = true;
                else
                {
                    var op = ((UnaryExpression)property.Body).Operand;
                    _dbContext.Entry<T>(entity)
                    .Property(((MemberExpression)op).Member.Name)
                    .IsModified = true;
                }
            }

            /* It is better call SaveChanges separately with Save() method in Service layer */
            //_dbContext.SaveChanges();
        }


        public void Delete(long Id)
        {
            //throw new Exception("Exception Test from Delete");
            var entity = _dbContext.Set<T>().Where(c => c.Id == Id).SingleOrDefault();
            if (entity != null)
                _dbContext.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            if (entities.Count() > 0)
                _dbContext.Set<T>().RemoveRange(entities);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public DbConnection GetConnection()
        {
            return _dbContext.Database.GetDbConnection();
        }
    }
}
