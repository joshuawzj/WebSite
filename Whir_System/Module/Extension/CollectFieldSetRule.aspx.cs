/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：
 * 文件描述：
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;

public partial class Whir_System_Module_Extension_CollectFieldSetRule : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 项目ID
    /// </summary>
    protected int CollectId { get; set; }

    /// <summary>
    /// 表单ID
    /// </summary>
    protected int FormId { get; set; }

    /// <summary>
    /// 采集实体
    /// </summary>
    protected Collect CollectModel { get; set; }

    protected CollectField CurrenField { get; set; }

    protected string Link { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

        CollectId = RequestUtil.Instance.GetQueryInt("collectid", 0);
        FormId = RequestUtil.Instance.GetQueryInt("formid", 0);
        if (CollectId <= 0 || FormId <= 0)
        {
            Error();
            return;
        }
        CollectModel = ServiceFactory.CollectService.SingleOrDefault<Collect>(CollectId);
        if (CollectModel == null)
        {
            Error();
            return;
        }
        if (!IsPostBack)
        {
            CurrenField = ServiceFactory.CollectFieldService.Query<CollectField>("SELECT * FROM Whir_Ext_CollectField WHERE FormId=@0 AND CollectId=@1", FormId, CollectId).FirstOrDefault() ?? ModelFactory<CollectField>.Insten();
            
            string pageHtml = HttpOperater.GetHtml(CollectModel.WebUrl);
            int startIndex = pageHtml.IndexOf(CollectModel.ListStartCode.Replace("\r",""), StringComparison.Ordinal);
            if (startIndex != -1)
            {
                pageHtml = pageHtml.Substring(startIndex + CollectModel.ListStartCode.Replace("\r", "").Length);
                int endIndex = pageHtml.IndexOf(CollectModel.ListEndCode.Replace("\r", ""), StringComparison.Ordinal);
                if (endIndex != -1)
                {
                    pageHtml = pageHtml.Substring(0, endIndex);
                }
            }

            Link = HttpOperater.GetLinkList(pageHtml, CollectModel.LinkStartCode.Replace("\r", ""), CollectModel.LinkEndCode.Replace("\r", ""), CollectModel.WebUrl).FirstOrDefault();
        }
    }


    protected void Error()
    {
        AlertScript("未找到对应信息", "setTimeout('frameElement.api.close();',2000);");
    }
}