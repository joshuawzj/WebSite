
using System;


public partial class Whir_System_Plugin_FrontFrame_Icons : Whir.ezEIP.Web.SysManagePageBase
{ 
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
 
}
