/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：whir_system_Plugin_shop_product_productlist.cs
 * 文件描述：商品列表操作类
 * 
 * 创建标识: yangwb
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
using System.Text;
using System.IO;
using System.Data;

public partial class whir_system_Plugin_shop_product_Recycle : Whir.ezEIP.Web.SysManagePageBase
{

    /// <summary>
    /// 商品列表
    /// </summary>
    public ShopProInfo ShopProInfo { get; set; }
    /// <summary>
    /// 开始价格
    /// </summary>
    public string CostAmount1 { get; set; }
    /// <summary>
    /// 结束价格
    /// </summary>
    public string CostAmount2 { get; set; }
    /// <summary>
    /// 商品分类下拉
    /// </summary>
    public DataTable OptionTable{get;set;}
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindOption();//绑定上级类目
        }
    }

    //绑定上级类目   
    private void bindOption()
    {
        List<DataRow> list = ShopCategoryService.Instance.GetAllCategoryList(0);
        if (list.Count == 0)
        {
            OptionTable = new DataTable();
        }
        else
        {
            OptionTable = list.CopyToDataTable();
        }
    }
    protected void lbnExport_Click(object sender, EventArgs e)
    {
        string SQL = "";
        IList<ShopProInfo> splist = ShopProInfoService.Instance.Query<ShopProInfo>(SQL).ToList();
        string[] columnName = new string[] { "ProNO", "ProName", "CategoryName", "CostAmount", "ProDesc", "IsAllowBuy" };
        string[] excelName = new string[] { "商品编号", "商品名称", "商品分类", "价格", "详细描述", "是否可购买" };
        //excel存放文件夹
        if (!File.Exists(Server.MapPath(UploadFilePath + "/ProExcel")))
        {
            Directory.CreateDirectory(Server.MapPath(UploadFilePath + "/ProExcel"));

        }
        //保存到服务器
        ShopProInfoService.Instance.ExportExcel<ShopProInfo>(splist,Server.MapPath( UploadFilePath + "/ProExcel/商品列表"), columnName, excelName);
        //通下流读写把文件下载到本地
        if (!FileUtil.Instance.DownFile("商品列表.xls", Server.MapPath(UploadFilePath + "/ProExcel/商品列表.xls")))
        {
            Alert("下载文件出错！");
        }
    }
   
}