using System;
//非系统的引用
using Shop.Domain;
using Whir.Framework;

using Whir.Service;
using Whir.ezEIP.Web;
using Whir.Domain;
using Whir.Language;

public partial class Whir_System_Handler_Plugin_shop_FieldForm : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }
    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("415"), true);
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int total = 0;
        var list = ShopFieldService.Instance.Page(pageIndex, pageSize, "SELECT * FROM dbo.Whir_Shop_Field WHERE IsDel=0 ORDER BY Sort Desc,Createdate Desc");
        string data = list.Items.ToJson();
        total = list.TotalItems.ToInt();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

    /// <summary>
    /// 保存编辑操作
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("415"));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            int FieldID = RequestUtil.Instance.GetFormString("FieldID").ToInt(0);
            string FieldName = RequestUtil.Instance.GetFormString("FieldName");
            string FieldType = RequestUtil.Instance.GetFormString("FieldType");
            bool IsAllowNull = RequestUtil.Instance.GetFormString("IsAllowNull").ToBoolean();
            string DefaultValue = RequestUtil.Instance.GetFormString("DefaultValue");
            string FieldAlias = RequestUtil.Instance.GetFormString("FieldAlias");
            int ShowType = RequestUtil.Instance.GetFormString("ShowType").ToInt(1);
            int RepeatColumn = RequestUtil.Instance.GetFormString("RepeatColumn").ToInt(0);
            int BindType = RequestUtil.Instance.GetFormString("BindType").ToInt(1);
            string ValidateType = RequestUtil.Instance.GetFormString("ValidateType");
            string ValidateExpression = RequestUtil.Instance.GetFormString("ValidateExpression");
            //判断字段名是否存在
            if (ShopFieldService.Instance.IsExistByFieldName(FieldID, FieldName.Trim()))
            {
                return new HandlerResult { Status = false, Message = "字段名称已经存在".ToLang() };
            }
            #region 保存自定义商品属性数据
            ShopField sf = new ShopField();
            if (FieldID > 0)
            {
                sf = ShopFieldService.Instance.GetShopFileById(FieldID);
            }
            else
            {
                sf = ModelFactory<ShopField>.Insten();
            }
            sf.FieldName = FieldName.Trim();
            sf.FieldType = "nvarchar";
            sf.IsAllowNull = IsAllowNull;
            sf.DefaultValue = DefaultValue;

            sf.FieldAlias = FieldAlias;
            sf.ShowType = ShowType;
            switch (ShowType)
            {
                case 2://单选
                    sf.RepeatColumn = RepeatColumn;//每行显示条数--显示        
                    break;
                case 3://多选
                    sf.RepeatColumn = RepeatColumn;//每行显示条数--显示
                    break;
                default://其它
                    break;
            }
            sf.BindType = BindType;
            switch (BindType)
            {
                case 1://绑定文本

                    sf.BindText = RequestUtil.Instance.GetFormString("BindText").Trim();

                    break;
                case 2://绑定SQL
                    sf.BindSql = RequestUtil.Instance.GetFormString("BindSQL").Trim(); ;
                    sf.BindTextField = RequestUtil.Instance.GetFormString("BindTextField").Trim();
                    sf.BindValueField = RequestUtil.Instance.GetFormString("BindValueField").Trim();

                    break;
                case 3://绑定多级类别
                    sf.BindTable = RequestUtil.Instance.GetFormString("BindTable").Trim();
                    sf.BindKeyID = RequestUtil.Instance.GetFormString("BindKeyId").Trim().ToInt(0);
                    sf.BindValueField = RequestUtil.Instance.GetFormString("BindValueField").Trim();
                    sf.BindTextField = RequestUtil.Instance.GetFormString("BindTextField").Trim();

                    break;
                default://其它

                    break;
            }

            sf.ValidateType = ValidateType.ToInt();
            sf.ValidateExpression = ValidateExpression;
            string ValidateText = RequestUtil.Instance.GetFormString("ValidateText");
            sf.ValidateText = ValidateText;
            bool IsUsing = RequestUtil.Instance.GetFormString("IsUsing").ToBoolean();
            sf.IsUsing = IsUsing;

            if (FieldID > 0)
            {
                ShopFieldService.Instance.Update(sf);
            }
            else
            {
                ShopFieldService.Instance.Save(sf);
            }

            #endregion

            if (FieldID == 0)
            {
                #region 更改商品主表列            
                ServiceFactory.DbService.AddColumnToTable("Whir_Shop_ProInfo", sf.FieldName, sf.FieldType, 1024);
                #endregion
            }

            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }

    public HandlerResult Del()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("415"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int FieldID = RequestUtil.Instance.GetFormString("FieldID").ToInt(0);
        bool del = true;
        //如果表已存在该列则删除
        ShopField sf = ShopFieldService.Instance.GetShopFileById(FieldID);
        if (ServiceFactory.DbService.IsExsitColumn("Whir_Shop_ProInfo", sf.FieldName))
        {
            del = del && ServiceFactory.DbService.RemoveColumnFromTable("Whir_Shop_ProInfo", sf.FieldName);

        }
        del = del && ShopFieldService.Instance.Delete<ShopField>(FieldID) > 0;
        if (del)
        {
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        else
        {
            return new HandlerResult { Status = true, Message = "操作失败".ToLang() };
        }
    }

    /// <summary>
    /// 批量排序
    /// </summary>
    /// <returns></returns>
    public HandlerResult BatchSort()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("415"));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            string strSort = RequestUtil.Instance.GetFormString("ids");
            string[] arrSort = strSort.Split(',');
            foreach (string str in arrSort)
            {
                int columnID = str.Split('|')[0].ToInt();
                long sort = str.Split('|')[1].ToLong(0);
                ShopFieldService.Instance.ModifyFiledSort(columnID, sort);
            }
            return new HandlerResult { Status = true, Message = "排序成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult BatchDel()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("415"));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            string selected = RequestUtil.Instance.GetFormString("ids");
            string[] arrSelected = selected.Split(',');
            foreach (string fieldID in arrSelected)
            {
                //如果商品主表已存在该列则删除
                ShopField sf = ShopFieldService.Instance.GetShopFileById(fieldID.ToInt());
                if (ServiceFactory.DbService.IsExsitColumn("Whir_Shop_ProInfo", sf.FieldName))
                {
                    ServiceFactory.DbService.RemoveColumnFromTable("Whir_Shop_ProInfo", sf.FieldName);

                }
                ShopFieldService.Instance.Delete<ShopField>(fieldID.ToInt());
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }
}