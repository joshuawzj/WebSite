using System;

using Whir.Repository;
using Whir.Framework;

public partial class label_member_getAreaParentPath : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int areaId = RequestUtil.Instance.GetQueryInt("id", 0);
        string parentPath = ",0,";
        if (areaId > 0)
        {
            parentPath = DbHelper.CurrentDb.ExecuteScalar<object>("SELECT ParentPath FROM Whir_Cmn_Area WHERE ID=@0", areaId).ToStr();
        }
        Response.Write(parentPath);
    }
}