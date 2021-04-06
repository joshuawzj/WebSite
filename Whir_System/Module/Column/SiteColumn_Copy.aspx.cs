/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：sitecolumn_copy.aspx.cs
 * 文件描述：站点或栏目复制页面, 用于输入复制后的站点/栏目名称和目录, 确定后执行复制操作
 *          
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;
using Whir.Label;

public partial class Whir_System_Module_Column_SiteColumn_Copy : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 复制源栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 复制源站点ID
    /// </summary>
    protected int SiteId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgeOpenPagePermission(IsDevUser||IsSuperUser);
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SiteId = RequestUtil.Instance.GetQueryInt("siteid", 0);

        //复制栏目可不填写目标路径
        if (ColumnId != 0)
        {
            phColumnInfo.Visible = true;
        }
        else
        {
            phColumnInfo.Visible = false;
        }


        if (!IsPostBack)
        {
            BindColumn();
            BindSourceName();
        }
    }

    //绑定复制源名称
    private void BindSourceName()
    {
        if (ColumnId != 0)
        {
            //栏目
            Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
            if (column != null)
                litSource.Text = column.ColumnName;
        }
        else if (SiteId != 0)
        {
            //站点
            SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(SiteId);
            if (siteInfo != null)
                litSource.Text = siteInfo.SiteName;
        }
    }

    private void BindColumn()
    {
        //绑定站点
        //ddlSite.DataSource = ServiceFactory.SiteInfoService.GetList();
        //ddlSite.DataValueField = "SiteId";
        //ddlSite.DataTextField = "SiteName";
        //ddlSite.DataBind();
        //ddlSite.Items.Insert(0, new ListItem("==请选择==".ToLang(), ""));
        //ddlColumn.Items.Insert(0, new ListItem("==请选择==".ToLang(), ""));
    }


    //站点列表
    protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
    {
        //绑定栏目
        //try
        //{
        //    IList<Column> list = ServiceFactory.ColumnService.GetList(0, ddlSite.SelectedValue.ToInt());
        //    Column colNull = new Column();
        //    colNull.ColumnId = 0;
        //    colNull.ColumnName = "==请选择==".ToLang();
        //    list.Insert(0, colNull);
        //    ddlColumn.DataSource = list;
        //    ddlColumn.DataValueField = "ColumnId";
        //    ddlColumn.DataTextField = "ColumnName";
        //    ddlColumn.DataBind();
        //}
        //catch (ArgumentOutOfRangeException ex)
        //{ }
    }
}