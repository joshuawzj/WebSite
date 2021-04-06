/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：subject_edit.aspx.cs
 * 文件描述：专题/模板子站编辑页面
 */

using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Language;

public partial class whir_system_module_subject_subject_edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 固定值 - 1:子站, 2:专题
    /// </summary>
    protected int SubjectTypeId { get; set; }

    /// <summary>
    /// 当前编辑的子站/专题所属的类型ID
    /// 当前要添加到哪个类型下
    /// </summary>
    protected int SubjectClassId { get; set; }

    /// <summary>
    /// 当前编辑的子站/专题ID
    /// </summary>
    protected int SubjectId { get; set; }

    public List<string> GuangliLink = new List<string>();
    public List<string> AddLink = new List<string>();
    protected Subject CurrentSubject { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser||IsSuperUser);
        SubjectTypeId = RequestUtil.Instance.GetQueryInt("subjecttypeid", 1);
        SubjectClassId = RequestUtil.Instance.GetQueryInt("subjectclassid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        if (SubjectTypeId == 1)
        {
            GuangliLink.Add("子站管理".ToLang());
            AddLink.Add("添加子站类型".ToLang());
        }
        else
        {
            GuangliLink.Add("专题管理".ToLang());
            AddLink.Add("添加专题类型".ToLang());
        }
        AddLink.Add("SubjectClass_Edit.aspx?subjecttypeid=" + SubjectTypeId);
       

        if (!IsPostBack)
        {
            BindSubjectInfo();
        }
    }


    //绑定子站/专题信息
    private void BindSubjectInfo()
    {
        CurrentSubject = ServiceFactory.SubjectService.SingleOrDefault<Subject>(SubjectId)??ModelFactory<Subject>.Insten();
    }

  
}