/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：DynamicForm.ascx.cs
 * 文件描述：公用的信息提交控件
 */

using System;
using System.Collections.Generic;
using System.Data;

using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;
using Whir.ezEIP.Web;

public partial class whir_system_UserControl_ContentControl_DynamicForm : Whir.ezEIP.Web.SysControlBase
{
    /// <summary>
    /// 栏目ID
    /// </summary>
    public int ColumnId { get; set; }

    /// <summary>
    /// 所属子站ID
    /// </summary>
    public int SubjectId { get; set; }

    /// <summary>
    /// 编辑状态下的主键ID
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// 是否弹出窗口
    /// </summary>
    public bool IsOpenFrame { get; set; }

    /// <summary>
    /// 表单类型
    /// </summary>
    public DynamicFormType FormType { get; set; }

    /// <summary>
    /// 是否单页图文 用来显示历史按钮
    /// </summary>
    public bool IsSinglePage { get; set; }

    /// <summary>
    /// 剔除不显示在动态表单里的表单，多个用英文“,”符号分离
    /// </summary>
    public string ExceptFields { get; set; }

    /// <summary>
    /// 当前编辑的数据行
    /// </summary>
    protected DataRow EditRow { get; set; }

    /// <summary>
    /// 后台目录名
    /// </summary>
    protected string SystemPath = AppSettingUtil.AppSettings["SystemPath"];

    /// <summary>
    /// 渲染控件Html
    /// </summary>
    protected string LeftHtml { get; set; }

    /// <summary>
    /// 渲染控件Html
    /// </summary>
    protected string RightHtml { get; set; }

    /// <summary>
    /// 提交表单js
    /// </summary>
    protected string SubmitFormScript { get; set; }

    /// <summary>
    /// 是否启用相关文章
    /// </summary>
    protected bool IsRelated { get; set; }

    /// <summary>
    /// 相关文章列表
    /// </summary>
    protected string RelatedList { get; set; }

    /// <summary>
    /// 是否显示开启流程按钮
    /// </summary>
    protected bool IsSaveToFlow { get; set; }

    /// <summary>
    /// 是否显示审核日志
    /// </summary>
    protected bool IsShowWorkFlowLogs { get; set; }

    /// <summary>
    /// 栏目名称
    /// </summary>
    protected string ColumnName { get; set; }

    /// <summary>
    /// 当前栏目
    /// </summary>
    public Column Column { get; set; }

    /// <summary>
    /// 显示审核状态文字 单篇
    /// </summary>
    public string SinglePageFlowStatusStr { get; set; }

    /// <summary>
    /// 显示审核状态 单篇
    /// </summary>
    public int SinglePageFlowStatus { get; set; }

