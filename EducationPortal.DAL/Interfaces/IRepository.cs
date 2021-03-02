using EducationPortal.Core.Models.Entities.Base;
using System;
using System.Collections.Generic;

namespace EducationPortal.DAL.Interfaces
{
    public interface IRepository
    {
        IEnumerable<TSource> GetDataBlock<TSource, TKey>(int indexStart, int count, Func<TSource, TKey> orderByKeySelector, Func<TSource, bool> wherePredicate) where TSource : BaseEntity;

        TSource FirstOrDefault<TSource>(Func<TSource, bool> predicate) where TSource : BaseEntity;

        TSource Create<TSource>(TSource entity) where TSource : BaseEntity;

        void Delete<TSource>(TSource entity) where TSource : BaseEntity;

        void Update<TSource>(TSource entity) where TSource : BaseEntity;

        bool Any<TSource>(Func<TSource, bool> anyPredicate) where TSource : BaseEntity;

        int Count<TSource>(Func<TSource, bool> wherePredicate) where TSource : BaseEntity;

        IEnumerable<TSource> Where<TSource>(Func<TSource, bool> wherePredicate) where TSource : BaseEntity;

        IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(IEnumerable<TOuter> outer, Func<TInner, bool> predicateInner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) where TOuter : BaseEntity where TInner : BaseEntity;

        void SaveChanges();
    }
}
