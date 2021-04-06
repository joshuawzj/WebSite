/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：history_bak_details.aspx.cs
 * 文件描述：历史备份文件详细页
 */

using System;

using Whir.Framework;
using Whir.Service;
using Whir.ezEIP.Web;

public partial class whir_system_ModuleMark_Common_HistoryBakDetail : SysManagePageBase
{
    /// <summary>
    /// 当前的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 子站id
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 当前要编辑的主键ID
    /// </summary>
    protected int ItemId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        ItemId = RequestUtil.Instance.GetQueryInt("itemid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("SubjectId", 0);

        detailsForm1.ColumnId = detailsForm2.ColumnId = ColumnId;
        detailsForm1.ItemId = detailsForm2.ItemId = ItemId;


        if (!IsPostBack)
        {
            JudgeOpenPagePermission(IsRoleHaveColumnRes("历史记录", ColumnId, SubjectId == 0 ? -1 : SubjectId));
            if (ItemId != 0)
            {
                PageMode = EnumPageMode.Update;
            }
        }
    }
}