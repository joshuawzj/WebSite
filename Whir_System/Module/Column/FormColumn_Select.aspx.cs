using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;
using Whir.Language;


public partial class Whir_System_Module_Column_FormColumn_Select : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前的栏目
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 子站点ID
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 回传到父页面的JS函数名
    /// </summary>
    protected string Callback { get; set; }

    /// <summary>
    /// 选择类型
    /// </summary>
    protected SelectedType SelectedType { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgeOpenPagePermission(IsDevUser);
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        Callback = RequestUtil.Instance.GetQueryString("callback");

        SelectedType = RequestUtil.Instance.GetQueryString("selectedtype").ToLower() == "checkbox"
                       ? SelectedType.CheckBox
                       : SelectedType.RadioBox;

        if (!IsPostBack)
        {
            BindSiteTab();
        }
    }
    //绑定站点群选项卡
    private void BindSiteTab()
    {
        var listSiteInfo = ServiceFactory.SiteInfoService.GetList();
        rptMultiSite.DataSource = listSiteInfo;
        rptMultiSite.DataBind();

        rptMultiSiteColumn.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(rptMultiSiteColumn_ItemDataBound);
        rptMultiSiteColumn.DataSource = listSiteInfo;
        rptMultiSiteColumn.DataBind();
    }
    void rptMultiSiteColumn_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Literal litScript = e.Item.FindControl("litScript") as Literal;
        if (litScript == null) return;

        SiteInfo siteInfo = e.Item.DataItem as SiteInfo;
        if (siteInfo == null) return;

        if (SelectedType == Whir.Service.SelectedType.RadioBox)
        {
            string script = "<script type='text/javascript'>$(function(){{ whir.ztree.column('columnTree{0}',{1}, onCheck, {{ enable: true, chkStyle: 'radio', radioType: 'all'}}); }})</script>";
            script = script.FormatWith(siteInfo.SiteId, GetInitNodes(siteInfo.SiteId));
            litScript.Text = script;
        }
        else
        {
            string script = "<script type='text/javascript'>$(function(){{ whir.ztree.column('columnTree{0}',{1}, onCheck); }})</script>";
            script = script.FormatWith(siteInfo.SiteId, GetInitNodes(siteInfo.SiteId));
            litScript.Text = script;
        }
    }

    //获取初始节点
    private string GetInitNodes(int siteId)
    {
        //此站点下所有栏目, 包含子站/专题栏目
        IList<Column> listColumn = ServiceFactory.ColumnService.GetListAllColumn(siteId, true);
        //当前栏目
        Column currentColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);

        string initNodes = "[";

        //栏目
        string columnChildJson = GetColumnJson(listColumn, currentColumn, 0, 0, 0, false);
        if (columnChildJson.Length > 5)
        {
            initNodes += "{ ";
            initNodes += " name : '{0}', ".FormatWith("站点栏目".ToLang());
            initNodes += " id : '0-0', ";
            initNodes += " nocheck:true, ";
            initNodes += " open:true, ";
            initNodes += " children : {0} ".FormatWith(columnChildJson);
            initNodes += " } ";
        }

        //子站
        string subsiteJson = GetSubsiteJson("子站栏目".ToLang(), currentColumn, listColumn, siteId);
        if (!subsiteJson.IsEmpty())
        {
            if (initNodes == "[")
                initNodes += subsiteJson;
            else
                initNodes += "," + subsiteJson;
        }

        //专题
        string subjectJson = GetSubjectJson("专题栏目".ToLang(), currentColumn, listColumn, siteId);
        if (!subjectJson.IsEmpty())
        {
            if (initNodes == "[")
                initNodes += subjectJson;
            else
                initNodes += "," + subjectJson;
        }

        initNodes += "]";
        return initNodes;
    }

    private string GetSubjectJson(string rootName, Column currentColumn, IList<Column> listColumn, int siteID)
    {
        string result = "";
        //此站点的专题
        IList<SubjectClass> listSubjectClass = ServiceFactory.SubjectClassService.GetSubjectClassList(siteID);
        if (listSubjectClass.Count > 0)
        {
            result += "{ ";
            result += " name : '{0}', ".FormatWith(rootName.Replace('\'', ' '));
            result += " id : '-1', ";
            result += " nocheck:true, ";
            result += " open:true, ";
            result += " children : [";

            #region 专题

            foreach (SubjectClass subsiteClass in listSubjectClass)
            {
                result += "{";
                result += " name : '{0}',".FormatWith(subsiteClass.SubjectClassName.Replace('\'', ' '));
                result += " id : '{0}',".FormatWith(subsiteClass.SubjectClassId);
                result += " nocheck:true";

                //子站
                IList<Subject> listSubsite = ServiceFactory.SubjectService.GetListBySubjectClassId(subsiteClass.SubjectClassId);
                if (listSubsite.Count > 0)
                {
                    result += ", children : [";
                    foreach (Subject subsite in listSubsite)
                    {
                        result += "{";
                        result += " name : '{0}',".FormatWith(subsite.SubjectName.Replace('\'', ' '));
                        result += " id : '{0}',".FormatWith(subsite.SubjectId);
                        result += " nocheck:true";

                        string subsiteColumn = GetColumnJson(listColumn, currentColumn, 0, subsite.SubjectClassId, subsite.SubjectId, false);
                        if (subsiteColumn.Length > 5)
                        {
                            //子站下的栏目
                            result += ", children : {0}".FormatWith(subsiteColumn);
                        }
                        result += "},";
                    }
                    result = result.TrimEnd(',');
                    result += "]";
                }

                result += "},";
            }

            #endregion 专题

            result = result.TrimEnd(',');
            result += "]";
            result += "} ";
        }
        return result;
    }

    private string GetSubsiteJson(string rootName, Column currentColumn, IList<Column> listColumn, int siteID)
    {
        string result = "";
        //此站点的子站
        IList<SubjectClass> listSubsiteClass = ServiceFactory.SubjectClassService.GetSubsiteClassList(siteID);
        IList<Subject> listCustomSubsite = ServiceFactory.SubjectService.GetListBySubjectClassId(0).Where(p => p.SiteId == siteID).ToList();
        if (listSubsiteClass.Count + listCustomSubsite.Count > 0)
        {
            result += "{ ";
            result += " name : '{0}', ".FormatWith(rootName);
            result += " id : '-1', ";
            result += " nocheck:true, ";
            result += " open:true, ";
            result += " children : [";

            #region 模板子站

            foreach (SubjectClass subsiteClass in listSubsiteClass)
            {
                result += "{";
                result += " name : '{0}',".FormatWith(subsiteClass.SubjectClassName);
                result += " id : '{0}',".FormatWith(subsiteClass.SubjectClassId);
                result += " nocheck:true";

                //子站
                IList<Subject> listSubsite = ServiceFactory.SubjectService.GetListBySubjectClassId(subsiteClass.SubjectClassId);
                if (listSubsite.Count > 0)
                {
                    result += ", children : [";
                    foreach (Subject subsite in listSubsite)
                    {
                        result += "{";
                        result += " name : '{0}',".FormatWith(subsite.SubjectName.Replace('\'', ' '));
                        result += " id : '{0}',".FormatWith(subsite.SubjectId);
                        result += " nocheck:true";

                        string subsiteColumn = GetColumnJson(listColumn, currentColumn, 0, subsite.SubjectClassId, subsite.SubjectId, false);
                        if (subsiteColumn.Length > 5)
                        {
                            //子站下的栏目
                            result += ", children : {0}".FormatWith(subsiteColumn);
                        }
                        result += "},";
                    }
                    result = result.TrimEnd(',');
                    result += "]";
                }

                result += "},";
            }

            #endregion 模板子站

            #region 自定义子站

            if (listCustomSubsite.Count > 0)
            {
                result += "{";
                result += " name : '{0}',".FormatWith("自定义子站".ToLang());
                result += " id : '{0}',".FormatWith(0);
                result += " nocheck:true,";
                result += " children : [";

                foreach (Subject customSubsite in listCustomSubsite)
                {
                    result += "{";
                    result += " name : '{0}',".FormatWith(customSubsite.SubjectName.Replace('\'', ' '));
                    result += " id : '{0}',".FormatWith(customSubsite.SubjectId);
                    result += " nocheck:true";

                    IList<Column> listCustomSubsiteColumn = ServiceFactory.ColumnService.GetSubjectColumnList(0,customSubsite.SubjectClassId, customSubsite.SubjectId, true);
                    string customSubsiteColumn = GetColumnJson(listCustomSubsiteColumn, currentColumn, 0, customSubsite.SubjectId, customSubsite.SubjectId, true);
                    if (customSubsiteColumn.Length > 5)
                    {
                        result += ", children : {0}".FormatWith(customSubsiteColumn);
                    }

                    result += "},";
                }
                result = result.TrimEnd(',');
                result += "]";
                result += "},";
            }

            #endregion 自定义子站

            result = result.TrimEnd(',');
            result += "]";
            result += "} ";
        }
        return result;
    }

    private string GetColumnJson(IList<Column> listColumn, Column currentColumn, int parentID, int siteType, int subjectId, bool isCustomSubsite)
    {
        string result = "[";

        var loop = listColumn.Where(p => p.ParentId == parentID && p.SiteType == siteType && p.IsCustomSubsite == isCustomSubsite).ToList();

        foreach (Column column in loop)
        {
            //模板子站,则需要读取别名，若不存在别名就读本身名
            string columnAlias = ServiceFactory.SubjectColumnService.GetColumnName(column.ColumnId, subjectId);
            column.ColumnName = columnAlias.IsEmpty() ? column.ColumnName : columnAlias;//c.ColumnName;

            result += "{";
            result += "id:'{0}|{1}',".FormatWith(column.ColumnId, subjectId);
            if (IsDevUser)
            {
                result += "name:'{0}',".FormatWith(trimStartChar(column.ColumnName.Replace('\'', ' ')));
            }
            else
            {
                result += "name:'{0}',".FormatWith(trimStartChar(column.ColumnName.Replace('\'', ' ')));
            }
            result += "subjectID:'{0}'".FormatWith(subjectId);

            ////是否和当前栏目重复
            //if (column.ColumnId == ColumnId)
            //    result += ",nocheck:true";

            //不是同一种模型
            if (currentColumn.ModelId != column.ModelId || (SelectedType.RadioBox == SelectedType && currentColumn.ColumnId == column.ColumnId && SubjectId == subjectId))
            {

                result += ",nocheck:true";
            }

            //检查栏目是否有添加功能，若没有则飞过
            if (subjectId <= 0)
            {
                if (!  Whir.Security.ServiceFactory.RolesService.IsRoleHaveColumnJurisdiction("添加", column.ColumnId, CurrentUser.RolesId, column.SiteId))
                {  
                    result += ",nocheck:true";
                }
            }
            else
            {
                if (!Whir.Security.ServiceFactory.RolesService.IsRoleHaveSubjectJurisdiction("subjectcolumn", "添加", column.ColumnId, CurrentUser.RolesId, column.SiteId, subjectId))
                {     
                    result += ",nocheck:true";
                }
            }

            //是否有子节点
            string childJson = GetColumnJson(listColumn, currentColumn, column.ColumnId, column.SiteType, subjectId, isCustomSubsite);
            if (childJson.Length > 5)
            {
                result += ",children:{0}".FormatWith(childJson);
            }

            result += "},";
        }
        result = result.TrimEnd(',');
        result += "]";
        return result;
    }

    private string trimStartChar(string columnName)
    {
        columnName = columnName.Trim();
        if (columnName.StartsWith("├"))
            return trimStartChar(columnName.TrimStart('├'));

        if (columnName.StartsWith("└"))
            return trimStartChar(columnName.TrimStart('└'));

        if (columnName.StartsWith("─"))
            return trimStartChar(columnName.TrimStart('─'));

        return columnName;
    }
}