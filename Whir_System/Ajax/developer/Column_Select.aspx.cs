/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：column_select.aspx.cs
 * 文件描述：栏目选择页面
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;
using Whir.Language;

public partial class Whir_System_Ajax_Developer_Column_Select : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前的栏目
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 子站点ID
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 回传到父页面的JS函数名
    /// </summary>
    protected string Callback { get; set; }

    /// <summary>
    /// 选择类型
    /// </summary>
    protected SelectedType SelectedType { get; set; }


    #endregion 

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        Callback = RequestUtil.Instance.GetQueryString("callback");

        SelectedType = RequestUtil.Instance.GetQueryString("selectedtype").ToLower() == "checkbox"
                       ? SelectedType.CheckBox
                       : SelectedType.RadioBox;

        if (!IsPostBack)
        {
            BindSiteTab();
        }
    }

    //绑定站点群选项卡
    private void BindSiteTab()
    {
        var listSiteInfo = ServiceFactory.SiteInfoService.GetList();
        rptMultiSite.DataSource = listSiteInfo;
        rptMultiSite.DataBind();

        rptMultiSiteColumn.DataSource = listSiteInfo;
        rptMultiSiteColumn.DataBind();
    }
}