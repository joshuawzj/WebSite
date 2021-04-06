/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：translater.aspx.cs
* 文件描述：中文处理辅助类。 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;

public partial class whir_system_ajax_common_chineseHelper : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Clear();
            string source = HttpUtility.UrlDecode(RequestUtil.Instance.GetQueryString("source"));
            string action = HttpUtility.UrlDecode(RequestUtil.Instance.GetQueryString("action"));
            string result = "";
            switch (action.ToLower())
            {
                case "getpinyintitle":
                    result = source.ToPinyin().ToLower();
                    break;
            }
            Response.Write(result);

            Response.End();
        }
    }
}