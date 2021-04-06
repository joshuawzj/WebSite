using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class whir_system_Handler_Developer_Field : SysHandlerPageBase
{
    SysManagePageBase SysManagePageBase = new SysManagePageBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetFormString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int fieldId = RequestUtil.Instance.GetFormString("FieldId").ToInt();

        //反射获取表单字段数据
        var type = typeof(Field);
        var Field = ServiceFactory.MenuService.SingleOrDefault<Field>(fieldId) ?? new Field();
        Field = GetPostObject(type, Field) as Field;

        if (Field.FieldId == 0)
        {

            ServiceFactory.FieldService.AddField(Field, Field.ModelId);
        }
        else
        {
            ServiceFactory.FieldService.Update(Field);
        }


        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Update()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int fieldId = RequestUtil.Instance.GetFormString("FieldId").ToInt();
        bool isWorkFlowTitle = RequestUtil.Instance.GetFormString("IsWorkFlowTitle").ToBoolean();
        //反射获取表单字段数据

        /* 只能设置一个通用标题
        var Field = ServiceFactory.MenuService.SingleOrDefault<Field>(fieldId);
        if (Field != null)
        {
            Field.IsWorkFlowTitle = isWorkFlowTitle;
            ServiceFactory.FieldService.Update(Field);
            return new HandlerResult { Status = true, Message = "保存成功".ToLang()};
        }*/
        var field = ServiceFactory.MenuService.SingleOrDefault<Field>(fieldId);
        if (field != null)
        {
            if (ServiceFactory.FieldService.SetWorkFlowField(fieldId, field.ModelId))
            {
                return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
            }
        }
        return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Del()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int FieldId = RequestUtil.Instance.GetFormString("FieldId").ToInt();

        //删除
        Field Field = ServiceFactory.FieldService.SingleOrDefault<Field>(FieldId);
        Model Model = ServiceFactory.ModelService.SingleOrDefault<Model>(Field.ModelId);
        if (Field != null && Model != null)
        {
            try
            {
                ServiceFactory.FieldService.DeleteField(Model, Field);
            }
            catch (Exception ex)
            {
                return new HandlerResult { Status = false, Message = "操作失败：" + ex.Message };

            }

        }

        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
    }
}
