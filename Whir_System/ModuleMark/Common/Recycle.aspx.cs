/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：recycle.aspx.cs
 * 文件描述：回收站页面
 */

using System;
using Whir.Framework;
using Whir.Service;

public partial class Whir_System_ModuleMark_Common_Recycle : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前的主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        JudgePagePermission(IsRoleHaveColumnRes("回收站", ColumnId, SubjectId == 0 ? -1 : SubjectId));
        contentManager1.ColumnId = ColumnId;
        contentManager1.SubjectId = SubjectId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = true;
        contentManager1.IsShowEdit = false;
        contentManager1.IsShowDelete = true;
        contentManager1.IsCurrentPageSetting = true;

    }
}