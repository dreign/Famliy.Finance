//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   SQL Server处理实例
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
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.EnterpriseServices;

namespace GW.Utils.DataAccess
{
    /// <summary>
    /// Sql Server 事务集合
    /// </summary>
    public partial class SqlDB : IDBHelper
    {
        private ArrayList _commands;
        private SqlConnection _conn;

        //是否要重新启动事务
        private bool _inManualTransaction;
        private SqlTransaction _tran;
        //次数，事务计数器
        private int _transDeep;
        private string _connectionString;
        private int _commandTimeout = 30;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        public SqlDB(string connectionString)
        {
            this._commands = new ArrayList();
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        /// <returns>数据库类型</returns>
        public DatabaseType CurrentDatabaseType
        {
            get
            {
                return DatabaseType.SqlServer;
            }
        }

        /// <summary>
        /// 数据库连接字符串属性
        /// </summary>
        public new string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
            set
            {
                this._connectionString = value;
            }
        }

        /// <summary>
        /// CommandTimeout
        /// </summary>
        public int CommandTimeout
        {
            get
            {
                return this._commandTimeout;
            }
            set
            {
                this._commandTimeout = value;
            }
        }

        /// <summary>
        /// 手动释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.Dispose(true);
                GC.SuppressFinalize(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="isDispose">是否Dispose</param>
        protected virtual void Dispose(bool isDispose)
        {
            try
            {
                if (isDispose && (this._conn != null))
                {
                    if (this._conn.State != ConnectionState.Closed)
                    {
                        this._conn.Dispose();
                        this._conn.Close();
                    }
                    this._conn = null;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// 析构函数，释放资源
        /// </summary>
        ~SqlDB()
        {
            try
            {
                this.Dispose(false);
                GC.SuppressFinalize(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 私有方法

        /// <summary>
        /// 取得连接对象
        /// </summary>
        /// <returns>SqlConnection连接对象</returns>
        private SqlConnection GetConnection()
        {
            int num = 0;
            for (int i = 0; i <= num; i++)
            {
                try
                {
                    if (this._conn == null)
                    {
                        this._conn = new SqlConnection(this._connectionString);
                    }
                    if (this._conn.State == ConnectionState.Closed)
                    {
                        this._conn.Open();
                    }
                }
                catch (Exception ex)
                {
                    SqlConnection.ClearAllPools();
                    num = 1;
                    throw ex;
                }
            }
            return this._conn;
        }

        /// <summary>
        /// 格式化sql
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="strpos">内容表达式</param>
        /// <returns>处理后的sql字符串</returns>
        private string ReplaceNull(string sql, string strpos)
        {
            sql = Regex.Replace(sql, strpos + "([^0-9])", "null$1");
            sql = Regex.Replace(sql, strpos + "$", "null");
            return sql;
        }

        /// <summary>
        /// 拼接Parameters参数
        /// </summary>
        /// <param name="command">SqlCommand</param>
        /// <param name="pars">参数列表</param>
        /// <param name="isHasReturnVal">是否有返回值</param>
        /// <returns>StringBuilder拼接参数</returns>  
        private StringBuilder GetDataAdapter(SqlCommand command, Hashtable pars, bool isHasReturnVal)
        {
            StringBuilder builder = new StringBuilder();

            #region 遍历

            if (isHasReturnVal)
            {
                command.Parameters.AddWithValue("@RETURN_VALUE", "").Direction = ParameterDirection.ReturnValue;
            }
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                foreach (string text2 in pars.Keys)
                {
                    if (text2.ToUpper() != command.Parameters[i].ParameterName.ToUpper())
                    {
                        continue;
                    }
                    if (pars[text2] == null)
                    {
                        command.Parameters[i].IsNullable = true;
                        command.Parameters[i].Value = DBNull.Value;
                        builder.Append("null");
                    }
                    else
                    {
                        if (command.Parameters[i].DbType == DbType.Boolean)
                        {
                            try
                            {
                                command.Parameters[i].Value = Convert.ToBoolean(pars[text2]);
                            }
                            catch
                            {
                                try
                                {
                                    command.Parameters[i].Value = Convert.ToInt32(pars[text2]) != 0;
                                }
                                catch
                                {
                                    command.Parameters[i].Value = false;
                                }
                            }
                        }
                        else
                        {
                            command.Parameters[i].Value = pars[text2];
                        }
                        if ((((command.Parameters[i].DbType == DbType.Decimal) || (command.Parameters[i].DbType == DbType.Double)) || ((command.Parameters[i].DbType == DbType.Int16) || (command.Parameters[i].DbType == DbType.Int32))) || (((command.Parameters[i].DbType == DbType.Int64) || (command.Parameters[i].DbType == DbType.Currency)) || ((command.Parameters[i].DbType == DbType.Single) || (command.Parameters[i].DbType == DbType.VarNumeric))))
                        {
                            builder.Append(pars[text2]);
                        }
                        else if (command.Parameters[i].DbType == DbType.Boolean)
                        {
                            builder.Append(Convert.ToInt16(pars[text2]));
                        }
                        else
                        {
                            builder.Append("'" + pars[text2] + "'");
                        }
                    }
                    if (i < (command.Parameters.Count - 1))
                    {
                        builder.Append(", ");
                    }
                    break;
                }
            }
            #endregion

            return builder;
        }

        #endregion

        #region 返回DataSet

        /// <summary>
        /// 根据sql返回DataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataSet数据集</returns>
        public DataSet ExcuteDataSet(string sql)
        {
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(sql))
            {
                SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    try
                    {
                        command.CommandTimeout = conn.ConnectionTimeout;
                        new SqlDataAdapter(command).Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }                
            }
            return ds;
        }

        /// <summary>
        /// 根据存储过程,返回DataSet
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数</param>
        /// <returns>DataSet数据集</returns>
        public DataSet ExcuteDataSet(string procname, Hashtable pars)
        {
            DataSet dataSet = Process("@RetVal", pars, procname);
            return dataSet;
        }

        #endregion

        #region 存储过程,返回DataTable

        /// <summary>
        /// 执行存储过程,返回DataTable
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <returns>DataTable数据集</returns>
        public DataTable ExcuteDataTable(string procname)
        {
            return this.ExcuteDataTable(procname, new Hashtable());
        }

        /// <summary>
        /// 执行存储过程,返回DataTable
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <returns>DataTable数据集</returns>
        public DataTable ExcuteDataTable(string procname, Hashtable pars)
        {
            return this.ExcuteDataTable(procname, pars, "@RetVal");
        }

        /// <summary>
        /// 执行存储过程,返回DataTable
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <param name="rtnParName">参数返回值</param>
        /// <returns>DataTable数据集</returns>
        public DataTable ExcuteDataTable(string procname, Hashtable pars, string rtnParName)
        {

            DataSet dataSet = Process(rtnParName, pars, procname);
            if (dataSet.Tables["result"] != null)
            {
                return dataSet.Tables["result"];
            }
            return null;
        }

        /// <summary>
        /// 拼接参数处理
        /// </summary>
        /// <param name="rtnParName">参数返回值</param>
        /// <param name="pars">参数列表</param>
        /// <param name="procname">存储过程名</param>
        /// <param name="adapter">adapter</param>
        /// <returns>DataSet数据集</returns>
        private DataSet Process(string rtnParName, Hashtable pars, string procname)
        {
            DataSet dataSet = new DataSet();

            if (!rtnParName.StartsWith("@"))
            {
                rtnParName = "@" + rtnParName;
            }
            string cmdText = procname;

            SqlConnection conn = new SqlConnection(_connectionString);
            //if (conn.State != ConnectionState.Closed)
            //    conn.Close();
            conn.Open();
            using (SqlCommand sqlCommand = new SqlCommand(cmdText, conn))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();

                adapter.SelectCommand = sqlCommand;
                //if (adapter.SelectCommand.Connection.State == ConnectionState.Closed)
                //{
                //    adapter.SelectCommand.Connection.Open();
                //}
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = conn.ConnectionTimeout;
                //if (this._inManualTransaction)
                //{
                //    adapter.SelectCommand.Transaction = this._tran;
                //}
                SqlCommandBuilder.DeriveParameters(adapter.SelectCommand);
                for (int i = 0; i < adapter.SelectCommand.Parameters.Count; i++)
                {
                    if (adapter.SelectCommand.Parameters[i].ParameterName.ToUpper() == rtnParName.ToUpper())
                    {
                        adapter.SelectCommand.Parameters[i].Value = 0;
                        continue;
                    }
                    foreach (string text2 in pars.Keys)
                    {
                        if (text2.ToUpper() != adapter.SelectCommand.Parameters[i].ParameterName.ToUpper())
                        {
                            continue;
                        }
                        if (pars[text2] == null)
                        {
                            adapter.SelectCommand.Parameters[i].IsNullable = true;
                            adapter.SelectCommand.Parameters[i].Value = DBNull.Value;
                        }
                        else if (adapter.SelectCommand.Parameters[i].DbType == DbType.Boolean)
                        {
                            try
                            {
                                adapter.SelectCommand.Parameters[i].Value = Convert.ToBoolean(pars[text2]);
                            }
                            catch
                            {
                                try
                                {
                                    adapter.SelectCommand.Parameters[i].Value = Convert.ToInt32(pars[text2]) != 0;
                                }
                                catch
                                {
                                    adapter.SelectCommand.Parameters[i].Value = false;
                                }
                            }
                        }
                        else
                        {
                            adapter.SelectCommand.Parameters[i].Value = pars[text2];
                        }
                        break;
                    }

                }
                try
                {
                    //if (this._conn.State != ConnectionState.Open)
                    //{
                    //    this._conn.Open();
                    //}

                    adapter.Fill(dataSet, "result");
                    //if (this._inManualTransaction)
                    //{
                    //    this._commands.Add(adapter.SelectCommand);
                    //}
                }
                catch (SqlException exception)
                {
                    //事务出错
                    throw exception;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            return dataSet;
        }

        #endregion

        #region 返回int
        /// <summary>
        /// 返回受影响的记录数
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数</param>
        /// <returns>string受影响的记录数</returns>
        public string ExecNonQuery(string procname, Hashtable pars)
        {
            return ExecNonQuery(procname, pars, false);
        }

        /// <summary>
        /// 返回受影响的记录ID
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数</param>
        /// <param name="isHasReturnVal">返回ID</param>
        /// <returns>string受影响的记录数</returns>
        public string ExecNonQuery(string procname, Hashtable pars, bool isHasReturnVal)
        {
            using (SqlConnection connection = this.GetConnection())
            {
                try
                {
                    StringBuilder builder = new StringBuilder();


                    SqlCommand command = new SqlCommand(procname, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = this._conn.ConnectionTimeout; //_commandTimeout;

                    SqlCommandBuilder.DeriveParameters(command);

                    builder = GetDataAdapter(command, pars, isHasReturnVal);

                    StringBuilder builder2 = new StringBuilder();

                    builder2.Append("Execute ");
                    builder2.Append(procname);
                    builder2.Append(" ");
                    builder2.Append(builder);


                    //command.ExecuteNonQuery();
                    string re = ParseHelper.Parse<string>(command.ExecuteNonQuery());
                    if (isHasReturnVal)
                        re = ParseHelper.Parse<string>(command.Parameters["@RETURN_VALUE"].Value);

                    return re;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    if (this._conn.State == ConnectionState.Open)
                        this._conn.Close();
                }
            }
        }

        #endregion

        #region 返回DataReader
        /// <summary>
        /// 执行存储过程，返回DataReader
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <returns>DbDataReader数据集</returns>
        public DbDataReader ExcuteDataReader(string procName, Hashtable pars)
        {
            using (SqlConnection connection = this.GetConnection())
            {
                try
                {
                    StringBuilder builder = new StringBuilder();

                    DbDataReader reader = null;
                    SqlCommand command = new SqlCommand(procName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = this._conn.ConnectionTimeout; //_commandTimeout;
                    if (this._inManualTransaction)
                    {
                        command.Transaction = this._tran;
                    }
                    SqlCommandBuilder.DeriveParameters(command);
                    builder = GetDataAdapter(command, pars, false);

                    StringBuilder builder2 = new StringBuilder();

                    builder2.Append("Execute ");
                    builder2.Append(procName);
                    builder2.Append(" ");
                    builder2.Append(builder);

                    reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    return reader;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    if (this._conn.State == ConnectionState.Open)
                        this._conn.Close();                    
                }
            }
        }

        #endregion

        #region 多个存储过程

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
        /// <param name="procList">存储过程的哈希表（value为存储过程语句，key是该语句的DbParameter[]）</param>
        /// <returns>是否执行成功</returns>
        public bool RunProcedureTran(Hashtable procList)
        {

            SqlConnection connection = this.GetConnection();
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            using (SqlTransaction trans = this.GetConnection().BeginTransaction())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in procList)
                        {
                            cmd.Connection = connection;
                            string storeName = myDE.Value.ToString();
                            DbParameter[] cmdParms = (DbParameter[])myDE.Key;

                            cmd.Transaction = trans;
                            cmd.CommandText = storeName;
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (cmdParms != null)
                            {
                                foreach (DbParameter parameter in cmdParms)
                                {
                                    cmd.Parameters.Add(parameter);
                                }
                            }
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        connection.Close();
                        connection.Dispose();

                        //return false;
                        throw ex;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 提交事务,页面需要设置《%@ Page Transaction="Required"%》
        /// </summary>
        public void CommitTransaction()
        {
            ContextUtil.SetComplete();
        }

        /// <summary>
        /// 回滚事务,页面需要设置《%@ Page Transaction="Required"%》
        /// </summary>
        public void RollBackTransaction()
        {
            ContextUtil.SetAbort();
        }
    }
}
