using Famliy.Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Famliy.Finance.BLL
{
    /// <summary>
    /// 上下文简单工厂
    /// <remarks>
    /// 
    /// </remarks>
    /// </summary>
    public class ContextFactory
    {
        /// <summary>
        /// 获取当前数据上下文
        /// </summary>
        /// <returns>数据上下文</returns>
        public static BankModel GetCurrentContext()
        {
            BankModel dbContext = CallContext.GetData("BankModel") as BankModel;
            if (dbContext == null)
            {
                dbContext = new BankModel();
                CallContext.SetData("BankModel", dbContext);
            }
            return dbContext;
        }       
    }
}
