/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：showformlist.aspx.cs
 * 文件描述：列表项的字段选择页面
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;

public partial class Whir_System_ModuleMark_Common_ShowFormList : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前编辑的栏目
    /// </summary>
    protected int ColumnId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);

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

                if (field.IsEnableListShow == -1)
                    continue;
                dictFormAndField.Add(form, field);
            }
        }

        rptFormInList.DataSource = dictFormAndField;
        rptFormInList.DataBind();
    }

   
}