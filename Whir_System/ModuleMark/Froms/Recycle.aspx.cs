/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：recycle.aspx.cs
* 文件描述：添加表单回收站页。 
*/

using System;
using Whir.Framework;
using Whir.Service;

public partial class Whir_System_ModuleMark_Froms_Recycle : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前的主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 所属子站的ID
    /// </summary>
    protected int SubjectId { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        contentManager1.ColumnId = ColumnId;
        contentManager1.SubjectId = SubjectId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = true;
        contentManager1.IsShowEdit = false;
        contentManager1.IsShowDelete = true;
        contentManager1.IsCurrentPageSetting = true;
    }
}