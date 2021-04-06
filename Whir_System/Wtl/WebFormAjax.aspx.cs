using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;

public partial class whir_system_wtl_WebFormAjax : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<SubmitForm> submitForms = ServiceFactory.SubmitFormService.GetList().ToList();
        Response.Write(submitForms.ToJson());
        Response.End();
    }
}