using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GW.Redis
{

    /// <summary>
    /// 数据库对象实体 与 Api接口返回对象实体 转化接口
    /// </summary>
    /// <typeparam name="T">泛型实体类</typeparam>
    /// <author>yanglin 20140813</author>
    public interface IConvertApiEntity<T> where T : class , new()
    {
        /// <summary>
        /// 将数据库对象实体 转换成 Api接口返回对象实体
        /// </summary>
        /// <returns></returns>
        T ConvertToApiEntity();
    }

    [Serializable]
    [DataContract]
    public class ApiResponseEntity
    {
        public ApiResponseEntity()
        { }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="data"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public ApiResponseEntity(object bizData, InterfaceStatus status, string message)
        {
            BizData = bizData;
            Status = status.GetHashCode();
            Message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bizData"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public ApiResponseEntity(object bizData, Int32 status, string message)
        {
            BizData = bizData;
            Status = status;
            Message = message;
        }
        [DataMember]
        /// <summary>
        /// 返回数据实体(动态对象)
        /// </summary>
        public Object BizData { get; set; }
        [DataMember]
        /// <summary>
        /// 返回状态,默认成功:0;失败：-1 
        /// </summary>
        public Int32 Status { get; set; }
        [DataMember]
        /// <summary>
        /// 返回描述消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 创建ApiResponseEntity实体
        /// </summary>
        /// <param name="data">返回数据实体</param>
        /// <param name="status">返回状态,默认成功:0;失败：-1</param>
        /// <param name="message">返回描述消息</param>
        /// <returns></returns>
        public static ApiResponseEntity GetEntity(object bizData, InterfaceStatus status, string message = "成功")
        {
            return new ApiResponseEntity(bizData, status, message);
        }

        /// <summary>
        /// 创建ApiResponseEntity实体
        /// </summary>
        /// <param name="status">返回状态,默认成功:0;失败：-1</param>
        /// <param name="message">返回描述消息</param>
        /// <returns></returns>
        public static ApiResponseEntity GetEntity(InterfaceStatus status, string message = "成功")
        {
            return new ApiResponseEntity(null, status, message);
        }

        /// <summary>
        /// 创建ApiResponseEntity实体
        /// </summary>
        /// <param name="status">返回状态码值(各个相关系统中返回状态值)</param>
        /// <param name="message">返回描述消息</param>
        /// <returns></returns>
        public static ApiResponseEntity GetEntity(Int32 status, string message = "成功")
        {
            return new ApiResponseEntity(null, status, message);
        }

        /// <summary>
        /// 创建ApiResponseEntity实体
        /// </summary>
        /// <param name="bizData">返回数据实体</param>
        /// <param name="message">返回描述消息(默认:成功)</param>
        /// <returns></returns>
        public static ApiResponseEntity GetSuccessEntity(object bizData, string message = "成功")
        {
            return new ApiResponseEntity(bizData, InterfaceStatus.Success, message);
        }
        /// <summary>
        /// 创建ApiResponseEntity实体
        /// </summary>
        /// <param name="message">返回描述消息(默认:失败)</param>
        /// <returns></returns>
        public static ApiResponseEntity GetFaileEntity(string message = "失败")
        {
            return new ApiResponseEntity(null, InterfaceStatus.Faile, message);
        }
    }

    /// <summary>
    /// 函数统一返回值泛型类
    /// </summary>
    /// <typeparam name="T">泛型实体类</typeparam>
    [Serializable]
    [DataContract]
    public class ApiResponseEntity<T>
    {
        public ApiResponseEntity() { }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="bizData"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public ApiResponseEntity(T bizData, InterfaceStatus status, string message)
        {
            BizData = bizData;
            Status = status.GetHashCode();
            Message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bizData"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public ApiResponseEntity(T bizData, Int32 status, string message)
        {
            BizData = bizData;
            Status = status;
            Message = message;
        }

       
        /// <summary>
        /// 返回数据实体(泛型对象)
        /// </summary>
        [DataMember]
        public T BizData { get; set; }

        /// <summary>
        /// 返回状态,默认成功:0;失败：-1 
        /// </summary>
        [DataMember]
        public Int32 Status { get; set; }

      
        /// <summary>
        /// 返回描述消息
        /// </summary>
        [DataMember]
        public string Message { get; set; }



        /// <summary>
        /// 创建ApiResponseEntity实体
        /// </summary>
        /// <param name="bizData">返回数据实体</param>
        /// <param name="status">返回状态,默认成功:0;失败：-1</param>
        /// <param name="message">返回描述消息</param>
        /// <returns></returns>
        public static ApiResponseEntity<T> GetEntity(T bizData, InterfaceStatus status, string message = "成功")
        {
            return new ApiResponseEntity<T>(bizData, status, message);
        }

        /// <summary>
        /// 创建ApiResponseEntity实体
        /// </summary>
        /// <param name="status">返回状态,默认成功:0;失败：-1</param>
        /// <param name="message">返回描述消息</param>
        /// <returns></returns>
        public static ApiResponseEntity<T> GetEntity(InterfaceStatus status, string message = "成功")
        {
            return new ApiResponseEntity<T>(default(T), status, message);
        }

        /// <summary>
        /// 创建ApiResponseEntity[T]实体
        /// </summary>
        /// <param name="status">返回状态码值(各个相关系统中返回状态值)</param>
        /// <param name="message">返回描述消息</param>
        /// <returns></returns>
        public static ApiResponseEntity<T> GetEntity(Int32 status, string message = "成功")
        {
            return new ApiResponseEntity<T>(default(T), status, message);
        }

        /// <summary>
        /// 创建ApiResponseEntity实体
        /// </summary>
        /// <param name="bizData">返回数据实体</param>
        /// <param name="message">返回描述消息(默认:成功)</param>
        /// <returns></returns>
        public static ApiResponseEntity<T> GetSuccessEntity(T bizData, string message = "成功")
        {
            return new ApiResponseEntity<T>(bizData, InterfaceStatus.Success, message);
        }
        /// <summary>
        /// 创建ApiResponseEntity实体
        /// </summary>
        /// <param name="message">返回描述消息(默认:失败)</param>
        /// <returns></returns>
        public static ApiResponseEntity<T> GetFaileEntity(string message = "失败")
        {
            return new ApiResponseEntity<T>(default(T), InterfaceStatus.Faile, message);
        }
    }




}