    /// <summary>
    /// 用于判断是否显示单篇的审核按钮
    ///  </summary>
    public bool IsShowSinglePageFlowBtn { get; set; }

    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (ColumnId == 1)
            {
                SysManagePageBase.JudgePagePermission(SysManagePageBase.IsCurrentRoleMenuRes("295") || IsCurrentRoleMenuRes("300"));
            }
            else
            {
                if (IsSinglePage)
                {
                    SysManagePageBase.JudgePagePermission(SysManagePageBase.IsRoleHaveColumnRes("修改", ColumnId, SubjectId == 0 ? -1 : SubjectId));
                }


            }
            EditRow = ServiceFactory.DynamicFormService.GetEditRow(ColumnId, ItemId);

        }
        Column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
        if (Column != null)
        {
            bindRelation();
            BindForm();
            ShowReturnToFlow();
            IsSinglePageFlow();
        }
    }

    /// <summary>
    /// 单篇的审核按钮,显示“未审核" 文字
    /// </summary>
    /// <returns></returns>
    protected void IsSinglePageFlow()
    {
        IsShowSinglePageFlowBtn = false;
        if (Column.WorkFlow > 0)
        {
            Model model = ServiceFactory.ModelService.GetModelByColumnId(ColumnId);
            if (model != null)
            {
                string sql = "";
                if (model.ModuleMark == "SinglePage_v0.0.01")
                {
                    //IsShowSinglePageFlowBtn = ServiceFactory.WorkFlowService.IsCanEdit(ColumnId, SubjectId, model.TableName, model.TableName + "_PID", ItemId, CurrentUser.RolesId, CurrentUserName, out sql);
                    //当前工作流的所有节点，已排好序
                    List<AuditActivity> AuditActivityList = ServiceFactory.AuditActivityService.GetListBySort(Column.WorkFlow);
                    if (AuditActivityList.Count > 0)
                        IsShowSinglePageFlowBtn = ServiceFactory.WorkFlowService.IsPassUserRoldFunc(ColumnId, SubjectId, CurrentUser.RolesId, AuditActivityList[0].ActivityId);

                    SinglePageFlowStatus = EditRow == null ? 0 : EditRow["State"].ToInt();
                    SinglePageFlowStatusStr = SinglePageFlowStatus == 0 ?    //0-未审核，-1 审核通过 ，-2 退审
                                              "<a class='entypo-cancel ' style='color:#d9534f'> {0}</a> ".FormatWith("未审核".ToLang()) :
                                              SinglePageFlowStatus == -1 ?
                                              "<a class='entypo-check '> {0}</a>".FormatWith("审核通过".ToLang()) :
                                              "<a class='entypo-cancel ' style='color:#d9534f'> 被退审</a>".FormatWith("被退审".ToLang());
                }
            }
        }
    }

    /// <summary>
    /// 是否显示退审转正常流程按钮
    /// </summary>
    private void ShowReturnToFlow()
    {
        if (Column.WorkFlow > 0)
        {
            Model model = ServiceFactory.ModelService.GetModelByColumnId(ColumnId);
            if (model != null)
            {
                //表名
                string tableName = model.TableName;
                string primaryKeyName = tableName + "_PID";
                IsSaveToFlow = ServiceFactory.WorkFlowService.IsCanReturnToFlow(ColumnId, tableName, primaryKeyName, ItemId, CurrentUser.RolesId, CurrentUserName);
            }
        }
    }

    //绑定表单
    private void BindForm()
    {
        string options = "var _options = \n";
        string onlyValid = "";
        string clickFun = "";
        string isOnlyValid = "false";//判断是否有 不可重复的值 的字段 默认为false 没有
        IList<Form> listLeftForm = new List<Form>();
        IList<Form> listRightForm = new List<Form>();

        if (Column != null)
        {
            ColumnName = Column.ColumnName;
        }
        else
            return;
        if (SubjectId > 0)
        {
            string name = ServiceFactory.SubjectColumnService.GetColumnName(ColumnId, SubjectId);
            ColumnName = name.IsEmpty() ? ColumnName : name;
        }

        int workFlowId = Column != null ? Column.WorkFlow : 0;

        if (workFlowId > 0)
        {
            IsShowWorkFlowLogs = true;
            rptList.DataSource = ServiceFactory.WorkFlowLogsService.GetItemList(ColumnId, ItemId);
            rptList.DataBind();
        }

        if (Column != null)
        {
            IsRelated = Column.IsRelated;
        }
        if (FormType == DynamicFormType.Left)
        {
            listLeftForm = ServiceFactory.FormService.GetMainListByColumnId(ColumnId, ExceptFields);
        }
        else if (FormType == DynamicFormType.Right)
        {
            listRightForm = ServiceFactory.FormService.GetAttachListByColumnId(ColumnId);
        }
        else if (FormType == DynamicFormType.LeftAndRight)
        {
            listLeftForm = ServiceFactory.FormService.GetMainListByColumnId(ColumnId, ExceptFields);
            listRightForm = ServiceFactory.FormService.GetAttachListByColumnId(ColumnId);
        }
        else
        {
            FormType = DynamicFormType.All;
            listLeftForm = ServiceFactory.FormService.GetListNoTypeIdAndSubjectId(ColumnId, ExceptFields);
        }
        foreach (var item in listLeftForm)
        {
            Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(item.FieldId);

            if (Column != null && Column.MarkType == "Category")  //如果是类别管理页面
            {
                var MainColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(Column.MarkParentId);
                if (MainColumn != null)
                {
                    if (MainColumn.IsCategory && MainColumn.CategoryLevel <= 1)  //只有一级，则不显示父节点
                    {
                        if (item.FieldAlias == "父类别")
                            field.IsHidden = true;
                    }
                }
            }
            else if (Column.ModuleMark == "Category_v0.0.01")
            {
                var MainColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(Column.ColumnId);
                if (MainColumn != null)
                {
                    if (MainColumn.CategoryLevel <= 1) //只有一级，则不显示父节点
                    {
                        if (item.FieldAlias == "父类别")
                            field.IsHidden = true;
                    }

                }
            }

            var val = EditRow != null ? EditRow[field.FieldName].ToStr() : "";
            if (field.FieldType == 14) //加密字段，需要解密后的值
            {
                val = TripleDESUtil.Decrypt(val);
            }
            LeftHtml += Whir.Controls.UI.Controls.RenderControl.RenderSingleControl(Column, item, field, DynamicFormType.Left, val, EditRow);
            if ((FieldType)field.FieldType == FieldType.Text && item.IsOnly)
            {
                if (onlyValid.IsEmpty())
                {
                    onlyValid += "{fields: { \n";
                }
                onlyValid += "    {0}: {1} \n".FormatWith(field.FieldName, "{");
                onlyValid += "        validators: { \n";
                onlyValid += "            remote: { \n";
                onlyValid += "               message: '{0}', \n".FormatWith(item.FieldAlias + "为不可重复的值".ToLang());
                onlyValid += "                  url : '{0}ajax/content/onlyValid.aspx', \n".FormatWith(SysPath);
                //onlyValid += "                delay :  500,";//延迟执行，减低服务器压力
                onlyValid += "                  data: { \n";
                onlyValid += "             columnid : '{0}', primaryValue : '{1}', fieldName : '{2}', fieldValue : function () {{ return $('#txt{3}').val();     bootstrapValidator.updateStatus('txt{3}', 'VALID');}} \n"
                        .FormatWith(ColumnId, ItemId, field.FieldName, field.FieldName);
                onlyValid += "               } \n";
                onlyValid += "           } \n";
                onlyValid += "       } \n";
                onlyValid += "   }, \n";

            }
        }
        foreach (var item in listRightForm)
        {
            Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(item.FieldId);

            RightHtml += Whir.Controls.UI.Controls.RenderControl.RenderSingleControl(Column, item, field, DynamicFormType.Right, EditRow != null ? EditRow[field.FieldName].ToStr() : "", EditRow);
            if ((FieldType)field.FieldType == FieldType.Text && item.IsOnly)
            {
                if (onlyValid.IsEmpty())
                {
                    onlyValid += "{fields: { \n";
                }
                onlyValid += "    {0}: {1} \n".FormatWith(field.FieldName, "{");
                onlyValid += "        validators: { \n";
                onlyValid += "            remote: { \n";
                onlyValid += "               message: '{0}', \n".FormatWith(item.FieldAlias + "为不可重复的值".ToLang());
                onlyValid += "                 url : '{0}ajax/content/onlyValid.aspx', \n".FormatWith(SysPath);
                onlyValid += "                data: { \n";
                onlyValid +=
                    "                   columnid : '{0}', primaryValue : '{1}', fieldName : '{2}', fieldValue : $('#txt{3}').val(), time : new Date() \n"
                        .FormatWith(ColumnId, ItemId, field.FieldName, field.FieldName);
                onlyValid += "               } \n";
                onlyValid += "           } \n";
                onlyValid += "       } \n";
                onlyValid += "   }, \n";

            }
        }
        if (!onlyValid.IsEmpty())
        {
            isOnlyValid = "true";
            onlyValid = options + onlyValid.TrimEnd(',');
            onlyValid += "       } \n";
            onlyValid += "       }; \n";
        }
        else
        {
            isOnlyValid = "false";
            onlyValid = options + "{};";
        }
        clickFun += onlyValid;
        clickFun += "$('#formEdit').Validator(_options, \n";
        clickFun += "     function (el) { \n";
        clickFun += "         var actionSuccess = el.attr('form-success'); \n";
        clickFun += "         var $form = $('#formEdit');\n";
        clickFun += "         $form.post({\n";
        clickFun += "             success: function (response) {\n";
        clickFun += "                 if (response.Status == true) {\n";
        clickFun += "                   actionSuccess == \"refresh\" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, \"{0}\"); \n".FormatWith(RequestUtil.Instance.GetQueryString("BackPageUrl"));
        clickFun += "                 } else {\n";
        clickFun += "                     whir.toastr.error(response.Message);\n";
        clickFun += "                 }\n";
        clickFun += "                 whir.loading.remove();\n";
        clickFun += "             },\n";
        clickFun += "             error: function (response) {\n";
        clickFun += "                 whir.toastr.error(response.responseText);\n";
        clickFun += "                 whir.loading.remove();\n";
        clickFun += "             }\n";
        clickFun += "         });\n";
        clickFun += "         return false;\n";
        clickFun += "        }";
        clickFun += "     {0});\n".FormatWith("," + isOnlyValid);
        SubmitFormScript = clickFun;
    }

    //绑定相关文章
    private void bindRelation()
    {
        if (Column != null)
        {
            if (Column.IsRelated)
            {
                IList<Relation> listRelation = ServiceFactory.RelationService.GetList(ItemId, ColumnId);
                string json = "[";
                foreach (Relation relation in listRelation)
                {
                    if (relation.AttachPrimaryId != 0 && relation.AttachColumnId != 0)
                    {
                        string title = ServiceFactory.RelationService.GetRelationTitle(relation.AttachPrimaryId, relation.AttachColumnId);
                        json += "{";
                        json += "pID : \"{0}\",".FormatWith(relation.AttachPrimaryId);
                        json += "columnID : \"{0}\",".FormatWith(relation.AttachColumnId);
                        json += "title : \"{0}\"".FormatWith(title);
                        json += "},";
                    }
                    else
                    {
                        json += "{";
                        json += "pID : \"{0}\",".FormatWith(relation.AttachPrimaryId);
                        json += "columnID : \"{0}\",".FormatWith(relation.AttachColumnId);
                        json += "linkText : \"{0}\",".FormatWith(relation.LinkTitle);
                        json += "linkUrl : \"{0}\"".FormatWith(relation.LinkUrl);
                        json += "},";
                    }
                }
                json = json.TrimEnd(',');
                json += "]";

                RelatedList = json.Replace("'", "&apos;");
                //string script = "addRelation('{0}'); \n".FormatWith(json);
                //Page.ClientScript.RegisterStartupScript(typeof(string), this.ClientID, script, true);
            }
        }
    }

    /// <summary>
    /// 检测文本中包含敏感词的文字, 忽略大小写
    /// </summary>
    /// <param name="listWord"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private string IsWord(IList<SensitiveWord> listWord, string value)
    {
        string result = string.Empty;
        foreach (SensitiveWord word in listWord)
        {
            if (value.ToLower().Contains(word.SensitiveWordName.ToLower()))
                result += word.SensitiveWordName;
        }

        return result;
    }

    /// <summary>
    /// 用于判断当前角色是否有当前栏目的编辑权限
    /// </summary>
    /// <returns></returns>
    protected bool IsShowBtn()
    {
        if (IsOpenFrame)
            return false;
        if (SysManagePageBase.IsDevUser)
            return true;

        if (ColumnId == 1)
        {
            if (ItemId > 0 && IsCurrentRoleMenuRes("300"))
                return true;
            else if (ItemId == 0 && IsCurrentRoleMenuRes("295"))
                return true;
            else
                return false;
        }
        else if (ColumnId != 1)
        {
            if (IsSinglePage && new Whir.ezEIP.Web.SysManagePageBase().IsRoleHaveColumnRes("修改"))
                return true;
            else if (ItemId > 0 && new Whir.ezEIP.Web.SysManagePageBase().IsRoleHaveColumnRes("修改"))
                return true;
            else if (ItemId == 0 && new Whir.ezEIP.Web.SysManagePageBase().IsRoleHaveColumnRes("添加"))
                return true;
            else
                return false;

        }
        return false;
    }

    /// <summary>
    /// 用于判断是否显示分享按钮
    /// </summary>
    /// <returns></returns>
    protected bool IsShowShare()
    {
        if (EditRow != null && Column != null && Column.CreateMode != 0
            && (Column.ContentTemp.IsNotEmpty() || (Column.ModelId == 83 && Column.DefaultTemp.IsNotEmpty()))
            && !EditRow["IsDel"].ToBoolean() && (Column.WorkFlow == 0 || (Column.WorkFlow > 0 && EditRow["State"].ToInt() == -1)))
            return true;
        return false;
    }
}