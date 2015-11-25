using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GW.Utils
{
    public static class  CommonExtension
    {

        /// <summary>
        /// 判断【List】列表对象是否存在有效值;
        /// 返回值 true:非null并且List.Count>0;
        /// </summary>
        /// <param name="listParams">【List】列表对象</param>
        /// <returns>true:非null并且List.Count>0;</returns>
        public static bool HasValidValues<T>(this IList<T> listParams)
        {
            return listParams != null && listParams.Count > 0;
        }
    }
}
