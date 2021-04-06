/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：column_link.aspx.cs
 * 文件描述：外部链接栏目的添加和编辑页面
 *          
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class Whir_System_Module_Column_Column_Link : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前编辑的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    protected IList<Column> Columns { get; set; }
    protected Column Column { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        if (!IsPostBack)
        {
            BindParentColumn();

            if (ColumnId != 0)
            {
                PageMode = EnumPageMode.Update;
                litProccess.Text = "编辑外部链接".ToLang();
            }
            else
            {
                litProccess.Text = "添加外部链接".ToLang();
            }
            ShowTitleAndButton();
        }
        BindColumnInfo();
    }

    /// <summary>
    /// 显示隐藏掉按钮
    /// </summary>
    private void ShowTitleAndButton()
    {

        string targetFlag = WebUtil.Instance.GetQueryString("flagtype");
        if (targetFlag != "")
        {
            div_submit.Visible = false;
            div_save.Visible = true;
        }
        else
        {
            div_submit.Visible = true;
            div_save.Visible = false;
        }
    }

    
    //绑定上级栏目
    private void BindParentColumn()
    {
        Columns = ServiceFactory.ColumnService.GetList(0, CurrentSiteId, 0, ColumnId);
    }

    //绑定要编辑的栏目信息
    private void BindColumnInfo()
    {
        Column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId) ?? ModelFactory<Column>.Insten();

    }

     
}