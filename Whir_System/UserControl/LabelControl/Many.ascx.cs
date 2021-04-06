/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：Many.ascx.cs
 * 文件描述：字符串拆分置标
 */
using System;
using System.Data;
using System.Web.UI;

using Whir.Framework;
using Whir.Label.Dynamic;
using Whir.Domain;
using Whir.Label;

public partial class whir_system_UserControl_LabelControl_Many : Whir.ezEIP.Web.SysControlBase
{
    #region 对外属性
    /// <summary>
    /// 多图片或多文件字符串值，格式如“a,b,c”。 
    /// </summary>
    private string _splitStr = "";
    public string SplitStr
    {
        get
        {
            return _splitStr;
        }
        set
        {
            _splitStr = value;
        }
    }

    /// <summary>
    /// 分隔符号，默认为","半角逗号
    /// </summary>
    private string _split = "*";
    public string Split
    {
        get
        {
            return _split;
        }
        set
        {
            _split = value;
        }
    }

    /// <summary>
    /// 读取数量，即最大的显示数，如果不指定则显示所有拆分记录。
    /// </summary>
    private string _count = int.MaxValue.ToStr();
    public string Count
    {
        get
        {
            return _count;
        }
        set
        {
            _count = value;
        }
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

    #region 对内属性

    private ITemplate itemTemplate = null;
    [TemplateContainer(typeof(Many.DataContainer))]
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
    /// 绑定
    /// </summary>
    public void Bind()
    {
        this.Controls.Clear();
        Many many = new Many(SplitStr, Split, Count.ToInt(), PageColumn, PageSiteInfo);
        DataTable dt = many.GetLabelData();//获得处理数据结果
        int ItemCount = dt.Rows.Count;//总记录数

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Many.DataContainer container = new Many.DataContainer(dt.Rows[i], i, ItemCount);//提供第一行数据（也仅一行）
            itemTemplate.InstantiateIn(container);
            this.Controls.Add(container);
        }
        DataBind();//必须绑定一次，否则前台无法调用数据
        //绑定后为子置标控件赋值PageColumn、PageSiteInfo
        LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn);
    }
}