using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

using Whir.Framework;
using Shop.Domain;
using Shop.Service;
using Whir.Service;
using Whir.Domain;
using Whir.Language;
using Whir.ezEIP.Web;

public partial class Whir_System_Handler_Plugin_shop_AttrForm : SysHandlerPageBase
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
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("412"), true);
        string searchKey = RequestUtil.Instance.GetFormString("SearchKey");
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int total = 0;
        string sql = "";
        List<object> param = new List<object>();
        if (searchKey.IsEmpty())
        {
            sql = "SELECT a.* FROM Whir_Shop_Attr a WHERE a.IsDel=0 Order By a.Sort Desc,a.Createdate Desc";
        }
        else
        {                                                                                                                                              
            sql = "SELECT a.* FROM Whir_Shop_Attr a WHERE a.IsDel=0 AND SearchName LIKE '%'+@0+'%' Order By a.Sort Desc,a.Createdate Desc";
            param.Add(searchKey);
        }
        //获取规格组分页列表
        var list = ShopAttrService.Instance.Page(pageIndex, pageSize, sql, param.ToArray());
        string data = list.Items.ToJson();
        total = list.TotalItems.ToInt();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 规格值列表
    /// </summary>
    public void GetAttrValueList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("412"), true);
        int total = 0;
        string attrId = Whir.ezEIP.BasePage.RequestString("attrId");
        List<ShopAttrValue> list = ShopAttrValueService.Instance.Query<ShopAttrValue>("SELECT a.* FROM Whir_Shop_AttrValue a WHERE a.IsDel=0 AND a.AttrID=@0 Order By a.Sort Desc,a.Createdate Desc", attrId).ToList();
        string data = list.ToJson();
        total = list.Count.ToInt();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);

    }
    /// <summary>
    /// 加载树形规格组数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult GetAttrTree()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("412"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string rootFolder = "规格组名称"; //当前文件、文件夹路径
        string searchKey = RequestUtil.Instance.GetFormString("searchKey");
        string folderTree = "[{text:'";
        folderTree += rootFolder + "',href:'/',nodes:[";

        folderTree += GetTree(Server.MapPath(rootFolder), searchKey) + "]}]";

        return new HandlerResult { Status = true, Message = folderTree };
    }

    /// <summary>
    /// 递归树形结构
    /// </summary>
    /// <returns></returns>
    public string GetTree(string path, string searchKey)
    {
        string tree = "";
        List<ShopAttr> list;
        if (searchKey.IsEmpty())
        {
            list = ShopAttrService.Instance.Query<ShopAttr>("SELECT a.* FROM Whir_Shop_Attr a WHERE a.IsDel=0 Order By a.Sort Desc,a.Createdate Desc").ToList();
        }
        else
        {
            list = ShopAttrService.Instance.Query<ShopAttr>("SELECT a.* FROM Whir_Shop_Attr a WHERE a.IsDel=0 AND SearchName LIKE '%{0}%' Order By a.Sort Desc,a.Createdate Desc".FormatWith(searchKey)).ToList();
        }
        if (list.Count > 0)
        {
            foreach (ShopAttr item in list)
            {
                string href = item.AttrID.ToStr();
                tree += "{text: '" + item.SearchName + "',href:'" + href + "'},";
            }
            tree = tree.Substring(0, tree.Length - 1);
        }
        return tree;
    }
    /// <summary>
    /// 编辑保存
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveAttrEdit()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("412"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int AttrID = RequestUtil.Instance.GetFormString("AttrID").ToInt(0);
        string SearchName = RequestUtil.Instance.GetFormString("SearchName");
        bool IsShowImage = RequestUtil.Instance.GetFormString("ShowImage").ToBoolean();
        try
        {
            if (AttrID > 0)
            {
                ShopAttr ModelUpdate = ShopAttrService.Instance.SingleOrDefault<ShopAttr>(AttrID);
                if (ModelUpdate != null)
                {
                    ModelUpdate.SearchName = SearchName;
                    ModelUpdate.IsShowImage = IsShowImage;
                    ShopAttrService.Instance.Update(ModelUpdate);
                }
            }
            else
            {
                ShopAttr Model = ModelFactory<ShopAttr>.Insten();
                Model.SearchName = SearchName.Trim();
                Model.IsShowImage = IsShowImage;
                ShopAttrService.Instance.Save(Model);
            }
            return new HandlerResult { Status = true, Message = "操作成功！".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败！".ToLang() + ex.Message };
        }
    }

    /// <summary>
    /// 编辑规格值
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveAttrValueEdit()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("412"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int AttrID = RequestUtil.Instance.GetFormString("AttrID").ToInt(0);
        int AttrValueID = RequestUtil.Instance.GetFormString("AttrValueID").ToInt(0);
        string AttrValueName = RequestUtil.Instance.GetFormString("AttrValueName");
        string ShowImage = RequestUtil.Instance.GetFormString("ShowImage");
        try
        {
            if (AttrValueID != 0)
            {
                ShopAttrValue ModelUpdate = ShopAttrValueService.Instance.SingleOrDefault<ShopAttrValue>(AttrValueID);
                if (ModelUpdate != null)
                {
                    ShopAttrProService.Instance.UpdateAttrNames(AttrValueID, AttrValueName.Trim());
                    ModelUpdate.AttrValueName = AttrValueName.Trim();
                    ModelUpdate.ShowImage = ShowImage;
                    ShopAttrValueService.Instance.Update(ModelUpdate);
                }
            }
            else
            {
                ShopAttrValue Model = ModelFactory<ShopAttrValue>.Insten();

                Model.AttrID = AttrID;
                Model.AttrValueName = AttrValueName.Trim();
                Model.ShowImage = ShowImage;

                ShopAttrValueService.Instance.Save(Model);
            }
            return new HandlerResult { Status = true, Message = "操作成功！".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败！".ToLang() + ex.Message };
        }
    }
    /// <summary>
    /// 删除规格值
    /// </summary>
    /// <returns></returns>
    public HandlerResult DelAttrValue()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("412"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string[] ids = RequestUtil.Instance.GetFormString("ids").Split('|');
        if (ids.Length < 2) { return new HandlerResult { Status = false, Message = "操作失败：".ToLang() }; }
        int attrValueId = ids[0].ToInt();
        if (ShopAttrValueService.Instance.AttrValueIsExistPro(attrValueId))//删除时需要判断有没有商品关联此规格
        {
            return new HandlerResult { Status = false, Message = "该规格值已被商品引用，不可删除!".ToLang() };
        }
        else
        {
            int result = ShopAttrValueService.Instance.Delete<ShopAttrValue>(attrValueId);
            if (result > 0)
            {
                return new HandlerResult { Status = true, Message = SysPath + "Plugin/shop/product/attr/attrlist.aspx?attrid=" + ids[1] };
            }
            else
            {
                return new HandlerResult { Status = false, Message = "操作失败！".ToLang() };
            }
        }
    }
    /// <summary>
    /// 删除规格组
    /// </summary>
    /// <returns></returns>
    public HandlerResult DelAttr()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("412"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //规格组ID
        int attrId = RequestUtil.Instance.GetFormString("attrId").ToInt(0);

        //删除时需要判断有没有商品关联此规格，需完善
        //.....................
        IList<ShopAttrValue> tempList = ShopAttrValueService.Instance.GetAttrValueByAttrID(attrId);
        if (tempList.Count > 0)
        {
            foreach (ShopAttrValue sav in tempList)
            {
                if (ShopAttrValueService.Instance.AttrValueIsExistPro(sav.AttrValueID))//删除时需要判断有没有商品关联此规格
                {
                    return new HandlerResult { Status = false, Message = "该规格组中规格值(" + sav.AttrValueName + ")已被商品引用，不可删除!".ToLang() };
                }
            }
        }
        //先删除对应的规格值
        ShopAttrValueService.Instance.Delete("Where AttrID=@0", attrId);
        int result = ShopAttrService.Instance.Delete<ShopAttr>(attrId);
        if (result > 0)
        {
            return new HandlerResult { Status = true, Message = "操作成功！".ToLang() };
        }
        else
        {
            return new HandlerResult { Status = false, Message = "操作失败！".ToLang() };
        }
    }
}