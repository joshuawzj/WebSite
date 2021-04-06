using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using System.Collections.Generic;
using System.Text;

public partial class whir_system_Handler_Developer_SiteMap : SysHandlerPageBase
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
    public HandlerResult GetData()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var columnService = ServiceFactory.ColumnService;
        var strBuild = new StringBuilder();
        var strDynamicBuild = new StringBuilder();
        var typeB = RequestUtil.Instance.GetFormString("rblType").ToInt(0) == 0;
        IList<Column> list = columnService.Query<Column>("WHERE IsDel=0 AND ParentId=@0 AND SiteId=@1 AND SiteType=@2 AND (MarkType='' OR MarkType IS NULL) ORDER BY Sort ASC,UpdateDate DESC", 0, CurrentSiteId, 0).ToList();

        strBuild.Append("<ul class=\"ul_sitemap\">");
        strDynamicBuild.Append("<ul class=\"ul_sitemap\">");

        foreach (var column in list)
        {
            string strLi = "<li><strong><a href=\"{0}\" target=\"_blank\" >{1}</a></strong>{2}</li>\n";
            strBuild.AppendFormat(strLi, PreviewColumnUrl(column, false), typeB ? column.ColumnName : column.ColumnNameStage, GetChildColumn(column.ColumnId, false));
            strDynamicBuild.AppendFormat(strLi
                , PreviewColumnUrl(column, true)
                , typeB ? column.ColumnName : column.ColumnNameStage
                , GetChildColumn(column.ColumnId, true)
                );
        }
        strBuild.Append("</ul>");
        strDynamicBuild.Append("</ul>");
 
        return new HandlerResult
        {
            Status = true,
            Message = strBuild.ToStr() + "|@|" + strDynamicBuild.ToStr()
        };
    }


    /// <summary>
    /// 根据父栏目Id，找子栏目
    /// </summary>
    /// <param name="parentid"></param>
    /// <param name="isDynamic">是否动态</param>
    /// <returns></returns>
    private string GetChildColumn(int parentid, bool isDynamic)
    {
        var sbTemp = new StringBuilder();
        var columnService = ServiceFactory.ColumnService;
        var typeB = RequestUtil.Instance.GetFormString("rblType").ToInt(0) == 0;
        IList<Column> columns = columnService.Query<Column>("WHERE IsDel=0 AND ParentId=@0 AND SiteId=@1 AND SiteType=@2 AND (MarkType='' OR MarkType IS NULL) ORDER BY Sort ASC,UpdateDate DESC", parentid, CurrentSiteId, 0).ToList();

        if (columns.Count > 0)
        {
            sbTemp.Append("\n<span>\n");

            for (int i = 0; i < columns.Count; i++)
            {
                if (i == columns.Count - 1)
                {
                    sbTemp.AppendFormat("   <a href=\"{0}\" target=\"_blank\">{1}</a>\n", PreviewColumnUrl(columns[i], isDynamic), typeB ? columns[i].ColumnName : columns[i].ColumnNameStage);
                }
                else
                {
                    sbTemp.AppendFormat("   <a href=\"{0}\" target=\"_blank\">{1}</a> | \n", PreviewColumnUrl(columns[i], isDynamic), typeB ? columns[i].ColumnName : columns[i].ColumnNameStage);
                }
            }
            sbTemp.Append("</span>");
        }
        return sbTemp.ToStr();
    }


    /// <summary>
    /// 获取栏目的列表页的地址
    /// </summary>
    /// <param name="model"></param>
    /// <param name="isDynamic">是否生成动态</param>
    /// <returns></returns>
    private string PreviewColumnUrl(Column model, bool isDynamic)
    {
        string previewUrl = "javascript:void(0);";

        if (model.CreateMode == 1)
        {
            previewUrl = CheckTemple(model, previewUrl, false, isDynamic);

        }
        else if (model.CreateMode == 2)
        {
            previewUrl = CheckTemple(model, previewUrl, true, isDynamic);
        }

        return previewUrl;
    }

    /// <summary>
    /// 判断当前栏目绑定的模板
    /// </summary>
    /// <param name="column"></param>
    /// <param name="previewUrl">预览Url</param>
    /// <param name="isAspx">是否为aspx页面</param>
    /// <param name="isDynamic"></param>
    /// <returns></returns>
    private string CheckTemple(Column column, string previewUrl, bool isAspx, bool isDynamic)
    {
        if (previewUrl == null) throw new ArgumentNullException("previewUrl");
        if (!string.IsNullOrEmpty(column.DefaultTemp))
        {
            previewUrl = isDynamic ? "<%=Whir.Service.ServiceFactory.ColumnService.GetColumnListLink({0},{1},1)%>".FormatWith(column.ColumnId, isAspx ? "true" : "false")
                : ServiceFactory.ColumnService.GetColumnListLink(column.ColumnId, isAspx, 1);
        }
        else if (!string.IsNullOrEmpty(column.ListTemp))
        {
            previewUrl = isDynamic ? "<%=Whir.Service.ServiceFactory.ColumnService.GetColumnListLink({0},{1},2)%>".FormatWith(column.ColumnId, isAspx ? "true" : "false")
               : ServiceFactory.ColumnService.GetColumnListLink(column.ColumnId, isAspx, 2);
        }
        else
        {
            previewUrl = isDynamic ? "<%=Whir.Service.ServiceFactory.ColumnService.GetColumnListLink({0},{1},3)%>".FormatWith(column.ColumnId, isAspx ? "true" : "false")
              : ServiceFactory.ColumnService.GetColumnListLink(column.ColumnId, isAspx, 3);
        }
        return previewUrl;
    }
}