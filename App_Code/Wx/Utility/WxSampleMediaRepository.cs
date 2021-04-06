using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP;
using Whir.Framework;
using Whir.Repository;

/// <summary>
/// 简单媒体持久化类
/// </summary>
public class WxSampleMediaRepository {


    /// <summary>
    /// 保存简单媒体
    /// </summary>
    /// <param name="media">媒体信息</param>
    public static bool Add(WxSampleMedia media) {
        return Whir.Repository.DbHelper.CurrentDb.Insert(media).ToInt32() > 0;
    }

    /// <summary>
    /// 更新简单媒体
    /// </summary>
    /// <param name="media">媒体信息</param>
    public static bool Update(WxSampleMedia media) {
        return Whir.Repository.DbHelper.CurrentDb.Update(media) > 0;
    }

    /// <summary>
    /// 获取简单媒体
    /// </summary>
    /// <param name="mediaId">微信端媒体编号</param>
    /// <returns></returns>
    public static WxSampleMedia Find(string mediaId) {
        return Whir.Repository.DbHelper.CurrentDb.FirstOrDefault<WxSampleMedia>("WHERE MediaId = @0", mediaId);
    }

    /// <summary>
    /// 获取素材的主键
    /// </summary>
    /// <param name="mediaId">微信端媒体编号</param>
    /// <returns></returns>
    public static int FindPrimaryKey(string mediaId) {
        object r = Whir.Repository.DbHelper.CurrentDb.ExecuteScalar<object>("SELECT TOP 1 Whir_Wx_SampleMediaId FROM Whir_Wx_SampleMedia WHERE MediaId = @0", mediaId);
        return r.ToInt32(0);
    }

    /// <summary>
    /// 删除简单媒体
    /// </summary>
    /// <param name="mediaId">微信端媒体编号</param>
    public static void Remove(string mediaId) {
        Whir.Repository.DbHelper.CurrentDb.Execute("DELETE FROM Whir_Wx_SampleMedia WHERE MediaId = @0", mediaId);
    }

    /// <summary>
    /// 获取媒体列表
    /// </summary>
    /// <param name="appid">公众号账户</param>
    /// <param name="mediaType">多媒体类型</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns></returns>
    public static Page<WxSampleMedia> Page(string appid, UploadMediaFileType mediaType, int page, int pageSize) {
        return Whir.Repository.DbHelper.CurrentDb.Page<WxSampleMedia>(page, pageSize, "WHERE AppId = @0 AND MediaType = @1 ORDER BY UpdateDate DESC", appid, Convert.ToInt32(mediaType));
    }
}