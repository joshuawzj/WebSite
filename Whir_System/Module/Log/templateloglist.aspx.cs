/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：templateloglist.aspx.cs
 * 文件描述：日志操作列表页面
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class Whir_System_Module_log_templateloglist : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 日志操作类型,0:网络操作日志，1：模版操作日志，2：系统运行日志
    /// </summary>
    public int OperateType = 1;

    /// <summary>
    /// 是否删除
    /// </summary>
    protected bool IsDelete { get; set; }

    /// <summary>
    /// 是否清空
    /// </summary>
    protected bool IsClear { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            LoadLogList();
            ConfirmDelete(lbDel, "selectAction();");
            ConfirmClear(lbClear);
        }
    }
    /// <summary>
    /// 为清空按钮添加确认清空的提示框
    /// </summary>
    /// <param name="linkButton">对应的按钮</param>
    public void ConfirmClear(LinkButton linkButton)
    {
        if (linkButton == null) return;
        string callback = "__doPostBack('{0}','')".FormatWith(linkButton.UniqueID);
        //linkButton.Attributes.Add("onclick", "return whir.dialog.confirm('"+"确认要清空日志吗？".ToLang()+"', function(){ " + callback + " })");

        linkButton.Attributes.Add("confirmText", "确认要清空日志吗？".ToLang());
        linkButton.Attributes.Add("class", "del_btn");
        linkButton.Attributes.Add("confirmId", "lbDelete" + linkButton.ClientID);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadLogList();
    }
    /// <summary>
    /// 绑定日志列表
    /// </summary>
    private void LoadLogList()
    {
        //string keyword = txtKeyword.Text.Trim();
        //string beginDate = dpBegin.Text;
        //string endDate = dpEnd.Text;

        //int recordCount = 0;
        //IList<OperateLog> list = new OperateLogService().GetOperationLogListByPager(keyword, beginDate, endDate, OperateType, pagerLog.CurrentPageIndex, pagerLog.PageSize,
        //                                                                     out recordCount);
        string Sql = " Where Type =@0 ";
        if (!IsDevUser)
        {
            string DevUserStr = Whir.Security.ServiceFactory.UsersService.GetUserStrByRoleId(1);//开发者
            if (DevUserStr != "")
            {
                Sql += " AND CreateUser NOT IN({0}) ".FormatWith(DevUserStr);
            }
        }
        Sql += "Order By Sort desc";
        var data = ServiceFactory.OperateLogService.Page(AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize,
                                                        Sql, OperateType);
        rptOperationLogs.DataSource = data.Items;
        rptOperationLogs.DataBind();
        //更新记录数
        AspNetPager1.RecordCount = data.TotalItems.ToInt();

        ltNoRecord.Text = data.TotalItems.ToInt() > 0 ? "" : "无数据".ToLang();
    }
    /// <summary>
    /// 分页方法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageChanged(object sender, EventArgs e)
    {
        LoadLogList();
    }
    /// <summary>
    /// Repeater行绑定
    /// </summary>
    protected void rptOperationLogs_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var lbDelete = e.Item.FindControl("lbDelete") as LinkButton;
            if (null != lbDelete)
            {
                ConfirmDelete(lbDelete);
            }
        }
    }
    /// <summary>
    /// Repeater行命令
    /// </summary>
    protected void rptOperationLogs_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("del"))
        {
            int OpreateId = e.CommandArgument.ToInt();
            bool IsSuccess = new OperateLogService().Delete(OpreateId);
            if (IsSuccess)
            {
                Alert("删除成功".ToLang(), true);
                LoadLogList();
            }
        }
    }
    /// <summary>
    /// 列表批量操作动作
    /// </summary>
    protected void Operate_Commad(object sender, CommandEventArgs e)
    {
        string commadArgs = e.CommandArgument.ToStr();
        string ids = Request["cb_Position"].ToStr().Trim();
        switch (commadArgs)
        {
            case "del"://批量删除
                foreach (string id in ids.Split(','))
                {
                    ServiceFactory.OperateLogService.Delete(id.ToInt());
                }
                Alert("操作成功".ToLang(), true);
                break;
            case "clear"://清除日志
                ServiceFactory.OperateLogService.DeleteByType(OperateType);
                Alert("操作成功".ToLang(), true);
                break;
        }
        LoadLogList();
    }
}
