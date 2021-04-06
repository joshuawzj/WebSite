/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：getcolumn.aspx.cs
* 文件描述：异步获取当前站点的栏目的页面。 
*/

using System;
using System.Collections.Generic;
using System.Text;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Repository;
using Whir.Language;

public partial class whir_system_ajax_common_getcolumn : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了

            int siteId = WebUtil.Instance.GetQueryInt("siteid", 0);
            IList<Column> list = ServiceFactory.ColumnService.GetList(0, siteId,0,0);
 
            StringBuilder sb = new StringBuilder("<option selected='selected' value=''>{0}</option>".FormatWith("==请选择==".ToLang()));

            foreach (Column col in list)
            {
                
                sb.Append("<option value='" + col.ColumnId + "'>" + col.ColumnName + "</option>");
            }
            Response.Write(sb.ToStr());
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
}