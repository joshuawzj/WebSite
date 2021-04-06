/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：workflowlist.aspx.cs
 * 文件描述：待审核信息
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Repository;
using Whir.Config;
using Whir.Language;
using System.Text;

public partial class whir_system_ModuleMark_common_workflowlist : Whir.ezEIP.Web.SysManagePageBase
{

    /// <summary>
    /// 只显示开启了工作流的栏目
    /// </summary>
    protected List<Column> ColumnsList { get; set; }

    /// <summary>
    /// 只显示开启了工作流的栏目
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDel { get; set; }

    /// <summary>
    /// 是否显示编辑按钮
    /// </summary>
    public bool IsShowEdit { get; set; }

    /// <summary>
    /// 编辑的页面地址
    /// </summary>
    public string EditPageUrl { get; set; }

    /// <summary>
    /// 是否显示删除按钮
    /// </summary>
    public bool IsShowDelete { get; set; }

    /// <summary>
    /// 是否显示排序
    /// </summary>
    public bool IsShowSort { get; set; }

    /// <summary>
    /// 是否显示预览
    /// </summary>
    public bool IsShowView { get; set; }

    /// <summary>
    /// 是否回收站
    /// </summary>
    public bool IsRecycle { get; set; }

    /// <summary>
    /// 主键
    /// </summary>
    protected string IdField { get; set; }

    /// <summary>
    /// 显示的列
    /// </summary>
    protected string Columns { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        IsShowEdit = IsCurrentRoleMenuRes("376"); ;
        IsShowDelete = IsCurrentRoleMenuRes("377"); ;
        IsShowView = IsCurrentRoleMenuRes("375");

        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("54"));
            BindColumList();
            BindColums();
        }
    }

    /// <summary>
    ///绑定前台展示的字段
    /// </summary>
    private void BindColums()
    {
        IdField = "Id";
        Columns += " columns: [{ title: '<input type=\"checkbox\"  id=\"btSelectAll\" />',field: 'IDtypeid',width: 40,align: 'center',valign: 'middle',formatter: function (value, row, index) { return GetCheckbox(value, row, index); }}, ";
        Columns += "{title: 'Id', field: 'Id',align: 'center',valign: 'middle'},";
        Columns += "{title: '" + "审核状态".ToLang() + "', field: 'WorkFlowName',align: 'center',valign: 'middle'},";
        Columns += "{title: '" + "标题".ToLang() + "', field: 'commontitle',filterControl: '1',align: 'center',valign: 'middle'},";
        Columns += "{title: '" + "所属栏目".ToLang() + "', field: 'ColumnName',filterControl: '9',align: 'center',valign: 'middle'},";
        Columns += "{title: '" + "发布时间".ToLang() + "', field: 'CreateDate',filterControl: '7',format:'yyyy-MM-dd HH:mm:ss',align: 'center',valign: 'middle',formatter: function(value, row, index) {return GetDateTimeFormat(value, row, index,'yyyy-MM-dd HH:mm:ss');}},";
        Columns += "{title: '" + "发布者".ToLang() + "', field: 'CreateUser',filterControl: '1',align: 'center',valign: 'middle'},";

        if (IsShowEdit || IsShowDelete)
        {
            Columns += "{title: '" + "操作".ToLang() + "',field: '" + IdField +
                       "',align: 'center',valign: 'middle',formatter: function(value, row, index) {return GetOperation(value, row, index);}}";
        }
        else
        {
            Columns = Columns.Substring(0, Columns.Length - 1);
        }
        Columns += "]";

    }

    /// <summary>
    ///绑定栏目
    /// </summary>
    private void BindColumList()
    {
        IList<Column> list = ServiceFactory.ColumnService.GetList(0, CurrentSiteId);
        List<int> canShowColumnIds = new List<int>();
        foreach (Column c in list)
        {
            if (c.ModelId > 0 && c.WorkFlow > 0)
            {
                canShowColumnIds.Add(c.ColumnId);

                if (c.ParentId > 0)
                {
                    canShowColumnIds.Add(c.ParentId);
                }
            }

        }

        //只显示开启了工作流的栏目
        ColumnsList = ServiceFactory.ColumnService.GetList(0, CurrentSiteId).Where(p => canShowColumnIds.Contains(p.ColumnId)).ToList();

    }

}