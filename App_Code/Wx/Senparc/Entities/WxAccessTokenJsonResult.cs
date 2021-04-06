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
    /// JSON返回结果（用于AccessToken获取）
    /// </summary>
    public class WxAccessTokenJsonResult {

        /// <summary>
        /// 访问令牌
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public int expires_in { get; set; }
    }
}