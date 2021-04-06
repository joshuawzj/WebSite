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
public class WxBroadcastRepository {


    /// <summary>
    /// 保存群发信息
    /// </summary>
    /// <param name="media">媒体信息</param>
    public static bool Add(WxBroadcast media) {
        return Whir.Repository.DbHelper.CurrentDb.Insert(media).ToInt32() > 0;
    }

    /// <summary>
    /// 获取群发信息
    /// </summary>
    /// <param name="messageId">微信端消息编号</param>
    /// <returns></returns>
    public static WxBroadcast Find(string messageId) {
        return Whir.Repository.DbHelper.CurrentDb.FirstOrDefault<WxBroadcast>("WHERE MessageId = @0 ORDER BY CreateDate DESC", messageId);
    }

    /// <summary>
    /// 删除群发信息
    /// </summary>
    /// <param name="messageId">微信端消息编号</param>
    public static void Remove(string messageId) {
        Whir.Repository.DbHelper.CurrentDb.Execute("DELETE FROM Whir_Wx_Broadcast WHERE MessageId = @0", messageId);
    }

    /// <summary>
    /// 获取媒体列表
    /// </summary>
    /// <param name="appid">公众号账户</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns></returns>
    public static Page<WxBroadcast> Page(string appid, int page, int pageSize) {
        return Whir.Repository.DbHelper.CurrentDb.Page<WxBroadcast>(page, pageSize, "WHERE AppId = @0 AND ArticleIndex = 0 ORDER BY UpdateDate DESC", appid);
    }

}