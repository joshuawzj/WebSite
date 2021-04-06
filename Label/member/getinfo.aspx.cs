using System;

public partial class label_member_getinfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write(WebUser.GetUserJson());
        Response.End();
    }
}