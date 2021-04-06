
/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：viewPage.aspx.cs
 * 文件描述：跳转到预览页面的媒介页面
 */

using System;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.ezEIP.Web;
using Whir.Language;
using System.IO;

public partial class Whir_System_ModuleMark_Common_ViewPage : SysManagePageBase
{
    /// <summary>
    /// 要跳转的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    protected int SubjectId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = WebUtil.Instance.GetQueryInt("columnid", 0);
        SubjectId = WebUtil.Instance.GetQueryInt("subjectid", 0);
        Column column = new Column();
        if (ColumnId >= 0)
        {
            column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId)??new Column();
            if (column.DefaultTemp.IsEmpty() && column.ListTemp.IsEmpty())
            {
                litRedirect.Text = "没有绑定模板".ToLang();
                return;
            }
        }
        else
        {
            column = ServiceFactory.ColumnService.GetSubIndexBySubjectId(SubjectId, CurrentSiteId);
        }
        if (column == null)
        {
            litRedirect.Text = "未找到对应栏目".ToLang();
            return;
        }
        string param = string.Empty;//专题、模版子站需要带subjectId
        if ((ColumnId  == -1) || (column.SiteType > 0 && !column.IsCustomSubsite && SubjectId > 0))//专题或模版子站
        {
            param = "subjectid=" + SubjectId;
        }
        switch (column.CreateMode)
        {
            case 1:
                string linkurl = Whir.Service.ServiceFactory.ColumnService.GetColumnListLink(column.ColumnId, param, 0);
                Response.Redirect(linkurl);
                break;
            case 2:
                linkurl = Whir.Service.ServiceFactory.ColumnService.GetColumnListLink(column.ColumnId, param, 0);

                string aspxPath = param.IsEmpty() ? linkurl : linkurl.Replace("?" + param, "");
                if (File.Exists(MapPath(aspxPath)))
                    Response.Redirect(linkurl);
                else
                    litRedirect.Text = "文件尚未发布成功或已经删除，若要预览文件请先发布".ToLang();
                break;

            case 0:
                litRedirect.Text = "如需预览请先修改生成模式".ToLang();
                break;
            default:
                litRedirect.Text = "正在跳转页面...".ToLang();
                break;
        }
    }
}