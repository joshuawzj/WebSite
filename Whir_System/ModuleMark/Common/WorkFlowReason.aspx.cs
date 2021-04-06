using System;
using Whir.Framework;
using System.Web.UI.WebControls;

public partial class Whir_System_ModuleMark_Common_WorkflowReason : Whir.ezEIP.Web.SysManagePageBase
{
    protected int ColumnId = RequestUtil.Instance.GetQueryInt("ColumnId",0);
    protected int SubjectId = RequestUtil.Instance.GetQueryInt("SubjectId", 0);
    protected int CurrentActivityId = RequestUtil.Instance.GetQueryInt("CurrentActivityId", 0);
    protected string Selected = RequestUtil.Instance.GetQueryString("selected");
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("374"));
    }
}