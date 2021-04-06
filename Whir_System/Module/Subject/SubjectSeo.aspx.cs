
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

using Whir.Config;
using Whir.Config.Models;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;
using System.Collections.Specialized;

public partial class Whir_System_Module_Subject_SubjectSeo : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前编辑的站点ID
    /// </summary>
    protected int SiteId { get; set; }

    /// <summary>
    /// 固定值 - 1:子站, 2:专题
    /// </summary>
    protected int SubjectTypeId { get; set; }

    /// <summary>
    /// 当前编辑的子站/专题所属的类型ID
    /// 当前要添加到哪个类型下
    /// </summary>
    protected int SubjectClassId { get; set; }

    protected Subject CurrentSubject { get; set; }
    /// <summary>
    /// 当前编辑的子站/专题ID
    /// </summary>
    protected int SubjectId { get; set; }


    protected void Page_Load(object sender, EventArgs e)
    {
        SubjectTypeId = RequestUtil.Instance.GetQueryInt("subjecttypeid", 1);
        SubjectClassId = RequestUtil.Instance.GetQueryInt("subjectclassid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        SiteId = RequestUtil.Instance.GetQueryInt("siteid", CurrentSiteId);

        JudgePagePermission(IsRoleHaveSubjectRes("subject", "SEO设置", SubjectId, SiteId, SubjectId));

        CurrentSubject = ServiceFactory.SubjectService.SingleOrDefault<Subject>(SubjectId) ?? ModelFactory<Subject>.Insten();

    }







}