using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GW.Redis
{
    /// <summary>
    /// 接口返回状态
    /// </summary>
    [Serializable]
    public enum InterfaceStatus
    {
        #region 通用返回值

        /// <summary>
        /// 错误的接口名称
        /// </summary>
        [XmlEnum("-3")]
        Wrong_Interface_Name = -3,

        /// <summary>
        /// 错误。一般是异常退出程序
        /// </summary>
        [XmlEnum("-2")]
        Error = -2,

        /// <summary>
        /// 失败
        /// </summary>
        [XmlEnum("-1")]
        Faile = -1,

        /// <summary>
        /// 成功
        /// </summary>
        [XmlEnum("0")]
        Success = 0,

        /// <summary>
        /// 非法IP
        /// </summary>
        [XmlEnum("2")]
        IllegalIp = 2,

        /// <summary>
        /// 无权限，加密验证失败
        /// </summary>
        [XmlEnum("3")]
        No_Access = 3,

        /// <summary>
        /// 传入参数不完整或者关键字KEY的字符串拼写错误，无法解析。
        /// </summary>
        [XmlEnum("4")]
        Parm_Missing = 4,
        /// <summary>
        /// 传入参数不完整或者关键字KEY的字符串拼写错误，无法解析。
        /// </summary>
        [XmlEnum("5")]
        Token_Faild = 5,
        /// <summary>
        /// 访问太频繁
        /// </summary>
        [XmlEnum("6")]
        Too_Frequently_Visit = 6,

        /// <summary>
        /// 参数异常（参数值不正常）
        /// </summary>
        [XmlEnum("10")]
        Abnormal = 10,


        /// <summary>
        /// 用户名为空
        /// </summary>
        [XmlEnum("11")]
        UserName_Null = 11,

        #endregion
    }

}
