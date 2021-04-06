/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：Error.aspx.cs
* 文件描述：错误页面。 
*/

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Whir.Framework;

public partial class whir_system_Error : System.Web.UI.Page
{
    /// <summary>
    /// 显示信息
    /// </summary>
    protected string Msg { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        Msg = RequestUtil.Instance.GetString("msg");
        var code = RequestUtil.Instance.GetString("Code").ToInt(500);
        Response.StatusCode = code;
    }
}