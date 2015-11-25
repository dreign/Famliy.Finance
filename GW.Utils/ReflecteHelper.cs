//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   反射处理类
//编写日期    :   2010-11-22
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;

using GW.Utils.Web;

namespace GW.Utils
{
    /// <summary>
    /// 反射处理类
    /// </summary>
    public sealed class ReflecteHelper
    {
        private const string CONST_ASSEMBLECACHE = "AssembleCache";
        private const string CONST_ERRORMESSAGE = "通过反射动态调用{0}.{1}的时候发生异常！";

        #region 对象实例化

        /// <summary>
        /// 创建指定类型的对象实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="args">构造函数参数</param>
        /// <returns>对象实例</returns>
        public static object CreateInstance(Type type, object[] args)
        {
            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// 从指定程序集中创建对象实例
        /// </summary>
        /// <param name="assemblyName">要创建类所在的程序集</param>
        /// <param name="className">要创建类的名称</param>
        /// <param name="ignoreCase">是否忽略要创建类名称的大小写</param>
        /// <param name="args">类创建的构造函数的参数</param>
        /// <returns>创建出的对象实例</returns>
        public static object CreateInstance(string assemblyName, string className, bool ignoreCase, object[] args)
        {
            Assembly assembly = GetAssembly(CONST_ASSEMBLECACHE, assemblyName);
            Type type = assembly.GetType(className, true, ignoreCase);
            return CreateInstance(type, args);
        }

        /// <summary>
        /// 从指定程序集中创建对象实例,本方法严格区分对象名称的大小写
        /// </summary>
        /// <param name="assemblyName">要创建类所在的程序集</param>
        /// <param name="className">要创建类的名称</param>
        /// <param name="args">类创建的构造函数的参数</param>
        /// <returns>创建出的对象实例</returns>
        public static object CreateInstance(string assemblyName, string className, object[] args)
        {
            return CreateInstance(assemblyName, className, false, args);
        }

        #endregion

        #region 方法反射

        /// <summary>
        /// 取得类型特定名称和参数类型的函数（方法）
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="methodName">函数名称</param>
        /// <param name="bindingAttribute">A bitmask comprised of one or more System.Reflection.BindingFlags that specify how the search is conducted.  -or- Zero, to return null.</param>
        /// <param name="argTypes">参数类型数组，如果没有参数，传入null或者Type[0]都可以</param>
        /// <returns>特定函数MethodInfo</returns>
        public static MethodInfo GetMethod(Type type, string methodName, BindingFlags bindingAttribute, Type[] argTypes)
        {
            if (argTypes == null)
                argTypes = new Type[0];
            return type.GetMethod(methodName, bindingAttribute, null, argTypes, null);
        }

