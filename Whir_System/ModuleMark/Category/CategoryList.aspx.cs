/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：categorylist.aspx.cs
 * 文件描述：公用的类别列表页面
 */

using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;
using Whir.Repository;

public partial class whir_system_ModuleMark_category_categorylist : Whir.ezEIP.Web.SysManagePageBase
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
        
       
        contentManager1.ColumnId = ColumnId;
        contentManager1.SubjectId = SubjectId;
        contentManager1.SelectedType = SelectedType.CheckBox;


        contentManager1.EditPageUrl = "content_edit.aspx?columnid=" + ColumnId + "&subjectid=" + SubjectId + "&BackPageUrl=" + CurrentPageUrl;
        BindAttchColumnList();
    }

    //绑定附属栏目
    private void BindAttchColumnList()
    {

        var listColumn = ServiceFactory.ColumnService.GetMarkListByColumnId(ColumnId).Where(q => q.MarkType != "Category");

        AttchlistColumn = listColumn.ToList();

        //AttchlistColumn = listColumn.Where(p => p.MarkParentId != 0).ToList();

        IsShowAttchlist = IsRoleHaveColumnRes("类别管理");
        
    }
}