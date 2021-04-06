/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：sensitivewordlist.aspx.cs
 * 文件描述：敏感词列表页面
 *
 *          1.绑定数据
 *          2.批量删除操作
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Wuqi.Webdiyer;
using Whir.Language;

public partial class Whir_System_Module_Extension_SensitiveWord_List : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性

    /// <summary>
    /// 是否具有添加敏感词权限
    /// </summary>
    protected bool IsAdd { get; set; }

    /// <summary>
    /// 是否具有编辑权限
    /// </summary>
    protected bool IsEdit { get; set; }

    /// <summary>
    /// 是否具有删除权限
    /// </summary>
    protected bool IsDelete { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //敏感词（添加敏感词、修改、删除）
        IsAdd = IsCurrentRoleMenuRes("178");
        IsEdit = IsCurrentRoleMenuRes("179");
        IsDelete = IsCurrentRoleMenuRes("180");

        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("35"));
            BindList();
        }
    }

    #region 绑定
    //绑定列表
    private void BindList()
    {
        var list = ServiceFactory.SensitiveWordService.Query<SensitiveWord>(" WHERE IsDel=0 ORDER BY CreateDate DESC ");
        rptSensitiveWordList.DataSource = list;
        rptSensitiveWordList.DataBind();

        ltNoRecord.Text = list.Any() ? "" : "找不到记录".ToLang();
    }

    /// <summary>
    /// 分页方法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageChanged(object sender, EventArgs e)
    {
        BindList();
    }
    #endregion

    #region 事件
    //行事件
    protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string commandArgs = e.CommandArgument.ToStr();
        if (!IsCurrentRoleMenuRes("180"))
        {
            string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.warning('{0}')</script>".FormatWith(NoPermissionMsg.ToLang());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
        }
        else
        {
            //删除
            if (e.CommandName.Equals("del"))
            {
                SensitiveWord model = ServiceFactory.SensitiveWordService.SingleOrDefault<SensitiveWord>(e.CommandArgument.ToInt());
                bool isSuccess = ServiceFactory.SensitiveWordService.Delete<SensitiveWord>(e.CommandArgument.ToInt()) > 0 ? true : false;
                if (isSuccess)
                {
                    //记录操作日志
                    ServiceFactory.SensitiveWordService.SaveLog(model, "delete");
                    SimpleAlert("删除成功".ToLang(),true);
                    BindList();
                }
            }
        }
    }

    //行绑定时
    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            SensitiveWord model = e.Item.DataItem as SensitiveWord;
            LinkButton lbtnDel = e.Item.FindControl("lbtnDel") as LinkButton;

            if (lbtnDel != null)
            {
                ConfirmDelete(lbtnDel);
            }
        }
    }
    #endregion
}