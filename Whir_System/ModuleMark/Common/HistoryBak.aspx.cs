/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：historyBak.aspx.cs
 * 文件描述：历史记录备份页面
 */

using System;
using System.Linq;
using System.Collections.Generic;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;

public partial class Whir_System_ModuleMark_Common_HistoryBak : Whir.ezEIP.Web.SysManagePageBase
{
    #region 内部公用属性

    /// <summary>
    /// 栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// SubjectId
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 主键ID
    /// </summary>
    protected int ItemID { get; set; }

    /// <summary>
    /// 所使用的主栏目实体
    /// </summary>
    protected Column MainColumn { get; private set; }

    /// <summary>
    /// 所使用到的主模型实体
    /// </summary>
    protected Model MainModel { get; private set; }

    /// <summary>
    /// 列表上显示的表单字段
    /// </summary>
    protected Dictionary<Form, Field> DictFormInList = new Dictionary<Form, Field>();

    /// <summary>
    /// 主键
    /// </summary>
    protected string IdField { get; set; }

    protected string Columns { get; set; }

    #endregion 内部公用属性

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        ItemID = RequestUtil.Instance.GetQueryInt("itemid", 0);
        JudgeOpenPagePermission(IsRoleHaveColumnRes("历史记录", ColumnId, SubjectId == 0 ? -1 : SubjectId));
        GetValueForProperties();

    }

    //为公用属性赋值
    private void GetValueForProperties()
    {
        //赋值主栏目实体
        MainColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);

        //赋值主模型实体
        if (MainColumn != null)
        {
            MainModel = ServiceFactory.ModelService.SingleOrDefault<Model>(MainColumn.ModelId);
            IdField = MainModel.TableName + "_Bak_PID";
        }

        //赋值列表上显示的表单字段
        IList<Form> listFormListShow = ServiceFactory.FormService.GetHistoryListShowByColumnId(ColumnId);
        listFormListShow.Add(ServiceFactory.FormService.GetHistoryUpdateDateByColumnId(ColumnId));
        foreach (Form formListShow in listFormListShow)
        {
            Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(formListShow.FieldId);
            if (field != null)
            {
                if (field.FieldName == "UpdateDate")
                {
                    field.FieldAlias = "删除时间";
                }
                DictFormInList.Add(formListShow, field);
            }
        }

        if (DictFormInList.Count > 0)
        {
            Columns += " columns: [{field: '" + IdField +
                       "',align: 'center',valign: 'middle',";
            Columns += "checkbox:true";
            Columns += "},";
            foreach (var formInList in DictFormInList)
            {
                //Columns += "{title: '" + item.Value.FieldAlias + "', field: '" + item.Value.FieldName +
                //           "',align: 'center',valign: 'middle',sortable:true},";

                #region 判断字段类型

                switch ((FieldType)formInList.Value.FieldType)
                {

                    case FieldType.Bool:
                        {
                            string json = "{\"0\": \"" + "否" + "\",\"1\": \"" + "是" + "\"}";
                            Columns += "{title: '" + formInList.Value.FieldAlias + "', field: '" +
                                       formInList.Value.FieldName +
                                       "',align: 'center',valign: 'middle',sortable:true,filterData:'json:" + json +
                                       "',filterControl:'" +
                                       formInList.Value.FieldType +
                                       "',formatter: function(value, row, index) {return GetBoolText(value, row, index);}},";

                        }
                        break;
                    case FieldType.ListBox:
                        {
                            string url = SysPath +
                                         "Handler/Common/Common.aspx?_action=GetSearchSelectOption&ColumnId=" + ColumnId +
                                         "&SubjectId=" + SubjectId + "&FormId=" + formInList.Key.FormId + "&Fieldid=" +
                                         formInList.Value.FieldId;
                            Columns += "{title: '" + formInList.Value.FieldAlias + "', field: '" +
                                       formInList.Value.FieldName +
                                       "',align: 'left',valign: 'middle',sortable:true,filterData:'url:" + url +
                                       "',filterControl:'" +
                                       formInList.Value.FieldType +
                                       "'},";
                        }
                        break;
                    case FieldType.Area:
                        {
                            Columns += "{title: '" + formInList.Value.FieldAlias + "', field: '" +
                                       formInList.Value.FieldName +
                                       "',align: 'center',valign: 'middle',sortable:true,filterControl:'" +
                                       formInList.Value.FieldType + "',leve:'" +
                                       GetAreaLeve(formInList.Key.FormId, formInList.Value.FieldType) + "'},";
                        }
                        break;
                    case FieldType.DateTime:
                        FormDate formDate = ServiceFactory.FormDateService.GetFormDateByFormID(formInList.Key.FormId);
                        if (formDate != null)
                            Columns += "{title: '" + formInList.Value.FieldAlias + "', field: '" +
                                       formInList.Value.FieldName +
                                       "',align: 'center',valign: 'middle',sortable:true,filterControl:'" +
                                       formInList.Value.FieldType + "',format:'" +
                                      formDate.DateFormat.ToDateTimeFormat() + "'," +
                                       "formatter: function(value, row, index) {return GetDateTimeFormat(value, row, index,'" + formDate.DateFormat.ToDateTimeFormat() + "');}},";
                        break;
                    default:
                        Columns += "{title: '" + formInList.Value.FieldAlias + "', field: '" +
                                   formInList.Value.FieldName +
                                   "',align: 'center',valign: 'middle',sortable:true,filterControl:'" +
                                   formInList.Value.FieldType + "'},";
                        break;

                }

                #endregion 判断字段类型
            }
            Columns += "{title: '操作',field: '" + IdField +
                           "',align: 'center',valign: 'middle',formatter: function(value, row, index) {return GetOperation(value, row, index);}}";

            Columns += "]";

        }

    }
    //获取日期格式 不用了
    public string GetFormat(int formId, int fieldType)
    {
        if (fieldType == (int)FieldType.DateTime)
        {
            FormDate formdate = ServiceFactory.FormDateService.GetFormDateByFormID(formId);
            if (formdate != null)
            {
                if (formdate.DateFormat == "yyyy-MM-dd HH:mm:ss")
                {
                    return "yyyy-mm-dd hh:ii:ss";
                }
                else
                {
                    return formdate.DateFormat.ToLower();
                }
            }
            else
            {
                return "yyyy-mm-dd";
            }
        }
        else
        {
            return "";
        }

    }

    //获取区域设置
    public int GetAreaLeve(int formId, int fieldType)
    {
        if (fieldType == (int)FieldType.Area)
        {
            FormArea formArea = ServiceFactory.FormAreaService.GetFormAreaByFormID(formId);
            if (formArea != null)
            {
                return formArea.ShowLevel;
            }
            else
            {
                return 3;
            }
        }
        else
        {
            return 3;
        }
    }

}