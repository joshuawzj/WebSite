/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：SendEmailLog.aspx.cs
 * 文件描述：邮件群发发送记录页面
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class Whir_System_Plugin_Email_SendEmailLog : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ConfirmDelete(btnDel, "selectAction();");//--批量删除提示
            JudgePagePermission(IsCurrentRoleMenuRes("348"));
            BandList();
        }
    }

    /// <summary>
    /// 绑定列表
    /// </summary>
    protected void BandList()
    {
        var list = ServiceFactory.SendEmailRecordService.Query<SendEmailRecord>("WHERE IsDel=0 Order By CreateDate Desc");
        rpList.DataSource = list;
        rpList.DataBind();
        ltNoRecord.Text = list.Any() ? "" : "找不到记录".ToLang();
    }

    /// <summary>
    /// 分页事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageChanged(object sender, EventArgs e)
    {
        BandList();
    }

    /// <summary>
    /// 为rplist删除按钮添加删除确认对话框s
    /// </summary>
    protected void rpList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var lbDelete = e.Item.FindControl("lbDel") as LinkButton;
            if (null != lbDelete)
            {
                ConfirmDelete(lbDelete);
            }
        }
    }

    /// <summary>
    /// 单条记录删除
    /// </summary>
    protected void rpList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int id = e.CommandArgument.ToStr().ToInt();
        if (e.CommandName.Equals("del"))
        {
            #region 操作日志

            if (!IsCurrentRoleMenuRes("367"))
            {
                string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.warning('{0}')</script>".FormatWith(NoPermissionMsg.ToLang());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
            }
            else
            {
                SendEmailRecord model = ServiceFactory.SendEmailRecordService.SingleOrDefault<SendEmailRecord>(id);

                #endregion 操作日志
                //主键ID
                ServiceFactory.SendEmailRecordService.SaveLog(id, "delete");
                int count = ServiceFactory.SendEmailRecordService.Delete<SendEmailRecord>(id);
                if (count > 0)
                {
                    //记录操作日志
                    ServiceFactory.OperateLogService.Save("删除邮件，邮件标题【{0}】".FormatWith(model.Title));

                    string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.success('{0}', true, false)</script>".FormatWith("删除成功".ToLang());
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
                }
                else
                {
                    string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.error('{0}', true, false)</script>".FormatWith("删除失败".ToLang());
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
                }
            }
        }
        else if (e.CommandName.Equals("resend"))//重新发送
        {
            if (!IsCurrentRoleMenuRes("365"))
            {
                string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.warning('{0}')</script>".FormatWith(NoPermissionMsg.ToLang());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
            }
            else
            {
                int successNum = 0, failNum = 0;
                ServiceFactory.SendEmailRecordService.ResendEamil(id, out successNum, out failNum);
                string message = "已经发送，成功{0}条，失败{1}条".ToLang().FormatWith(successNum, failNum);

                string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.success('" + message + "', true, false)</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
            }

        }
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    protected void Link_Command(object sender, CommandEventArgs e)
    {
        string commadArgs = e.CommandArgument.ToStr();
        string ids = Request["cb_Position"].ToStr().Trim();
        if (!IsCurrentRoleMenuRes("367"))
        {
            string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.warning('{0}')</script>".FormatWith(NoPermissionMsg.ToLang());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
        }
        else
        {
            if (commadArgs == "del")
            {
                foreach (var id in ids.Split(','))
                {
                    #region 操作日志

                    SendEmailRecord model = ServiceFactory.SendEmailRecordService.SingleOrDefault<SendEmailRecord>(id.ToInt());

                    #endregion 操作日志

                    ServiceFactory.SendEmailRecordService.Delete<SendEmailRecord>(id.ToInt());

                    //记录操作日志
                    ServiceFactory.OperateLogService.Save("删除邮件，邮件标题【{0}】".FormatWith(model.Title));
                }

                string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.success('操作成功', true, false)</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
            }
        }
    }

    /// <summary>
    /// 转换发送方式
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public string ChangeCategory(int categoryId)
    {
        switch (categoryId)
        {
            case 0:
                return "按选择会员".ToLang();
                break;
            case 1:
                return "按会员类别".ToLang();
                break;
            case 2:
                return "指定邮箱".ToLang();
                break;
            default:
                return "未知".ToLang();
                break;
        }
    }

    /// <summary>
    /// 转换发送状态
    /// </summary>
    /// <param name="sendState"></param>
    /// <returns></returns>
    public string ChangeSendState(int sendState)
    {
        switch (sendState)
        {
            case 0:
                return "发送失败".ToLang();
                break;
            case 1:
                return "发送成功".ToLang();
                break;
            case 2:
                return "部分成功".ToLang();
                break;
            case 3:
                return "未发送".ToLang();
                break;
            default:
                return "未知".ToLang();
                break;
        }
    }

}