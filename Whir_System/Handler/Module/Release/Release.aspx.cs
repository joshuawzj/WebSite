using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Language;
using Whir.Label;
using System.Xml.Linq;
using Whir.Repository;

public partial class Whir_System_Handler_Module_Release_Release : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }


    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int AttachedId = RequestUtil.Instance.GetFormString("AttachedId").ToInt(0);
        try
        {
            Attached model = ServiceFactory.AttachedService.SingleOrDefault<Attached>(AttachedId) ?? ModelFactory<Attached>.Insten();
            var type = typeof(Attached);
            model = GetPostObject(type, model) as Attached;
            model.CreateFileUrl = model.CreateFileUrl.Trim().TrimStart('/');
            model.SiteId = CurrentSiteId;
            string[] tempUrl = model.CreateFileUrl.Split('/');
            foreach (string temp in tempUrl)
            {
                string t = temp.ToLower();
                if (t.StartsWith("index") || t.StartsWith("/index")
                    || t.StartsWith("list") || t.StartsWith("/list")
                    || t.StartsWith("info") || t.StartsWith("info"))
                {
                    return new HandlerResult { Status = false, Message = "页面名称不可用index、list或info开头".ToLang() };
                }
            }
            if (tempUrl[tempUrl.Length - 1].Contains("_"))
            {
                return new HandlerResult { Status = false, Message = "页面名称不可包含下划线".ToLang() };
            }
            model.CreateMode = 1;//默认静态
            if (model.AttachedId == 0)
            {
                ServiceFactory.AttachedService.Save(model);
                //记录操作日志
                ServiceFactory.AttachedService.SaveLog(model, "insert");
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };


            }
            else
            {

                ServiceFactory.AttachedService.Update(model);
                //记录操作日志
                ServiceFactory.AttachedService.SaveLog(model, "update");
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

            }

        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }

    }


    /// <summary>
    /// 发布
    /// </summary>
    /// <returns></returns>
    public HandlerResult AttachedCreate()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int attachedID = RequestUtil.Instance.GetFormString("AttachedId").ToInt(0);
        Attached model = ServiceFactory.AttachedService.SingleOrDefault<Attached>(attachedID);
        try
        {
            SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(CurrentSiteId);
            if (siteInfo != null)
                LabelHelper.Instance.BuildInclude(1, 0, siteInfo);

            LabelHelper.Instance.BuildAttach(1, 1, attachedID);

            string xmlPath = Server.MapPath(LabelHelper.BuildXmlPath);
            XElement xml = XElement.Load(xmlPath);
            var message = (from ele
                        in xml.Descendants("message")
                           select ele).LastOrDefault();
            var previewUrl = (from ele
                        in xml.Descendants("previewUrl")
                              select ele).FirstOrDefault();
            var alertContent = "{0}".FormatWith(message.Value);

            return new HandlerResult { Status = true, Message = alertContent };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = ex.Message };
        }

    }


    /// <summary>
    /// 删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult Delete()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int attachedID = RequestUtil.Instance.GetFormString("AttachedId").ToInt(0);
        Attached model = ServiceFactory.AttachedService.SingleOrDefault<Attached>(attachedID);
        ServiceFactory.AttachedService.Delete<Attached>(attachedID);
        ServiceFactory.AttachedService.SaveLog(model, "delete");
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }


    /// <summary>
    /// 批量发布
    /// </summary>
    /// <returns></returns>
    public HandlerResult AttachedAll()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string ids = RequestUtil.Instance.GetFormString("ids");

        SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(CurrentSiteId);
        if (siteInfo != null)
            LabelHelper.Instance.BuildInclude(1, 0, siteInfo);//发布公共页面
        string[] arrId = ids.Split(',');
        for (int i = 0; i < arrId.Length; i++)
        {
            int rid = arrId[i].ToInt();
            if (rid <= 0)
                continue;
            LabelHelper.Instance.BuildAttach(1, 1, arrId[i].ToInt());
        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

    }

    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser, true);
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int total = 0;
        var pageData = ServiceFactory.AttachedService.Page(out total, pageIndex, pageSize, " a.SiteId={0}".FormatWith(CurrentSiteId), "a.Sort DESC,a.CreateDate DESC");
        string data = pageData.ToJson();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 获取栏目参数配置 静态页面使用
    /// </summary>
    /// <returns></returns>
    public HandlerResult GetColumnParameter()
    {
        int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
        int subjectId = RequestUtil.Instance.GetFormString("SubjectId").ToInt(0);

        var list = DbHelper.CurrentDb.Query<ReleaseParameter>("Where ColumnId=@0", columnId).ToList();
        return new HandlerResult { Status = true, Message = list.ToJson() };

    }

    /// <summary>
    /// 保存栏目参数 静态页面使用
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveParameter()
    {
        int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
        int pageSize = RequestUtil.Instance.GetFormString("PageSize").ToInt(10);
        bool isPage = RequestUtil.Instance.GetFormString("IsPage").ToBoolean();
        var parameterNames = RequestUtil.Instance.GetFormString("ParameterName").Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        var parameterBinds = RequestUtil.Instance.GetFormString("ParameterBind").Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        var parameterSources = RequestUtil.Instance.GetFormString("ParameterSource").Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        var parameterTypes = RequestUtil.Instance.GetFormString("ParameterType").Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        var sql = "Where ColumnId=@0";
        DbHelper.CurrentDb.Delete<ReleaseParameter>(sql, columnId);

        for (int i = 0; i < parameterNames.Length; i++)
        {
            ReleaseParameter param = new ReleaseParameter();
            param.ColumnId = columnId;
            param.IsPage = isPage;
            param.PageSize = pageSize;
            param.ParameterBind = parameterBinds[i];
            param.ParameterName = parameterNames[i];
            param.ParameterSource = parameterSources[i];
            param.ParameterType = parameterTypes[i];

            DbHelper.CurrentDb.Insert(param);
        }


        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };


    }
}