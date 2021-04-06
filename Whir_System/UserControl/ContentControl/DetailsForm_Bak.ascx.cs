/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：DetailsForm_Bak.ascx.cs
 * 文件描述：历史记录通用详情查看页面
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Globalization;
using System.Data;
using System.Web.UI;
using System.Reflection;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.ezEIP.Web;
using Whir.Language;

public partial class whir_system_UserControl_ContentControl_DetailsForm_Bak : SysControlBase
{
    /// <summary>
    /// 栏目ID
    /// </summary>
    public int ColumnId { get; set; }

    /// <summary>
    /// 编辑状态下的主键ID
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// 表单类型
    /// </summary>
    public DynamicFormType FormType { get; set; }

    /// <summary>
    /// 当前显示的数据行
    /// </summary>
    protected DataRow ShowRow { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowRow = ServiceFactory.DynamicFormService.GetBakEditRow(ColumnId, ItemId);
        }

        BindForm();
    }

    //绑定表单
    private void BindForm()
    {
        IList<Form> listForm;

        if (FormType == DynamicFormType.Left)
        {
            listForm = ServiceFactory.FormService.GetMainListByColumnId(ColumnId);
            rptFormLeft.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(rptForm_ItemDataBound);
            rptFormLeft.DataSource = listForm;
            rptFormLeft.DataBind();
        }
        else if (FormType == DynamicFormType.Right)
        {
            listForm = ServiceFactory.FormService.GetAttachListByColumnId(ColumnId);
            rptFormRight.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(rptForm_ItemDataBound);
            rptFormRight.DataSource = listForm;
            rptFormRight.DataBind();
        }
    }
    void rptForm_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            var divgroup = e.Item.FindControl("divgroup") as HtmlGenericControl;
            var cell = e.Item.FindControl("tdDynamicFormInput") as HtmlGenericControl;
            if (cell == null) return;

            Form form = e.Item.DataItem as Form;
            if (form == null) return;

            Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(form.FieldId);
            if (field == null) return;

            if (field.IsHidden)
                divgroup.Style.Add("display", "none");

            switch ((FieldType)field.FieldType)
            {
                case FieldType.MultipleHtmlText:
                    {
                        #region 编辑器
                        cell.InnerHtml = ShowRow[field.FieldName].ToStr();
                        #endregion 编辑器
                    }
                    break;
                case FieldType.ListBox:
                    {
                        #region 选项

                        if (ShowRow != null)
                            cell.InnerHtml = ServiceFactory.DynamicFormService.GetOptionText(form.FormId, ShowRow[field.FieldName].ToStr());


                        #endregion 选项
                    }
                    break;
                case FieldType.DateTime:
                    {
                        #region 日期

                        FormDate formDate = ServiceFactory.FormDateService.GetFormDateByFormID(form.FormId);
                        if (formDate != null)
                        {
                            if (ShowRow != null)
                            {
                                //编辑已有信息
                                cell.InnerHtml = ShowRow[field.FieldName].ToDateTime().ToString(formDate.DateFormat.ToDateTimeFormat());
                            }
                        }
                        else
                        {
                            cell.InnerHtml = ShowRow[field.FieldName].ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        #endregion
                    }
                    break;
                case FieldType.Bool:
                    {
                        #region 是/否

                        if (ShowRow != null)
                            cell.InnerHtml = ShowRow[field.FieldName].ToBoolean() ? "是".ToLang() : "否".ToLang();


                        #endregion 是/否
                    }
                    break;
                case FieldType.Picture:
                    {
                        #region 图片

                        if (ShowRow != null && ShowRow[field.FieldName].ToStr() != "")
                        {
                            foreach (string imageValue in ShowRow[field.FieldName].ToStr().Split('*'))
                            {
                                cell.InnerHtml += "<a href='{0}' target='_blank'><img src='{0}'  width='100' height='100' style='margin-right:5px;float:left;'></a>".FormatWith(
                                        UploadFilePath + imageValue, SysPath
                                    );
                            }
                        }

                        #endregion 图片
                    }
                    break;
                case FieldType.Video:
                case FieldType.File:
                    {
                        #region 视频/文件

                        if (ShowRow != null)
                        {
                            foreach (string fileValue in ShowRow[field.FieldName].ToStr().Split('*'))
                            {
                                cell.InnerHtml += "<a href='{0}' target='_blank'>{1}</a><br/>".FormatWith(
                                        UploadFilePath + fileValue, Whir.Service.ServiceFactory.UploadService.GetFileName(fileValue)
                                    );
                            }
                        }

                        #endregion 视频/文件
                    }
                    break;
                case FieldType.Area:
                    {
                        #region 地区

                        if (ShowRow != null)
                        {
                            string areaValue = ShowRow[field.FieldName].ToStr();
                            cell.InnerHtml = ServiceFactory.AreaService.GetParentsName(areaValue.ToInt());
                        }

                        #endregion 地区
                    }
                    break;
                case FieldType.PassWord:
                    {
                        #region 密码型字段
                        if (ShowRow != null)
                            cell.InnerHtml = "●●●●●●";


                        #endregion 密码型字段
                    }
                    break;
                case FieldType.Text:
                    {
                        #region 单行文本


                        if (form.IsBold || form.IsColor)
                        {
                            string text = "<a style='{0} {1}'>{2}</a>";
                            string bold = ShowRow[field.FieldName + "_Bold"].ToBoolean() ? "font-weight:bold;" : "";
                            string color = ShowRow[field.FieldName + "_Color"].ToStr().IsEmpty() ? "" : "color:{0}".FormatWith(ShowRow[field.FieldName + "_Color"]);
                            cell.InnerHtml = text.FormatWith(bold,
                                color,
                                ShowRow == null ? form.DefaultValue : ShowRow[field.FieldName]
                                );
                        }
                        else
                        {
                            if (ShowRow != null)
                                cell.InnerHtml = ShowRow[field.FieldName].ToStr();
                            else
                                cell.InnerHtml = form.DefaultValue;
                        }


                        #region 固定表单字段
                        switch (field.FieldName.ToLower())
                        {
                            case "typeid"://栏目ID
                                cell.InnerHtml = ColumnId.ToStr();
                                break;
                            case "subjectid"://所属子站ID
                                cell.InnerHtml = RequestUtil.Instance.GetQueryInt("SubjectId", 0).ToStr();
                                break;
                            case "state"://状态
                                if (ShowRow != null)
                                    cell.InnerHtml = ShowRow[field.FieldName].ToStr();
                                else
                                    cell.InnerHtml = "0";
                                break;
                            case "sort"://排序
                                if (ShowRow != null)
                                    cell.InnerHtml = ShowRow[field.FieldName].ToStr();
                                else
                                    cell.InnerHtml = Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmssff", DateTimeFormatInfo.InvariantInfo)).ToStr();
                                break;
                            case "createuser"://创建人
                                if (ShowRow != null)
                                    cell.InnerHtml = ShowRow[field.FieldName].ToStr();
                                else
                                    cell.InnerHtml = CurrentUserName;
                                break;
                            case "updateuser"://更改人
                                cell.InnerHtml = CurrentUserName;
                                break;
                        }
                        #endregion 固定表单字段

                        #region URL参数值

                        //字段的默认值以"@"符号开头的, 被认为是URL传参数的值
                        if (form.DefaultValue.ToStr().StartsWith("@"))
                        {
                            string defaultValue = form.DefaultValue.TrimStart('@');
                            cell.InnerHtml = RequestUtil.Instance.GetQueryString(defaultValue);
                        }

                        #endregion URL参数值


                        #endregion 单行文本
                    }
                    break;
                default:
                    {
                        #region 单行文本


                        if (ShowRow != null)
                            cell.InnerHtml = ShowRow[field.FieldName].ToStr();

                        #endregion 单行文本
                    }
                    break;
            }


        }
    }

}