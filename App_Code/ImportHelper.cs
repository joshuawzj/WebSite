using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Globalization;

using Whir.Repository;
using Whir.Service;
using Whir.Framework;
using Whir.Language;

/// <summary>
/// ImportHelper 的摘要说明
/// </summary>
public class ImportHelper
{

    /// <summary>
    /// 内容导入辅助方法
    /// </summary>
    /// <param name="columnId"></param>
    /// <param name="subjectId"></param>
    /// <param name="filePath"></param>
    /// <param name="strategy">重复策略：True:覆盖，False:跳过</param>
    /// <param name="dict"></param>
    /// <param name="currentUserName"></param>
    /// <returns></returns>
    public static string ContentImport(int columnId, int subjectId, string filePath, bool strategy, Dictionary<string, string[]> dict, string currentUserName)
    {
        string result = "";//导入状态
        int successCount = 0;//成功个数
        int failCount = 0;//失败个数

        string fileDir = HttpContext.Current.Server.MapPath(filePath);
        if (!FileSystemHelper.IsFieldExist(fileDir))
            throw new Exception("未找到导入文件".ToLang());
        if (dict == null || dict.Count <= 0)
            throw new Exception("无导入匹配项".ToLang());

        DataTable excelTable = ExcelUtil.Import(fileDir);
        FileSystemHelper.DeleteFile(fileDir);
        var totalCount = excelTable.Rows.Count;

        string tableName = DbHelper.CurrentDb.ExecuteScalar<object>("SELECT TableName FROM WHIR_DEV_MODEL WHERE ModelId=(SELECT ModelId FROM WHIR_DEV_COLUMN WHERE COLUMNID=@0 and IsDel=0)", columnId).ToStr();
        if (tableName.IsEmpty())
        {
            throw new Exception("未找到对应栏目".ToLang());
        }

        //是否导入时间，不导入时间默认当前时间
        bool importCreateDate = dict.Any(p => p.Key.ToLower() == "createdate");
        try
        {
            DbHelper.CurrentDb.BeginTransaction();
            for (int i = 0; i < totalCount; i++)
            {
                DataRow row = excelTable.Rows[i];

                //拼接插入SQL
                StringBuilder sbInsertSql = new StringBuilder();
                sbInsertSql.AppendFormat("INSERT INTO {0} ", tableName);

                //拼接更新SQL
                StringBuilder sbUpdateSql = new StringBuilder();
                sbUpdateSql.AppendFormat("UPDATE {0} SET ", tableName);

                string insertTabColNames = string.Empty;
                string insertTabColValues = string.Empty;
                List<object> Params = new List<object>();
                List<object> paramsOnly = new List<object>();
                string whereOnly = "";
                string whereUpdate = "";

                int j = 0;
                int onlyCount = 0;
                foreach (KeyValuePair<string, string[]> pair in dict)
                {
                    object val = TransformType(row[pair.Value[0]], pair.Value[1]);
                    if (pair.Value[2] == "1")//唯一性判断
                    {
                        whereOnly = whereOnly + pair.Key + "=@" + onlyCount + " AND ";
                        whereUpdate = whereUpdate + pair.Key + "=@" + (dict.Count + onlyCount) + " AND ";
                        paramsOnly.Add(val);
                        onlyCount++;
                    }

                    Params.Add(val);
                    insertTabColNames = insertTabColNames + ServiceFactory.DbService.GetColumnName(pair.Key) + ",";//拼接字段
                    insertTabColValues = insertTabColValues + "@" + j + ",";//拼接参数名

                    sbUpdateSql.AppendFormat("{0}=@{1},", ServiceFactory.DbService.GetColumnName(pair.Key), j);//拼接更新语句
                    j += 1;
                }

                if (paramsOnly.Count > 0)//唯一性判断
                {
                    string sqlOnly = "SELECT COUNT(1) FROM {0} WHERE {1} TYPEID={2} AND SUBJECTID={3}".FormatWith(tableName, whereOnly, columnId, subjectId); ;
                    int count = DbHelper.CurrentDb.ExecuteScalar<object>(sqlOnly, paramsOnly.ToArray()).ToInt();
                    if (count > 0)//存在记录，并且覆盖策略
                    {
                        if (strategy)
                        {
                            Params.AddRange(paramsOnly);
                            sbUpdateSql.Remove(sbUpdateSql.Length - 1, 1);//去除","号
                            sbUpdateSql.AppendFormat(" WHERE {0} TYPEID={1} AND SUBJECTID={2}", whereUpdate, columnId, subjectId);
                            bool isSuccess = DbHelper.CurrentDb.Execute(sbUpdateSql.ToStr(), Params.ToArray()) > 0;//更新操作
                            if (isSuccess)
                            {
                                ++successCount;
                            }
                            else
                            {
                                ++failCount;
                            }
                        }
                        continue;
                    }
                }
                //添加系统字段
                if (importCreateDate)
                {
                    insertTabColNames = insertTabColNames + "TYPEID,SUBJECTID,STATE,SORT,ISDEL,CREATEUSER,UPDATEUSER,UPDATEDATE";
                    insertTabColValues = insertTabColValues + "@" + j + ",@" + (j + 1) + ",@" + (j + 2) + ",@" + (j + 3)
                    + ",@" + (j + 4) + ",@" + (j + 5) + ",@" + (j + 5) + ",@" + (j + 6);
                }
                else
                {
                    insertTabColNames = insertTabColNames + "CREATEDATE,TYPEID,SUBJECTID,STATE,SORT,ISDEL,CREATEUSER,UPDATEUSER,UPDATEDATE";
                    insertTabColValues = insertTabColValues + "@" + j + ",@" + (j + 1) + ",@" + (j + 2) + ",@" + (j + 3)
                   + ",@" + (j + 4) + ",@" + (j + 5) + ",@" + (j + 5) + ",@" + (j + 6) + ",@" + (j + 7);
                    Params.Add(DateTime.Now);
                }

                Params.Add(columnId);
                Params.Add(subjectId);
                Params.Add(0);
                Params.Add(Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmssff", DateTimeFormatInfo.InvariantInfo)));
                Params.Add(false);
                Params.Add(currentUserName);
                Params.Add(DateTime.Now);
                sbInsertSql.AppendFormat("({0})VALUES({1})", insertTabColNames, insertTabColValues);
                bool isOk = DbHelper.CurrentDb.Execute(sbInsertSql.ToStr(), Params.ToArray()) > 0;

                if (isOk)
                {
                    ++successCount;
                }
                else
                {
                    ++failCount;
                    result += "第{0}行".ToLang().FormatWith(i + 1) + "：导入失败<br />".ToLang();
                }
            }
            DbHelper.CurrentDb.CompleteTransaction();
            string tip = "总导入个数：".ToLang() + totalCount + "<br />";
            tip = tip + "成功个数：".ToLang() + successCount + "<br />";
            tip = tip + "失败个数：".ToLang() + failCount + "<br />";
            result = tip + result;
            if (successCount > 0)
            {
                //清除静态文件
                ServiceFactory.ColumnService.CleanupStaticFiles(columnId);
            }
        }
        catch (Exception ex)
        {
            DbHelper.CurrentDb.AbortTransaction();
            result = ex.Message.ToLang();
        }
        return result;
    }

    /// <summary>
    /// 根据类型名称更改值类型
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="typeName">数据库字段类型</param>
    /// <returns></returns>
    private static object TransformType(object value, string typeName)
    {
        object val;
        switch (typeName.ToLower())
        {
            case "nvarchar":
            case "char":
            case "varchar":
            case "ntext":
            case "text":
                val = value.ToStr();
                break;
            case "datetime":
                val = value.ToDateTime(DateTime.Now);
                break;
            case "int":
                val = value.ToInt();
                break;
            case "bit":
                val = value.ToBoolean();
                break;
            case "money":
                val = value.ToDecimalFormat();
                break;
            default:
                val = value.ToStr();
                break;
        }
        return val;
    }
}