
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP;
using Whir.Repository;


/// <summary>
/// 微信消息
/// </summary>
[TableName("Whir_Wx_Message")]
[PrimaryKey("Whir_Wx_MessageId", sequenceName = "seq_ezEIP")]
public class WxMessage {

    /// <summary>
    /// 主键
    /// </summary>
    public int Whir_Wx_MessageId { get; set; }

    /// <summary>
    /// 消息编号
    /// </summary>
    public string MessageId { get; set; }

    /// <summary>
    /// 发送人
    /// </summary>
    public string FromUserName { get; set; }

    /// <summary>
    /// 接收人
    /// </summary>
    public string ToUserName { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public string MessageType { get; set; }

    /// <summary>
    /// 原始信息
    /// </summary>
    public string FormData { get; set; }

    /// <summary>
    /// 回复内容
    /// </summary>
    public string ReplyData { get; set; }

    /// <summary>
    /// 创建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

}