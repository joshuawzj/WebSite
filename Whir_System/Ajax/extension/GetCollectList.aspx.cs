using System;
using System.Collections.Generic;
using System.Linq;

using Whir.Framework;
using Whir.Service;
using Whir.Domain;
using System.Web.Script.Serialization;
using Whir.ezEIP.Web;

public partial class whir_system_ajax_extension_GetCollectList : System.Web.UI.Page
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("354"), true);
        int collectId = RequestUtil.Instance.GetQueryInt("collectid", 0);

        List<TitleUrl> list = new List<TitleUrl>();
        var model = ServiceFactory.CollectService.SingleOrDefault<Collect>(collectId);
        if (model != null)
        {
            if (model.PageRules.ToLower().Contains("{$page}"))//使用分页采集规则
            {
                int count = model.PageNum < 1 ? 1 : model.PageNum;
                int index = 1;
                while (count >= index)
                {
                    var url = model.PageRules.Replace("{$page}", index.ToStr());

                    var pageItems = GetDataList(HttpOperater.GetHtml(url), model, url);
                    list.AddRange(pageItems);
                    index++;
                }
            }
            else
            {
                var pageItems = GetDataList(HttpOperater.GetHtml(model.WebUrl), model, model.WebUrl);
                list.AddRange(pageItems);
            }
            if (!model.IsOrderDesc)
            {
                List<TitleUrl> listTemp = new List<TitleUrl>();
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    listTemp.Add(list[i]);
                }
                list = listTemp;
            }
        }

        JavaScriptSerializer jss = new JavaScriptSerializer();
        Response.Write(jss.Serialize(list));
    }

    private List<TitleUrl> GetDataList(string pageCode, Collect model, string url)
    {
        List<TitleUrl> list = new List<TitleUrl>();

        int listStartIndex = pageCode.IndexOf(model.ListStartCode.Replace("\r",""));
        if (listStartIndex != -1)//以下获取列表代码
        {
            pageCode = pageCode.Substring(listStartIndex + model.ListStartCode.Replace("\r", "").Length);
            int listEndIndex = pageCode.IndexOf(model.ListEndCode.Replace("\r", ""));
            if (listEndIndex != -1)
            {
                pageCode = pageCode.Substring(0, listEndIndex);
            }
        }
        if (!pageCode.IsEmpty())
        {
            //链接代码块
            string sbLinkCode = pageCode;
            //标题代码块
            string sbTitleCode = pageCode;
            var exsit = true;
            while (exsit)
            {
                TitleUrl t = new TitleUrl();
                //链接
                var linkStartIndex = sbLinkCode.IndexOf(model.LinkStartCode.Replace("\r", ""));
                if (linkStartIndex != -1)
                {
                    linkStartIndex += model.LinkStartCode.Replace("\r", "").Length; //加上自己的长度
                    var linkExt = sbLinkCode.Substring(linkStartIndex);
                    var linkEndIndex = linkExt.IndexOf(model.LinkEndCode.Replace("\r", ""));
                    if (linkEndIndex != -1)
                    {
                        t.Url = HttpOperater.GetReallyUrl(linkExt.Substring(0, linkEndIndex), url);
                        sbLinkCode = linkExt.Substring(linkEndIndex + model.LinkEndCode.Replace("\r", "").Length);//获取链接
                    }
                    else
                    {
                        exsit = false;
                    }
                }
                else
                {
                    exsit = false;
                }
                //标题
                var titleStartIndex = sbTitleCode.IndexOf(model.TitleStartCode.Replace("\r", ""));
                if (titleStartIndex != -1)
                {
                    titleStartIndex += model.TitleStartCode.Replace("\r", "").Length; //加上自己的长度
                    var titleExt = sbTitleCode.Substring(titleStartIndex);
                    var titleEndIndex = titleExt.IndexOf(model.TitleEndCode.Replace("\r", ""));
                    if (titleEndIndex != -1)
                    {
                        t.Title = titleExt.Substring(0, titleEndIndex);
                        sbTitleCode = titleExt.Substring(titleEndIndex + model.TitleEndCode.Replace("\r", "").Length);//获取链接
                    }
                    else
                    {
                        exsit = false;
                    }
                }
                else
                {
                    exsit = false;
                }
                if (exsit)
                {
                    list.Add(t);
                }
            }
        }
        return list;
    }

}

public class TitleUrl
{
    /// <summary>
    /// 详细地址
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
}