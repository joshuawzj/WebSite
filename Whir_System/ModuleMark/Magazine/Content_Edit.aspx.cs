/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：content_edit.aspx.cs
* 文件描述：电子期刊编辑页面。 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Repository;
using Whir.Service;

public partial class Whir_System_ModuleMark_Magazine_Content_Edit : Whir.ezEIP.Web.SysManagePageBase
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


    /// <summary>
    /// 是否有历史记录
    /// </summary>
    protected bool IsHavaHistory { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        ItemID = RequestUtil.Instance.GetQueryInt("itemid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        dynamicForm1.ColumnId = ColumnId;
        dynamicForm1.ItemId = ItemID;
        dynamicForm1.SubjectId = SubjectId;
    }
}