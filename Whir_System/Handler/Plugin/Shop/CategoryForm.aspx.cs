using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
//非系统的引用
using Shop.Domain;
using Whir.Framework;
using Whir.ezEIP.Web;
using Whir.Domain;
using Whir.Language;
public partial class Whir_System_Handler_Plugin_order_CategoryForm : SysHandlerPageBase
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
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("411"), true);
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int total = 0;
        List<DataRow> list = ShopCategoryService.Instance.GetAllCategoryList(0);
        string data = list.CopyToDataTable().ToJson();
        total = list.CopyToDataTable().Rows.Count;
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }
    /// <summary>
    /// 添加分類
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveCategory()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("411"));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            int categoryId = RequestUtil.Instance.GetFormString("CategoryID").ToInt(0);
            string categoryName = RequestUtil.Instance.GetFormString("CategoryName");
            string categoryImages = RequestUtil.Instance.GetFormString("CategoryImages");
            int selecCategoryID = RequestUtil.Instance.GetFormString("selecCategory").ToInt(0);
            string metaTitle = RequestUtil.Instance.GetFormString("MetaTitle");
            string metaKeyword = RequestUtil.Instance.GetFormString("MetaKeyword");
            string metaDescription = RequestUtil.Instance.GetFormString("MetaDescription");
            if (ShopCategoryService.Instance.IsExistByCategoryName(categoryId, categoryName.Trim()))
            {
                return new HandlerResult { Status = false, Message = "类别名称已经存在".ToLang() };
            }

            ShopCategory sc = new ShopCategory();
            if (categoryId > 0)
            {
                if (categoryId == selecCategoryID)
                {
                    return new HandlerResult { Status = false, Message = "不能选择本身类别作为上级类别".ToLang() };
                }
                sc = ShopCategoryService.Instance.GetCategoryByID(categoryId);
            }
            else
            {
                sc = ModelFactory<ShopCategory>.Insten();
            }
            sc.CategoryName = categoryName.Trim();
            sc.CategoryImages = categoryImages.Trim();
            sc.ParentID = selecCategoryID;
            sc.ParentPath = sc.ParentID == 0 ? "0" : ShopCategoryService.Instance.GetCategoryByID(sc.ParentID).ParentPath + "," + sc.ParentID;
            sc.MetaTitle = metaTitle;
            sc.MetaKeyword = metaKeyword;
            sc.MetaDescription = metaDescription;

            if (categoryId > 0)
            {
                ShopCategoryService.Instance.Update(sc);
            }
            else
            {
                ShopCategoryService.Instance.Save(sc);
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }

    /// <summary>
    /// 删除类别
    /// </summary>
    /// <returns></returns>
    public HandlerResult DelCategory()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("411"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int categoryID = RequestUtil.Instance.GetFormString("categoryID").ToInt(0);
        //判断是否有下级类别
        if (ShopCategoryService.Instance.IsExistChildCategory(categoryID))
        {
            return new HandlerResult { Status = false, Message = "该类别存在下级类别，请先删除下级类别！".ToLang() };
        }
        //删除类别相关商品信息

        //删除类别信息
        if (ShopCategoryService.Instance.Delete<ShopCategory>(categoryID) > 0)
        {
            return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
        }
        else
        {
            return new HandlerResult { Status = false, Message = "操作失败！".ToLang() };
        }
    }

    /// <summary>
    ///类别排序
    /// </summary>
    /// <returns></returns>
    public HandlerResult SortCategory()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("411"));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            string strSort = RequestUtil.Instance.GetFormString("strSort");
            string[] arrSort = strSort.Split(',');
            foreach (string str in arrSort)
            {
                int columnID = str.Split('|')[0].ToInt();
                long sort = str.Split('|')[1].ToLong(0);
                ShopCategoryService.Instance.ModifyCategorySort(columnID, sort);
            }
            return new HandlerResult { Status = true, Message = "排序成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "排序失败" + ex.Message };
        }
    }
}