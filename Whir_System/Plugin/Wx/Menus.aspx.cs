using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;

public partial class Whir_System_Plugin_Wx_Menus : WxBasePage {

    protected void Page_Load(object sender, EventArgs e) {
        JudgePagePermission(IsCurrentRoleMenuRes("401"));
    }
}