//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   抽象类，定义不同数据库共同数据处理方法接口
//编写日期    :   2010-11-22
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace GW.Utils.DataAccess
{
    /// <summary>
    /// 数据库处理方法接口，定义公共事务和方法
    /// </summary>
    public interface IDBHelper
    {
        /// <summary>
        /// commandTimeout
        /// </summary>
        int CommandTimeout { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 获取数据库类别
        /// </summary>
        /// <returns>数据库类型</returns>
         DatabaseType CurrentDatabaseType { get; }

        /// <summary>
         /// 根据sql查询，返回DataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataSet数据集</returns>
        DataSet ExcuteDataSet(string sql);

        /// <summary>
        /// 根据sql查询，返回DataSet
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="commandParameters">参数列表</param>
        /// <returns>DataSet数据集</returns>
        DataSet ExcuteDataSet(string procname, Hashtable commandParameters);

        /// <summary>
        /// 执行存储过程，返回DataTable
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <returns>DataTable数据集</returns>
        DataTable ExcuteDataTable(string procname, Hashtable pars);

        /// <summary>
        /// 执行存储过程，返回DataTable
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <param name="rtnParName">返回参数</param>
        /// <returns>DataTable数据集</returns>
        DataTable ExcuteDataTable(string procname, Hashtable pars, string rtnParName);

        /// <summary>
        /// 执行存储过程，返回DataReader
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <returns>DbDataReader数据集</returns>
        DbDataReader ExcuteDataReader(string procname, Hashtable pars);

        /// <summary>
        /// 执行多个存储过程，实现数据库事务
        ///  SqlParameter[] pare = new SqlParameter[2];
        ///    pare[0] = new SqlParameter("@PageSize", SqlDbType.Int);
        ///    pare[0].Value = 30;
        ///    pare[1] = new SqlParameter("@PageIndex",SqlDbType.Int);
        ///    pare[1].Value=1;            
        ///    SqlParameter[] pare2 = new SqlParameter[1];
        ///    pare2[0] = new SqlParameter("@CustomerID", SqlDbType.NChar);
        ///    pare2[0].Value = "aaa";
        ///    hsh.Add(pare, "usp_GetCustomers");
        ///    hsh.Add(pare2, "WebFrameworkSample_Customers_GetOne");
        ///    bool Re = idb.RunProcedureTran(hsh);
        /// </summary>
        /// <param name="ProcList">存储过程列表</param>
        /// <returns>bool执行多个存储过程成功或失败</returns>
        bool RunProcedureTran(Hashtable ProcList);

        /// <summary>
        /// 执行存储过程，返回受影响的记录数
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <returns>string受影响的记录数</returns>
        string ExecNonQuery(string procname, Hashtable pars);

        /// <summary>
        /// 执行存储过程，返回纪录ID
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <param name="isHasReturnVal">是否返回ID</param>
        /// <returns>string受影响的记录数</returns>
        string ExecNonQuery(string procname, Hashtable pars, bool isHasReturnVal);

        /// <summary>
        /// Dispose释放资源
        /// </summary>
        void Dispose();
  
        /// <summary>
        /// 执行无参数存储过程，返回DataTable
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <returns>DataTable数据集</returns>
        DataTable ExcuteDataTable(string procname);

        /// <summary>
        /// 提交事务,页面需要设置《%@ Page Transaction="Required"%》
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 回滚事务,页面需要设置《%@ Page Transaction="Required"%》
        /// </summary>
        void RollBackTransaction();
       
    }
}
