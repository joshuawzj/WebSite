/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：adminchangepwd.aspx.cs
* 文件描述：管理员修改密码页面。 
*/

using System;

using Whir.Security;
using Whir.Security.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Security.Service;

public partial class whir_system_module_security_adminchangepwd : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 
    /// </summary>
    protected Users CurrenUsers { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        CurrenUsers = ServiceFactory.UsersService.SingleOrDefault<Users>(CurrentUserId);
    }
}