/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：Jurisdiction_column .aspx.cs
 * 文件描述：开发者分配栏目权限给客户
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Whir.Service;
using Whir.Framework;
using Whir.Domain;
using Whir.Security.Domain;
using System.Text;
using Whir.Language;

public partial class whir_system_module_developer_Jurisdiction_column : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前站点
    /// </summary>
    protected int SiteId { get; set; }

    /// <summary>
    /// 已授权给客户后台的资源
    /// </summary>
    protected List<string> ClientResources { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        SiteId = RequestUtil.Instance.GetQueryInt("siteid", -1);

        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            if (SiteId == -1)
            {//初始时跳到当前站点的栏目授权
                Response.Redirect("Jurisdiction_column.aspx?siteid=" + CurrentSiteId + "&type=0");
            }
            //已授权给客户后台的权限
            ClientResources = Whir.Security.ServiceFactory.RolesService.GetColumnJurisdictionListByRoleId(2);
            //累加当前角色拥有的工作流权限
            ClientResources.AddRange(Whir.Security.ServiceFactory.RolesService.GetWorkFlowJurisdictionListByRoleId(2));

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
        rptColumnList.DataSource = ServiceFactory.ColumnService.GetColumnList(0, 2, SiteId, "", true);
        rptColumnList.DataBind();

        if (SiteId != -1)//有选择要显示的栏目时才显示
        {
            Table_ColumnFunctionList.Visible = Table_ColumnList.Visible = true;
        }
 

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
            result.Append(list.Count > 0 ? "<span style='color:green'>" + "工作流权限：".ToLang() + "</span>" : "");
            foreach (var auditActivity in list)
            {
                //①先判断上级角色是否有工作流节点权限
                var parentRole = Whir.Security.ServiceFactory.RolesService.SingleOrDefault<Roles>(2);
                var parentHavePurview = IsDevUser || IsSuperUser ||
                     Whir.Security.ServiceFactory.RolesService.IsRoleHaveWorkFlowJurisdiction(column.ColumnId, parentRole.ParentId.ToInt(), CurrentSiteId, -1, auditActivity.ActivityId);
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
            bool isCategoryPower = false;
            IList<Column> listColumns = ServiceFactory.ColumnService.GetMarkListByColumnId(columnId.ToInt());
            Column mainColumn = listColumns.SingleOrDefault(p => p.MarkType.IsEmpty());

            if (mainColumn != null)
            {
                isCategoryPower = mainColumn.IsCategoryPower;
            }
            if (isCategoryPower)
            {
                var listSelect = Whir.Security.ServiceFactory.RolesService.GetCategoryJurisdictionListByRoleId(2);

                var selectCategory = string.Join(",",
                    listSelect.Where(p =>
                        p.Contains("category|siteId{0}|type{3}|{1}|{2}".FormatWith(SiteId, 0, columnId, 0))).Select(p=>p.Substring(p.LastIndexOf('|')+1)));

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
    /// 获取某一栏目的所有功能复选框<input type="checkbox"  name="columncheckbox" value="功能名|siteId+站点ID|栏目id" columnid="栏目id" />功能名
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
                result += GetColumnFunctionCheckBox(columnId.ToInt(), siteId.ToInt(), s);
            }
        }
        else
        {
            foreach (string s in functionIds.Split(','))
            {
                result += GetColumnFunctionCheckBox(columnId.ToInt(), siteId.ToInt(), s.ToInt());
            }
        }

        return result;
    }

    /// <summary>
    /// 获取某一栏目的某一个功能checkbox:<input type="checkbox"  name="columncheckbox" value="功能名|siteId+站点ID|栏目id" columnid="栏目id" />功能名
    /// </summary>
    /// <param name="columnId">栏目id</param>
    /// <param name="siteId">站点id</param>
    /// <param name="functionId">功能id</param>
    /// <returns></returns>
    private string GetColumnFunctionCheckBox(int columnId, int siteId, int functionId)
    {
        string FunctionName = ModuleFunctionType.GetFunctionNameById(functionId);
        //对于已授权的把勾打上
        string CheckBoxCheckState = ClientResources.Contains(string.Format("{0}|siteId{1}|{2}", FunctionName, siteId, columnId)) ? "checked='checked'" : "";

        string CheckBoxStr = string.Format("<input type=\"checkbox\"  name=\"columncheckbox\" value=\"{0}|siteId{1}|{2}\" columnid=\"{2}\" functionname='{4}' {3} />{4}", FunctionName, siteId, columnId, CheckBoxCheckState, FunctionName.ToLang());
        return CheckBoxStr;
    }
    /// <summary>
    /// 获取所有栏目功能
    /// </summary>
    /// <returns></returns>
    public string GetAllFunctionCheckBox()
    {
        string CheckBoxStr = "";

        string[] FunctionNames = ModuleFunctionType.FunctionTypeString;//所有栏目功能
        foreach (string s in FunctionNames)
        {
            CheckBoxStr += string.Format("<input type=\"checkbox\"  name=\"selectColumn\" value=\"{0}\" />{0}", s);

        }
        return CheckBoxStr;
    }
    #endregion
     
}