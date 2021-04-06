/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：Jurisdiction_category.aspx.cs
 * 文件描述：单独类别权限分配页面
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Repository;
using Whir.Security.Domain;
using Whir.Service;
using ServiceFactory = Whir.Security.ServiceFactory;

public partial class whir_system_module_security_Jurisdiction_category : SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 类别栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 用于唯一标识
    /// </summary>
    protected int MainColumnid { get; set; }

    /// <summary>
    /// 子站/专题ID
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 完成后执行父页面的JS函数, 此处为父页面的JS函数名
    /// </summary>
    protected string JsCallback { get; set; }

    /// <summary>
    /// 选中的ID
    /// </summary>
    protected string Ids { get; set; }

    protected string UnSelectIds { get; set; }

    /// <summary>
    /// 当前编辑的角色ID
    /// </summary>
    protected int RoleId { get; set; }

    /// <summary>
    /// 当前站点
    /// </summary>
    protected int SiteId { get; set; }

    /// <summary>
    /// 类型：1=子站  2=专题  0=内容管理
    /// </summary>
    protected int SubjectTypeId { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        SubjectTypeId = RequestUtil.Instance.GetQueryInt("SubjectTypeId", 0);
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        MainColumnid = RequestUtil.Instance.GetQueryInt("MainColumnid", 0);
        JsCallback = RequestUtil.Instance.GetQueryString("jscallback");
        Ids = RequestUtil.Instance.GetQueryString("ids");
        RoleId = RequestUtil.Instance.GetQueryInt("roleId", 0);
        SiteId = RequestUtil.Instance.GetQueryInt("siteId", 0);
        JudgeOpenPagePermission(IsCurrentRoleMenuRes("329"));
    }

    /// <summary>
    /// 树绑定
    /// </summary>
    /// <returns></returns>
    protected string BindTree()
    {
        List<ColumnLeftTree> ctList = new List<ColumnLeftTree>();
        if (ColumnId <= 0)
        {
            return "";
        }
        //获取类别表名
        string sql = "SELECT model.TableName FROM Whir_Dev_Column col INNER JOIN Whir_Dev_Model model ON col.ModelID=model.ModelID WHERE col.ColumnId=@0 and col.IsDel=0";
        string tableName = DbHelper.CurrentDb.ExecuteScalar<string>(sql, ColumnId);

        if (!tableName.IsEmpty())
        {
            string sql2 = "SELECT * FROM {0} WHERE  IsDel=0 AND TypeID=@0 AND SubjectId=@1 ORDER BY Sort DESC,CREATEDATE DESC".FormatWith(tableName);

            var table = DbHelper.CurrentDb.Query(sql2, ColumnId, SubjectId).Tables[0];
            if (table.Rows.Count > 0)
            {
                var parentRole = ServiceFactory.RolesService.SingleOrDefault<Roles>(RoleId);
                var listParent = ServiceFactory.RolesService.GetCategoryJurisdictionListByRoleId(parentRole.ParentId);
                string pidName = tableName + "_PID";
                foreach (DataRow row in table.Rows)
                {
                    ColumnLeftTree ct = new ColumnLeftTree();
                    ct.Id = row[pidName].ToStr().ToInt();
                    string columnname = Regex.Replace(row["CategoryName"].ToStr(), "[\\W]", "");
                    ct.name = columnname;

                    ct.pId = row["ParentId"].ToStr().ToInt();
                    ct.open = true;
                    //单独类别权限资源  category|siteId站点id{0}|栏目类型{1：0、1、2}|子站{2}|栏目{3}|类别id{4}
                    if (parentRole.ParentId == 0 || listParent.Contains("category|siteId{0}|type{1}|{2}|{3}|{4}".FormatWith(SiteId, SubjectTypeId, SubjectId, ColumnId, ct.Id)))//父节点包含的分类，才显示出来
                        ctList.Add(ct);

                }

            }
        }

        return ctList.ToJson();

    }
}