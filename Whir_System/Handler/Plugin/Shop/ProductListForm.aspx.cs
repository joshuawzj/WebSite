using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System;
using Whir.Framework;
using Shop.Service;
using Shop.Domain;
using System.Text;
using System.IO;
using System.Data;
using Whir.ezEIP.Web;
using Whir.Language;
using Whir.Domain;
using System.Xml;
using Whir.Repository;
using System.Text.RegularExpressions;
public partial class Whir_System_Handler_Plugin_order_ProductListForm : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }
    /// <summary>
    /// 获取列表数据
    /// </summary>
    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("410"), true);
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int total = 0;

        var list = GetShopProInfoList(pageIndex, pageSize);
        string data = list.Items.ToJson();
        total = list.TotalItems.ToInt();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);

    }

    private Page<ShopProInfo> GetShopProInfoList(int pageIndex, int pageSize)
    {
        string sql = "SELECT P.*,C.CategoryName FROM Whir_Shop_ProInfo P LEFT JOIN Whir_Shop_Category C ON P.CategoryID=C.CategoryID ";
        #region 搜索条件
        string where = "";
        where = " WHERE 1=1 ";
        int i = 0;
        var parms = new List<object>();

        string filter = RequestUtil.Instance.GetQueryString("filter");
        Dictionary<string, string> searchDic = ToDictionary(filter);
        foreach (var kv in searchDic)
        {
            if (kv.Value.Contains("<&&<"))
            {
                where += " and {0} between @{1} and @{2} ".FormatWith(kv.Key, i++, i++);
                parms.Add(Regex.Split(kv.Value, "<&&<")[0]);
                parms.Add(Regex.Split(kv.Value, "<&&<")[1]);
            }
            else if (kv.Key.ToLower().Contains("categoryname"))
            {
                where += " and p.CategoryId = @{0} ".FormatWith(i++);
                parms.Add(kv.Value);
            }
            else if (kv.Key.ToLower().Contains("name"))
            {
                where += " and {0} like '%'+@{1}+'%' ".FormatWith(kv.Key, i++);
                parms.Add(kv.Value);
            }
            else
            {
                where += " and {0} = @{1} ".FormatWith(kv.Key, i++);
                parms.Add(kv.Value);
            }
        }


        if (!string.IsNullOrEmpty(Whir.ezEIP.BasePage.RequestString("isdel")))
        {
            where += " AND P.IsDel=1 ";
        }
        else
        {
            where += " AND P.IsDel=0 ";
        }
        #endregion
        sql = sql + where.ToString() + " order by P.Sort DESC, P.ProID desc ";

        return ShopProInfoService.Instance.Page(pageIndex, pageSize, sql, parms.ToArray());
    }

    private Dictionary<string, string> ToDictionary(string str)
    {
        try
        {
            if (str.IsEmpty())
                return new Dictionary<string, string>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<Dictionary<string, string>>(str);
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

    /// <summary>
    /// 过滤关键字
    /// </summary>
    /// <param name="keyWord">要过滤的字符串</param>
    /// <returns></returns>
    private string FilterHotWord(string keyWord)
    {
        if (keyWord.Contains("@"))
        {
            keyWord = keyWord.Replace("@", "@@");
        }
        return keyWord.Replace("'", "''").Trim();
    }
    /// <summary>
    /// 所属事件
    /// </summary>
    public HandlerResult SearchClick()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("410"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        StringBuilder urlParam = new StringBuilder();
        string searchProName = RequestUtil.Instance.GetFormString("SearchProName");
        if (!string.IsNullOrEmpty(searchProName.Trim()))
        {
            urlParam.Append("&ProName=" + Server.UrlEncode(searchProName.Trim()));
        }
        string selecCategoryId = RequestUtil.Instance.GetFormString("selecCategory");
        if (selecCategoryId.ToInt(0) > 0)
        {
            urlParam.Append("&CategoryID=" + selecCategoryId.ToInt());
        }
        string searchCostAmount1 = RequestUtil.Instance.GetFormString("SearchCostAmount1");
        if (searchCostAmount1.ToDecimal() > 0.00m)
        {
            urlParam.Append("&CostAmount1=" + searchCostAmount1.ToDecimal());
        }
        string searchCostAmount2 = RequestUtil.Instance.GetFormString("SearchCostAmount2");
        if (searchCostAmount2.ToDecimal() > 0.00m)
        {
            urlParam.Append("&CostAmount2=" + searchCostAmount2.ToDecimal());
        }
        string cbIsAllowBuy1 = RequestUtil.Instance.GetFormString("cbIsAllowBuy1");
        string cbIsAllowBuy2 = RequestUtil.Instance.GetFormString("cbIsAllowBuy2");
        //两个选一时才归入条件查询
        if (!string.IsNullOrEmpty(cbIsAllowBuy1))
        {
            urlParam.Append("&IsAllowBuy=" + cbIsAllowBuy1);
        }
        if (!string.IsNullOrEmpty(cbIsAllowBuy2))
        {
            urlParam.Append("&IsAllowBuy=" + cbIsAllowBuy2);
        }
        if (!string.IsNullOrEmpty(RequestUtil.Instance.GetString("isdel")))
        {
            urlParam.Append("&isdel=" + RequestUtil.Instance.GetString("isdel"));
        }
        return new HandlerResult { Status = true, Message = SysPath + "Plugin/shop/product/productlist.aspx?op=select" + urlParam.ToStr() };
        //Response.Redirect(SysPath + "Plugin/shop/product/productlist.aspx?op=select" + UrlParam.ToStr());
    }

    /// <summary>
    /// 导出excel
    /// </summary>
    /// <returns></returns>
    public void ExportClick()
    {
        try
        {
            SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("410"), true);
            IList<ShopProInfo> splist = GetShopProInfoList(1, int.MaxValue).Items;
            string[] columnName = new string[] { "ProNO", "ProName", "CategoryName", "CostAmount", "ProDesc", "IsAllowBuy".ToLang() };
            string[] excelName = new string[] { "商品编号", "商品名称", "商品分类", "价格", "详细描述", "是否可购买".ToLang() };
            //excel存放文件夹
            if (!File.Exists(Server.MapPath(UploadFilePath + "/ProExcel")))
            {
                Directory.CreateDirectory(Server.MapPath(UploadFilePath + "/ProExcel"));

            }
            //保存到服务器
            ShopProInfoService.Instance.ExportExcel<ShopProInfo>(splist, Server.MapPath(UploadFilePath + "/ProExcel/商品列表"), columnName, excelName);

            ////通下流读写把文件下载到本地
            //if (!FileUtil.Instance.DownFile("商品列表.xls", Server.MapPath(UploadFilePath + "/ProExcel/商品列表.xls")))
            //{
            //    return new HandlerResult { Status = false, Message = "下载文件出错".ToLang() };
            //}
            Response.Clear();
            Response.Write(UploadFilePath + "ProExcel/商品列表.xls");
            //return   UploadFilePath + "/ProExcel/商品列表.xls";
        }
        catch (Exception ex)
        {
            Response.Clear();
            Response.Write("");
        }
    }

    /// <summary>
    /// 保存和新增商品基本信息
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveProduct()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("410"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int proId = RequestUtil.Instance.GetFormString("ProID").ToInt(0);
        string attrIDs = RequestUtil.Instance.GetFormString("attrIDs").TrimEnd(',');
        string proName = RequestUtil.Instance.GetFormString("ProName");
        string proNo = RequestUtil.Instance.GetFormString("ProNo");
        int categoryId = RequestUtil.Instance.GetFormString("CategoryID").ToInt(0);
        int isAllowBuy = RequestUtil.Instance.GetFormString("IsAllowBuy").ToInt(0);
        decimal costAmount = RequestUtil.Instance.GetFormString("CostAmount").ToDecimal();
        string images = RequestUtil.Instance.GetFormString("Images").Trim('*');//商品多图
        bool isChangeAttr = false;
        ShopProInfo pro;
        if (proId > 0)
        {
            pro = ShopProInfoService.Instance.GetShopProById(proId);
            if (pro != null && pro.AttrIDs != attrIDs)
            {
                isChangeAttr = true;
            }
        }
        else
        {
            pro = ModelFactory<ShopProInfo>.Insten();
        }
        if (pro != null)
        {
            pro.ProName = proName;
            pro.ProNO = proNo;
            pro.CategoryID = categoryId;
            pro.IsAllowBuy = isAllowBuy == 1;
            pro.CostAmount = costAmount;
            if (costAmount > 999999.999m)
            {
                return new HandlerResult { Status = false, Message = "金额数字过大，请输入小于999999.999的金额！".ToLang() };
            }
            if (!attrIDs.IsEmpty())
            {
                pro.AttrIDs = attrIDs;
            }
            else
            {
                pro.AttrIDs = "";
            }
            if (isChangeAttr && proId > 0)//规格值改变时删除原有规格商品
            {
                ShopAttrProService.Instance.DeleteShopAttrProByProID(proId);
            }


            pro.Images = images;
            pro.ProImg = images.Length > 0 ? images.Split('*')[0] : "";
            if (string.IsNullOrEmpty(pro.Images))
            {
                pro.ProImg = "";
            }
            if (proId > 0)
            {
                if (ShopProInfoService.Instance.Update(pro) > 0)
                {
                    return new HandlerResult { Status = true, Message = proId.ToStr() };
                }
                return new HandlerResult { Status = false, Message = "更新商品基本信息失败".ToLang() };
            }
            int result = ShopProInfoService.Instance.Insert(pro).ToInt();
            if (result > 0)
            {
                return new HandlerResult { Status = true, Message = result.ToStr() };
            }
        }
        return new HandlerResult { Status = false, Message = "新增商品基本信息失败".ToLang() };
    }

    /// <summary>
    /// 保存附加信息
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveExtraProduct()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("410"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int proId = RequestUtil.Instance.GetFormString("ProID").ToInt(0);
        if (proId > 0)
        {
            List<object> parms = new List<object>();
            string proDesc = RequestUtil.Instance.GetFormString("ProDesc");
            string metaTitle = RequestUtil.Instance.GetFormString("MetaTitle");
            string metaKeyword = RequestUtil.Instance.GetFormString("MetaKeyword");
            string metaDescription = RequestUtil.Instance.GetFormString("MetaDescription");
            string sql = " SET MetaTitle=@0,MetaKeyword=@1,MetaDescription=@2,ProDesc=@3";
            parms.AddRange(new List<object>() { metaTitle, metaKeyword, metaDescription, proDesc });
            IList<ShopField> list = ShopFieldService.Instance.GetAllShopFileList();
            foreach (ShopField item in list)
            {
                if (item.IsUsing)
                {
                    string value = RequestUtil.Instance.GetFormString(item.FieldName);
                    sql = sql + "," + item.FieldName + "=@{0}".FormatWith(parms.Count);
                    parms.Add(value);
                }
            }
            sql = sql.Trim(',');
            sql = sql + " WHERE ProID=" + proId;
            ShopProInfoService.Instance.Update<ShopProInfo>(sql,parms.ToArray());
            return new HandlerResult { Status = true, Message = "保存附加信息成功".ToLang() };
        }
        else
        {
            return new HandlerResult { Status = false, Message = "请先保存基本信息".ToLang() };
        }
    }

    /// <summary>
    /// 保存搜选项
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveProductSearchValue()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("410"));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            int proId = RequestUtil.Instance.GetFormString("ProID").ToInt(0);
            string searchValueIDs = RequestUtil.Instance.GetFormString("SearchValueIDs").TrimEnd(',');
            ShopProInfoService.Instance.Update<ShopProInfo>("SET SearchValueIDs=@0 WHERE ProID=@1", searchValueIDs, proId);
            return new HandlerResult { Status = true, Message = "保存搜选项信息成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }

    /// <summary>
    /// 保存规格商品
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveSpecProduct()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("410"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string rqStr = RequestUtil.Instance.GetFormString("ProAttrXml");
        int proId = RequestUtil.Instance.GetFormString("ProID").ToInt(0);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(rqStr);
        XmlNode xn = doc.SelectSingleNode("attrprolist");
        XmlNodeList xnl = xn.ChildNodes;
        string AttrProIDs = "";
        if (xnl.Count > 0)
        {

            foreach (XmlNode item in xnl)
            {
                AttrProIDs += item.ChildNodes[6].InnerText.ToInt() + ",";
            }
            AttrProIDs = AttrProIDs.Trim(',');

        }
        if (!string.IsNullOrEmpty(AttrProIDs))
        {
            ShopAttrProService.Instance.DeleteShopAttrProByProIDNotAttrProids(proId, AttrProIDs);//先删除原有的
        }
        else
        {
            ShopAttrProService.Instance.DeleteShopAttrProByProID(proId);//先删除原有的
        }
        if (xnl.Count > 0)
        {
            foreach (XmlNode item in xnl)
            {
                ShopAttrPro attrpro = new ShopAttrPro();
                attrpro.AttrValueNames = item.ChildNodes[0].InnerText;
                attrpro.AttrValueIDs = ShopAttrValueService.Instance.OrderByIDs(item.ChildNodes[1].InnerText);
                attrpro.CostAmount = item.ChildNodes[2].InnerText.ToDecimal();
                attrpro.IsUseMainImage = item.ChildNodes[3].InnerText.ToBoolean();
                attrpro.Images = item.ChildNodes[4].InnerText;
                attrpro.ProImg = item.ChildNodes[5].InnerText;
                if (string.IsNullOrEmpty(attrpro.ProImg))
                {
                    if (attrpro.Images.Split('*').Length > 0)
                    {
                        attrpro.ProImg = attrpro.Images.Split('*')[0];
                    }
                }
                attrpro.ProID = proId;
                attrpro.AttrProID = item.ChildNodes[6].InnerText.ToInt();
                if (attrpro.AttrProID > 0)
                {
                    ShopAttrProService.Instance.Update(attrpro);
                }
                else
                {
                    ShopAttrProService.Instance.Insert(attrpro);
                }
            }
        }
        return new HandlerResult { Status = true, Message = "保存规格商品信息成功".ToLang() };
        // Alert("保存规格商品信息成功", SysPath + "Plugin/shop/product/product_edit.aspx?proid=" + ProID);
    }

    /// <summary>
    /// 拖拽排序
    /// </summary>
    /// <returns></returns>
    public HandlerResult SortDrag()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("410"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var ids = RequestUtil.Instance.GetFormString("Ids").TrimEnd(',').Split(',').Select(p => p.ToInt()).ToList();


        decimal minSort = ShopProInfoService.Instance.GetMaxSort(ids);
        foreach (var id in ids)
        {
            ShopProInfoService.Instance.ModifyProInfoSort(id, minSort - ids.IndexOf(id));
        }

        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

    }

    /// <summary>
    /// 批量排序
    /// </summary>
    /// <returns></returns>
    public HandlerResult BatchSort()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("410"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string strSort = RequestUtil.Instance.GetFormString("strSort");
        if (!string.IsNullOrEmpty(strSort))
        {
            string[] arrSort = strSort.Split(',');
            foreach (string str in arrSort)
            {
                int columnID = str.Split('|')[0].ToInt();
                long sort = str.Split('|')[1].ToLong(0);
                ShopProInfoService.Instance.ModifyProInfoSort(columnID, sort);
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        else
        {
            return new HandlerResult { Status = false, Message = "操作失败！".ToLang() };
        }
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult BatchDel()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("410"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string ids = RequestUtil.Instance.GetFormString("ids");
        string[] arr = ids.Split(',');
        if (arr.Length > 0)
        {
            foreach (string item in arr)
            {
                if (!string.IsNullOrEmpty(RequestUtil.Instance.GetString("isdel")))
                {
                    //删除规格从表信息
                    ShopAttrProService.Instance.DeleteShopAttrProByProID(item.ToInt());
                    //删除主表信息
                    ShopProInfoService.Instance.Delete<ShopProInfo>(item.ToInt());
                }
                else
                {
                    ShopProInfoService.Instance.DeleteShopProById(item.ToInt(), 1);
                }
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        else
        {
            return new HandlerResult { Status = false, Message = "操作失败！".ToLang() };
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult Del()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("410"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string id = RequestUtil.Instance.GetFormString("id");
        if (!string.IsNullOrEmpty(RequestUtil.Instance.GetString("isdel")))
        {
            //删除规格从表信息
            ShopAttrProService.Instance.DeleteShopAttrProByProID(id.ToInt());
            //删除主表信息
            ShopProInfoService.Instance.Delete<ShopProInfo>(id.ToInt());
        }
        else
        {
            ShopProInfoService.Instance.DeleteShopProById(id.ToInt(), 1);
        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }


    /// <summary>
    /// 批量还原
    /// </summary>
    /// <returns></returns>
    public HandlerResult BatchReduction()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("410"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string ids = RequestUtil.Instance.GetFormString("ids");
        if (!string.IsNullOrEmpty(ids))
        {
            string[] arr = ids.Split(',');
            foreach (string ProID in arr)
            {
                ShopProInfoService.Instance.DeleteShopProById(ProID.ToInt(), 0);

            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        else
        {
            return new HandlerResult { Status = false, Message = "操作失败！".ToLang() };
        }
    }
}