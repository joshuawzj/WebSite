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

namespace Senparc.Weixin.MP.Entities
{
    /// <summary>
    /// JSON返回结果（用于获取用户列表信息）
    /// </summary>
    public class WxUsersJsonResult
    {

        /// <summary>
        /// 总用户数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 当前拉取用户数
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 用户openid集合
        /// </summary>
        public Dictionary<string, string[]> data { get; set; }

        /// <summary>
        /// 下次拉取开始openid
        /// </summary>
        public string next_openid { get; set; }
    }
}