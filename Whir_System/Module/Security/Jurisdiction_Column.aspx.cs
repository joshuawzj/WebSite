/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：Jurisdiction_column .aspx.cs
 * 文件描述：管理员分配栏目权限
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Whir.Security.Domain;
using Whir.Service;
using Whir.Framework;
using Whir.Domain;
using Whir.Language;
using System.Text;

public partial class Whir_System_Module_Security_Jurisdiction_Column : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前站点
    /// </summary>
    protected int SiteId { get; set; }
    /// <summary>
    /// 角色ID
    /// </summary>
    protected int RoleId { get; set; }
    /// <summary>
    /// 父节点角色ID
    /// </summary>
    protected int parentRoleId { get; set; }

    /// <summary>
    /// 当前角色已有的权限
    /// </summary>
    protected List<string> ClientResources { get; set; }

    /// <summary>
    /// 父节点站点的所有栏目资源
    /// </summary>
    protected List<string> ParentAllClientColumnAndFunction { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        SiteId = RequestUtil.Instance.GetQueryInt("siteid", -1);
        RoleId = RequestUtil.Instance.GetQueryInt("roleid", -1);

        if (!IsPostBack)
        {
            JudgeOpenPagePermission(IsCurrentRoleMenuRes("329"));
            if (SiteId == -1)
            {//初始时跳到当前站点的栏目授权
                Response.Redirect("Jurisdiction_column.aspx?siteid=" + CurrentSiteId + "&type=0&roleid=" + RoleId);
            }

            //已有的栏目与功能权限 2013-8-15 增加上级角色权限范围控制
            var role = Whir.Security.ServiceFactory.RolesService.SingleOrDefault<Roles>(RoleId);
            parentRoleId = role == null ? 0 : role.ParentId.ToInt();
            ParentAllClientColumnAndFunction = Whir.Security.ServiceFactory.RolesService.GetColumnJurisdictionListByRoleId(parentRoleId);


            //当前角色拥有的权限
            ClientResources = Whir.Security.ServiceFactory.RolesService.GetColumnJurisdictionListByRoleId(RoleId);
            //累加当前角色拥有的工作流权限
            ClientResources.AddRange(Whir.Security.ServiceFactory.RolesService.GetWorkFlowJurisdictionListByRoleId(RoleId));

            //栏目列表 
            BindList();
        }
    }

    #region 绑定


    /// <summary>
    /// 绑定列表
    /// </summary>
    private void BindList()
    {
        //string FunctionName = Whir.Service.ModuleFunctionType.GetFunctionNameById(1);
        ////所有已授权给客户的站点目录，string格式“资源名称|关联ID|站点ID”
        //

        rptColumnList.DataSource = ServiceFactory.ColumnService.GetColumnList(0, RoleId, SiteId, "", true, true, 0, 0);
        rptColumnList.DataBind();

        if (SiteId != -1)//有选择要显示的栏目时才显示
        {
            Table_ColumnFunctionList.Visible = Table_ColumnList.Visible = true;
        }
 

    }



    /// <summary>
    /// 获取某一栏目的所有功能复选框<input type="checkbox"  name="columncheckbox" value="功能名|siteId站点ID|栏目id" columnid="栏目id" />功能名
    /// </summary>
    /// <param name="columnId"></param>
    /// <param name="siteId"></param>
    /// <param name="functionIds"></param>
    /// <returns></returns>
    public string GetColumnFunctionCheckBox(string columnId, string siteId, string functionIds)
    {
        string result = "";
        functionIds = functionIds.Trim(',');
        if (functionIds == "" || ServiceFactory.ColumnService.IsParentColumn(columnId.ToInt()))
        {
            //作为结构用的栏目至少有查看、复制、栏目删除可勾选
            foreach (int s in ModuleFunctionType.StuctFunctionDefault)
            {
                ////没有权限的功能飞过
                if (!ParentAllClientColumnAndFunction.Contains(string.Format("{0}|siteId{1}|{2}", ModuleFunctionType.GetFunctionNameById(s), siteId, columnId)) && parentRoleId != 1)
                { continue; }

                result += GetColumnFunctionCheckBox(columnId.ToInt(), siteId.ToInt(), s);

            }
        }
        else
        {
            foreach (string s in functionIds.Split(','))
            {
                ////没有权限的功能飞过
                if (!ParentAllClientColumnAndFunction.Contains(string.Format("{0}|siteId{1}|{2}", ModuleFunctionType.GetFunctionNameById(s.ToInt()), siteId, columnId)) && parentRoleId != 1)
                { continue; }

                result += GetColumnFunctionCheckBox(columnId.ToInt(), siteId.ToInt(), s.ToInt());

            }
        }

        return result;
    }

    /// <summary>
    /// 获取某一栏目的所有工作流节点
    /// </summary>
    /// <param name="columnId"></param> 
    /// <returns></returns>
    public string GetColumnWorkFlowCheckBox(string columnId)
    {
        var result = new StringBuilder();
        var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId.ToInt());
        var workFlowId = column.WorkFlow;
        if (workFlowId > 0)
        {
            var list = ServiceFactory.AuditActivityService.GetListBySort(workFlowId);
            result.Append(list.Count > 0 ? "<span style='color:green'>" + "工作流权限:".ToLang() + "</span>" : "");
            foreach (var auditActivity in list)
            {
                //①先判断上级角色是否有工作流节点权限
                var parentRole = Whir.Security.ServiceFactory.RolesService.SingleOrDefault<Roles>(RoleId);
                var parentHavePurview = Whir.Security.ServiceFactory.RolesService.IsRoleHaveWorkFlowJurisdiction(column.ColumnId, parentRole.ParentId.ToInt(), CurrentSiteId, -1, auditActivity.ActivityId);
                if (parentHavePurview)
                {
                    //②再判断当前角色是否有工作流节点权限
                    string checkedStr = ClientResources.Contains(string.Format("workFlow{0}|siteId{1}|type{3}|{2}", auditActivity.ActivityId, SiteId, columnId, 0)) ? "checked='checked'" : "";
                    string checkBox = string.Format(
                        "<input type=\"checkbox\"  name=\"columnWorkFlowCheckBox\" value=\"workFlow{0}|siteId{1}|type{3}|{2}\" columnid=\"{2}\" {5}/>{4}",
                        auditActivity.ActivityId, SiteId, columnId, 0, auditActivity.ActivityName, checkedStr);
                    result.Append(checkBox);
                }
            }
        }
        return result.ToStr();
    }

    /// <summary>
    /// 获取分配的栏目类别权限信息
    /// </summary>
    /// <param name="columnId"></param>
    /// <param name="siteId"></param>
    /// <param name="markType"></param>
    /// <returns></returns>
    public string GetColumnCategoryFunction(string columnId, string siteId, string markType)
    {
        StringBuilder sb = new StringBuilder();
        if (markType == "Category")
        {
            bool IsCategoryPower = false;
            IList<Column> listColumns = ServiceFactory.ColumnService.GetMarkListByColumnId(columnId.ToInt());
            Column mainColumn = listColumns.SingleOrDefault(p => p.MarkType.IsEmpty());

            if (mainColumn != null)
            {
                IsCategoryPower = mainColumn.IsCategoryPower;
            }
            if (IsCategoryPower)
            {
                var listSelect = Whir.Security.ServiceFactory.RolesService.GetCategoryJurisdictionListByRoleId(RoleId);

                var selectCategory = string.Join(",",
                    listSelect.Where(p =>
                        p.Contains("category|siteId{0}|type{3}|{1}|{2}".FormatWith(SiteId, 0, columnId, 0))).Select(p => p.Substring(p.LastIndexOf('|') + 1)));

                sb.AppendFormat("<a href=\"javascript:void(0);\" selectCategory=\"{0}\" columnid=\"{1}\" maincolumnid=\"{2}\" class=\"a_power\" title=\"{3}\">" +
                                " <span class=\"icon-lock\" ></span></a>"
                        , selectCategory
                        , columnId
                        , mainColumn.ColumnId
                        , "分配类别权限"
                        );
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// 获取某一栏目的某一个功能checkbox:<input type="checkbox"  name="columncheckbox" value="功能名|siteId+站点ID|栏目id|" columnid="栏目id" />功能名
    /// </summary>
    /// <param name="columnId">栏目id</param>
    /// <param name="siteId">站点id</param>
    /// <param name="functionId">功能id</param>
    /// <returns></returns>
    private string GetColumnFunctionCheckBox(int columnId, int siteId, int functionId)
    {
        string functionName = ModuleFunctionType.GetFunctionNameById(functionId);
        //对于已授权的把勾打上
        string checkBoxCheckState = ClientResources.Contains(string.Format("{0}|siteId{1}|{2}", functionName, siteId, columnId)) ? "checked='checked'" : "";

        string checkBoxStr = string.Format("<input type=\"checkbox\"  name=\"columncheckbox\" value=\"{0}|siteId{1}|{2}\" columnid=\"{2}\" functionname='{0}' {3} lang='{4}' />{4}", functionName, siteId, columnId, checkBoxCheckState, functionName.ToLang());
        return checkBoxStr;
    }
    #endregion


}