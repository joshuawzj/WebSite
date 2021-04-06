/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：ContentManager.ascx.cs
 * 文件描述：公用的信息列表控件
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;
using Whir.ezEIP.Web;
using System.Text.RegularExpressions;
using Whir.Config.Models;


public partial class Whir_ContentManager : Whir.ezEIP.Web.SysControlBase
{

    #region 对外属性

    /// <summary>
    /// 栏目ID
    /// </summary>
    public int ColumnId { get; set; }

    /// <summary>
    /// 是否分类栏目
    /// </summary>
    public string IsMarkType { get; set; }

    /// <summary>
    /// 是否删除 数据库记录
    /// </summary>
    public bool IsDel { get; set; }

    /// <summary>
    /// 是否显示编辑按钮
    /// </summary>
    public bool IsShowEdit { get; set; }

    /// <summary>
    /// 是否显示查看按钮
    /// </summary>
    public bool IsShow{ get; set; }

    /// <summary>
    /// 编辑的页面地址
    /// </summary>
    public string EditPageUrl { get; set; }

    /// <summary>
    /// 工作流where语句
    /// </summary>
    public string Where { get; set; }

    /// <summary>
    /// 是否打开窗口的列表页
    /// </summary>
    public bool IsOpenFrame { get; set; }

    /// <summary>
    /// 是否显示删除按钮
    /// </summary>
    public bool IsShowDelete { get; set; }

    /// <summary>
    /// 是否显示排序
    /// </summary>
    public bool IsShowSort { get; set; }

    /// <summary>
    /// 是否显示排序窗口
    /// </summary>
    public bool IsShowOpenSort { get; set; }

    /// <summary>
    /// 是否显示批量审核
    /// </summary>
    public bool IsShowAudit { get; set; }

    /// <summary>
    /// 是否显示批量退审
    /// </summary>
    public bool IsShowReturned { get; set; }

    /// <summary>
    /// 是否显示批量推送
    /// </summary>
    public bool IsShowPush { get; set; }

    /// <summary>
    /// 是否显示批量转移
    /// </summary>
    public bool IsShowTransfer { get; set; }

    /// <summary>
    /// 绑定附属操作
    /// </summary>
    public bool IsShowAttchlist { get; set; }

    /// <summary>
    /// 是否显示历史记录
    /// </summary>
    public bool IsShowHistory { get; set; }

    /// <summary>
    /// 是否显示章节管理
    /// </summary>
    public bool IsShowChapter { get; set; }

    /// <summary>
    /// 是否显示文章管理
    /// </summary>
    public bool IsShowArticle { get; set; }

    /// <summary>
    /// 是否显示应聘
    /// </summary>
    public bool IsShowJobRequest { get; set; }

    /// <summary>
    /// 是否显示导入
    /// </summary>
    public bool IsShowImport { get; set; }
    /// <summary>
    /// 是否显示导出
    /// </summary>
    public bool IsShowExport { get; set; }
    /// <summary>
    /// 是否显示导出文档
    /// </summary>
    public bool IsShowExportDoc { get; set; }

    /// <summary>
    /// 是否显示查看详细
    /// </summary>
    public bool IsShowDetail { get; set; }
    /// <summary>
    /// 详细地址
    /// </summary>
    public string DetailUrl { get; set; }

    /// <summary>
    /// 是否显示答案
    /// </summary>
    public bool IsShowAnswer { get; set; }
    public bool IsShowSurveyAnswer { get; set; }
    /// <summary>
    /// 是否显示问题
    /// </summary>
    public bool IsShowQuestion { get; set; }
    public bool IsShowSurveyPreview { get; set; }
    public bool IsShowSurveyStatistics { get; set; }
    public bool IsShowSurveyDetail { get; set; }

    /// <summary>
    /// 是否显示预览
    /// </summary>
    public bool IsShowPreview { get; set; }
    /// <summary>
    /// 是否显示统计
    /// </summary>
    public bool IsShowStatistics { get; set; }
    /// <summary>
    /// 是否显示投票详细
    /// </summary>
    public bool IsShowVoteDetail { get; set; }
    /// <summary>
    /// 是否显示启用
    /// </summary>
    public bool IsShowEnable { get; set; }

    /// <summary>
    /// 选择类型
    /// </summary>
    public SelectedType SelectedType { get; set; }

    /// <summary>
    /// 显示对应的子站/专题记录。若不是子站或专题则值为0
    /// </summary>
    public int SubjectId { get; set; }
    /// <summary>
    ///是否显示退审节点
    /// </summary>
    public bool IsShowReturn = true;

