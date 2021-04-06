using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class whir_system_Handler_Developer_Jurisdiction : SysHandlerPageBase
{
    SysManagePageBase SysManagePageBase = new SysManagePageBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetFormString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveColumn()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(new SysManagePageBase().IsCurrentRoleMenuRes("329"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }

        int siteId = RequestUtil.Instance.GetString("siteId").ToInt();
        int roleId = RequestUtil.Instance.GetString("RoleId").ToInt();

        string strColumn = RequestUtil.Instance.GetString("columncheckbox");
        string strCategory = RequestUtil.Instance.GetString("CategoryData");
        string strWorkFlow = RequestUtil.Instance.GetString("columnWorkFlowCheckBox");

        Whir.Security.ServiceFactory.RolesService.UpdateRoleJurisdiction(1, siteId, roleId, strColumn, 0);

        Whir.Security.ServiceFactory.RolesService.UpdateRoleJurisdiction(5, siteId, roleId, strCategory, 0, 0);

        Whir.Security.ServiceFactory.RolesService.UpdateRoleJurisdiction(4, siteId, roleId, strWorkFlow, 0, 0);


        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 保存数据 子站、专题
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveSubjectColumn()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(new SysManagePageBase().IsCurrentRoleMenuRes("329"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int siteId = RequestUtil.Instance.GetString("siteId").ToInt();
        int roleId = RequestUtil.Instance.GetString("RoleId").ToInt();
        int subjectId = RequestUtil.Instance.GetString("subjectId").ToInt();
        int classId = RequestUtil.Instance.GetString("classid").ToInt();
        int subjectTypeId = RequestUtil.Instance.GetString("type").ToInt();

        string strColumn = RequestUtil.Instance.GetString("columncheckbox");
        string strCategory = RequestUtil.Instance.GetString("CategoryData");
        string strWorkFlow = RequestUtil.Instance.GetString("columnWorkFlowCheckBox");

        var isSame = RequestUtil.Instance.GetString("isSame").ToBoolean();

        if (subjectTypeId == 2) //专题
            Whir.Security.ServiceFactory.RolesService.UpdateRoleJurisdiction(3, siteId, subjectId, classId, roleId, strColumn, 0, subjectTypeId);
        else                   //子站
            Whir.Security.ServiceFactory.RolesService.UpdateRoleJurisdiction(2, siteId, subjectId, classId, roleId, strColumn, 0, subjectTypeId);

        Whir.Security.ServiceFactory.RolesService.UpdateRoleJurisdiction(5, siteId, subjectId, classId, roleId, strCategory, 0, subjectTypeId);

        Whir.Security.ServiceFactory.RolesService.UpdateRoleJurisdiction(4, siteId, subjectId, classId, roleId, strWorkFlow, 0, subjectTypeId);

        if (isSame)
        {
            var listSubject = ServiceFactory.SubjectService.GetListBySubjectClassId(classId);
            foreach (var item in listSubject)
            {
                if (item.SubjectId == subjectId)
                    continue;
                strColumn = RequestUtil.Instance.GetString("columncheckbox") + ",";
                strColumn = strColumn.Replace("|{1}|siteId{0}|{1},".FormatWith(siteId, subjectId), "|{1}|siteId{0}|{1},".FormatWith(siteId, item.SubjectId));
                strColumn = strColumn.Replace("siteId{0}|{1},".FormatWith(siteId, subjectId), "siteId{0}|{1},".FormatWith(siteId, item.SubjectId));
                if (subjectTypeId == 2) //专题
                    Whir.Security.ServiceFactory.RolesService.UpdateRoleJurisdiction(3, siteId, item.SubjectId, classId, roleId, strColumn, 0, subjectTypeId);
                else                   //子站
                    Whir.Security.ServiceFactory.RolesService.UpdateRoleJurisdiction(2, siteId, item.SubjectId, classId, roleId, strColumn, 0, subjectTypeId);

                strWorkFlow = RequestUtil.Instance.GetString("columnWorkFlowCheckBox") + ",";
                strWorkFlow = strWorkFlow.Replace("|{0},".FormatWith(subjectId), "|{0},".FormatWith(item.SubjectId));
                Whir.Security.ServiceFactory.RolesService.UpdateRoleJurisdiction(4, siteId, item.SubjectId, classId, roleId, strWorkFlow, 0, subjectTypeId);
            }

        }

        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }
}