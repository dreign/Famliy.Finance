using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Famliy.Finance.DAL;
using Famliy.Finance.Models;

namespace Famliy.Finance.BLL
{
    /// <summary>
    /// 业务逻辑基类
    /// <remarks>
    /// 
    /// </remarks>
    /// </summary>
    public class BaseService<T> where T : class, new()
    {
        private BaseRepository<T> _baseRepository;
        public BankModel DbContext = null;
        /// <summary>
        /// 使用默认连接
        /// </summary>
        public BaseService()
        {
            DbContext =ContextFactory.GetCurrentContext();
            _baseRepository = new BaseRepository<T>(DbContext);
        }
        /// <summary>
        /// 创建业务逻辑类
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public BaseService(DbContext db)
        {
            _baseRepository = new BaseRepository<T>(db);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>添加后的数据实体</returns>
        public T Add(T entity, bool isSave = true)
        {
            return _baseRepository.Add(entity, isSave);
        }

        ///// <summary>
        ///// 添加【必须先实例化才能使用】
        ///// </summary>
        ///// <param name="entity">数据实体</param>
        ///// <returns>添加后的记录ID</returns>
        //public virtual int Add(T entity) { return 0; }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">数据列表</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns></returns>
        public int AddRange(IEnumerable<T> entities, bool isSave = true)
        {
            return _baseRepository.AddRange(entities, isSave);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns></returns>
        public bool Update(T entity, bool isSave = true)
        {
            return _baseRepository.Update(entity, isSave);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns></returns>
        public bool Delete(T entity, bool isSave = true)
        {
            return _baseRepository.Delete(entity, isSave);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">数据列表</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>删除的记录数</returns>
        public int DeleteRange(IEnumerable<T> entities, bool isSave = true)
        {
            return _baseRepository.DeleteRange(entities, isSave);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>受影响的记录数</returns>
        public int Save()
        {
            return _baseRepository.Save();
        }

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="countLamdba">查询表达式</param>
        /// <returns>记录数</returns>
        public int Count(Expression<Func<T, bool>> countLamdba)
        {
            return _baseRepository.Count(countLamdba);
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="ID">实体ID</param>
        /// <returns></returns>
        public T Find(int ID)
        {
            return _baseRepository.Find(ID);
        }     

        /// <summary>
        /// 查找实体 
        /// </summary>
        /// <param name="findLambda">Lambda表达式</param>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> findLambda)
        {
            return _baseRepository.Find(findLambda);
        }

        /// <summary>
        /// 查找实体 
        /// </summary>
        /// <param name="findLambda">Lambda表达式</param>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> findLambda, string includeProperty)
        {
            return _baseRepository.Find(findLambda, includeProperty);
        }
        /// <summary>
        /// 查找所有列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> FindAll()
        {
            return _baseRepository.FindAll();
        }

        public IQueryable<T> FindList<TKey>(Expression<Func<T, bool>> whereLambda)
        {
            return _baseRepository.FindList<TKey>(whereLambda);
        }

        public IQueryable<T> FindList<TKey>(Expression<Func<T, bool>> whereLambda, string includeProperty)
        {
            return _baseRepository.FindList<TKey>(whereLambda, includeProperty);
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
            return _baseRepository.FindList<TKey>(number, whereLambda, orderType, orderLambda);
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
            return _baseRepository.FindPageList<TKey>(pageIndex, pageSize, out totalNumber, whereLambda, orderType, orderLambda);
        }
    }
}
