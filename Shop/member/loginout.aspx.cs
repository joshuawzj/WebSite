using System;

public partial class Shop_member_loginout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WebUser.LoginOut();
        Response.Redirect("login.aspx");
    }
}