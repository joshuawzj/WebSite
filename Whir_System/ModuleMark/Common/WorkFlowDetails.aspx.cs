/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：workflowdetails.aspx.cs
 * 文件描述：通过信息查看
 */

using System;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Repository;
using Whir.ezEIP.Web;

public partial class whir_system_ModuleMark_jobs_workflowdetails : SysManagePageBase
{
    /// <summary>
    /// 当前的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 当前要编辑的主键ID
    /// </summary>
    protected int ItemID { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        ItemID = RequestUtil.Instance.GetQueryInt("itemid", 0);

        detailsForm1.ColumnId = detailsForm2.ColumnId = ColumnId;
        detailsForm1.ItemID = detailsForm2.ItemID = ItemID;


        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("375"));
            if (ItemID != 0)
            {
                PageMode = EnumPageMode.Update;
            }
        }
    }
}