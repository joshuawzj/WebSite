using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;
using Whir.Security;
using Whir.Security.Service;

public partial class whir_system_Login : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //获取url中的PageUrl用于登录后跳转
        string pageUrl = RequestUtil.Instance.GetString("PageUrl");
        pageUrl = HttpUtility.UrlEncode(pageUrl);
        if (pageUrl == "")
            new LoginService().CheckLoginPage();
        else
            new LoginService().CheckLoginPage(pageUrl);
    }
}