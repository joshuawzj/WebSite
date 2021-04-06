/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：InforArea.ascx.cs
 * 文件描述：内容置标
 */
using System;
using System.Data;
using System.Web.UI;

using Whir.Framework;
using Whir.Label.Dynamic;
using Whir.Domain;
using Whir.Label;

public partial class whir_system_UserControl_LabelControl_InforArea : Whir.ezEIP.Web.SysControlBase
{
    #region 对外属性

    /// <summary>
    /// 栏目ID值，如不指定则获取URL传递过来的typeid值
    /// </summary>
    public string ColumnId = WebUtil.Instance.GetQueryInt("typeid", 0).ToStr();
    /// <summary>
    /// 查询的SQL语句，指定了SQL后ItemID和Fields则无效
    /// </summary>
    public string Sql { get; set; }
    /// <summary>
    /// 主键ID值,如不指定则获取URL传递过来的itemid值 
    /// </summary>
    public string ItemID = WebUtil.Instance.GetQueryInt("itemid", 0).ToStr();
    /// <summary>
    /// 显示的字段，默认为“*”全部 
    /// </summary>
    public string Field { get; set; }
    /// <summary>
    /// 当前页面所属栏目实体（仅为栏目的首页、列表页、内容页时有值）
    /// </summary>
    public Column PageColumn { get; set; }
    /// <summary>
    /// 当前页面所属站点实体
    /// </summary>
    public SiteInfo PageSiteInfo { get; set; }
    /// <summary>
    /// 子站id 
    /// </summary>
    public string SubjectId = WebUtil.Instance.GetQueryInt("SubjectId", 0).ToStr();
    #endregion

    #region 对内属性

    private ITemplate itemTemplate = null;
    [TemplateContainer(typeof(InforArea.DataContainer))]
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
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        this.Controls.Clear();
        InforArea ia = new InforArea(ColumnId.ToInt(), SubjectId.ToInt(), Sql, ItemID.ToInt(), Field, PageColumn, PageSiteInfo, this);
        DataTable dt = ia.GetLabelData();//获得处理数据结果

        if (dt != null && dt.Rows.Count > 0)//存在有数据时再绑定，否则会引起未将对象引用实例
        {
            InforArea.DataContainer container = new InforArea.DataContainer(dt);//提供第一行数据（也仅一行）
            itemTemplate.InstantiateIn(container);
            this.Controls.Add(container);

            DataBind();//必须绑定一次，否则前台无法调用数据
            //绑定后为子置标控件赋值PageColumn、PageSiteInfo
            LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn);
        }
        else
        {
            /*恢复不跳转404
            if (PageColumn != null && (PageColumn.ModuleMark == "SinglePage_v0.0.01" || PageColumn.ModuleMark == "SubsiteSinglePage_v0.0.01"))
            {
                return;
            }
            Response.Redirect(AppName + "404.aspx");
             * */
        }
    }
}

