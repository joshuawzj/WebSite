using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Service;
public partial class Whir_System_Handler_Extension_SensitiveWord : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetFormString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        int wordId = RequestUtil.Instance.GetFormString("SensitiveWordId").ToInt();
        var handlerResult = new HandlerResult();
        if (wordId > 0)
        {
            SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("347"));
        }
        else
        {
            SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("346"));
        }
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //反射获取表单字段数据
        var type = typeof(SensitiveWord);
        var word = ServiceFactory.SiteInfoService.SingleOrDefault<SensitiveWord>(wordId) ?? ModelFactory<SensitiveWord>.Insten();
        try
        {
            word = GetPostObject(type, word) as SensitiveWord;
            ServiceFactory.SiteInfoService.Save(word);
            if (word != null && word.SensitiveWordId > 0)
            {
                //记录操作日志
                ServiceFactory.SensitiveWordService.SaveLog(word, "update");
            }
            else
            {
                ServiceFactory.SensitiveWordService.SaveLog(word, "insert");
            }
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "保存失败".ToLang() };
        }
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Del()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("180"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int wordId = RequestUtil.Instance.GetFormString("SensitiveWordId").ToInt();
        var word = ServiceFactory.SiteInfoService.SingleOrDefault<SensitiveWord>(wordId);
        if (word == null)
            return new HandlerResult { Status = false, Message = "要删除的菜单数据不存在".ToLang() };

        ServiceFactory.SiteInfoService.Delete(word);
        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
    }
}