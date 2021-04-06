/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：getshopcategorylist.cs
 * 文件描述：
 * 
 * 创建标识: 
 * 
 * 修改标识：
 */
using System;
using System.Data;

using Whir.Framework;
using Whir.Repository;

public partial class whir_system_ajax_common_getshopcategorylist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string result = "";
        int parentid = RequestUtil.Instance.GetQueryInt("id", 0);
        int lcid = RequestUtil.Instance.GetString("lcid").ToInt();
        string SQL = "SELECT CategoryID,CategoryName,ParentID FROM Whir_Shop_Category " +
            "WHERE IsDel=0  AND ParentID=@0 AND CategoryID!=@1 ORDER BY Sort DESC, CreateDate DESC";
        DataTable table = DbHelper.CurrentDb.Query(SQL, parentid, lcid).Tables[0];

        result = "[";
        for (int i = 0; i < table.Rows.Count; i++)
        {
            string optionValue = table.Rows[i]["CategoryID"].ToStr();
            string optionText = table.Rows[i]["CategoryName"].ToStr();

            result += "{";
            result += "id:'{0}',".FormatWith(optionValue);
            result += "name:'{0}'".FormatWith(optionText);


            //是否有下级
            string childSQL = "SELECT CategoryID,CategoryName,ParentID FROM Whir_Shop_Category " +
                "WHERE IsDel=0  AND ParentID=@0 AND CategoryID!=@1 ORDER BY Sort DESC, CreateDate DESC";
            DataTable childTable = DbHelper.CurrentDb.Query(childSQL, optionValue, lcid).Tables[0];
            if (childTable.Rows.Count > 0)
                result += ",isParent:true";


            result += "},";
        }
        result = result.TrimEnd(',');
        result += "]";
        Response.Write(result);
    }
}