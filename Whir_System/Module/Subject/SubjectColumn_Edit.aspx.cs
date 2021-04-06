/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：subjectcolumn_edit.aspx.cs
 * 文件描述：专题/模板子站的栏目编辑页面
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;
using Whir.Language;
using Whir.Config;
using Whir.Config.Models;

public partial class whir_system_module_subject_subjectcolumn_edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前编辑的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 栏目父ID, 当前要添加在哪个栏目下面, 列表页上点击[添加子栏目]传入的栏目ID
    /// </summary>
    protected int ParentId { get; set; }

    /// <summary>
    /// 固定值 - 1:子站, 2:专题
    /// </summary>
    protected int SubjectTypeId { get; set; }

    /// <summary>
    /// 当前的子站/专题类型ID
    /// </summary>
    protected int SubjectClassId { get; set; }

    /// <summary>
    /// 当前修改的模板子站ID, 仅用于模板子站修改栏目
    /// </summary>
    protected int SubjectId { get; set; }

    protected bool BaseVisible { get; set; }
    protected bool SeoVisible { get; set; }

    /// <summary>
    /// 所有功能模型，用于绑定下拉框
    /// </summary>
    protected List<Model> AllModel { get; set; }

    /// <summary>
    /// 所有工作流，用户绑定下拉框
    /// </summary>
    protected List<WorkFlow> AllWorkFlow { get; set; }

    /// <summary>
    /// 所有栏目，用户绑定下拉框
    /// </summary>
    protected List<Column> AllColumn { get; set; }
    protected List<Column> ColumnTreeList { get; set; }

    protected Column CurrentColumn { get; set; }

    protected SubjectColumn SubColumn { get; set; }

    protected SystemConfig SystemConfig { get; set; }

    /// <summary>
    /// 可上传文件后缀名, 以英文逗号分隔开
    /// </summary>
    protected string EnableExtensionNames { get; private set; }

    /// <summary>
    /// 可上传文件后缀名, 'jpg','png','gif','bmp'
    /// </summary>
    protected string AllowPicType { get; private set; }

    /// <summary>
    /// 弹窗选文件只显示指定类型文件，格式： ".jpg,.png,.gif,.bmp"
    /// </summary>
    protected string AcceptPicType { get; private set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        ParentId = RequestUtil.Instance.GetQueryInt("parentid", 0);
        SubjectTypeId = RequestUtil.Instance.GetQueryInt("subjecttypeid", 1);
        SubjectClassId = RequestUtil.Instance.GetQueryInt("subjectclassid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        JudgePagePermission(IsRoleHaveColumnRes("栏目修改", ColumnId, SubjectId));
        AllWorkFlow = ServiceFactory.WorkFlowService.GetList().OrderBy(p => p.WorkFlowName).ToList();
        AllModel = ServiceFactory.ModelService.GetListByParentId(0)
            .Where(p => !p.IsSubsite)
            .OrderBy(p => p.ModelName).ToList();
        AllColumn = ServiceFactory.ColumnService.GetSubjectColumnList(0, SubjectClassId,SubjectId, false, ColumnId).ToList();

        ColumnTreeList = ServiceFactory.ColumnService.GetSubjectColumnList(0, SubjectClassId, SubjectId, false, ColumnId).ToList();

        CurrentColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId)
                        ?? ModelFactory<Column>.Insten();

        SubColumn = ServiceFactory.ColumnService.SingleOrDefault<SubjectColumn>("where ColumnId=@0 and SubjectId=@1", ColumnId, SubjectId)
           ?? ModelFactory<SubjectColumn>.Insten();

        if (!IsPostBack)
        {
            JurisdictionControl();  //控制权限
        }

        SystemConfig = ConfigHelper.GetSystemConfig();
        //上传控件参数
        UploadConfig uploadConfig = ConfigHelper.GetUploadConfig();
        foreach (string extension in uploadConfig.AllowPicType.Split('|')) {
            AllowPicType += "'" + extension + "'" + ",";
            EnableExtensionNames += extension + ",";
            AcceptPicType += "." + extension + ",";
        }
        AllowPicType = AllowPicType.TrimEnd(',');
        AcceptPicType = AcceptPicType.TrimEnd(',');
    }
    /// <summary>
    /// 控制权限
    /// </summary>
    private void JurisdictionControl()
    {
        if (!IsDevUser)
        {

            BaseVisible = Whir.Security.ServiceFactory.RolesService.IsRoleHaveColumnJurisdiction("栏目修改", ColumnId, CurrentUser.RolesId, CurrentSiteId);
            SeoVisible = Whir.Security.ServiceFactory.RolesService.IsRoleHaveColumnJurisdiction("SEO设置", ColumnId, CurrentUser.RolesId, CurrentSiteId);
        }
    }


}