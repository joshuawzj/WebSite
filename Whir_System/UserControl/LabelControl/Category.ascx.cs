/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：Category.ascx.cs
 * 文件描述：Category置标(树型结构类别读取标签（只适用于模块版本号为Category的栏目）)
 */

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Data;
using System.Reflection;
using System.Linq;

using Whir.Repository;
using Whir.Framework;
using Whir.Label.Dynamic;
using Whir.Domain;
using Whir.Label;
using Whir.Service;

public partial class whir_system_UserControl_LabelControl_Category : Whir.ezEIP.Web.SysControlBase
{
    #region 对外属性
    /// <summary>
    /// 栏目ID指定读某个栏目的数据
    /// </summary>
    public string ColumnId { get; set; }
    /// <summary>
    /// 子站id
    /// </summary>
    public string SubjectId { get; set; }
    /// <summary>
    /// 要查询的字段
    /// </summary>
    public string Fields { get; set; }
    /// <summary>
    /// 所属父ID,可指定从那一级开始读
    /// </summary>
    public string ParentID { get; set; }
    /// <summary>
    /// 指定的ColumnId是否是父栏目的
    /// </summary>
    public bool IsParentColumnId = true;
    /// <summary>
    /// 是否用于菜单
    /// </summary>
    public bool IsMenu = false;
    /// <summary>
    /// SQL语句
    /// </summary>
    public string Sql { get; set; }
    /// <summary>
    /// Where条件
    /// </summary>
    public string Where { get; set; }
    /// <summary>
    /// 排序条件
    /// </summary>
    public string Order { get; set; }
    /// <summary>
    /// 当前页面所属栏目实体（仅为栏目的首页、列表页、内容页时有值）
    /// </summary>
    public Column PageColumn { get; set; }
    /// <summary>
    /// 当前页面所属站点实体
    /// </summary>
    public SiteInfo PageSiteInfo { get; set; }
    #endregion

    #region 对内属性
    private ITemplate itemTemplate = null;
    [TemplateContainer(typeof(Category.DataContainer))]
    public ITemplate ItemTemplate
    {
        get { return itemTemplate; }
        set { itemTemplate = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
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
    /// 绑定显示
    /// </summary>
    public void Bind()
    {
        try
        {
            this.Controls.Clear();
            if (ParentIsMenu() || IsMenu)
            {
                try
                {
                    ColumnId = ((Whir.Label.Dynamic.DynamicDataContainer)this.Parent).DataRow["ColumnId"].ToStr();
                    var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId.ToInt(0)) ??
                                 new Column();
                    if (!column.IsCategory || !column.IsCategoryShow)
                        return;
                }
                catch
                {
                    if (ColumnId.ToInt() != 0)
                    {
                        var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId.ToInt(0)) ??
                                     new Column();

                        if (!column.IsCategory || !column.IsCategoryShow)
                            return;
                    }
                }
            }
            SubjectId = SubjectId.IsEmpty() ? Request.QueryString["SubjectId"] : SubjectId;
            Category c = new Category(ColumnId.ToInt(), SubjectId.ToInt(), Fields, ParentID.ToInt(), Sql, Where, Order, ItemTemplate, IsParentColumnId, PageColumn, PageSiteInfo, this);

            List<Category.DataContainer> list = new List<Category.DataContainer>();
            c.GetLabelData(out list);//获得处理数据结果

            foreach (Category.DataContainer d in list)
            {
                this.Controls.Add(d);
            }
            this.DataBind();

            //绑定后为子置标控件赋值PageColumn、PageSiteInfo
            LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn);

        }
        catch (Exception ex)
        {
            throw new Exception(string.Format("'{0}'标签解析异常!<br />{1}<br />{2}", this.ID, ex.Message, ex.StackTrace));
        }
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