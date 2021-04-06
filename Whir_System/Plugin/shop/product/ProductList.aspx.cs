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
using Whir.Language;

public partial class whir_system_Plugin_shop_product_productlist : Whir.ezEIP.Web.SysManagePageBase
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
    /// 商品分类下拉Json
    /// </summary>
    public string CategoryNameJson { get;set;}
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("410"));
            bindOption();//绑定上级类目
        }
    }

    //绑定上级类目   
    private void bindOption()
    {
        List<DataRow> list = ShopCategoryService.Instance.GetAllCategoryList(0);
        DataTable OptionTable= new DataTable();
        if (list.Count > 0)
        {
            OptionTable = list.CopyToDataTable();
        }
        CategoryNameJson = OptionTable.ToBootsTrapTableFilterJson("CategoryID", "CategoryName");
    }
    protected void lbnExport_Click(object sender, EventArgs e)
    {
        string SQL = "";
        IList<ShopProInfo> splist = ShopProInfoService.Instance.Query<ShopProInfo>(SQL).ToList();
        string[] columnName = new string[] { "ProNO", "ProName", "CategoryName", "CostAmount", "ProDesc", "IsAllowBuy" };
        string[] excelName = new string[] { "商品编号".ToLang(), "商品名称".ToLang(), "商品分类".ToLang(), "价格".ToLang(), "详细描述".ToLang(), "是否可购买".ToLang() };
        //excel存放文件夹
        if (!File.Exists(Server.MapPath(UploadFilePath + "/ProExcel")))
        {
            Directory.CreateDirectory(Server.MapPath(UploadFilePath + "/ProExcel"));

        }
        //保存到服务器
        ShopProInfoService.Instance.ExportExcel<ShopProInfo>(splist,Server.MapPath( UploadFilePath + "/ProExcel/商品列表"), columnName, excelName);
        //通下流读写把文件下载到本地
        if (!FileUtil.Instance.DownFile("{0}.xls".FormatWith("商品列表".ToLang()), Server.MapPath(UploadFilePath + "/ProExcel/{0}.xls".FormatWith("商品列表".ToLang()))))
        {
            Alert("下载文件出错！".ToLang());
        }
    }
   
}