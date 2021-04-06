using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 媒体信息（图文）
/// </summary>
public class ArticleMediaJsonResult {

    /// <summary>
    /// 媒体编号
    /// </summary>
    public string media_id { get; set; }

    /// <summary>
    /// 媒体信息
    /// </summary>
    public MediaData content { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public long update_time { get; set; }

    /// <summary>
    /// 媒体信息
    /// </summary>
    public class MediaData {

        /// <summary>
        /// 媒体集合
        /// </summary>
        public List<MediaDataEntity> news_item { get; set; }

        /// <summary>
        /// 媒体详情
        /// </summary>
        public class MediaDataEntity {

            /// <summary>
            /// 标题
            /// </summary>
            public string title { get; set; }

            /// <summary>
            /// 封面媒体编号
            /// </summary>
            public string thumb_media_id { get; set; }

            /// <summary>
            /// 是否显示封面（0：不显示，1：显示）
            /// </summary>
            public int show_cover_pic { get; set; }

            /// <summary>
            /// 作者
            /// </summary>
            public string author { get; set; }

            /// <summary>
            /// 摘要信息
            /// </summary>
            public string digest { get; set; }

            /// <summary>
            /// 内容
            /// </summary>
            public string content { get; set; }

            /// <summary>
            /// 图文地址
            /// </summary>
            public string url { get; set; }

            /// <summary>
            /// 原文地址
            /// </summary>
            public string content_source_url { get; set; }

        }
    }

}