using System.Linq.Expressions;
using LMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;
        internal DbSet<TEntity> dbSet;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            this.dbSet = _unitOfWork.Db.Set<TEntity>();
        }

        public virtual List<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", bool ignoreLanguage = false)
        {
            var query = GetQuery(filter, includeProperties, ignoreLanguage);

            if (orderBy != null)
                return orderBy(query).ToList();


            return query.ToList();
        }

        public virtual List<TEntity> Get(int limit, int skip, Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", bool ignoreLanguage = false)
        {
            var query = GetQuery(filter, includeProperties, ignoreLanguage);
            if (orderBy != null)
                return orderBy(query).Skip(skip).Take(limit).ToList();
            return query.Skip(skip).Take(limit).ToList();
        }

        public virtual List<TResult> GetAs<TResult>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "", bool ignoreLanguage = false) where TResult : class
        {
            var query = GetQuery(filter, includeProperties);
            return query.Select(select).ToList();
        }
        public virtual Task<List<TResult>> GetAsAsync<TResult>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> filter = null, string includeProperties = "") where TResult : class
        {
            var query = GetQuery(filter, includeProperties);
            return query.Select(select).ToListAsync();
        }

        public virtual Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", bool ignoreLanguage = false)
        {

            var query = GetQuery(filter, includeProperties);

            if (orderBy != null)
                return orderBy(query).ToListAsync();

            return query.ToListAsync();
        }


        public virtual Task<List<TEntity>> GetAsync(int limit, int skip, Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = ""
          )
        {

            var query = GetQuery(filter, includeProperties);

            if (orderBy != null)
                return orderBy(query).Skip(skip).Take(limit).ToListAsync();

            return query.Skip(skip).Take(limit).ToListAsync();
        }

        public int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = GetQuery(filter);
            var count = query.Count();
            return count;
        }

        private IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "", bool ignoreLanguage = false)
        {
            IQueryable<TEntity> query = dbSet;

            Type[] types = typeof(TEntity).GetInterfaces();

            //here we re filter deleted item


            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }


        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return dbSet.FirstOrDefault(filter);
        }
        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            return dbSet.FirstOrDefaultAsync(filter);
        }

        public virtual void Insert(TEntity entity, bool saveChanges = true)
        {
            dbSet.Add(entity);
            if (saveChanges == true)
                _unitOfWork.Save();
        }


        public virtual async Task<bool> InsertAsync(TEntity entity, bool saveChanges = true)
        {
            bool resultModel = false;
            try
            {
                await dbSet.AddAsync(entity);
                if (saveChanges == true)
                    _unitOfWork.Save();
                resultModel = true;
            }
            catch (Exception)
            {
                resultModel = false;
            }

            return resultModel;


        }

        public virtual void InsertAll(List<TEntity> entities, bool saveChanges = true)
        {
            foreach (var entity in entities)
            {
                dbSet.Add(entity);
            }
            if (saveChanges == true)
                _unitOfWork.Save();
        }

        public virtual void DeleteAll(List<TEntity> entities, bool saveChanges = true)
        {
            foreach (var entityToDelete in entities)
            {
                if (_unitOfWork.Db.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }

                else
                {
                    dbSet.Remove(entityToDelete);
                }
            }
            if (saveChanges == true)
                _unitOfWork.Save();
        }

        public virtual void UpdateAll(List<TEntity> entities, bool saveChanges = true)
        {
            foreach (var entityToUpdate in entities)
            {
                if (_unitOfWork.Db.Entry(entityToUpdate).State == EntityState.Detached)
                    dbSet.Attach(entityToUpdate);
                _unitOfWork.Db.Entry(entityToUpdate).State = EntityState.Modified;
            }
            if (saveChanges == true)
                _unitOfWork.Save();
        }

        public virtual void Delete(object id, bool saveChanges = true)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete, saveChanges);
        }

        public virtual void Delete(TEntity entityToDelete, bool saveChanges = true)
        {

            if (_unitOfWork.Db.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }

            else
            {
                dbSet.Remove(entityToDelete);
            }
            if (saveChanges == true)
                _unitOfWork.Save();
        }

        public virtual void Update(TEntity entityToUpdate, bool saveChanges = true)
        {
            if (_unitOfWork.Db.Entry(entityToUpdate).State == EntityState.Detached)
                dbSet.Attach(entityToUpdate);

            _unitOfWork.Db.Entry(entityToUpdate).State = EntityState.Modified;
            if (saveChanges == true)
                _unitOfWork.Save();
        }
        public bool Any(Expression<Func<TEntity, bool>> filter)
        {
            return dbSet.Any(filter);
        }
        public bool Exists(Expression<Func<TEntity, bool>> whereCondition)
        {
            return dbSet.Any(whereCondition);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.AsEnumerable();
        }
        public IQueryable<TEntity> GetAllAsQueryable()
        {
            return dbSet.AsQueryable<TEntity>();
        }
        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> whereCondition)
        {
            return dbSet.Where(whereCondition).AsEnumerable();
        }
        public IQueryable<TEntity> GetAllAsQueryable(Expression<Func<TEntity, bool>> whereCondition)
        {
            return dbSet.Where(whereCondition).AsQueryable<TEntity>();
        }
        public IQueryable<TEntity> Include(Expression<Func<TEntity, object>> includeExpressions)
        {
            var expression = (MemberExpression)includeExpressions.Body;
            string propertyName = expression.Member.Name;
            return dbSet.Include(propertyName).AsQueryable<TEntity>();
        }


        //public virtual List<T> FromSQL<T>(String SqlQuery, object[] parameters)
        //{

        //  //  var retval =(List<T>)this.context.Database.ExecuteSqlCommand(SqlQuery, parameters);
        //    return retval;
        //}
    }
}


