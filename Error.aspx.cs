
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;

public partial class whir_Error : System.Web.UI.Page
{
    /// <summary>
    /// 显示信息
    /// </summary>
    protected string Msg { get; set; }

    /// <summary>
    /// 错误代码
    /// </summary>
    protected int Code { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        Msg = HttpUtility.UrlDecode(RequestUtil.Instance.GetString("msg"));
        if (Msg.IsEmpty())
        {
            Msg = "系统请求错误，请联系管理员！";
        }
        Code = RequestUtil.Instance.GetString("Code").ToInt(500);
        //如果要显示具体的错误信息屏蔽以下代码
        //Response.StatusCode = Code;
    }
}