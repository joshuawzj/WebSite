/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：member_edit.aspx.cs
 * 文件描述：会员组列表
 */

using System;

using Whir.Framework;

public partial class Whir_System_ModuleMark_Member_Member_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 当前要编辑的主键ID
    /// </summary>
    protected int ItemId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        ItemId = RequestUtil.Instance.GetQueryInt("itemid", 0);

        dynamicForm1.ColumnId = ColumnId;
        dynamicForm1.ItemId = ItemId;

       
    }
    
}