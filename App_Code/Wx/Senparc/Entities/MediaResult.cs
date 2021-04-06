/*----------------------------------------------------------------
    Copyright (C) 2015 Senparc
  
    文件名：WxJsonResult.cs
    文件功能描述：JSON返回结果
    
    
    创建标识：Senparc - 20150319
----------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senparc.Weixin.MP.Entities {

    /// <summary>
    /// JSON返回结果（素材）
    /// </summary>
    public class MediaResult<T> {

        /// <summary>
        /// 总记录数
        /// </summary>
        public int total_count { get; set; }

        /// <summary>
        /// 当前获取到的记录数
        /// </summary>
        public int item_count { get; set; }

        /// <summary>
        /// 素材集合
        /// </summary>
        public List<T> item { get; set; }
    }
}