/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：subjectclass_edit.aspx.cs
 * 文件描述：专题/模板子站的模板类型编辑页面
 */

using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class whir_system_module_subject_subjectclass_edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 固定值 - 1:子站, 2:专题
    /// </summary>
    protected int SubjectTypeId { get; set; }

    /// <summary>
    /// 当前编辑的子站/专题类型ID
    /// </summary>
    protected int SubjectClassId { get; set; }

    public List<string> GuangliLink = new List<string>();
    protected SubjectClass CurrentSubjectClass { get; set; }
    protected Column CurrentColumn { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser||IsSuperUser);
        SubjectTypeId = RequestUtil.Instance.GetQueryInt("subjecttypeid", 1);
        SubjectClassId = RequestUtil.Instance.GetQueryInt("subjectclassid", 0);

        if (SubjectTypeId == 1)
        {
            GuangliLink.Add("子站类型管理".ToLang());
        }
        else
        {
            GuangliLink.Add("专题类型管理".ToLang());
        }
        GuangliLink.Add("subjectlist.aspx?subjecttypeid=" + SubjectTypeId);


        if (!IsPostBack)
        {
            BindSubjectClassInfo();
        }
    }

    //绑定要编辑的子站/专题类型信息
    private void BindSubjectClassInfo()
    {
        CurrentSubjectClass = ServiceFactory.SubjectClassService.SingleOrDefault<SubjectClass>(SubjectClassId) ??
                              ModelFactory<SubjectClass>.Insten();
        CurrentColumn = ServiceFactory.ColumnService.GetSubjectIndexColumn(CurrentSubjectClass.SubjectClassId) ?? 
                        ModelFactory<Column>.Insten();
    }


}