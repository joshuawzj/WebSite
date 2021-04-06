/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：WorkFlowLogs.ascx.cs
* 文件描述：工作流操作日志公共控件
*/

using System;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Service;

public partial class whir_system_UserControl_ContentControl_WorkFlowLogs : SysControlBase
{
    #region 对外属性

    /// <summary>
    ///记录ID
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// 栏目ID.通过URL参数接收
    /// </summary>
    public int ColumnId { get; set; }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Column model = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
            int WorkFlowId = model != null ? model.WorkFlow : 0;

            if (WorkFlowId <= 0)
            {
                phShowLogs.Visible = false;
            }
            else
            {
                rptList.DataSource = ServiceFactory.WorkFlowLogsService.GetItemList(ColumnId, ItemId);
                rptList.DataBind();
            }
        }
    }
}