using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;

using System;
using Whir.Framework;
using Shop.Service;
using Shop.Domain;
using Whir.ezEIP.Web;
using Whir.Domain;
using Whir.Language;

public partial class Whir_System_Handler_Plugin_shop_SearchForm : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 搜选项值列表
    /// </summary>
    public void GetSearchValueList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("413"), true);
        int total = 0;
        string searchId = Whir.ezEIP.BasePage.RequestString("searchId");
        IList<ShopSearchValue> list = ShopSearchValueService.Instance.Query<ShopSearchValue>("SELECT a.* FROM Whir_Shop_SearchValue a WHERE a.IsDel=0 AND a.SearchID=@0 Order By a.Sort Desc,a.Createdate Desc", searchId).ToList();
        string data = list.ToJson();
        total = list.Count.ToInt();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 获取列表数据
    /// </summary>
    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("413"), true);
        string searchName = RequestUtil.Instance.GetFormString("SearchName");
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int total = 0;
        string sql = "";
        List<object> param = new List<object>();
        if (searchName.IsEmpty())
        {
            sql = "SELECT a.* FROM Whir_Shop_Search a WHERE a.IsDel=0 Order By a.Sort Desc,a.Createdate Desc";
           
        }
        else
        {
            sql = "SELECT a.* FROM Whir_Shop_Search a WHERE a.IsDel=0 AND SearchName LIKE '%'+@0+'%' Order By a.Sort Desc,a.Createdate Desc";
            param.Add(searchName);
        }
        //获取规格组分页列表
        var list = ShopSearchService.Instance.Page(pageIndex, pageSize, sql, param.ToArray());
        string data = list.Items.ToJson();
        total = list.TotalItems.ToInt();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }
 
    /// <summary>
    /// 添加编辑搜选项组
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveSearchEdit()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("413"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int SearchID = RequestUtil.Instance.GetFormString("SearchID").ToInt(0);
        string SearchName = RequestUtil.Instance.GetFormString("SearchName");
        try
        {
            if (SearchID != 0)
            {
                ShopSearch ModelUpdate = ShopSearchService.Instance.SingleOrDefault<ShopSearch>(SearchID);
                if (ModelUpdate != null)
                {
                    ModelUpdate.SearchName = SearchName.Trim();
                    ShopSearchService.Instance.Update(ModelUpdate);
                }
            }
            else
            {
                ShopSearch Model = ModelFactory<ShopSearch>.Insten();
                Model.SearchName = SearchName.Trim();
                ShopSearchService.Instance.Save(Model);
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败".ToLang() + ex.Message };
        }
    }

    /// <summary>
    /// 添加编辑搜选项值
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveSearchValueEdit()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("413"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int SearchID=RequestUtil.Instance.GetFormString("SearchID").ToInt(0);
        int SearchValueID=RequestUtil.Instance.GetFormString("SearchValueID").ToInt(0);
        string SearchValueName=RequestUtil.Instance.GetFormString("SearchValueName");
        try
        {
            if (SearchValueID != 0)
            {
                ShopSearchValue ModelUpdate = ShopSearchValueService.Instance.SingleOrDefault<ShopSearchValue>(SearchValueID);
                if (ModelUpdate != null)
                {
                    ModelUpdate.SearchValueName = SearchValueName.Trim();
                    ShopSearchValueService.Instance.Update(ModelUpdate);
                }
            }
            else
            {
                ShopSearchValue Model = ModelFactory<ShopSearchValue>.Insten();

                Model.SearchID = SearchID;
                Model.SearchValueName = SearchValueName.Trim();

                ShopSearchValueService.Instance.Save(Model);
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败".ToLang() + ex.Message };
        }
    }
    /// <summary>
    /// 删除搜选项组
    /// </summary>
    /// <returns></returns>
    public HandlerResult DelSearch()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("413"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //规格组ID
        int searchid = RequestUtil.Instance.GetFormString("searchid").ToInt(0);
        //先删除对应的规格值
        ShopSearchValueService.Instance.Delete("Where SearchID=@0", searchid);
        int result = ShopSearchService.Instance.Delete<ShopSearch>(searchid);
        if (result > 0)
        {
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        else
        {
            return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
        }
    }

    /// <summary>
    /// 删除搜选项值
    /// </summary>
    /// <returns></returns>
    public HandlerResult DelSearchValue()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("413"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //删除时需要判断有没有商品关联此规格，需完善
        //.....................
        //搜选项值ID
        string[] ids = RequestUtil.Instance.GetFormString("ids").Split('|');
        if (ids.Length < 2) { return new HandlerResult { Status = false, Message = "操作失败：".ToLang() }; }

        int searchValueId = ids[0].ToInt();
        int result = ShopSearchValueService.Instance.Delete<ShopSearchValue>(searchValueId);
        if (result > 0)
        {
             return new HandlerResult { Status = true, Message = SysPath + "Plugin/shop/product/search/searchlist.aspx?searchid=" + ids[1] };
        }
        else
        {
             return new HandlerResult { Status = false, Message = "false".ToLang() };
        }
    }
}