using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;

public partial class whir_system_wtl_WtlGenerate : Whir.ezEIP.Web.SysManagePageBase
{
    protected List<Column> Columns = new List<Column>();
    protected void Page_Load(object sender, EventArgs e)
    {
        //int siteId = Request.QueryString["siteId"].ToInt(1);

        //Columns = GetColumn(siteId, "", 0) ?? new List<Column>();

    }


    private List<Column> GetColumn(int siteId, string str, int parentId)
    {
        List<Column> list = ServiceFactory.ColumnService.GetList(parentId, siteId).ToList();
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Column column = list[i];
                var str2 = str;
                column.ColumnName = GetDropDownListText(column.ColumnName, list.Count, i, ref str2);
                list.AddRange(GetColumn(siteId, str2, column.ColumnId));
            }
        }
        return list;
    }


    public string GetDropDownListText(string itemName, int listCount, int index, ref string str)
    {
        string val = "";
        if (index == (listCount - 1)) //当前层次最后一个栏目时
        {
            str = str.Replace("├─", "└─");
        }

        val = str + itemName;

        if (str.IsEmpty())
            str = "　├─";
        else
            str = (str.Replace("├─", "│").Replace("└─", "　") + "　├─");

        return val;
    }
}