/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：List.ascx.cs
 * 文件描述：列表置标，
 */
using System;
using System.Data;
using System.Web.UI;
using System.Collections.Generic;

using Whir.Framework;
using Whir.Label.Dynamic;
using Whir.Domain;
using Whir.Label;

public partial class whir_system_UserControl_LabelControl_List : Whir.ezEIP.Web.SysControlBase
{
    #region 对外属性

    /// <summary>
    /// 栏目ID
    /// </summary>
    public string ColumnId { get; set; } 
    
    /// <summary>
    /// 栏目ID
    /// </summary>
    public string SubjectId { get; set; }

    /// <summary>
    /// 要查询的字段
    /// </summary>
    public string Fields { get; set; }

    /// <summary>
    /// 其它栏目ID，用于读取同一个栏目类型其它栏目值,要求格式"栏目ID,栏目ID"
    /// </summary>
    public string OtherColumnId = "";

    /// <summary>
    /// 读取数量
    /// </summary>
    public string Count = "0";

    /// <summary>
    /// 条件语句
    /// </summary>
    public string Where = "";

    /// <summary>
    /// 排序语句
    /// </summary>
    public string Order { get; set; }

    /// <summary>
    /// SQL语句
    /// </summary>
    public string Sql { get; set; }

    /// <summary>
    /// 无数据提示文字
    /// </summary>
    public string NullTip = "";

    /// <summary>
    /// 是否需要分页
    /// </summary>
    public bool NeedPage = false;

    /// <summary>
    /// 类别ID
    /// </summary>
    public string CategoryId { get; set; } 

    /// <summary>
    /// 模板
    /// </summary>
    private ITemplate itemTemplate = null;
    [TemplateContainer(typeof(List.DataContainer))]
    public ITemplate ItemTemplate
    {
        get { return itemTemplate; }
        set { itemTemplate = value; }
    }

    #region 头条模板属性
    /// <summary>
    /// 模板
    /// </summary>
    private ITemplate _top = null;
    [TemplateContainer(typeof(List.DataContainer))]
    public ITemplate Top
    {
        get { return _top; }
        set { _top = value; }
    }
    /// <summary>
    /// 头条记录数量
    /// </summary>
    public int TopCount = 1;
    /// <summary>
    /// 主键名
    /// </summary>
    public string TopKeyName = "";
    /// <summary>
    /// 头条排序
    /// </summary>
    public string TopOrder = "";
    /// <summary>
    /// 在ItemTemplate条件上再次过滤的条件
    /// </summary>
    public string TopWhere = "";
    /// <summary>
    /// 是否在分页时每页都显示头条信息
    /// </summary>
    public bool TopAlwayShow = false;
    /// <summary>
    /// 当前页面所属栏目实体（仅为栏目的首页、列表页、内容页时有值）
    /// </summary>
    public Column PageColumn { get; set; }
    /// <summary>
    /// 当前页面所属站点实体
    /// </summary>
    public SiteInfo PageSiteInfo { get; set; }

    #endregion

    #endregion 对外属性

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Bind();
        }
        catch (Exception ex)
        {
            Show_Error(ex, Controls, ID);
        }

    }

    /// <summary>
    /// 加载数据
    /// </summary>
    public void Bind()
    {
        Controls.Clear();
        List labelList = new List(ID, ColumnId.ToInt(),SubjectId.ToInt(), Fields, OtherColumnId, Count.ToInt(), 
                                Where, Order, Sql, NeedPage, CategoryId, ItemTemplate, Top, Parent.Page, 
                                TopWhere, TopCount, TopKeyName, TopOrder, TopAlwayShow, PageColumn, PageSiteInfo, this);
        List<List.DataContainer> itemplateList;//正常模板的返回记录
        List<List.DataContainer> topItemplateList;//头条模板的返回记录

        labelList.GetLabelData(out itemplateList, out topItemplateList);
        if (itemplateList.Count == 0 && topItemplateList.Count == 0)
        {
            LiteralControl lt = new LiteralControl();
            lt.Text = NullTip.IsEmpty() ? PageSiteInfo.NullTip : NullTip; //无信息时的提示

            Controls.Add(lt);
        }
        else
        {
            //头条先于正常模板绑定
            foreach (List.DataContainer d in topItemplateList)
            {
                Controls.Add(d);
            }

            foreach (List.DataContainer d in itemplateList)
            {
                Controls.Add(d);
            }

        }
        DataBind();//必须绑定一次，否则前台无法调用数据
        //绑定后为子置标控件赋值PageColumn、PageSiteInfo
        LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn);

    }
}