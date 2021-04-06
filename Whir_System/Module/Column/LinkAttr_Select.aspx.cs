using System;
using System.Data;
using Whir.Service;
using Whir.Framework;
using Whir.Language;


public partial class Whir_System_Module_Column_LinkAttr_Select : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前的栏目
    /// </summary>
    protected int FieldId { get; set; }

    /// <summary>
    /// 子站点ID
    /// </summary>
    protected int FormId { get; set; }

    /// <summary>
    /// 回传到父页面的JS函数名
    /// </summary>
    protected string Callback { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgeOpenPagePermission(IsDevUser);
        FieldId = RequestUtil.Instance.GetQueryInt("FieldId", 0);
        FormId = RequestUtil.Instance.GetQueryInt("FormId", 0);
        DataTable table = null;
        if (FieldId > 0 && FormId == 0)
        {
            table = ServiceFactory.FormService.GetLinkAttrByFieldId(CurrentSiteId, FieldId);

        }
        else if (FormId > 0)
        {
            table = ServiceFactory.FormService.GetLinkAttrByFormId(CurrentSiteId, FormId);
        }
        rptLinkList.DataSource = table;
        rptLinkList.DataBind();
        if (table == null || table.Rows.Count == 0)
        {
            ltNodata.Text = "<li>{0}</li>".FormatWith("无数据".ToLang());
        }
    }
    
}