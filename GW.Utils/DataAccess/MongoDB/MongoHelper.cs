using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using GW.Utils.TypeExtension;
using MongoDB.Driver.Builders;

namespace GW.Utils.DataAccess
{

    /// <summary>
    /// MongoHelper MongoDB辅助类:   
    ///   <para> 1>实现MongoDB的隐式连接</para>
    ///   <para> 2>实现CRUD 通用封装</para>
    ///   <para> 3>实现MongoDB Collection对象与OOP隐式转换</para>
    /// </summary>
    public class MongoHelper
    {
        #region 静态变量

        /// <summary>
        /// [MongoDB] 服务器地址 如本机(mongodb://127.0.0.1:27017)
        /// </summary>
        public static readonly string MongoServer;//= System.Configuration.ConfigurationManager.AppSettings["MongoServer"];

        /// <summary>
        /// [MongoDB] 数据库名称
        /// </summary>
        public static readonly string MongoDatabase;// = System.Configuration.ConfigurationManager.AppSettings["MongoDatabase"];

        #endregion

        #region 静态构造函数

        static MongoHelper()
        {
            //MongoServer = System.Configuration.ConfigurationManager.ConnectionStrings["CloudConnectionString"].ToString();
            //MongoDatabase = new Uri(MongoServer.Replace("mongodb://", "http://")).Segments.LastOrDefault();

            MongoServer = System.Configuration.ConfigurationManager.ConnectionStrings["MongoServer"].ToString();
            MongoDatabase = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDatabase"].ToString();

           
        }

        #endregion

        #region 创建 MongoServer、MongoDatabase、MongoCollection 对象

        public static MongoServer GetMongoServer(string strMongoServer)
        {
            return new MongoClient(strMongoServer).GetServer();
            
        }

        public static MongoDatabase GetMongoDatabase(string strMongoServer, string strMongoDatabase)
        {
            return GetMongoServer(strMongoServer).GetDatabase(strMongoDatabase);
        }

        public static MongoCollection GetCollection<TDocument>(string strMongoServer, string strMongoDatabase, string collectionName)
        {
            return GetMongoDatabase(strMongoServer, strMongoDatabase).GetCollection<TDocument>(collectionName);
        }

        public static MongoServer GetMongoServer()
        {
            return GetMongoServer(MongoServer);
        }

        public static MongoDatabase GetMongoDatabase()
        {
            return GetMongoDatabase(MongoServer, MongoDatabase);
        }

        public static MongoCollection GetCollection<TDocument>(string collectionName)
        {
            return GetCollection<TDocument>(MongoServer, MongoDatabase, collectionName);
        }

        public static MongoCollection GetCollection<TDocument>()
        {
            return GetCollection<TDocument>(MongoServer, MongoDatabase, typeof(TDocument).Name);
        }

        #endregion

        #region [MongoDB] 新增操作


