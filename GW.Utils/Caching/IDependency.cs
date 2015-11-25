//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   依赖关系接口
//编写日期    :   2010-12-15
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace GW.Utils.Caching
{
    /// <summary>
    /// 依赖关系接口
    /// </summary>
    public interface IDependency
    {
        /// <summary>
        /// 依赖对象是否满足了一定的条件
        /// </summary>
        /// <returns>true | false</returns>
        bool ConditionMatched();
    }
}
