/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：categorylist.aspx.cs
 * 文件描述：产品展示类型
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.ezEIP.Web;
using Whir.Language;

public partial class whir_system_ModuleMark_product_categorylist : SysManagePageBase
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
    /// 绑定附属操作
    /// </summary>
    protected IList<Column> AttchlistColumn { get; set; }

    protected bool IsShowAttchlist { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

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