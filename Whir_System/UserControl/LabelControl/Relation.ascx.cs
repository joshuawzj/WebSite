/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：Relation.ascx.cs
 * 文件描述：相关文章置标
 */
using System;
using System.Web.UI;
using System.Collections.Generic;

using Whir.Framework;
using Whir.Domain;
using Whir.Label;
using Relation = Whir.Label.Dynamic.Relation;

public partial class Whir_System_UserControl_LabelControl_Relation : Whir.ezEIP.Web.SysControlBase
{
    #region 对外属性

    public string MainInfoParmName { get; set; }

    /// <summary>
    /// 读取数量
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 无数据提示文字
    /// </summary>
    public string NullTip = "";

    /// <summary>
    /// 模板
    /// </summary>
    private ITemplate _itemTemplate = null;


    [TemplateContainer(typeof(Whir.Label.Dynamic.Relation.DataContainer))]
    public ITemplate ItemTemplate
    {
        get { return _itemTemplate; }
        set { _itemTemplate = value; }
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
            if (Count <= 0)
                Count = 100;
            if (MainInfoParmName.IsEmpty())
                MainInfoParmName = "itemId";

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
        Relation labelList = new Relation(ID, PageColumn, PageSiteInfo, ItemTemplate, Count, MainInfoParmName);
        List<Relation.DataContainer> itemplateList;//正常模板的返回记录

        labelList.GetLabelData(out itemplateList);
        if (itemplateList.Count == 0)
        {
            //无信息时的提示
            LiteralControl lt = new LiteralControl
            {
                Text = NullTip.IsEmpty() ? PageSiteInfo.NullTip : NullTip
            };
            Controls.Add(lt);
        }
        else
        {
            foreach (Relation.DataContainer d in itemplateList)
            {
                Controls.Add(d);
            }

        }
        DataBind();//必须绑定一次，否则前台无法调用数据
        //绑定后为子置标控件赋值PageColumn、PageSiteInfo
        LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn);
    }
}