/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：multisite.aspx.cs
 * 文件描述：站点选项字段列表页面
 */
using System;

using Whir.Framework;
using Whir.Repository;
using Whir.Language;

public partial class Whir_System_Module_Setting_OptionField : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 站点ID
    /// </summary>
    protected int SiteId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        SiteId = RequestUtil.Instance.GetQueryInt("siteid", 0);
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            BindList();
        }
    }

    //绑定列表
    private void BindList()
    {
        string SQL = "SELECT col.ColumnName,col.ColumnId,field.fieldName,form.FieldAlias,opt.BindType from Whir_Dev_FormOption opt"
            + " INNER JOIN Whir_Dev_Form form ON opt.FormId=form.FormId"
            + " INNER JOIN Whir_Dev_Column col ON form.ColumnId=col.ColumnId"
            + " INNER JOIN Whir_Sit_SiteInfo site ON col.SiteId=site.SiteId"
            + " INNER JOIN Whir_Dev_Field field ON form.FieldID=field.FieldID"
            + " WHERE opt.BindType IN (1,2,3) AND site.SiteId=@0 and col.IsDel=0";
        var data = DbHelper.CurrentDb.Query(AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, SQL, SiteId);
        rpList.DataSource = data.Items;
        rpList.DataBind();
        AspNetPager1.RecordCount = data.TotalItems.ToInt();
        ltNoRecord.Text = data.TotalItems.ToInt() > 0 ? "" : "找不到记录".ToLang();
    }

    /// <summary>
    /// 分页方法
    /// </summary>
    protected void PageChanged(object sender, EventArgs e)
    {
        BindList();
    }

    /// <summary>
    /// 转换
    /// </summary>
    /// <param name="bindType"></param>
    /// <returns></returns>
    protected string Tran(string bindType)
    {
        switch (bindType)
        {
            case "1":
                return "绑定文本".ToLang();
                break;
            case "2":
                return "绑定SQL".ToLang();
                break;
            case "3":
                return "绑定多级类别".ToLang();
                break;
            default:
                return "未知".ToLang();
                break;
        }
    }
}