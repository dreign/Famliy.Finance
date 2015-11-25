using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW.Redis.Common
{
    /// <summary>
    /// Redis缓存键模板静态类
    /// <para>
    /// 模板格式说明 --> 表名:列名1:列值1[:列名n:列值n],模板中表名、列名均为小写
    /// </para>
    /// </summary>
    /// <author>kevin</author>
    /// <createDate>2014-04-14</createDate>
    public static class RedisCacheKey
    {
        #region 队列
        public static string QueueActionList = "GW.Cloud.QueueActionList";
        public static string QueueCoinOprationLogList = "GW.Cloud.QueueCoinOprationLogList";
        public static string QueueErrorActionList = "GW.Cloud.QueueErrorActionList";
        #endregion

        #region 对应表:[Cloud_ThirdPartyAccount] 的Redis缓存键模板

        /// <summary>
        /// 以UserName作为缓存键模板
        /// </summary>
        public static string CloudThirdPartyAccount_UserName = "cloudthirdpartyaccount:username:{0}";
        #endregion

        #region Bank模板
        /// <summary>
        /// 币种详细流水查询模板
        /// </summary>
        public static string BankDetail = "bankdetail:username:{0}";
        /// <summary>
        /// 币种总数查询模板
        /// </summary>
        public static string BankTotal_UserName = "banktotal:username:{0}";
        /// <summary>
        /// Order ID 判重模板
        /// </summary>
        public static string Bank_IsOrderInHash = "bank_isorderinhash";
        /// <summary>
        /// OSN判重模板
        /// </summary>
        public static string Bank_IsOSNInHash = "bank_isosninhash";

        /// <summary>
        /// 币种统计总数查询模板
        /// </summary>
        public static string StatsTotal_UserName = "statstotal:username:{0}";
        /// <summary>
        /// 红包用户
        /// </summary>
        public static string BankRedPaper_User_UserName = "bankredpaper:username:{0}:{1}";
        /// <summary>
        /// 红包配置
        /// </summary>
        public static string BankRedPaper_Event = "bankredpaper:event:{0}";
        /// <summary>
        /// 红包分配队列
        /// </summary>
        public static string BankRedPaper_Event_Queue = "bankredpaperevent:queue:{0}";
        /// <summary>
        /// 红包并发的状态位
        /// </summary>
        public static string BankRedPaper_Flag = "bankredpaper:flag:{0}";
        /// <summary>
        /// 红包明细
        /// </summary>
        public static string BankRedPaper_UserList = "bankredpaper:userlist:{0}";
        /// <summary>
        /// 返回给前端的当前活动红包配置
        /// </summary>
        public static string BankRedPaper_GiftInfo = "bankredpaper:eventconfig:typeid:{0}";
        /// <summary>
        /// 前一个红包活动的红包明细(手气王)
        /// </summary>
        public static string BankRedPaper_Prev_UserList = "bankredpaper:prev:userlist:{0}";
        /// <summary>
        /// 奖品配置
        /// </summary>
        public static string Surprise_Config = "surprise:config:{0}";
        /// <summary>
        /// 当前奖品
        /// </summary>
        public static string Surprise_Config_Current = "surprise:config:current";
        /// <summary>
        /// 奖品队列
        /// </summary>
        public static string Surprise_Config_Queue = "surprise:config:queue:{0}";
        /// <summary>
        /// 奖品用户
        /// </summary>
        public static string Surprise_Config_User = "surprise:config:user:{0}:{1}";




        #endregion

        #region 积分模板
        /// <summary>
        /// PointDetail_userId_type_dir
        /// </summary>
        public static string PointDetail = "getpointdetail:userid:{0}username:{1}type:{2}dir:{3}";

        public static string PointTotal_UserName = "pointtotal:username:{0}";

        public static string PointTotal_UserID = "pointtotal:userid:{0}";
        #endregion

        #region redis智慧币key模板
        public static string BTC_CoinAccount_Total_UserName = "coinAccount:total:username:{0}";

        public static string BTC_CoinOperateLog = "coinDetail:username:{0}";

        public static string Coin_IsOrderInList = "Coin_IsOrderInList";

        public static string Coin_IsOSNInList = "Coin_IsOSNInList";
        #endregion

        #region Redis中用户模板
        public static string GW_Service_Account = "userName:{0}";
        #endregion

        #region Redis中OSN模板
        public static string OSNList = "osn:{0}";
        #endregion

        #region 多重虚拟币中智慧币模板
        /// <summary>
        /// 智慧币总量控制策略，总配额，模板
        /// 参数0：币种编号
        /// 参数1：【总量类型编号。1商户；2渠道；3reasonID；4科目；】）需要再加前缀。
        /// 参数2：实际的编号（参数1指示的字典表中的编号）
        /// redis中示例  键 1:amountstrategy:1:2    值 200000
        /// </summary>
        public static string AmountStrategy = "{0}:{1}:{2}:amount";
        /// <summary>
        /// 智慧币总量控制策略，已分配智慧币，模板
        /// 参数0：币种编号
        /// 参数1：【总量类型编号。1商户；2渠道；3reasonID；4科目；】）需要再加前缀。
        /// 参数2：实际的编号（参数1指示的字典表中的编号）
        /// redis中示例  键 1:amountstrategy:1:2    值 200000
        /// </summary>
        public static string AssignedAmount = "{0}:{1}:{2}:assigned";
 
        #endregion

        #region 加币策略配置
        /// <summary>
        ///  策略配置
        ///  rule:reasonid:3:7110
        ///  3 币种编号，7110 reasonid
        /// </summary>
        public static string Bank_rule_reasonid = "rule:reasonid:{0}:{1}";
        /// <summary>
        ///  用户策略次数
        ///  rule:3:username:1
        ///  3 数据库，username 用户名称，1 策略 id
        /// </summary>
        public static string Bank_rule_user = "rule:username:{0}:{1}:{2}";
        #endregion

        #region api_oauth
        public static string App_ApiResource = "OAuth:App:ApiResource:{0}";

        public static string App_Token = "OAuth:App:Token:{0}";

        public static string App_ServerConfig = "OAuth:App:ServerConfig";

        public static string App_ISV = "OAuth:AppISV:{0}:{1}";
        #endregion
    }

    ///// <summary>
    ///// Redis键的项目前缀(目前尚未启用)
    ///// </summary>
    //public enum RedisPrefixNameEnum
    //{
    //    /// <summary>
    //    /// 操盘大师
    //    /// </summary>
    //    Cpds = 100,
    //    /// <summary>
    //    /// 云操盘
    //    /// </summary>
    //    Cloud = 200,
    //    /// <summary>
    //    /// 智慧币
    //    /// </summary>
    //    Coin = 300,
    //    /// <summary>
    //    /// 积分
    //    /// </summary>
    //    Jifen = 400,
    //    /// <summary>
    //    /// 积分后台管理
    //    /// </summary>
    //    JFManager = 500,
    //    /// <summary>
    //    /// 高手汇
    //    /// </summary>
    //    Heroes = 600
    //}

}
