using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP;
using Whir.Framework;
using Whir.Repository;

/// <summary>
/// 微信消息持久化类
/// </summary>
public class WxMessageRepository {


    /// <summary>
    /// 保存消息
    /// </summary>
    /// <param name="media">媒体信息</param>
    public static bool Add(WxMessage media) {
        return Whir.Repository.DbHelper.CurrentDb.Insert(media).ToInt32() > 0;
    }


    /// <summary>
    /// 获取消息
    /// </summary>
    /// <param name="messageId">消息编号</param>
    /// <returns></returns>
    public static WxMessage Find(string messageId) {
        return Whir.Repository.DbHelper.CurrentDb.FirstOrDefault<WxMessage>("WHERE MessageId = @0", messageId);
    }

    /// <summary>
    /// 删除消息
    /// </summary>
    /// <param name="messageId">消息编号</param>
    public static void Remove(string messageId) {
        Whir.Repository.DbHelper.CurrentDb.Execute("DELETE FROM Whir_Wx_Message WHERE MessageId = @0", messageId);
    }

    /// <summary>
    /// 获取媒体列表
    /// </summary>
    /// <param name="toUser">接收人</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns></returns>
    public static Page<WxMessage> Page(string toUser, int page, int pageSize) {
        return Whir.Repository.DbHelper.CurrentDb.Page<WxMessage>(page, pageSize, "WHERE ToUserName = @0 ORDER BY CreateDate DESC", toUser);
    }

    /// <summary>
    /// 获取媒体列表
    /// </summary>
    /// <param name="toUser">接收人</param>
    /// <param name="msgType">消息类型</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns></returns>
    public static Page<WxMessage> Page(string toUser,string msgType, int page, int pageSize) {
        return Whir.Repository.DbHelper.CurrentDb.Page<WxMessage>(page, pageSize, "WHERE ToUserName = @0 AND MessageType = @1 ORDER BY CreateDate DESC", toUser, msgType);
    }
}