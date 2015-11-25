using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Famliy.Finance.Models;
using System.Data.Entity.Infrastructure;

namespace Famliy.Finance.DAL
{
    /// <summary>
    /// 数据仓储基类
    /// <remarks>
    /// 
    /// </remarks>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    
    public class BaseRepository<T> where T : class, new()
    {
        private DbContext _baseDbContext;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public BaseRepository(DbContext dbContext)
        {
            _baseDbContext = dbContext;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns></returns>
        public T Add(T entity, bool isSave = true)
        {
            _baseDbContext.Set<T>().Add(entity);
            if (isSave) _baseDbContext.SaveChanges();
            return entity;
        }

        /// <summary>
        /// 批量添加【立即执行】
        /// </summary>
        /// <param name="entities">数据列表</param>
        /// <returns>添加的记录数</returns>
        public int AddRange(IEnumerable<T> entities, bool isSave = true)
        {
            _baseDbContext.Set<T>().AddRange(entities);
            return isSave ? _baseDbContext.SaveChanges() : 0;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns></returns>
        public bool Update(T entity, bool isSave = true)
        {
           // RemoveHoldingEntityInContext(entity);

            _baseDbContext.Set<T>().Attach(entity);
            _baseDbContext.Entry<T>(entity).State = EntityState.Modified;
            return isSave ? _baseDbContext.SaveChanges() > 0 : true;
        }

        //用于监测Context中的Entity是否存在，如果存在，将其Detach，防止出现问题。
        private Boolean RemoveHoldingEntityInContext(T entity)
        {
            var objContext = ((IObjectContextAdapter)_baseDbContext).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);

            if (exists)
            {
                objContext.Detach(foundEntity);
            }

            return (exists);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns></returns>
        public bool Delete(T entity, bool isSave = true)
        {
            _baseDbContext.Set<T>().Attach(entity);
            _baseDbContext.Entry<T>(entity).State = EntityState.Deleted;
            return isSave ? _baseDbContext.SaveChanges() > 0 : true;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">数据列表</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>删除的记录数</returns>
        public int DeleteRange(IEnumerable<T> entities, bool isSave = true)
        {
            _baseDbContext.Set<T>().RemoveRange(entities);
            return isSave ? _baseDbContext.SaveChanges() : 0;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>受影响的记录数</returns>
        public int Save()
        {
            return _baseDbContext.SaveChanges();
        }

        /// <summary>
        /// 是否有满足条件的记录
        /// </summary>
        /// <param name="anyLamdba">条件表达式</param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> anyLamdba)
        {
            return _baseDbContext.Set<T>().Any(anyLamdba);
        }

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="countLamdba">查询表达式</param>
        /// <returns>记录数</returns>
        public int Count(Expression<Func<T, bool>> countLamdba)
        {
            return _baseDbContext.Set<T>().Count(countLamdba);
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="ID">实体ID</param>
        /// <returns></returns>
        public T Find(int ID)
        {
            return _baseDbContext.Set<T>().Find(ID);
        }

        /// <summary>
        /// 查找实体 
        /// </summary>
        /// <param name="findLambda">Lambda表达式</param>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> findLambda)
        {
            return _baseDbContext.Set<T>().SingleOrDefault(findLambda);
        }
        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="findLambda">Lambda表达式</param>
        /// <param name="includeProperty">需要include的属性</param>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> findLambda,string includeProperty)
        {
            return _baseDbContext.Set<T>().Include(includeProperty).SingleOrDefault(findLambda);
        }
        /// <summary>
        /// 查找所有列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> FindAll()
        {
            return FindList<int>(0, T => true, OrderType.No, null);
        }
        public IQueryable<T> FindList<TKey>(Expression<Func<T, bool>> whereLambda)
        {
            IQueryable<T> _tIQueryable = _baseDbContext.Set<T>().Where(whereLambda);           
            return _tIQueryable;
        }

        public IQueryable<T> FindList<TKey>(Expression<Func<T, bool>> whereLambda, string includeProperty)
        {
            IQueryable<T> _tIQueryable = _baseDbContext.Set<T>().Include(includeProperty).Where(whereLambda);
            return _tIQueryable;
        }
        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <param name="number">返回的记录数【0-返回所有】</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderType">排序方式</param>
        /// <param name="orderLambda">排序条件</param>
        /// <returns></returns>
        public IQueryable<T> FindList<TKey>(int number, Expression<Func<T, bool>> whereLambda, OrderType orderType, Expression<Func<T, TKey>> orderLambda)
        {
            IQueryable<T> _tIQueryable = _baseDbContext.Set<T>().Where(whereLambda);
            switch (orderType)
            {
                case OrderType.Asc:
                    _tIQueryable = _tIQueryable.OrderBy(orderLambda);
                    break;
                case OrderType.Desc:
                    _tIQueryable = _tIQueryable.OrderByDescending(orderLambda);
                    break;
            }
            if (number > 0) _tIQueryable = _tIQueryable.Take(number);
            return _tIQueryable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="pageIndex">页码【从1开始】</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalNumber">总记录数</param>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="orderLambda">排序表达式</param>
        /// <returns></returns>
        public IQueryable<T> FindPageList<TKey>(int pageIndex, int pageSize, out int totalNumber, Expression<Func<T, bool>> whereLambda, OrderType orderType, Expression<Func<T, TKey>> orderLambda)
        {
            IQueryable<T> _tIQueryable = _baseDbContext.Set<T>().Where(whereLambda);
            totalNumber = _tIQueryable.Count();
            switch (orderType)
            {
                case OrderType.Asc:
                    _tIQueryable = _tIQueryable.OrderBy(orderLambda);
                    break;
                case OrderType.Desc:
                    _tIQueryable = _tIQueryable.OrderByDescending(orderLambda);
                    break;
            }
            _tIQueryable = _tIQueryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return _tIQueryable;
        }
    }
}
