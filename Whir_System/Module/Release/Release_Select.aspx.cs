/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：release_select.aspx.cs
 * 文件描述：发布栏目
 */
using System;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.ezEIP.Web;

public partial class Whir_System_Module_Release_Release_Select : SysManagePageBase
{
    /// <summary>
    /// 栏目ID，负数表明是站点文件夹
    /// </summary>
    protected int ColumnId { get; set; }
    /// <summary>
    /// 菜单ID，0:内容，1：子站，2：专题
    /// </summary>
    protected int MenuId { get; set; }

    /// <summary>
    /// 类别ID
    /// </summary>
    protected int ClassId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = WebUtil.Instance.GetQueryInt("columnid", 0);
        MenuId = WebUtil.Instance.GetQueryInt("menuid", 0);
        ClassId = WebUtil.Instance.GetQueryInt("classid", 0);
        JudgeOpenPagePermission(new SysManagePageBase().IsCurrentRoleMenuRes("363"));
    }

}