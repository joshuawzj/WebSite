using System;
using Whir.Framework;
using System.Text.RegularExpressions;
using Whir.Repository;

/// <summary>
///FrontHelper 的摘要说明
/// </summary>
public class FrontHelper
{
	public FrontHelper()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public static string GetExternalLink(object linkurl,int ColumnID)
    {
        string url = linkurl.ToStr();
        if (url.IsEmpty())
        { return Whir.Service.ServiceFactory.ColumnService.GetColumnListLink(ColumnID, true, 0); }
        Regex reg = new Regex("^(http|https|ftp)://.*",RegexOptions.IgnoreCase);
        if (reg.IsMatch(url))
            return url;
        else
           return (WebUtil.Instance.AppPath() + url.Replace("\\", "/")).Replace("//","/");
       // return url;
    }

    public static string GetExternalLink(object ColumnID)
    {
      
        string url = DbHelper.CurrentDb.ExecuteScalar<string>("Select LocationUrl From Whir_Dev_Column Where ColumnID=@0",ColumnID.ToInt());
        if (url.IsEmpty())
        { return Whir.Service.ServiceFactory.ColumnService.GetColumnListLink(ColumnID.ToInt(), true, 0); }
        Regex reg = new Regex("^(http|https|ftp)://.*", RegexOptions.IgnoreCase);
        if (reg.IsMatch(url))
            return url;
        else
            return (WebUtil.Instance.AppPath() + url.Replace("\\", "/")).Replace("//", "/");
        // return url;
    }


    public static int GetRootParentID(object columnid)
    {
        int result = 0;
        if (columnid.ToInt() > 0)
        {
            int ParentID = DbHelper.CurrentDb.ExecuteScalar<int>("Select Top 1 ParentID From Whir_Dev_Column Where ColumnID=@0",columnid.ToInt());
            if (ParentID > 0)
            {
                result = GetRootParentID(ParentID);
            }
            else
            {
                result = columnid.ToInt();
            }
        }
        return result;
    }


}