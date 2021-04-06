/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：member_memberlist.aspx.cs
 * 文件描述：会员列表*/

using System;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class Whir_System_ModuleMark_Member_MemberList : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性

    /// <summary>
    /// 当前的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 是否具有添加权限
    /// </summary>
    protected bool IsAdd { get; set; }

    /// <summary>
    /// 是否具有导出权限
    /// </summary>
    protected bool IsOutput { get; set; }

    /// <summary>
    /// 是否具有回收站权限
    /// </summary>
    protected bool IsRecycle { get; set; }

    /// <summary>
    /// 是否具有编辑权限
    /// </summary>
    protected bool IsEdit { get; set; }

    /// <summary>
    /// 是否具有删除权限
    /// </summary>
    protected bool IsDelete { get; set; }

    /// <summary>
    /// 是否具有排序权限
    /// </summary>
    protected bool IsSort { get; set; }
    /// <summary>
    /// 前当流程节点ID
    /// </summary>
    protected int CurrentActivityId { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        CurrentActivityId = RequestUtil.Instance.GetQueryInt("flowid", 0);

        IsAdd = IsCurrentRoleMenuRes("149");
        
        IsRecycle = IsCurrentRoleMenuRes("151");
        IsEdit = IsCurrentRoleMenuRes("152");
        IsDelete = IsCurrentRoleMenuRes("153");
        IsSort = IsCurrentRoleMenuRes("154");

        //固定1为会员栏目
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 1);

        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = false;
        contentManager1.IsShowDelete = IsDelete; //是否显示删除
        contentManager1.IsShowEdit = IsEdit; //是否显示编辑
        contentManager1.IsShowSort = IsSort;
        contentManager1.IsShowImport = true;
        contentManager1.IsShowExport = true;
        contentManager1.Where = workFlowBar1.GetWhereSql();
        contentManager1.EditPageUrl = "Member_edit.aspx?columnid=" + ColumnId + "&BackPageUrl=" + CurrentPageUrl;
    }
}