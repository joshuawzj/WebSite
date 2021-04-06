/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：column_bind_templater.aspx.cs
 * 文件描述：栏目模版批处理
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO;

using Whir.Language;
using Whir.Service;
using Whir.Domain;
using Whir.Framework;
using System.Collections;
using System.Linq;

public partial class whir_system_module_column_bind_templater : Whir.ezEIP.Web.SysManagePageBase
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

    public string ClearStaticHTML
    {
        get
        {
            return "自动清理".ToLang();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected int TrCount { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            BindTemplate();
            BindColumnDrop();
            BindListInfo();
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

    /// <summary>
    /// 获取模版文件
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="list"></param>
    private void GetTemplateFiles(string dir, List<string> list)
    {

        var files = Directory.GetFiles(dir);
        var allowFiles = files.Where(p => (new FileInfo(p).Extension == ".shtml" || new FileInfo(p).Extension == ".html" || new FileInfo(p).Extension == ".xml"));
        //添加文件
        list.AddRange(allowFiles);
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
        IList<Model> model = ServiceFactory.ModelService.GetListByParentId(0, false);
        dropColumn.DataSource = model;
        dropColumn.DataTextField = "ModelName";
        dropColumn.DataValueField = "ModelID";
        dropColumn.DataBind();
        dropColumn.Items.Insert(0, new ListItem("==" + "无功能模块".ToLang() + "==", "0"));


        //获取全部
        IList<Model> model2 = ServiceFactory.ModelService.GetListByParentId(0, true);
        ddpSubsite.DataSource = model2;
        ddpSubsite.DataTextField = "ModelName";
        ddpSubsite.DataValueField = "ModelID";
        ddpSubsite.DataBind();
        ddpSubsite.Items.Insert(0, new ListItem("==" + "无功能模块".ToLang() + "==", "0"));
    }

    /// <summary>
    /// 绑定数据到列表
    /// </summary>
    private void BindListInfo()
    {
        #region 内容管理

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
        rptMainColumnList.DataSource = list;
        rptMainColumnList.DataBind();
        TrCount = list.Count;
        hfStrMap.Value += ShowData(list);
        #endregion 内容管理

        #region 模版子站


        IList<SubjectClass> listSubTemp = ServiceFactory.SubjectClassService.GetSubsiteClassList(CurrentSiteId);

        rptSubTem.DataSource = listSubTemp;
        rptSubTem.DataBind();

        #endregion 模版子站

        #region 专题

        IList<SubjectClass> listSubject = ServiceFactory.SubjectClassService.GetSubjectClassList(CurrentSiteId);
        rptSubject.DataSource = listSubject;
        rptSubject.DataBind();

        #endregion 专题

        #region 自定义子站

        //IList<Subject> listSubsite = ServiceFactory.SubjectService.GetListSubsiteBySiteID(CurrentSiteID);
        //rptSubsite.DataSource = listSubsite;
        //rptSubsite.DataBind();

        #endregion 自定义子站
    }

    /// <summary>
    /// 展示数据显示方式和内容
    /// </summary>
    /// <param name="list"></param>
    private string ShowData(IList<Column> list)
    {
        Hashtable hash = new Hashtable();
        string map = string.Empty;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null)
                continue;
            if (list[i].ParentId == 0 && TrCount == list.Count)
            {
                map += 0 + ",";
            }
            else if (list[i].ParentId == 0 && TrCount != list.Count)
            {
                map += TrCount - list.Count + ",";
            }
            else
            {
                if (!hash.ContainsValue(list[i].ParentId))//如果不存在则添加 2条
                {
                    if (TrCount > list.Count)
                    {
                        map += (TrCount - (list.Count - i)) + ",";
                    }
                    else
                    {
                        map += i + ",";
                    }
                    hash.Add(i, list[i].ParentId);
                }
                else
                {
                    foreach (int ikey in hash.Keys)
                    {
                        if (list[i].ParentId == hash[ikey].ToInt())
                        {
                            if (TrCount > list.Count)
                            {
                                map += (TrCount - list.Count + ikey) + ",";
                            }
                            else
                            {
                                map += ikey + ",";
                            }
                        }
                    }
                }
            }
        }
        return map.TrimEnd(',');
    }


    /// <summary>
    /// 保存更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbUpdate_Click(object sender, EventArgs e)
    {
        #region  栏目名称、栏目别名

        string columnIds = Request["txtColumnId"];
        string columnNames = Request["txtColumn"];
        string columnNameStages = Request["txtColumnNameStage"];
        for (int i = 0; i < columnIds.Split(',').Length; i++)
        {
            Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnIds.Split(',')[i].ToInt(0));
            if (column == null)
                continue;
            column.ColumnName = columnNames.Split(',')[i];
            column.ColumnNameStage = columnNameStages.Split(',')[i];
            ServiceFactory.ColumnService.Update(column);
        }

        if (!columnIds.IsEmpty())
        {
            ContentHelper.SetColumnCookie("column_refresh_flag", "0", "0");
            ContentHelper.SetColumnCookie("subsite_refresh_flag", "0", "0");
            ContentHelper.SetColumnCookie("subject_refresh_flag", "0", "0");
        }


        #endregion

        #region  栏目类型绑定
        string ids = Request[dropColumn.UniqueID].TrimEnd('0').TrimEnd(',');
        string subsiteIds = Request[ddpSubsite.UniqueID].TrimEnd('0').TrimEnd(',');

        if (subsiteIds != "")
        {
            if (ids != "")
            {
                ids += "," + subsiteIds;
            }
            else
            {
                ids = subsiteIds;
            }
        }
        //栏目
        string val = this.ModelFieldValue.Value;

        string[] list = ids == "" ? new string[] { } : ids.Split(',');
        string[] vallist = val == "0" ? new string[] { } : val.Split(',');
        for (int i = 0; i <= list.Length - 1; i++)
        {
            string[] info = vallist[i].Split('|');
            Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(info[1].ToInt());
            if (column != null)
            {
                column.ModelId = list[i].ToInt();
                ServiceFactory.ColumnService.UpdateColumn(column);
            }
        }
        #endregion

        #region 模板绑定
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
                if (column == null)
                    continue;
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
        #endregion

        #region 绑定生成方式
        string createVals = hdCreateMode.Value.TrimEnd(',');
        string[] colModeList = createVals.Split(',');
        foreach (string colModel in colModeList)
        {
            string[] arr = colModel.Split('|');
            if (arr.Length < 4)
            {
                continue;
            }
            if (arr[0].ToInt() == -1)//网站首页
            {
                SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(CurrentSiteId);
                siteInfo.CreateMode = arr[1].ToInt();
                ServiceFactory.SiteInfoService.Update(siteInfo);
            }
            else
            {
                Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(arr[0].ToInt());
                if (column != null)
                {
                    column.CreateMode = arr[1].ToInt();
                    column.IsAutoCleanupStaticFiles = arr[2].ToBoolean();
                    column.IsCategory = arr[3].ToBoolean();
                    if (column.IsCategory)
                    {
                        if (column.CategoryLevel <= 0)
                        {
                            column.CategoryLevel = 1;
                        }
                    }
                    ServiceFactory.ColumnService.Update(column);
                }
            }
        }
        #endregion

        Alert("操作成功".ToLang(), true);
    }

    #region 模版子站、自定义子站、专题 栏目绑定事件
    /// <summary>
    /// 绑定模版子站、专题栏目
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptSubTem_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var hid = e.Item.FindControl("hidSubjectClassID") as HiddenField;
            var rpt = e.Item.FindControl("rptList") as Repeater;
            if (rpt != null && hid != null)
            {
                int subjectClassId = hid.Value.ToInt();
                IList<Column> list = ServiceFactory.ColumnService.GetSubjectColumnListByNomark(0, subjectClassId, false, 0);
                //加入首页
                list.Insert(0, ServiceFactory.ColumnService.GetSubjectIndexColumn(subjectClassId));
                rpt.DataSource = list;
                rpt.DataBind();
                TrCount++;
                TrCount += list.Count;
                if (hfStrMap.Value.IsEmpty())
                {
                    hfStrMap.Value = "0," + ShowData(list);
                }
                else
                {
                    hfStrMap.Value = hfStrMap.Value.TrimEnd(',') + ",0," + ShowData(list);
                }
            }
        }
    }

    /// <summary>
    /// 绑定自定义子站栏目
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptSubsite_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var hid = e.Item.FindControl("hidSubjectID") as HiddenField;
            var rpt = e.Item.FindControl("rptList") as Repeater;
            if (rpt != null && hid != null)
            {
                int subjectId = hid.Value.ToInt();
                IList<Column> list = ServiceFactory.ColumnService.GetSubjectColumnListByNomark(0, subjectId, true, 0);
                //加入首页
                list.Insert(0, ServiceFactory.ColumnService.GetSubjectIndexColumn(subjectId));
                rpt.DataSource = list;
                rpt.DataBind();
                TrCount++;
                TrCount += list.Count;
                if (hfStrMap.Value.IsEmpty())
                {
                    hfStrMap.Value = "0," + ShowData(list);
                }
                else
                {
                    hfStrMap.Value = hfStrMap.Value.TrimEnd(',') + ",0," + ShowData(list);
                }
            }
        }
    }

    //public  string GetModelName(string modelId)
    //{
    //    ModelService modelService=new ModelService();
    //    modelService.GetModelByFormID()
    //}


    #endregion

    //去除模板后缀
    public string getTempName(string tempName)
    {
        if (tempName.IsEmpty())
        {
            return "";
        }
        return tempName.Substring(0, tempName.LastIndexOf("."));
    }
}