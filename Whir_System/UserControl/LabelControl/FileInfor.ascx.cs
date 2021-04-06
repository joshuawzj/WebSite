/*
 * Copyright © 2009-2018 万户网络技术有限公司
 * 文 件 名：File.ascx.cs
 * 文件描述：文件信息置标
 */
using System;
using System.Data;
using System.Web.UI;

using Whir.Framework;
using Whir.Label.Dynamic;
using Whir.Domain;
using Whir.Label;

public partial class whir_system_UserControl_LabelControl_FileInfor : Whir.ezEIP.Web.SysControlBase
{
    #region 对外属性
    /// <summary>
    /// 栏目ID值，如不指定则获取URL传递过来的typeid值
    /// </summary>
    public string ColumnId = WebUtil.Instance.GetQueryInt("typeid", 0).ToStr();
    /// <summary>
    /// 主键ID值,如不指定则获取URL传递过来的itemid值 
    /// </summary>
    public string ItemID = WebUtil.Instance.GetQueryInt("itemid", 0).ToStr();
    /// <summary>
    /// 显示的字段
    /// </summary>
    public string Field { get; set; }
    /// <summary>
    /// 下载次数字段 
    /// </summary>
    public string DownloadsField { get; set; }
    /// <summary>
    /// 是否转换文件大小
    /// </summary>
    public string IsConvert { get; set; }
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
    [TemplateContainer(typeof(FileInfor.DataContainer))]
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
        FileInfor file = new FileInfor(ColumnId.ToInt(), ItemID.ToInt(), Field, DownloadsField, IsConvert, PageColumn, PageSiteInfo, this);
        DataTable dt = file.GetLabelData();//获得处理数据结果

        if (dt != null && dt.Rows.Count > 0)//存在有数据时再绑定，否则会引起未将对象引用实例
        {
            FileInfor.DataContainer container = new FileInfor.DataContainer(dt);//提供第一行数据（也仅一行）
            itemTemplate.InstantiateIn(container);
            this.Controls.Add(container);

            DataBind();//必须绑定一次，否则前台无法调用数据
            //绑定后为子置标控件赋值PageColumn、PageSiteInfo
            LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn);
        }
    }
}