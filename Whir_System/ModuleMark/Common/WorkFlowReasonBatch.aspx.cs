using System;
using Whir.Framework;
using System.Web.UI.WebControls;

public partial class Whir_System_ModuleMark_Common_WorkFlowReasonBatch : Whir.ezEIP.Web.SysManagePageBase
{
    protected int SubjectId = RequestUtil.Instance.GetQueryInt("SubjectId", 0);
    protected string Keys = RequestUtil.Instance.GetQueryString("cbPosition");

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("374"));
    }
}