        /// <summary>
        /// [MongoDB] 通用插入文档方法
        /// </summary>
        /// <typeparam name="TDocument">泛型类型</typeparam>
        /// <param name="tDocument">泛型实体</param>
        /// <returns></returns>
        public static bool Insert<TDocument>(TDocument tDocument)
        {
            try
            {
                var result = GetCollection<TDocument>(typeof(TDocument).Name).Insert(tDocument);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// [MongoDB] 通用插入文档方法
        /// </summary>
        /// <typeparam name="TDocument">泛型类型</typeparam>
        /// <param name="collectionName">MongoDB中对应的Collection Name</param>
        /// <param name="tDocument">泛型实体</param>
        /// <returns></returns>
        public static bool Insert<TDocument>(string collectionName, TDocument tDocument)
        {
            try
            {
                var result = GetCollection<TDocument>(collectionName).Insert(tDocument);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// [MongoDB] 通用批量插入文档方法
        /// </summary>
        /// <typeparam name="TDocument">泛型类型</typeparam>
        /// <param name="tDocuments">泛型实体</param>
        /// <returns></returns>
        public static bool InsertBatch<TDocument>(IList<TDocument> tDocuments)
        {
            try
            {
                var result = GetCollection<TDocument>(typeof(TDocument).Name).InsertBatch(typeof(TDocument), tDocuments);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// [MongoDB] 通用批量插入文档方法
        /// </summary>
        /// <typeparam name="TDocument">泛型类型</typeparam>
        /// <param name="collectionName">MongoDB中对应的Collection Name</param>
        /// <param name="tDocuments">泛型实体</param>
        /// <returns></returns>
        public static bool InsertBatch<TDocument>(string collectionName, IEnumerable<TDocument> tDocuments)
        {
            try
            {
                var result = GetCollection<TDocument>(typeof(TDocument).Name).InsertBatch(typeof(TDocument), tDocuments);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        #endregion

        #region [MongoDB] 删除操作


        /// <summary>
        /// [MongoDB] 通用全部删除文档方法
        /// </summary>
        /// <typeparam name="TDocument">泛型类型</typeparam>
        /// <returns></returns>
        public static bool RemoveAll<TDocument>()
        {
            try
            {
                var result = GetCollection<TDocument>(typeof(TDocument).Name).RemoveAll();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// [MongoDB] 通用根据条件删除文档方法
        /// </summary>
        /// <typeparam name="TDocument">泛型类型</typeparam>
        /// <param name="queryList">查询实体集合</param>
        /// <returns></returns>
        public static bool Remove<TDocument>(IList<IMongoQuery> queryList)
        {
            try
            {
             
                var result = GetCollection<TDocument>(typeof(TDocument).Name).Remove(GetIMongoQueryBy(queryList), RemoveFlags.None);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// [MongoDB] 通用根据条件删除文档方法
        /// </summary>
        /// <typeparam name="TDocument">泛型类型</typeparam>
        /// <param name="collectionName">集合名称</param>
        /// <param name="queryList"></param>
        /// <returns></returns>
        public static bool Remove<TDocument>(string collectionName,IList<IMongoQuery> queryList)
        {
            try
            {

                var result = GetCollection<TDocument>(collectionName).Remove(GetIMongoQueryBy(queryList), RemoveFlags.None);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// [MongoDB] 通用根据查询条件删除首个匹配的文档对象
        /// </summary>
        /// <typeparam name="TDocument">泛型类型</typeparam>
        /// <param name="queryList">查询实体集合</param>
        /// <param name="sortBy">排序实体 创建示例：SortBy.Descending("Title") OR SortBy.Descending("Title").Ascending("Author")</param>
        /// <returns></returns>
        public static bool FindAndRemove<TDocument>(IList<IMongoQuery> queryList, IMongoSortBy sortBy)
        {
            try
            {
                var findAndRemoveArgs = new FindAndRemoveArgs { Query = GetIMongoQueryBy(queryList), SortBy = sortBy };
                var result = GetCollection<TDocument>(typeof(TDocument).Name).FindAndRemove(findAndRemoveArgs);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region [MongoDB] 更新操作

        /// <summary>
        /// [MongoDB] 通用更新方法
        /// </summary>
        /// <typeparam name="TDocument">泛型类型</typeparam>
        /// <param name="queries">查询实体集合</param>
        /// <param name="updates">更新实体集合</param>
        /// <param name="updateFlag">更新枚举对象</param>
        /// <returns></returns>
        public static bool Update<TDocument>(IList<IMongoQuery> queries, IList<IMongoUpdate> updates, UpdateFlags updateFlag = UpdateFlags.Multi)
        {
            try
            {
                return Update<TDocument>(typeof(TDocument).Name, queries, updates, updateFlag);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// [MongoDB] 通用更新方法
        /// </summary>
        /// <typeparam name="TDocument">泛型类型</typeparam>
        /// <param name="collectionName"></param>
        /// <param name="queries">查询实体集合</param>
        /// <param name="updates">更新实体集合</param>
        /// <param name="updateFlag">更新枚举对象</param>
        /// <returns></returns>
        public static bool Update<TDocument>(string collectionName, IList<IMongoQuery> queries, IList<IMongoUpdate> updates, UpdateFlags updateFlag = UpdateFlags.Multi)
        {
            try
            {
                var result = GetCollection<TDocument>(collectionName).Update(GetIMongoQueryBy(queries), GetIMongoUpdateBy(updates), updateFlag);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region [MongoDB] 查询操作


        /// <summary>
        /// [MongoDB] 通用查询方法
        /// </summary>
        /// <typeparam name="T">泛型实体类(要求与MongoDB中的Collection同名)</typeparam>
        /// <param name="query">MongoDB 查询条件实体</param>
        /// <returns>泛型实体集合</returns>
        public static List<T> FindAs<T>(IMongoQuery query)
        {
            return FindAs<T>(typeof(T).Name, query);
        }

        /// <summary>
        /// [MongoDB] 通用查询方法
        /// </summary>-
        /// <typeparam name="T">泛型实体类(要求与MongoDB中的Collection同名)</typeparam>
        /// <param name="queryList">MongoDB 查询条件实体集合</param>
        /// <returns>泛型实体集合</returns>
        public static List<T> FindAs<T>(IList<IMongoQuery> queryList)
        {
            return FindAs<T>(typeof(T).Name, queryList.HasValidValues() ? Query.And(queryList) : Query.Null);
        }

        /// <summary>
        /// [MongoDB] 通用查询方法
        /// </summary>
        /// <typeparam name="T">泛型实体类</typeparam>
        /// <param name="collectionName">MongoDB中对应的Collection Name</param>
        /// <param name="query">MongoDB 查询条件实体</param>
        /// <returns>泛型实体集合</returns>
        public static List<T> FindAs<T>(string collectionName, IMongoQuery query)
        {
            var result = new List<T>();
            try
            {
                var mongoCursor = GetCollection<T>(collectionName).FindAs<T>(query);
                result.AddRange(mongoCursor.Select(document => (T)document));
            }
            catch (Exception ex)
            {
                //todo:
            }
            return result;
        }

        /// <summary>
        ///  [MongoDB] 通用查询方法
        /// </summary>
        /// <typeparam name="T">泛型实体类</typeparam>
        /// <param name="collectionName">MongoDB中对应的CollectionName</param>
        /// <param name="queryList">MongoDB 查询条件实体集合</param>
        /// <returns>泛型实体集合</returns>
        public static List<T> FindAs<T>(string collectionName, IList<IMongoQuery> queryList)
        {
            return FindAs<T>(collectionName, queryList.HasValidValues() ? Query.And(queryList) : Query.Null);
        }



        /// <summary>
        /// [MongoDB] 通用查询方法
        /// </summary>
        /// <typeparam name="T">泛型实体类</typeparam>
        /// <param name="query">MongoDB 查询条件对象</param>
        /// <returns></returns>
        public static T FindOneAs<T>(IMongoQuery query)
        {
            return FindOneAs<T>(new List<IMongoQuery> { query });
        }

        /// <summary>
        /// [MongoDB] 通用查询方法
        /// </summary>
        /// <typeparam name="T">泛型实体类</typeparam>
        /// <param name="queries">MongoDB 查询条件对象集合</param>
        /// <returns></returns>
        public static T FindOneAs<T>(List<IMongoQuery> queries)
        {
            var result = default(T);
            try
            {
                result = GetCollection<T>().FindOneAs<T>(GetIMongoQueryBy(queries));
            }
            catch (Exception)
            {
                //todo:
            }
            return result;
        }

        /// <summary>
        /// [MongoDB] 通用查询方法
        /// </summary>
        /// <typeparam name="T">泛型实体类</typeparam>
        /// <param name="query">MongoDB 查询条件对象</param>
        /// <param name="sortBy">MongoDB 排序对象</param>
        /// <returns></returns>
        public static T FindOneAs<T>(IMongoQuery query, IMongoSortBy sortBy)
        {
            return FindOneAs<T>(new List<IMongoQuery> { query }, sortBy);
        }

        /// <summary>
        /// [MongoDB] 通用查询方法
        /// </summary>
        /// <typeparam name="T">泛型实体类</typeparam>
        /// <param name="queries">MongoDB 查询条件对象集合</param>
        /// <param name="sortBy">MongoDB 排序对象</param>
        /// <returns></returns>
        public static T FindOneAs<T>(List<IMongoQuery> queries, IMongoSortBy sortBy)
        {
            var result = default(T);
            try
            {
                result = GetCollection<T>().FindOneAs<T>(new FindOneArgs() { Query = GetIMongoQueryBy(queries), SortBy = sortBy });
            }
            catch (Exception)
            {
                //todo:
            }
            return result;
        }



        /// <summary>
        /// [MongoDB] 通用分页查询方法
        /// </summary>
        /// <typeparam name="T">泛型实体类</typeparam>
        /// <param name="query">MongoDB 查询条件对象</param>
        /// <param name="startIndex">起始记录位置</param>
        /// <param name="endIndex">终止记录位置</param>
        /// <param name="recordCount">总记录数量</param>
        /// <returns>泛型实体集合</returns>
        public static List<T> FindByPageAs<T>(IMongoQuery query, int startIndex, int endIndex, out Int32 recordCount)
        {
            return FindByPageAs<T>(new List<IMongoQuery> { query }, SortBy.Null, startIndex, endIndex, out recordCount);
        }

        /// <summary>
        /// [MongoDB] 通用分页查询方法
        /// </summary>
        /// <typeparam name="T">泛型实体类</typeparam>
        /// <param name="query">MongoDB 查询条件对象</param>
        /// <param name="sortBy">MongoDB 排序对象</param>
        /// <param name="startIndex">起始记录位置</param>
        /// <param name="endIndex">终止记录位置</param>
        /// <param name="recordCount">总记录数量</param>
        /// <returns>泛型实体集合</returns>
        public static List<T> FindByPageAs<T>(IMongoQuery query, IMongoSortBy sortBy, int startIndex, int endIndex, out Int32 recordCount)
        {
            return FindByPageAs<T>(new List<IMongoQuery> { query }, sortBy, startIndex, endIndex, out recordCount);
        }

        /// <summary>
        /// [MongoDB] 通用分页查询方法
        /// </summary>
        /// <typeparam name="T">泛型实体类</typeparam>
        /// <param name="queries">MongoDB 查询条件对象集合</param>
        /// <param name="startIndex">起始记录位置</param>
        /// <param name="endIndex">终止记录位置</param>
        /// <param name="recordCount">总记录数量</param>
        /// <returns>泛型实体集合</returns>
        public static List<T> FindByPageAs<T>(List<IMongoQuery> queries, int startIndex, int endIndex, out Int32 recordCount)
        {
            return FindByPageAs<T>(queries, SortBy.Null, startIndex, endIndex, out recordCount);
        }

        /// <summary>
        /// [MongoDB] 通用分页查询方法
        /// </summary>
        /// <typeparam name="T">泛型实体类</typeparam>
        /// <param name="queries">MongoDB 查询条件对象集合</param>
        /// <param name="sortBy">MongoDB 排序对象</param>
        /// <param name="startIndex">起始记录位置</param>
        /// <param name="endIndex">终止记录位置</param>
        /// <param name="recordCount">总记录数量</param>
        /// <returns>泛型实体集合</returns>
        public static List<T> FindByPageAs<T>(List<IMongoQuery> queries, IMongoSortBy sortBy, int startIndex, int endIndex, out Int32 recordCount)
        {
            var result = new List<T> { };
            recordCount = 0;
            try
            {
                var mongoCursor = GetCollection<T>().FindByPageAs<T>(new FindOneArgs() { Query = GetIMongoQueryBy(queries), SortBy = sortBy });
                if (mongoCursor.HasValidValues())
                {
                    recordCount = Convert.ToInt32(mongoCursor.Count());
                    result.AddRange(
                        mongoCursor.Skip(startIndex - 1)
                            .Take(endIndex - startIndex + 1)
                            .Select(document => (T)document));
                }
            }
            catch (Exception)
            {
                //todo:
            }
            return result;
        }



        #endregion

        #region 辅助方法


        /// <summary>
        ///  [MongoDB] 构造查询对象 IMongoQuery 实例
        /// </summary>
        /// <param name="queries">查询对象集合</param>
        /// <returns>查询对象 IMongoQuery 实例</returns>
        public static IMongoQuery GetIMongoQueryBy(IList<IMongoQuery> queries)
        {
            return queries.HasValidValues() ? Query.And(queries) : Query.Null;
        }

        /// <summary>
        ///  [MongoDB] 构造更新对象 IMongoUpdate 实例
        /// </summary>
        /// <param name="updates">更新对象集合</param>
        /// <returns>更新对象 IMongoUpdate 实例</returns>
        public static IMongoUpdate GetIMongoUpdateBy(IList<IMongoUpdate> updates)
        {
            return updates.HasValidValues() ? MongoDB.Driver.Builders.Update.Combine(updates) : MongoDB.Driver.Builders.Update.Combine(updates);//注意：updates==null
        }


        #endregion

    }
}
