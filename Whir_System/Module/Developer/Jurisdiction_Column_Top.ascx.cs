
/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：Jurisdiction_column_top.aspx.cs
 * 文件描述：开发者分配栏目权限给客户文件的公共头部
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using Whir.Service;
using Whir.Framework;
using Whir.Domain;

public partial class whir_system_module_developer_Jurisdiction_column_top : System.Web.UI.UserControl
{
    /// <summary>
    /// 当前站点
    /// </summary>
    protected int SiteId { get; set; }
    /// <summary>
    /// 类型（0=站点栏目 1=子站 2=专题）
    /// </summary>
    protected int SelectType { get; set; }

    /// <summary>
    /// 子站、专题的类型 数据
    /// </summary>
    protected List<SubjectClass> ListClass = new List<SubjectClass>();

    /// <summary>
    /// 子站、专题的  数据
    /// </summary>
    protected List<Subject> ListSubject = new List<Subject>();

    protected void Page_Load(object sender, EventArgs e)
    {

        SiteId = RequestUtil.Instance.GetQueryInt("siteid", -1);
        SelectType = RequestUtil.Instance.GetQueryInt("type", -1);

        GetMultiSite();
        if (SelectType > 0)
            GetData();

    }

    private void GetData()
    {
        if (SelectType == 1)
            ListClass = ServiceFactory.SubjectClassService.GetSubsiteClassList(SiteId).ToList();
        else
            ListClass = ServiceFactory.SubjectClassService.GetSubjectClassList(SiteId).ToList();

        ListSubject = ServiceFactory.SubjectService.GetListSubsiteBySiteId(SiteId).ToList();
    }

    /// <summary>
    /// 绑定多站点
    /// </summary>
    private void GetMultiSite()
    {
        rptList.DataSource = ServiceFactory.SiteInfoService.GetList();
        rptList.DataBind();
    }
}