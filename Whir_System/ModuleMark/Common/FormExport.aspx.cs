/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：formexport.aspx.cs
 * 文件描述：根据选择字段导出Excel文件的页面
 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;

using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;
using Whir.Repository;
using System.Web.Script.Serialization;
using Whir.Security.Service;
using System.Text.RegularExpressions;

public partial class Whir_System_ModuleMark_Common_FormExport : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前编辑的栏目
    /// </summary>
    protected int ColumnId { get; set; }
    /// <summary>
    /// 流程节点，只导出当前流程的文件
    /// </summary>
    private int FlowId { get; set; }
    /// <summary>
    /// 子站ID
    /// </summary>
    protected int SubjectId { get; set; }
    /// <summary>
    /// 列表上显示的表单字段
    /// </summary>
    protected Dictionary<Form, Field> DictFormInList = new Dictionary<Form, Field>();
    /// <summary>
    /// 所使用的主栏目实体
    /// </summary>
    protected Column MainColumn { get; private set; }
    /// <summary>
    /// 所使用到的主模型实体
    /// </summary>
    protected Model MainModel { get; private set; }
    /// <summary>
    /// 排序语句
    /// </summary>
    protected string OrderBy { get; set; }
    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDel { get; set; }
    /// <summary>
    /// 条件语句
    /// </summary>
    public string Where { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        FlowId = RequestUtil.Instance.GetQueryInt("flowid", 0);
        IsDel = Whir.ezEIP.BasePage.RequestString("IsDel").ToBoolean();
        if (!IsPostBack)
        {
            if (ColumnId > 1)
                JudgeOpenPagePermission(IsRoleHaveColumnRes("导出", ColumnId, SubjectId == 0 ? -1 : SubjectId));
            BindFormInList();
        }
    }

    //绑定此栏目的表单
    private void BindFormInList()
    {
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

                if (field.IsHidden)
                    continue;
                dictFormAndField.Add(form, field);
            }
        }

        rptFormInList.ItemDataBound += new RepeaterItemEventHandler(rptFormInList_ItemDataBound);
        rptFormInList.DataSource = dictFormAndField;
        rptFormInList.DataBind();
    }

    void rptFormInList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            KeyValuePair<Form, Field> dataItem = (KeyValuePair<Form, Field>)e.Item.DataItem;

            CheckBox cbxSelected = e.Item.FindControl("cbxSelected") as CheckBox;
            if (cbxSelected != null)
            {
                cbxSelected.Attributes.Add("FieldID", dataItem.Value.FieldName);
                cbxSelected.Attributes.Add("FormFieldAlias", dataItem.Key.FieldAlias);
            }
        }
    }

    //点击导出
    protected void Save_Click(object sender, EventArgs e)
    {
        Dictionary<string, string> dictExportColumns = new Dictionary<string, string>();

        foreach (RepeaterItem item in rptFormInList.Items)
        {
            CheckBox cbxSelected = item.FindControl("cbxSelected") as CheckBox;
            if (cbxSelected != null)
            {
                if (cbxSelected.Checked)
                {
                    dictExportColumns.Add(cbxSelected.Attributes["FieldID"], cbxSelected.Attributes["FormFieldAlias"]);
                }
            }
        }


        int activityId = RequestUtil.Instance.GetQueryInt("flowid", 0);//当前页面的工作流节点

        if (Where.IsEmpty())
        {
            Where = " AND ISDEL=0 ";
        }

        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
        if (column.WorkFlow > 0)
        {
            if (activityId != 0)
            {
                List<AuditActivity> auditActivityList = ServiceFactory.AuditActivityService.GetListBySort(column.WorkFlow);
                if (auditActivityList.Count > 0)
                {
                    if (auditActivityList[0].ActivityId == activityId)
                    {
                        string stateIds = string.Empty;
                        foreach (AuditActivity activity in auditActivityList)
                        {
                            if (activity.ActivityId != activityId)
                            {
                                stateIds += activity.ActivityId + ",";
                            }
                        }
                        stateIds = stateIds.TrimEnd(',');

                        if (stateIds != string.Empty)
                        {
                            Where += string.Format(" AND (state NOT IN({0},-1,-2) OR State IS NULL)", stateIds);
                        }
                        else
                        {
                            Where += string.Format(" AND (state NOT IN(-1,-2) OR State IS NULL)");
                        }
                    }
                    else
                    {
                        Where += string.Format(" AND state={0}", activityId);
                    }
                }
            }
        }
        if (SubjectId > 0)
        {
            Where += " AND SubjectId={0}".FormatWith(SubjectId);
        }


        GetValueForProperties();
        string Filter = filter.Value;
        Dictionary<string, string> searchDic = ToDictionary(Filter);


        var parms = new List<object>();

        if (MainColumn == null || MainModel == null)
        {
            return;
        }
        if (DictFormInList.Count > 0)
        {
            int i = 0;
            foreach (var formInList in DictFormInList)
            {
                foreach (var kv in searchDic)
                {
                    if (kv.Key.ToLower() == formInList.Value.FieldName.ToLower())
                    {
                        switch ((FieldType)formInList.Value.FieldType)
                        {
                            case FieldType.DateTime:
                                {
                                    if (kv.Value.IndexOf("<&&<", StringComparison.Ordinal) > -1)
                                    {
                                        Where += " and {0} between @{1} and @{2} ".FormatWith(kv.Key,
                                            i,
                                            i + 1);
                                        parms.Add(Regex.Split(kv.Value, "<&&<")[0]);
                                        i++;
                                        parms.Add(Regex.Split(kv.Value, "<&&<")[1]);
                                        i++;
                                    }
                                }
                                break;
                            case FieldType.Number:
                            case FieldType.Money:
                                {
                                    if (kv.Value.IndexOf("<&&<", StringComparison.Ordinal) > -1)
                                    {
                                        Where += " and {0} between @{1} and @{2} ".FormatWith(kv.Key,
                                            i,
                                            i + 1);
                                        parms.Add(Regex.Split(kv.Value, "<&&<")[0]);
                                        i++;
                                        parms.Add(Regex.Split(kv.Value, "<&&<")[1]);
                                        i++;
                                    }
                                }
                                break;
                            case FieldType.Area:
                                {
                                    Where += " and {0} = '{1}' ".FormatWith(kv.Key, kv.Value);
                                    parms.Add(kv.Value);
                                    i++;
                                }
                                break;
                            case FieldType.ListBox:
                                {
                                    var formOption = ServiceFactory.FormOptionService.GetFormOptionByFormID(formInList.Key.FormId.ToInt());
                                    if (formOption.BindType == 3 || formOption.BindType == 4)//绑定多级类别
                                    {
                                        List<string> ids = GetCategoryIds(kv.Value, formOption);
                                        ids.Add(kv.Value);
                                        if (ids.Count > 1)
                                        {
                                            Where += " and (";
                                            foreach (var id in ids)
                                            {
                                                Where += "','+{0}+',' like '%,{1},%' or ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), id);
                                            }
                                            Where = Where.Substring(0, Where.Length - 3) + ")";
                                        }
                                        else
                                            Where += " and ','+{0}+',' like '%,{1},%' ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), kv.Value);
                                    }
                                    else
                                    {
                                        Where += " and {0} = @{1} ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), i);
                                        parms.Add(kv.Value);
                                        i++;
                                    }
                                }
                                break;
                            case FieldType.Bool:
                                {
                                    Where += " and {0} = @{1} ".FormatWith(kv.Key, i);
                                    parms.Add(kv.Value);
                                    i++;
                                }
                                break;
                            default:
                                Where += " and {0} like '%'+@{1}+'%' ".FormatWith(kv.Key, i);
                                parms.Add(kv.Value);
                                i++;
                                break;

                        }
                    }
                }
            }
        }
        else
        {
            return;
        }
        DataTable table = ServiceFactory.GridViewService.GetExportTable(ColumnId, Where, dictExportColumns, parms);
        ExcelUtil.CreateExcel(table, ColumnName + ".xls");
    }

    #region  GetList()附带方法
    private Dictionary<string, string> ToDictionary(string str)
    {
        if (str.IsEmpty())
            return new Dictionary<string, string>();
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Deserialize<Dictionary<string, string>>(str);
    }

    //为公用属性赋值
    private void GetValueForProperties()
    {
        //赋值主栏目实体
        MainColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);

        //赋值主模型实体
        if (MainColumn != null)
            MainModel = ServiceFactory.ModelService.SingleOrDefault<Model>(MainColumn.ModelId);

        //赋值列表上显示的表单字段
        IList<Form> listFormListShow = ServiceFactory.FormService.GetListShowByColumnId(ColumnId);
        foreach (Form formListShow in listFormListShow)
        {
            Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(formListShow.FieldId);
            if (field != null)
                DictFormInList.Add(formListShow, field);
        }

        //根据URL参数的排序
        string queryOrderby = RequestUtil.Instance.GetQueryString("orderby").ToLower();
        string queryOrderType = RequestUtil.Instance.GetQueryString("ordertype").ToLower();
        if (!queryOrderby.IsEmpty())
        {
            queryOrderType = queryOrderType.IsEmpty() ? "desc" : queryOrderType;
            OrderBy = "{0} {1}".FormatWith(queryOrderby, queryOrderType);
        }
    }

    /// <summary>
    /// 获取所选的分类以及下级的所有id
    /// </summary>
    /// <returns></returns>
    private List<string> GetCategoryIds(string value, FormOption formOption)
    {
        List<string> ids = new List<string>();

        if (formOption.BindType == 3)//绑定多级类别
        {
            var tableName = formOption.BindTable;
            var valueField = formOption.BindValueField;
            var textField = formOption.BindTextField;
            var key = formOption.BindKeyId.ToStr();
            var subjectID = 0;

            string sql = "SELECT {0},{1},ParentId FROM {2} WHERE IsDel=0 AND TypeID IN({3}) {4} AND ParentId={5} ORDER BY Sort DESC, CreateDate DESC".FormatWith(
                 valueField,
                 textField,
                 tableName,
                 key,
                 subjectID != -99999 ? "AND SubjectID=" + subjectID : "",
                 value
            );
            var list = DbHelper.CurrentDb.Query<string>(sql, value).ToList();
            foreach (var id in list)
            {
                ids.Add(id);
                ids.AddRange(GetCategoryIds(id, formOption));
            }
        }
        else if (formOption.BindType == 4)//绑定单独分类
        {
            string sql = "SELECT {0}_PID FROM {0} WHERE  IsDel=0 AND ParentId=@0 ".FormatWith(MainModel.TableName + "_Category");
            var list = DbHelper.CurrentDb.Query<string>(sql, value).ToList();
            foreach (var id in list)
            {
                ids.Add(id);
                ids.AddRange(GetCategoryIds(id, formOption));
            }
        }
        return ids;
    }
    #endregion
}