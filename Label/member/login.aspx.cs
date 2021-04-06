/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：login.cs
* 文件描述：会员异步登录处理页面
*/
using System;
using Whir.ezEIP.Web.HttpHandlers;

using Whir.Framework;
using Whir.Repository;

public partial class label_member_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string loginName = RequestUtil.Instance.GetFormString("LoginName");//用户名
        string password = RequestUtil.Instance.GetFormString("Password");//密码
        bool autoLogin = RequestUtil.Instance.GetFormString("autologin").ToBoolean();//下次自动登录

        var validator = Request.Form["Validator"];
        if (validator != null)//验证验证码
        {
            if (this.Session[CheckCodeHandler.CheckCode_Key] == null
                || !this.Session[CheckCodeHandler.CheckCode_Key].ToString().Equals(validator.ToStr(), StringComparison.OrdinalIgnoreCase))
            {
                //验证码无效
                //请输入正确的验证码
                Response.Write(-1);
                Response.End();
                return;
            }
        }
        //会员是否开启工作流判断
        int workFlowId = DbHelper.CurrentDb.ExecuteScalar<object>("SELECT WorkFlow From Whir_Dev_Column WHERE ColumnId=@0 and IsDel=0", 1).ToInt();
        //cookies保存时间
        int cookiesTime = autoLogin ? (24 * 7) : WebUser.CookiesSaveTime;
        string state = WebUser.Login(loginName, password, cookiesTime, workFlowId > 0);
        Response.Write(state);//返回状态，客户端处理（多语言提示）
        Response.End();
    }


}