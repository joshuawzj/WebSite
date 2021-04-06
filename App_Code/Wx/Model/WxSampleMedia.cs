
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP;
using Whir.Repository;


/// <summary>
/// 简单媒体信息（图片、视频、语音、缩略图等）
/// </summary>
[TableName("Whir_Wx_SampleMedia")]
[PrimaryKey("Whir_Wx_SampleMediaId", sequenceName = "seq_ezEIP")]
public class WxSampleMedia {

    /// <summary>
    /// 主键
    /// </summary>
    public int Whir_Wx_SampleMediaId { get; set; }

    /// <summary>
    /// 公众号账号
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 本地地址
    /// </summary>
    public string LocalURL { get; set; }

    /// <summary>
    /// 素材地址
    /// </summary>
    public string WebURL { get; set; }

    /// <summary>
    /// 微信端的媒体编号
    /// </summary>
    public string MediaId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string MediaName { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 介绍
    /// </summary>
    public string Introduction { get; set; }

    /// <summary>
    /// 媒体类型
    /// </summary>
    public UploadMediaFileType MediaType { get; set; }

    /// <summary>
    /// 分组边阿訇
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// 文件大小
    /// </summary>
    public int MediaSize { get; set; }

    /// <summary>
    /// 创建日期
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 更新日期
    /// </summary>
    public DateTime UpdateDate { get; set; }

}