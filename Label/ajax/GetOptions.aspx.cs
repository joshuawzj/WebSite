/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：GetOptions.aspx.cs
 * 文件描述：webform表单动获取选项字段控件
 */

using System;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Generic;

public partial class label_ajax_GetOptions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //字段表单ID
        int formId = RequestUtil.Instance.GetQueryInt("formid", 0);
        bool isShowFirstOption = RequestUtil.Instance.GetQueryString("isshowfirstoption").ToBoolean();
        string firstOption = RequestUtil.Instance.GetQueryString("firstoption");
        if (formId > 0)
        {
            Form form = ServiceFactory.FormService.SingleOrDefault<Form>(formId);
            if (form != null)
            {
                Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(form.FieldId);//Field
                FormOption formOption = ServiceFactory.FormOptionService.GetFormOptionByFormID(form.FormId);//FormOption选项
                if (field != null && formOption != null)
                {
                    //下拉列表选项集合
                    var optionItems = ServiceFactory.DynamicFormService.GetOptions(field, form, formOption);
                    string optionType = "1";//下拉列表
                    StringBuilder OptionsBuilder = new StringBuilder();
                    if (formOption.SelectedType == 1 || formOption.SelectedType == 5)//下拉框
                    {
                        OptionsBuilder.AppendFormat("<select id=\"{0}\" name=\"{0}\">", form.FieldId);
                        if (isShowFirstOption)
                        {
                            OptionsBuilder.AppendFormat("<option value=\"\">{0}</option>", firstOption);
                        }
                        foreach (var item in optionItems)
                        {
                            OptionsBuilder.AppendFormat("<option value=\"{0}\">{1}</option>", item.Value, item.Text);
                        }
                        OptionsBuilder.AppendFormat("</select>");
                    }
                    else if (formOption.SelectedType == 2)//单选
                    {
                        optionType = "2";
                        for (int i = 0; i < optionItems.Count; i++)
                        {
                            //第一项选中
                            OptionsBuilder.AppendFormat("<label><input GroupName=\"{0}\" name=\"{0}\" type=\"radio\" value=\"{1}\"/>{2}</label>\n",
                                form.FieldId, optionItems[i].Value, optionItems[i].Text);
                        }
                    }
                    else if (formOption.SelectedType == 3|| formOption.SelectedType == 6)//多选
                    {
                        optionType = "3";
                        foreach (var item in optionItems)
                        {
                            OptionsBuilder.AppendFormat("<label><input name=\"{0}\" type=\"checkbox\"  value=\"{1}\"/>{2}</label>",
                                form.FieldId, item.Value, item.Text.Replace("└","").Replace("─", "").Replace("├", "").Replace("│", "").Replace("　", ""));
                        }
                    }
                    else if (formOption.SelectedType == 4)//二级联动
                    {
                    }
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>(optionType, OptionsBuilder.ToStr());
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    Response.Write(jss.Serialize(pair));
                    Response.End();
                }
            }
        }
    }
}