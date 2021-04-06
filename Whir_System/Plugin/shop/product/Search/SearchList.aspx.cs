/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：whir_system_Plugin_shop_product_Search_Searchlist.cs
 * 文件描述：商品规格组、规格值操作类
 * 
 * 创建标识: liuyong 2013-01-30
 * 
 * 修改标识：
 */
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;

using System;
using Whir.Framework;
using Shop.Service;
using Shop.Domain;

public partial class whir_system_Plugin_shop_product_search_searchlist : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 搜索关键字
    /// </summary>
    private string SearchKey { get; set; }
    /// <summary>
    /// 编辑、删除时返回的ID
    /// </summary>
    protected string SearchID { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("413"));
    }
    /// <summary>
    /// 排序操作
    /// </summary>
    protected void Operate_Command(object sender, CommandEventArgs e)
    {
        //string cmd = e.CommandName;
        //if (cmd == "sort")
        //{
        //    string attrIdSort = hidSort.Value.Trim(',');//主键ID与Sort键值对字符串
        //    var idSorts = attrIdSort.Split(',');//主键ID与Sort主键对
        //    foreach (var s in idSorts)
        //    {
        //        //ID与Sort
        //        var idSort = s.Split('|');
        //        //更新排序
        //        long sort = 0;
        //        if (idSort.Length < 2 || !long.TryParse(idSort[1], out sort))
        //        {
        //            continue;
        //        }
        //        ShopSearchService.Instance.ModifySort(idSort[0].ToInt(), idSort[1].ToInt64());
        //    }
        //    Alert("排序成功", true);
     //   }
    }
}