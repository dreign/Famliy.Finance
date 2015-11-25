using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using GW.Utils;

namespace Famliy.Finance.Common
{
    public class ConfigBusiness
    {
        private static ConfigBusiness configBusiness = new ConfigBusiness();

        /// <summary>
        /// 日期时间格式化
        /// </summary>
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        public const string Session_User_Key = "SysUser";

        public static bool IsInitDB = false;
        public static int PageSize = 10;

        public static string StockAUrl = string.Empty;
        public static string StockHKUrl = string.Empty;
        public static string StockUSUrl = string.Empty;
        public static string StockApikey = string.Empty;


        #region Bank
        /// <summary>
        /// 个人币种明细显示总条数（结束索引-开始索引+1）
        /// </summary>
        public static int BankAccountDetailRedisCount = 10;
        /// <summary>
        /// 个人币种明细开始索引（从1开始，使用时需 -1）
        /// </summary>
        public static int BankAccountDetailRedisStartIndex = 1;
        /// <summary>
        /// 个人币种明细结束索引（从1开始，使用时需 -1）
        /// </summary>
        public static int BankAccountDetailRedisEndIndex = 10;

        /// <summary>
        /// BankAccountTotalRedisCachTime定时器，单位：小时 BankAccountTotalRedisCachTime
        /// </summary>
        public static int BankAccountTotalRedisCachTime = 0;
        /// <summary>
        /// BankAccountDetailRedisCachTime定时器，单位：小时
        /// </summary>
        public static long BankAccountDetailRedisCachTime = 0;

        /// <summary>
        /// 账户加币数量汇总缓存时间，单位：秒 
        /// </summary>
        public static int BankAccountStatsTotalRedisCachTime = 0;

        /// <summary>
        /// 账户加币数量汇总是否保存Redis缓存（Y保存，N不保存）
        /// </summary>
        public static string BankAccountStatsTotalIsRedis = "N";


        /// <summary>
        /// 默认柜台类型编号（Default CloudType）
        /// </summary>
        public static int BankCloudType = 1;

        /// <summary>
        /// 默认智慧币类型
        /// </summary>
        public static int BankDefaultType = 3;

        ///// <summary>
        ///// 筛选入金的记录
        ///// </summary>
        //public static string PlusReasonIDList = string.Empty;

        ///// <summary>
        ///// 筛选出金的记录
        ///// </summary>
        //public static string MinusReasonIDList = string.Empty;

        /// <summary>
        /// 云操盘版柜台列表(逗号分割CloudType值，例如：7,8)，若未指定，则默认是家庭理财师版。
        /// </summary>
        public static string CounterTypeList_Cloud = string.Empty;

        /// <summary>
        /// 家庭理财师柜台默认大赛ID
        /// </summary>
        public static string JC_GameId = string.Empty;
        /// <summary>
        /// 家庭理财师默认柜台交易品种、币种
        /// </summary>
        public static string JC_Currency = string.Empty;
        /// <summary>
        /// 智慧币ReasonId
        /// </summary>
        public static int ZHBReasonId = 372;
        /// <summary>
        /// 金牌投顾培训币ReasonId
        /// </summary>
        public static int JPTGPXBReasonId = 7107;
        /// <summary>
        /// 抢红包ReasonId
        /// </summary>
        public static int RedPaperReasonId = 7128;
        /// <summary>
        /// 抢红包的GameId
        /// </summary>
        public static string BankRedPaper_GameId = "BankRedPaperGameId";
        /// <summary>
        /// 跨行转账的GameId
        /// </summary>
        public static string BankTransfer_GameId = "BankTransferGameId";
        /// <summary>
        /// 有效红包的一页大小
        /// </summary>
        public static int GiftPageSize = 100;
        /// <summary>
        /// 单个红包队列长度
        /// </summary>
        public static int SingleGiftQueueSize = 10000;
        /// <summary>
        /// 有效红包详情的存活时间
        /// </summary>
        public static int GiftDetailSurvivalTime = 600;
        /// <summary>
        /// 有效红包活动配置的存活时间
        /// </summary>
        public static int GiftInfoSurvivalTime = 60;
        /// <summary>
        /// 单个红包的最大额度
        /// </summary>
        public static decimal GiftMaxMoney = 50m;
        /// <summary>
        /// 单个红包的最小额度
        /// </summary>
        public static decimal GiftMinMoney = 0.1m;
        /// <summary>
        /// 上一个红包明细的存活时间
        /// </summary>
        public static long GiftPrevDetailSurvivalTime = 600;

        /// <summary>
        /// 是否启用策略控制，Y启用；N不启用
        /// </summary>
        public static string IsEnableStrategy = "N";
        /// <summary>
        /// 策略表缓存到redis 的过期时间
        /// </summary>
        public static long AmountStrategyRedisCachTime = 0;

