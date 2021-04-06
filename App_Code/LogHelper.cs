using System;
using System.IO;
using System.Web;

/// <summary>
/// LogHelper 的摘要说明
/// </summary>
public class LogHelper
{
    public LogHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public static void Log(Exception ex)
    {
        try
        {
            string fileDir = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/protected/logs/{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));
          
            FileInfo logFileInfo = new FileInfo(fileDir);
            if (!Directory.Exists(logFileInfo.DirectoryName))
            {
                Directory.CreateDirectory(logFileInfo.DirectoryName);
            }

            using (StreamWriter sw = File.AppendText(fileDir))
            {
                string logStr = string.Format("{0} # {1}\r\n{2}\r\n", DateTime.Now, ex.Message + (ex.InnerException == null ? "" : " -- " + ex.InnerException.Message), ex.StackTrace);
                sw.WriteLine(logStr);
                sw.Flush();
                sw.Close();
            }
        }
        catch
        { }
    }

    public static void Log(string msg)
    {
        try
        {
            string fileDir = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/protected/logs/{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));
            FileInfo logFileInfo = new FileInfo(fileDir);
            if (!Directory.Exists(logFileInfo.DirectoryName))
            {
                Directory.CreateDirectory(logFileInfo.DirectoryName);
            }
            using (StreamWriter sw = File.AppendText(fileDir))
            {
                string logStr = string.Format("{0} # {1}\r\n", DateTime.Now, msg);
                sw.WriteLine(logStr);
                sw.Flush();
                sw.Close();
            }
        }
        catch
        { }
    }

    public static void Log(string path, Exception ex)
    {
        try
        {
            string fileDir = path;
            FileInfo logFileInfo = new FileInfo(fileDir);
            if (!Directory.Exists(logFileInfo.DirectoryName))
            {
                Directory.CreateDirectory(logFileInfo.DirectoryName);
            }

            using (StreamWriter sw = File.AppendText(fileDir))
            {
                string logStr = string.Format("{0} # {1}\r\n{2}\r\n", DateTime.Now, ex.Message + (ex.InnerException == null ? "" : " -- " + ex.InnerException.Message), ex.StackTrace);
                sw.WriteLine(logStr);
                sw.Flush();
                sw.Close();
            }
        }
        catch
        { }
    }

    public static void Log(string path, string msg)
    {
        try
        {
            string fileDir = path;
            FileInfo logFileInfo = new FileInfo(fileDir);
            if (!Directory.Exists(logFileInfo.DirectoryName))
            {
                Directory.CreateDirectory(logFileInfo.DirectoryName);
            }
            using (StreamWriter sw = File.AppendText(fileDir))
            {
                string logStr = string.Format("{0} # {1}\r\n", DateTime.Now, msg);
                sw.WriteLine(logStr);
                sw.Flush();
                sw.Close();
            }
        }
        catch
        { }
    }
}