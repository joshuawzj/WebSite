using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;

public partial class Whir_System_Handler_Developer_Language : SysHandlerPageBase
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
        var cn = RequestUtil.Instance.GetString("cn");
        var hkText = RequestUtil.Instance.GetString("hkText");
        var enText = RequestUtil.Instance.GetString("enText");

        LanguageHelper.SetLanguage(cn, hkText, enText);

        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 获取分页数据
    /// </summary>
    public void GetList()
    {
        //test();
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int currentRolesId = Whir.ezEIP.BasePage.RequestInt32("rolesid", 0);
        var key = RequestUtil.Instance.GetString("search").ToLower();

        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser, true);

        var list = LanguageHelper.GetLanguageList();
        if (key.IsNotEmpty())
        {
            list = list.Where(p => p.CN.ToLower().Contains(key) || p.EnText.ToLower().Contains(key) || p.HkText.ToLower().Contains(key)).ToList();
        }

        long total = list.Count;

        int count = (pageIndex) * pageSize > list.Count ? list.Count - (pageIndex - 1) * pageSize : pageSize;

        string data = list.ToList().GetRange(pageSize * (pageIndex - 1), count).ToJson();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Delete()
    {

        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var cn = RequestUtil.Instance.GetString("cn");


        LanguageHelper.DeleteLanguage(cn);

        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }


}