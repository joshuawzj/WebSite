using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Whir.Service;
using Whir.Framework;
using Whir.Domain;
using Whir.Language;

public partial class Whir_System_UserControl_DeveloperControl_GetColumn : Whir.ezEIP.Web.SysControlBase
{
    #region 属性
    /// <summary>
    /// 站点ID
    /// </summary>
    public int SiteId { get; set; }

    /// <summary>
    /// 栏目ID
    /// </summary>
    public int ColumnId { get; set; }

    protected IList<SiteInfo> Sitelist { get; set; }

    protected IList<Column> Columnlist { get; set; }

    #endregion 属性

    protected void Page_Load(object sender, EventArgs e)
    {
        BindSite();
        BindColumnById();
    }

    /// <summary>
    /// 绑定站点
    /// </summary>
    private void BindSite()
    {
        Sitelist = (IList<SiteInfo>)ServiceFactory.SiteInfoService.GetList();
    }

    /// <summary>
    /// 根据ColumnId来绑定
    /// </summary>
    private void BindColumnById()
    {
        if (SiteId > 0)
            Columnlist = ServiceFactory.ColumnService.GetList(0, SiteId,0, 0);
        else
            Columnlist = new List<Column>();
         
    }
}