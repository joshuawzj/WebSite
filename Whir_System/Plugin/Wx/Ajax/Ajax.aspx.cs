using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.Media.MediaJson;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class Whir_System_Handler_Plugin_Wx_Ajax : WxBaseHandler
{

    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }


    #region 公众号管理

    /// <summary>
    /// 获取公众号信息
    /// </summary>
    public void WxCredences()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("391"), true);
        List<WxCredence> list = WxConfigRepository.GetConfig().CredenceSet;
        long total = list.Count;
        string data = list.Skip((this.CurrentPage - 1) * this.PageSize).Take(this.PageSize).ToJson();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 保存公众号信息
    /// </summary>
    public HandlerResult SaveCredence()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("392"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();

        WxCredence credence = WxConfigRepository.GetCredence(RequestUtil.Instance.GetFormString("AppId")) ?? new WxCredence() { };
        credence.AppName = RequestUtil.Instance.GetFormString("AppName");
        credence.Token = RequestUtil.Instance.GetFormString("Token");
        credence.AppSecret = RequestUtil.Instance.GetFormString("AppSecret");
        credence.OriginalId = RequestUtil.Instance.GetFormString("OriginalId");
        credence.WxAccount = RequestUtil.Instance.GetFormString("WxAccount");

        try
        {
            if (!credence.AppId.IsEmpty())
            {
                WxConfigRepository.UpdateCredence(credence);
            }
            else
            {
                credence.AppId = RequestUtil.Instance.GetFormString("AppId");
                WxConfigRepository.AddCredence(credence);
            }
            result.Status = true;
            result.Message = "保存成功".ToLang();
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
        }

        return result;
    }

    /// <summary>
    /// 设置为当前公众号
    /// </summary>
    /// <returns></returns>
    public HandlerResult SetCurrentCredence()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("391"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();

        string appId = RequestUtil.Instance.GetFormString("appid");
        try
        {
            WxUtility.SetCurrentCredence(appId);
            result.Status = true;
            result.Message = "设置成功".ToLang();
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
        }

        return result;
    }

    /// <summary>
    /// 删除公众号信息
    /// </summary>
    /// <returns></returns>
    public HandlerResult RemoveCredence()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("391"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();

        string[] apps = RequestUtil.Instance.GetFormString("appids").Split(',');

        try
        {
            WxConfigRepository.RemoveCredence(apps);
            result.Status = true;
            result.Message = "删除成功".ToLang();
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
        }

        return result;
    }

    #endregion

    #region 用户管理

    /// <summary>
    /// 获取用户集合
    /// </summary>
    public void GetUsers()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("393"), true);
        string total = "0", data = "[]", tag = RequestUtil.Instance.GetQueryString("tag");
        WxCredence credence = this.CurrentCredence;
        if (credence != null)
        {
            if (tag.ToInt32() > 0)
            {
                var result = WxUserRepository.Page(credence.AppId, tag.ToInt32(), this.CurrentPage, this.PageSize);
                total = result.TotalItems.ToStr();
                data = result.Items.ToJson();
            }
            else
            {
                var result = WxUserRepository.Page(credence.AppId, this.CurrentPage, this.PageSize);
                total = result.TotalItems.ToStr();
                data = result.Items.ToJson();
            }
        }

        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 同步用户信息
    /// </summary>
    public HandlerResult SyncUsers()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("393"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }

        AsyncUsers();

        result.Status = true;
        result.Message = "已启动同步任务，同步结果请查看用户列表".ToLang();

        return result;
    }

    /// <summary>
    /// 设置用户备注
    /// </summary>
    /// <returns></returns>
    public HandlerResult SetUserRemark()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("393"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        string openid = RequestUtil.Instance.GetFormString("openid"), remark = RequestUtil.Instance.GetFormString("remark");
        result.Status = WxUtility.SetUserRemark(credence.AccessToken, openid, remark);
        if (result.Status)
        {
            WxUser user = WxUserRepository.Find(openid);
            if (user != null)
            {
                user.remark = remark;
                WxUserRepository.Update(user);
            }
            result.Message = "设置成功".ToLang();
        }
        else
        {
            result.Message = "设置失败";
        }

        return result;
    }

    /// <summary>
    /// 刷新用户信息
    /// </summary>
    /// <returns></returns>
    public HandlerResult SyncUser()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("393"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        string openid = RequestUtil.Instance.GetFormString("openid");
        AsyncSingleUser(credence.AppId, credence.AccessToken, openid);
        result.Status = true;
        result.Message = "信息已刷新".ToLang();

        return result;
    }

    /// <summary>
    /// 异步更新用户信息
    /// </summary>
    void AsyncUsers()
    {
        var factory = new System.Threading.Tasks.TaskFactory();
        string nextId = string.Empty;
        WxCredence credence = this.CurrentCredence;
        var task = factory.StartNew(() =>
        {
            int currPage = 1, pageSize = 100, records = 0, pages = 0;
            do
            {
                if (credence.TokenExpired <= DateTime.Now)
                {
                    credence = WxUtility.GetCredence(credence.AppId);
                }
                WxUsersJsonResult result = WxUtility.GetUsers(credence.AccessToken, nextId, currPage, pageSize);
                records = result.total;
                pages = records % pageSize != 0 ? records / pageSize + 1 : records / pageSize;
                string[] openids = result.data["openid"];
                foreach (var openid in openids)
                {
                    AsyncSingleUser(credence.AppId, credence.AccessToken, openid);
                }
                nextId = result.next_openid;
                currPage++;
            }
            while (pages >= currPage);

        });

        // 处理完通知用户
        //factory.ContinueWhenAll(new System.Threading.Tasks.Task[] { task }, (tasks) => {

        //});
    }

    /// <summary>
    /// 同步单个用户
    /// </summary>
    /// <param name="appId">公众号账号</param>
    /// <param name="accessToken">访问令牌</param>
    /// <param name="openid">用户openid</param>
    void AsyncSingleUser(string appId, string accessToken, string openid)
    {
        WxUserJsonResult user = WxUtility.GetUser(accessToken, openid);
        if (user != null)
        {
            WxUser wx = WxUserRepository.Map(user);
            int userId = WxUserRepository.FindPrimaryKey(wx.openid);
            if (userId > 0)
            {
                // 更新
                wx.Whir_Wx_UsersId = userId;
                wx.appid = appId;
                WxUserRepository.Update(wx);
            }
            else
            {
                // 添加
                wx.appid = appId;
                WxUserRepository.Add(wx);
            }
        }
    }

    #endregion

    #region 标签管理

    /// <summary>
    /// 获取标签
    /// </summary>
    public void GetTags()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("393"), true);
        string total = "0", data = "[]";
        WxCredence credence = this.CurrentCredence;
        if (credence != null)
        {
            ArrayList result = WxUtility.GetTags(credence.AccessToken);
            if (result != null)
            {
                total = result.Count.ToStr();
                data = result.ToJson();
            }
        }

        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 添加标签
    /// </summary>
    /// <returns></returns>
    public HandlerResult AddTag()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("393"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        string tag = RequestUtil.Instance.GetFormString("name");
        WxJsonResult response = WxUtility.AddTag(credence.AccessToken, tag);
        result.Status = response.errcode > 0;
        if (result.Status)
        {
            result.Message = "保存成功".ToLang();
        }
        else
        {
            result.Message = response.errmsg;
        }

        return result;
    }

    /// <summary>
    /// 更新标签
    /// </summary>
    /// <returns></returns>
    public HandlerResult UpdateTag()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("393"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        string tagid = RequestUtil.Instance.GetFormString("tagid"), tag = RequestUtil.Instance.GetFormString("name");
        WxJsonResult response = WxUtility.UpdateTag(credence.AccessToken, tagid.ToInt32(), tag);
        result.Status = response.errcode == 0;
        if (result.Status)
        {
            result.Message = "保存成功".ToLang();
        }
        else
        {
            result.Message = response.errmsg;
        }

        return result;
    }

    /// <summary>
    /// 删除标签
    /// </summary>
    /// <returns></returns>
    public HandlerResult RemoveTag()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("393"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        WxJsonResult response = WxUtility.RemoveTag(credence.AccessToken, RequestUtil.Instance.GetFormString("tagid").ToInt32());
        result.Status = response.errcode == 0;
        if (result.Status)
        {
            result.Message = "保存成功".ToLang();
        }
        else
        {
            result.Message = response.errmsg;
        }

        return result;
    }

    /// <summary>
    /// 设置标签
    /// </summary>
    /// <returns></returns>
    public HandlerResult SetTags()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("393"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        string[] openids = RequestUtil.Instance.GetFormString("users").Split(','), tags = RequestUtil.Instance.GetFormString("tags").Split(',');
        if (openids.Length == 0 || tags.Length == 0)
        {
            result.Status = false;
            result.Message = "请选择至少一个用户和一个标签".ToLang();
            return result;
        }
        foreach (var tag in tags)
        {
            var response = WxUtility.SetTag(credence.AccessToken, tag.ToInt32(), openids);
            result.Status = response.errcode == 0;
            if (result.Status)
            {
                result.Message = result.Message + response.errmsg;
            }
        }
        foreach (var openid in openids)
        {
            AsyncSingleUser(credence.AppId, credence.AccessToken, openid);
        }
        if (result.Status)
        {
            result.Message = "保存成功";
        }

        return result;
    }

    /// <summary>
    /// 取消标签
    /// </summary>
    /// <returns></returns>
    public HandlerResult CancelTags()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("393"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        string[] openids = RequestUtil.Instance.GetFormString("users").Split(','), tags = RequestUtil.Instance.GetFormString("tags").Split(',');
        if (openids.Length == 0 || tags.Length == 0)
        {
            result.Status = false;
            result.Message = "请选择至少一个用户和一个标签".ToLang();
            return result;
        }
        foreach (var tag in tags)
        {
            var response = WxUtility.CancelTag(credence.AccessToken, tag.ToInt32(), openids);
            result.Status = response.errcode == 0;
            if (result.Status)
            {
                result.Message = result.Message + response.errmsg;
            }
        }
        foreach (var openid in openids)
        {
            AsyncSingleUser(credence.AppId, credence.AccessToken, openid);
        }
        if (result.Status)
        {
            result.Message = "保存成功".ToLang();
        }

        return result;
    }

    #endregion

    #region 素材相关

    /// <summary>
    /// 上传素材
    /// </summary>
    /// <returns></returns>
    public HandlerResult UploadMedia()
    {
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        if (Request.Files.Count == 0)
        {
            result.Status = false;
            result.Message = "请选择文件".ToLang();
            return result;
        }
        try
        {
            string title = RequestUtil.Instance.GetFormString("title"), introduction = RequestUtil.Instance.GetFormString("introduction");

            HttpPostedFile postedFile = Request.Files[0];
            UploadMediaFileType mediaType = (UploadMediaFileType)System.Enum.Parse(typeof(UploadMediaFileType), RequestUtil.Instance.GetFormString("media"));
            string virtualPath = UploadFilePath + "wx/" + Path.GetFileName(postedFile.FileName), filePath = Server.MapPath(virtualPath), direcoty = Path.GetDirectoryName(filePath);
            if (mediaType == UploadMediaFileType.video)
            {
                if (postedFile.ContentLength > 10485760)
                {
                    result.Status = false;
                    result.Message = "上传的文件不能超过10M".ToLang();
                    return result;
                }
                if (title.IsEmpty())
                {
                    title = Path.GetFileName(postedFile.FileName);
                }
                if (introduction.IsEmpty())
                {
                    introduction = title;
                }
            }

            if (!direcoty.Contains(Server.MapPath(UploadFilePath).Trim('\\')))
            {
                result.Status = false;
                result.Message = "上传失败：试图在非法路径上传文件".ToLang();
                return result;
            }

            if (mediaType == UploadMediaFileType.image || mediaType == UploadMediaFileType.thumb)
            {
                filePath = ServiceFactory.UploadFilesService.UploadImage(postedFile, direcoty, UploadFilePath);
            }
            else
            {
                filePath = ServiceFactory.UploadFilesService.UploadFile(postedFile, direcoty);
            }

            //上传文件名称和物理名称到数据库
            Upload upload = new Upload();
            upload.Name = postedFile.FileName;
            upload.RealName = filePath.Substring(filePath.LastIndexOf('\\') + 1); ;
            upload.Path = filePath.Replace(Server.MapPath(UploadFilePath), "").Replace("\\", "/");
            upload.IsDel = false;
            upload.State = 0;
            upload.CreateDate = DateTime.Now;
            upload.UpdateDate = DateTime.Now;
            upload.UpdateUser = CurrentUser.LoginName;
            upload.CreateUser = CurrentUser.LoginName;
            ServiceFactory.UploadService.Save(upload);

            UploadForeverMediaResult response = WxUtility.AddSampleMedia(credence.AccessToken, filePath, mediaType, title, introduction);
            if (response.errcode == 0 && !response.media_id.IsEmpty())
            {
                // 保存到数据库
                var o = new WxSampleMedia()
                {
                    AppId = credence.AppId,
                    WebURL = response.url,
                    LocalURL = virtualPath,
                    Title = title.IsEmpty() ? Path.GetFileName(filePath) : title,
                    MediaType = mediaType,
                    MediaSize = 0,
                    MediaName = Path.GetFileName(filePath),
                    MediaId = response.media_id,
                    Introduction = introduction,
                    GroupId = 0,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };
                WxSampleMediaRepository.Add(o);

                result.Status = true;
                result.Message = string.Format("{0}|{1}|{2}|{3}", response.media_id, response.url, virtualPath, o.Title);
            }
            else
            {
                result.Message = response.errmsg;
            }
        }
        catch (Exception ex)
        {
            // TODO
            result.Message = ex.Message;
        }


        return result;
    }

    public void KindEditorImageUploader()
    {
        string result = "{{\"error\":{0},\"url\":\"{1}\",\"message\":\"{2}\"}}";
        WxCredence credence = this.CurrentCredence;
        Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        if (credence == null)
        {
            result = string.Format(result, 1, "", "请设置当前公众号".ToLang());
            Response.Write(result);
            return;
        }
        if (Request.Files.Count == 0)
        {
            result = string.Format(result, 1, "", "请选择文件".ToLang());
            Response.Write(result);
            return;
        }
        try
        {

            HttpPostedFile postedFile = Request.Files[0];
            string virtualPath = UploadFilePath +"wx/"+ Path.GetFileName(postedFile.FileName), filePath = Server.MapPath(virtualPath), direcoty = Path.GetDirectoryName(filePath);

            if (postedFile.ContentLength > 1048000)
            {
                result = string.Format(result, 1, "", "图片大小不能超过1M".ToLang());
                Response.Write(result);
                return;
            }

            filePath = ServiceFactory.UploadFilesService.UploadImage(postedFile, direcoty, UploadFilePath);

            //上传文件名称和物理名称到数据库
            Upload upload = new Upload();
            upload.Name = postedFile.FileName;
            upload.RealName = filePath.Substring(filePath.LastIndexOf('\\') + 1); ;
            upload.Path = filePath.Replace(Server.MapPath(UploadFilePath), "").Replace("\\", "/");
            upload.IsDel = false;
            upload.State = 0;
            upload.CreateDate = DateTime.Now;
            upload.UpdateDate = DateTime.Now;
            upload.UpdateUser = CurrentUser.LoginName;
            upload.CreateUser = CurrentUser.LoginName;
            ServiceFactory.UploadService.Save(upload);


            UploadForeverMediaResult response = WxUtility.AddSampleMedia(credence.AccessToken, filePath, UploadMediaFileType.news, string.Empty, string.Empty);
            if (response.errcode == 0 && !response.url.IsEmpty())
            {
                result = string.Format(result, 0, virtualPath + "?m=" + this.Server.UrlEncode(response.url), "");
            }
            else
            {
                result = string.Format(result, 1, "", "上传失败".ToLang());
            }
        }
        catch (Exception ex)
        {
            // TODO

        }
        Response.Write(result);
    }

    /// <summary>
    /// 获取素材
    /// </summary>
    public void GetMedias()
    {
        string type = RequestUtil.Instance.GetQueryString("media");
        if (type == "news")
        {
            SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("394"), true);
        }
        else if (type == "image")
        {
            SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("395"), true);
        }
        else if (type == "voice")
        {
            SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("396"), true);
        }
        else if (type == "video")
        {
            SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("397"), true);
        }
        string total = "0", data = "[]";
        WxCredence credence = this.CurrentCredence;
        if (credence != null)
        {
            UploadMediaFileType mediaType = (UploadMediaFileType)System.Enum.Parse(typeof(UploadMediaFileType), RequestUtil.Instance.GetQueryString("media"));
            if (mediaType == UploadMediaFileType.news)
            {
                var result = WxArticleRepository.Page(credence.AppId, this.CurrentPage, this.PageSize);
                total = result.TotalItems.ToStr();
                List<object> list = new List<object>();
                result.Items.ForEach(m =>
                {
                    list.Add(new
                    {
                        m.AppId,
                        m.ArticleIndex,
                        m.ArticleType,
                        m.Author,
                        m.Detail,
                        m.IsShowCover,
                        m.MediaId,
                        m.SourceURL,
                        m.Summary,
                        m.ThumbMediaId,
                        m.ThumbMediaURL,
                        m.Title,
                        m.UpdateDate,
                        m.Whir_Wx_ArticleId,
                        Children = WxArticleRepository.ChildrenArticle(m.MediaId)
                    });
                });
                data = list.ToJson();
            }
            else
            {
                var result = WxSampleMediaRepository.Page(credence.AppId, mediaType, this.CurrentPage, this.PageSize);
                total = result.TotalItems.ToStr();
                data = result.Items.ToJson();
            }
        }

        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 同步媒体信息
    /// </summary>
    public HandlerResult SyncMedias()
    {
        string mediaType = RequestUtil.Instance.GetFormString("media");
        string menures = "";
        if (mediaType == "news")
        {
            menures = "394";

        }
        else if (mediaType == "image")
        {
            menures = "395";

        }
        else if (mediaType == "voice")
        {
            menures = "396";

        }
        else if (mediaType == "video")
        {
            menures = "397";

        }
        if (!menures.IsEmpty())
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes(menures));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        List<UploadMediaFileType> medias = mediaType.IsEmpty() ?
            new List<UploadMediaFileType>() {
                UploadMediaFileType.image,
                UploadMediaFileType.news,
                 UploadMediaFileType.thumb,
                 UploadMediaFileType.video,
                 UploadMediaFileType.voice
            } : new List<UploadMediaFileType>() {
                (UploadMediaFileType)System.Enum.Parse(typeof(UploadMediaFileType),mediaType)
            };

        AsyncMedias(medias);

        result.Status = true;
        result.Message = "已启动同步任务，同步结果请查看列表".ToLang();

        return result;
    }

    /// <summary>
    /// 异步更新媒体
    /// </summary>
    void AsyncMedias(List<UploadMediaFileType> medias)
    {
        var factory = new System.Threading.Tasks.TaskFactory();
        string nextId = string.Empty;
        WxCredence credence = this.CurrentCredence;
        var task = factory.StartNew(() =>
        {
            medias.ForEach(m =>
            {
                int currPage = 1, pageSize = 10, records = 0, pages = 0;
                do
                {
                    if (credence.TokenExpired <= DateTime.Now)
                    {
                        credence = WxUtility.GetCredence(credence.AppId);
                    }
                    // 修复问题（假设分页大小是10，当最后一页不满10的时候，返回的数据依旧是10调）
                    if (records > 0 && (currPage * pageSize) > records)
                    {
                        pageSize = records - ((currPage - 1) * pageSize);
                    }
                    switch (m)
                    {
                        case UploadMediaFileType.image:
                        case UploadMediaFileType.video:
                        case UploadMediaFileType.voice:
                        case UploadMediaFileType.thumb:
                            MediaResult<SampleMediaJsonResult> sample = WxUtility.GetMedias<SampleMediaJsonResult>(credence.AccessToken, m, currPage, pageSize);
                            records = sample.total_count;
                            pages = records % pageSize != 0 ? records / pageSize + 1 : records / pageSize;
                            sample.item.ForEach(w =>
                            {
                                AsyncSingleSampleMedia(credence.AppId, m, w);
                            });
                            break;
                        case UploadMediaFileType.news:
                            MediaResult<ArticleMediaJsonResult> article = WxUtility.GetMedias<ArticleMediaJsonResult>(credence.AccessToken, m, currPage, pageSize);
                            records = article.total_count;
                            pages = records % pageSize != 0 ? records / pageSize + 1 : records / pageSize;
                            article.item.ForEach(w =>
                            {
                                AsyncSingleArticleMedia(credence.AppId, w);
                            });
                            break;
                    }

                    currPage++;
                }
                while (pages >= currPage);

            });
        });

        // 处理完通知用户
        //factory.ContinueWhenAll(new System.Threading.Tasks.Task[] { task }, (tasks) => {

        //});
    }

    /// <summary>
    /// 同步单个简单素材
    /// </summary>
    /// <param name="appId">公众号账号</param>
    /// <param name="mediaType">媒体类型</param>
    /// <param name="media">媒体信息</param>
    void AsyncSingleSampleMedia(string appId, UploadMediaFileType mediaType, SampleMediaJsonResult media)
    {
        WxSampleMedia model = WxSampleMediaRepository.Find(media.media_id);
        if (model != null)
        {
            model.MediaName = media.name;
            model.Title = media.name;
            WxSampleMediaRepository.Update(model);
        }
        else
        {
            model = new WxSampleMedia()
            {
                AppId = appId,
                CreateDate = DateTime.Now,
                GroupId = 0,
                Introduction = string.Empty,
                MediaId = media.media_id,
                MediaName = media.name,
                MediaSize = 0,
                MediaType = mediaType,
                Title = media.name,
                UpdateDate = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(media.update_time),
                WebURL = media.url
            };
            WxSampleMediaRepository.Add(model);
        }
    }

    /// <summary>
    /// 同步图文信息
    /// </summary>
    /// <param name="appId">公众号账号</param>
    /// <param name="media">媒体信息</param>
    void AsyncSingleArticleMedia(string appId, ArticleMediaJsonResult media)
    {

        // 先清除原有信息
        WxArticleRepository.Remove(media.media_id);
        int articleIndex = 0;
        media.content.news_item.ForEach(m =>
        {
            WxArticleRepository.Add(new WxArticle()
            {
                AppId = appId,
                ArticleIndex = articleIndex,
                ArticleType = media.content.news_item.Count > 0 ? ArticleType.Multiple : ArticleType.Single,
                Author = m.author,
                OriginalDetail = m.content,
                Detail = m.content,
                IsShowCover = m.show_cover_pic == 1,
                MediaId = media.media_id,
                SourceURL = m.content_source_url,
                Summary = m.digest,
                ThumbMediaId = m.thumb_media_id,
                Title = m.title,
                ViewURL = m.url,
                UpdateDate = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(media.update_time),
            });
            articleIndex++;
        });
    }

    /// <summary>
    /// 删除素材
    /// </summary>
    /// <returns></returns>
    public HandlerResult RemoveMedia()
    {
        string mediaType = RequestUtil.Instance.GetFormString("media");
        string menures = "";
        if (mediaType == "news")
        {
            menures = "394";
        }
        else if (mediaType == "image")
        {
            menures = "395";
        }
        else if (mediaType == "voice")
        {
            menures = "396";
        }
        else if (mediaType == "video")
        {
            menures = "397";
        }
        if (!menures.IsEmpty())
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes(menures));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
        }
        HandlerResult result = new HandlerResult()
        {
            Message = "删除失败".ToLang()
        };
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        string[] medias = RequestUtil.Instance.GetFormString("medias").Split(',');
        int success = 0;
        foreach (var mediaId in medias)
        {
            WxJsonResult response = WxUtility.RemoveMedia(credence.AccessToken, mediaId);
            if (response.errcode == 0)
            {
                if (mediaType.IsEmpty())
                {
                    WxSampleMediaRepository.Remove(mediaId);
                    // TODO 删除图文
                }
                else
                {
                    UploadMediaFileType media = (UploadMediaFileType)System.Enum.Parse(typeof(UploadMediaFileType), mediaType);
                    if (media == UploadMediaFileType.news)
                    {
                        // TODO 删除图文
                        WxArticleRepository.Remove(mediaId);
                    }
                    else
                    {
                        WxSampleMediaRepository.Remove(mediaId);
                    }
                }
                success++;
            }
            else
            {
                result.Message = result.Message + response.errmsg;
            }
        }
        if (success > 0)
        {
            result.Status = true;
            if (success == medias.Length)
            {
                result.Message = "删除成功".ToLang();
            }
            else
            {
                result.Message = "部分删除失败".ToLang() + " " + result.Message;
            }
        }
        return result;
    }

    ///// <summary>
    ///// 设置素材名称
    ///// </summary>
    ///// <returns></returns>
    //public HandlerResult SetMediaName() {
    //    HandlerResult result = new HandlerResult();
    //    WxCredence credence = this.CurrentCredence;
    //    if (credence == null) {
    //        result.Status = false;
    //        result.Message = "请先设置当前公众号";
    //        return result;
    //    }
    //    string mediaId = RequestUtil.Instance.GetFormString("media"), name = RequestUtil.Instance.GetFormString("name");
    //    WxSampleMedia media = WxSampleMediaRepository.Find(mediaId);
    //    if (media != null) {
    //        media.MediaName = name;
    //        result.Status = WxSampleMediaRepository.Update(media);
    //    }
    //    if (result.Status) {
    //        result.Message = "设置成功";
    //    }
    //    else {
    //        result.Message = "设置失败";
    //    }

    //    return result;
    //}

    /// <summary>
    /// 获取多媒体信息
    /// </summary>
    /// <returns></returns>
    public HandlerResult GetMedia()
    {
        HandlerResult result = new HandlerResult();

        var media = WxSampleMediaRepository.Find(RequestUtil.Instance.GetFormString("media"));
        if (media != null)
        {
            result.Status = true;
            result.Message = media.ToJson();
        }
        else
        {
            result.Message = "未获取到媒体信息".ToLang();
        }

        return result;
    }

    /// <summary>
    /// 设定值多媒体信息
    /// </summary>
    /// <returns></returns>
    public HandlerResult SetMedia()
    {
        HandlerResult result = new HandlerResult();

        var media = WxSampleMediaRepository.Find(RequestUtil.Instance.GetFormString("media"));
        if (media != null)
        {
            string title = RequestUtil.Instance.GetFormString("title"), introduction = RequestUtil.Instance.GetFormString("introduction");
            media.Title = title;
            media.Introduction = introduction;
            media.UpdateDate = DateTime.Now;

            result.Status = WxSampleMediaRepository.Update(media);
        }
        if (result.Status)
        {
            result.Message = "保存成功".ToLang();
        }
        else
        {
            result.Message = "保存失败".ToLang();
        }

        return result;
    }

    /// <summary>
    /// 获取文章
    /// </summary>
    /// <returns></returns>
    public HandlerResult GetArticle()
    {
        HandlerResult result = new HandlerResult();

        var media = WxArticleRepository.Find(RequestUtil.Instance.GetFormString("media"));
        if (media != null)
        {
            media.ForEach(m =>
            {
                if (m.OriginalDetail.IsEmpty())
                {
                    m.OriginalDetail = m.Detail;
                }
            });
            result.Status = true;
            result.Message = media.ToJson();
        }
        else
        {
            result.Message = "未获取到媒体信息".ToLang();
        }

        return result;
    }

    /// <summary>
    /// 保存图文信息
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveArticle()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("394"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        string mediaId = RequestUtil.Instance.GetFormString("media"), data = RequestUtil.Instance.GetFormString("articles");
        if (data.IsEmpty())
        {
            result.Message = "文章信息错误".ToLang();
            return result;
        }
        try
        {
            List<WxArticle> articles = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<WxArticle>>(data);
            if (articles.Count == 0)
            {
                result.Message = "请完善表单信息".ToLang();
                return result;
            }
            if (mediaId.IsEmpty())
            {
                // 新增图文信息
                UploadForeverMediaResult response = WxUtility.AddArticleMedia(credence.AccessToken, articles);
                if (response.errcode == 0)
                {
                    mediaId = response.media_id;
                    articles.ForEach(m =>
                    {
                        m.AppId = credence.AppId;
                        m.MediaId = response.media_id;
                        m.ArticleType = articles.Count > 0 ? ArticleType.Multiple : ArticleType.Single;
                        m.UpdateDate = DateTime.Now;
                        WxArticleRepository.Add(m);
                    });
                    result.Status = true;
                    result.Message = "保存成功".ToLang();
                }
                else
                {
                    result.Message = response.errmsg;
                }
            }
            else
            {
                // 修改图文信息

                bool isSuccess = false;
                articles.ForEach(m =>
                {
                    UploadForeverMediaResult response = WxUtility.UpateArticleMedia(credence.AccessToken, m);
                    isSuccess = isSuccess || response.errcode == 0;
                });
                if (isSuccess)
                {
                    // 清除原来的，再保存回去
                    WxArticleRepository.Remove(mediaId);
                    articles.ForEach(m =>
                    {
                        m.AppId = credence.AppId;
                        m.MediaId = mediaId;
                        m.ArticleType = articles.Count > 0 ? ArticleType.Multiple : ArticleType.Single;
                        m.ArticleType = articles.Count > 0 ? ArticleType.Multiple : ArticleType.Single;
                        m.UpdateDate = DateTime.Now;
                        WxArticleRepository.Add(m);
                    });
                }
                result.Status = true;
                result.Message = "保存成功".ToLang();
            }
            // 更新ViewURL地址
            var media = WxUtility.GetArticleMedia(credence.AccessToken, mediaId);
            if (media != null && media.news_item != null)
            {
                for (int i = 0; i < media.news_item.Count; i++)
                {
                    if (articles[i] != null)
                    {
                        articles[i].ViewURL = media.news_item[i].url;
                    }
                }
                articles.ForEach(k =>
                {
                    WxArticleRepository.Update(k);
                });
            }
        }
        catch (Exception ex)
        {
            // TODO
        }


        return result;
    }

    #endregion

    #region 菜单相关

    /// <summary>
    /// 获取菜单
    /// </summary>
    /// <returns></returns>
    public HandlerResult GetMenus()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("401"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();

        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }

        MenuJsonResult response = WxUtility.GetMenus(credence.AccessToken);
        if (response.errcode == 0)
        {
            if (response.menu != null && response.menu.button != null)
            {
                response.menu.button.ForEach(m =>
                {
                    m = this.RepairMenuButton(credence.AppId, m);
                    if (m.sub_button != null && m.sub_button.Count > 0)
                    {
                        m.sub_button.ForEach(c =>
                        {
                            c = this.RepairMenuButton(credence.AppId, c);
                        });
                    }
                });
            }
            result.Status = true;
            result.Message = response.ToJson();
        }
        else
        {
            result.Message = response.errmsg;
        }

        return result;
    }

    /// <summary>
    /// 保存菜单配置
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveMenus()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("401"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        List<Senparc.Weixin.MP.Entities.MenuJsonResult.Menu.Button> menus = null;
        try
        {
            menus = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<Senparc.Weixin.MP.Entities.MenuJsonResult.Menu.Button>>(RequestUtil.Instance.GetFormString("menus"));
        }
        catch (Exception ex)
        {

        }
        if (menus == null)
        {
            result.Status = false;
            result.Message = "菜单信息错误".ToLang();
            return result;
        }
        WxJsonResult response = WxUtility.SetMenus(credence.AccessToken, menus);
        if (response.errcode == 0)
        {
            // 处理菜单文本类型，写入自动回复
            menus.ForEach(m =>
            {
                if (m.type == "click")
                {
                    WxReplyRepository.SetMenuEventReply(credence.AppId, m);
                }
                if (m.sub_button != null && m.sub_button.Count > 0)
                {
                    m.sub_button.ForEach(w =>
                    {
                        if (w.type == "click")
                        {
                            WxReplyRepository.SetMenuEventReply(credence.AppId, w);
                        }
                    });
                }
            });

            result.Status = true;
            result.Message = "设置成功".ToLang();
        }
        else
        {
            result.Message = "设置失败".ToLang();
        }

        return result;
    }

    /// <summary>
    /// 修正按钮数据
    /// </summary>
    /// <param name="button">需要修正的按钮</param>
    /// <returns></returns>
    Senparc.Weixin.MP.Entities.MenuJsonResult.Menu.Button RepairMenuButton(string appId, Senparc.Weixin.MP.Entities.MenuJsonResult.Menu.Button button)
    {
        switch (button.type)
        {
            case "view":
                button.user_type = "url";
                break;
            case "click":
                button.user_type = "text";
                //  需要从自动回复处取（保存菜单的时候文本类型自动保存到自动回复）
                button.data = (WxReplyRepository.Find(appId, ReplyType.Keyword).Where(m => m.RoleName == button.name).OrderByDescending(o => o.UpdateDate).FirstOrDefault() ?? new WxReply()).MessageDetail;
                break;
            case "view_limited":
                button.user_type = "article";
                // TODO 需要从图文信息取
                List<WxArticle> articles = WxArticleRepository.Find(button.media_id);
                if (articles != null)
                {
                    List<object> rows = new List<object>();
                    articles.ForEach(w =>
                    {
                        rows.Add(new
                        {
                            title = w.Title,
                            image = w.ThumbMediaURL
                        });
                    });
                    button.data = rows;
                }
                break;
            case "media_id":
                WxSampleMedia sample = WxSampleMediaRepository.Find(button.media_id);
                if (sample != null)
                {
                    switch (sample.MediaType)
                    {
                        case UploadMediaFileType.image:
                            button.user_type = "image";
                            button.data = sample.LocalURL.IsEmpty() ? sample.WebURL : sample.LocalURL;
                            break;
                        case UploadMediaFileType.video:
                            button.user_type = "video";
                            button.data = sample.Title;
                            break;
                        case UploadMediaFileType.voice:
                            button.user_type = "voice";
                            button.data = sample.Title;
                            break;
                    }
                }
                break;
        }
        return button;
    }

    #endregion

    #region 群发相关

    /// <summary>
    /// 群发信息
    /// </summary>
    /// <returns></returns>
    public HandlerResult Broadcast()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("402"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        string data = RequestUtil.Instance.GetFormString("data"), castStyle = RequestUtil.Instance.GetFormString("style");
        BroadcastType castType = (BroadcastType)RequestUtil.Instance.GetFormString("dataType").ToInt32();
        BroadcastJsonResult response = null;
        switch (castStyle)
        {
            case "tag": // 根据标签群发
                int tagid = RequestUtil.Instance.GetFormString("tagid").ToInt32();
                response = WxUtility.Broadcast(credence.AccessToken, castType, data, tagid);
                break;
            case "user": // 根据openid群发
                string[] users = RequestUtil.Instance.GetFormString("users").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                response = WxUtility.Broadcast(credence.AccessToken, castType, data, users);
                break;
            default: // 发送给所有用户
                response = WxUtility.Broadcast(credence.AccessToken, castType, data);
                break;
        }
        if (response.errcode == 0 && !response.msg_id.IsEmpty())
        {
            WxBroadcastRepository.Add(new WxBroadcast()
            {
                AppId = credence.AppId,
                CreateDate = DateTime.Now,
                MessageDetail = data,
                MessageId = response.msg_id,
                MessageDataId = response.msg_data_id,
                MessageType = castType,
                Status = BroadcastStatus.Success,
            });
            result.Status = true;
            result.Message = "发送成功".ToLang();
        }
        else
        {
            result.Status = false;
            result.Message = "发送失败".ToLang();
        }
        return result;
    }

    #endregion

    #region 回复相关

    /// <summary>
    /// 保存回复
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveReply()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("398"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        int id = RequestUtil.Instance.GetFormString("id").ToInt32();
        ReplyType replyType = (ReplyType)RequestUtil.Instance.GetFormString("replyType").ToInt32();
        ReplyDataType dataType = (ReplyDataType)RequestUtil.Instance.GetFormString("dataType").ToInt32();
        string message = RequestUtil.Instance.GetFormString("message");
        WxReply reply = WxReplyRepository.Find(id);
        if (reply == null)
        {
            reply = new WxReply()
            {
                AppId = credence.AppId,
                CreateDate = DateTime.Now
            };
        }
        reply.ReplyType = replyType;
        reply.ReplyDataType = dataType;
        reply.MessageDetail = message;
        reply.UpdateDate = DateTime.Now;
        if (reply.ReplyType == ReplyType.Keyword)
        {
            reply.Keywords = RequestUtil.Instance.GetFormString("keywords").Trim();
            if (reply.Keywords.IsEmpty())
            {
                result.Status = false;
                result.Message = "请填写关键词".ToLang();
                return result;
            }
            reply.RoleName = RequestUtil.Instance.GetFormString("roleName").Trim();
            if (reply.RoleName.IsEmpty() || reply.RoleName.Length > 60)
            {
                result.Status = false;
                result.Message = "规则名称必须在30个字符以内".ToLang();
                return result;
            }
        }

        if (reply.MessageDetail.IsEmpty())
        {
            result.Status = false;
            result.Message = "请填写或选择素材".ToLang();
            return result;
        }
        if (reply.Whir_Wx_ReplyId > 0)
        {
            result.Status = WxReplyRepository.Update(reply);
        }
        else
        {
            result.Status = WxReplyRepository.Add(reply);
        }
        if (result.Status)
        {
            result.Message = reply.Whir_Wx_ReplyId.ToStr();
        }
        else
        {
            result.Message = "保存失败".ToLang();
        }

        return result;
    }

    /// <summary>
    /// 获取关键词回复集合
    /// </summary>
    public void GetKeywordReplys()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("398"), true);
        string total = "0", data = "[]", tag = RequestUtil.Instance.GetQueryString("tag");
        WxCredence credence = this.CurrentCredence;
        if (credence != null)
        {
            var result = WxReplyRepository.Page(credence.AppId, ReplyType.Keyword, this.CurrentPage, this.PageSize);
            total = result.TotalItems.ToStr();
            List<object> o = new List<object>();
            result.Items.ForEach(m =>
            {
                o.Add(WxReplyRepository.RepairData(m));
            });
            data = o.ToJson();
        }

        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 删除回复
    /// </summary>
    /// <returns></returns>
    public HandlerResult RemoveReplys()
    {
        HandlerResult result = new HandlerResult();
        string[] replys = RequestUtil.Instance.GetFormString("items").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in replys)
        {
            WxReplyRepository.Remove(item.ToInt32());
        }
        result.Status = true;
        result.Message = "删除成功".ToLang();
        return result;
    }

    #endregion

    #region 消息相关

    /// <summary>
    /// 获取消息
    /// </summary>
    public void GetMessages()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("404"), true);
        string total = "0", data = "[]";
        WxCredence credence = this.CurrentCredence;
        if (credence != null)
        {
            var result = WxMessageRepository.Page(credence.OriginalId, this.CurrentPage, this.PageSize);
            total = result.TotalItems.ToStr();
            List<object> o = new List<object>();
            result.Items.ForEach(m =>
            {
                WxUser u = WxUserRepository.Find(m.FromUserName) ?? new WxUser();
                o.Add(new
                {
                    m.CreateDate,
                    m.FormData,
                    m.FromUserName,
                    m.MessageId,
                    m.MessageType,
                    m.ReplyData,
                    m.ToUserName,
                    m.Whir_Wx_MessageId,
                    NickName = u.nickname,
                    Avatar = u.headimgurl,
                    Sex = u.sex == 1 ? "男" : (u.sex == 2 ? "女" : "未知")
                });
            });
            data = o.ToJson();
        }

        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 删除消息
    /// </summary>
    /// <returns></returns>
    public HandlerResult RemoveMessages()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("404"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();

        string[] items = RequestUtil.Instance.GetFormString("items").Split(',');

        try
        {
            foreach (var item in items)
            {
                WxMessageRepository.Remove(item.ToStr());
            }
            result.Status = true;
            result.Message = "删除成功".ToLang();
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
        }

        return result;
    }

    #endregion

    #region 二维码相关

    /// <summary>
    /// 添加二维码
    /// </summary>
    /// <returns></returns>
    public HandlerResult AddQrcode()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("405"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();
        WxCredence credence = this.CurrentCredence;
        if (credence == null)
        {
            result.Status = false;
            result.Message = "请先设置当前公众号".ToLang();
            return result;
        }
        WxQrcode code = new WxQrcode()
        {
            AppId = credence.AppId,
            CreateDate = DateTime.Now,
            ExpiredDate = RequestUtil.Instance.GetFormString("Expired").ToDateTime(DateTime.Now),
            ExtraData = RequestUtil.Instance.GetFormString("Keyword"),
            QrcodeType = (RequestUtil.Instance.GetFormString("CodeType").ToInt32() == 1 ? QrocodeType.Temp : QrocodeType.Forever),
            Summary = RequestUtil.Instance.GetFormString("Summary"),
        };
        long expireSeconds = (long)(code.ExpiredDate.Value - DateTime.Now).TotalSeconds;
        QrcodeJsonResult response = null;
        if (code.QrcodeType == QrocodeType.Forever)
        {
            code.ExpiredDate = null;
            response = WxUtility.CreateForeverQrcode(credence.AccessToken, code.ExtraData);
        }
        else
        {
            response = WxUtility.CreateTempQrcode(credence.AccessToken, 1, expireSeconds);
        }
        result.Status = response.errcode == 0;
        if (result.Status)
        {
            code.Ticket = response.ticket;
            WxQrcodeRepository.Add(code);
            result.Message = "保存成功".ToLang();
        }
        else
        {
            result.Message = response.errmsg;
        }

        return result;
    }

    /// <summary>
    /// 获取二维码
    /// </summary>
    public void GetQrcodes()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("405"), true);
        string total = "0", data = "[]";
        WxCredence credence = this.CurrentCredence;
        if (credence != null)
        {
            var result = WxQrcodeRepository.Page(credence.AppId, this.CurrentPage, this.PageSize);
            total = result.TotalItems.ToStr();

            data = result.Items.ToJson();
        }

        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 删除二维码
    /// </summary>
    /// <returns></returns>
    public HandlerResult RemoveQrcodes()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("405"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        HandlerResult result = new HandlerResult();

        string[] items = RequestUtil.Instance.GetFormString("items").Split(',');

        try
        {
            foreach (var item in items)
            {
                WxQrcodeRepository.Remove(item.ToInt32());
            }
            result.Status = true;
            result.Message = "删除成功".ToLang();
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
        }

        return result;
    }

    #endregion
}