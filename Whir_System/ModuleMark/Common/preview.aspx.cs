
/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：preview.aspx.cs
 * 文件描述：信息内容预览页面
 */

using System;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Domain;
using Whir.Label;
using Whir.Service;

public partial class whir_system_ModuleMark_common_preview : SysManagePageBase
{
    /// <summary>
    /// 要跳转的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }
    /// <summary>
    /// 要预览的itemID
    /// </summary>
    protected int ItemId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = WebUtil.Instance.GetQueryInt("columnid", 0);
        ItemId = WebUtil.Instance.GetQueryInt("ItemId", 0);

        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
        if (column != null)
        {
            SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(column.SiteId);
            if (siteInfo != null)
            {
                string previewPath = LabelHelper.Instance.BuildPreview(siteInfo, column);
                if (ItemId > 0)
                    previewPath += "?ItemId=" + ItemId;
                Response.Redirect(previewPath);
            }
        }
    }
}