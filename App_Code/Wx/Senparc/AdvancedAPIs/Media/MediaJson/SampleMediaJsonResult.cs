using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 简单媒体信息（图片、视频、语音、缩略图等）
/// </summary>
public class SampleMediaJsonResult {

    /// <summary>
    /// 素材编号
    /// </summary>
    public string media_id { get; set; }

    /// <summary>
    /// 素材名
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public long update_time { get; set; }

    /// <summary>
    /// 素材地址
    /// </summary>
    public string url { get; set; }
}