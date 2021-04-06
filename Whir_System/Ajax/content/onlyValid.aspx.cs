/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：onlyValid.aspx.cs
* 文件描述：异步验证是否唯一 
*/

using System;
using System.Data;

using Whir.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

public partial class whir_system_ajax_content_onlyValid : System.Web.UI.Page
{
    public int ColumnId { get; set; }
    public int PrimaryValue { get; set; }
    public string FieldName { get; set; }
    public string FieldValue { get; set; }


    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        PrimaryValue = RequestUtil.Instance.GetQueryInt("primaryValue", 0);
        FieldName = RequestUtil.Instance.GetQueryString("fieldname");
        FieldValue = RequestUtil.Instance.GetQueryString("fieldvalue");
        
        if (FieldName.IsSafeSqlaParms())
            Response.Write("存在非法字符串！");
        else
            WriteExist();
    }

    private void WriteExist()
    {
        Model model = ServiceFactory.ModelService.GetModelByColumnId(ColumnId);
        if (model == null || FieldValue.IsEmpty())
        {
            Response.Write("{\"valid\":true}");
            return;
        }

        string sql = "SELECT COUNT(1) FROM {0} WHERE {1}=@0 AND {0}_PID<>@2 AND TypeID=@1".FormatWith(model.TableName, FieldName);
        int count = DbHelper.CurrentDb.ExecuteScalar<object>(sql, FieldValue, ColumnId, PrimaryValue).ToInt();
        if (count > 0)
        {
            Response.Write("{\"valid\":false}");
            return;
        }

        Response.Write("{\"valid\":true}");
        return;
    }
}