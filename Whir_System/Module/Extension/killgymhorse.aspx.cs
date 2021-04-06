/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：killgymhorse.aspx
 * 文件描述：木马查杀页面
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

using Whir.Framework;
using Whir.Domain.Common;
using Wuqi.Webdiyer;
using Whir.Language;

public partial class whir_system_module_extension_killgymhorse : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 木马查杀结果
    /// </summary>
    protected Dictionary<string, string> SearchResult { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("36"));
        //给清除缓存按钮注册点击事件
        Confirm(lbtnClearCache, "温馨提示：是否清除缓存？".ToLang());
    }
    protected void lbtnCheck_Click(object sender, EventArgs e)
    {
        if (!IsCurrentRoleMenuRes("336"))
        {
            string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.warning('{0}')</script>".FormatWith(NoPermissionMsg.ToLang());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
        }
        else
        {
            ExecuteCheck();
            BindData();
        }
    }

    /// <summary>
    /// 检测木马
    /// </summary>
    private void ExecuteCheck()
    {
        //每一次查杀都首先清空
        ltNoRecord.Text = "";

        SearchResult = CacheUtil.Instance.GetCache("searchresult") as Dictionary<string, string>;
        if (null != SearchResult && SearchResult.Count > 0)
        {
            return;
        }

        //初始化字典
        SearchResult = new Dictionary<string, string>();

        //获取服务器上 ASP.NET 应用程序的应用程序根路径。
        string root = Server.MapPath(Request.ApplicationPath);

        string[] defaultFileTypes = { "asp", "jsp", "php" };

        List<string> fileTypes = txtFileType.Text.Trim().ToLower().Split('|').ToList<string>();

        //添加默认的 asp,php,jsp
        fileTypes.AddRange(defaultFileTypes);

        string[] codeTypes = txtCodeType.Text.Trim().ToLower().Split('|');

        //遍历用户需要检测文件类型
        foreach (string fileType in fileTypes)
        {

            //在当前站点的目录下搜索所有的文件
            string[] files = Directory.GetFiles(root, "*." + fileType.Cut(7), SearchOption.AllDirectories);
            //遍历当前站点目录的所有文件（包含子目录）
            foreach (string file in files)
            {
                //获取扩展名
                string extension = Path.GetExtension(file);
                if (extension == ".asp" || extension == ".jsp" || extension == ".php")
                {
                    //获取文件名称
                    string filename = Path.GetFileName(file);
                    bool result = Regex.IsMatch(filename, string.Format(@"^\w+\.{0}$", fileType));
                    if (result)
                    {
                        SearchResult.Add(file.Replace(root, ""), "");
                        continue;
                    }
                }


                if (file.ToLower().Replace("\\","/").Contains(SysPath))
                {
                    continue;
                }

                string content = ReadFile(file.ToLower()).ToLower();

                //遍历用户需要检测的木马类型
                foreach (string codeType in codeTypes)
                {

                    //使用正则表达式匹配当前的文件中是否包含木马
                    if (Regex.IsMatch(content, string.Format(@"\s*{0}\s*", codeType)))
                    {
                        //首先判断是否已经存在当前的Key
                        if (IsExistKey(file.Replace(root, "")))
                        {
                            //判断是否已经存在当前的Value
                            if (!IsExistValue(codeType))
                            {
                                //如果没有存在当前的Value那么就修改Value到字典中
                                SearchResult[file.Replace(root, "")] += "、" + codeType; ;
                            }
                        }
                        else
                        {
                            //如果字典中没有存在当前的Key,则添加
                            SearchResult.Add(file.Replace(root, ""), codeType);
                        }
                    }
                }
            }
        }

        //缓存查杀的结果
        CacheUtil.Instance.SetCache("searchresult", SearchResult);
    }


    /// <summary>
    /// 读取文件中的内容
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    protected string ReadFile(string filename)
    {
        return File.ReadAllText(filename, Encoding.UTF8);
    }

    /// <summary>
    /// 判断字典中是否存在当前的key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private bool IsExistKey(string key)
    {
        return SearchResult.ContainsKey(key);
    }

    /// <summary>
    /// 判断字典是否存在当前的value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private bool IsExistValue(string value)
    {
        return SearchResult.ContainsValue(value);
    }

    /// <summary>
    /// 绑定数据方法
    /// </summary>
    private void BindData()
    {
        if (null != SearchResult && SearchResult.Count > 0)
        {
            List<KillGymhorse> list = new List<KillGymhorse>();

            foreach (var key in SearchResult.Keys)
            {
                KillGymhorse model = new KillGymhorse();
                model.FileName = key;
                model.Gymhorse = SearchResult[key];

                //添加到泛型集合中
                list.Add(model);
            }

            //绑定
            rptFileInfos.DataSource = list;
            rptFileInfos.DataBind();
        }
        else
        {
            //绑定
            rptFileInfos.DataSource = null;
            rptFileInfos.DataBind();
            ltNoRecord.Text = "恭喜您：没有查杀到任何可疑文件".ToLang();
        }
    }
    
    /// <summary>
    /// 清除缓存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnClearCache_Click(object sender, EventArgs e)
    {
        if (!IsCurrentRoleMenuRes("337"))
        {
            string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.warning('{0}')</script>".FormatWith(NoPermissionMsg.ToLang());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
        }
        else
        {
            //Confirm(lbtnClearCache, "是否清除缓存？");
            //从缓存中读取
            string[] filenames = CacheUtil.Instance.GetCache("sitefilename") as string[];
            SearchResult = CacheUtil.Instance.GetCache("searchresult") as Dictionary<string, string>;
            //如果缓存已经存在，就清除
            if (filenames != null || null != SearchResult)
            {
                CacheUtil.Instance.Remove("sitefilename");
                CacheUtil.Instance.Remove("searchresult");

                Alert("清除缓存成功".ToLang());
            }
            else
            {
                Alert("当前没有缓存".ToLang());
            }
        }
    }

    /// <summary>
    /// 对URL进行编码
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string UrlEncode(object obj)
    {
        if (obj != null)
        {
            string filename = obj.ToStr();

            return UrlConvertor(filename);
        }
        else
        {
            Alert("参数类型非法".ToLang());
            return "";
        }
    }

    //本地物理路径转换成URL相对路径
    protected string UrlConvertor(string filename)
    {
        if (Request.Url.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) || Request.Url.Host == "127.0.0.1")
        {
            //本地
            filename = filename.Replace(@"\", "/");
            Match match = Regex.Match(filename, @"\S+({0}\S+)".FormatWith(AppName));

            if (match.Groups.Count > 1)
            {
                return match.Groups[1].Value;
            }
            else
                return "";
        }
        else  //IIS上
        {
            //如果在本地执行是不可以的，必须部署在IIS上才可以 ，因为IIS上的网站的根目录为 / 
            return filename.Replace(Request.PhysicalApplicationPath, "/").Replace("\\", "/");
        }
    }
}