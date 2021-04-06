using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Whir.Cache;
using Whir.Cache.Enum;
using Whir.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;
using Whir.ezEIP.Web;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using Whir.Language;

public partial class whir_system_module_extension_CollectSaveToDb : SysManagePageBase
{
    /// <summary>
    /// 图片地址匹配正则
    /// </summary>
    public static Regex regex = new Regex("(?i)src\\s*=\\s*(['|\"])(?<path>[^\"']*?\\.(?<ext>(bmp|gif|i" + "con|jpeg|jpg|png)))\\1",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        JudgeActionPermission(IsCurrentRoleMenuRes("354"), true);
        if (!IsPostBack)
        {
            var sw = new Stopwatch();
            sw.Start();
            int collectId = RequestUtil.Instance.GetFormString("collectId").ToInt();
            string url = RequestUtil.Instance.GetFormString("url");
            string title = RequestUtil.Instance.GetFormString("title");
            var isAllowRepeat = RequestUtil.Instance.GetFormString("isAllowRepeat").ToBoolean(false);
            var filterField = RequestUtil.Instance.GetFormString("filterField");
            var sort = RequestUtil.Instance.GetFormString("sort");

            //try
            //{
            if (collectId > 0 & !string.IsNullOrEmpty(url))
            {
                var collection = ServiceFactory.CollectService.SingleOrDefault<Collect>(collectId);

                if (collection != null)
                {
                    string tableName =
                               DbHelper.CurrentDb.ExecuteScalar<object>(
                                   @"SELECT tableName FROM   Whir_Dev_Model wdm WHERE  wdm.ModelID IN (SELECT modelId FROM   Whir_Dev_Column wdc WHERE  wdc.ColumnId = @0 and IsDel=0)",
                                   collection.ColumnId).ToStr();

                    #region 采集规则

                    string ruleCacheKey = CacheKeys.CollectionRulesPrefix + collection.CollectId;
                    var rules = SiteCache.Get(ruleCacheKey) as DataTable;
                    if (rules == null)
                    {
                        rules = ServiceFactory.CollectFieldService.Query(
                            @"SELECT Id,FormId,TextStart,TextEnd,CollectType,CollectId,DefaultValue FROM   Whir_Ext_CollectField",
                            collection.CollectId).Tables[0];
                        SiteCache.Insert(ruleCacheKey, rules, 10 * 60);
                    }

                    #endregion

                    #region 表单字段

                    string formCacheKey = CacheKeys.CollectionFormsPrefix + collection.CollectId;

                    var fields = SiteCache.Get(formCacheKey) as DataTable;
                    if (fields == null)
                    {
                        fields = DbHelper.CurrentDb.Query(@"SELECT form.FormId,field.FieldName,form.FieldAlias,field.FieldType,form.DefaultValue,form.Sort FROM Whir_Dev_Form form INNER JOIN Whir_Dev_Field field ON form.FieldID=field.FieldID
                    WHERE form.IsDel=0 AND ColumnId=@0 AND field.IsHidden=0 AND field.FieldName!='RedirectUrl' AND field.fieldName!='EnableRedirectUrl' ORDER BY Sort Desc",
                            collection.ColumnId).Tables[0];
                        SiteCache.Insert(formCacheKey, fields, 10 * 60);
                    }

                    #endregion

                    #region   第一步：获取网页源代码

                    string content = HttpOperater.GetHtml(url);

                    #endregion

                    if (!content.IsEmpty())
                    {
                        #region 第二步：匹配字段值

                        var fieldKeyValue = new Dictionary<string, object>();

                        foreach (DataRow row in fields.Rows)
                        {
                            int formId = row["FormId"].ToInt();
                            string fieldName = row["FieldName"].ToStr();
                            int fieldType = row["FieldType"].ToInt();
                            object fieldValue = null;
                            if (!fieldKeyValue.ContainsKey(fieldName))
                            {
                                foreach (DataRow rule in rules.Rows)
                                {
                                    int ruleFromId = rule["FormId"].ToInt();
                                    if (formId == ruleFromId)
                                    {
                                        int collectType = rule["CollectType"].ToInt();
                                        string defaultValue = rule["DefaultValue"].ToStr();
                                        string textStart = rule["TextStart"].ToStr().Replace("\r", "");
                                        string textEnd = rule["TextEnd"].ToStr().Replace("\r", "");
                                        switch (collectType)
                                        {
                                            case 1:
                                                fieldValue = defaultValue;
                                                break;
                                            case 2:

                                                int startIndex = content.IndexOf(textStart, StringComparison.Ordinal);
                                                var indexHtml = content.Substring(startIndex + textStart.Length);
                                                int endIndex = indexHtml.IndexOf(textEnd, StringComparison.Ordinal);
                                                if (endIndex != -1)
                                                {
                                                    fieldValue = indexHtml.Substring(0, endIndex).Trim();
                                                }
                                                break;
                                        }
                                        switch ((FieldType)fieldType)
                                        {
                                            #region 字段类型转换

                                            case FieldType.MultipleHtmlText:
                                            case FieldType.Picture:

                                                var htmlFrag = fieldValue.ToStr();
                                                if (collection.IsDownloadImages)//下载远程图片到本地
                                                {
                                                    var matchCollection = regex.Matches(htmlFrag);
                                                    for (int index = 0; index < matchCollection.Count; index++)
                                                    {
                                                        string imageUrl = matchCollection[index].Groups["path"].Value;
                                                        string absoluteUrl = HttpOperater.GetReallyUrl(imageUrl, url);
                                                        string strdir = "collect/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/";
                                                        string imageName = HttpOperater.DownloadImage(absoluteUrl, Server.MapPath(UploadFilePath) + strdir);
                                                        if (!imageName.IsEmpty())
                                                        {
                                                            htmlFrag = htmlFrag.Replace(imageUrl, UploadFilePath + strdir + imageName);
                                                        }
                                                    }
                                                }
                                                fieldValue = htmlFrag;
                                                break;
                                            case FieldType.MultipleText:
                                            case FieldType.Link:
                                            case FieldType.Video:
                                            case FieldType.File:
                                            case FieldType.Area:
                                                fieldValue = fieldValue.ToStr();
                                                break;
                                            case FieldType.PassWord:
                                            case FieldType.Text:
                                                fieldValue = fieldValue.ToStr().RemoveHtml();
                                                break;
                                            case FieldType.ListBox:
                                                break;
                                            case FieldType.Number:
                                                fieldValue = fieldValue.ToInt(0);
                                                break;
                                            case FieldType.Money:
                                                fieldValue = fieldValue.ToDecimal(0);
                                                break;
                                            case FieldType.DateTime:
                                                fieldValue = fieldValue.ToDateTime() <= "1900-01-01".ToDateTime()
                                                                 ? DateTime.Now
                                                                 : fieldValue.ToDateTime();
                                                break;
                                            case FieldType.Bool:
                                                fieldValue = fieldValue.ToBoolean();
                                                break;

                                            #endregion
                                        }
                                        fieldKeyValue.Add(fieldName, fieldValue);
                                        break;
                                    }
                                }
                            }
                        }
                        fieldKeyValue.Add("TypeID", collection.ColumnId);
                        fieldKeyValue.Add("State", 0);
                        fieldKeyValue.Add("IsDel", false);
                        fieldKeyValue.Add("Sort", sort);//DateTime.Now.ToString("yyMMddHHmmssff", DateTimeFormatInfo.InvariantInfo)
                        fieldKeyValue.Add("CreateUser", CurrentUser.LoginName);

                        #endregion

                        #region 第三步：拼接字符串并写入数据库

                        #region 拼接sql语句

                        string sql = "INSERT INTO {0} (".FormatWith(tableName);
                        int count = 0;
                        string s = "";

                        foreach (var pair in fieldKeyValue)
                        {
                            sql += pair.Key + ",";
                            s += "@" + count + ",";
                            count++;
                        }
                        sql = sql.TrimEnd(',');
                        sql += ") VALUES (" + s.TrimEnd(',') + ")";

                        #endregion

                        IList<object> parmsList = new List<object>();

                        foreach (KeyValuePair<string, object> pair in fieldKeyValue)
                        {
                            parmsList.Add(pair.Value);
                        }

                        bool exsit = false;
                        if (isAllowRepeat)
                        {
                            var exsitSql = "SELECT COUNT(*) FROM {0} WHERE {1}=@0 AND TypeId=@1".FormatWith(tableName, filterField);
                            exsit = DbHelper.CurrentDb.ExecuteScalar<object>(exsitSql, fieldKeyValue[filterField], collection.ColumnId).ToInt() > 0;
                        }
                        if (!exsit)
                        {
                            if (DbHelper.CurrentDb.Execute(sql, parmsList.ToArray()) > 0)
                            {
                                sw.Stop();
                                TimeSpan ts = sw.Elapsed;
                                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes,
                                                                   ts.Seconds,
                                                                   ts.Milliseconds / 10);
                                Response.Write(string.Format("{0} {2}{1}", title, elapsedTime,"采集成功，耗时：".ToLang()));
                            }
                            else
                            {
                                sw.Stop();
                                TimeSpan ts = sw.Elapsed;
                                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes,
                                                                   ts.Seconds,
                                                                   ts.Milliseconds / 10);
                                Response.Write("{0} {2}{1}".FormatWith(title, elapsedTime,"采集失败，耗时：".ToLang()));
                            }
                        }
                        else
                        {
                            Response.Write(title + " 已存在，忽略操作".ToLang());
                        }

                        #endregion
                    }

                }
                else
                {
                    Response.Write(title + " 对应采集项目不存在".ToLang());
                }
            }
            else
            {
                Response.Write(title + " 参数错误".ToLang());
            }
            //}
            //catch (Exception ex)
            //{
            //    Response.Write(title + " 采集异常：" + ex.Message);
            //}
        }
        Response.End();
    }
}