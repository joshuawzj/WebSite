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
using Whir.Framework;

public partial class Shop_member_password : Shop.Common.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            WebUser.IsLogin("shop/member/login.aspx");
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string strOldPwd = txtOldPassword.Text;//旧密码
        string strNewPwd = txtNewPassWord.Text;//新密码
        bool isSuccess = false;

        int userId = WebUser.GetUserValue("Whir_Mem_Member_PID").ToInt();//主键
        string msg = "";
        //获取记录
        int record = DbHelper.CurrentDb.ExecuteScalar<int>(
            "SELECT Count(*) From Whir_Mem_Member WHERE Whir_Mem_Member_PID=@0 AND Password=@1",
            userId,
            TripleDESUtil.Encrypt(strOldPwd));
        if (record != 1)
        {
            msg = "原密码不正确";
        }
        else
        {
            strNewPwd = TripleDESUtil.Encrypt(strNewPwd);//加密
            int count = DbHelper.CurrentDb.Execute("UPDATE Whir_Mem_Member SET Password=@0 WHERE Whir_Mem_Member_PID=@1", strNewPwd, userId);
            msg = count > 0 ? "密码修改成功" : "密码修改失败";
            isSuccess = count > 0;
        }

        string script = "<script language=\"javascript\" defer=\"defer\">alert('{0}');{1}</script>".FormatWith(msg, isSuccess ? "location.href='login.aspx';" : "");
        Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
    }
}