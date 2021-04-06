/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：
 * 文件描述：
 * 
 * 创建标识: liuyong 2012-02-07
 * 
 * 修改标识：
 */
using System;
using Whir.Framework;
using Whir.Repository;

public partial class Shop_member_ajaxonly : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string onlyValue = RequestUtil.Instance.GetQueryString("onlyvalue");
        int flag = RequestUtil.Instance.GetQueryInt("flag", 0);//0：用户名 1：Email

        int isExist = 1;
        if (!onlyValue.IsEmpty())
        {
            string SQL2 = "SELECT COUNT(*) FROM Whir_Mem_Member WHERE {0}=@0 ";
            switch (flag)
            { 
                case 0:
                    SQL2 = SQL2.FormatWith("LoginName");
                    break;
                case 1:
                    SQL2 = SQL2.FormatWith("Email");
                    break;
                default:
                    SQL2 = SQL2.FormatWith("LoginName");
                    break;
            }

            int count = DbHelper.CurrentDb.ExecuteScalar<int>(SQL2, onlyValue);
            isExist = count > 0 ? 1 : 0;
        }
        Response.Write(isExist);
        Response.End();
    }
}