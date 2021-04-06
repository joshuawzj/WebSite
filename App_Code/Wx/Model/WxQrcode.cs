
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP;
using Whir.Repository;


/// <summary>
/// 二维码
/// </summary>
[TableName("Whir_Wx_Qrcode")]
[PrimaryKey("Whir_Wx_QrcodeId", sequenceName = "seq_ezEIP")]
public class WxQrcode {

    /// <summary>
    /// 主键
    /// </summary>
    public int Whir_Wx_QrcodeId { get; set; }

    /// <summary>
    /// 公众号编号
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 二维码描述
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    /// 微信端编号
    /// </summary>
    public string Ticket { get; set; }

    /// <summary>
    /// 二维码类型
    /// </summary>
    public QrocodeType QrcodeType { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime? ExpiredDate { get; set; }

    /// <summary>
    /// 附加信息
    /// </summary>
    public string ExtraData { get; set; }

    /// <summary>
    /// 创建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

}