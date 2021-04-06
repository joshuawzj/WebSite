/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：
 * 文件描述：
 */
using System;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;

public partial class Whir_System_Module_Extension_CollectStep2 : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 项目ID
    /// </summary>
    protected int CollectId { get; set; }

    protected Collect CurrentCollect { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("38"));
        CollectId = RequestUtil.Instance.GetQueryInt("collectid", 0);
        if (CollectId <= 0)
        {
            Response.Redirect("CollectList.aspx");
            Response.End();
        }
        if (!IsPostBack)
        {
            CurrentCollect = ServiceFactory.CollectService.SingleOrDefault<Collect>(CollectId) ?? ModelFactory<Collect>.Insten(); ;
            if (CurrentCollect == null)
            {
                Response.Redirect("CollectList.aspx");
                Response.End();
            }
            else
            {
                lblItemName.Text = CurrentCollect.ItemName;
                
            }
        }
    }
}