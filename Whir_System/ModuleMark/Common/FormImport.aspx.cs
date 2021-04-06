/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：formimport.aspx.cs
 * 文件描述：信息导入页面
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using System.Text.RegularExpressions;
using Whir.Language;
using System.Data;

public partial class Whir_System_ModuleMark_Common_FormImport : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// Excel文件中的列
    /// </summary>
    protected IList<string> ExcelColumnList { get; set; }

    /// <summary>
    /// Excel文件的存放路径
    /// </summary>
    protected string FilePath
    {
        get { return ViewState["FilePath"].ToStr(); }
        set { ViewState["FilePath"] = value; }
    }

    /// <summary>
    /// 当前编辑的栏目
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 子站ID
    /// </summary>
    protected int SubjectId { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        if (!IsPostBack)
        {
            if (ColumnId > 1)
                JudgeOpenPagePermission(IsRoleHaveColumnRes("导入", ColumnId, SubjectId == 0 ? -1 : SubjectId));
            
        }
    }

    //绑定列表
    private void BindList()
    {
        FilePath = UploadFilePath + txtPath.Value;
        string fileDir = Server.MapPath(FilePath);
        ExcelColumnList = ExcelUtil.GetExcelHead(fileDir);

        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
        if (column == null) return;

        Dictionary<Form, Field> dictFormAndField = new Dictionary<Whir.Domain.Form, Field>();
        IList<Form> listForm = ServiceFactory.FormService.GetListByColumnId(ColumnId);
        foreach (Form form in listForm)
        {
            Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(form.FieldId);
            if (field != null)
            {
                //未启用转向链接，不显示转向链接的相关字段
                if (!column.IsRedirect)
                {
                    if (field.FieldName.ToLower() == "enableredirecturl" || field.FieldName.ToLower() == "redirecturl")
                        continue;
                }
                if (field.IsEnableListShow == -1)
                    continue;
                if (field.FieldName.ToLower() == "categoryid")//类别使用数字
                {
                    field.TypeName = "int";
                }
                //if (field.IsHidden)
                //    continue;
                dictFormAndField.Add(form, field);
            }
        }

        rptList.ItemDataBound += new RepeaterItemEventHandler(rptList_ItemDataBound);
        rptList.DataSource = dictFormAndField;
        rptList.DataBind();
    }

    private void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            var ddlExcelColumn = e.Item.FindControl("ddlExcelColumn") as Whir.Framework.DropDownList;
            var ckbOnly = e.Item.FindControl("ckbOnly") as CheckBox;
            if (ddlExcelColumn != null && ckbOnly != null)
            {
                ddlExcelColumn.DataSource = ExcelColumnList;
                ddlExcelColumn.DataBind();
                ddlExcelColumn.Items.Insert(0, new ListItem("==请选择==", ""));

                //默认选中
                int selectedIndex = e.Item.ItemIndex + 1;
                if (ddlExcelColumn.Items.Count >= selectedIndex)
                {
                    ddlExcelColumn.SelectedIndex = selectedIndex;
                }

                string typeName = ckbOnly.Attributes["TypeName"].ToLower();
                if (typeName == "ntext" || typeName == "text")
                {
                    ckbOnly.Visible = false;
                }
            }
        }
    }

    /// <summary>
    /// 上传成功后，绑定数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbnUploaded_Click(object sender, EventArgs e)
    {
        phUpload.Visible = false;
        phField.Visible = true;
        BindList();
    }

    /// <summary>
    /// 导入
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbnImport_Click(object sender, EventArgs e)
    {
        Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
        bool strategy = rdbStrategy.Value.ToBoolean();//重复策略
        foreach (RepeaterItem item in rptList.Items)
        {
            Literal litFieldName = item.FindControl("litFieldName") as Literal;//字段名
            Literal litTypeName = item.FindControl("litTypeName") as Literal;//类型名称
            CheckBox ckbOnly = item.FindControl("ckbOnly") as CheckBox;//是否进行唯一性验证
            Whir.Framework.DropDownList ddlExcelColumn = item.FindControl("ddlExcelColumn") as Whir.Framework.DropDownList;
            if (ddlExcelColumn != null && !ddlExcelColumn.SelectedValue.IsEmpty())
            {
                if (litFieldName != null)
                    if (litTypeName != null)
                        dict.Add(litFieldName.Text, new[]{
                            ddlExcelColumn.SelectedValue,
                            litTypeName.Text,
                            ckbOnly != null && ckbOnly.Checked?"1":"0"
                        });
            }
        }

        try
        {
            //string result = ContentHelper.ContentImport(ColumnId, SubjectId, FilePath, strategy, dict, CurrentUserName);//此代码已封装
            string result = ImportHelper.ContentImport(ColumnId, SubjectId, FilePath, strategy, dict, CurrentUserName);//开放源码
            CustomAlert(result);
        }
        catch (Exception ex)
        {
            CustomAlert(ex.Message);
        }
    }

    private void CustomAlert(string message)
    {
        message = Regex.Replace(message, @"\s+", " ");
        message = message.Replace("'", "\\'");
        string script = "<script language=\"javascript\" > window.parent.whir.toastr.success( '" + message + "');window.parent.$table.bootstrapTable('refresh');window.parent.whir.dialog.remove(); </script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
    }

    /// <summary>
    /// 下载模版
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbnDownTemplate_Click(object sender, EventArgs e)
    {
        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
        if (column == null) return;

        IList<Form> listForm = ServiceFactory.FormService.GetListByColumnId(ColumnId);
        DataTable table = new DataTable();

        IList<object> list = new List<object>();

        foreach (Form form in listForm)
        {
            Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(form.FieldId);
            if (field != null)
            {
                //未启用转向链接，不显示转向链接的相关字段
                if (!column.IsRedirect)
                {
                    if (field.FieldName.ToLower() == "enableredirecturl" || field.FieldName.ToLower() == "redirecturl")
                        continue;
                }

                if (field.IsEnableListShow == -1)
                    continue;
                DataColumn col = null;
                //设置列类型
                switch ((FieldType)field.FieldType)
                {
                    case FieldType.Area://地区
                        table.Columns.Add(form.FieldAlias);
                        list.Add(1);
                        break;
                    case FieldType.Bool://bool
                        table.Columns.Add(form.FieldAlias);
                        list.Add("true");
                        break;
                    case FieldType.Color:
                        table.Columns.Add(form.FieldAlias);
                        list.Add("#FF0");
                        break;
                    case FieldType.File:
                        table.Columns.Add(form.FieldAlias);
                        list.Add("示例文件.xls");
                        break;
                    case FieldType.Video:
                        table.Columns.Add(form.FieldAlias);
                        list.Add("示例视频.flv");
                        break;
                    case FieldType.Picture:
                        table.Columns.Add(form.FieldAlias);
                        list.Add("示例图片.jpg");
                        break;
                    case FieldType.Label:
                    case FieldType.MultipleHtmlText:
                    case FieldType.MultipleText:
                    case FieldType.None:
                    case FieldType.Text:
                        table.Columns.Add(form.FieldAlias, typeof(string));
                        list.Add("示例" + form.FieldAlias);
                        break;
                    case FieldType.PassWord:
                        table.Columns.Add(form.FieldAlias, typeof(string));
                        list.Add("123456");
                        break;
                    case FieldType.DateTime:
                        table.Columns.Add(form.FieldAlias, typeof(DateTime));
                        list.Add(DateTime.Now);
                        break;
                    case FieldType.Money:
                        table.Columns.Add(form.FieldAlias, typeof(decimal));
                        list.Add(100);
                        break;
                    case FieldType.Number:
                        table.Columns.Add(form.FieldAlias, typeof(int));
                        list.Add(10);
                        break;
                    case FieldType.Link:
                        table.Columns.Add(form.FieldAlias, typeof(string));
                        list.Add("http://www.baidu.com");
                        break;
                    case FieldType.ListBox:
                        table.Columns.Add(form.FieldAlias, typeof(string));
                        list.Add("2");
                        break;
                    default:
                        table.Columns.Add(form.FieldAlias, typeof(string));
                        list.Add("示例" + form.FieldAlias);
                        break;
                }
            }
        }
        DataRow row = table.NewRow();
        for (int i = 0; i < list.Count; i++)
        {
            row[i] = list[i];
        }
        table.Rows.Add(row);
        ExcelUtil.CreateExcel(table, column.ColumnName + "_导入模版.xls");
    }
}