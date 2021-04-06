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
using System.Web.UI;
using Whir.Repository;
using Whir.ezEIP.Web.HttpHandlers;

public partial class Shop_member_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (WebUser.IsLogin())
            {
                Response.Redirect("personal.aspx");
            }
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string msg = "";//提示
        if (this.Session[CheckCodeHandler.CheckCode_Key] == null)
        {
            msg = "验证码无效";
        }
        else
        {
            string checkCode = this.Session[CheckCodeHandler.CheckCode_Key].ToString();
            string codeStr = txtCode.Text.Trim();

            if (!checkCode.Equals(codeStr, StringComparison.OrdinalIgnoreCase))
            {
                msg = "验证码不正确";
            }
            else
            {
                string userName = txtUserName.Text.Trim();
                string pwd = txtPassword.Text.Trim();

                int workFlow = DbHelper.CurrentDb.ExecuteScalar<int>("SELECT WorkFlow From Whir_Dev_Column WHERE ColumnID=@0 and IsDel=0", 1);
                string state = WebUser.Login(userName, pwd, 2, workFlow > 0);
                switch (state)
                {
                    case "0":
                        msg = "用户名或密码错误";
                        break;
                    case "1":
                        msg = "帐号未审核";
                        break;
                    case "2"://登录成功
                        Response.Redirect("personal.aspx");
                        return;
                }
            }
        }
        string script = "<script language=\"javascript\" defer=\"defer\">alert('" + msg + "');$('#" + txtCode.ClientID + "').val('');</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
    }
}