        /// <summary>
        /// 用户信息缓存过期时间（单位：秒）
        /// </summary>
        public static double UserInfoCacheTime = 1;

        #endregion

        public ConfigBusiness()
        {
            PageSize = ParseHelper.Parse<int>(ConfigManager.GetAppSettingValue("PageSize"), 10);
            IsInitDB = ParseHelper.Parse<bool>(ConfigManager.GetAppSettingValue("IsInitDB"), false);

            StockAUrl = ConfigManager.GetAppSettingValue("StockAUrl");
            StockHKUrl = ConfigManager.GetAppSettingValue("StockHKUrl");
            StockUSUrl = ConfigManager.GetAppSettingValue("StockUSUrl");
            StockApikey = ConfigManager.GetAppSettingValue("StockApikey");

            #region Bank
            BankAccountDetailRedisStartIndex = ParseHelper.Parse<Int32>(ConfigManager.GetAppSettingValue("BankAccountDetailRedisStartIndex"), 1);
            BankAccountDetailRedisEndIndex = ParseHelper.Parse<Int32>(ConfigManager.GetAppSettingValue("BankAccountDetailRedisEndIndex"), 10);

            BankAccountDetailRedisCount = BankAccountDetailRedisEndIndex - BankAccountDetailRedisStartIndex + 1;

            BankAccountTotalRedisCachTime = ParseHelper.Parse<Int32>(ConfigManager.GetAppSettingValue("BankAccountTotalRedisCachTime"), 4);
            BankAccountDetailRedisCachTime = ParseHelper.Parse<long>(ConfigManager.GetAppSettingValue("BankAccountDetailRedisCachTime"), 12);
            BankAccountStatsTotalRedisCachTime = ParseHelper.Parse<Int32>(ConfigManager.GetAppSettingValue("BankAccountStatsTotalRedisCachTime"), 4);
            BankAccountStatsTotalIsRedis = ConfigManager.GetAppSettingValue("BankAccountStatsTotalIsRedis") == null ? "N" : ConfigManager.GetAppSettingValue("BankAccountStatsTotalIsRedis");

            //CoinAccountDetailRedisCount = ParseHelper.Parse<Int32>(ConfigManager.GetAppSettingValue("CoinAccountDetailRedisCount"), 30);
            BankCloudType = ParseHelper.Parse<Int32>(ConfigManager.GetAppSettingValue("BankCloudType"), 0);
            //PlusReasonIDList = ParseHelper.Parse<string>(ConfigManager.GetAppSettingValue("PlusReasonIDList"));
            //MinusReasonIDList = ParseHelper.Parse<string>(ConfigManager.GetAppSettingValue("MinusReasonIDList"));
            CounterTypeList_Cloud = ParseHelper.Parse<string>(ConfigManager.GetAppSettingValue("CounterTypeList_Cloud"));
            JC_GameId = ParseHelper.Parse<string>(ConfigManager.GetAppSettingValue("JC_GameId"));
            JC_Currency = ParseHelper.Parse<string>(ConfigManager.GetAppSettingValue("JC_Currency"));
            ZHBReasonId = ParseHelper.Parse<int>(ConfigManager.GetAppSettingValue("ZHBReasonId"));
            JPTGPXBReasonId = ParseHelper.Parse<int>(ConfigManager.GetAppSettingValue("JPTGPXBReasonId"));
            GiftPageSize = ParseHelper.Parse<int>(ConfigManager.GetAppSettingValue("GiftPageSize"));
            GiftDetailSurvivalTime = ParseHelper.Parse<int>(ConfigManager.GetAppSettingValue("GiftDetailSurvivalTime"));
            GiftInfoSurvivalTime = ParseHelper.Parse<int>(ConfigManager.GetAppSettingValue("GiftInfoSurvivalTime"));
            SingleGiftQueueSize = ParseHelper.Parse<int>(ConfigManager.GetAppSettingValue("SingleGiftQueueSize"));
            GiftMaxMoney = ParseHelper.Parse<decimal>(ConfigManager.GetAppSettingValue("GiftMaxMoney"));
            GiftMinMoney = ParseHelper.Parse<decimal>(ConfigManager.GetAppSettingValue("GiftMinMoney"));
            GiftPrevDetailSurvivalTime = ParseHelper.Parse<int>(ConfigManager.GetAppSettingValue("GiftPrevDetailSurvivalTime"));
            AmountStrategyRedisCachTime = ParseHelper.Parse<long>(ConfigManager.GetAppSettingValue("AmountStrategyRedisCachTime"), 86400);
            IsEnableStrategy = ConfigManager.GetAppSettingValue("IsEnableStrategy") == null ? "N" : ConfigManager.GetAppSettingValue("IsEnableStrategy");
            UserInfoCacheTime = ParseHelper.Parse<double>(ConfigManager.GetAppSettingValue("UserInfoCacheTime"), 1);
            #endregion

        }
    }
}
