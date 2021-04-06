/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：backup_edit.aspx.cs
 * 文件描述：新建备份页面
 */

using System;
using Whir.Service;
using Whir.Framework;
using Whir.Language;

public partial class Whir_System_Module_Extension_Backup_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 是否显示，用于MySQL数据库
    /// </summary>
    protected string IsMySql { get; set; }

    /// <summary>
    /// 是否显示，用于Oracle数据库
    /// </summary>
    protected string IsOracle { get; set; }

    /// <summary>
    /// 备份备注显示的样式宽度
    /// </summary>
    protected int DisplayWidth { get; set; }

    /// <summary>
    /// 是否显示隐藏的提示信息
    /// </summary>
    protected string IsShow { get; set; }

    /// <summary>
    /// 提示的信息
    /// </summary>
    protected string Msg { get; set; }

    /// <summary>
    /// 参数
    /// </summary>
    protected string CmdArgs { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("333"));

        //如果不是SQLServer就不允许备份
        if (CurrentDbType.CurDbType != EnumType.DbType.SqlServer)
        {
            IsShow = "true";

            Msg = "当前使用的是{0}数据库，不支持备份，请直接到数据库管理系统中备份".ToLang().FormatWith(CurrentDbType.CurDbType.ToStr());
            return;
        }

       

        //数据库服务器主机
        string dbServerHost = null;
        string webServerHost = Request.UserHostAddress;

        if (CurrentDbType.CurDbType != EnumType.DbType.SqlServer)
        {
            //首先判断当前Web服务器是否和SqlServer数据库服务器同一个主机
            if (!ServiceFactory.BackupService.IsSameHost(out dbServerHost))
            {
                //不同主机
                IsShow = "true";
                Msg = string.Format("当前使用的是{2}数据库，不支持远程数据库备份，请到服务器上进行备份！Web服务器主机：{0}，数据库服务器主机：{1}。".ToLang(), webServerHost, dbServerHost, CurrentDbType.CurDbType.ToStr());
                return;
            }
        }

        IsShow = "false";
        Msg = "";

        //是否显示MySQL安装目录的行
        if (CurrentUseDbType.Equals(EnumType.DbType.MySql))
        {
            IsMySql = "";
        }
        else
        {
            IsMySql = "none";
        }

        //是否显示Oracle的备份提示
        if (CurrentUseDbType.Equals(EnumType.DbType.Oracle))
        {
            IsOracle = "";
        }
        else
        {
            IsOracle = "none";
        }

        //设置备份描述的单元格 宽度
        if (CurrentUseDbType.Equals(EnumType.DbType.MySql))
        {
            DisplayWidth = 120;
        }
        else
        {
            DisplayWidth = 60;
        }
    }

   
}