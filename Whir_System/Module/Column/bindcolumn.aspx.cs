/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：bindcolumn.aspx.cs
 * 文件描述：栏目模版批处理
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;


public partial class whir_system_module_column_bindcolumn : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    public string CreateModeNull
    {
        get
        {
            return "不生成".ToLang();
        }

    }

    public string CreateModeHtml
    {
        get
        {
            return "静态".ToLang();
        }
    }

    public string CreateModelAspx
    {
        get
        {
            return "动态".ToLang();
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            BindTemplate();
            BindColumnDrop();
            Bind();
            //因为在绑定栏目类型时使用过栏目类型的DROP,所以绑定多一次DTOP的默认值为0
            dropColumn.SelectedValue = "0";
        }
    }

    /// <summary>
    /// 绑定模版列表
    /// </summary>
    private void BindTemplate()
    {
        //设置当前路径
        string pathFront = AppName + CurrentSitePath;//如：/WebSite/sitecn

        //固定为template
        pathFront += "/template";

        string dir = Server.MapPath(pathFront);
        if (!FileSystemHelper.IsFolderExist(dir))
        {
            return;
        }

        //dropTemplate.DataSource = FileSystemHelper.GetDirectoryInfos(dir, FsoMethod.File, "|.svn|", ".shtml");
        //dropTemplate.DataTextField = "name";
        //dropTemplate.DataValueField = "name";
        //dropTemplate.DataBind();
        //dropTemplate.Items.Insert(0, new ListItem("==" + "请选择".ToLang() + "==", ""));

        var list = new List<string>();
        GetTemplateFiles(dir, list);
        var dic = new Dictionary<string, string>();

        foreach (string filepath in list)
        {
            //E:\05.工作专区\v405\WebSite\en\template\test\test.html
            var file = new FileInfo(filepath);
            if (file.FullName.Contains(".svn"))
                continue;
            var path = file.FullName.Replace(dir + "\\", "").Replace("\\", "/");
            var name = path;
            dic.Add(path, name);
        }
        dropTemplate.Items.Clear();
        dropTemplate.DataSource = dic;
        dropTemplate.DataTextField = "Value";
        dropTemplate.DataValueField = "Key";
        dropTemplate.DataBind();
        dropTemplate.Items.Insert(0, new ListItem("==" + "请选择".ToLang() + "==", ""));

    }

    private void GetTemplateFiles(string dir, List<string> list)
    {
        //添加文件
        list.AddRange(Directory.GetFiles(dir));

        //如果是目录，则递归
        var directories = new DirectoryInfo(dir).GetDirectories();
        foreach (DirectoryInfo item in directories)
        {
            GetTemplateFiles(item.FullName, list);
        }
    }


    /// <summary>
    /// 绑定栏目类型
    /// </summary>
    private void BindColumnDrop()
    {
        //获取全部
        IList<Model> model = ServiceFactory.ModelService.GetListByParentId(0);
        dropColumn.DataSource = model;
        dropColumn.DataTextField = "ModelName";
        dropColumn.DataValueField = "ModelID";
        dropColumn.DataBind();
        dropColumn.Items.Insert(0, new ListItem("==" + "无功能模块".ToLang() + "==", "0"));
    }

    /// <summary>
    /// 绑定数据到rp
    /// </summary>
    private void Bind()
    {
        //网站首页
        SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(CurrentSiteId);
        Column virtualColumn = ModelFactory<Column>.Insten();
        if (siteInfo != null)
        {
            virtualColumn.ColumnId = -1;
            virtualColumn.ParentId = 0;
            virtualColumn.DefaultTemp = siteInfo.DefaultTemp;
            virtualColumn.CreateMode = siteInfo.CreateMode;
            virtualColumn.ColumnName = "网站首页".ToLang();
        }
        // 获取栏目列表, 以层级显示栏目名
        IList<Column> list = ServiceFactory.ColumnService.GetListByNoRemark(0, CurrentSiteId);

        list.Insert(0, virtualColumn);
        rptSiteMap.DataSource = list;
        rptSiteMap.DataBind();

        ShowData(list);
    }

    /// <summary>
    /// 展示数据显示方式和内容
    /// </summary>
    /// <param name="list"></param>
    private void ShowData(IList<Column> list)
    {
        Hashtable hash = new Hashtable();
        string map = string.Empty;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].ParentId == 0)
            {
                map += 0 + ",";
            }
            else
            {
                if (!hash.ContainsValue(list[i].ParentId))//如果不存在则添加 2条
                {
                    map += i + ",";
                    hash.Add(i, list[i].ParentId);
                }
                else
                {
                    foreach (int ikey in hash.Keys)
                    {
                        if (list[i].ParentId == hash[ikey].ToInt())
                        {
                            map += ikey + ",";
                        }
                    }
                }
            }
        }
        if (map.Length > 0)
        {
            hfStrMap.Value = map.TrimEnd(',');
        }
    }


    /// <summary>
    /// 保存更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbUpdate_Click(object sender, EventArgs e)
    {
        //栏目类型绑定
        string ids = Request[dropColumn.UniqueID];
        //栏目
        string val = this.ModelFieldValue.Value;

        string[] list = ids == "0" ? new string[] { } : ids.Split(',');
        string[] vallist = val == "0" ? new string[] { } : val.Split(',');
        for (int i = 0; i < list.Length - 1; i++)
        {
            string[] info = vallist[i].Split('|');
            Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(info[1].ToInt());
            column.ModelId = list[i].ToInt();
            ServiceFactory.ColumnService.UpdateColumn(column);
        }

        //模板绑定
        string tempids = Request[dropTemplate.UniqueID];
        string columnids = ColumnFieldValue.Value;

        string[] templist = tempids.IsEmpty() ? new string[] { } : tempids.Split(',');
        string[] columnlist = columnids == "0" ? new string[] { } : columnids.Split(',');

        if (templist.Length > 0)
        {
            for (int i = 0; i < columnlist.Length - 1; i++)
            {
                if (columnlist[i] == "")
                    continue;
                string[] vArray = columnlist[i].Split('|');
                if (vArray[0].ToStr() == "DefaultTemp" && vArray[1].ToInt() == -1)//网站首页
                {
                    SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(CurrentSiteId);
                    siteInfo.DefaultTemp = templist[i];
                    ServiceFactory.SiteInfoService.Update(siteInfo);
                    continue;
                }
                Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(vArray[1].ToInt());
                //首页模版
                if (vArray[0].ToStr() == "DefaultTemp")
                {
                    column.DefaultTemp = templist[i];
                }
                //列表模版
                else if (vArray[0].ToStr() == "ListTemp")
                {
                    column.ListTemp = templist[i];
                }
                //详细也模版
                else if (vArray[0].ToStr() == "ContentTemp")
                {
                    column.ContentTemp = templist[i];
                }
                ServiceFactory.ColumnService.Update(column);
                //操作记录日志
                ServiceFactory.ColumnService.SaveLog(column, "update");
            }
        }
        //绑定生成方式
        string createVals = hdCreateMode.Value.TrimEnd(',');
        string[] colModeList = createVals.Split(',');
        foreach (string colModel in colModeList)
        {
            string[] arr = colModel.Split('|');
            if (arr.Length < 2)
            {
                continue;
            }
            if (arr[0].ToInt() == -1)//网站首页
            {
                SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(CurrentSiteId);
                siteInfo.CreateMode = arr[1].ToInt();
                ServiceFactory.SiteInfoService.Update(siteInfo);
            }
            Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(arr[0].ToInt());
            if (column != null)
            {
                column.CreateMode = arr[1].ToInt();
                ServiceFactory.ColumnService.Update(column);
            }
        }
        Alert("操作成功".ToLang(), true);
    }
}