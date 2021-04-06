using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Whir.Repository;
using Whir.ezEIP.Web;

public partial class whir_system_module_release_ClearReleaseFiles : SysManagePageBase
{
    protected List<string> Dirs;
    protected List<string> Files;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            LoadConfig();
        }
    }

    private void LoadConfig()
    {
        #region 默认系统文件

        Dirs = new List<string>();
        Files = new List<string>();

        Dirs.Add("Ajax");
        Dirs.Add("App_Code");
        Dirs.Add("App_Data");
        Dirs.Add("App_GlobalResources");
        Dirs.Add("Bin");
        Dirs.Add("cn");
        Dirs.Add("config");
        Dirs.Add("Editor");
        Dirs.Add("label");
        Dirs.Add("member");
        Dirs.Add("mobile");
        Dirs.Add("Payment");
        Dirs.Add("res");
        Dirs.Add("Shop");
        Dirs.Add("uploadfiles");
        Dirs.Add("UserControl");
        Dirs.Add("whir_system");

        Files.Add("404.aspx");
        Files.Add("404.aspx.cs");
        Files.Add("ADClick.aspx");
        Files.Add("ADClick.aspx.cs");
        Files.Add("Global.asax");
        Files.Add("web.config");

        #endregion

        string config = Whir.Framework.FileUtil.Instance.ReadFile(Server.MapPath("~/Config/SystemFiles.config"));
        var doc = new XmlDocument();
        doc.XmlResolver = null;
        doc.LoadXml(config);
        XmlNodeList folders = doc.SelectNodes("root/folders/folder");
        XmlNodeList filenodes = doc.SelectNodes("root/files/file");
        Dirs = GetList(Dirs, folders);
        Files = GetList(Files, filenodes);
        listFiles.DataSource = Files;
        listFiles.DataBind();
        listFolders.DataSource = Dirs;
        listFolders.DataBind();
    }

    protected void btnClearFiles_Click(object sender, EventArgs e)
    {
        try
        {
            LoadConfig();
            var rootDir = new DirectoryInfo(Server.MapPath("~/"));
            foreach (DirectoryInfo dir in rootDir.GetDirectories())
            {
                if (!Dirs.Contains(dir.Name))
                {
                    Delete(dir.FullName);
                }
            }
            foreach (FileInfo fileInfo in rootDir.GetFiles())
            {
                if (!Files.Contains(fileInfo.Name))
                {
                    File.Delete(fileInfo.FullName);
                }
            }
            const string sql = "DELETE FROM Whir_Cnt_CreateLog";
            DbHelper.CurrentDb.Execute(sql);
            Alert("生成文件已清理");
        }
        catch (Exception ex)
        {
            Alert("异常：" + ex.Message);
        }
    }

    private List<string> GetList(List<string> list, XmlNodeList nodes)
    {
        foreach (XmlNode node in nodes)
        {
            var name = node.InnerText.Trim();
            if (!list.Contains(name))
            {
                list.Add(name);
            }
        }
        return list;
    }

    private void Delete(string strFolder)
    {
        //删除目录下的所有文件  
        string[] listFiles = Directory.GetFiles(strFolder);
        if (listFiles.Length > 0)
        {
            for (int index = 0; index < listFiles.Length; ++index)
            {
                var fi = new FileInfo(listFiles[index]);
                fi.Attributes = FileAttributes.Normal; //防止有只读文件无法删除  
                File.Delete(listFiles[index]);
            }
        }

        //删除此目录下的所有目录,即递归向下删  
        string[] listFolders = Directory.GetDirectories(strFolder);
        if (listFolders.Length > 0)
        {
            for (int index = 0; index < listFolders.Length; ++index)
            {
                Delete(listFolders[index]);
            }
        }

        //最后删除当前目录  
        Directory.Delete(strFolder);
    }
}