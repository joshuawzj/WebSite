/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：Menu.ascx.cs
 * 文件描述：菜单置标，
 */
using System;
using System.Collections.Generic;
using System.Web.UI;
using Whir.Domain;
using Whir.Framework;
using Whir.Label;
using Whir.Service;

public partial class whir_system_UserControl_LabelControl_Menu : Whir.ezEIP.Web.SysControlBase
{
    #region 对外属性

    /// <summary>
    /// 栏目ID
    /// </summary>
    public string ColumnId { get; set; }

    /// <summary>
    /// 子站Id
    /// </summary>
    public string SubjectId { get; set; }

    /// <summary>
    /// 站点目录
    /// </summary>
    public string SitePath { get; set; }

    /// <summary>
    /// 类别ID,大于0则自动加上此属性
    /// </summary>
    public string CategoryId = WebUtil.Instance.GetQueryInt("lcid", 0).ToStr();

    /// <summary>
    /// 是否只显示当前栏目的最顶级栏目，默认是false
    /// </summary>
    public bool OnlyOne { get; set; }

    /// <summary>
    /// 遍历菜单层级，显示多少层菜单由写了多少层menu置标决定
    /// v5.3.0版本添加 cxz 2018.10.22
    /// 默认0-从当前栏目id遍历到顶级再遍历所有子栏目 
    ///     1-从当前栏目id遍历所有子栏目，包括自身栏目 
    ///     2-从当前栏目id遍历所有子栏目，不包括自身栏目
    /// </summary>
    public int MenuType { get; set; }

    /// <summary>
    /// 模板
    /// </summary>
    private ITemplate itemTemplate = null;
    [TemplateContainer(typeof(Whir.Label.Dynamic.Menu.DataContainer))]
    public ITemplate ItemTemplate
    {
        get { return itemTemplate; }
        set { itemTemplate = value; }
    }

    /// <summary>
    /// 当前页面所属栏目实体（仅为栏目的首页、列表页、内容页时有值）
    /// </summary>
    public Column PageColumn { get; set; }
    /// <summary>
    /// 当前页面所属站点实体
    /// </summary>
    public SiteInfo PageSiteInfo { get; set; }

    #endregion 对外属性

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Bind();
        }
        catch (Exception ex)
        {
            Show_Error(ex, this.Controls, ID);
        }

    }

    /// <summary>
    /// 加载数据
    /// </summary>
    public void Bind()
    {
        this.Controls.Clear();

        try
        {
            ColumnId = ((Whir.Label.Dynamic.DynamicDataContainer)this.Parent).DataRow["ColumnId"].ToStr();
        }
        catch
        { }

        var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId.ToInt(0)) ?? new Column();
        if (column.IsCategory && column.IsCategoryShow && !OnlyOne)
        {
            LiteralControl lt = new LiteralControl();
            lt.Text = ""; //无信息时的提示

            this.Controls.Add(lt);

        }
        else
        {
            if (!SitePath.IsEmpty())
                PageSiteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>("Where Path=@0", SitePath) ?? new SiteInfo();
            bool isEnd = ParentIsMenu();
            SubjectId = SubjectId.IsEmpty() ? Request.QueryString["SubjectId"] : SubjectId;
            Whir.Label.Dynamic.Menu labelList = new Whir.Label.Dynamic.Menu(ID, ColumnId.ToInt(), SubjectId.ToInt(), CategoryId, isEnd, OnlyOne, MenuType, ItemTemplate, this.Parent.Page, PageColumn, PageSiteInfo, this);
            List<Whir.Label.Dynamic.Menu.DataContainer> itemplateList;//正常模板的返回记录

            labelList.GetLabelData(out itemplateList);

            if (itemplateList.Count == 0)
            {

                LiteralControl lt = new LiteralControl();
                lt.Text = ""; //无信息时的提示

                this.Controls.Add(lt);
            }
            else
            {

                foreach (Whir.Label.Dynamic.Menu.DataContainer d in itemplateList)
                {
                    this.Controls.Add(d);
                }

            }
        }

        DataBind();//必须绑定一次，否则前台无法调用数据
        //绑定后为子置标控件赋值PageColumn、PageSiteInfo
        LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn);

    }

    /// <summary>
    /// 判断父控件是否Menu置标
    /// </summary>
    /// <returns></returns>
    private bool ParentIsMenu()
    {
        try
        {
            var ascxPath = this.BindingContainer.TemplateControl.AppRelativeVirtualPath;
            if (ascxPath.ToLower().Contains("Menu.ascx".ToLower()))
                return true;
            else
                return false;
        }
        catch
        {
            return false;
        }

    }
}