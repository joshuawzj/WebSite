using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Xml;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Repository;
using Whir.Security.Service;
using Whir.Service;


public partial class Whir_System_Handler_Config_Common : SysHandlerPageBase
{
    /// <summary>
    /// 栏目ID
    /// </summary>
    public int ColumnId { get; set; }

    /// <summary>
    /// 条件语句
    /// </summary>
    public string Where { get; set; }

    /// <summary>
    /// 排序字段
    /// </summary>
    public string SortField { get; set; }

    /// <summary>
    /// 排序类型
    /// </summary>
    public string OrderType { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDel { get; set; }

    /// <summary>
    /// 选择类型
    /// </summary>
    public SelectedType SelectedType { get; set; }

    /// <summary>
    /// 显示对应的子站/专题记录。若不是子站或专题则值为0
    /// </summary>
    public int SubjectId { get; set; }

    /// <summary>
    /// 当前页码
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 每页显示数量
    /// </summary>
    public int PageSize { get; set; }

    #region 内部公用属性

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

    #endregion 内部公用属性
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetString("ColumnId").ToInt();
        PageSize = RequestUtil.Instance.GetString("limit").ToInt(10);
        PageIndex = RequestUtil.Instance.GetString("offset").ToInt(0) / PageSize + 1;
        SubjectId = RequestUtil.Instance.GetString("SubjectId").ToInt();
        IsDel = RequestUtil.Instance.GetString("IsDel").ToBoolean();
        Where = RequestUtil.Instance.GetString("Where");
        SortField = RequestUtil.Instance.GetString("sort");
        OrderType = RequestUtil.Instance.GetString("order");
        var action = RequestUtil.Instance.GetString("_action");
        switch (action)
        {
            case "InportJobRequest":
                InportJobRequest();
                break;
            case "InportJobRequestAll":
                InportJobRequestAll();
                break;
            case "GetList":
                GetList();
                break;
            case "GetHistoryBakList":
                GetHistoryBakList();
                break;
            default:
                Exec(this, action);
                break;
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

        //赋值列表上显示的表单字段
        IList<Form> listFormListShow = ServiceFactory.FormService.GetListShowByColumnId(ColumnId);
        if (RequestUtil.Instance.GetString("_action") == "GetHistoryBakList")
        {
            listFormListShow = ServiceFactory.FormService.GetHistoryListShowByColumnId(ColumnId);
        }
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
    }

    //填充数据
    public void GetList()
    {
        //权限判断
        if (IsDel)
        {
            SysManagePageBase.JudgeActionPermission(ColumnId == 1 ? SysManagePageBase.IsCurrentRoleMenuRes("296") : SysManagePageBase.IsRoleHaveColumnRes("回收站", ColumnId, SubjectId == 0 ? -1 : SubjectId), true);
        }
        else
        {
            if (RequestUtil.Instance.GetString("IsMarkType") == "true")
            {
                MainColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
                if (MainColumn != null)
                {
                    SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("类别管理", MainColumn.MarkParentId, SubjectId == 0 ? -1 : SubjectId), true);
                }
            }
            else
            {
                SysManagePageBase.JudgeActionPermission(ColumnId == 1 ? SysManagePageBase.IsCurrentRoleMenuRes("7") : SysManagePageBase.IsRoleHaveColumnRes("查看", ColumnId, SubjectId == 0 ? -1 : SubjectId), true);
            }
        }

        DataTable dt = new DataTable();
        long total = 0;
        GetValueForProperties();
        string filter = RequestUtil.Instance.GetQueryString("filter");
        Dictionary<string, string> searchDic = ToDictionary(filter);

        if (RequestUtil.Instance.GetString("IsMarkType") == "true") //判断当前栏目是否 分类栏目
        {
            //单独类别权限控制 用于读取分类页面数据
            int currentUserRolesId = AuthenticateHelper.User == null ? 0 : AuthenticateHelper.User.RolesId;
            var pd = GetCategoryTable(PageIndex, PageSize, ColumnId, SubjectId, currentUserRolesId, searchDic);
            if (pd.Items.Tables.Count > 0)
            {
                dt = pd.Items.Tables[0];
                total = pd.TotalItems;
            }
        }
        else
        {
            var parms = new List<object>();
            if (Where.IsEmpty())
            {
                Where = "";
            }
            if (MainColumn == null || MainModel == null)
            {
                return;
            }

            string sql = "SELECT {0} FROM {1} WHERE TypeID={2} AND IsDel={3} {4} ORDER BY {5}";

            //1. 表名 & 主键
            string queryTableName = MainModel.TableName;
            string queryPrimaryKey = queryTableName + "_PID";

            #region 2. 要查询的列
            string queryColumns = queryPrimaryKey + "," + queryPrimaryKey + " as Id " + "  ";

            if (DictFormInList.Count > 0)
            {
                int i = 0;
                foreach (var formInList in DictFormInList)
                {
                    queryColumns += ",{0}".FormatWith(ServiceFactory.DbService.GetColumnName(formInList.Value.FieldName));
                    foreach (var kv in searchDic)
                    {
                        if (kv.Key.ToLower() == formInList.Value.FieldName.ToLower())
                        {
                            switch ((FieldType)formInList.Value.FieldType)
                            {
                                case FieldType.DateTime:
                                    {
                                        if (kv.Value.IndexOf("<&&<", StringComparison.Ordinal) > -1)
                                        {
                                            Where += " and {0} between @{1} and @{2} ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key),
                                                i,
                                                i + 1);
                                            parms.Add(Regex.Split(kv.Value, "<&&<")[0]);
                                            i++;
                                            parms.Add(Regex.Split(kv.Value, "<&&<")[1]);
                                            i++;
                                        }
                                    }
                                    break;
                                case FieldType.Number:
                                case FieldType.Money:
                                    {
                                        if (kv.Value.IndexOf("<&&<", StringComparison.Ordinal) > -1)
                                        {
                                            Where += " and {0} between @{1} and @{2} ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key),
                                                i,
                                                i + 1);
                                            parms.Add(Regex.Split(kv.Value, "<&&<")[0]);
                                            i++;
                                            parms.Add(Regex.Split(kv.Value, "<&&<")[1]);
                                            i++;
                                        }
                                    }
                                    break;
                                case FieldType.Area:
                                    {
                                        Where += " and {0} = '{1}' ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), kv.Value);
                                        parms.Add(kv.Value);
                                        i++;
                                    }
                                    break;
                                case FieldType.ListBox:
                                    {
                                        var formOption = ServiceFactory.FormOptionService.GetFormOptionByFormID(formInList.Key.FormId.ToInt());
                                        if ((formOption.BindType == 3 || formOption.BindType == 4) && MainModel.ModuleMark != "Category_v0.0.01")//绑定多级类别
                                        {
                                            List<string> ids = GetCategoryIds(kv.Value, formOption);
                                            ids.Add(kv.Value);
                                            if (ids.Count > 1)
                                            {
                                                Where += " and (";
                                                foreach (var id in ids)
                                                {
                                                    Where += "','+{0}+',' like '%,{1},%' or ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), id);
                                                }
                                                Where = Where.Substring(0, Where.Length - 3) + ")";
                                            }
                                            else
                                                Where += " and ','+{0}+',' like '%,{1},%' ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), kv.Value);
                                        }
                                        else
                                        {
                                            Where += " and {0} = @{1} ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), i);
                                            parms.Add(kv.Value);
                                            i++;
                                        }
                                    }
                                    break;
                                case FieldType.Bool:
                                    {
                                        Where += " and {0} = @{1} ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), i);
                                        parms.Add(kv.Value);
                                        i++;
                                    }
                                    break;
                                case FieldType.File:
                                case FieldType.Picture:
                                    {
                                        var list = ServiceFactory.UploadService.GetPathListByRealName(kv.Value);
                                        if (list.Count > 0)
                                        {
                                            Where += " and ({0} like '%'+@{1}+'%' or ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), i);
                                            foreach (var name in list)
                                            {
                                                Where += "'*'+{0}+'*' like '%*{1}*%' or ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), name);
                                            }
                                            Where = Where.Substring(0, Where.Length - 3) + ")";
                                        }
                                        else
                                            Where += " and {0} like '%'+@{1}+'%' ".FormatWith(
                                                ServiceFactory.DbService.GetColumnName(kv.Key), i);
                                        parms.Add(kv.Value);

                                        i++;
                                    }
                                    break;
                                default:
                                    Where += " and {0} like '%'+@{1}+'%' ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), i);
                                    parms.Add(kv.Value);
                                    i++;
                                    break;

                            }
                        }
                    }
                    if (formInList.Key.IsBold)
                        queryColumns += ",{0}".FormatWith(ServiceFactory.DbService.GetColumnName(formInList.Value.FieldName + "_bold"));
                    if (formInList.Key.IsColor)
                        queryColumns += ",{0}".FormatWith(ServiceFactory.DbService.GetColumnName(formInList.Value.FieldName + "_color"));
                }
            }
            else
            {
                return;
            }
            #endregion

            //3.1 若子站、专题的参数subjectid>0则应加入过滤
            if (SubjectId > 0)
            {
                Where += " AND subjectid={0}".FormatWith(SubjectId);
            }