    /// <summary>
    /// 是否以当前页面的权限设置为准，默认FALSE：以ContentManager.ascx为准，其他页面内容管理页面设置的均过期无效，看到对应的代码可以删掉
    /// </summary>
    public bool IsCurrentPageSetting = false;

    #endregion 对外属性

    #region 内部公用属性

    /// <summary>
    /// 前当流程节点ID
    /// </summary>
    protected int CurrentActivityId { get; set; }

    /// <summary>
    /// 所使用的主栏目实体
    /// </summary>
    protected Column MainColumn { get; private set; }

    /// <summary>
    /// 所使用到的主模型实体
    /// </summary>
    protected Model MainModel { get; private set; }

    /// <summary>
    /// 列表上显示的表单字段
    /// </summary>
    protected Dictionary<Form, Field> DictFormInList = new Dictionary<Form, Field>();

    /// <summary>
    /// 排序语句
    /// </summary>
    protected string OrderBy { get; set; }

    /// <summary>
    /// 记录总数
    /// </summary>
    protected int TotalItemsCount { get; private set; }

    /// <summary>
    /// 显示的列
    /// </summary>
    protected string Columns { get; set; }

    /// <summary>
    /// 主键
    /// </summary>
    protected string IdField { get; set; }

    protected int ChapterColumnId { get; set; }
    protected int ArticleColumnId { get; set; }
    protected int JobRequestColumnId { get; set; }
    protected int AnswerColumnId { get; set; }
    protected int VoteDetailColumnId { get; set; }
    protected int SurveyQuestionColumnId { get; set; }
    protected int SurveyDetailColumnId { get; set; }
    protected int SurveyAnswerColumnId { get; set; }
    protected string DeletString { get; set; }
    protected int MinPageSize { get; set; }
    protected int CurrentPage { get; set; }

    #endregion 内部公用属性

    #region 属性
    /// <summary>
    /// 工作流主键
    /// </summary>
    private int WorkFlowId
    {
        get
        {
            return ViewState["WorkFlowId"].ToInt();
        }
        set
        {
            ViewState["WorkFlowId"] = value;
        }
    }
  public string user { get; set; }
    #endregion

    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        Init();

        SetListPageAndPageSize();


        //1. 为公用属性赋值
        DeletString = "确定删除所选记录吗？".ToLang();
        CurrentActivityId = RequestUtil.Instance.GetQueryInt("flowid", 0);
        GetValueForProperties();
 user = RequestUtil.Instance.GetQueryString("user");
        if (ColumnId == 1)
        {
            SysManagePageBase.JudgePagePermission(SysManagePageBase.IsCurrentRoleMenuRes("7"));
        }
        else
        {
            if (IsMarkType.ToBoolean())
            {
                if (MainColumn != null)
                {
                    SysManagePageBase.JudgePagePermission(SysManagePageBase.IsRoleHaveColumnRes("类别管理", MainColumn.MarkParentId, SubjectId == 0 ? -1 : SubjectId));
                }
            }
            else
            {
                SysManagePageBase.JudgePagePermission(SysManagePageBase.IsRoleHaveColumnRes("查看", ColumnId, SubjectId == 0 ? -1 : SubjectId));
            }
        }
        if (!IsDel)
        {
            WorkFlowButton();
            ShowOperaTionControl();
        }

