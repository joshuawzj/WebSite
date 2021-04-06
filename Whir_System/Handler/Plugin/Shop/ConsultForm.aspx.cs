using System;
using System.Web.UI.WebControls;

using Whir.Framework;
using Shop.Service;
using Shop.Domain;
using Whir.ezEIP.Web;
using Whir.Security.Service;
using Whir.Language;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Whir.Repository;
public partial class Whir_System_Handler_Plugin_shop_ConsultForm : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }
    /// <summary>
    /// 获取商品咨询列表
    /// </summary>
    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("414"), true);
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int total = 0;
        var list = GetList(pageIndex, pageSize);
        string data = list.Items.ToJson();
        total = list.TotalItems.ToInt();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 获取sql语句
    /// </summary>
    /// <returns></returns>
    private Page<ShopConsult> GetList(int pageIndex, int pageSize)
    {
        //string ProNameKey = Whir.ezEIP.BasePage.RequestString("ProNameKey");
        //string ConsultKey = Whir.ezEIP.BasePage.RequestString("ConsultKey");
        //string ConsultUserKey = Whir.ezEIP.BasePage.RequestString("ConsultUserKey");
        string State = Whir.ezEIP.BasePage.RequestString("State");
        string sql = "SELECT c.*, m.LoginName, p.ProName, p.CostAmount, p.ProImg "
                         + "FROM Whir_Shop_Consult c INNER JOIN Whir_Mem_Member m ON c.MemberID = m.Whir_Mem_Member_PID "
                         + "INNER JOIN Whir_Shop_ProInfo p ON c.ProID = p.ProID WHERE c.IsDel=0 {0} Order By c.Sort Desc,c.Createdate Desc";

        #region 搜索条件
        string where = "";
        int i = 0;
        var parms = new List<object>();

        string filter = RequestUtil.Instance.GetQueryString("filter");
        Dictionary<string, string> searchDic = ToDictionary(filter);
        foreach (var kv in searchDic)
        {
            if (kv.Value.Contains("<&&<"))
            {
                where += " and c.{0} between @{1} and @{2} ".FormatWith(kv.Key, i++, i++);
                parms.Add(Regex.Split(kv.Value, "<&&<")[0]);
                parms.Add(Regex.Split(kv.Value, "<&&<")[1]);
            }
            else if (kv.Key.ToLower().Contains("name") || kv.Key.ToLower().Contains("consult"))
            {
                where += " and {0} like '%'+@{1}+'%' ".FormatWith(kv.Key, i++);
                parms.Add(kv.Value);
            }
            else if (kv.Key.ToLower().Contains("state"))
            {
                if (kv.Value == "1")//审核
                {
                    where += " AND c.State={0} ".FormatWith(kv.Value);
                }
                else  
                {
                    where += " AND c.State=0 ";
                }
            }
            else
            {
                where += " and {0} = @{1} ".FormatWith(kv.Key, i++);
                parms.Add(kv.Value);
            }
        }



        #endregion

        sql = sql.FormatWith(where);
        return ShopConsultService.Instance.Page(pageIndex, pageSize, sql, parms.ToArray());

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
    /// 审核回复
    /// </summary>
    /// <returns></returns>
    public HandlerResult Reply()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("414"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int consultId = RequestUtil.Instance.GetFormString("consultId").ToInt(0);
        string reptyContent = RequestUtil.Instance.GetFormString("reptyContent");
        int repty = RequestUtil.Instance.GetFormString("repty").ToInt(0);
        try
        {
            if (reptyContent != null)
            {
                ShopConsult Model = ShopConsultService.Instance.SingleOrDefault<ShopConsult>(consultId);
                if (Model != null)
                {
                    Model.Reply = reptyContent;
                    Model.ReplyDate = DateTime.Now;
                    Model.ReplyUser = AuthenticateHelper.UserName;
                    Model.State = repty;
                    ShopConsultService.Instance.Save(Model);
                }
            }
            return new HandlerResult { Status = true, Message = "操作成功！".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败！".ToLang() + ex.Message };
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult Del()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("414"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int consultId = RequestUtil.Instance.GetFormString("consultId").ToInt(0);
        try
        {
            ShopConsultService.Instance.Delete<ShopConsult>(consultId);
            return new HandlerResult { Status = true, Message = "操作成功！".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败！".ToLang() + ex.Message };
        }
    }
    /// <summary>
    /// 批量删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult BatchDel()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("414"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string ids = RequestUtil.Instance.GetFormString("ids");
        try
        {
            foreach (string id in ids.Split(','))
            {
                ShopConsultService.Instance.Delete<ShopConsult>(id.ToInt());
            }
            return new HandlerResult { Status = true, Message = "操作成功！".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败！".ToLang() + ex.Message };
        }
    }
}