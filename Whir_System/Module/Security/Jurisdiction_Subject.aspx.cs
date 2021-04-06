/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：Jurisdiction_column.aspx.cs
 * 文件描述：分配子站或专题权限
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Security.Domain;
using Whir.Service;

public partial class whir_system_module_security_Jurisdiction_subject : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前站点
    /// </summary>
    protected int SiteId { get; set; }
    /// <summary>
    /// 类型：1=子站  2=专题
    /// </summary>
    protected int SubjectTypeId { get; set; }
    /// <summary>
    ///  子站 id
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    ///  子站类型 id
    /// </summary>
    protected int ClassId { get; set; }
    /// <summary>
    /// 角色ID
    /// </summary>
    protected int RoleId { get; set; }
    /// <summary>
    /// 父节点角色ID
    /// </summary>
    protected int ParentRoleId { get; set; }
    /// <summary>
    ///  当前角色已有的权限
    /// </summary>
    protected List<string> ClientResources { get; set; }
    /// <summary>
    /// 站点的所有子站资源
    /// </summary>
    protected List<string> AllClientSubjectAndFunction { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        SiteId = RequestUtil.Instance.GetQueryInt("siteid", -1);
        SubjectTypeId = RequestUtil.Instance.GetQueryInt("type", -1);
        RoleId = RequestUtil.Instance.GetQueryInt("roleid", -1);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectId", 0);
        ClassId = RequestUtil.Instance.GetQueryInt("ClassId", 0);
        if (SubjectId == 0 || ClassId == 0)
            return;
        if (!IsPostBack)
        {
            JudgeOpenPagePermission(IsCurrentRoleMenuRes("329"));
            //已有的栏目与功能权限，2013-8-15 增加上级角色权限范围控制
            var role = Whir.Security.ServiceFactory.RolesService.SingleOrDefault<Roles>(RoleId);
            ParentRoleId = role == null ? 0 : role.ParentId.ToInt();

            //已授权给客户后台的权限
            if (SubjectTypeId == 2)
            {
                ClientResources = Whir.Security.ServiceFactory.RolesService.GetSubjectColumnJurisdictionListByRoleId(RoleId);
                AllClientSubjectAndFunction = Whir.Security.ServiceFactory.RolesService.GetSubjectColumnJurisdictionListByRoleId(ParentRoleId);
            }
            else
            {
                ClientResources = Whir.Security.ServiceFactory.RolesService.GetSubSiteColumnJurisdictionListByRoleId(RoleId);
                AllClientSubjectAndFunction = Whir.Security.ServiceFactory.RolesService.GetSubSiteColumnJurisdictionListByRoleId(ParentRoleId);
            }
            //累加当前角色拥有的工作流权限
            ClientResources.AddRange(Whir.Security.ServiceFactory.RolesService.GetWorkFlowJurisdictionListByRoleId(RoleId));
            AllClientSubjectAndFunction.AddRange(Whir.Security.ServiceFactory.RolesService.GetWorkFlowJurisdictionListByRoleId(ParentRoleId));

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
        rptColumnList.DataSource = ServiceFactory.ColumnService.GetSubjectList(SubjectTypeId, SubjectId, ClassId, RoleId, SiteId, true);
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
    /// <param name="subjectId"></param>
    /// <returns></returns>
    public string GetColumnWorkFlowCheckBox(string columnId, string subjectId, string resultType)
    {
        if (resultType != "subjectcolumn")
            return "";

        var result = new StringBuilder();
        var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId.ToInt());
        var workFlowId = column == null ? 0 : column.WorkFlow;
        if (workFlowId > 0)
        {
            var list = ServiceFactory.AuditActivityService.GetListBySort(workFlowId);
            result.Append(list.Count > 0 ? "<span style='color:green'>" + "工作流权限:".ToLang() + "</span>" : "");
            foreach (var auditActivity in list)
            {
                //①先判断上级角色是否有工作流节点权限
                if (RoleId <= 0)
                    continue;
                var parentRole = Whir.Security.ServiceFactory.RolesService.SingleOrDefault<Roles>(RoleId);
                var parentHavePurview =
                    Whir.Security.ServiceFactory.RolesService.IsRoleHaveWorkFlowJurisdiction(column.ColumnId, parentRole.ParentId.ToInt(), CurrentSiteId, subjectId.ToInt(), auditActivity.ActivityId);
                if (parentHavePurview)
                {
                    //②再判断当前角色是否有工作流节点权限
                    string checkedStr = ClientResources.Contains(string.Format("workFlow{0}|siteId{1}|type{4}|{2}|{3}", auditActivity.ActivityId, SiteId, columnId, subjectId, SubjectTypeId)) ? "checked='checked'" : "";

                    string checkBox = string.Format(
                    "<input type=\"checkbox\"  name=\"columnWorkFlowCheckBox\" value=\"workFlow{0}|siteId{1}|type{4}|{2}|{3}\" functionname=\"{5}\" columnid=\"{2}\" {6}/>{5}",
                       auditActivity.ActivityId, SiteId, columnId, subjectId, SubjectTypeId, auditActivity.ActivityName, checkedStr);
                    result.Append(checkBox);
                }
            }
        }
        return result.ToStr();
    }

    public string GetColumnCategoryFunction(string resultType, string columnId, string subjectId)
    {
        //类型:subjectcolumn=子站或专题栏目;subjectclass=子站或专题;subject=子站或专题
        SubjectShow d = new SubjectShow();
        StringBuilder sb = new StringBuilder();
        //子站或专题栏目
        if (resultType == "subjectcolumn")
        {
            bool IsCategoryPower = false;
            Column col = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId.ToInt());
            if (col == null || col.MarkType.IsEmpty())//主栏目
            {
                return "";
            }
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
                        p.Contains("category|siteId{0}|type{1}|{2}|{3}".FormatWith(SiteId, SubjectTypeId, subjectId, columnId))).Select(p => p.Substring(p.LastIndexOf('|') + 1)));
                sb.AppendFormat("<a href=\"javascript:void(0);\" selectCategory=\"{0}\" columnid=\"{1}\" maincolumnid=\"{2}\" class=\"a_power\" title=\"{3}\" subjectId=\"{4}\"><span class=\"icon-lock\" ></span></a>"
                            , selectCategory
                            , columnId
                            , mainColumn.ColumnId
                            , "分配类别权限"
                            , subjectId
                            );
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// <summary>
    /// 获取某一栏目的所有功能复选框<input type="checkbox"  name="columncheckbox" value="类型|功能名|栏目id|siteId站点ID|子站id" columnid="栏目id" />功能名
    /// </summary>
    /// <param name="ResultType"> 类型:subjectcolumn=子站或专题栏目;subjectclass=子站或专题类型;subject=子站或专题</param>
    /// <param name="IdValue">各自主键值</param>
    /// <param name="subjectId">栏目所属的子站或专题，只有当resultType='subjectcolumn' 才用到</param>
    /// <param name="functionIds">功能参数</param>
    /// <returns></returns>
    public string GetColumnFunctionCheckBox(string resultType, string IdValue, string subjectId, string functionIds)
    {
        string result = "";

        if (resultType == "subjectclass")
        {
            string CheckBoxCheckState = "";
            string str = "添加子站".ToLang();
            if (SubjectTypeId == 2) { str = "添加专题".ToLang(); }
            if (AllClientSubjectAndFunction.Contains(string.Format("subjectclass|{0}|{1}|siteId{2}|0", str, IdValue, SiteId)) || ParentRoleId == 1)//要开放给客户后台的才显示出来
            {
                CheckBoxCheckState = ClientResources.Contains(string.Format("subjectclass|{0}|{1}|siteId{2}|0", str, IdValue, SiteId)) ? "checked='checked'" : "";
                result += string.Format("<input type=\"checkbox\"  name=\"columncheckbox\" value=\"subjectclass|{0}|{1}|siteId{2}|0\" idvalue=\"{1}\" functionname='{4}' {3} lang='{4}' />{4}", str, IdValue, SiteId, CheckBoxCheckState, str.ToLang());
            }
            if (IdValue != "0")//自定义子站没有修改
            {
                if (AllClientSubjectAndFunction.Contains(string.Format("subjectclass|{0}|{1}|siteId{2}|0", "修改", IdValue, SiteId)) || ParentRoleId == 1)//要开放给客户后台的才显示出来
                {
                    CheckBoxCheckState = ClientResources.Contains(string.Format("subjectclass|{0}|{1}|siteId{2}|0", "修改", IdValue, SiteId)) ? "checked='checked'" : "";
                    result += string.Format("<input type=\"checkbox\"  name=\"columncheckbox\" value=\"subjectclass|{0}|{1}|siteId{2}|0\" idvalue=\"{1}\" functionname='{4}' {3} lang='{4}' />{4}", "修改", IdValue, SiteId, CheckBoxCheckState, "修改".ToLang());
                }
            }

            if (IdValue == "0")//自定义子站
            {
                if (AllClientSubjectAndFunction.Contains(string.Format("subjectclass|{0}|{1}|siteId{2}|0", "添加栏目", IdValue, SiteId)) || ParentRoleId == 1)//要开放给客户后台的才显示出来
                {
                    CheckBoxCheckState = ClientResources.Contains(string.Format("subjectclass|{0}|{1}|siteId{2}|0", "添加栏目", IdValue, SiteId)) ? "checked='checked'" : "";
                    result += string.Format("<input type=\"checkbox\"  name=\"columncheckbox\" value=\"subjectclass|{0}|{1}|siteId{2}|0\" idvalue=\"{1}\" functionname='{4}' {3} lang='{4}' />{4}", "添加栏目", IdValue, SiteId, CheckBoxCheckState, "添加栏目".ToLang());
                }
            }
        }
        else if (resultType == "subject")
        {
            string CheckBoxCheckState = "";
            if (AllClientSubjectAndFunction.Contains(string.Format("subject|{0}|{1}|siteId{2}|{1}", "查看", IdValue, SiteId)) || ParentRoleId == 1)//要开放给客户后台的才显示出来
            {
                CheckBoxCheckState = ClientResources.Contains(string.Format("subject|{0}|{1}|siteId{2}|{1}", "查看", IdValue, SiteId)) ? "checked='checked'" : "";
                result += string.Format("<input type=\"checkbox\"  name=\"columncheckbox\" value=\"subject|{0}|{1}|siteId{2}|{1}\" idvalue=\"{1}\" functionname='{4}' {3} lang='{4}' />{4}", "查看", IdValue, SiteId, CheckBoxCheckState, "查看".ToLang());
            }
            if (AllClientSubjectAndFunction.Contains(string.Format("subject|{0}|{1}|siteId{2}|{1}", "SEO设置", IdValue, SiteId)) || ParentRoleId == 1)//要开放给客户后台的才显示出来
            {
                CheckBoxCheckState = ClientResources.Contains(string.Format("subject|{0}|{1}|siteId{2}|{1}", "SEO设置", IdValue, SiteId)) ? "checked='checked'" : "";
                result += string.Format("<input type=\"checkbox\"  name=\"columncheckbox\" value=\"subject|{0}|{1}|siteId{2}|{1}\" idvalue=\"{1}\" functionname='{4}' {3} lang='{4}' />{4}", "SEO设置", IdValue, SiteId, CheckBoxCheckState, "SEO设置".ToLang());
            }

            if (AllClientSubjectAndFunction.Contains(string.Format("subject|{0}|{1}|siteId{2}|{1}", "修改", IdValue, SiteId)) || ParentRoleId == 1)//要开放给客户后台的才显示出来
            {
                CheckBoxCheckState = ClientResources.Contains(string.Format("subject|{0}|{1}|siteId{2}|{1}", "修改", IdValue, SiteId)) ? "checked='checked'" : "";
                result += string.Format("<input type=\"checkbox\"  name=\"columncheckbox\" value=\"subject|{0}|{1}|siteId{2}|{1}\" idvalue=\"{1}\" functionname='{4}' {3} lang='{4}' />{4}", "修改", IdValue, SiteId, CheckBoxCheckState, "修改".ToLang());
            }
            if (AllClientSubjectAndFunction.Contains(string.Format("subject|{0}|{1}|siteId{2}|{1}", "删除", IdValue, SiteId)) || ParentRoleId == 1)//要开放给客户后台的才显示出来
            {
                CheckBoxCheckState = ClientResources.Contains(string.Format("subject|{0}|{1}|siteId{2}|{1}", "删除", IdValue, SiteId)) ? "checked='checked'" : "";
                result += string.Format("<input type=\"checkbox\"  name=\"columncheckbox\" value=\"subject|{0}|{1}|siteId{2}|{1}\" idvalue=\"{1}\" functionname='{4}' {3}  lang='{4}' />{4}", "删除", IdValue, SiteId, CheckBoxCheckState, "删除".ToLang());

            }
        }
        else
        {
            functionIds = ("," + functionIds + ",").Replace(",23,", ",").Replace(",,", ",").Trim(',');

            if (functionIds == "" || ServiceFactory.ColumnService.IsParentColumn(IdValue.ToInt()))
            {
                functionIds = string.Join(",", ModuleFunctionType.StuctFunctionDefault.Where(p => p != 23)); //子站、专题没有复制栏目功能
            }
            foreach (string s in functionIds.Split(','))
            {
                //没开放给客户的权限飞过
                if (!AllClientSubjectAndFunction.Contains(string.Format("{0}|{1}|{2}|siteId{3}|{4}", resultType,
                    ModuleFunctionType.GetFunctionNameById(s.ToInt()), IdValue, SiteId, subjectId)) && ParentRoleId != 1)
                { continue; }
                result += GetColumnFunctionCheckBox(IdValue.ToInt(), SiteId, subjectId, s.ToInt());
            }
        }
        return result;
    }
    /// 获取某一栏目的某一个功能checkbox:<input type="checkbox"  name="columncheckbox" value="subjectcolumn|功能名|栏目id|站点ID｜子站或专题ID" columnid="栏目id" />功能名
    /// </summary>
    /// <param name="columnId">栏目id</param>
    /// <param name="siteId">站点id</param>
    /// <param name="subjectId">栏目所属的子站或专题ID</param>
    /// <param name="functionId">功能id</param>
    /// <returns></returns>
    private string GetColumnFunctionCheckBox(int columnId, int siteId, string subjectId, int functionId)
    {

        string FunctionName = ModuleFunctionType.GetFunctionNameById(functionId);
        //对于已授权的把勾打上                                               
        string CheckBoxCheckState = ClientResources.Contains(string.Format("subjectcolumn|{0}|{1}|siteId{2}|{3}", FunctionName, columnId, siteId, subjectId)) ? "checked='checked'" : "";

        string CheckBoxStr = string.Format("<input type=\"checkbox\"  name=\"columncheckbox\" value=\"subjectcolumn|{0}|{1}|siteId{2}|{3}\" columnid=\"{1}\" functionname='{5}' {4}   lang='{5}'/>{5}", FunctionName, columnId, siteId, subjectId, CheckBoxCheckState, FunctionName.ToLang());
        return CheckBoxStr;
    }
    #endregion


}