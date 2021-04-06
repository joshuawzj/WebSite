using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using Whir.Cache;
using Whir.Cache.Enum;
using Whir.Config;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using System.Linq;
using Whir.Language;
using System.Collections.Generic;
using System.Text.RegularExpressions;


public partial class Whir_System_Handler_Extension_UploadImages : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 新建文件夹
    /// </summary>
    /// <returns></returns>
    public HandlerResult AddNewFolder()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("306"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string path = Server.UrlDecode(Request.Form["path"]); //当前文件夹路径
        string folderName = Request.Form["folderName"]; //新建文件夹名称
        path = Server.MapPath(path);
        string newDirectory = path + folderName;

        if (Regex.IsMatch(folderName, @"[\/|\*]"))
            return new HandlerResult { Status = false, Message = "操作失败：文件夹名称不能包含*、/ 等特殊符号".ToLang() };
        if (!newDirectory.Contains(Server.MapPath(UploadFilePath).Trim('\\')))
            return new HandlerResult { Status = false, Message = "操作失败：试图在非法路径下创建文件夹".ToLang() };
        try
        {
            bool result = ServiceFactory.UploadFilesService.CreateDirectory(newDirectory);
            if (result)
            {
                //记录操作日志
                ServiceFactory.OperateLogService.Save(string.Format("创建目录【{0}】", newDirectory));
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

            }
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };

        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    /// <summary>
    /// 删除文件、文件夹
    /// </summary>
    public HandlerResult Delete()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("309"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }

        string singleFile = Request.Form["file"]; //当前删除的单个文件
        string commandName = Request.Form["commandName"]; //删除命令
        string hidChoose = Request.Form["hidChoose"]; //选择的多个文件

        try
        {
            if (commandName == "Delete")//列表内删除
            {
                singleFile = Server.MapPath(UploadFilePath + singleFile);
                string adverPath = Server.MapPath(UploadFilePath + "adver");
                string moduleTempPath = Server.MapPath(UploadFilePath + "ModuleTemp");

                if (singleFile.Equals(adverPath, StringComparison.OrdinalIgnoreCase)
                    || singleFile.Equals(moduleTempPath, StringComparison.OrdinalIgnoreCase))
                {
                    return new HandlerResult { Status = false, Message = "系统目录不允许删除".ToLang() };

                }
                if (ServiceFactory.UploadFilesService.GetFolderAndFile(UploadFilePath, singleFile).Rows.Count > 0)
                {
                    return new HandlerResult { Status = false, Message = "非空目录，不可删除".ToLang() };

                }
                ServiceFactory.UploadFilesService.Delete(singleFile);
                string realName = singleFile.Substring(singleFile.LastIndexOf('\\') + 1);
                ServiceFactory.UploadService.Delete("WHERE RealName=@0", realName);

                //记录操作日志
                ServiceFactory.OperateLogService.Save(string.Format("删除文件【{0}】", singleFile));
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

            }
            else if (commandName == "CheckDelete")//选中之后批量删除
            {
                string selected = hidChoose.Trim('*');
                foreach (string filePath in selected.Split('*'))
                {
                    string file = Server.MapPath(UploadFilePath + filePath);
                    ServiceFactory.UploadFilesService.Delete(file);
                    //记录操作日志
                    ServiceFactory.OperateLogService.Save(string.Format("删除文件【{0}】", filePath));
                    string realName = file.Substring(file.LastIndexOf('\\') + 1);
                    ServiceFactory.UploadService.Delete("WHERE RealName=@0", realName);
                }
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };

        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    /// <summary>
    /// 获取分页数据
    /// </summary>
    /// <returns></returns>
    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("32"), true);

        string path = Request.QueryString["path"];//当前文件夹路径
        string searchKeyWord = Request.QueryString["key"];//搜索 
        string allowFileType = Request.QueryString["allowFileType"];//列出指定格式文件 
        int offset = Request.QueryString["offset"].ToInt();//offset=20&limit=10
        int pageSize = Request.QueryString["limit"].ToInt();
        int pageIndex = offset / pageSize + 1;
        path = Server.MapPath(path);
        string extensionNames = "";

        if (!path.Contains(Server.MapPath(UploadFilePath).Trim('\\')))
        {
            Response.Clear();
            Response.Write("操作失败：非法路径");
            return;
        }

        if (allowFileType.IsEmpty())
        {
            var uploadConfig = ConfigHelper.GetUploadConfig();
            extensionNames = "{0}".FormatWith(uploadConfig.AllowPicType);
        }
        else
            extensionNames = allowFileType;

        string where = "Ext='/' ";
        foreach (string ext in extensionNames.Split('|'))
        {
            where += "or Ext='.{0}' ".FormatWith(ext);
        }

        DataTable dt = new DataTable();
        //如果是搜索操作，即循环所有文件夹进行筛选
        if (!searchKeyWord.IsEmpty())
            dt = ServiceFactory.UploadFilesService.GetFileByPath(Server.MapPath(UploadFilePath), path, searchKeyWord);
        else
            dt = ServiceFactory.UploadFilesService.GetFolderAndFile(Server.MapPath(UploadFilePath), path);

        DataView dv = dt.DefaultView;
        dv.RowFilter = where;

        dt = dv.ToTable();

        long total = dt.Rows.Count;
        dt = SplitDataTable(dt, pageIndex, pageSize);
        var js = dt.ToJson();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, js);
        Response.Clear();
        Response.Write(json);

    }


    /// <summary>
    /// 根据索引和pagesize返回记录
    /// </summary>
    /// <param name="dt">记录集 DataTable</param>
    /// <param name="pageIndex">当前页</param>
    /// <param name="pageSize">一页的记录数</param>
    /// <returns></returns>
    public static DataTable SplitDataTable(DataTable dt, int pageIndex, int pageSize)
    {
        if (pageIndex == 0)
            return dt;
        DataTable newdt = dt.Clone();
        int rowbegin = (pageIndex - 1) * pageSize;
        int rowend = pageIndex * pageSize;

        if (rowbegin >= dt.Rows.Count)
            return newdt;

        if (rowend > dt.Rows.Count)
            rowend = dt.Rows.Count;
        for (int i = rowbegin; i <= rowend - 1; i++)
        {
            DataRow newdr = newdt.NewRow();
            DataRow dr = dt.Rows[i];
            foreach (DataColumn column in dt.Columns)
            {
                newdr[column.ColumnName] = dr[column.ColumnName];
            }
            newdt.Rows.Add(newdr);
        }

        return newdt;
    }

    /// <summary>
    /// 加水印
    /// </summary>
    /// <returns></returns>
    public HandlerResult Watermark()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("308"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string hidChoose = Request.Form["hidChoose"]; //选择的多个文件

        string selectedImageName = hidChoose;
        foreach (string selected in selectedImageName.Split('*'))
        {
            string fileName = selected.Substring(selected.LastIndexOf('/') + 1);
            string path = Server.MapPath(UploadFilePath + selected).Replace(fileName, "");
            ServiceFactory.UploadFilesService.Watermark(path, fileName, UploadFilePath);
        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

    }


    #region 绑定树形文件夹结构

    //绑定上传文件目录树形结构
    public HandlerResult GetFolderTree()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("32"), true);

        string rootFolder = Request.Form["rootFolder"]; //当前文件、文件夹路径
        string folderTree = "[{text:'";
        rootFolder = rootFolder.TrimEnd('/');
        folderTree += rootFolder + "',href:'/',nodes:[";
        rootFolder = Server.MapPath(rootFolder);

        if (!rootFolder.Contains(Server.MapPath(UploadFilePath).Trim('\\')))
            return new HandlerResult { Status = false, Message = "获取文件夹失败：非法路径".ToLang() };

        folderTree += GetTree(rootFolder) + "]}]";

        return new HandlerResult { Status = true, Message = folderTree };

    }

    //获取文件夹树
    private string GetTree(string path)
    {
        string tree = "";
        IList<DirectoryInfo> listDirectoryInfo = ServiceFactory.UploadFilesService.GetChildDirectoryInfo(path);
        if (listDirectoryInfo.Count > 0)
        {
            foreach (DirectoryInfo di in listDirectoryInfo)
            {
                string temp = GetTree(di.FullName);
                string href = di.FullName.Replace(Server.MapPath(UploadFilePath), "").Replace("\\", "/") + "/";
                if (temp.IsEmpty())
                    tree += "{text: '" + di.Name + "',href:'" + href + "'},";
                else
                    tree += "{text: '" + di.Name + "',href:'" + href + "',nodes:[" + temp + "]},";

            }
            tree = tree.Substring(0, tree.Length - 1);
        }
        return tree;
    }

    #endregion 绑定树形文件夹结构

}