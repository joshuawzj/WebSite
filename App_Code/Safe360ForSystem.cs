using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Web请求安全检查：防止跨站点脚本,Sql注入等攻击,来自:http://bbs.webscan.360.cn/forum.php?mod=viewthread&tid=711&page=1&extra=#pid1927
/// 检查数据包括:
/// 1.Cookie
/// 2.当前页面地址
/// 3.ReferrerUrl
/// 4.Post数据
/// 5.Get数据
/// </summary>
public class Safe360ForSystem
{
    #region 白名单
    public static List<string> WhiteList = new List<string>()
    {
        "Content",
        "其他Html字段"
    };
    #endregion

    #region 执行安全检查

    /// <summary>
    /// 执行安全检查
    /// </summary>
    public static void Procress()
    {
        const string errmsg = "<div>对不起，您的请求中带有不合法参数,服务器已终止请求！ 拦截时间：{$DateTime} <a href='/'> 点击跳转到网站首页！ </a></div>";
        if (RawUrl())
        {
            WriteErrMsg(errmsg);
        }

        if (CookieData())
        {
            WriteErrMsg(errmsg);
        }

        if (HttpContext.Current.Request.UrlReferrer != null)
        {
            if (Referer())
            {
                WriteErrMsg(errmsg);
            }
        }

        if (HttpContext.Current.Request.RequestType.ToUpper() == "POST")
        {
            if (PostData())
            {
                WriteErrMsg(errmsg);
            }
        }
        if (HttpContext.Current.Request.RequestType.ToUpper() == "GET")
        {
            if (GetData())
            {
                WriteErrMsg(errmsg);
            }
        }
    }

    /// <summary>
    /// 取得错误信息提示页内容
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static void WriteErrMsg(string content)
    {
        var errmsg = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                        <html xmlns='http://www.w3.org/1999/xhtml'>
                        <head>
                            <meta charset='utf-8' />
                            <title>系统安全检查模块</title>
                            <style type='text/css'>
                                body { font-size: 14px;font-family: 微软雅黑;}
                                .title { width: 80%; margin: 20px auto; border: 1px solid gainsboro; padding: 20px; background-color: #F7F7F7; font-size: 16px; text-align: center;font-weight: bold; color: brown; }
                                .content { width: 80%; margin: 20px auto; border: 1px solid gainsboro; padding: 20px; background-color: #F7F7F7; }
                                .footer { width: 80%; margin: 20px auto; border: 1px solid gainsboro; padding: 20px; background-color: #F7F7F7; text-align: center; color: gray; }
                                .err { color: red; padding: 10px;}
                             </style>
                        </head>
                        <body>
                            <div class='title'>
                               系统安全检查模块
                            </div>
                            <div class='content'>          
                                 <div class='err'>
                                      {$Content}
                                 </div>
                            </div>
                            <div class='footer'>
                                © " + DateTime.Now.Year + @" 万户网络. All Rights Reserved
                            </div>
                        </body>
                        </html>";

        errmsg = errmsg.Replace("{$Content}", content);
        errmsg = errmsg.Replace("{$DateTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        HttpContext.Current.Response.Write(errmsg);
        HttpContext.Current.Response.StatusCode = 400;
        HttpContext.Current.Response.ContentType = "text/html";
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    #endregion

    #region 安全检查正则

    /// <summary>
    /// 安全检查正则
    /// </summary>
    private const string StrRegex = @"\s*(<|>|ʺ|ʹ|'|alert|script|0style|<style|3cstyle|iframe|href)|<[^>]+?style=[\w]+?:expression\(|\b(ltrim|window|function|and|insert|select|delete|update|having|import|onmouseover|onfocus|onclick|expression|eval|alert|confirm|prompt|iframe|href)\b|^\+/v(8|9)|<[^>]*?=[^>]*?&#[^>]*?>|\b(and|or)\b.{1,6}?(=|>|<|\bin\b|\blike\b)|/\*.+?\*/|'|<\s*img\b|\bEXEC\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";


    #endregion

    #region 检查Post数据

    /// <summary>
    /// 检查Post数据
    /// </summary>
    /// <returns></returns>
    private static bool PostData()
    {
        bool result = false;

        for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
        {
            if (!WhiteList.Contains(HttpContext.Current.Request.Form.Keys[i]))
            {
                result = CheckData(HttpContext.Current.Request.Form[i]);
                if (result)
                {
                    break;
                }
            }
        }
        return result;
    }

    #endregion

    #region 检查Get数据

    /// <summary>
    /// 检查Get数据
    /// </summary>
    /// <returns></returns>
    private static bool GetData()
    {
        bool result = false;

        for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
        {
            if (!WhiteList.Contains(HttpContext.Current.Request.QueryString.Keys[i]))
            {
                result = CheckData(HttpContext.Current.Request.QueryString[i]);
                if (result)
                {
                    break;
                }
            }
        }
        return result;
    }

    #endregion

    #region 检查Cookie数据

    /// <summary>
    /// 检查Cookie数据
    /// </summary>
    /// <returns></returns>
    private static bool CookieData()
    {
        bool result = false;
        for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
        {
            result = CheckData(HttpContext.Current.Request.Cookies[i].Value);
            if (result)
            {
                break;
            }
        }
        return result;
    }

    #endregion

    #region 检查Referer

    /// <summary>
    /// 检查Referer
    /// </summary>
    /// <returns></returns>
    private static bool Referer()
    {
        return CheckData(HttpContext.Current.Request.UrlReferrer.ToString());
    }

    #endregion

    #region 检查当前请求路径

    /// <summary>
    /// 检查当前请求路径
    /// </summary>
    /// <returns></returns>
    private static bool RawUrl()
    {
        return CheckData(HttpContext.Current.Request.RawUrl);
    }

    #endregion

    #region 正则匹配

    /// <summary>
    /// 正则匹配
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static bool CheckData(string inputData)
    {
        if (inputData != null)
            return Regex.IsMatch(inputData, StrRegex, RegexOptions.IgnoreCase);
        else
            return false;
    }

    #endregion
}