
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP;
using Whir.Repository;


/// <summary>
/// 群发信息
/// </summary>
[TableName("Whir_Wx_Broadcast")]
[PrimaryKey("Whir_Wx_BroadcastId", sequenceName = "seq_ezEIP")]
public class WxBroadcast {

    /// <summary>
    /// 主键
    /// </summary>
    public int Whir_Wx_BroadcastId { get; set; }

    /// <summary>
    /// 公众号账户
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 消息编号
    /// </summary>
    public string MessageId { get; set; }

    /// <summary>
    /// 消息数据编号
    /// </summary>
    public string MessageDataId { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public BroadcastType MessageType { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string MessageDetail { get; set; }

    /// <summary>
    /// 消息状态
    /// </summary>
    public BroadcastStatus Status { get; set; }

    /// <summary>
    /// 创建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

}