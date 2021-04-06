using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.ezEIP.Web;

public partial class Whir_System_Module_Release_generationwebsites : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgeOpenPagePermission(new SysManagePageBase().IsCurrentRoleMenuRes("363"));
    }
}