        /// <summary>
        /// 获取方法的参数
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="methodName">函数名称</param>
        /// <returns>方法的参数列表</returns>
        public static ParameterInfo[] GetMethodParameter(Type type, string methodName)
        {
            return GetMethodParameter(type, methodName, BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// 获取方法的参数
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="methodName">函数名称</param>
        /// <param name="bindingAttribute">A bitmask comprised of one or more System.Reflection.BindingFlags that specify how the search is conducted.  -or- Zero, to return null</param>
        /// <returns>方法的参数列表</returns>
        public static ParameterInfo[] GetMethodParameter(Type type, string methodName, BindingFlags bindingAttribute)
        {
            MethodInfo methodInfo = type.GetMethod(methodName, bindingAttribute);

            if (methodInfo != null)
            {
                return methodInfo.GetParameters();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 检查类型是否存在特定名称和参数类型的函数（方法）
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="methodName">函数名称</param>
        /// <param name="bindingAttribute">A bitmask comprised of one or more System.Reflection.BindingFlags that specify how the search is conducted.  -or- Zero, to return null.</param>
        /// <param name="argTypes">参数类型数组，如果没有参数，传入null或者Type[0]都可以</param>
        /// <returns>是否存在特定函数</returns>
        public static bool IsMethodExist(Type type, string methodName, BindingFlags bindingAttribute, Type[] argTypes)
        {
            return (GetMethod(type, methodName, bindingAttribute, argTypes) != null);
        }

        /// <summary>
        /// 检查类型是否存在特定名称和参数类型的成员公开函数（方法）
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="methodName">函数名称</param>
        /// <param name="argTypes">参数类型数组，如果没有参数，传入null或者Type[0]都可以</param>
        /// <returns>是否存在特定函数</returns>
        public static bool IsInstancePublicMethodExist(Type type, string methodName, Type[] argTypes)
        {
            return IsMethodExist(type, methodName, BindingFlags.Public | BindingFlags.Instance, argTypes);
        }

        /// <summary>
        /// 检查对象是否存在特定名称和参数类型的成员公开函数（方法）
        /// </summary>
        /// <param name="instance">对象实例</param>
        /// <param name="methodName">函数名称</param>
        /// <param name="argTypes">参数类型数组，如果没有参数，传入null或者Type[0]都可以</param>
        /// <returns>是否存在特定函数</returns>
        public static bool IsInstancePublicMethodExist(object instance, string methodName, Type[] argTypes)
        {
            return IsInstancePublicMethodExist(instance.GetType(), methodName, argTypes);
        }

        /// <summary>
        /// 检查类是否存在特定名称和参数类型的成员公开函数（方法）
        /// </summary>
        /// <param name="assemblyName">类所在的程序集</param>
        /// <param name="className">类的名称</param>
        /// <param name="ignoreCase">是否忽略类名称的大小写</param>
        /// <param name="methodName">函数名称</param>
        /// <param name="argTypes">参数类型数组，如果没有参数，传入null或者Type[0]都可以</param>
        /// <returns>是否存在特定函数</returns>
        public static bool IsInstancePublicMethodExist(string assemblyName, string className, bool ignoreCase, string methodName, Type[] argTypes)
        {
            Assembly assembly = GetAssembly(CONST_ASSEMBLECACHE, assemblyName);
            return IsInstancePublicMethodExist(assembly.GetType(className, true, ignoreCase), methodName, argTypes);
        }

        /// <summary>
        /// 取程序集，如果有缓存，直接从缓存取，如果没有，则载入
        /// </summary>
        /// <param name="cacheName">指定缓存名称</param>
        /// <param name="assemblyName">载入的程序集</param>
        /// <returns>程序集</returns>
        private static Assembly GetAssembly(string cacheName, string assemblyName)
        {
            object assemblyObject = CacheHelper.GetCache(cacheName);

            if (assemblyObject == null)
            {
                Assembly assembly = null;
                assembly = Assembly.Load(assemblyName);
                CacheHelper.SetCache(cacheName, assembly, DateTime.Now.AddMinutes(60));
                return assembly;
            }
            else
            {
                return (Assembly)assemblyObject;
            }
        }

        /// <summary>
        /// 检查类是否存在特定名称和参数类型的成员公开函数（方法），类名大小写敏感
        /// </summary>
        /// <param name="assemblyName">类所在的程序集</param>
        /// <param name="className">类的名称</param>
        /// <param name="methodName">函数名称</param>
        /// <param name="argTypes">参数类型数组，如果没有参数，传入null或者Type[0]都可以</param>
        /// <returns>是否存在特定函数</returns>
        public static bool IsInstancePublicMethodExist(string assemblyName, string className, string methodName, Type[] argTypes)
        {
            return IsInstancePublicMethodExist(assemblyName, className, false, methodName, argTypes);
        }

        /// <summary>
        /// 检查类是否存在特定名称和参数类型的静态公开函数（方法）
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="methodName">函数名称</param>
        /// <param name="argTypes">参数类型数组，如果没有参数，传入null或者Type[0]都可以</param>
        /// <returns>是否存在特定函数</returns>
        public static bool IsStaticPublicMethodExist(Type type, string methodName, Type[] argTypes)
        {
            return IsMethodExist(type, methodName, BindingFlags.Public | BindingFlags.Static, argTypes);
        }

        /// <summary>
        /// 检查类是否存在特定名称和参数类型的静态公开函数（方法）
        /// </summary>
        /// <param name="assemblyName">类所在的程序集</param>
        /// <param name="className">类的名称</param>
        /// <param name="ignoreCase">是否忽略类名称的大小写</param>
        /// <param name="methodName">函数名称</param>
        /// <param name="argTypes">参数类型数组，如果没有参数，传入null或者Type[0]都可以</param>
        /// <returns>是否存在特定静态函数</returns>
        public static bool IsStaticPublicMethodExist(string assemblyName, string className, bool ignoreCase, string methodName, Type[] argTypes)
        {
            //Assembly assembly = Assembly.Load(assemblyName);            
            Assembly assembly = GetAssembly(CONST_ASSEMBLECACHE, assemblyName);
            Type type = assembly.GetType(className, true, ignoreCase);
            return IsStaticPublicMethodExist(type, methodName, argTypes);
        }

        /// <summary>
        /// 检查类是否存在特定名称和参数类型的静态公开函数（方法），类名大小写敏感
        /// </summary>
        /// <param name="assemblyName">类所在的程序集</param>
        /// <param name="className">类的名称</param>
        /// <param name="methodName">函数名称</param>
        /// <param name="argTypes">参数类型数组，如果没有参数，传入null或者Type[0]都可以</param>
        /// <returns>是否存在特定静态函数</returns>
        public static bool IsStaticPublicMethodExist(string assemblyName, string className, string methodName, Type[] argTypes)
        {
            return IsStaticPublicMethodExist(assemblyName, className, false, methodName, argTypes);
        }

        /// <summary>
        /// 执行传入类型或对象的指定名称和参数的方法，并返回执行结果
        /// </summary>
        /// <param name="type">要执行方法的类型</param>
        /// <param name="_Object">要执行方法的对象</param>
        /// <param name="methodName">要执行的方法名称，严格区分大小写</param>
        /// <param name="bindingAttribute">A bitmask comprised of one or more System.Reflection.BindingFlags that specify how the search is conducted.  -or- Zero, to return null.</param>
        /// <param name="args">要执行的方法的参数集</param>
        /// <returns>执行成功,返回此方法的执行结果，不成功，丢出AppException异常,或函数执行时的异常</returns>
        private static object InvokeMethod(Type type, object obj, string methodName, BindingFlags bindingAttribute, object[] args)
        {
            Type[] argTypes = null;
            if (args != null)
            {
                argTypes = new Type[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    argTypes[i] = args[i].GetType();
                }
            }
            else
            {
                argTypes = new Type[0];
            }

            MethodInfo mi = GetMethod(type, methodName, bindingAttribute, argTypes);
            if (mi != null)
            {
                try
                {
                    return mi.Invoke(obj, args);
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException != null)
                    {
                        LogHelper.Write(string.Format(CONST_ERRORMESSAGE, type.Name, methodName), ex.InnerException);
                        throw ex.InnerException;
                    }
                    else
                    {
                        LogHelper.Write(string.Format(CONST_ERRORMESSAGE, type.Name, methodName), ex);
                        throw ex;
                    }
                }
            }
            throw new MissingMethodException(type.Name, methodName);
        }

        /// <summary>
        /// 执行传入对象的指定名称和参数的方法，并返回执行结果
        /// </summary>
        /// <param name="instance">要执行方法的对象</param>
        /// <param name="methodName">要执行的方法名称，严格区分大小写</param>
        /// <param name="args">要执行的方法的参数集</param>
        /// <returns>
        /// 执行成功,返回此方法的执行结果，不成功，丢出AppException异常,或函数执行时的异常
        /// </returns>
        public static object InvokeMethod(object instance, string methodName, object[] args)
        {
            return InvokeMethod(instance.GetType(), instance, methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, args);
        }

        /// <summary>
        /// 执行传入类型的指定名称和参数的静态方法，并返回执行结果
        /// </summary>
        /// <param name="type">要执行方法的类型</param>
        /// <param name="methodName">要执行的方法名称，严格区分大小写</param>
        /// <param name="args">要执行的方法的参数集</param>
        /// <returns>执行成功,返回此方法的执行结果，不成功，丢出AppException异常,或函数执行时的异常
        /// </returns>
        public static object InvokeMethod(Type type, string methodName, object[] args)
        {
            return InvokeMethod(type, null, methodName, BindingFlags.Public | BindingFlags.Static, args);
        }

        /// <summary>
        ///  从指定程序集的指定类中，执行指定的静态方法,并返回函数执行的结果
        /// </summary>
        /// <param name="assemblyName">要创建类所在的程序集</param>
        /// <param name="className">要创建类的名称</param>
        /// <param name="ignoreCase">是否忽略类名称的大小写</param>
        /// <param name="methodName">要执行的方法名称</param>
        /// <param name="args">要执行的方法的参数集</param>
        /// <returns>
        /// 执行成功,返回此方法的执行结果，不成功，丢出AppException异常,或函数执行时的异常
        /// </returns>
        public static object InvokeMethod(string assemblyName, string className, bool ignoreCase, string methodName, object[] args)
        {
            //Assembly assembly = Assembly.Load(assemblyName);
            Assembly assembly = GetAssembly(CONST_ASSEMBLECACHE, assemblyName);
            Type type = assembly.GetType(className, true, ignoreCase);

            return InvokeMethod(type, null, methodName, BindingFlags.Public | BindingFlags.Static, args);
        }

        /// <summary>
        ///  从指定程序集的指定类中，执行指定的静态方法,并返回函数执行的结果,类名称严格区分大小写,函数名称严格区分大小写.
        /// </summary>
        /// <param name="assemblyName">类所在的程序集</param>
        /// <param name="className">类的名称</param>
        /// <param name="methodName">要执行的方法名称</param>
        /// <param name="args">要执行的方法的参数集</param>
        /// <returns>
        /// 执行成功,返回此方法的执行结果，不成功，丢出AppException异常,或函数执行时的异常
        /// </returns>
        public static object InvokeMethod(string assemblyName, string className, string methodName, object[] args)
        {
            return InvokeMethod(assemblyName, className, false, methodName, args);
        }

        #endregion

        #region 属性反射

        /// <summary>
        /// 将原对象的属性复制到目标对象的属性上，也可以将IDictionary对象的key作为目标对象的属性进行复制。不支持嵌套复制属性
        /// 映射规则：属性名相同且类型相同(CaseSensitive)
        /// </summary>
        /// <param name="destObject">目标对象</param>
        /// <param name="srcObject">源对象</param>
        public static void CopyProperties(object destObject, object srcObject)
        {
            if (destObject != null && srcObject != null)
            {
                if (srcObject is IDictionary)
                {
                    IDictionary origDict = (IDictionary)srcObject;
                    foreach (string key in origDict.Keys)
                    {
                        object val = origDict[key];
                        PropertyInfo destPropInfo = GetPropertyInfo(destObject, key, null);
                        if (destPropInfo != null && destPropInfo.PropertyType == val.GetType())
                        {
                            if (destPropInfo.CanWrite)
                            {
                                destPropInfo.SetValue(destObject, val, null);
                            }
                        }
                    }
                }
                else
                {
                    Type _origType = srcObject.GetType();

                    PropertyInfo[] origProps = _origType.GetProperties();
                    foreach (PropertyInfo origProp in origProps)
                    {
                        PropertyInfo destPropInfo = GetPropertyInfo(destObject, origProp.Name, null);
                        if (destPropInfo != null && destPropInfo.PropertyType == origProp.PropertyType)
                        {
                            if (destPropInfo.CanWrite && origProp.CanRead)
                            {
                                destPropInfo.SetValue(destObject, origProp.GetValue(srcObject, null), null);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 在指定对象指定属性上设置值。
        /// 属性支持嵌套.( bean.prop1.prop2.prop3 )
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <returns>true:设置成功; false:设置失败</returns>
        public static bool SetProperty(object obj, string propName, object value, object[] index)
        {
            bool isOK = false;

            if (obj != null && !string.IsNullOrEmpty(propName))
            {
                string[] propNames = propName.Split('.');
                string name = propNames[0];
                PropertyInfo propInfo = obj.GetType().GetProperty(name);

                if (propInfo != null)
                {
                    isOK = true;

                    if (propNames.Length > 1)
                    {
                        object propVal = propInfo.GetValue(obj, index);
                        if (propVal == null)
                        {
                            propVal = Activator.CreateInstance(propInfo.PropertyType);
                        }
                        SetProperty(propVal, StringHelper.SubstringAfter(propName, "."), value, index);
                    }
                    else
                    {
                        if (propInfo.CanWrite)
                        {
                            propInfo.SetValue(obj, Convert.ChangeType(value, propInfo.PropertyType), index);
                        }
                    }
                }
            }

            return isOK;
        }

        /// <summary>
        /// 获取指定对象属性的值。
        /// 属性支持嵌套.( bean.prop1.prop2.prop3 )
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propName">属性名</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <returns>属性的值。[ 如果没有获取则返回null ]</returns>
        public static object GetProperty(object obj, string propName, object[] index)
        {
            object val = null;
            if (obj != null && !string.IsNullOrEmpty(propName))
            {
                string[] propNames = propName.Split('.');
                string name = propNames[0];
                PropertyInfo propInfo = obj.GetType().GetProperty(name);

                if (propNames.Length > 1)
                {
                    val = GetProperty(propInfo.GetValue(obj, index), StringHelper.SubstringAfter(propName, "."), index);
                }
                else
                {
                    if (propInfo.CanRead)
                    {
                        val = propInfo.GetValue(obj, index);
                    }
                }
            }

            return val;
        }

        /// <summary>
        /// 获取指定对象属性信息。
        /// 属性支持嵌套.( bean.prop1.prop2.prop3 )
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propName">属性名</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <returns>属性描述信息。[ 如果没有获取则返回null ]</returns>
        public static PropertyInfo GetPropertyInfo(object obj, string propName, object[] index)
        {
            PropertyInfo val = null;
            if (obj != null && !string.IsNullOrEmpty(propName))
            {
                Type type = obj.GetType();

                string[] propNames = propName.Split('.');
                string name = propNames[0];
                PropertyInfo propInfo = type.GetProperty(name);

                if (propNames.Length > 1)
                {
                    if (propInfo != null)
                    {
                        val = GetPropertyInfo(propInfo.GetValue(obj, index), StringHelper.SubstringAfter(propName, "."), index);
                    }
                }
                else
                {
                    val = propInfo;
                }
            }

            return val;
        }

        /// <summary>
        /// 指定对象属性是否存在。
        /// 属性支持嵌套.( bean.prop1.prop2.prop3 )
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propName">属性名</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <returns>true:存在; false:不存在</returns>
        public static bool IsPropertyExist(object obj, string propName, object[] index)
        {
            return (GetPropertyInfo(obj, propName, index) != null);
        }

        #endregion
    }
}
