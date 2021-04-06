/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：content_edit.aspx.cs
 * 文件描述：人才招聘添加修改
 */

using System;
using Whir.ezEIP.Web;
using Whir.Framework;

public partial class Whir_System_ModuleMark_Jobs_Content_Edit : SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 当前要编辑的主键ID
    /// </summary>
    protected int ItemId { get; set; }

    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        ItemId = RequestUtil.Instance.GetQueryInt("itemid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        dynamicForm1.ColumnId =  ColumnId;
        dynamicForm1.ItemId = ItemId;
        dynamicForm1.SubjectId = SubjectId;
    }
}