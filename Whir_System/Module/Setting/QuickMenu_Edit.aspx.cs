
using System;
using Whir.Config;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;

public partial class Whir_System_Module_Setting_QuickMenu_Edit : Whir.ezEIP.Web.SysManagePageBase
{
  
    public QuickMenu CurrentMenu { get; set; }

    
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("379"));
        var Id = RequestInt32("Id");
        CurrentMenu = ServiceFactory.QuickMenuService.SingleOrDefault<QuickMenu>(Id) ??
                             ModelFactory<QuickMenu>.Insten();
        

    }
}