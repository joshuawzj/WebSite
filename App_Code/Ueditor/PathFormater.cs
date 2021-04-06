
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Whir.Config;
using Whir.Framework;

/// <summary>
/// PathFormater 的摘要说明
/// </summary>
public static class PathFormatter
{
    public static string Format(string originFileName, string pathFormat)
    {
        Whir.Config.Models.UploadConfig config = ConfigHelper.GetUploadConfig();//上传文件在后台的配置
        string extensionName = FileUtil.Instance.GetExtension(originFileName).ToLower();//上传文件的后缀名
        string strdir = string.Empty;
        //上传文件存放目录，0：按天存放，1：按月存放
        switch (config.DirectoryType)
        {
            case "0":
                strdir = DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/";
                break;
            case "1":
                strdir = DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/";
                break;
        }

        //最终要保存的文件名
        if (config.IsRename)
        {
            switch (config.SaveFileNameType)//上传文件名方式，0：按日期时间存入，1：按随机数存入
            {
                case "0":
                    originFileName = strdir+"{0}.{1}".FormatWith(DateTime.Now.ToString("yyyyMMddHHmmssfff"), extensionName);
                    break;
                case "1":
                    originFileName = strdir + "{0}.{1}".FormatWith(Rand.Instance.Number(17), extensionName);
                    break;
            }
        }


        return WebUtil.Instance.AppPath() + AppSettingUtil.AppSettings["UploadFilePath"]+ originFileName;
    }
}