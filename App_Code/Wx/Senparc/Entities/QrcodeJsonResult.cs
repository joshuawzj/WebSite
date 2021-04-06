/*----------------------------------------------------------------
    Copyright (C) 2015 Senparc
    
    文件名：NewsModel.cs
    文件功能描述：群发图文消息模型
    
    
    创建标识：Senparc - 20150211
    
    修改标识：Senparc - 20150303
    修改描述：整理接口
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senparc.Weixin.MP.Entities
{
    /// <summary>
    /// 二维码响应
    /// </summary>
    public class QrcodeJsonResult:WxJsonResult
    {

        /// <summary>
        /// 获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 该二维码有效时间，以秒为单位。 最大不超过2592000（即30天）
        /// </summary>
        public long expire_seconds { get; set; }

        /// <summary>
        /// 二维码图片解析后的地址，开发者可根据该地址自行生成需要的二维码图片
        /// </summary>
        public string url { get; set; }

    }
}