using System;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Config;
using Whir.Config.Models;
using Whir.Language;
using System.Text.RegularExpressions;


public partial class Whir_System_Handler_Config_SeoSetting : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetFormString("_action");
        Exec(this, action);
    }


    /// <summary>
    /// 获取数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult GetSeoSeting()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("27"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int siteId = RequestUtil.Instance.GetFormString("siteId").ToInt(0);
        SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(siteId);
        if (siteInfo == null)
        {
            siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(CurrentSiteId);
        }
        return new HandlerResult { Status = true, Message = siteInfo.ToJson() };

    }
    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveSeoSetting()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("298"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int siteId = RequestUtil.Instance.GetFormString("Site").ToInt(0);
        try
        {
            SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(siteId) ?? ModelFactory<SiteInfo>.Insten();
            var type = typeof(SiteInfo);
            siteInfo = GetPostObject(type, siteInfo) as SiteInfo;
            ServiceFactory.SiteInfoService.Save(siteInfo);

            //记录操作日志
            ServiceFactory.SiteInfoService.SaveLog(siteInfo, "update");
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }

    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveSysSetting()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("371"));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            string editorCode = RequestUtil.Instance.GetFormString("EditorCode");

            SystemConfig sysSetting = ConfigHelper.GetSystemConfig();
            var type = typeof(SystemConfig);
            sysSetting = GetPostObject(type, sysSetting) as SystemConfig;
            XmlUtil.SerializerObject(ConfigHelper.GetAppConfigPath("SystemConfig.config"), type, sysSetting);

            //设置Ewebeditor的授权码
            string newLicense = editorCode.Trim();
            string sFileName = AppName + "Editor/EWebEditor/aspx/config.aspx";
            string configText = EWebEditorHelper.ReadFile(sFileName);
            string mapPath = Server.MapPath(sFileName);
            EWebEditorHelper.ModifyLicense(newLicense, configText, mapPath);

            if (sysSetting != null)
                ServiceFactory.OperateLogService.Save(
                    string.Format("修改系统配置，访问DNS【{0}】，是否打开日志【{1}】", sysSetting.DNS,
                        sysSetting.OpenLog));
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    public HandlerResult SaveSubjectSeo()
    {
        try
        { 
            int subjectId = RequestUtil.Instance.GetFormString("SubjectId").ToInt();
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveSubjectRes("subject", "SEO设置", subjectId, CurrentSiteId, subjectId));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            Subject subject = ServiceFactory.SubjectService.SingleOrDefault<Subject>(subjectId) ??
                              ModelFactory<Subject>.Insten();
            var type = typeof(Subject);
            subject = GetPostObject(type, subject) as Subject;
            ServiceFactory.SubjectService.Save(subject);
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }
}