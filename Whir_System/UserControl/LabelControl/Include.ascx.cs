/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：Include.ascx.cs
 * 文件描述：Include动态置标（用户控件），适应于.aspx页面
 */
using System;
using System.Web.UI;
using System.Collections;
using System.Reflection;

using Whir.Framework;
using Whir.Service;
using Whir.Domain;



public partial class whir_system_UserControl_LabelControl_Include : Whir.ezEIP.Web.SysControlBase
{
    #region 属性

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 参数
    /// </summary>
    public string Params { get; set; }

    /// <summary>
    /// 站点目录
    /// </summary>
    public string SitePath { get; set; }
    /// <summary>
    /// 当前页面所属栏目实体（仅为栏目的首页、列表页、内容页时有值）
    /// </summary>
    public Column PageColumn { get; set; }
    /// <summary>
    /// 当前页面所属站点实体
    /// </summary>
    public SiteInfo PageSiteInfo { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Bind();
        }
        catch (Exception ex)
        {
            Show_Error(ex, this.Controls, ID);
        }
    }
    /// <summary>
    ///绑定数据
    /// </summary>
    public void Bind()
    { 
        if (SitePath.IsEmpty() && PageSiteInfo != null)
        {
            SitePath = PageSiteInfo.SiteId.ToStr();
        }
        if (SitePath.IsInt())
        {//是数字则按ID查找
            SitePath = ServiceFactory.SiteInfoService.GetSitePath(SitePath.ToInt());
        }

        //模板的绝对路径 
        string IncludeFilePath = Server.MapPath("~/") + SitePath + "/include/" + FileName.ToStr().ToLower().Replace(".html", "") + ".ascx";

        bool IsUserControl = false;
        //如果传入的文件名带有"/"则从网站根目录开始加载
        if (FileName.ToStr().Length > 0)
        {
            FileName = FileName.ToStr().Replace("\\", "/");
            if (FileName.Contains("/"))
            {
                IncludeFilePath = (Server.MapPath("~/").Replace("\\", "/") + FileName.ToStr().ToLower().Replace(".html", "")).Replace("//", "/");
                IsUserControl = true;//传手工建的用户控件
            }
        }
        if (FileSystemHelper.IsFieldExist(IncludeFilePath))
        {
            //模板相对路径 
            string path = AppName + SitePath + "/include/" + FileName.ToStr().ToLower().Replace(".html", "") + ".ascx";
            if (IsUserControl)
            {
                path = (AppName + FileName.ToStr().ToLower().Replace(".html", "")).Replace("//", "/");
            }
            
            UserControl myusercontrol = (UserControl)LoadControl(path);
            Type myusertype = myusercontrol.GetType();

            //params参数
            if (Params.ToStr() != "")
            {
                Hashtable hashParas = GetHashParas(Params);
                foreach (DictionaryEntry de in hashParas)
                {
                    try
                    {
                        PropertyInfo mypassinfo = myusertype.GetProperty(de.Key.ToStr());
                        mypassinfo.SetValue(myusercontrol, de.Value.ToStr(), null);//给用户控件属性赋值
                    }
                    catch { continue; } //防止传多于实际的参数报错
                }

            }
            try
            {
                PropertyInfo mypassinfo = myusertype.GetProperty("PageColumn");
                mypassinfo.SetValue(myusercontrol, PageColumn, null);//给用户控件属性赋值
                mypassinfo = myusertype.GetProperty("PageSiteInfo");
                mypassinfo.SetValue(myusercontrol, PageSiteInfo, null);//给用户控件属性赋值
            }
            catch { }

            ph.Controls.Add(myusercontrol);//动态添加用户控件
        }
        else
        {
            throw new Exception("找不到模板：{0}".FormatWith(IncludeFilePath));
        }
    }

    /// <summary>
    /// 处理params参数 
    /// </summary>
    /// <param name="paras">id:1|type:0</param>
    private Hashtable GetHashParas(string paras)
    {
        Hashtable hashParas = new Hashtable();
        paras = paras.Trim().Trim('{', '}').Trim();
        string[] StrParas = new string[0];
        if (paras.Contains("|"))
        {
            StrParas = new string[paras.Split('|').Length];
            StrParas = paras.Split('|');

            foreach (string s in StrParas)
            {
                if (paras.Contains(":"))
                {
                    string[] parasValue = s.Split(':');
                    if (parasValue.Length > 1)
                    {
                        if (!hashParas.ContainsKey(parasValue[0].ToLower()))
                        hashParas.Add(parasValue[0].ToLower(), parasValue[1]);
                    }
                }
            }
        }
        else
        {
            if (paras.Contains(":"))
            {
                string[] parasValue = paras.Split(':');
                if (parasValue.Length > 1)
                {
                    if (!hashParas.ContainsKey(parasValue[0].ToLower()))
                    hashParas.Add(parasValue[0].ToLower(), parasValue[1]);
                }
            }
        }
        return hashParas;

    }

}