/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：content_edit.aspx.cs
* 文件描述：添加提交表单页面。 
*/

using System;
using Whir.Framework;

public partial class Whir_System_ModuleMark_Froms_Content_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 当前要编辑的主键ID
    /// </summary>
    protected int ItemID { get; set; }

    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        ItemID = RequestUtil.Instance.GetQueryInt("itemid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        dynamicForm1.ColumnId = ColumnId;
        dynamicForm1.ItemId =  ItemID;
        dynamicForm1.SubjectId = SubjectId;
    }
}