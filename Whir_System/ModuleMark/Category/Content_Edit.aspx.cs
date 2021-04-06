/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：content_edit.aspx.cs
 * 文件描述：类别添加修改
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Repository;
using Whir.Service;

public partial class whir_system_ModuleMark_category_content_edit : Whir.ezEIP.Web.SysManagePageBase
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
    /// 子站和专题ID
    /// </summary>
    protected int SubjectId { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        ItemID = RequestUtil.Instance.GetQueryInt("itemid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        dynamicForm1.ColumnId =  ColumnId;
        dynamicForm1.ItemId = ItemID;
        dynamicForm1.SubjectId =  SubjectId;

        if (!IsPostBack)
        {
            
        }
    }

     
}