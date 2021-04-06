/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：changeskin.aspx.cs
* 文件描述：异步更新皮肤。 
*/
using System;

using Whir.Framework;
using Whir.Security;
using Whir.Security.Service;
using Whir.Security.Domain;

public partial class whir_system_ajax_changeskin : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string SkinName = RequestUtil.Instance.GetQueryString("skinname");
        if (SkinName == "blue" || SkinName == "green" || SkinName == "gray" || SkinName == "yellow")
        {
            if (AuthenticateHelper.User != null)
            {
                
                ServiceFactory.UsersService.Update<Users>("SET SystemSkin=@0 WHERE UserId=@1", SkinName, AuthenticateHelper.User.UserId);

                Users Model = ServiceFactory.UsersService.SingleOrDefault<Users>(AuthenticateHelper.User.UserId);
                if (Model != null)
                {
                    //更新缓存
                    LoginUser user = new LoginUser(Model);
                    AuthenticateHelper.SetUserCache(user);
                    Response.Write("true");//成功
                }
                else
                {
                    Response.Write("false");//失败
                }
               
            }
        }
    }
}