            //单独类别权限控制
            int currentUserRolesId = AuthenticateHelper.User == null ? 0 : AuthenticateHelper.User.RolesId;
            if (MainColumn.IsCategoryPower && currentUserRolesId != 1)//开启类别权限
            {
                #region 单独类别权限控制

                string categoryWhere = "";
                Column categoryColumn =
                    ServiceFactory.ColumnService.GetMarkListByColumnId(ColumnId)
                        .SingleOrDefault(p => p.MarkType == "Category");
                if (categoryColumn != null)
                {
                    //获取类别表名
                    string SQLC =
                        "SELECT model.TableName FROM Whir_Dev_Column col INNER JOIN Whir_Dev_Model model ON col.ModelID=model.ModelID WHERE col.ColumnId=@0 and col.IsDel=0";
                    string tableName = DbHelper.CurrentDb.ExecuteScalar<string>(SQLC, categoryColumn.ColumnId);

                    if (!tableName.IsEmpty())
                    {
                        string sql2C =
                            "SELECT {0}_PID FROM {0} WHERE  IsDel=0 AND TypeID=@0 AND SubjectID=@1 ORDER BY Sort DESC,CREATEDATE DESC"
                                .FormatWith(tableName);

                        DataTable table = DbHelper.CurrentDb.Query(sql2C, categoryColumn.ColumnId, SubjectId).Tables[0];

                        if (table.Rows.Count > 0)
                        {
                            string pidName = tableName + "_PID";
                            foreach (DataRow row in table.Rows)
                            {
                                string categoryId = row[0].ToStr();
                                bool isHavePower = Whir.Security.ServiceFactory.RolesService.IsRoleHaveCategoryColumnRes(categoryId, categoryColumn.ColumnId, currentUserRolesId, categoryColumn.SiteId, SubjectId);
                                if (isHavePower)
                                {
                                    categoryWhere += categoryId + ",";
                                }
                            }
                        }
                    }

                    if (categoryWhere.Length > 0)
                    {
                        Where += " AND (CategoryID IN (" + categoryWhere.Trim(',') + ") OR CategoryID='' OR CategoryID IS NULL)";
                    }
                }

                #endregion 单独类别权限控制
            }
			 string user = RequestUtil.Instance.GetQueryString("user");
            if (user == "gq")
            {
              Where += "  and DATEDIFF(DAY,GETDATE(),EndTime)>=0";
            }

            //3.2. 条件语句, 传入的Where条件没有AND开头, 就自动加上AND
            string queryWhere = !Where.Trim().ToUpper().StartsWith("AND") && !Where.IsEmpty()
                ? " AND " + Where
                : Where;

            //4. 排序字段
            string queryOrderBy = GetMultipleSort();
            if (!SortField.IsEmpty())
            {
                queryOrderBy = " {0} {1}".FormatWith(SortField, OrderType.IsEmpty() ? "DESC" : OrderType);
            }
            if (queryOrderBy.IsEmpty())
                queryOrderBy = "Sort DESC, CreateDate DESC";

