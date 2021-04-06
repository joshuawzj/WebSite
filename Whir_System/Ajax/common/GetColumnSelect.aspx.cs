/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：getcolumn.aspx.cs
* 文件描述：异步获取当前站点的栏目的页面。 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class whir_system_ajax_GetColumnSelect : Whir.ezEIP.Web.SysManagePageBase
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
    /// 是否全部都可以勾选
    /// </summary>
    protected bool IsAll { get; set; }

    /// <summary>
    /// 选择类型
    /// </summary>
    protected SelectedType SelectedType { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
            SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
            IsAll = RequestUtil.Instance.GetQueryInt("IsAll", 0).ToBoolean();
            SelectedType = RequestUtil.Instance.GetQueryString("selectedtype").ToLower() == "checkbox"
                           ? SelectedType.CheckBox
                           : SelectedType.RadioBox;

            int siteId = RequestUtil.Instance.GetQueryInt("siteId", 0);
            var key = RequestUtil.Instance.GetString("id");
            Column currentColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId) ?? new Column();
            var result = "";
            if (key.Contains("|"))
            {
                var kv = new KeyValuePair<int, int>(key.Split('|')[0].ToInt(), key.Split('|')[1].ToInt());
                if (kv.Key == 0)
                {
                    if (kv.Value == 0) //case"0|0"获取内容管理栏目
                        result = GetColumnJson(siteId, currentColumn, 0, 0, 0);
                    else  //case "0|1":case "0|2"://获取子站、专题类型
                        result = GetClassJson(kv.Value, siteId);
                }
                else if (kv.Key > 0 && kv.Value <= 0)  //case"2|0"获取内容管理栏目，子站id为0
                {
                    var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(kv.Key);
                    result = GetColumnJson(siteId, currentColumn, column.ColumnId, 0, 0);
                }
                else if (kv.Key == -1 && kv.Value > 0)  //获取子站、专题类型
                    result = GetSubJson(kv.Value);
                else if (kv.Key <= 0 && kv.Value <= 0)  //根据 子站类型 classId，subjectId 获取栏目信息 去绝对值
                    result = GetColumnJson(siteId, currentColumn, 0, Math.Abs(kv.Key), Math.Abs(kv.Value));
                else
                {
                    var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(kv.Key);
                    result = GetColumnJson(siteId, currentColumn, column.ColumnId, 0, kv.Value);
                }
            }
            else
                result = GetInitNodes(siteId);
            Response.Write(result);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    /// <summary>
    /// 获取初始节点
    /// </summary>
    /// <param name="siteId"></param>
    /// <returns></returns>
    private string GetInitNodes(int siteId)
    {
        string initNodes = "[";

        //栏目

        initNodes += "{ ";
        initNodes += " name : '{0}', ".FormatWith("内容栏目".ToLang());
        initNodes += " id : '0|0', ";
        initNodes += " nocheck:true, ";
        initNodes += " isParent:true ";
        // initNodes += " children : {0} ".FormatWith(columnChildJson);
        initNodes += " } ";


        //子站
        if (ServiceFactory.ColumnService.GetListCount(1, siteId) > 0)
        {
            initNodes += ",{ ";
            initNodes += " name : '{0}', ".FormatWith("子站栏目".ToLang());
            initNodes += " id : '0|1', ";
            initNodes += " nocheck:true, ";
            initNodes += " isParent:true ";
            // initNodes += " children : {0} ".FormatWith(columnChildJson);
            initNodes += " } ";
        }

        //专题
        if (ServiceFactory.ColumnService.GetListCount(2, siteId) > 0)
        {
            initNodes += ",{ ";
            initNodes += " name : '{0}', ".FormatWith("专题栏目".ToLang());
            initNodes += " id : '0|2', ";
            initNodes += " nocheck:true, ";
            initNodes += " isParent:true ";
            // initNodes += " children : {0} ".FormatWith(columnChildJson);
            initNodes += " } ";
        }

        initNodes += "]";
        return initNodes;
    }

    /// <summary>
    /// 获取子站类型下所有子站
    /// </summary>
    /// <param name="subjectClassId"></param>
    /// <returns></returns>
    private string GetSubJson(int subjectClassId)
    {
        string result = "[";
        IList<Subject> listSubsite = ServiceFactory.SubjectService.GetListBySubjectClassId(subjectClassId);
        if (listSubsite.Count > 0)
        {
            foreach (Subject subsite in listSubsite)
            {
                result += "{";
                result += " name : '{0}',".FormatWith(subsite.SubjectName.Replace('\'', ' '));
                result += " id : '-{0}|-{1}',".FormatWith(subjectClassId, subsite.SubjectId);
                result += " nocheck:true, ";
                result += " isParent:true";
                result += "},";
            }
            result = result.TrimEnd(',');
            result += "]";
        }
        return result;
    }

    /// <summary>
    /// 获取当前站点下所有子站、专题 类型
    /// </summary>
    /// <param name="subjectTypeId"></param>
    /// <param name="siteID"></param>
    /// <returns></returns>
    private string GetClassJson(int subjectTypeId, int siteID)
    {
        string result = "";
        //此站点的子站
        IList<SubjectClass> listSubsiteClass;
        if (subjectTypeId == 1)
            listSubsiteClass = ServiceFactory.SubjectClassService.GetSubsiteClassList(siteID);
        else
            listSubsiteClass = ServiceFactory.SubjectClassService.GetSubjectClassList(siteID);


        if (listSubsiteClass.Count > 0)
        {
            result += "[";
            #region 模板子站

            foreach (SubjectClass subsiteClass in listSubsiteClass)
            {
                result += "{";
                result += " name : '{0}',".FormatWith(subsiteClass.SubjectClassName);
                result += " id : '-1|{0}',".FormatWith(subsiteClass.SubjectClassId);
                result += " nocheck:true, ";
                result += " isParent:true";
                result += "},";
            }
            result = result.Trim(',');
            #endregion 模板子站
            result += "]";
        }
        return result;
    }

    /// <summary>
    /// 获取parentid下一级栏目
    /// </summary>
    /// <returns></returns>
    private string GetColumnJson(int siteId, Column currentColumn, int parentID, int siteType, int subjectId)
    {
        string result = "[";

        IList<Column> listColumn = ServiceFactory.ColumnService.GetListAllColumn(siteId, true);

        var loop = listColumn.Where(p => p.ParentId == parentID && p.SiteType == siteType && !p.IsDel).ToList();
        if (parentID > 0)
            loop = listColumn.Where(p => p.ParentId == parentID && !p.IsDel).ToList();

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

            //不是同一种模型
            if (!IsAll && currentColumn.ModelId != column.ModelId || (SelectedType.RadioBox == SelectedType && currentColumn.ColumnId == column.ColumnId && SubjectId == subjectId))
            {
                result += ",nocheck:true";
            }

            //检查栏目是否有添加功能，若没有则飞过
            var isRoleHave = false;
            if (subjectId <= 0)
            {
                if (column.ModuleMark == "SinglePage_v0.0.01")
                    isRoleHave = Whir.Security.ServiceFactory.RolesService.IsRoleHaveColumnJurisdiction("修改", column.ColumnId, CurrentUserRolesId, column.SiteId);
                else
                    isRoleHave = Whir.Security.ServiceFactory.RolesService.IsRoleHaveColumnJurisdiction("添加", column.ColumnId, CurrentUserRolesId, column.SiteId);
                if (!isRoleHave)
                    result += ",nocheck:true";
            }
            else
            {
                if (column.ModuleMark == "SinglePage_v0.0.01")
                    isRoleHave = Whir.Security.ServiceFactory.RolesService.IsRoleHaveSubjectJurisdiction("subjectcolumn", "修改", column.ColumnId, CurrentUserRolesId, column.SiteId, subjectId);
                else
                    isRoleHave = Whir.Security.ServiceFactory.RolesService.IsRoleHaveSubjectJurisdiction("subjectcolumn", "添加", column.ColumnId, CurrentUserRolesId, column.SiteId, subjectId);
                if (!isRoleHave)
                    result += ",nocheck:true";
            }

            result += ",isParent:{0}".FormatWith(listColumn.Where(p => p.ParentId == column.ColumnId && !p.IsDel).Count() > 0 ? "true" : "false");

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