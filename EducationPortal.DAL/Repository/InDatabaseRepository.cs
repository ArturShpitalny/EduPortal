using EducationPortal.Core.Models.Entities;
using EducationPortal.Core.Models.Entities.Base;
using EducationPortal.DAL.EF.Context;
using EducationPortal.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.DAL.Repository
{
    public class InDatabaseRepository : IRepository
    {
        private readonly EDUDbContext dbContext;

        public InDatabaseRepository()
        {
            this.dbContext = new EDUDbContext();
        }

        public TSource Create<TSource>(TSource entity) where TSource : BaseEntity
        {
            if (entity != null)
            {
                this.dbContext.Set<TSource>().Add(entity);
                return entity;
            }
            return null;
        }

        public void Delete<TSource>(TSource entity) where TSource : BaseEntity
        {
            if(entity != null)
            {
                this.dbContext.Set<TSource>().Remove(entity);                
            }
        }

        public TSource FirstOrDefault<TSource>(Func<TSource, bool> predicate) where TSource : BaseEntity
        {
            return this.dbContext.Set<TSource>().FirstOrDefault<TSource>(predicate);
        }

        public IEnumerable<TSource> GetDataBlock<TSource, TKey>(int indexStart, int count, Func<TSource, TKey> orderByKeySelector, Func<TSource, bool> wherePredicate) where TSource : BaseEntity
        {
            return this.dbContext.Set<TSource>().Where(wherePredicate).OrderBy(orderByKeySelector).Skip(indexStart).Take(count);
        }        

        public void Update<TSource>(TSource entity) where TSource : BaseEntity
        {
            if (entity != null)
            {
                this.dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public bool Any<TSource>(Func<TSource, bool> anyPredicate) where TSource : BaseEntity
        {
            return this.dbContext.Set<TSource>().Any(anyPredicate);
        }

        public int Count<TSource>(Func<TSource, bool> wherePredicate) where TSource : BaseEntity
        {
            return this.dbContext.Set<TSource>().Where(wherePredicate).Count();
        }

        public IEnumerable<TSource> Where<TSource>(Func<TSource, bool> wherePredicate) where TSource : BaseEntity
        {
            return this.dbContext.Set<TSource>().Where(wherePredicate);
        }

        public IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(IEnumerable<TOuter> outer, Func<TInner, bool> predicateInner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) where TOuter : BaseEntity where TInner : BaseEntity
        {
            return outer.Join(this.dbContext.Set<TInner>().Where(predicateInner), outerKeySelector, innerKeySelector, resultSelector); 
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }        
    }
}
