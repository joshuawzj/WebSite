
using System.Text.RegularExpressions;
using System.Web;
using Whir.Service;
using Whir.Framework;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

/// <summary>
///RequestHelper 的摘要说明
/// </summary>
public class RequestHelper
{
	public RequestHelper()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    
    /// <summary>
    /// 判断文本中是否包含关键字符，关键字符在后台目录下的Config/NotAllowFieldName.config中配置
    /// 此配置文件同时关联了后台新建字段和新表单的“数据库字段名”验证的全文匹配
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsDangerWord(string text)
    {
        return GetDangerWord(text).Count() > 0;
    }

    /// <summary>
    /// 根据后台目录下的Config/NotAllowFieldName.config中配置，替换掉关键词
    /// 此配置文件同时关联了后台新建字段和新表单的“数据库字段名”验证的全文匹配
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string ReplaceDangerWord(string text, string replaceStr)
    {
        string[] arr = GetDangerWord(text);
        foreach (string str in arr)
        {
            text = text.Replace(str, replaceStr);
        }
        return text;
    }

    /// <summary>
    /// 根据后台目录下的Config/NotAllowFieldName.config中配置，找出文本中匹配的关键字
    /// 此配置文件同时关联了后台新建字段和新表单的“数据库字段名”验证的全文匹配
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string[] GetDangerWord(string text)
    {
        if (text.IsEmpty()) return new string[] { };
        text = text.ToLower();

        List<string> listWord = new List<string>();

        string filePath = WebUtil.Instance.AppPath() + AppSettingUtil.AppSettings["SystemPath"] + "Config/NotAllowFieldName.config";
        string fileDir = HttpContext.Current.Server.MapPath(filePath);

        XDocument doc = XDocument.Load(fileDir);

        XElement root = doc.Root;
        var nodes = from node in root.Elements()
                    select node;
        foreach (var node in nodes)
        {
            var arr = node.Value.Split('|').Select(p => p.ToLower());

            foreach (string str in arr)
            {
                if (str.IsEmpty()) continue;

                if (text.Contains(str))
                    listWord.Add(str);
            }
        }
        return listWord.ToArray();
    }
}