using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.AdvancedAPIs.Media.MediaJson;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Utilities.HttpUtility;
using Whir.Framework;

/// <summary>
/// 微信辅助类
/// </summary>
public class WxUtility {

    static string COOKIE_CURRENT = "__wx__";
    static string COOKIE_CURRENT_SECRET_KEY = "__quanx__wax__";

    static object GET_ACCESS_TOKEN_LOCK = new object();

    /// <summary>
    /// 获取当前管理的公众号
    /// </summary>
    /// <returns></returns>
    public static WxCredence GetCurrentCredence() {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIE_CURRENT];
        if (cookie != null) {
            try {
                string appId = new TripleDESService(COOKIE_CURRENT_SECRET_KEY).Decrypt(cookie.Value);
                return GetCredence(appId);
            }
            catch (Exception ex) {

            }
        }
        return null;
    }

    /// <summary>
    /// 设置当前公众号
    /// </summary>
    /// <param name="appId">公账号账号</param>
    public static void SetCurrentCredence(string appId) {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIE_CURRENT];
        string cookieValue = string.Empty;
        try {
            GetCredence(appId);
            cookieValue = new TripleDESService(COOKIE_CURRENT_SECRET_KEY).Encrypt(appId);
        }
        catch (Exception ex) {

        }
        if (cookie != null) {
            cookie.Value = cookieValue;
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
        else {
            cookie = new HttpCookie(COOKIE_CURRENT);
            cookie.Value = cookieValue;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }

    /// <summary>
    /// 获取公众号信息
    /// </summary>
    /// <param name="appId">公众号账号</param>
    /// <returns></returns>
    public static WxCredence GetCredence(string appId)
    {
        WxCredence credence = WxConfigRepository.GetCredence(appId);
        if (credence != null)
        {
            if (credence.TokenExpired == null || credence.TokenExpired.ToStr().IsEmpty() || credence.TokenExpired <= DateTime.Now)
            {
                // 已过期重新获取
                // 从公众号平台获取ACCESSTOKEN时锁定获取操作
                lock (GET_ACCESS_TOKEN_LOCK)
                {
                    string accessToken = GetAccessToken(credence.AppId, credence.AppSecret);
                    if (!accessToken.IsEmpty())
                    {
                        credence.AccessToken = accessToken;
                        credence.TokenExpired = DateTime.Now.AddSeconds(7200);
                        WxConfigRepository.UpdateCredence(credence);
                    }
                }
            }
            else {
                string accessToken = GetAccessToken(credence.AppId, credence.AppSecret);
                if (!accessToken.IsEmpty())
                {
                    credence.AccessToken = accessToken;
                    credence.TokenExpired = DateTime.Now.AddSeconds(7200);
                    WxConfigRepository.UpdateCredence(credence);
                }
            }
        }
        return credence;
    }

    /// <summary>
    /// 获取访问令牌
    /// </summary>
    /// <param name="appId">公众号账户</param>
    /// <param name="appSecret">密钥</param>
    /// <returns></returns>
    public static string GetAccessToken(string appId, string appSecret) {
        try {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret);
            WxAccessTokenJsonResult result = Senparc.Weixin.HttpUtility.Get.GetJson<WxAccessTokenJsonResult>(url);
            return result.access_token;
        }
        catch (Exception ex) {
            // TODO
        }
        return string.Empty;
    }

    /// <summary>
    /// 获取对象指定属性值
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="o">要提取的对象</param>
    /// <param name="name">属性名称</param>
    /// <returns></returns>
    static T GetValue<T>(object o, string name) {
        try {
            var value = o.GetType().GetProperty(name).GetValue(o, null);
            return (T)value;
        }
        catch (Exception ex) {

        }
        return default(T);
    }

    #region 用户相关

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="accesToken">访问令牌</param>
    /// <param name="nextId">下一openid</param>
    /// <param name="page">当前页</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns></returns>
    public static WxUsersJsonResult GetUsers(string accesToken, string nextId, int page, int pageSize) {
        try {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}", accesToken, nextId);
            WxUsersJsonResult result = Senparc.Weixin.HttpUtility.Get.GetJson<WxUsersJsonResult>(url);
            string[] openids = result.data["openid"];
            if (openids.Length > pageSize) {
                result.data["openid"] = openids.Take(pageSize).ToArray();
                result.next_openid = openids[pageSize - 1];
            }
            return result;
        }
        catch (Exception ex) {
            // TODO
        }
        return null;
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="accesToken">访问令牌</param>
    /// <param name="tagid">标签编号</param>
    /// <param name="nextId">下一openid</param>
    /// <param name="page">当前页</param>
    /// <param name="pageSize">分页大小</param>
    /// <returns></returns>
    public static WxUsersJsonResult GetUsers(string accesToken, int tagid, string nextId, int page, int pageSize) {
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/user/tag/get?access_token={0}";
            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"tagid",tagid},
                {"next_openid",nextId}
            };
            WxUsersJsonResult result = CommonJsonSend.Send<WxUsersJsonResult>(accesToken, url, data);
            string[] openids = result.data["openid"];
            if (openids.Length > pageSize) {
                result.data["openid"] = openids.Take(pageSize).ToArray();
                result.next_openid = openids[pageSize - 1];
            }
            return result;
        }
        catch (Exception ex) {
            // TODO
        }
        return null;
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="openId">用户openid</param>
    /// <returns></returns>
    public static WxUserJsonResult GetUser(string accessToken, string openId) {
        try {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", accessToken, openId);
            return Senparc.Weixin.HttpUtility.Get.GetJson<WxUserJsonResult>(url);
        }
        catch (Exception ex) {
            // TODO
        }
        return null;
    }

    /// <summary>
    /// 设置用户备注
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="openid">用户openid</param>
    /// <param name="remark">备注信息</param>
    public static bool SetUserRemark(string accessToken, string openid, string remark) {
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token={0}";
            Dictionary<string, string> data = new Dictionary<string, string>() {
                {"openid",openid},
                {"remark",remark}
            };
            WxJsonResult result = CommonJsonSend.Send<WxJsonResult>(accessToken, url, data, CommonJsonSendType.POST);
            return result.errcode == 0;
        }
        catch (Exception ex) {
            // TODO
        }
        return false;
    }

    #endregion

    #region 分组相关

    /// <summary>
    /// 获取用户分组
    /// </summary>
    /// <param name="accessToken">访问凭证</param>
    /// <returns></returns>
    public static object[] UserGroups(string accessToken) {
        try {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}", accessToken);
            Dictionary<string, object> result = Senparc.Weixin.HttpUtility.Get.GetJson<Dictionary<string, object>>(url);
            if (!result.ContainsKey("errcode")) {
                return (object[])result["groups"];
            }
        }
        catch (Exception ex) {
            // TODO
        }
        return null;
    }

    /// <summary>
    /// 创建分组
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="name">分组名称</param>
    /// <returns></returns>
    public static int AddGroup(string accessToken, string name) {
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token={0}";
            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"group",new {
                    name=name
                }},
            };
            Dictionary<string, object> result = CommonJsonSend.Send<Dictionary<string, object>>(accessToken, url, data, CommonJsonSendType.POST);
            if (!result.ContainsKey("errcode")) {
                return GetValue<int>(result["group"], "id");
            }
            else {
                return 0;
            }
        }
        catch (Exception ex) {
            // TODO
        }
        return 0;
    }

    #endregion

    #region 标签相关

    /// <summary>
    /// 获取标签
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <returns></returns>
    public static ArrayList GetTags(string accessToken) {
        try {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/get?access_token={0}", accessToken);
            Dictionary<string, object> result = Senparc.Weixin.HttpUtility.Get.GetJson<Dictionary<string, object>>(url);
            if (!result.ContainsKey("errcode")) {
                return (ArrayList)result["tags"];
            }
        }
        catch (Exception ex) {
            // TODO
        }
        return null;
    }

    /// <summary>
    /// 创建标签
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="name">标签名称</param>
    /// <returns></returns>
    public static WxJsonResult AddTag(string accessToken, string name) {
        WxJsonResult result = new WxJsonResult();
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/tags/create?access_token={0}";
            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"tag",new {
                    name=name
                }},
            };
            Dictionary<string, object> response = CommonJsonSend.Send<Dictionary<string, object>>(accessToken, url, data, CommonJsonSendType.POST);
            if (!response.ContainsKey("errcode")) {
                result.errcode = ((Dictionary<string, object>)response["tag"])["id"].ToInt32();
                result.errmsg = "ok";
            }
            else {
                int errCode = response["errcode"].ToInt32();
                switch (errCode) {
                    case -1:
                        result.errmsg = "系统繁忙";
                        break;
                    case 45157:
                        result.errmsg = "标签名重复";
                        break;
                    case 45158:
                        result.errmsg = "标签名最多30个字符";
                        break;
                    case 45056:
                        result.errmsg = "创建的标签数不可超过100个";
                        break;
                    default:
                        result.errmsg = "未知错误";
                        break;
                }
                LogHelper.Log("添加Tag\r\n错误代码：" + errCode + " 错误信息：" + response["errmsg"]);
            }
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    /// <summary>
    /// 设置标签
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="tagid">标签编号</param>
    /// <param name="name">标签名称</param>
    /// <returns></returns>
    public static WxJsonResult UpdateTag(string accessToken, int tagid, string name) {
        WxJsonResult result = new WxJsonResult();
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/tags/update?access_token={0}";
            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"tag",new {
                    id=tagid,
                    name=name
                }},
            };
            result = CommonJsonSend.Send<WxJsonResult>(accessToken, url, data, CommonJsonSendType.POST);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    /// <summary>
    /// 删除标签
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="tagid">标签编号</param>
    /// <returns></returns>
    public static WxJsonResult RemoveTag(string accessToken, int tagid) {
        WxJsonResult result = new WxJsonResult();
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/tags/delete?access_token={0}";
            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"tag",new {
                    id=tagid
                }},
            };
            result = CommonJsonSend.Send<WxJsonResult>(accessToken, url, data, CommonJsonSendType.POST);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    /// <summary>
    /// 批量设置标签
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="tagid">标签标号</param>
    /// <param name="openids">用户openid集合</param>
    /// <returns></returns>
    public static WxJsonResult SetTag(string accessToken, int tagid, string[] openids) {
        WxJsonResult result = new WxJsonResult();
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/tags/members/batchtagging?access_token={0}";
            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"tagid",tagid},
                {"openid_list",openids},
            };
            result = CommonJsonSend.Send<WxJsonResult>(accessToken, url, data, CommonJsonSendType.POST);

        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    /// <summary>
    /// 批量取消标签
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="tagid">标签标号</param>
    /// <param name="openids">用户openid集合</param>
    /// <returns></returns>
    public static WxJsonResult CancelTag(string accessToken, int tagid, string[] openids) {
        WxJsonResult result = new WxJsonResult();
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/tags/members/batchuntagging?access_token={0}";
            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"tagid",tagid},
                {"openid_list",openids},
            };
            result = CommonJsonSend.Send<WxJsonResult>(accessToken, url, data, CommonJsonSendType.POST);

        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    #endregion

    #region 素材相关

    /// <summary>
    /// 添加图文媒体信息
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="articles">文章详情</param>
    /// <returns></returns>
    public static UploadForeverMediaResult AddArticleMedia(string accessToken, List<WxArticle> articles) {
        UploadForeverMediaResult result = new UploadForeverMediaResult();
        try {
            //string url = "https://api.weixin.qq.com/cgi-bin/material/add_news?access_token={0}";
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/material/add_news?access_token={0}", accessToken);
            List<object> artData = new List<object>();
            articles.ForEach(m => {
                artData.Add(new {
                    title = m.Title,
                    thumb_media_id = m.ThumbMediaId,
                    author = m.Author,
                    digest = m.Summary,
                    show_cover_pic = m.IsShowCover ? 1 : 0,
                    content = m.Detail,
                    content_source_url = m.SourceURL
                });
            });
            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"articles",artData},
            };
            result = CommonJsonSend.Send<UploadForeverMediaResult>(accessToken, url, data, CommonJsonSendType.POST);
            //result = Post.PostGetJson<UploadForeverMediaResult>(url, null, data);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    /// <summary>
    /// 更新图文媒体信息
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="articles">文章详情</param>
    /// <returns></returns>
    public static UploadForeverMediaResult UpateArticleMedia(string accessToken, WxArticle article) {
        UploadForeverMediaResult result = new UploadForeverMediaResult();
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/material/update_news?access_token={0}";
            var artData = new {
                title = article.Title,
                thumb_media_id = article.ThumbMediaId,
                author = article.Author,
                digest = article.Summary,
                show_cover_pic = article.IsShowCover ? 1 : 0,
                content = article.Detail,
                content_source_url = article.SourceURL
            };
            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"media_id",article.MediaId},
                {"index",article.ArticleIndex},
                {"articles",artData},
            };
            result = CommonJsonSend.Send<UploadForeverMediaResult>(accessToken, url, data, CommonJsonSendType.POST);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    /// <summary>
    /// 添加永久素材（图片、语音、视频等）
    /// </summary>
    /// <param name="fileStream">素材文件流</param>
    /// <param name="title">视频素材标题</param>
    /// <param name="description">视频素材描述</param>
    /// <returns></returns>
    public static UploadForeverMediaResult AddSampleMedia(string accessToken, string filePath, UploadMediaFileType mediaType, string title = "", string introduction = "") {
        UploadForeverMediaResult result = new UploadForeverMediaResult();
        try {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={0}&type={1}", accessToken, mediaType.ToString());
            if (mediaType == UploadMediaFileType.thumb) {
                url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", accessToken, mediaType.ToString());
            }
            
            if (mediaType == UploadMediaFileType.news) {
                url = string.Format("https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token={0}", accessToken);
            }
            Dictionary<string, string> files = new Dictionary<string, string>(){
                {"media",filePath}
            };
            if (!title.IsEmpty() || !introduction.IsEmpty()) {
                files.Add("description", string.Format("{{\"title\":\"{0}\", \"introduction\":\"{1}\"}}", title, introduction));
            }
            result = Post.PostFileGetJson<UploadForeverMediaResult>(url, null, files);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    /// <summary>
    /// 获取素材
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="mediaType"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static MediaResult<T> GetMedias<T>(string accessToken, UploadMediaFileType mediaType, int page, int pageSize) {
        MediaResult<T> result = new MediaResult<T>();
        try {
            if (pageSize > 19) {
                pageSize = 19;
            }
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token={0}", accessToken);

            Dictionary<string, string> data = new Dictionary<string, string>(){
                {"type",mediaType.ToString()},
                {"offset",((page-1)*pageSize).ToStr()},
                {"count",pageSize.ToStr()}
            };
            return Post.PostGetJson<MediaResult<T>>(url, null, data);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    /// <summary>
    /// 删除永久素材
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="mediaId">素材编号</param>
    public static WxJsonResult RemoveMedia(string accessToken, string mediaId) {
        WxJsonResult result = new WxJsonResult();
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/material/del_material?access_token={0}";
            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"media_id",mediaId},
            };
            result = CommonJsonSend.Send<WxJsonResult>(accessToken, url, data, CommonJsonSendType.POST);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    /// <summary>
    /// 获取图文信息
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="mediaId">图文编号</param>
    /// <returns></returns>
    public static ArticleMediaJsonResult.MediaData GetArticleMedia(string accessToken, string mediaId) {
        ArticleMediaJsonResult.MediaData result = new ArticleMediaJsonResult.MediaData();
        try {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/material/get_material?access_token={0}", accessToken);

            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"media_id",mediaId},
            };
            result = CommonJsonSend.Send<ArticleMediaJsonResult.MediaData>(accessToken, url, data, CommonJsonSendType.POST);
            //result = Post.PostGetJson<UploadForeverMediaResult>(url, null, data);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    #endregion

    #region 菜单相关

    /// <summary>
    /// 获取自定义菜单
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <returns></returns>
    public static MenuJsonResult GetMenus(string accessToken) {
        MenuJsonResult result = new MenuJsonResult();
        try {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", accessToken);
            result = Senparc.Weixin.HttpUtility.Get.GetJson<MenuJsonResult>(url);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    /// <summary>
    /// 添加菜单
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="buttons">按钮集合</param>
    /// <returns></returns>
    public static WxJsonResult SetMenus(string accessToken, List<Senparc.Weixin.MP.Entities.MenuJsonResult.Menu.Button> buttons) {
        WxJsonResult result = new WxJsonResult();
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
            Dictionary<string, object> data = new Dictionary<string, object>(){
                {"button",buttons}
            };
            result = CommonJsonSend.Send<WxJsonResult>(accessToken, url, data);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    #endregion

    #region 群发相关

    /// <summary>
    /// 向所有用户发用信息
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="castType">群发类型</param>
    /// <param name="data">发送的信息</param>
    /// <returns></returns>
    public static BroadcastJsonResult Broadcast(string accessToken, BroadcastType castType, string message) {
        return InternalBroadcast(accessToken, castType, message, 0);
    }

    /// <summary>
    /// 向指定标签用户群发信息
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="castType">群发类型</param>
    /// <param name="message">信息</param>
    /// <param name="tagid">用户标签</param>
    /// <returns></returns>
    public static BroadcastJsonResult Broadcast(string accessToken, BroadcastType castType, string message, int tagid) {
        return InternalBroadcast(accessToken, castType, message, tagid);
    }

    /// <summary>
    /// 根据用户openid群发信息
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="castType"></param>
    /// <param name="openids"></param>
    /// <returns></returns>
    public static BroadcastJsonResult Broadcast(string accessToken, BroadcastType castType, string message, string[] openids) {
        return InternalBroadcast(accessToken, castType, message, 0, openids);
    }

    /// <summary>
    /// 群发信息
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="castType">群发类型</param>
    /// <param name="message">信息</param>
    /// <param name="tagid">用户标签</param>
    /// <returns></returns>
    static BroadcastJsonResult InternalBroadcast(string accessToken, BroadcastType castType, string message, int tagid = 0, string[] openids = null) {
        BroadcastJsonResult result = new BroadcastJsonResult();
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}";
            if (openids != null && openids.Length > 0) {
                url = "https://api.weixin.qq.com/cgi-bin/message/mass/send";
            }
            Dictionary<string, object> data = new Dictionary<string, object>(){
                {"send_ignore_reprint",0}
            };
            if (tagid > 0) {
                data.Add("filter", new {
                    is_to_all = tagid == 0,
                    tag_id = tagid
                });
            }
            else {
                if (openids != null && openids.Length > 0) {
                    data.Add("touser", openids);
                }
                else {
                    data.Add("filter", new {
                        is_to_all = true
                    });
                }
            }
            switch (castType) {
                case BroadcastType.image:
                    data.Add("msgtype", "image");
                    data.Add("image", new {
                        media_id = message
                    });
                    break;
                case BroadcastType.mpnews:
                    data.Add("msgtype", "mpnews");
                    data.Add("mpnews", new {
                        media_id = message
                    });
                    break;
                case BroadcastType.text:
                    data.Add("msgtype", "text");
                    data.Add("text", new {
                        content = message
                    });
                    break;
                case BroadcastType.video:
                    data.Add("msgtype", "mpvideo");
                    data.Add("mpvideo", new {
                        media_id = message
                    });
                    break;
                case BroadcastType.voice:
                    data.Add("msgtype", "voice");
                    data.Add("voice", new {
                        media_id = message
                    });
                    break;
            }
            result = CommonJsonSend.Send<BroadcastJsonResult>(accessToken, url, data);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    #endregion

    #region 二维码相关

    /// <summary>
    /// 创建永久二维码
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="key">1到64位自定义KEY</param>
    /// <returns></returns>
    public static QrcodeJsonResult CreateForeverQrcode(string accessToken, string key) {
        QrcodeJsonResult result = new QrcodeJsonResult();
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"action_name","QR_LIMIT_STR_SCENE"},
                {"action_info",new {
                    scene=new {
                        scene_str=key
                    }
                }},
            };
            result = CommonJsonSend.Send<QrcodeJsonResult>(accessToken, url, data, CommonJsonSendType.POST);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    /// <summary>
    /// 创建临时二维码
    /// </summary>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="sceneid">场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000）</param>
    /// <param name="expire_seconds">该二维码有效时间，以秒为单位。 最大不超过2592000（即30天），此字段如果不填，则默认有效期为30秒</param>
    /// <returns></returns>
    public static QrcodeJsonResult CreateTempQrcode(string accessToken, int sceneid, long expire_seconds) {
        QrcodeJsonResult result = new QrcodeJsonResult();
        try {
            string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            Dictionary<string, object> data = new Dictionary<string, object>() {
                {"expire_seconds",expire_seconds},
                {"action_name","QR_SCENE"},
                {"action_info",new {
                    scene=new {
                        scene_id=sceneid
                    }
                }},
            };
            result = CommonJsonSend.Send<QrcodeJsonResult>(accessToken, url, data, CommonJsonSendType.POST);
        }
        catch (Exception ex) {
            // TODO
        }
        return result;
    }

    #endregion

}