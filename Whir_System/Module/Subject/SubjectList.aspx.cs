/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：subjectlist.aspx.cs
 * 文件描述：专题/模板子站的列表页面
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class Whir_System_Module_Subject_SubjectList : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 固定值 - 1:子站, 2:专题
    /// </summary>
    protected int SubjectTypeId { get; set; }

    public List<string> GuangliLink = new List<string>();
    public List<string> AddLink = new List<string>();

    public List<SubjectClass> SubjectClass = new List<SubjectClass>();
    public bool IsShowSort { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser || IsSuperUser);
        SubjectTypeId = RequestUtil.Instance.GetQueryInt("subjecttypeid", 1);
        if (SubjectTypeId == 1)
        {
            GuangliLink.Add("子站管理".ToLang());
            GuangliLink.Add("SubjectList.aspx?subjecttypeid=" + SubjectTypeId);
            AddLink.Add("添加子站类型".ToLang());
            AddLink.Add("SubjectcLass_Edit.aspx?subjecttypeid=" + SubjectTypeId);
        }
        else
        {
            GuangliLink.Add("专题管理".ToLang());
            GuangliLink.Add("subjectlist.aspx?subjecttypeid=" + SubjectTypeId);
            AddLink.Add("添加专题类型".ToLang());
            AddLink.Add("SubjectcLass_Edit.aspx?subjecttypeid=" + SubjectTypeId);
        }

        if (!IsPostBack)
        {
            BindSubjectClassList();
            
        }
    }
    

    //绑定子站/专题类型
    private void BindSubjectClassList()
    {
        IList<SubjectClass> list = new List<SubjectClass>();
        if (SubjectTypeId == 1) //查出所有的子站类型
            list = ServiceFactory.SubjectClassService.GetSubsiteClassList(CurrentSiteId);
        else if (SubjectTypeId == 2)//查出所有的专题类型
            list = ServiceFactory.SubjectClassService.GetSubjectClassList(CurrentSiteId);

        SubjectClass = (List<SubjectClass>) list;
    }

    /// <summary>
    /// 获取下级栏目
    /// </summary>
    /// <param name="allColumn"></param>
    /// <param name="parentId"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public List<Column> GetNodeColumns(IList<Column> allColumn, int parentId, int level)
    {
        var columns = new List<Column>();
        foreach (var column in allColumn.Where(p => p.ParentId == parentId))
        {
            column.LevelNum = level;
            columns.Add(column);
            columns.AddRange(GetNodeColumns(allColumn, column.ColumnId, level + 1));
        }
        return columns;
    }

 



 


   




}