/*----------------------------------------------------------------
    Copyright (C) 2015 Senparc
  
    文件名：WxJsonResult.cs
    文件功能描述：JSON返回结果
    
    
    创建标识：Senparc - 20150319
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senparc.Weixin.MP.Entities {

    /// <summary>
    /// JSON返回结果（用于用户分组）
    /// </summary>
    public class WxUserGroupJsonResult {

        /// <summary>
        /// 错误码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }
}