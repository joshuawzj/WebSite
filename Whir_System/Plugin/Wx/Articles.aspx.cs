﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Whir_System_Plugin_Wx_Articles : WxBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("394"));
    }
}