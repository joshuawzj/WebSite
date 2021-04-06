/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：backup.aspx.cs
 * 文件描述：备份页面
 */

using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using Whir.Service;
using Whir.Framework;
using Whir.Domain;
using Whir.Language;

public partial class Whir_System_Module_Extension_Backup : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 是否具有新建备份权限
    /// </summary>
    protected bool IsAdd { get; set; }

    /// <summary>
    /// 是否具有还原权限
    /// </summary>
    protected bool IsRestore { get; set; }

    /// <summary>
    /// 是否具有下载权限
    /// </summary>
    protected bool IsDownload { get; set; }

    /// <summary>
    /// 是否具有删除权限
    /// </summary>
    protected bool IsDelete { get; set; }

    /// <summary>
    /// LinkButton
    /// </summary>
    public LinkButton LbtnRes { get; set; }

    /// <summary>
    /// 下载的信息提示
    /// </summary>
    protected string DownloadMsg { get { return "下载的数据不存在".ToLang(); } }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("37"));
            BindData();
        }

    }

    #region 事件

    /// <summary>
    /// 行绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptBackup_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            Backup model = e.Item.DataItem as Backup;

            PlaceHolder lbtnDel = e.Item.FindControl("lbtnDel") as PlaceHolder;
            Literal litDownload = e.Item.FindControl("litDownload") as Literal;

            PlaceHolder lbtnRestore = e.Item.FindControl("lbtnRestore") as PlaceHolder;

            //#region 权限控制

            //if (null != lbtnDel)
            //{
            //    if (IsDelete)
            //        lbtnDel.Visible = true;
            //    else
            //        lbtnDel.Visible = false;
            //}

            //if (null != litDownload)
            //{
            //    if (IsDownload)
            //        litDownload.Visible = true;
            //    else
            //        litDownload.Visible = false;
            //}

            //if (null != lbtnRestore)
            //{
            //    if (IsRestore)
            //        lbtnRestore.Visible = true;
            //    else
            //        lbtnRestore.Visible = false;
            //}


            //#endregion 权限控制

            if (null == model || null == litDownload)
            {
                return;
            }

            EnumBackupMsg result = ServiceFactory.BackupService.DownloadData(AppName + model.BackupPath);

            if (result != EnumBackupMsg.DownloadSuccess)
            {
                litDownload.Text = "<a class='btn btn-info' href='javascript:;' name=\"aNon\">[{0}]</a>".FormatWith("下载".ToLang());
            }
            else
            {
                litDownload.Text = "<a class='btn btn-info' href='Download.aspx?path={0}&name={1}' target=\"_blank\">{2}</a>".FormatWith(
                    //由于备份路径已经是写死了 站点根目录/dbbackup/
                            AppName + "dbbackup/",
                               model.BackupName,
                               "下载".ToLang());
            }
        }
    }
    #endregion

    #region 绑定
    //绑定列表
    private void BindData()
    {
        var list = ServiceFactory.BackupService.Query<Backup>(" WHERE IsDel=0 ORDER BY CreateDate DESC ");
        rptBackup.DataSource = list;
        rptBackup.DataBind();

        ltNoRecord.Text = list.Any() ? "" : "找不到记录".ToLang();
    }

    #endregion

   
}