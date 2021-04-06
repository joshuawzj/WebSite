/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：only.aspx.cs
 * 文件描述：webform表单动判断值是否唯一
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Repository;

public partial class label_ajax_only : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int formId = RequestUtil.Instance.GetQueryInt("formid", 0);
        int fieldId = RequestUtil.Instance.GetQueryInt("fieldid", 0);
        string onlyValue = RequestUtil.Instance.GetQueryString("onlyvalue");

        int isExist = 1;
        if (!onlyValue.IsEmpty())
        {
            Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(fieldId);
            Form form = ServiceFactory.FormService.SingleOrDefault<Form>(formId);

            if (form != null && field != null)
            {
                string SQL = "SELECT TableName FROM whir_dev_column wdc INNER JOIN whir_dev_model wdm ON wdc.modelid=wdm.modelid WHERE columnid=@0  and wdc.IsDel=0";

                string tableName = DbHelper.CurrentDb.ExecuteScalar<string>(SQL, form.ColumnId);

                if (!tableName.IsEmpty())
                {
                    string SQL2 = "SELECT COUNT(*) FROM {0} WHERE {1}=@0 AND TypeID=@1".FormatWith(tableName, field.FieldName, form.ColumnId);
                    int count = DbHelper.CurrentDb.ExecuteScalar<int>(SQL2, onlyValue, form.ColumnId);
                    isExist = count > 0 ? 1 : 0;
                }
            }
        }
        Response.Write(isExist);
        Response.End();
    }
}