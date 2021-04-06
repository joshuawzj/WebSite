/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：generatelabel.aspx.cs
* 文件描述：异步获取当前需要跳转的页面。 
*/

using System;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;

public partial class whir_system_ajax_developer_generatelabel : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了

        int id = WebUtil.Instance.GetQueryInt("id", 0);
        string showselectid = WebUtil.Instance.GetQueryString("showselectid");
        if (!IsPostBack)
        {
            GenerateLabel model = ServiceFactory.GenerateLabelService.GetModelById(id);
            if (model == null)
            {
                Response.Write("error");
            }
            else
            {
                string url = model.LabelName.ToLower() + ".aspx?time=" + DateTime.Now.Millisecond + "&id=" + id + "&showselectid=" + showselectid;
                Response.Write(url);
            }
        }
    }
}