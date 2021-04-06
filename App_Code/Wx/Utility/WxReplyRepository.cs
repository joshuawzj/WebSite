using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP;
using Whir.Framework;
using Whir.Repository;

/// <summary>
/// 自动回复持久化实体
/// </summary>
public class WxReplyRepository {


    /// <summary>
    /// 保存自动回复
    /// </summary>
    /// <param name="media">媒体信息</param>
    public static bool Add(WxReply media) {
        return Whir.Repository.DbHelper.CurrentDb.Insert(media).ToInt32() > 0;
    }

    /// <summary>
    /// 更新自动回复
    /// </summary>
    /// <param name="media">媒体信息</param>
    public static bool Update(WxReply media) {
        return Whir.Repository.DbHelper.CurrentDb.Update(media) > 0;
    }

    /// <summary>
    /// 根据主键获取回复信息
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    public static WxReply Find(int id) {
        return Whir.Repository.DbHelper.CurrentDb.FirstOrDefault<WxReply>("WHERE Whir_Wx_ReplyId = @0", id);
    }

    /// <summary>
    /// 获取自动回复
    /// </summary>
    /// <param name="appId">公众号账户</param>
    /// <param name="replyType">回复类型</param>
    /// <returns></returns>
    public static List<WxReply> Find(string appId, ReplyType replyType) {
        return Whir.Repository.DbHelper.CurrentDb.Query<WxReply>("WHERE AppId = @0 AND ReplyType = @1 ORDER BY UpdateDate DESC", appId, Convert.ToInt32(replyType)).ToList();
    }

    /// <summary>
    /// 获取公众号关键词匹配信息
    /// </summary>
    /// <param name="appId">公众号账户</param>
    /// <param name="replyType">回复类型</param>
    /// <param name="keywrod">关键词</param>
    /// <returns></returns>
    public static List<WxReply> Find(string appId, ReplyType replyType, string keywrod) {
        return Whir.Repository.DbHelper.CurrentDb.Query<WxReply>("WHERE AppId = @0 AND ReplyType = @1 AND Keywords LIKE '%' + @2 + '%' ORDER BY UpdateDate DESC", appId, Convert.ToInt32(replyType), keywrod).ToList();
    }

    /// <summary>
    /// 设置菜单文本回复信息
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="button"></param>
    public static void SetMenuEventReply(string appId, Senparc.Weixin.MP.Entities.MenuJsonResult.Menu.Button button) {
        WxReply repley = WxReplyRepository.Find(appId, ReplyType.Keyword).Where(m => m.RoleName == button.name).OrderByDescending(o => o.UpdateDate).FirstOrDefault();
        if (repley == null) {
            repley = new WxReply() {
                AppId = appId,
                CreateDate = DateTime.Now,

                ReplyDataType = ReplyDataType.text,
                ReplyType = ReplyType.Keyword,
                UpdateDate = DateTime.Now
            };
        }
        repley.Keywords = button.key;
        repley.RoleName = button.name;
        repley.MessageDetail = button.data.ToStr();
        repley.UpdateDate = DateTime.Now;
        if (repley.Whir_Wx_ReplyId > 0) {
            Update(repley);
        }
        else {
            Add(repley);
        }
    }

    /// <summary>
    /// 删除自动回复
    /// </summary>
    /// <param name="id">主键编号</param>
    public static void Remove(int id) {
        Whir.Repository.DbHelper.CurrentDb.Execute("DELETE FROM Whir_Wx_Reply WHERE Whir_Wx_ReplyId = @0", id);
    }

    /// <summary>
    /// 获取媒体列表
    /// </summary>
    /// <param name="appid">公众号账户</param>
    /// <param name="replyType">回复类型</param>
    /// <param name="page">查询页码</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns></returns>
    public static Page<WxReply> Page(string appid, ReplyType replyType, int page, int pageSize) {
        return Whir.Repository.DbHelper.CurrentDb.Page<WxReply>(page, pageSize, "WHERE AppId = @0  AND ReplyType = @1 ORDER BY UpdateDate DESC", appid, replyType);
    }

    /// <summary>
    /// 获取修正后的数据
    /// </summary>
    /// <param name="reply">回复信息</param>
    /// <returns></returns>
    public static object RepairData(WxReply reply) {
        if (reply != null) {
            object data = null;
            if (reply.ReplyDataType == ReplyDataType.article) {
                List<WxArticle> articles = WxArticleRepository.Find(reply.MessageDetail);
                if (articles != null) {
                    List<object> rows = new List<object>();
                    articles.ForEach(w => {
                        rows.Add(new {
                            title = w.Title,
                            image = w.ThumbMediaURL
                        });
                    });
                    data = rows;
                }
            }
            else if (reply.ReplyDataType == ReplyDataType.text) {
                data = reply.MessageDetail;
            }
            else {
                WxSampleMedia sample = WxSampleMediaRepository.Find(reply.MessageDetail);
                if (sample != null) {
                    switch (reply.ReplyDataType) {
                        case ReplyDataType.image:
                            data = sample.LocalURL.IsEmpty() ? sample.WebURL : sample.LocalURL;
                            break;
                        case ReplyDataType.video:
                            data = sample.Title;
                            break;
                        case ReplyDataType.voice:
                            data = sample.Title;
                            break;
                    }
                }
            }

            return new {
                reply.AppId,
                reply.CreateDate,
                reply.RoleName,
                reply.Keywords,
                reply.MessageDetail,
                ReplyDataType = reply.ReplyDataType.ToStr(),
                ReplyType = reply.ReplyType.ToStr(),
                reply.UpdateDate,
                reply.Whir_Wx_ReplyId,
                Data = data
            };
        }
        return string.Empty;
    }
}