        if (DetailUrl.IsEmpty())
        {
            DetailUrl = "Details.aspx?columnid={0}&itemid={2}&subjectid={1}".FormatWith(ColumnId, SubjectId, "{itemid}");
        }
    }

    /// <summary>
    /// 设置最小分页数 记录当前分页数和页码 cookies["ezEIPListPage"]
    /// </summary>
    protected void SetListPageAndPageSize()
    {
        SystemConfig config = Whir.Config.ConfigHelper.GetSystemConfig();
        if (config != null && !config.PageSetting.IsEmpty())
        {
            string[] items = config.PageSetting.Trim().Split('|');
            if (items.Length > 0)
                MinPageSize = items[0].ToInt(10);
            else
                MinPageSize = 10;
        }
        else
            MinPageSize = 10;

        CurrentPage = RequestUtil.Instance.GetString("page").ToInt(1);
        //cookies里面pageSize优先 相当于每个列表页设置了一次分页数，其他栏目统一使用
         MinPageSize = CookieUtil.Instance.GetCookieValue("ezEIPListPageSize", "ezEIPListPageSize").ToInt(MinPageSize);
     
    }

    //页面权限初始化
    private void Init()
    {     //是否以当前页面的权限设置为准，true:则以当前页面设置为准，不执行下面代码
        if (IsCurrentPageSetting)
            return;

        if (ColumnId == 1)
        {
            IsShowDelete = SysManagePageBase.IsCurrentRoleMenuRes("301");
            IsShowEdit = SysManagePageBase.IsCurrentRoleMenuRes("300");
            IsShowOpenSort = IsShowSort = SysManagePageBase.IsCurrentRoleMenuRes("302");
        }
        else
        {
            IsShow = SysManagePageBase.IsRoleHaveColumnRes("查看");
            IsShowDelete = SysManagePageBase.IsRoleHaveColumnRes("删除");
            IsShowEdit = SysManagePageBase.IsRoleHaveColumnRes("修改");
            IsShowHistory = SysManagePageBase.IsRoleHaveColumnRes("历史记录");
            IsShowOpenSort = IsShowSort = SysManagePageBase.IsRoleHaveColumnRes("排序");
            IsShowImport = SysManagePageBase.IsRoleHaveColumnRes("导入");
            IsShowExport = SysManagePageBase.IsRoleHaveColumnRes("导出");
            IsShowEnable = SysManagePageBase.IsRoleHaveColumnRes("启用");
            IsShowPreview = SysManagePageBase.IsRoleHaveColumnRes("预览");
            IsShowStatistics = IsShowSurveyStatistics = SysManagePageBase.IsRoleHaveColumnRes("统计");
            IsShowPush = SysManagePageBase.IsRoleHaveColumnRes("批量推送");
            IsShowTransfer = SysManagePageBase.IsRoleHaveColumnRes("批量转移");
            IsShowChapter = SysManagePageBase.IsRoleHaveColumnRes("章节管理");
            IsShowArticle = SysManagePageBase.IsRoleHaveColumnRes("文章管理");
            IsShowQuestion = SysManagePageBase.IsRoleHaveColumnRes("问题管理");
            IsShowAnswer = SysManagePageBase.IsRoleHaveColumnRes("答案管理");
            IsShowSurveyDetail = SysManagePageBase.IsRoleHaveColumnRes("调查明细");
            IsShowJobRequest = SysManagePageBase.IsRoleHaveColumnRes("应聘信息");

            //以下注释的权限 暂时不适用内容管理列表，如果某个页面单独设置了，以页面的为准
            //IsShowDelete = SysManagePageBase.IsRoleHaveColumnRes("回复");
            //IsShowDelete = SysManagePageBase.IsRoleHaveColumnRes("类别管理");
            //IsShowDelete = SysManagePageBase.IsRoleHaveColumnRes("导入");
            //IsShowDelete = SysManagePageBase.IsRoleHaveColumnRes("复制");
            //IsShowDelete = SysManagePageBase.IsRoleHaveColumnRes("栏目删除"); 
            //IsShowDelete = SysManagePageBase.IsRoleHaveColumnRes("栏目修改");
            //IsShowDelete = SysManagePageBase.IsRoleHaveColumnRes("栏目移动");
            //IsShowDelete = SysManagePageBase.IsRoleHaveColumnRes("SEO设置");

        }


    }

    //为公用属性赋值
    private void GetValueForProperties()
    {
        //赋值主栏目实体
        MainColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);

        //赋值主模型实体
        if (MainColumn != null)
            MainModel = ServiceFactory.ModelService.SingleOrDefault<Model>(MainColumn.ModelId);
        else
            return;

        //赋值列表上显示的表单字段
        IList<Form> listFormListShow = ServiceFactory.FormService.GetListShowByColumnId(ColumnId);
        foreach (Form formListShow in listFormListShow)
        {
            Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(formListShow.FieldId);
            if (field != null)
                DictFormInList.Add(formListShow, field);
        }

        //根据URL参数的排序
        string queryOrderby = RequestUtil.Instance.GetQueryString("orderby").ToLower();
        string queryOrderType = RequestUtil.Instance.GetQueryString("ordertype").ToLower();
        if (!queryOrderby.IsEmpty())
        {
            queryOrderType = queryOrderType.IsEmpty() ? "desc" : queryOrderType;
            OrderBy = "{0} {1}".FormatWith(queryOrderby, queryOrderType);
        }

        string queryPrimaryKey = IdField = MainModel.TableName + "_PID";

        if (DictFormInList.Count > 0)
        {
            Columns += " columns: [{field: '" + queryPrimaryKey +
                       "',align: 'center',valign: 'middle',";
            if (SelectedType == SelectedType.RadioBox)
            {
                Columns += "radio:true";
            }
            else if (SelectedType == SelectedType.CheckBox)
            {
                Columns += "checkbox:true";
            }
            if(IsOpenFrame)
            {
                Columns += ",formatter: function(value, row, index) {return GetTitle(value, row, index);}";
            }
            if (IsShowSort && !IsOpenFrame)
                Columns += "},{ title: 'Id', field: 'Id', align: 'center', valign: 'middle',width: '50px', formatter: function (value, row, index) {return ForSort(value, row, index);}},";
            else
            {
                Columns += "},";
            }

            foreach (var formInList in DictFormInList)
            {
                #region 判断字段类型

                switch ((FieldType)formInList.Value.FieldType)
                {

                    case FieldType.Bool:
                        {
                            string json = "{\"0\": \"" + "否".ToLang() + "\",\"1\": \"" + "是".ToLang() + "\"}";
                            Columns += "{title: '" + formInList.Key.FieldAlias + "', field: '" +
                                       formInList.Value.FieldName +
                                       "',align: 'center',valign: 'middle',sortable:true,filterData:'json:" + json +
                                       "',filterControl:'" +
                                       formInList.Value.FieldType +
                                       "',formatter: function(value, row, index) {return GetBoolText(value, row, index);}},";

                        }
                        break;
                    case FieldType.ListBox:
                        {
                            string url = SysPath +
                                         "Handler/Common/Common.aspx?_action=GetSearchSelectOption&ColumnId=" + ColumnId +
                                         "&SubjectId=" + SubjectId + "&FormId=" + formInList.Key.FormId + "&Fieldid=" +
                                         formInList.Value.FieldId;
                            Columns += "{title: '" + formInList.Key.FieldAlias + "', field: '" +
                                       formInList.Value.FieldName +
                                       "',align: 'left',valign: 'middle',sortable:true,filterData:'url:" + url +
                                       "',filterControl:'" +
                                       formInList.Value.FieldType +
                                       "'},";
                        }
                        break;
                    case FieldType.Area:
                        {
                            Columns += "{title: '" + formInList.Key.FieldAlias + "', field: '" +
                                       formInList.Value.FieldName +
                                       "',align: 'center',valign: 'middle',sortable:true,filterControl:'" +
                                       formInList.Value.FieldType + "',leve:'" +
                                       GetAreaLeve(formInList.Key.FormId, formInList.Value.FieldType) + "'},";
                        }
                        break;
                    case FieldType.DateTime:
                        FormDate formDate = ServiceFactory.FormDateService.GetFormDateByFormID(formInList.Key.FormId) ?? new FormDate() { DateFormat = "yyyy-mm-dd" };

                        Columns += "{title: '" + formInList.Key.FieldAlias + "', field: '" +
                                   formInList.Value.FieldName +
                                   "',align: 'center',valign: 'middle',sortable:true,filterControl:'" +
                                   formInList.Value.FieldType + "',format:'" +
                                   formDate.DateFormat.ToDateTimeShow() + "'," +
                                   "formatter: function(value, row, index) {return GetDateTimeFormat(value, row, index,'" + formDate.DateFormat.ToDateTimeShow() + "');}},";
                        break;
                    default:
                        Columns += "{title: '" + formInList.Key.FieldAlias + "', field: '" +
                                   formInList.Value.FieldName +
                                   "',align: 'center',valign: 'middle',sortable:true,filterControl:'" +
                                   formInList.Value.FieldType + "'},";
                        break;

                }

                #endregion 判断字段类型

            }
            if ((IsShowEdit || IsShowDelete|| IsShow) && !IsOpenFrame)
            {
                Columns += "{title: '"+"操作".ToLang()+"',field: '" + IdField +
                           "',align: 'center',valign: 'middle',formatter: function(value, row, index) {return GetOperation(value, row, index);}}";
            }
            else
            {
                Columns = Columns.Substring(0, Columns.Length - 1);
            }
            Columns += "]";
        }
    }

    /// <summary>
    /// 审核按钮显示
    /// </summary>
    private void WorkFlowButton()
    {
        //判断是否有权限
        bool hadPower = SysManagePageBase.IsCurrentRoleWorkFolw(-1);
        IsShowAudit = hadPower;
        IsShowReturned = hadPower && IsShowReturn && CurrentActivityId > 0;

        //是否开启了工作流
        bool isUseWorkFolw = ServiceFactory.WorkFlowService.IsUseWorkFlow(ColumnId);
        DeletString = "确定删除所选记录吗？".ToLang();
        if (isUseWorkFolw)
        {
            DeletString = "该栏目启用了工作流，选择的记录里若没权限将无法删除".ToLang();
        }
    }


    //显示列表操作按钮
    protected void ShowOperaTionControl()
    {

        //配置章节、文章管理连接
        if (IsShowChapter)
        {
            IList<Column> columns = ServiceFactory.ColumnService.GetListByMainColumnId(ColumnId);
            var firstOrDefault = columns.FirstOrDefault(p => p.MarkType == "Chapter");
            if (firstOrDefault != null)
                ChapterColumnId = firstOrDefault.ColumnId;
            else
            {
                IsShowChapter = false;
            }
        }
        if (IsShowArticle)
        {
            IList<Column> columns = ServiceFactory.ColumnService.GetListByMainColumnId(ColumnId);
            var orDefault = columns.FirstOrDefault(p => p.MarkType == "Infor");
            if (orDefault != null)
                ArticleColumnId = orDefault.ColumnId;
            else
            {
                IsShowArticle = false;
            }
        }
        if (IsShowJobRequest)
        {
            Column column =
                ServiceFactory.ColumnService.GetMarkListByColumnId(ColumnId)
                    .FirstOrDefault(p => p.MarkType == "JobRequest");
            if (column != null)
            {
                JobRequestColumnId = column.ColumnId;

            }
            else
            {
                IsShowJobRequest = false;
            }
        }
        //投票
        if (IsShowAnswer)
        {
            Column answercolumn =
                ServiceFactory.ColumnService.GetListByMainColumnId(ColumnId).FirstOrDefault(p => p.MarkType == "Answer");
            if (answercolumn == null)
            {
                IsShowAnswer = false;
            }
            else
            {
                AnswerColumnId = answercolumn.ColumnId;
            }

        }
        if (IsShowVoteDetail)
        {
            Column column =
                ServiceFactory.ColumnService
                    .GetListByMainColumnId(ColumnId)
                    .FirstOrDefault(p => p.MarkType == "Detail");
            if (column != null)
            {
                VoteDetailColumnId = column.ColumnId;
            }
            else
            {
                IsShowVoteDetail = false;
            }
        }
        if (IsShowQuestion)
        {
            Column column =
                ServiceFactory.ColumnService.GetListByMainColumnId(ColumnId)
                    .FirstOrDefault(p => p.MarkType == "Question");
            if (column == null)
            {
                IsShowQuestion = false;
            }
            else
            {
                SurveyQuestionColumnId = column.ColumnId;
            }
        }
        if (IsShowSurveyDetail)
        {
            Column column = ServiceFactory.ColumnService.GetListByMainColumnId(ColumnId).FirstOrDefault(p => p.MarkType == "Detail");
            if (column == null)
            {
                IsShowSurveyDetail = false;
            }
            else
            {
                SurveyDetailColumnId = column.ColumnId;
            }
        }

        if (IsShowSurveyAnswer)
        {
            Column column = ServiceFactory.ColumnService.GetMarkListByColumnId(ColumnId).FirstOrDefault(p => p.MarkType == "Answer");
            if (column == null)
            {
                IsShowSurveyAnswer = false;
            }
            else
            {
                SurveyAnswerColumnId = column.ColumnId;
            }
        }
    }

    //获取日期格式 不用了
    public string GetFormat(int formId, int fieldType)
    {
        if (fieldType == (int)FieldType.DateTime)
        {
            FormDate formdate = ServiceFactory.FormDateService.GetFormDateByFormID(formId);
            if (formdate != null)
            {
                if (formdate.DateFormat == "yyyy-MM-dd HH:mm:ss")
                {
                    return "yyyy-mm-dd hh:ii:ss";
                }
                else
                {
                    return formdate.DateFormat.ToLower();
                }
            }
            else
            {
                return "yyyy-mm-dd";
            }
        }
        else
        {
            return "";
        }

    }

    //获取区域设置
    public int GetAreaLeve(int formId, int fieldType)
    {
        if (fieldType == (int)FieldType.Area)
        {
            FormArea formArea = ServiceFactory.FormAreaService.GetFormAreaByFormID(formId);
            if (formArea != null)
            {
                return formArea.ShowLevel;
            }
            else
            {
                return 3;
            }
        }
        else
        {
            return 3;
        }
    }
}