/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：selectfield.aspx.cs
 * 文件描述：列表项的字段选择页面
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;

public partial class Whir_System_ModuleMark_Common_SelectField : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前编辑的栏目
    /// </summary>
    protected int ColumnId { get; set; }
    /// <summary>
    /// 子站和专题ID
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 编辑的信息ID
    /// </summary>
    protected string ItemIds { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
            SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
            ItemIds = RequestUtil.Instance.GetQueryString("itemIds");
            BindFormInList();
        }
    }

    //绑定此栏目的表单
    private void BindFormInList()
    {
        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
        if (column == null) return;

        Dictionary<Form, Field> dictFormAndField = new Dictionary<Form, Field>();
        IList<Form> listForm = ServiceFactory.FormService.GetListByColumnId(ColumnId);
        listForm = listForm.Where(p => !p.IsOnly).ToList();
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

                dictFormAndField.Add(form, field);
            }
        }

        rptFormInList.ItemDataBound += rptFormInList_ItemDataBound;
        rptFormInList.DataSource = dictFormAndField;
        rptFormInList.DataBind();
    }

    void rptFormInList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            KeyValuePair<Form, Field> dataItem = (KeyValuePair<Form, Field>)e.Item.DataItem;

            CheckBox cbxIsListShow = e.Item.FindControl("cbxIsListShow") as CheckBox;
            if (cbxIsListShow != null)
            {
                cbxIsListShow.Checked = dataItem.Key.IsListShow;
                cbxIsListShow.Attributes.Add("FormId", dataItem.Key.FormId.ToStr());

                if (dataItem.Value.IsEnableListShow == 0)
                    cbxIsListShow.Enabled = false;
            }
        }
    }

    /// <summary>
    /// 点击确定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Save_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in rptFormInList.Items)
        {
            CheckBox cbxIsListShow = item.FindControl("cbxIsListShow") as CheckBox;
            if (cbxIsListShow != null)
            {
                int formID = cbxIsListShow.Attributes["FormId"].ToInt();
                bool isListShow = cbxIsListShow.Checked;

                ServiceFactory.FormService.ModifyListShow(formID, isListShow);
            }
        }

        string script = "<script>callback();</script>";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", script);
    }
}