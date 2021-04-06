using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;
using Shop.Service;
using Shop.Domain;
using System.Text;
using Whir.Service;

public partial class Shop_productlist : Shop.Common.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindSearchValue();
            BindChooseSearchValue();
            bindProList();
            BindCategoryLocation();
        }
    }

    //绑定搜选项
    private void BindSearchValue()
    {
        StringBuilder searchHtml = new StringBuilder();
        string SQL = @"SELECT * FROM Whir_Shop_Search WHERE IsDel=0 AND (SELECT COUNT(*) FROM Whir_Shop_SearchValue WHERE Whir_Shop_SearchValue.SearchID=Whir_Shop_Search.SearchID)>0  ORDER BY sort DESC,createdate DESC";
        List<ShopSearch> list = ShopSearchService.Instance.Query<ShopSearch>(SQL).ToList();
        foreach (ShopSearch ss in list)
        {
            searchHtml.Append("<dl>");

            searchHtml.Append("<dd>" + ss.SearchName + "：</dd>");

            searchHtml.Append("<dt>");
            IList<ShopSearchValue> ssvlist = ShopSearchValueService.Instance.GetSearchValueBySearchID(ss.SearchID);
            foreach (ShopSearchValue ssv in ssvlist)
            {
                searchHtml.Append("<a svid=\"" + ssv.SearchValueID + "\" style=\"cursor: pointer;\">" + ssv.SearchValueName + "</a> ");
            }
            searchHtml.Append("</dt>");

            searchHtml.Append("</dl>");
        }
        ltSearchValue.Text = searchHtml.ToString();
    }

    //绑定已选中的搜选项
    private void BindChooseSearchValue()
    {
        string SearchValues = RequestUtil.Instance.GetString("svids").Replace("'", "");
        SearchValues = SearchValues.Trim(',');
        if (!string.IsNullOrEmpty(SearchValues))
        {
            StringBuilder searchHtml = new StringBuilder();
            SearchValues = Server.UrlDecode(SearchValues);
            string SQL = "SELECT S.SearchName,SV.SearchValueID,SV.SearchValueName FROM Whir_Shop_SearchValue SV LEFT JOIN Whir_Shop_Search S ON SV.SearchID=S.SearchID WHERE SV.SearchValueID IN (" + SearchValues + ")";
            List<ShopSearchValue> ssvlist = ShopSearchValueService.Instance.Query<ShopSearchValue>(SQL).ToList();
            foreach (ShopSearchValue ssv in ssvlist)
            {
                searchHtml.Append("<a  svid=\"" + ssv.SearchValueID + "\" style=\"cursor:pointer;\">" + ssv.SearchName + "：" + ssv.SearchValueName + "&nbsp;&nbsp;×</a>");
            }
            ltChooseSearchValue.Text = searchHtml.ToString();
        }
    }

    //绑定商品列表
    private void bindProList()
    {
        #region 排序
        string OrderBy = "";
        string ob = RequestUtil.Instance.GetString("ob");
        string price = RequestUtil.Instance.GetString("price");
        string orderCount = RequestUtil.Instance.GetString("oc");
        if (!string.IsNullOrEmpty(ob))
        {
            if (!string.IsNullOrEmpty(price) || !string.IsNullOrEmpty(orderCount))
            {
                OrderBy = " ORDER BY ";
            }
            if (ob == "price" && !string.IsNullOrEmpty(price))
            {
                OrderBy += " p.CostAmount " + (price == "asc" ? "ASC," : "DESC,");
                if (!string.IsNullOrEmpty(orderCount))
                {
                    OrderBy += " p.saleCount " + (price == "asc" ? "ASC," : "DESC,");
                }
            }
            else
            {
                if (ob == "oc" && !string.IsNullOrEmpty(orderCount))
                {
                    OrderBy += " p.saleCount " + (orderCount == "asc" ? "ASC," : "DESC,");
                    if (!string.IsNullOrEmpty(price))
                    {
                        OrderBy += " p.CostAmount " + (price == "asc" ? "ASC," : "DESC,");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(orderCount))
                    {
                        OrderBy += " p.saleCount " + (price == "asc" ? "ASC," : "DESC,");
                    }
                }
            }
            OrderBy = OrderBy.Trim(',');
        }
        else
        {
            OrderBy = " ORDER BY p.ProID DESC ";
        }
        #endregion

        #region 条件
        string where = "";
        //分类
        int cid = RequestUtil.Instance.GetQueryInt("categoryid", 0);
        if (cid > 0)
        {
            string charindex = "";
            switch (Whir.Service.CurrentDbType.CurDbType)
            {
                case EnumType.DbType.SqlServer:
                    charindex = "charindex(','+cast(" + cid.ToStr() + " as varchar(20))+',',','+ParentPath+',')>0) )";
                    break;
                case EnumType.DbType.Oracle:
                    charindex = "INSTR" + "(','||ParentPath||',',','||cast(" + cid.ToStr() + " as varchar(20))||',')>0) )";
                    break;
                case EnumType.DbType.MySql:
                    charindex = "INSTR" + "(CONCAT(',',ParentPath,','),CONCAT(',',cast(" + cid.ToStr() + " as char),','))>0) )";
                    break;
                default:
                    charindex = "charindex" + "(','+cast(" + cid.ToStr() + " as varchar(20))+',',','+ParentPath+',')>0) )";
                    break;
            }
            //包括下级的商品也呈现，如果不需要，则去掉括号里的or条件
            where += " AND (CategoryID=" + cid.ToString() + " OR CategoryID IN (select CategoryID from Whir_Shop_Category where " + charindex;
        }
        //搜选项
        string SearchValues = RequestUtil.Instance.GetString("svids").Replace("'", "");
        if (!string.IsNullOrEmpty(SearchValues))
        {
            SearchValues = Server.UrlDecode(SearchValues);
            string[] searchArr = SearchValues.Split(',');
            string charindex = "";
            switch (Whir.Service.CurrentDbType.CurDbType)
            {
                case EnumType.DbType.SqlServer:
                    foreach (string s in searchArr)
                    {
                        charindex += " AND charindex" + "(','+'" + s + "'+',',','+SearchValueIDs+',')>0 ";
                    }
                    break;
                case EnumType.DbType.Oracle:
                    foreach (string s in searchArr)
                    {
                        charindex += " AND INSTR" + "(','||SearchValueIDs||',',','||'" + s + "'||',')>0 ";
                    }
                    break;
                case EnumType.DbType.MySql:
                    foreach (string s in searchArr)
                    {
                        charindex += " AND INSTR" + "(CONCAT(',',SearchValueIDs,','),CONCAT(',','" + s + "',','))>0 ";
                    }
                    break;
                default:
                    foreach (string s in searchArr)
                    {
                        charindex += " AND charindex" + "(','+'" + s + "'+',',','+SearchValueIDs+',')>0 ";
                    }
                    break;
            }
            where += charindex;
        }
        //搜选项
        #endregion
        string SQL = @"SELECT p.* FROM (SELECT Whir_Shop_ProInfo.*," + (CurrentUseDbType == EnumType.DbType.SqlServer ? "ISNULL" : CurrentUseDbType == EnumType.DbType.MySql ? "IFNULL" : "nvl") + @"((SELECT SUM(OP.Count) FROM  Whir_Shop_OrderInfo O LEFT JOIN Whir_Shop_OrderProduct OP
                       ON O.OrderID=OP.OrderID WHERE OP.ProID=Whir_Shop_ProInfo.ProID AND O.Status=0),0) AS saleCount,"
                         + (CurrentUseDbType == EnumType.DbType.SqlServer ? "ISNULL" : CurrentUseDbType == EnumType.DbType.MySql ? "IFNULL" : "nvl") +
                        "((select min(ap.costamount) from whir_shop_attrpro ap where ap.proid=Whir_Shop_ProInfo.ProID),0) as AttrMinPrice,"
        + " (select count(atp.attrproid) from whir_shop_attrpro atp where atp.proid=Whir_Shop_ProInfo.ProID) as AttrCount FROM Whir_Shop_ProInfo) p WHERE p.IsDel=0  " + where + OrderBy;

        var list = ShopProInfoService.Instance.Page(pager1.PageIndex, pager1.PageSize, SQL);
        pager1.RecordsTotal = list.TotalItems.ToInt();
        ltAllProCount.Text = list.TotalItems.ToString();
        ltPageTip.Text = pager1.PageIndex.ToString() + "/" + ((list.TotalItems.ToInt() % pager1.PageSize == 0) ? (list.TotalItems.ToInt() / pager1.PageSize) : ((list.TotalItems.ToInt() / pager1.PageSize) + 1)).ToString();
        rptProList.DataSource = list.Items;
        rptProList.DataBind();
        if (rptProList.Items.Count == 0)
            ltPageTip.Text = "0/0";
    }

    /// <summary>
    /// 绑定当前类别位置
    /// </summary>
    private void BindCategoryLocation()
    {
        int cid = RequestUtil.Instance.GetQueryInt("categoryid", 0);
        if (cid > 0)
        {
            ltCategory.Text = "";
            IList<ShopCategory> scList = ShopCategoryService.Instance.GetCategoryListByCategoryID(cid);
            if (scList.Count > 0)
            {
                for (int i = 0; i < scList.Count; i++)
                {
                    ltCategory.Text += "<a href=\"productlist.aspx?categoryid=" + scList[i].CategoryID + "\">" + scList[i].CategoryName + "</a>";
                    if (i != scList.Count - 1)
                    {
                        ltCategory.Text += "&nbsp;>&nbsp;";
                    }
                }
            }
        }
        else
        {
            ltCategory.Text = "<i>商品列表页</i>";
        }
    }
}