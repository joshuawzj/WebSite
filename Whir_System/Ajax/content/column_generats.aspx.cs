using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using Whir.Framework;
using Whir.Service;
using System.Text;
using Whir.Domain;
using System.Text.RegularExpressions;
using Whir.ezEIP.Web;

public partial class Whir_System_ajax_content_column_generats : SysManagePageBase
{

    /// <summary>
    /// 页面加载, 处理异步请求, 根据栏目名称返回一串json字符串
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Page_Load(object sender, EventArgs e)
    {

        var subjectTypeId = RequestUtil.Instance.GetString("subjecttypeid").ToInt(0);//子站为1，专题为2，否则为0，默认为栏目内容管理
        switch (subjectTypeId)
        {
            case 0://内容管理
                Response.Write(GetMenuHtml(0, CurrentSiteId, 0, 0).ToJson());
                break;
            case 1://子站
                Response.Write(GetSubSiteMenuHtml(CurrentSiteId).ToJson());
                break;
            case 2://专题
                Response.Write(GetSubjectMenuHtml(CurrentSiteId).ToJson());
                break;
        }
    }

    /// <summary>
    ///  递归获取菜单 内容管理
    /// </summary>
    /// <param name="parentId">父节点</param>
    /// <param name="siteId">站点id</param>
    /// <param name="siteType">子站、专题</param>
    /// <param name="subjectId">子站id</param>
    /// <param name="depth">递归深度</param>
    /// <returns></returns>
    public List<ColumnTree> GetMenuHtml(int parentId, int siteId, int siteType, int subjectId)
    {

        List<ColumnTree> allList = new List<ColumnTree>();

        var list = ServiceFactory.ColumnService.Query<Column>("WHERE IsDel=0 AND ParentId=@0 AND SiteId=@1 AND SiteType=@2 AND (MarkType='' OR MarkType IS NULL) ORDER BY Sort ASC", parentId, siteId, siteType).ToList();

        foreach (var column in list)
        {
            if (subjectId > 0)
            {
                if (!new SysManagePageBase().IsRoleHaveSubjectRes("subjectcolumn", "查看", column.ColumnId, siteId, subjectId)) continue; //没有查看栏目权限
            }
            else
            {
                if (!new SysManagePageBase().IsRoleHaveColumnRes("查看", column.ColumnId, subjectId == 0 ? -1 : subjectId)) continue; //没有查看栏目权限
            }
            ColumnTree temp = new ColumnTree();
            temp.columnid = column.ColumnId;
            temp.subjectid = subjectId;
            temp.sitetype = siteType;
            temp.text = column.ColumnName;
            temp.nodes = GetMenuHtml(column.ColumnId, siteId, siteType, subjectId);
            if (temp.nodes.Count == 0)
                temp.nodes = null;
            allList.Add(temp);
        }

        return allList;
    }

    ///<summary>
    /// 递归获取菜单 子站
    /// </summary>
    /// <param name="columnId"></param>
    /// <param name="siteId"></param>
    /// <param name="siteType"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    public List<ColumnTree> GetSubSiteMenuHtml(int siteId)
    {
        List<ColumnTree> allList = new List<ColumnTree>();
        var listSubjectClass = ServiceFactory.SubjectClassService.GetSubsiteClassList(siteId).ToList();

        foreach (SubjectClass sc in listSubjectClass)
        {

            ColumnTree temp = new ColumnTree();
            temp.columnid = sc.SubjectClassId;
            temp.subjectid = 0;
            temp.sitetype = 1;
            temp.text = sc.SubjectClassName;
            temp.nodes = GetMenuHtml(0, siteId, sc.SubjectClassId, 0);
            allList.Add(temp);

        }
        return allList;
    }

    ///<summary>
    /// 递归获取菜单 专题
    /// </summary>
    /// <param name="columnId"></param>
    /// <param name="siteId"></param>
    /// <param name="siteType"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    public List<ColumnTree> GetSubjectMenuHtml(int siteId)
    {

        List<ColumnTree> allList = new List<ColumnTree>();
        var listSubjectClass = ServiceFactory.SubjectClassService.GetSubjectClassList(siteId).ToList();

        foreach (SubjectClass sc in listSubjectClass)
        {
            ColumnTree temp = new ColumnTree();
            temp.columnid = sc.SubjectClassId;
            temp.subjectid = 0;
            temp.sitetype = 1;
            temp.text = sc.SubjectClassName;
            temp.nodes = GetMenuHtml(0, siteId, sc.SubjectClassId, 0);
            allList.Add(temp);

        }
        return allList;

    }
}


/// <summary>
/// 创建一个树形类
/// </summary>
public class ColumnTree
{
    public string text { get; set; }//显示名称
    public int columnid { get; set; }//栏目id
    public int sitetype { get; set; }//子站为1，专题为2，否则为0，默认为栏目内容管理
    public int subjectid { get; set; }//子站、专题id
    public List<ColumnTree> nodes { get; set; }//子节点
}


