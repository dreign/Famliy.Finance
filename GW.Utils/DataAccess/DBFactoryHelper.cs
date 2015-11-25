//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   数据处理工厂,数据库处理入口
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
using System.Xml;
using System.Configuration;

namespace GW.Utils.DataAccess
{
    /// <summary>
    /// 数据访问工厂
    /// </summary>
    public class DBFactoryHelper
    {
        /// <summary>
        /// 工厂：配置数据库插件
        /// </summary>
        /// <param name="dataType">数据库类型</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>IDBHelper，工厂接口</returns>
        public static IDBHelper GetDatabase(DatabaseType dataType, string connectionString)
        {
            //这里添加配置数据库对象工厂实例
            switch (dataType)
            {
                case DatabaseType.SqlServer:
                    return new SqlDB(connectionString);
                case DatabaseType.Oracle:
                    return new OracleDB(connectionString);
                default:
                    return null;
            }

        }
    }
}
