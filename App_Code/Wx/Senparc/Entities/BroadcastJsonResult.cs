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
    /// JSON返回结果（群发消息）
    /// </summary>
    public class BroadcastJsonResult : WxJsonResult {

        /// <summary>
        /// 消息发送任务的ID
        /// </summary>
        public string msg_id { get; set; }

        /// <summary>
        /// 消息的数据ID，该字段只有在群发图文消息时，才会出现。可以用于在图文分析数据接口中，获取到对应的图文消息的数据，是图文分析数据接口中的msgid字段中的前半部分，详见图文分析数据接口中的msgid字段的介绍
        /// </summary>
        public string msg_data_id { get; set; }

    }
}