/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：recycle.aspx.cs
 * 文件描述：会员回收站*/

using System;
using Whir.Framework;
using Whir.Service;

public partial class Whir_System_ModuleMark_Member_Recycle : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前的主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("296"));

        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);

        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = true;
        contentManager1.IsShowEdit = false;
        contentManager1.IsShowDelete = true;
        contentManager1.IsCurrentPageSetting = true;
        
    }

  

}