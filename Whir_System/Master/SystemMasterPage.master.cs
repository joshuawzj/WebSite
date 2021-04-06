/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：SystemMasterPage.master.cs
 * 文件描述：公用母板页
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Service;
using Whir.Language;
using Whir.Framework;
using System.Web;

public partial class Whir_System_Master_SystemMasterPage : SysMasterPageBase
{
    
    //网站路径
    protected string Location { get; set; }

    //站点内容是否有记录 是否显示
    protected bool HasSite { get; set; }
    //子站点是否有记录 是否显示
    protected bool HasSubSite { get; set; }
    //专题是否有记录 是否显示
    protected bool HasSubject { get; set; }

    protected SysManagePageBase SysManagePageBase = new Whir.ezEIP.Web.SysManagePageBase();

    /// <summary>
    /// 菜单list
    /// </summary>
    protected List<Menu> MenuList = new List<Menu>();

    protected void Page_Load(object sender, EventArgs e)
    {
       
        //绑定网站导航
        BindLocation();

        if (!IsPostBack)
        {
            GetSiteId();//指定当前站点
            this.libSiteName.Text = SiteInfoHelper.SiteInfo.SiteName;

            //绑定多站点
            GetMultiSite();
            int siteId = SiteInfoHelper.SiteInfo.SiteId;
            HasSite = ServiceFactory.ColumnService.GetListCount(0, siteId) > 0 ? true : false;
            HasSubSite = ServiceFactory.ColumnService.GetListCount(1, siteId) > 0 ? true : false;
            HasSubject = ServiceFactory.ColumnService.GetListCount(2, siteId) > 0 ? true : false;
            MenuList = ServiceFactory.MenuService.Query<Menu>("Where ParentId=0 and MenuType=@0", "top").OrderBy(p => p.Sort).ToList();

        }
    }
 
    /// <summary>
    /// 绑定网站导航
    /// </summary>
    private void BindLocation()
    {
        //取得路径
        string Url = HttpContext.Current.Request.Path.ToLower();
        Url = Url.Substring(Url.IndexOf("whir_system/") + 12);

        string SQL = string.Format("SELECT TOP 1 MenuId FROM Whir_Dev_Menu WHERE LOWER(Url) like '%{0}%' and IsDel=0", Url);
        //根据路径查询MenuId
        int MenuId = ServiceFactory.MenuService.ExecuteScalar<object>(SQL).ToInt(0);

        if (MenuId > 0)
        {
            SQL = string.Format(@";with tab as
                        (
                         select MenuId,ParentId,MenuName from Whir_Dev_Menu where MenuId={0} 
                         union all
                         select b.MenuId,b.ParentId,b.MenuName 
                         from tab a, Whir_Dev_Menu b   
                         where a.ParentId=b.MenuId  
                        )
                        select * from tab ", MenuId);
            List<Menu> MenuList = ServiceFactory.MenuService.Query<Menu>(SQL).ToList();
            Stack<Menu> list = new Stack<Menu>();

            foreach (Menu Item in MenuList)
            {
                list.Push(Item);
            }
            foreach (Menu Item in list)
            {
                Location += "<li><i class=\"fa fa-lg fa-angle-right\"></i></li>";
                Location += "<li><a title=\"{0}\">{0}</a></li>".FormatWith(Item.MenuName.ToLang());
            }
        }
        else
        {
            if (RequestUtil.Instance.GetString("columnId").ToInt() > 0)
            {
                Location += "<li><i class=\"fa fa-lg fa-angle-right\"></i></li>";
                Location += "<li><a title=\"{0}\">{0}</a></li>".FormatWith("内容管理".ToLang());
            }
            else
                Location = "";
        }
    }

    /// <summary>
    /// 绑定多站点
    /// </summary>
    private void GetMultiSite()
    {
        rptSiteList.DataSource = ServiceFactory.SiteInfoService.GetList();
        rptSiteList.DataBind();
    }

    /// <summary>
    /// 切换站点，根据传参“siteid”是有否值来判断
    /// </summary>
    private void GetSiteId()
    {
        int SiteID = WebUtil.Instance.GetQueryInt("siteid", 0);
        if (SiteID != 0)
        {
            SiteInfoHelper.SiteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(SiteID);
        }
    }
 
}
