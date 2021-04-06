using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP;
using Whir.Framework;
using Whir.Repository;

/// <summary>
/// 二维码持久化实体
/// </summary>
public class WxQrcodeRepository {


    /// <summary>
    /// 保存二维码
    /// </summary>
    /// <param name="media">媒体信息</param>
    public static bool Add(WxQrcode media) {
        return Whir.Repository.DbHelper.CurrentDb.Insert(media).ToInt32() > 0;
    }

    /// <summary>
    /// 更新二维码
    /// </summary>
    /// <param name="media">媒体信息</param>
    public static bool Update(WxQrcode media) {
        return Whir.Repository.DbHelper.CurrentDb.Update(media) > 0;
    }

    /// <summary>
    /// 获取二维码
    /// </summary>
    /// <param name="ticket">微信端编号</param>
    /// <returns></returns>
    public static WxQrcode Find(string ticket) {
        return Whir.Repository.DbHelper.CurrentDb.FirstOrDefault<WxQrcode>("WHERE Ticket = @0", ticket);
    }

    /// <summary>
    /// 删除二维码
    /// </summary>
    /// <param name="ticket">微信端编号</param>
    public static void Remove(string ticket) {
        Whir.Repository.DbHelper.CurrentDb.Execute("DELETE FROM Whir_Wx_Qrcode WHERE Ticket = @0", ticket);
    }

    /// <summary>
    /// 删除二维码
    /// </summary>
    /// <param name="ticket">微信端编号</param>
    public static void Remove(int id) {
        Whir.Repository.DbHelper.CurrentDb.Execute("DELETE FROM Whir_Wx_Qrcode WHERE Whir_Wx_QrcodeId = @0", id);
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="appid">公众号账户</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns></returns>
    public static Page<WxQrcode> Page(string appid, int page, int pageSize) {
        return Whir.Repository.DbHelper.CurrentDb.Page<WxQrcode>(page, pageSize, "WHERE AppId = @0  ORDER BY CreateDate DESC", appid);
    }

    
}