/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：if.ascx.cs
 * 文件描述：条件判断置标
 */
using System;
using System.Data;
using System.Web.UI;

using Whir.Framework;
using Whir.Label.Dynamic;
using Whir.Domain;
using Whir.Label;

public partial class whir_system_UserControl_LabelControl_If : Whir.ezEIP.Web.SysControlBase
{
    #region  属性

    /// <summary>
    /// 测试类型（要比较的字符串）
    /// </summary> 
    public string TestType = "";

    /// <summary>
    /// 测试操作
    /// </summary> 
    public string TestOperate = "";

    /// <summary>
    /// 测试值
    /// </summary> 
    public string TestValue = "";

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

    /// <summary>
    /// 为true的模板
    /// </summary>
    private ITemplate successTemplate = null;
    [TemplateContainer(typeof(IfLabel.DataContainer))]
    public ITemplate SuccessTemplate
    {
        get { return successTemplate; }
        set { successTemplate = value; }
    }

    /// <summary>
    /// 为false的模板
    /// </summary>
    private ITemplate failureTemplate = null;
    [TemplateContainer(typeof(IfLabel.DataContainer))]
    public ITemplate FailureTemplate
    {
        get { return failureTemplate; }
        set { failureTemplate = value; }
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

        IfLabel il = new IfLabel(TestType, TestOperate, TestValue, PageColumn, PageSiteInfo);

        //是否可以比较
        bool IsShow = true;
        bool Resut = il.GetLabelData(out IsShow);//获得处理数据结果

        if (IsShow)
        {
             
                IfLabel.DataContainer container = new IfLabel.DataContainer();
                if (Resut)
                {
                    if (successTemplate!=null)
                    successTemplate.InstantiateIn(container);
                }
                else
                {
                    if (failureTemplate != null)
                    failureTemplate.InstantiateIn(container);
                }
                this.Controls.Add(container);

                DataBind();//必须绑定一次，否则前台无法调用数据
                //绑定后为子置标控件赋值PageColumn、PageSiteInfo
                LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn);
            
        } 
    }
}