using System;

public partial class label_member_islogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool isLogin = WebUser.IsLogin();
        string userName = WebUser.GetUserValue("LoginName");

        Response.Write("{'islogin':'" + (isLogin ? "1" : "0") + "','username':'" + userName + "'}");
    }
}