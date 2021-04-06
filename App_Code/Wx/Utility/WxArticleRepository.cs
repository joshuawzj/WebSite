using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP;
using Whir.Framework;
using Whir.Repository;

/// <summary>
/// 微信图文持久化实体
/// </summary>
public class WxArticleRepository {


    /// <summary>
    /// 保存图文信息
    /// </summary>
    /// <param name="media">媒体信息</param>
    public static bool Add(WxArticle media) {
        return Whir.Repository.DbHelper.CurrentDb.Insert(media).ToInt32() > 0;
    }

    /// <summary>
    /// 更新图文信息
    /// </summary>
    /// <param name="media">媒体信息</param>
    public static bool Update(WxArticle media) {
        return Whir.Repository.DbHelper.CurrentDb.Update(media) > 0;
    }

    /// <summary>
    /// 获取图文信息
    /// </summary>
    /// <param name="mediaId">微信端媒体编号</param>
    /// <returns></returns>
    public static List<WxArticle> Find(string mediaId) {
        return Whir.Repository.DbHelper.CurrentDb.Query<WxArticle>("WHERE MediaId = @0 ORDER BY ArticleIndex ASC", mediaId).ToList();
    }

    /// <summary>
    /// 删除图文信息
    /// </summary>
    /// <param name="mediaId">微信端媒体编号</param>
    public static void Remove(string mediaId) {
        Whir.Repository.DbHelper.CurrentDb.Execute("DELETE FROM Whir_Wx_Article WHERE MediaId = @0", mediaId);
    }

    /// <summary>
    /// 获取媒体列表
    /// </summary>
    /// <param name="appid">公众号账户</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns></returns>
    public static Page<WxArticle> Page(string appid, int page, int pageSize) {
        return Whir.Repository.DbHelper.CurrentDb.Page<WxArticle>(page, pageSize, "WHERE AppId = @0 AND ArticleIndex = 0 ORDER BY UpdateDate DESC", appid);
    }

    /// <summary>
    /// 获取多页文章的子页集合
    /// </summary>
    /// <param name="mediaId">媒体编号</param>
    /// <returns></returns>
    public static List<WxArticle> ChildrenArticle(string mediaId) {
        return Whir.Repository.DbHelper.CurrentDb.Query<WxArticle>("WHERE MediaId = @0 AND ArticleIndex > 0 ORDER BY ArticleIndex ASC", mediaId).ToList();
    }

}