//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   Oracle处理类
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
using System.Data.OracleClient;
using System.ComponentModel;
using System.EnterpriseServices;

namespace GW.Utils.DataAccess
{
    /// <summary>
    /// Oracle处理类
    /// </summary>
    public class OracleDB : IDBHelper
    {
        private ArrayList _commands;
        private OracleConnection _conn;
        private bool _inManualTransaction;
        private OracleTransaction _tran;
        private int _transDeep;
        private string _connectionString;
        private int _commandTimeout = 30;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        public OracleDB(string connectionString)
        {
            this._commands = new ArrayList();
            _connectionString = connectionString;
        }

        /// <summary>
        /// 数据连接对象
        /// </summary>
        /// <returns>OracleConnection数据连接对象</returns>
        private OracleConnection GetConnection()
        {
            int num = 0;
            for (int i = 0; i <= num; i++)
            {
                try
                {
                    if (this._conn == null)
                    {
                        this._conn = new OracleConnection(this._connectionString);
                    }
                    if (this._conn.State == ConnectionState.Closed)
                    {
                        this._conn.Open();
                    }
                }
                catch (Exception ex)
                {
                    OracleConnection.ClearAllPools();
                    num = 1;
                    throw ex;
                }
            }
            return this._conn;
        }

        /// <summary>
        /// 数据库连接字符串属性
        /// </summary>
        public string ConnectionString
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
        /// 执行存储过程
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <returns>string受影响的记录数</returns>
        public string ExecNonQuery(string procname, Hashtable pars)
        {
            return ExecNonQuery(procname, pars,false);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <param name="isHasReturnVal">是否有返回值</param>
        /// <returns>string受影响的记录数</returns>
        public string ExecNonQuery(string procname, Hashtable pars,bool isHasReturnVal)
        {
            OracleCommand command;
            command = new OracleCommand(procname, this.GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            OracleCommandBuilder.DeriveParameters(command);
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                if (command.Parameters[i].ParameterName.ToUpper() == "P_")
                {
                    command.Parameters[i].OracleType = OracleType.Int32;
                    command.Parameters[i].Value = 0;
                    break;
                }
                foreach (string text in pars.Keys)
                {
                    string text2 = (text.ToUpper().Substring(2) == "P_") ? text.ToUpper() : text.ToUpper().Replace("@", "P_");
                    if (text2 == command.Parameters[i].ParameterName.ToUpper())
                    {
                        if (pars[text] == null)
                        {
                            command.Parameters[i].IsNullable = true;
                            command.Parameters[i].Value = DBNull.Value;
                        }
                        else if (pars[text].GetType().ToString() == "System.Boolean")
                        {
                            command.Parameters[i].Value = Convert.ToBoolean(pars[text]) ? 1 : 0;
                        }
                        else if (command.Parameters[i].OracleType.ToString() == "DateTime")
                        {
                            if ((pars[text].GetType().ToString() == "System.string") && (pars[text].ToString().Trim() == ""))
                            {
                                command.Parameters[i].IsNullable = true;
                                command.Parameters[i].Value = DBNull.Value;
                            }
                            else
                            {
                                command.Parameters[i].Value = Convert.ToDateTime(pars[text]);
                            }
                        }
                        else
                        {
                            if (command.Parameters[i].OracleType == OracleType.Clob && (pars[text] == null || pars[text].ToString() == ""))
                                command.Parameters[i].Value = DBNull.Value;
                            else
                                command.Parameters[i].Value = pars[text];

                            //command.Parameters[i].Value = pars[text];
                        }
                        break;
                    }
                }
            }
            if (this._transDeep > 0)
            {
                this._commands.Add(command);
                return "0";
            }
            if (this._conn.State == ConnectionState.Open)
            {
                this._conn.Close();
            }
            return ParseHelper.Parse<string>(command.ExecuteNonQuery());

        }

        /// <summary>
        /// 析构函数，释放资源
        /// </summary>
        ~OracleDB()
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

        /// <summary>
        /// 释放资源
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        /// <returns>数据库类型</returns>
        public DatabaseType CurrentDatabaseType
        {
            get
            {
                return DatabaseType.Oracle;
            }
        }

        #region 私有方法

        /// <summary>
        /// 处理拼接SQL
        /// </summary>
        /// <param name="SqlText">参数列表</param>
        /// <returns>格式化后的string</returns>
        private string Pretreatment(string SqlText)
        {
            if ((SqlText.IndexOf("[") != -1) && (SqlText.IndexOf("]") != -1))
            {
                string[] textArray = SqlText.Split(new char[] { '\'' });
                string text = "";
                for (int i = 0; i < textArray.Length; i++)
                {
                    if ((i % 2) != 0)
                    {
                        text = text + "'" + textArray[i];
                    }
                    else
                    {
                        text = text + "'" + textArray[i].Replace("[", "").Replace("]", "");
                    }
                }
                SqlText = text.Substring(1);
            }
            SqlText = SqlText.Replace("\r", " ");
            return SqlText;
        }

        /// <summary>
        /// 处理接拼参数
        /// </summary>
        /// <param name="SqlText">SQL参数</param>
        /// <param name="pars">参数列表</param>
        /// <returns>string参数</returns>
        private string Pretreatment(string SqlText, Hashtable pars)
        {
            SqlText = this.Pretreatment(SqlText);
            int length = SqlText.Split(new char[] { '@' }).Length;
            int item = 0;
            string[] textArray = new string[pars.Count];
            ArrayList list = new ArrayList();
            Regex regex = new Regex(@"@(?<num>[\d]+)");
            foreach (Match match in regex.Matches(SqlText))
            {
                item = Convert.ToInt32(match.Value.Substring(1));
                textArray[item - 1] = (item - 1).ToString();
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
            for (int i = pars.Count - 1; i >= 0; i--)
            {
                if (textArray[i] == null)
                {
                    pars.Remove(i);
                }
            }
            list.Sort();
            for (int j = 0; j < list.Count; j++)
            {
                SqlText = Regex.Replace(SqlText, "@" + list[j].ToString() + @"(?<Code>[^\d])", "@" + ((j + 1)).ToString() + "${Code}");
            }
            return SqlText;
        }

        /// <summary>
        /// 处理参数
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>格式化string</returns>
        private string ReplaceParameters(string sql)
        {
            string pattern = @"@(?<num>[\d]+)";
            string replacement = ":p_${num}";
            return Regex.Replace(sql, pattern, replacement);
        }

        #endregion

        #region 返回DataSet
        /// <summary>
        /// 根据SQL语句返回DataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataSet数据集</returns>
        public DataSet ExcuteDataSet(string sql)
        {
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(sql))
            {
                try
                {
                    OracleCommand command = new OracleCommand(sql, this.GetConnection());

                    command.CommandTimeout = this._conn.ConnectionTimeout; //_commandTimeout;
                    new OracleDataAdapter(command).Fill(ds);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (this._conn.State == ConnectionState.Open)
                    {
                        this._conn.Close();
                        this._conn.Dispose();
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// 根据sql返回DataSet
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="pars">参数</param>
        /// <returns>DataSet数据集</returns>
        public DataSet ExcuteDataSet(string procName, Hashtable pars)
        {
            OracleDataAdapter adapter = new OracleDataAdapter();

            DataSet dataSet = Process("P_RetVal", pars, procName, adapter);
            return dataSet;
        }

        #endregion

        #region DataTable

        /// <summary>
        /// 执行存储过程,返回DataView
        /// </summary>
        /// <param name="sqlText">存储过程名</param>
        /// <returns>DataTable数据集</returns>
        public DataTable ExcuteDataTable(string sqlText)
        {
            return this.ExcuteDataTable(sqlText, new Hashtable());
        }

        /// <summary>
        /// 执行存储过程,返回DataView
        /// </summary>
        /// <param name="sqlText">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <returns>DataTable数据集</returns>
        public DataTable ExcuteDataTable(string sqlText, Hashtable pars)
        {
            return this.ExcuteDataTable(sqlText, pars, "P_RetVal");
        }

        /// <summary>
        /// 执行存储过程,返回DataView
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <param name="rtnParName">返回参数</param>
        /// <returns>DataTable数据集</returns>
        public DataTable ExcuteDataTable(string procname, Hashtable pars, string rtnParName)
        {
            //if (!rtnParName.StartsWith("@"))
            //{
            //    rtnParName = "@" + rtnParName;
            //}
            OracleDataAdapter adapter = new OracleDataAdapter();
            {
                DataSet dataSet = Process(rtnParName, pars, procname, adapter);
                if (dataSet.Tables["result"] != null)
                {
                    return dataSet.Tables["result"];
                }
            }
            return null;
        }

        /// <summary>
        /// 参数处理
        /// </summary>
        /// <param name="rtnParName">返回参数</param>
        /// <param name="pars">参数列表</param>
        /// <param name="procname">存储过程名</param>
        /// <param name="adapter">adapter</param>
        /// <returns>DataSet数据集</returns>
        private DataSet Process(string rtnParName, Hashtable pars, string procname, OracleDataAdapter adapter)
        {
            
            string cmdText = procname;
            adapter.SelectCommand = new OracleCommand(cmdText, this.GetConnection());

            if (adapter.SelectCommand.Connection.State == ConnectionState.Closed)
            {
                adapter.SelectCommand.Connection.Open();
            }

            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand.CommandTimeout = this._conn.ConnectionTimeout; //_commandTimeout;

            if (this._inManualTransaction)
            {
                adapter.SelectCommand.Transaction = this._tran;
            }

            OracleCommandBuilder.DeriveParameters(adapter.SelectCommand);

            for (int i = 0; i < adapter.SelectCommand.Parameters.Count; i++)
            {
                //if (adapter.SelectCommand.Parameters[i].ParameterName.ToUpper() == rtnParName.ToUpper())
                //{
                //    adapter.SelectCommand.Parameters[i].Value = 0;
                //    continue;
                //}
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
                        if (adapter.SelectCommand.Parameters[i].OracleType == OracleType.Clob && pars[text2].ToString() == "")
                            adapter.SelectCommand.Parameters[i].Value = DBNull.Value;
                        else
                            adapter.SelectCommand.Parameters[i].Value = pars[text2];
                    }
                    break;
                }
            }
            DataSet dataSet = new DataSet();
            try
            {
                adapter.Fill(dataSet, "result");
                if (this._inManualTransaction)
                {
                    this._commands.Add(adapter.SelectCommand);
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            finally
            {
                if (this._conn.State == ConnectionState.Open)
                {
                    this._conn.Close();
                    this._conn.Dispose();
                }
            }
            return dataSet;
        }

        #endregion

        #region DataReader

        /// <summary>
        /// 执行存储过程，返回DataReader
        /// </summary>
        /// <param name="procName">存储过程名</param>
        /// <param name="pars">参数列表</param>
        /// <returns>DbDataReader数据集</returns>
        public DbDataReader ExcuteDataReader(string procName, Hashtable pars)
        {
            Hashtable par = new Hashtable(pars);
            string cmdText = this.Pretreatment(procName, pars);
            OracleConnection connection = this.GetConnection();

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            DbDataReader reader = null;

            OracleCommand command = new OracleCommand(cmdText, connection);
            command.CommandType = CommandType.Text;
            command.CommandTimeout = this._conn.ConnectionTimeout; //Convert.ToInt32(_commandTimeout);
            for (int i = 1; i < (pars.Count + 1); i++)
            {
                object obj2 = pars[i - 1];
                if (obj2 == null)
                {
                    command.Parameters.Add(new OracleParameter("p_" + i, DBNull.Value));
                }
                else if (obj2.GetType().ToString() == "System.string")
                {
                    if (Regex.IsMatch(obj2.ToString(), @"^\d+[\.\/\-]\d+[\.\/\-]\d+(\s+\d+:\d+:\d+){0,1}$"))
                    {
                        command.Parameters.Add("p_" + i, OracleType.DateTime).Value = Convert.ToDateTime(obj2);
                    }
                    else if (obj2.ToString().Length == 0)
                    {
                        command.Parameters.Add("p_" + i, OracleType.VarChar, 0x40).Value = obj2;
                    }
                    else
                    {
                        command.Parameters.Add("p_" + i, OracleType.VarChar).Value = Convert.ToString(obj2);
                    }
                }
                else if (obj2.GetType().ToString() == "System.Boolean")
                {
                    command.Parameters.Add("p_" + i, OracleType.Number, 1).Value = Convert.ToBoolean(obj2) ? 1 : 0;
                }
                else
                {
                    command.Parameters.Add(new OracleParameter("p_" + i, obj2));
                }
            }
            command.CommandText = cmdText;
            if ((this._transDeep > 0) && !this._inManualTransaction)
            {
                this._commands.Add(command);
                return null;
            }
            try
            {
                if (this._inManualTransaction)
                {
                    command.Transaction = this._tran;
                }
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                if (this._inManualTransaction)
                {
                    this._commands.Add(command);
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
           
            return reader;
        }

        #endregion

        #region 多个存储过程

        /// <summary>
        /// 执行多个存储过程，实现数据库事务
        /// </summary>
        /// <param name="procList">存储过程的哈希表（value为存储过程语句，key是该语句的DbParameter[]）</param>
        /// <returns>bool执行多个存储过程成功或失败</returns>
        public bool RunProcedureTran(Hashtable procList)
        {

            OracleConnection connection = this.GetConnection();
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            using (OracleTransaction trans = this.GetConnection().BeginTransaction())
            {
                using (OracleCommand cmd = new OracleCommand())
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
                        return false;
                        throw ex;
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                            connection.Dispose();
                        }
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
