using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;

public partial class whir_system_wtl_ColumnAjax : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int columnId = Request.QueryString["columnid"].ToInt();
        List<Field> field = ServiceFactory.FieldService.GetByColumnID(columnId);



        Response.Write(field.ToJson());
        Response.End();
    }
}