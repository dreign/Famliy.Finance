//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   Excel读取操作，适用范围：IIS6+32位操作系统、IIS7+32/64位操作系统
//编写日期    :   2010-11-19
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;

/// <summary>
/// ExcelHelper 的摘要说明
/// </summary>
namespace GW.Utils
{
    /// <summary>
    /// Excel读取操作，不适用IIS6+64位系统
    /// </summary>
    public class ExcelHelper
    {

        #region 成员变量

        private static string ConnectionStringFormat = "Provider={0};Data Source={1};Extended Properties='Excel {2}; HDR={3}; IMEX={4};';";
        private string _connectionString = string.Empty;
        private string _filePath;
        private string[] _sheets = null;
        private string _version = "8.0";
        private string _providerName = "Microsoft.Jet.OleDb.4.0";
        private bool _hasRowHeader;

        #endregion

        #region 公共属性

        /// <summary>
        /// Excel文件路径
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
        }

        /// <summary>
        /// 是否有标题行
        /// </summary>
        public bool HasRowHeader
        {
            get { return _hasRowHeader; }
            //set { _hasRowHeader = value; }
        }

        /// <summary>
        /// Excel中的Sheet
        /// </summary>
        public string[] Sheets
        {
            get
            {
                if (_sheets == null)
                {
                    _sheets = GetSheetNameList();
                }
                return _sheets;
            }
        }

        #endregion

        #region 构造函数
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="hasRowHeader">是否有标题行</param>
        /// <exception cref="FileNotFoundException">没有找到文件的异常</exception>
        public ExcelHelper(string filePath, bool hasRowHeader)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("File:{0} not found!", filePath));
            }

            this._filePath = filePath;
            this._hasRowHeader = hasRowHeader;

            string extension = Path.GetExtension(filePath);
            switch (extension.ToLower())
            {
                case ".xls":
                    _version = "8.0";
                    break;
                case ".xlsx":
                    _version = "12.0";
                    break;
                default:
                    break;
            }
            _providerName = GetProviderName(_version);

            //TODO 最后一个参数待完善
            _connectionString = GenerateConnectionString(filePath, _version, hasRowHeader, true);

        }

        #endregion

        #region 私有辅助方法

        private static string GetProviderName(string version)
        {
            string providername = string.Empty;
            switch (version)
            {
                case "8.0":
                    providername = "Microsoft.Jet.OleDb.4.0";
                    break;
                case "12.0":
                    providername = "Microsoft.Ace.OleDb.12.0";
                    break;
                default:
                    providername = "Microsoft.Jet.OleDb.4.0";
                    break;
            }

            return providername;
        }

        private static string GenerateConnectionString(string path, string version, bool hasHeader, bool mex)
        {
            /*
             * 1.Microsoft.Jet.OleDb.4.0 及Excel 8.0 针对EXCEL 2000 或更高版本；Excel 5.0 FOR EXCEL 97
             *   Microsoft.Ace.OleDb.12.0 及Excel 12.0针对 Excel 2007 
             * 2.Excel 8.0 针对EXCEL 2000 或更高版本；Excel 5.0 FOR EXCEL 97
             * 3.HDR == HEADER ROW    表示第一行是否为字段名。Yes为首行字段，No为无首行字段
             * 4.IMEX 表示对同一列中有混合数据类型的列，是统一按字符型处理，还是将个别不同类型的值读为DBNULL。为混合，为不混合
             */

            return string.Format(ConnectionStringFormat,
                version == "8.0" ? "Microsoft.Jet.OleDb.4.0" : "Microsoft.Ace.OleDb.12.0",
                path, version, hasHeader ? "Yes" : "No", mex ? 1 : 2);
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 读取一个Excel文件的sheet至DataTable
        /// </summary>
        /// <param name="sheet">sheet名</param>
        /// <returns>传入sheet的数据表</returns>
        public DataTable GetDataTableBySheet(string sheet)
        {
            if (string.IsNullOrEmpty(sheet))
            {
                sheet = "sheet1$";
            }

            DataSet ds = null;
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();

                OleDbDataAdapter myCommand = null;

                string sql = string.Format("select * from [{0}]", sheet);

                myCommand = new OleDbDataAdapter(sql, _connectionString);
                ds = new DataSet();
                myCommand.Fill(ds);
                myCommand.Dispose();
                conn.Close();
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 读取一个Excel文件至DataSet
        /// </summary>
        /// <returns>包含所有sheet数据的Dataset</returns>
        public DataSet ExcelSheetsToDS()
        {
            DataSet ds = null;

            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                OleDbDataAdapter myCommand = null; 
                ds = new DataSet();
                foreach (string sheet in Sheets)
                {
                    string sql = string.Format("select * from [{0}]", sheet);
                    myCommand = new OleDbDataAdapter(sql, _connectionString);
                    myCommand.Fill(ds, sheet);
                }

                myCommand.Dispose();
                conn.Close();
            }

            return ds;
        }

        /// <summary>
        /// 获取Excel所有工作表的名称
        /// </summary>
        /// <returns>sheet的名称列表</returns>
        public string[] GetSheetNameList()
        {
            DataTable schemaTableView;
            List<string> alData = new List<string>();
            DataTableReader rsResult = null;

            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                //得到全部的表、视图
                schemaTableView = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (schemaTableView != null)
                {
                    rsResult = schemaTableView.CreateDataReader();
                    if (rsResult != null)
                    {
                        if (alData != null)
                        {
                            while (rsResult.Read())
                            {
                                alData.Add(rsResult.GetString(2));  // Table Name;
                            }
                        }
                        rsResult.Close();
                        rsResult = null;
                    }
                    schemaTableView = null;
                }
                conn.Close();
            }
            return alData.ToArray();
        }

        #endregion
    }
}
