using DastgyrAPI.Interfaces.Repositories;
using DastgyrAPI.Models.DomainModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DastgyrAPI.Entity;

namespace DastgyrAPI.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : EntityBase
    {
        #region Properties

        private readonly IHttpContextAccessor _httpContextAccessor;

        protected readonly ApplicationDbContext _dbContext;

        #endregion

        #region Constructor


        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public BaseRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)

        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Interface Methods

        public virtual async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> List()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> List(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> List(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            query = query.Where(predicate);
            return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> List(Expression<Func<T, bool>> predicate, Expression<Func<T, string>> orderBy, int pageSize = 15, int pageNumber = 1, params Expression<Func<T, object>>[] includes)
        {

            IQueryable<T> query = _dbContext.Set<T>();
            query = query.Where(predicate).OrderBy(orderBy).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return await includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
        }

        public virtual async Task<int> Count(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>()
                   .Where(predicate)
                   .CountAsync();
        }

        public virtual async Task<int> Add(T entity)
        {
            //entity.CreatedDate = DateTime.UtcNow;
            //entity.ModifiedDate = DateTime.UtcNow;
            //entity.CreatedBy = LoggedInUserId;
            //entity.ModifiedBy = LoggedInUserId;
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }
        public virtual async Task<int> Update(T entity)
        {
            //entity.ModifiedDate = DateTime.UtcNow;
            //entity.ModifiedBy = LoggedInUserId;
            _dbContext.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<int> Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }


        #endregion

        #region Interface Properties
        public int LoggedInUserId
        {
            get
            {
                if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null &&
                    _httpContextAccessor.HttpContext.User != null)
                {
                    var first = (Users)_httpContextAccessor.HttpContext.Items["User"];
                    return first == null ? 0 : first.Id;
                }
                else
                    return 0;
            }
        }
        public TimeSpan UserTimezoneOffSet
        {
            get
            {
                var userTimeZoneOffsetClaim = TimeSpan.FromMinutes(0);
                // TODO: Temporary Comment
                //HttpContext.Current.Session["UserTimezoneOffset"]; //ClaimHelper.GetClaimsByType<string>(SmdClaimTypes.UserTimezoneOffset);
                if (userTimeZoneOffsetClaim == null)
                {
                    return TimeSpan.FromMinutes(0);
                }

                TimeSpan userTimeZoneOffset;
                TimeSpan.TryParse(userTimeZoneOffsetClaim.ToString(), out userTimeZoneOffset);

                return userTimeZoneOffset;
            }
        }

        #endregion

    }
}
