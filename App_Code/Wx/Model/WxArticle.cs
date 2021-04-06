
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP;
using Whir.Repository;


/// <summary>
/// 微信用户信息
/// </summary>
[TableName("Whir_Wx_Article")]
[PrimaryKey("Whir_Wx_ArticleId", sequenceName = "seq_ezEIP")]
public class WxArticle {

    /// <summary>
    /// 主键
    /// </summary>
    public int Whir_Wx_ArticleId { get; set; }

    /// <summary>
    /// 公众号账号
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 微信端媒体编号
    /// </summary>
    public string MediaId { get; set; }

    /// <summary>
    /// 图文标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 缩略图图片编号
    /// </summary>
    public string ThumbMediaId { get; set; }

    /// <summary>
    /// 缩略图本地地址
    /// </summary>
    public string ThumbMediaURL { get; set; }

    /// <summary>
    /// 缩略图WEB地址
    /// </summary>
    public string ThumbMediaWebURL { get; set; }

    /// <summary>
    /// 作者
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// 文章摘要
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    /// 是否显示封面
    /// </summary>
    public bool IsShowCover { get; set; }

    /// <summary>
    /// 原始信息
    /// </summary>
    public string OriginalDetail { get; set; }

    /// <summary>
    /// 文章详情
    /// </summary>
    public string Detail { get; set; }

    /// <summary>
    /// 原文章地址
    /// </summary>
    public string SourceURL { get; set; }

    /// <summary>
    /// 文章类型，1：单页，2：多页
    /// </summary>
    public ArticleType ArticleType { get; set; }

    /// <summary>
    /// 多页图文索引
    /// </summary>
    public int ArticleIndex { get; set; }

    /// <summary>
    /// 预览地址
    /// </summary>
    public string ViewURL { get; set; }

    /// <summary>
    /// 更新日期
    /// </summary>
    public DateTime UpdateDate { get; set; }

}