            try
            {
                //5. 得到最终的查询语句, 并查询出数据
                sql = sql.FormatWith(
                    queryColumns,
                    queryTableName,
                    ColumnId,
                    IsDel ? "1" : "0",
                    queryWhere,
                    queryOrderBy);
                PageDataSet pds = DbHelper.CurrentDb.Query(PageIndex, PageSize, sql, parms.ToArray());
                dt = pds.Items.Tables[0];
                total = pds.TotalItems;
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                Response.Clear();
                Response.Write("获取数据失败！".ToLang());
                Response.End();
            }
        }

        #region 判断字段类型 绑定对应的Html
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (var formInList in DictFormInList)
            {
                switch ((FieldType)formInList.Value.FieldType)
                {
                    case FieldType.Picture:
                        {
                            dt = GetPictureHtml(formInList.Value.FieldName, dt);
                        }
                        break;
                    case FieldType.File:
                    case FieldType.Video:
                        {
                            dt = GetFileHtml(formInList.Value.FieldName, dt);

                        }
                        break;
                    case FieldType.Area:
                        {
                            dt = GetAreaHtml(formInList.Value.FieldName, dt);
                        }
                        break;
                    case FieldType.ListBox:
                        {
                            dt = GetListBoxHtml(formInList.Key.FormId, formInList.Value.FieldName, dt);

                        }
                        break;
                    case FieldType.MultipleHtmlText:
                        {
                            dt = GetMultipleHtmlTextHtml(formInList.Value.FieldName, dt);
                        }
                        break;
                    case FieldType.MultipleText:
                    case FieldType.Text:
                        dt = GetTextHtml(formInList.Key, formInList.Value, formInList.Value.FieldName, dt);
                        break;
                    default:

                        break;

                }
            }
        }
        #endregion 判断字段类型

        string data = dt.ToJson();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
        Response.End();
    }

    /// <summary>
    /// 获取所选的分类以及下级的所有id
    /// </summary>
    /// <returns></returns>
    private List<string> GetCategoryIds(string value, FormOption formOption)
    {
        List<string> ids = new List<string>();

        if (formOption.BindType == 3)//绑定多级类别
        {
            var tableName = formOption.BindTable;
            var valueField = formOption.BindValueField;
            var textField = formOption.BindTextField;
            var key = formOption.BindKeyId.ToStr();
            var subjectID = 0;

            string sql = "SELECT {0},{1},ParentId FROM {2} WHERE IsDel=0 AND TypeID IN({3}) {4} AND ParentId={5} ORDER BY Sort DESC, CreateDate DESC".FormatWith(
                 valueField,
                 textField,
                 tableName,
                 key,
                 subjectID != -99999 ? "AND SubjectID=" + subjectID : "",
                 value
            );
            var list = DbHelper.CurrentDb.Query<string>(sql, value).ToList();
            foreach (var id in list)
            {
                ids.Add(id);
                ids.AddRange(GetCategoryIds(id, formOption));
            }
        }
        else if (formOption.BindType == 4)//绑定单独分类
        {
            string sql = "";
            if (MainColumn.MarkType != "Category")
                sql = "SELECT {0}_PID FROM {0} WHERE  IsDel=0 AND ParentId=@0 ".FormatWith(MainModel.TableName + "_Category");
            else
                sql = "SELECT {0}_PID FROM {0} WHERE  IsDel=0 AND ParentId=@0 ".FormatWith(MainModel.TableName);
            var list = DbHelper.CurrentDb.Query<string>(sql, value).ToList();
            foreach (var id in list)
            {
                ids.Add(id);
                ids.AddRange(GetCategoryIds(id, formOption));
            }
        }
        return ids;
    }

    private Dictionary<string, string> ToDictionary(string str)
    {
        if (str.IsEmpty())
            return new Dictionary<string, string>();
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Deserialize<Dictionary<string, string>>(str);
    }

    #region 列表显示之前对内容做校验

    /// <summary>
    /// 转义单行文本、多行文本，转义html标签，裁剪字符长度
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    private DataTable GetTextHtml(Form form, Field field, string columnName, DataTable dt)
    {

        var style = "<span style='Œisbold Œiscolor '>{0}</span>";

        foreach (DataRow dr in dt.Rows)
        {

            string html = HttpContext.Current.Server.HtmlDecode(dr[columnName].ToStr());
            // 对< > " \ 转义
            html = html.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
            int cutLength = Whir.Config.ConfigHelper.GetSystemConfig().ListTextLength;
            if (html.Length > cutLength)
            {
                html = html.Cut(cutLength, "...");
            }

            if (form.IsBold || form.IsColor)
            {
                if (form.IsBold && dt.Columns.Contains(field.FieldName + "_bold"))
                {
                    var isBold = dr[field.FieldName + "_bold"].ToBoolean();
                    if (isBold)
                    {
                        html = style.FormatWith(html).Replace("Œisbold", "font-weight:bold;");
                    }
                }

                if (form.IsColor && dt.Columns.Contains(field.FieldName + "_color"))
                {
                    var isColor = dr[field.FieldName + "_color"].ToStr();
                    html = style.FormatWith(html).Replace("Œiscolor", "color:#" + isColor);
                }

            }

            dr[columnName] = html.Replace("Œisbold", "").Replace("Œiscolor", "");
        }
        return dt;
    }

    /// <summary>
    /// 转义编辑器内容，去掉html标签，裁剪字符长度
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    private DataTable GetMultipleHtmlTextHtml(string columnName, DataTable dt)
    {
        foreach (DataRow dr in dt.Rows)
        {
            string html = HttpContext.Current.Server.HtmlDecode(dr[columnName].ToStr());
            html = html.RemoveHtml();
            int cutLength = Whir.Config.ConfigHelper.GetSystemConfig().ListTextLength;
            if (html.Length > cutLength)
            {
                html = html.Cut(cutLength, "...");
            }
            dr[columnName] = html;
        }
        return dt;
    }

    /// <summary>
    /// 获取下拉框值，通常用在类别下拉框
    /// </summary>
    /// <param name="formId"> </param>
    /// <param name="columnName"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    private DataTable GetListBoxHtml(int formId, string columnName, DataTable dt)
    {
        DataTable newdt = dt.Clone();  //通常下拉框的字段为int的，所以需要复制一个新dt来改变字段为string类型，例如 panertid=2，需要的数据 根类别>类别2
        newdt.Columns[columnName].DataType = Type.GetType("System.String");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            newdt.Rows.Add(dt.Rows[i].ItemArray);
            newdt.Rows[i][columnName] = ServiceFactory.DynamicFormService.GetOptionText(formId, dt.Rows[i][columnName].ToStr(), SubjectId);
        }
        return newdt;

    }

    /// <summary>
    /// 获取地区的Html
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    private DataTable GetAreaHtml(string columnName, DataTable dt)
    {
        foreach (DataRow dr in dt.Rows)
        {
            dr[columnName] = Whir.Service.ServiceFactory.AreaService.GetParentsName(dr[columnName].ToInt());
        }
        return dt;
    }

    /// <summary>
    /// 获取文件的Html(非图片的文件)
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    private DataTable GetFileHtml(string columnName, DataTable dt)
    {
        foreach (DataRow dr in dt.Rows)
        {
            List<string> list = dr[columnName].ToStr().Trim('*').Split('*').ToList();
            if (list.Count > 0)
                dr[columnName] = "";
            foreach (string file in list)
            {
                if (file != "")
                {
                    string fileName = Whir.Service.ServiceFactory.UploadService.GetFileName(file);
                    dr[columnName] += "<a  href='{1}'>{0}</a><br/>".FormatWith(fileName, UploadFilePath + file);
                }
            }

        }
        return dt;
    }

    /// <summary>
    /// 获取图片、多图片的Html
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    private DataTable GetPictureHtml(string columnName, DataTable dt)
    {
        string temp;
        foreach (DataRow dr in dt.Rows)
        {
            List<string> list = dr[columnName].ToStr().Trim('*').Split('*').ToList();
            if (list.Count > 0)
                dr[columnName] = "";
            foreach (string picture in list)
            {
                if (picture != "")
                {
                    temp = picture;
                    if (picture.Contains("?"))
                        temp = picture.Split('?')[0];
                    string fileName = Whir.Service.ServiceFactory.UploadService.GetFileName(temp);
                    dr[columnName] += "<a  href=\"javascript:view('{1}');\">{0}</a><br/>".FormatWith(fileName, UploadFilePath + picture);
                }
            }

        }
        return dt;
    }
    #endregion

    public void GetSearchSelectOption()
    {
        int columnId = RequestUtil.Instance.GetString("ColumnId").ToInt();
        int formid = RequestUtil.Instance.GetString("FormId").ToInt();
        int fieldid = RequestUtil.Instance.GetString("Fieldid").ToInt();
        FormOption formOption = ServiceFactory.FormOptionService.GetFormOptionByFormID(formid);
        if (formOption == null) return;
        Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(fieldid);
        Form form = ServiceFactory.FormService.SingleOrDefault<Form>(formid);
        IList<ListItem> options = ServiceFactory.DynamicFormService.GetOptionsInSubject(field, form, formOption,
            RequestUtil.Instance.GetQueryInt("SubjectId", 0));

        Model model = ServiceFactory.ModelService.GetModelByColumnId(columnId);
        if (model != null)
        {
            //判断是类别栏目, 在父类别中不显示本分类和本分类的下级分类
            if (model.ModuleMark.Contains("Category_") && field.FieldName.ToLower() == "parentid")
            {
                string primaryKey = model.TableName + "_PID";
                options = ServiceFactory.DynamicFormService.GetOptionsInSubject(field, form, formOption, 0,
                    RequestUtil.Instance.GetQueryInt("SubjectId", 0));
            }
        }
        Response.Clear();
        Response.Write(options.ToJson());
        //Response.End();
    }

    //获取历史记录
    public void GetHistoryBakList()
    {
        GetValueForProperties();
        int itemid = RequestUtil.Instance.GetQueryInt("itemid", 0);
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("历史记录", ColumnId, SubjectId == 0 ? -1 : SubjectId), true);

        string SQL = "SELECT {0} FROM {1}_Bak WHERE {1}_PID={2} ORDER BY {3}";

        //要查询的列
        string queryPrimaryKey = MainModel.TableName + "_Bak_PID";
        string queryColumns = queryPrimaryKey + "," + queryPrimaryKey + " AS Id,";
        queryColumns += MainModel.TableName + "_PID";

        foreach (var formInList in DictFormInList)
        {
            Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(formInList.Value.FieldId);
            if (field == null) continue;

            queryColumns += ",{0}".FormatWith(
                    ServiceFactory.DbService.GetColumnName(field.FieldName),
                    formInList.Key.FieldAlias
                );
        }
        queryColumns += ",UpdateDate";
        string queryOrderBy = GetMultipleSort();
        if (!SortField.IsEmpty())
        {
            queryOrderBy = " {0} {1}".FormatWith(SortField, OrderType.IsEmpty() ? "DESC" : OrderType);
        }
        if (!queryOrderBy.IsEmpty())
        {
        }
        else
        {
            queryOrderBy = "UpdateDate DESC";
        }
        SQL = SQL.FormatWith(queryColumns, MainModel.TableName, itemid, queryOrderBy);

        DataTable dt = DbHelper.CurrentDb.Query(SQL, null).Tables[0];

        foreach (var formInList in DictFormInList)
        {
            switch ((FieldType)formInList.Value.FieldType)
            {
                case FieldType.Picture:
                    {
                        dt = GetPictureHtml(formInList.Value.FieldName, dt);
                    }
                    break;
                case FieldType.File:
                case FieldType.Video:
                    {
                        dt = GetFileHtml(formInList.Value.FieldName, dt);

                    }
                    break;
                case FieldType.Area:
                    {
                        dt = GetAreaHtml(formInList.Value.FieldName, dt);
                    }
                    break;
                case FieldType.ListBox:
                    {
                        dt = GetListBoxHtml(formInList.Key.FormId, formInList.Value.FieldName, dt);

                    }
                    break;
                case FieldType.MultipleHtmlText:
                    {
                        dt = GetMultipleHtmlTextHtml(formInList.Value.FieldName, dt);

                    }
                    break;
                case FieldType.MultipleText:
                case FieldType.Text:
                    dt = GetTextHtml(formInList.Key, formInList.Value, formInList.Value.FieldName, dt);
                    break;
                //case FieldType.Bool:
                //case FieldType.Text:
                default:

                    break;

            }
        }

        long total = dt.Rows.Count;
        string data = dt.ToJson();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
        Response.End();
    }

    //历史记录删除
    public HandlerResult DeleteHistoryRow()
    {
        //删除
        ColumnId = RequestUtil.Instance.GetString("ColumnID").ToInt(0);
        int itemid = RequestUtil.Instance.GetString("itemid").ToInt(0);
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("历史记录", ColumnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int result = ServiceFactory.HistoryBakHelper.DeleteBak(ColumnId, itemid);
        if (result > 0)
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        else
            return new HandlerResult { Status = false, Message = "操作失败".ToLang() };

    }

    //历史记录选择
    public HandlerResult DeleteHistoryAll()
    {
        //删除
        ColumnId = RequestUtil.Instance.GetString("ColumnID").ToInt(0);
        string selected = RequestUtil.Instance.GetString("selected");
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("历史记录", ColumnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int result = ServiceFactory.HistoryBakHelper.DeleteBak(ColumnId, selected);
        if (result > 0)
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        else
            return new HandlerResult { Status = false, Message = "操作失败".ToLang() };


    }

    //历史记录还原
    public HandlerResult DeleteHistoryRestore()
    {
        //删除
        ColumnId = RequestUtil.Instance.GetString("ColumnID").ToInt(0);
        SubjectId = RequestUtil.Instance.GetString("SubjectId").ToInt(0);
        int itemid = RequestUtil.Instance.GetString("itemid").ToInt(0);
        int backid = RequestUtil.Instance.GetString("backid").ToInt(0);
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("历史记录", ColumnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string sql = "";//通过工作流设置是否可以编辑，同时返回要处理的sql语句
        MainModel = ServiceFactory.ModelService.GetModelByColumnId(ColumnId);
        if (MainModel != null)
        {
            //能过检查工作流设置看是否可以修改
            bool r = ServiceFactory.WorkFlowService.IsCanEdit(ColumnId, SubjectId, MainModel.TableName, MainModel.TableName + "_PID", itemid, CurrentUser.RolesId, CurrentUser.LoginName, out sql);
            if (!r)
            {
                return new HandlerResult { Status = false, Message = "操作失败，当前管理员没有此操作权限".ToLang() };
            }
            else
            {
                //恢复
                int result = ServiceFactory.HistoryBakHelper.RestoreFromBak(ColumnId, itemid, backid);
                if (result >= 2)
                {
                    //清除静态文件
                    ServiceFactory.ColumnService.CleanupStaticFiles(ColumnId);
                    return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
                }
                else
                    return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
            }
        }
        else
        {
            return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
        }

    }

    //导出简历信息
    public void InportJobRequest()
    {
        //删除
        ColumnId = RequestUtil.Instance.GetString("ColumnId").ToInt(0);
        int itemid = RequestUtil.Instance.GetString("itemid").ToInt(0);

        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
        Model model = ServiceFactory.ModelService.SingleOrDefault<Model>(column.ModelId);
        if (model == null) return;

        string tableName = model.TableName;
        string primaryKeyName = ContentHelper.GetCustomTablePrimaryKeyName(tableName);

        try
        {
            IList<Field> listField = ServiceFactory.FieldService.GetListByColumnId(ColumnId);
            //拼装查询的SQL语句
            string str = string.Empty;
            Dictionary<string, string> keyValue = new Dictionary<string, string>();//是指键值字典
            foreach (Field field in listField)
            {
                List<string> columnStr = new List<string>() { "IsDel", "Sort", "State", "MetaTitle", "MetaKeyword", "MetaDesc", "TypeID", "SubjectId", "CreateDate", "CreateUser", "UpdateUser", "UpdateDate" };
                if (!columnStr.Contains(field.FieldName))
                {
                    str += "[" + field.FieldName + "],";
                    keyValue.Add(field.FieldId.ToStr(), field.FieldName);
                }
            }
            str = str.TrimEnd(',');

            string sql = "SELECT {0} FROM {1} WHERE {2} = {3}".FormatWith(str, tableName, primaryKeyName, itemid);
            DataTable dt = DbHelper.CurrentDb.Query(sql, null).Tables[0];

            string folderName = DateTime.Now.ToString("yyyyMMddHHmmssfff");//导出随机的文件名
            string exportFileNames = string.Empty;
            string template = "<tr><td><b>{0}</b></td><td width='70%'>{1}</td></tr>";
            foreach (DataRow row in dt.Rows)
            {
                string strInner = string.Empty;//显示表格
                foreach (KeyValuePair<string, string> kvp in keyValue)
                {
                    Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(kvp.Key.ToInt());
                    Form form = ServiceFactory.FormService.FirstOrDefault("where fieldid=@0 and ColumnId=@1", field.FieldId, column.ColumnId);

                    switch ((FieldType)field.FieldType)
                    {
                        case FieldType.ListBox:
                            string formvalue = ServiceFactory.DynamicFormService.GetOptionText(form.FormId, row[kvp.Value].ToStr());
                            strInner += template.FormatWith(field.FieldAlias, formvalue);
                            break;
                        case FieldType.DateTime:
                            strInner += template.FormatWith(field.FieldAlias, row[kvp.Value].ToDateTime());
                            break;
                        case FieldType.Bool:
                            strInner += template.FormatWith(field.FieldAlias, row[kvp.Value].ToBoolean() ? "是" : "否");
                            break;
                        case FieldType.Area:
                            string areavalue = ServiceFactory.AreaService.GetParentsName(row[kvp.Value].ToInt());
                            strInner += template.FormatWith(field.FieldAlias, areavalue);
                            break;
                        default:
                            strInner += template.FormatWith(field.FieldAlias, row[kvp.Value]);
                            break;
                    }
                }
                WordHelper.Instance.CreateJobRequestWord(strInner, folderName + ".doc");
                return;
            }
        }
        catch (Exception ex)
        {
            return;
        }
        return;

    }

    //批量导出
    public void InportJobRequestAll()
    {
        ColumnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
        string selected = RequestUtil.Instance.GetFormString("Selected");
        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
        Model model = ServiceFactory.ModelService.SingleOrDefault<Model>(column.ModelId);
        if (model == null) return;
        string tableName = model.TableName;
        string primaryKeyName = ContentHelper.GetCustomTablePrimaryKeyName(tableName);

        try
        {
            IList<Field> listField = ServiceFactory.FieldService.GetListByColumnId(ColumnId);
            //拼装查询的SQL语句
            string str = string.Empty;
            Dictionary<string, string> keyValue = new Dictionary<string, string>();//是指键值字典
            foreach (Field field in listField)
            {
                List<string> ColumnStr = new List<string>() { "IsDel", "Sort", "State", "MetaTitle", "MetaKeyword", "MetaDesc", "TypeID", "SubjectId", "CreateDate", "CreateUser", "UpdateUser", "UpdateDate" };
                if (!ColumnStr.Contains(field.FieldName))
                {
                    str += field.FieldName + ",";
                    keyValue.Add(field.FieldId.ToStr(), field.FieldName);
                }
            }
            str = str.TrimEnd(',');

            string sql = "SELECT {0} FROM {1} WHERE {2} IN ({3})".FormatWith(str, tableName, primaryKeyName, selected);
            DataTable dt = DbHelper.CurrentDb.Query(sql, null).Tables[0];

            string folderName = DateTime.Now.ToString("yyyyMMddHHmmssfff");//导出随机的文件名
            string exportFileNames = string.Empty;
            string template = "<tr><td><b>{0}</b></td><td width='70%'>{1}</td></tr>";
            foreach (DataRow row in dt.Rows)
            {
                string strInner = string.Empty;//显示表格
                foreach (KeyValuePair<string, string> kvp in keyValue)
                {
                    Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(kvp.Key.ToInt());
                    Form form = ServiceFactory.FormService.FirstOrDefault("where fieldid=@0 and ColumnId=@1", field.FieldId, column.ColumnId);

                    switch ((FieldType)field.FieldType)
                    {
                        case FieldType.ListBox:
                            string formvalue = ServiceFactory.DynamicFormService.GetOptionText(form.FormId, row[kvp.Value].ToStr());
                            strInner += template.FormatWith(field.FieldAlias, formvalue);
                            break;
                        case FieldType.DateTime:
                            strInner += template.FormatWith(field.FieldAlias, row[kvp.Value].ToDateTime());
                            break;
                        case FieldType.Bool:
                            strInner += template.FormatWith(field.FieldAlias, row[kvp.Value].ToBoolean() ? "是" : "否");
                            break;
                        case FieldType.Area:
                            string areavalue = ServiceFactory.AreaService.GetParentsName(row[kvp.Value].ToInt());
                            strInner += template.FormatWith(field.FieldAlias, areavalue);
                            break;
                        default:
                            strInner += template.FormatWith(field.FieldAlias, row[kvp.Value]);
                            break;
                    }
                }
                exportFileNames += row["Name"].ToStr() + "*";//记录导出的名称
                this.CreateWord(WordHelper.Instance.GetWordContent(strInner), folderName, row["Name"].ToStr());
            }
            if (exportFileNames == string.Empty) return;
            exportFileNames = exportFileNames.TrimEnd('*');
            SharpZipHelper szh = new SharpZipHelper();
            string rarName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".zip";
            szh.CompressDirectory(Server.MapPath(UploadFilePath + "jobrequest/" + folderName + "/"), Server.MapPath(UploadFilePath + "jobrequest/" + rarName), 5, 4096);
            FileSystemHelper.DeleteFolder(Server.MapPath(UploadFilePath + "jobrequest/" + folderName + "/"));

            Response.Redirect(UploadFilePath + "jobrequest/" + rarName);
        }
        catch (Exception ex)
        {
            return;
        }
    }

    /// <summary>
    /// 人才招聘,应聘信息简历导出
    /// </summary>
    /// <param name="inner">简历内容</param>
    /// <param name="folderName">文件夹名</param>
    /// <param name="fileNameId">文件名</param>
    private void CreateWord(string inner, string folderName, string fileName)
    {
        string dir = Server.MapPath(UploadFilePath + "jobrequest/");//首先在类库添加using System.web的引用
        string name = fileName + ".doc"; //DateTime.Now.ToLongDateString() + ".doc";
        string strFileName = dir + folderName + "\\" + name; //文件保存路径
        FileInfo info = new FileInfo(strFileName);
        if (!Directory.Exists(info.DirectoryName))
        {
            Directory.CreateDirectory(info.DirectoryName);
        }
        FileSystemHelper.WriteFile(strFileName, inner);
    }

    //排序
    public HandlerResult Sort()
    {
        var apidSort = RequestUtil.Instance.GetString("SortIds");
        int columnId = RequestUtil.Instance.GetString("ColumnId").ToInt();
        var handlerResult = SysManagePageBase.JudgeActionPermission(ColumnId == 1 ? SysManagePageBase.IsCurrentRoleMenuRes("302") : SysManagePageBase.IsRoleHaveColumnRes("排序", ColumnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        if (!apidSort.IsEmpty())
        {
            var idSorts = apidSort.Split(','); //主键ID与Sort主键对
            foreach (var s in idSorts)
            {
                //ID与Sort
                var idSort = s.Split('|');
                //更新排序
                long sort = 0;
                if (idSort.Length < 2 || !long.TryParse(idSort[1], out sort))
                {
                    continue;
                }

                Model model = ServiceFactory.ModelService.GetModelByColumnId(columnId);
                if (model == null)
                    return new HandlerResult { Status = false, Message = "排序失败".ToLang() };

                string updateSql = "UPDATE {0} SET Sort={1} WHERE {0}_PID={2}".FormatWith(
                   model.TableName, sort, idSort[0].ToInt());
                DbHelper.CurrentDb.Execute(updateSql);

            }
        }
        return new HandlerResult { Status = true, Message = "排序成功".ToLang() };
    }

    //排序
    public HandlerResult OpenSort()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(ColumnId == 1 ? SysManagePageBase.IsCurrentRoleMenuRes("302") : SysManagePageBase.IsRoleHaveColumnRes("排序", ColumnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var selected = RequestUtil.Instance.GetFormString("Selected");
        var selectedOpen = RequestUtil.Instance.GetFormString("SelectedOpen");
        int result = ServiceFactory.GridViewService.SortInfo(ColumnId, selected, selectedOpen.ToInt());
        if (result == 0)
            return new HandlerResult { Status = false, Message = "排序失败".ToLang() };
        else
            return new HandlerResult { Status = true, Message = "排序成功".ToLang() };
    }

    //排序 输入数字
    public HandlerResult OpenSortByNum()
    {
        var selected = RequestUtil.Instance.GetFormString("Selected");
        var selectedOpen = RequestUtil.Instance.GetFormString("SelectedOpen");
        int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt();
        int Total = RequestUtil.Instance.GetFormString("Total").ToInt();
        var handlerResult = SysManagePageBase.JudgeActionPermission(columnId == 1 ? SysManagePageBase.IsCurrentRoleMenuRes("302") : SysManagePageBase.IsRoleHaveColumnRes("排序", columnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int result = ServiceFactory.GridViewService.SortInfo(columnId,SubjectId, selected, selectedOpen.ToLong(), Total);
        if (result == 0)
            return new HandlerResult { Status = false, Message = "排序失败".ToLang() };
        else
            return new HandlerResult { Status = true, Message = "排序成功".ToLang() };
    }

    //删除
    public HandlerResult DeleteRow()
    {
        //删除
        string sql = "";
        ColumnId = RequestUtil.Instance.GetString("ColumnID").ToInt(0);
        SubjectId = RequestUtil.Instance.GetString("SubjectID").ToInt(0);
        IsDel = RequestUtil.Instance.GetString("IsDel").ToBoolean();

        var handlerResult = SysManagePageBase.JudgeActionPermission(ColumnId == 1 ? SysManagePageBase.IsCurrentRoleMenuRes("301") : SysManagePageBase.IsRoleHaveColumnRes("删除", ColumnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }

        int primaryValue = RequestUtil.Instance.GetString("id").ToInt(0);
        //清除静态文件
        ServiceFactory.ColumnService.CleanupStaticFiles(ColumnId);

        MainColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);

        //赋值主模型实体
        if (MainColumn != null)
            MainModel = ServiceFactory.ModelService.SingleOrDefault<Model>(MainColumn.ModelId);

        if (IsDel || MainColumn.MarkType == "Category")//回收站中彻底删除
        {
            sql = "DELETE FROM {0} WHERE {0}_PID=@0".FormatWith(MainModel.TableName);
            int result = DbHelper.CurrentDb.Execute(sql, primaryValue);

            if (result > 0)
            {
                //操作日志
                ServiceFactory.OperateLogService.Save("删除，从栏目【{1}】，表【{0}】中移除数据".FormatWith(MainModel.TableName, MainColumn.ColumnName));
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
            else
                return new HandlerResult { Status = false, Message = "数据库无影响记录".ToLang() };
        }
        else
        {
            //改变删除状态, 扔到回收站
            sql = "UPDATE {0} SET IsDel=1 WHERE {0}_PID=@0".FormatWith(MainModel.TableName);
            if (ServiceFactory.WorkFlowService.IsCanDelete(ColumnId, SubjectId, primaryValue, CurrentUser.RolesId, CurrentUser.LoginName))
            {
                int result = DbHelper.CurrentDb.Execute(sql, primaryValue);

                if (result > 0)
                {
                    //操作日志
                    ServiceFactory.OperateLogService.Save("删除，从栏目【{1}】，表【{0}】中移除数据".FormatWith(MainModel.TableName, MainColumn.ColumnName));
                    return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
                }
                else
                    return new HandlerResult { Status = false, Message = "数据库无影响记录".ToLang() };
            }
            else
            {
                return new HandlerResult { Status = false, Message = "操作失败，当前管理员没有此操作权限".ToLang() };
            }
        }

    }

    //批量通过审核
    public HandlerResult PassFlow()
    {
        string selected = RequestUtil.Instance.GetString("selected");
        ColumnId = RequestUtil.Instance.GetString("ColumnID").ToInt(0);
        int currentActivityId = RequestUtil.Instance.GetString("CurrentActivityId").ToInt(0);

        if (currentActivityId != 0)
        {
            //审核到下一个节点
            if (ServiceFactory.GridViewService.PassWorkFlow(ColumnId, currentActivityId, SysManagePageBase.GetIntArrayByStringSplit(selected)))
            {
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
            else
            {
                return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
            }
        }
        return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
    }

    //批量通过退审
    public HandlerResult ReturnFlow()
    {
        string selected = RequestUtil.Instance.GetString("selected");
        ColumnId = RequestUtil.Instance.GetString("ColumnID").ToInt(0);
        int currentActivityId = RequestUtil.Instance.GetString("CurrentActivityId").ToInt(0);
        string Reasion = RequestUtil.Instance.GetString("Reasion");
        if (currentActivityId != 0)
        {
            //退审
            if (ServiceFactory.GridViewService.ReturnWorkFlow(ColumnId, currentActivityId, SysManagePageBase.GetIntArrayByStringSplit(selected), Reasion))
            {
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
            else
            {
                return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
            }
        }
        return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
    }

    //批量删除
    public HandlerResult DeleteAll()
    {
        string selected = RequestUtil.Instance.GetString("selected");
        ColumnId = RequestUtil.Instance.GetString("ColumnID").ToInt(0);
        SubjectId = RequestUtil.Instance.GetString("SubjectID").ToInt(0);
        IsDel = RequestUtil.Instance.GetString("IsDel").ToBoolean();

        var handlerResult = SysManagePageBase.JudgeActionPermission(ColumnId == 1 ? SysManagePageBase.IsCurrentRoleMenuRes("301") : SysManagePageBase.IsRoleHaveColumnRes("删除", ColumnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }

        MainColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);

        //赋值主模型实体
        if (MainColumn != null)
            MainModel = ServiceFactory.ModelService.SingleOrDefault<Model>(MainColumn.ModelId);
        if (MainColumn.MarkType == "Category")
            IsDel = true;  //分类管理页面的数据直接删除，不存放回收站

        ServiceFactory.GridViewService.DeleteInfo(ColumnId, 0, IsDel, selected, CurrentUser.RolesId, CurrentUser.LoginName);
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    //批量推送
    public HandlerResult Copy()
    {
        string selected = RequestUtil.Instance.GetString("selected");
        ColumnId = RequestUtil.Instance.GetString("ColumnID").ToInt(0);
        var selectedOpen = RequestUtil.Instance.GetString("SelectedOpen");
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("批量推送", ColumnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        ServiceFactory.GridViewService.CopyInfo(ColumnId,
                       SysManagePageBase.GetIntArrayByStringSplit(selected),
                       SysManagePageBase.GetStringArrayByStringSplit(selectedOpen)
                   );
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    //批量转移
    public HandlerResult Cut()
    {
        string selected = RequestUtil.Instance.GetString("selected");
        ColumnId = RequestUtil.Instance.GetString("ColumnID").ToInt(0);
        var selectedOpen = RequestUtil.Instance.GetString("SelectedOpen");
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("批量转移", ColumnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        ServiceFactory.GridViewService.CutInfo(ColumnId,
                        SysManagePageBase.GetIntArrayByStringSplit(selected),
                        SysManagePageBase.GetStringArrayByStringSplit(selectedOpen)[0]
                    );
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    public HandlerResult Preview()
    {
        int columnId = RequestUtil.Instance.GetFormString("columnid").ToInt(0);
        bool hasWord = false;

        Dictionary<string, object> dict = GetFieldValue(columnId, "", out hasWord, 0);
        Dictionary<string, object> dictAll = new Dictionary<string, object>();
        if (hasWord)
        {
            return new HandlerResult { Status = false, Message = "操作失败，内容中包含敏感词".ToLang() };
        }
        foreach (var item in dict)
        {
            dictAll.Add(item.Key.Trim('[').Trim(']').Trim('`'), item.Value);
        }

        CacheUtil.Instance.Remove(Whir.Label.Dynamic.PreviewField.CacheName);
        CacheUtil.Instance.SetCache(Whir.Label.Dynamic.PreviewField.CacheName, dictAll);
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    //保存
    public HandlerResult Save()
    {
        int columnId = RequestUtil.Instance.GetString("TypeId").ToInt(0);
        int itemId = RequestUtil.Instance.GetString("ItemId").ToInt(0);
        HandlerResult handlerResult;
        if (itemId == 0)
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(columnId == 1 ? SysManagePageBase.IsCurrentRoleMenuRes("295") :
                SysManagePageBase.IsRoleHaveColumnRes("添加", columnId, SubjectId == 0 ? -1 : SubjectId)
                ||
                SysManagePageBase.IsRoleHaveColumnRes("修改", columnId, SubjectId == 0 ? -1 : SubjectId)
                );
        }
        else
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(columnId == 1 ? SysManagePageBase.IsCurrentRoleMenuRes("300") : SysManagePageBase.IsRoleHaveColumnRes("修改", columnId, SubjectId == 0 ? -1 : SubjectId));
        }
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }

        Model model = ServiceFactory.ModelService.GetModelByColumnId(columnId);
        if (model == null)
        {
            return new HandlerResult { Status = false, Message = "操作失败，无法获取到栏目模型实体".ToLang() };
        }

        //赋值主栏目实体
        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId);

        string tableName = model.TableName;
        string primaryKeyName = model.TableName + "_PID";

        bool hasWord = false;

        Dictionary<string, object> dict = GetFieldValue(columnId, "", out hasWord, itemId);

        if (hasWord)
        {
            return new HandlerResult { Status = false, Message = "操作失败，内容中包含敏感词".ToLang() };
        }
        if (column.IsRelated)
        {
            ServiceFactory.RelationService.Remove(itemId, columnId);
            var relationData = RequestUtil.Instance.GetFormString("hidRelationData");

            foreach (string relation in relationData.Split(','))
            {
                string[] data = relation.Split('|');
                if (data.Length < 5) continue;

                int mainColumnId = columnId;
                int attachPrimaryId = data[0].ToInt();
                int attachColumnId = data[1].ToInt();
                string linkText = Server.UrlDecode(data[2]);
                string linkUrl = Server.UrlDecode(data[3]);

                ServiceFactory.RelationService.Insert(itemId, mainColumnId, attachPrimaryId, attachColumnId,
                                                      linkText, linkUrl);
            }
        }

        if (itemId == 0)
        {
            #region 添加

            string sql = ServiceFactory.DynamicFormService.GetInsertSql(columnId, SubjectId, dict);
            object[] parms = dict.Select(p => p.Value).ToArray();


            int primaryValue = DbHelper.CurrentDb.Insert(primaryKeyName, sql, parms).ToInt();
            if (primaryValue > 0)
            {
                if (column.MarkType == "Category")
                {
                    int subjectTypeId = ServiceFactory.SubjectService.GetSubjectClassId(SubjectId);
                    //单独类别权限资源  category|siteId站点id{0}|栏目类型{1：0、1、2}|子站{2}|栏目{3}|类别id{4}
                    string txt = "category|siteId{0}|type{1}|{2}|{3}|{4}".FormatWith(column.SiteId, subjectTypeId, SubjectId > 0 ? 1 : 0, columnId, primaryValue);

                    Whir.Security.ServiceFactory.RolesService.AddRoleJurisdiction(5, 2, txt); //默认给admin加上
                    if (AuthenticateHelper.User.RolesId != 1 && AuthenticateHelper.User.RolesId != 2)
                        Whir.Security.ServiceFactory.RolesService.AddRoleJurisdiction(5, AuthenticateHelper.User.RolesId, txt); //给当前角色加上

                    //整理ParentPath
                    ServiceFactory.DynamicFormService.ConllationParentPath(columnId);
                }

                //添加关联信息
                AddRelation(columnId, primaryValue);

                //表是否存在类别
                bool isExistCategoryColumn = false;
                string hidColumnId = RequestUtil.Instance.GetFormString("hidColumnID");
                if (hidColumnId.Length > 0)
                {
                    isExistCategoryColumn = ServiceFactory.DbService.IsExsitColumn(tableName, "categoryid");
                }
                //添加到其它栏目
                foreach (string ids in hidColumnId.Split(','))
                {
                    if (ids.Split('|').Length != 2) continue;
                    int typeId = ids.Split('|')[0].ToInt();
                    int subid = ids.Split('|')[1].ToInt();
                    if (typeId == 0) continue;

                    string otherSql = ServiceFactory.DynamicFormService.GetInsertSql(typeId, subid, dict);
                    object[] parms2 = dict.Select(p => p.Value).ToArray();
                    int primaryId = DbHelper.CurrentDb.Insert(primaryKeyName, otherSql, parms2).ToInt();

                    if (typeId == columnId) //发布到同一个栏目，需更改排序号，清空类别
                    {
                        DbHelper.CurrentDb.Execute(
                            "UPDATE {0} SET Sort=@0 WHERE {0}_PID=@1".FormatWith(tableName)
                            ,
                            Convert.ToInt64(DateTime.Now.AddSeconds(2.00)
                                .ToString("yyMMddHHmmssff", DateTimeFormatInfo.InvariantInfo))
                            , primaryId
                        );
                    }
                    else
                    {
                        if (isExistCategoryColumn) //表存在类别
                        {
                            DbHelper.CurrentDb.Execute(
                                "UPDATE {0} SET CategoryID=@0 WHERE {0}_PID=@1".FormatWith(tableName)
                                , null
                                , primaryId
                            );
                        }
                        //清除静态文件
                        ServiceFactory.ColumnService.CleanupStaticFiles(typeId);
                    }
                }

                //操作日志
                ServiceFactory.OperateLogService.SaveLog("insert", primaryValue, model, column);

                //清除静态文件
                ServiceFactory.ColumnService.CleanupStaticFiles(columnId);

                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

            }
            else
                return new HandlerResult { Status = false, Message = "操作失败".ToLang() };

            #endregion 添加
        }
        else
        {
            #region 编辑

            string sql = ""; //通过工作流设置是否可以编辑，同时返回要处理的sql语句
                             //能过检查工作流设置看是否可以修改
            bool result = ServiceFactory.WorkFlowService.IsCanEdit(columnId, SubjectId, tableName, primaryKeyName,
                                                                   itemId, CurrentUser.RolesId, CurrentUser.LoginName,
                                                                   out sql);
            if (!result)
            {
                return new HandlerResult { Status = false, Message = "操作失败，当前管理员没有此操作权限".ToLang() };
            }

            //备份原数据
            ServiceFactory.HistoryBakHelper.BakInfo(columnId, itemId);

            #region 类别层级数判断
            if (column.MarkType == "Category")
            {
                var masterColumn = column;
                if (column.MarkParentId > 0)
                    masterColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(column.MarkParentId);

                if (masterColumn.IsCategory)
                {
                    int maxDepth = masterColumn.CategoryLevel;
                    if (maxDepth == 1)
                        DbHelper.CurrentDb.Execute("Update  {0} Set ParentId=@0 WHERE TypeId=@1".FormatWith(tableName), 0, column.ColumnId);
                    else
                    {
                        int level = GetLevel(tableName, dict["[ParentID]"].ToInt(), column.ColumnId, itemId);

                        if (maxDepth > 0 && level > maxDepth)
                        {
                            return new HandlerResult { Status = false, Message = "操作失败(可能原因：类别深度不得超过{0}个等级)".FormatWith(maxDepth) };
                        }
                    }
                }
            }
            #endregion

            string SQL = ServiceFactory.DynamicFormService.GetUpdateSql(columnId, itemId, dict);
            object[] parms = dict.Select(p => p.Value).ToArray();

            int resultValue = DbHelper.CurrentDb.Execute(SQL, parms);
            if (resultValue > 0)
            {

                //添加关联信息
                ServiceFactory.RelationService.Remove(itemId, columnId);
                AddRelation(columnId, itemId);

                //清除静态文件
                ServiceFactory.ColumnService.CleanupStaticFiles(columnId);

                //通过工作流设置是否可以编辑，同时返回要处理的sql语句,一般是设置审核通过后的修改状态改为初始
                if (sql.Length > 0)
                    DbHelper.CurrentDb.Execute(sql);

                //string commandName = RequestUtil.Instance.GetFormString("commandname");
                //if (commandName == "SaveToFlow")
                //{
                //    //退审的转入正常流程待审核
                //    ServiceFactory.WorkFlowService.SetStateToFirstFlow(tableName, primaryKeyName, itemId);
                //}
                if (column.MarkType == "Category")
                {
                    ServiceFactory.DynamicFormService.ConllationParentPath(column.ColumnId);
                }
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
            else
                return new HandlerResult { Status = false, Message = "操作失败".ToLang() };


            #endregion 编辑
        }
    }

    //获取选项的级数
    public int GetLevel(string tableName, int parentId, int columnId, int itemId)
    {
        int level = 0;
        string parentPath = "";
        string sql = @"select top 1 ParentPath from {0}  
                                             where ParentPath like '%'+( 
                                             SELECT Top 1 ParentPath FROM {0}  WHERE ParentId=@0 and typeid=@1)
                                             +'%'  and typeid=@1
                                             order by ParentPath desc";
        sql = sql.FormatWith(tableName);
        parentPath = DbHelper.CurrentDb.SingleOrDefault<string>(sql, itemId, columnId) ?? "";

        if (parentPath == "")
            level = 1;
        else
            level = parentPath.Split(',').Count();

        if (parentPath.Split(',').Contains(parentId.ToStr()))
            return level;
        sql = "SELECT  Top 1 ParentPath FROM {0} WHERE  {0}_PID=@0 order by ParentPath desc";
        sql = sql.FormatWith(tableName);
        parentPath = DbHelper.CurrentDb.SingleOrDefault<string>(sql, parentId) ?? "";
        if (parentPath == "")
            return level;
        level += parentPath.Split(',').Count();
        return level;
    }

    //批量更新
    public HandlerResult UpdateList()
    {
        int columnId = RequestUtil.Instance.GetFormString("columnid").ToInt(0);
        string itemIds = RequestUtil.Instance.GetFormString("itemIds");
        string exceptFields = RequestUtil.Instance.GetFormString("exceptFields");
        var handlerResult = SysManagePageBase.JudgeActionPermission(columnId == 1 ? SysManagePageBase.IsCurrentRoleMenuRes("300") : SysManagePageBase.IsRoleHaveColumnRes("修改", columnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        Model model = ServiceFactory.ModelService.GetModelByColumnId(columnId);
        if (model == null)
        {
            return new HandlerResult { Status = false, Message = "操作失败，无法获取到栏目模型实体".ToLang() };
        }

        //赋值主栏目实体
        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId);

        string tableName = model.TableName;
        string primaryKeyName = model.TableName + "_PID";

        bool hasWord = false;

        Dictionary<string, object> dict = GetFieldValue(columnId, exceptFields, out hasWord, 0);

        if (hasWord)
        {
            return new HandlerResult { Status = false, Message = "操作失败，内容中包含敏感词".ToLang() };
        }


        #region 编辑
        if (dict.Count > 0)
        {
            string sql = ServiceFactory.DynamicFormService.GetUpdateSql(columnId, dict);

            object[] parms = dict.Select(p => p.Value).ToArray();

            var primaryIds = string.Join(",", itemIds.Split(',').Select(p => p.ToInt()).ToArray());  //先分割，再转换int，再拼ids，防止中间有非数字类型
            if (primaryIds.Length > 0)
            {
                sql += string.Format(" AND {0} IN ({1})", primaryKeyName, primaryIds);
            }
            else
            {
                return new HandlerResult { Status = false, Message = "未能获取到要编辑的主键参数".ToLang() };
            }
            foreach (string itemId in primaryIds.Split(','))
            {
                ServiceFactory.HistoryBakHelper.BakInfo(columnId, itemId.ToInt());
            }

            int resultValue = DbHelper.CurrentDb.Execute(sql, parms);
            if (resultValue > 0)
            {
                //清除静态文件
                ServiceFactory.ColumnService.CleanupStaticFiles(columnId);
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
            else
                return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
        }
        else
            return new HandlerResult { Status = false, Message = "未能获取到要编辑的字段".ToLang() };

        #endregion 编辑


    }

    //列表显示
    public HandlerResult ShowFormatList()
    {
        string selected = RequestUtil.Instance.GetString("selected");
        if (!selected.IsEmpty())
        {
            string[] arrStr = selected.Split('|');
            for (int i = 0; i < arrStr.Length; i++)
            {
                string[] arrform = arrStr[i].Split(',');
                if (arrform.Length == 2)
                {
                    ServiceFactory.FormService.ModifyListShow(arrform[0].ToInt(), arrform[1].ToBoolean());
                }
            }
        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

    }

    //添加相关的信息
    private void AddRelation(int columnId, int mainPrimaryId)
    {
        var relationData = RequestUtil.Instance.GetFormString("hidRelationData").Trim(',');
        foreach (string relation in relationData.Split(','))
        {
            string[] data = relation.Split('|');
            if (data.Length < 5) continue;

            int mainColumnId = columnId;
            int attachPrimaryId = data[0].ToInt();
            int attachColumnId = data[1].ToInt();
            string linkText = Server.UrlDecode(data[2]);
            string linkUrl = Server.UrlDecode(data[3]);

            ServiceFactory.RelationService.Insert(mainPrimaryId, mainColumnId, attachPrimaryId, attachColumnId, linkText, linkUrl);
        }
    }

    //批量还原
    public HandlerResult Restore()
    {
        string selected = RequestUtil.Instance.GetString("selected");
        int columnId = RequestUtil.Instance.GetFormString("columnId").ToInt(0);
        var handlerResult = SysManagePageBase.JudgeActionPermission(columnId == 1 ? SysManagePageBase.IsCurrentRoleMenuRes("296") : SysManagePageBase.IsRoleHaveColumnRes("回收站", columnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        ServiceFactory.GridViewService.RestoreInfo(columnId, selected);
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    //还原单个
    public HandlerResult Rest()
    {
        string id = RequestUtil.Instance.GetString("id");
        int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
        var handlerResult = SysManagePageBase.JudgeActionPermission(columnId == 1 ? SysManagePageBase.IsCurrentRoleMenuRes("296") : SysManagePageBase.IsRoleHaveColumnRes("回收站", columnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        Model model = ServiceFactory.ModelService.GetModelByColumnId(columnId);
        if (model == null) return new HandlerResult { Status = true, Message = "操作失败".ToLang() };

        string sql = "UPDATE {0} SET IsDel=0 WHERE {0}_PID=@0".FormatWith(model.TableName);
        int result = DbHelper.CurrentDb.Execute(sql, id);

        if (result > 0)
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        else
            return new HandlerResult { Status = false, Message = "数据库无响应记录".ToLang() };
    }

    //启用
    public HandlerResult Enable()
    {
        string id = RequestUtil.Instance.GetString("id");
        int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("启用", columnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        Model model = ServiceFactory.ModelService.GetModelByColumnId(columnId);
        if (model == null) return new HandlerResult { Status = true, Message = "操作失败".ToLang() };
        //禁用全部
        string sql = "UPDATE {0} SET Enable=0 WHERE TypeID=@0".FormatWith(model.TableName);
        //启用
        string sqlUnEnable = "UPDATE {0} SET Enable=1 WHERE {0}_PID=@0".FormatWith(model.TableName);
        DbHelper.CurrentDb.Execute(sql, ColumnId);
        int result = DbHelper.CurrentDb.Execute(sqlUnEnable, id);
        if (result > 0)
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        else
            return new HandlerResult { Status = false, Message = "数据库无响应记录".ToLang() };
    }

    //禁用
    public HandlerResult Disable()
    {
        string id = RequestUtil.Instance.GetString("id");
        int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("启用", columnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        Model model = ServiceFactory.ModelService.GetModelByColumnId(columnId);
        if (model == null) return new HandlerResult { Status = true, Message = "操作失败".ToLang() };
        //禁用
        string sqlUnEnable = "UPDATE {0} SET Enable=0 WHERE {0}_PID=@0".FormatWith(model.TableName);
        int result = DbHelper.CurrentDb.Execute(sqlUnEnable, id);
        if (result > 0)
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        else
            return new HandlerResult { Status = false, Message = "数据库无响应记录".ToLang() };


    }

    //一键添加测试数据
    public HandlerResult AddTestData()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("416"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        XmlTextReader xmlTextReader = new XmlTextReader(Server.MapPath(SysPath + "Config/TestData.config"));
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.XmlResolver = null;
            xmlDoc.Load(xmlTextReader);
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;

            List<Column> AllColumn = ServiceFactory.ColumnService.GetListAllColumn(CurrentSiteId).ToList();
            int result = 0;
            foreach (var item in AllColumn)
            {
                Model model = ServiceFactory.ModelService.GetModelByColumnId(item.ColumnId);
                if (model == null)
                    continue;

                foreach (XmlNode xn in nodes)
                {
                    int ModelId = xn["ModelId"].InnerXml.ToInt(0);
                    if (ModelId == 0 || ModelId != item.ModelId)
                        continue;

                    #region  根据栏目的字段 添加相应的测试数据

                    IList<Form> listForm = ServiceFactory.FormService.GetListByColumnId(item.ColumnId);
                    Dictionary<string, object> dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    string col = "";
                    object val = new object();
                    foreach (Form form in listForm)
                    {
                        Field field = ServiceFactory.FieldService.GetFieldByFormId(form.FormId);
                        if (field == null) continue;

                        switch ((FieldType)field.FieldType)
                        {
                            case FieldType.ListBox:
                                {
                                    #region 选项
                                    col = ServiceFactory.DbService.GetColumnName(field.FieldName);
                                    val = 0;
                                    dict.Add(col, val);
                                    #endregion 选项
                                }
                                break;
                            case FieldType.DateTime:
                                {
                                    #region 日期
                                    col = ServiceFactory.DbService.GetColumnName(field.FieldName);
                                    val = DateTime.Now;
                                    dict.Add(col, val);
                                    #endregion 日期
                                }
                                break;
                            case FieldType.Bool:
                                {
                                    #region 是/否
                                    col = ServiceFactory.DbService.GetColumnName(field.FieldName);
                                    val = 0;
                                    dict.Add(col, val);
                                    #endregion 是/否
                                }
                                break;
                            case FieldType.Picture:
                            case FieldType.Video:
                            case FieldType.File:
                            case FieldType.Area:
                                {
                                    col = ServiceFactory.DbService.GetColumnName(field.FieldName);
                                    val = "";
                                    dict.Add(col, val);
                                }
                                break;
                            case FieldType.MultipleHtmlText:
                                {
                                    #region 用户控件
                                    col = ServiceFactory.DbService.GetColumnName(field.FieldName);
                                    if (xn[field.FieldName] != null)
                                        val = xn[field.FieldName].InnerXml;
                                    else
                                        val = "测试数据";
                                    dict.Add(col, val);
                                    #endregion 用户控件
                                }
                                break;
                            case FieldType.PassWord:
                            case FieldType.Text:
                            case FieldType.MultipleText:
                                {
                                    #region 单行文本

                                    col = ServiceFactory.DbService.GetColumnName(field.FieldName);
                                    if (field.FieldName == "CreateUser" || field.FieldName == "UpdateUser")
                                        val = SysManagePageBase.CurrentUserName;
                                    else if (xn[field.FieldName] != null)
                                        val = xn[field.FieldName].InnerXml;
                                    else
                                        val = "测试数据";
                                    dict.Add(col, val);
                                    #endregion 单行文本
                                }
                                break;
                            case FieldType.Money:
                                #region 金额
                                col = ServiceFactory.DbService.GetColumnName(field.FieldName);
                                dict.Add(col, 1.2); //默认1.2 金额
                                #endregion
                                break;
                            default:
                                {
                                    #region 其他字段
                                    col = ServiceFactory.DbService.GetColumnName(field.FieldName);

                                    if (field.FieldName.ToLower() == "sort")
                                    {
                                        val = DateTime.Now.ToString("yyMMddHHmmssff", DateTimeFormatInfo.InvariantInfo);
                                        //排序字段要赋值
                                        dict.Add(col, val);
                                    }
                                    else
                                    {
                                        val = 1;
                                        dict.Add(col, val);
                                    }
                                    #endregion
                                }
                                break;
                        }
                    }
                    string sql = ServiceFactory.DynamicFormService.GetInsertSql(item.ColumnId, 0, dict);
                    object[] parms = dict.Select(p => p.Value).ToArray();
                    string primaryKeyName = model.TableName + "_PID";
                    result = DbHelper.CurrentDb.Insert(primaryKeyName, sql, parms).ToInt();
                    #endregion
                }
            }
            if (result > 0)
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            else
                return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
        }
        finally
        {
            xmlTextReader.Close();
        }
    }

    //转回第一步工作流
    public HandlerResult SaveToFlow()
    {
        int itemId = RequestUtil.Instance.GetFormString("ItemId").ToInt(0);
        int ColumnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
        Model model = ServiceFactory.ModelService.GetModelByColumnId(ColumnId);
        if (model != null)
        {
            ServiceFactory.WorkFlowService.SetStateToFirstFlow(model.TableName, model.TableName + "_PID", itemId);
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        return new HandlerResult { Status = false, Message = "找不到相应的工作流".ToLang() };
    }

    //审核单篇工作流
    public HandlerResult Audit()
    {
        int itemId = RequestUtil.Instance.GetFormString("ItemId").ToInt(0);
        int ColumnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
        Model model = ServiceFactory.ModelService.GetModelByColumnId(ColumnId);
        if (model != null)
        {
            string sql = "UPDATE {0} SET state={1} WHERE {0}_PID=@0".FormatWith(model.TableName, -1, itemId);
            if (DbHelper.CurrentDb.Execute(sql, itemId).ToInt() > 0)
            {
                ServiceFactory.WorkFlowLogsService.AddLogs(ColumnId, itemId, "通过审核".ToLang());
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
        }

        return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
    }

    //单篇栏目推送
    public HandlerResult SinglePageCopy()
    {
        int itemId = RequestUtil.Instance.GetFormString("ItemId").ToInt(0);
        int ColumnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
        var selectedOpen = RequestUtil.Instance.GetString("SelectedOpen");
        Model model = ServiceFactory.ModelService.GetModelByColumnId(ColumnId);
        if (model != null)
        {
            string selected = RequestUtil.Instance.GetString("selected");
            CopySinglePageInfo(ColumnId,
                           SysManagePageBase.GetIntArrayByStringSplit(selected),
                           SysManagePageBase.GetStringArrayByStringSplit(selectedOpen)
                       );


            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }

        return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
         
    }

    /// <summary>
    /// 根据类别栏目ID, 获取类别层级表格
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="columnId"></param>
    /// <param name="subjectId">子站或专题ID，如不使用则传小于0的值</param>
    /// <param name="currentUserRolesId"></param>
    /// <param name="searchDic"></param>
    /// <param name="pageIndex"></param>
    /// <param name="orderBy"></param>
    /// <returns></returns>
    public PageDataSet GetCategoryTable(int pageIndex, int pageSize, int columnId, int subjectId,
        int currentUserRolesId, Dictionary<string, string> searchDic)
    {
        IList<Column> listColumns = ServiceFactory.ColumnService.GetMarkListByColumnId(columnId);
        Column column = listColumns.SingleOrDefault(p => p.MarkType == "Category" || p.ModuleMark == "Category_v0.0.01");
        Column mainColumn = listColumns.SingleOrDefault(p => p.MarkType.IsEmpty());

        if (mainColumn == null) return new PageDataSet();

        if (column != null)
        {
            Model model = ServiceFactory.ModelService.SingleOrDefault<Model>(column.ModelId);
            if (model == null) return new PageDataSet();

            //IList<Form> listFormListShow = ServiceFactory.FormService.GetListShowByColumnId(columnId);

            string sql = "SELECT {0} FROM {1} WHERE TypeID={2} {3} AND IsDel=0 ORDER BY {4}";

            //1. 表名 & 主键
            string queryTableName = model.TableName;
            string queryPrimaryKey = queryTableName + "_PID";

            #region 2. 要查询的列
            string queryColumns = queryPrimaryKey + "," + queryPrimaryKey + " as Id " + "  ";
            var parms = new List<object>();
            if (DictFormInList.Count > 0)
            {
                int i = 0;
                foreach (var formInList in DictFormInList)
                {
                    queryColumns += ",{0}".FormatWith(
                        ServiceFactory.DbService.GetColumnName(formInList.Value.FieldName)
                        );
                    foreach (var kv in searchDic)
                    {
                        if (kv.Key.ToLower() == formInList.Value.FieldName.ToLower())
                        {
                            switch ((FieldType)formInList.Value.FieldType)
                            {
                                case FieldType.DateTime:
                                    {
                                        if (kv.Value.IndexOf("<&&<", StringComparison.Ordinal) > -1)
                                        {
                                            Where += " and {0} between @{1} and @{2} ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key),
                                                i,
                                                i + 1);
                                            parms.Add(Regex.Split(kv.Value, "<&&<")[0]);
                                            i++;
                                            parms.Add(Regex.Split(kv.Value, "<&&<")[1]);
                                            i++;
                                        }
                                    }
                                    break;
                                case FieldType.Number:
                                case FieldType.Money:
                                    {
                                        if (kv.Value.IndexOf("<&&<", StringComparison.Ordinal) > -1)
                                        {
                                            Where += " and {0} between @{1} and @{2} ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key),
                                                i,
                                                i + 1);
                                            parms.Add(Regex.Split(kv.Value, "<&&<")[0]);
                                            i++;
                                            parms.Add(Regex.Split(kv.Value, "<&&<")[1]);
                                            i++;
                                        }
                                    }
                                    break;
                                case FieldType.Area:
                                    {
                                        Where += " and {0} = '{1}' ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), kv.Value);
                                        parms.Add(kv.Value);
                                        i++;
                                    }
                                    break;
                                case FieldType.ListBox:
                                    {
                                        var formOption = ServiceFactory.FormOptionService.GetFormOptionByFormID(formInList.Key.FormId.ToInt());
                                        if ((formOption.BindType == 3 || formOption.BindType == 4) && MainColumn.MarkType != "Category")//绑定多级类别  
                                        {
                                            List<string> ids = GetCategoryIds(kv.Value, formOption);
                                            ids.Add(kv.Value);
                                            if (ids.Count > 1)
                                            {
                                                Where += " and (";
                                                foreach (var id in ids)
                                                {
                                                    Where += "','+{0}+',' like '%,{1},%' or ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), id);
                                                }
                                                Where = Where.Substring(0, Where.Length - 3) + ")";
                                            }
                                            else
                                                Where += " and ','+{0}+',' like '%,{1},%' ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), kv.Value);
                                        }
                                        else
                                        {
                                            Where += " and {0} = @{1} ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), i);
                                            parms.Add(kv.Value);
                                            i++;
                                        }
                                    }
                                    break;
                                case FieldType.Bool:
                                    {
                                        Where += " and {0} = @{1} ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), i);
                                        parms.Add(kv.Value);
                                        i++;
                                    }
                                    break;
                                default:
                                    Where += " and {0} like '%'+@{1}+'%' ".FormatWith(ServiceFactory.DbService.GetColumnName(kv.Key), i);
                                    parms.Add(kv.Value);
                                    i++;
                                    break;

                            }
                        }
                    }
                }
            }

            #endregion

            if (subjectId > 0)//子站的信息
            {
                Where += " AND subjectid={0}".FormatWith(subjectId);
            }

            //单独类别权限控制
            if (mainColumn.IsCategoryPower && currentUserRolesId != 1)//开启类别权限
            {
                #region 单独类别权限控制
                string categoryWhere = "";

                if (column != null)
                {
                    //获取类别表名
                    string SQLC = "SELECT model.TableName FROM Whir_Dev_Column col INNER JOIN Whir_Dev_Model model ON col.ModelId=model.ModelId WHERE col.ColumnId=@0 and col.IsDel=0";
                    string tableName = DbHelper.CurrentDb.ExecuteScalar<string>(SQLC, column.ColumnId);

                    if (!tableName.IsEmpty())
                    {
                        string SQL2C = "SELECT {0}_PID FROM {0} WHERE  IsDel=0 AND TypeID=@0 AND SubjectID=@1 ORDER BY Sort DESC,CREATEDATE DESC".FormatWith(tableName);

                        DataTable tableT = DbHelper.CurrentDb.Query(SQL2C, column.ColumnId, subjectId).Tables[0];

                        if (tableT.Rows.Count > 0)
                        {
                            string pidName = tableName + "_PID";
                            foreach (DataRow row in tableT.Rows)
                            {
                                string categoryId = row[0].ToStr();
                                bool isHavePower = Whir.Security.ServiceFactory.RolesService.IsRoleHaveCategoryColumnRes(categoryId, column.ColumnId, currentUserRolesId, column.SiteId, subjectId);
                                if (isHavePower)
                                {
                                    categoryWhere += categoryId + ",";
                                }
                            }
                        }
                    }

                    if (categoryWhere.Length > 0)
                    {
                        Where += " AND {0}_PID IN ({1}) ".FormatWith(tableName, categoryWhere.Trim(','));
                    }
                    else
                    {
                        Where += " AND {0}_PID IN ({1}) ".FormatWith(tableName, -1);//相当于当前角色没有该栏目下的分类权限，返回的数据值应该为空
                    }
                }
                #endregion 单独类别权限控制
            }

            var queryOrderBy = "";
            if (!SortField.IsEmpty())
            {
                queryOrderBy = " {0} {1}".FormatWith(SortField, OrderType.IsEmpty() ? "DESC" : OrderType);
            }
            if (queryOrderBy.IsEmpty())
                queryOrderBy = "Sort DESC, CreateDate DESC";

            //3. 得到最终的查询语句, 并查询出数据
            sql = sql.FormatWith(
                queryColumns,
                queryTableName,
                columnId,
                Where,
                queryOrderBy);
            return DbHelper.CurrentDb.Query(pageIndex, pageSize, sql, parms.ToArray());
        }

        return new PageDataSet();
    }

    /// <summary>
    /// 公用列表批处理:推送单篇信息(复制信息)
    /// </summary>
    /// <param name="columnId">推送方的栏目ID</param>
    /// <param name="primaryValues">推送方的主键ID</param>
    /// <param name="toColumnIDs">接收方的栏目ID,格式:columnid|subjectid</param>
    /// <returns></returns>
    public bool CopySinglePageInfo(int columnId, int[] primaryValues, string[] toColumnIDs)
    {
        //1. 根据栏目ID获取到模型
        Model model = ServiceFactory.ModelService.GetModelByColumnId(columnId);
        if (model == null) return false;

        //2. 根据模型获取到此模型包含的所有字段
        var listField = ServiceFactory.FieldService.GetListByModelId(model.ModelId).Where(p => p.FieldName.ToLower() != "typeid" && p.FieldName.ToLower() != "subjectid");
        if (!listField.Any()) return false;
        var listForm = ServiceFactory.FormService.GetListByColumnId(columnId).ToList();

        Dictionary<string, object> dict = new Dictionary<string, object>();

        // 得到要推送的数据行
        foreach (int primary in primaryValues)
        {
            string querySql = "SELECT {0} FROM {1} WHERE {1}_PID={2}".FormatWith("*", model.TableName, primary);
            DataTable table = DbHelper.CurrentDb.Query(querySql, null).Tables[0];
            if (table.Rows.Count <= 0) continue;
            DataRow row = table.Rows[0];

            // 处理推送的列和数据                
            foreach (Field field in listField)
            {
                if (!listForm.Exists(p => p.FieldId == field.FieldId))
                    continue;

                if (field.FieldName.ToLower() == "categoryid")//批量推送信息不推送类别                     
                    continue;
                else
                    dict.Add(field.FieldName, row[field.FieldName].ToStr());
            }

            foreach (string toColumnId in toColumnIDs)
            {
                var targetColumnId = toColumnId.Split('|')[0].ToInt();
                int subjectId = toColumnId.Split('|').Length > 0 ? toColumnId.Split('|')[1].ToInt() : 0;

                //查询接受推送的栏目是否存在单篇信息
                string querySqlTo = "SELECT * FROM {1} WHERE TypeId={2} And SubjectId={0}".FormatWith(subjectId, model.TableName, targetColumnId);
                DataTable tableTo = DbHelper.CurrentDb.Query(querySqlTo, null).Tables[0];
            
                if (tableTo.Rows.Count > 0)
                {
                    var itemId = tableTo.Rows[0][model.TableName + "_PID"].ToInt();
                    ServiceFactory.HistoryBakHelper.BakInfo(targetColumnId, itemId);   //存入历史记录

                    string sql = ServiceFactory.DynamicFormService.GetUpdateSql(targetColumnId, itemId, dict);
                    object[] parms = dict.Select(p => p.Value).ToArray();
                    DbHelper.CurrentDb.Execute(sql, parms);
                }
                else
                {
                    var sql = ServiceFactory.DynamicFormService.GetInsertSql(targetColumnId, subjectId, dict);
                    object[] parms = dict.Select(p => p.Value).ToArray();
                    DbHelper.CurrentDb.Insert(model.TableName + "_PID", sql, parms);
                }
                ServiceFactory.ColumnService.CleanupStaticFiles(targetColumnId);
            }
        } 
        return true;
    }
}

