/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：admininfo.aspx.cs
* 文件描述：管理员修改个人信息页。 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Config;
using Whir.Config.Models;
using Whir.Security;
using Whir.Security.Domain;
using Whir.Framework;
using Whir.Language;

public partial class whir_system_module_security_admininfo : Whir.ezEIP.Web.SysManagePageBase
{

    protected Users CurrenUsers { get; set; }

    protected string AllowPicType { get; set; }

    protected UploadConfig UploadConfig { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //绑定管理员信息
            BindAdminInfo();
        }
    }
    /// <summary>
    /// 绑定管理员信息
    /// </summary>
    private void BindAdminInfo()
    {
        if (CurrentUserId > 0)
        {
            CurrenUsers = ServiceFactory.UsersService.SingleOrDefault<Users>(CurrentUserId);

            UploadConfig = ConfigHelper.GetUploadConfig();

            if (!UploadConfig.AllowPicType.IsEmpty())
            {
                string type = "";

                foreach (var item in UploadConfig.AllowPicType.Split('|'))
                {
                    type += "'" + item + "'" + ",";
                }
                if (type.Length > 0)
                {
                    AllowPicType = type.TrimEnd(',');

                }

            }

            if (CurrenUsers != null)
            {
                ltLoginName.Text = CurrenUsers.LoginName;

                if (string.IsNullOrEmpty(CurrenUsers.LastLoginIP))
                {
                    this.ltLastLoginIP.Text = "第一次登录".ToLang();
                    this.ltLastLoginTime.Text = "";
                }
                else
                {
                    if (CurrenUsers.LastLoginIP.ToLower().Contains("fe80") || CurrenUsers.LastLoginIP.ToLower().Contains("::1"))
                    {
                        this.ltLastLoginIP.Text = "127.0.0.1";
                    }
                    else
                    {
                        this.ltLastLoginIP.Text = CurrenUsers.LastLoginIP;
                    }
                    this.ltLastLoginTime.Text = CurrenUsers.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }
    }
   
}