using System;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.ezEIP.Web;
using Whir.Language;

public partial class Whir_System_Handler_Pligin_VisitService : SysHandlerPageBase
{

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
        //从配置文件中获取信息
        var type = typeof(SiteInfo);
        var model = SiteInfoHelper.SiteInfo ?? ModelFactory<SiteInfo>.Insten();
        model = GetPostObject(type, model) as SiteInfo;
        ServiceFactory.SiteInfoService.Update(model);

        //操作日志
        ServiceFactory.OperateLogService.Save("修改在线客服管理代码，在线客服应用代码【{0}】，在线客服开启模式【{1}】".FormatWith(model.VisitServiceCode, model.VisitServiceMode));
        
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }
}