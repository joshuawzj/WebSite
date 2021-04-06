/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：contentlist.aspx.cs
 * 文件描述：类别管理
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class whir_system_ModuleMark_category_contentlist : SysManagePageBase
{
    #region 属性

    /// <summary>
    /// 当前的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }
    
    /// <summary>
    /// 当前的子栏目ID
    /// </summary>
    protected int SubjectId { get; set; }

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
    /// 绑定附属操作
    /// </summary>
    protected IList<Column> AttchlistColumn { get; set; }

    protected bool IsShowAttchlist { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        IsAdd = true;

        IsRecycle = false;
        IsEdit = IsCurrentRoleMenuRes("152");
        IsDelete = IsCurrentRoleMenuRes("153");
        IsSort = IsCurrentRoleMenuRes("154");
 

        contentManager1.ColumnId = ColumnId;
        contentManager1.SubjectId = SubjectId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = false;
   
        contentManager1.EditPageUrl = "content_edit.aspx?columnid=" + ColumnId + "&subjectid=" + SubjectId + "&BackPageUrl=" + CurrentPageUrl;
        BindAttchColumnList();
    }

    //绑定附属栏目
    private void BindAttchColumnList()
    {
        var listColumn = ServiceFactory.ColumnService.GetMarkListByColumnId(ColumnId);
        var mainColumn = listColumn.FirstOrDefault(p => p.MarkParentId == 0);
        if (mainColumn != null)
        {
            if (!mainColumn.IsCategory)
                AttchlistColumn = listColumn.Where(p => p.MarkParentId != 0 && p.MarkType != "Category").ToList();
            else
                AttchlistColumn = listColumn.Where(p => p.MarkParentId != 0).ToList();

            IsShowAttchlist = IsRoleHaveColumnRes("类别管理");
        }
    }
    
}