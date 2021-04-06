
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP;
using Whir.Repository;


/// <summary>
/// 自动回复
/// </summary>
[TableName("Whir_Wx_Reply")]
[PrimaryKey("Whir_Wx_ReplyId", sequenceName = "seq_ezEIP")]
public class WxReply {

    /// <summary>
    /// 主键
    /// </summary>
    public int Whir_Wx_ReplyId { get; set; }

    /// <summary>
    /// 公众号账户
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 回复类型
    /// </summary>
    public ReplyType ReplyType { get; set; }

    /// <summary>
    /// 回复数据类型
    /// </summary>
    public ReplyDataType ReplyDataType { get; set; }

    /// <summary>
    /// 规则名称
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// 关键词
    /// </summary>
    public string Keywords { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string MessageDetail { get; set; }

    /// <summary>
    /// 创建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 更新日期
    /// </summary>
    public DateTime UpdateDate { get; set; }

}