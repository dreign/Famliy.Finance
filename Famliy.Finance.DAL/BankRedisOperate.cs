using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famliy.Finance.Models;
using Famliy.Finance.Common;
using GW.Redis.Common;
using GW.Redis;
using System.Diagnostics;

namespace Famliy.Finance.DAL
{

    /// <summary>
    /// redis操作类
    /// </summary>
    public class BankRedisOperate
    {
        /// <summary>
        /// 用户资金账户设置
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SetBankAccountRedis(int typeId, bank_account model)
        {
            var redisDetailNameKey = string.Empty;
            bool flag = false;
            redisDetailNameKey = string.Format(RedisCacheKey.BankTotal_UserName, model.user_name);
            var redisCachTime = ConfigBusiness.BankAccountTotalRedisCachTime;
            var overTime = DateTime.Now.AddHours(redisCachTime);
            try
            {
                flag = RedisNetHelper.Set<decimal>(typeId, redisDetailNameKey, model.money, overTime).Item1;
            }
            catch (Exception ex)
            {
                GW.Utils.LogHelper.Write(string.Format("{0}.{1}() error;StackTrace:{2} ", GetType().FullName, new StackFrame(0).GetMethod().Name, ex.StackTrace), ex);
            }
            return flag;
        }
    }
}
