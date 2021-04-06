using System;
using System.Web.UI.WebControls;

using Shop.Domain;
using Shop.Service;
using Whir.Framework;
using Whir.ezEIP.Web;
using Whir.Domain;
using Whir.Language;
public partial class Whir_System_Handler_Plugin_order_CourierForm : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult Del()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("409"));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            int courierId = RequestUtil.Instance.GetFormString("submitId").ToInt(0);
            //先删除对应的规格值
            ShopCourierService.Instance.Delete("Where CourierID=@0", courierId);
            ShopCourierService.Instance.Delete<ShopCourier>(courierId);
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }
    /// <summary>
    /// 获取列表数据
    /// </summary>
    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("409"), true);
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int total = 0;
        string SQL = "Where IsDel=0 Order By Sort Desc,Createdate Desc";
        var list = ShopCourierService.Instance.Page(pageIndex, pageSize, SQL);
        string data = list.Items.ToJson();
        total = list.TotalItems.ToInt();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }
    /// <summary>
    /// 保存配送方式
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveCourier()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("419"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int CourierID = RequestUtil.Instance.GetFormString("CourierID").ToInt(0);
        string CourierName = RequestUtil.Instance.GetFormString("CourierName");
        string Com = RequestUtil.Instance.GetFormString("Com");
        try
        {
            if (CourierID == 0)
            {
                ShopCourier Model = ModelFactory<ShopCourier>.Insten();
                Model.CourierName = CourierName;
                Model.Com = Com;
                ShopCourierService.Instance.Save(Model);
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
            else
            {
                ShopCourier Model = ShopCourierService.Instance.SingleOrDefault<ShopCourier>(CourierID);
                Model.CourierName = CourierName.Trim();
                Model.Com = Com.Trim();
                ShopCourierService.Instance.Update(Model);
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }
}