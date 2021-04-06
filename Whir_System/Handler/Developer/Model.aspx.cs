using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class whir_system_Handler_Developer_Model : SysHandlerPageBase
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
        int modelId = RequestUtil.Instance.GetFormString("ModelId").ToInt();

        //反射获取表单字段数据
        var type = typeof(Model);
        var model = ServiceFactory.MenuService.SingleOrDefault<Model>(modelId) ?? ModelFactory<Model>.Insten();
        model = GetPostObject(type, model) as Model;
        if (model.ModelId == 0)
            model.TableName = "Whir_U_" + model.TableName;

        if (model.ModelId == 0)
        {
            //判断表名是否存在
            if (ServiceFactory.DbService.IsExsitTable(model.TableName))
            {
                return new HandlerResult { Status = false, Message = "表名{0}已存在".FormatWith(model.TableName) };

            }

            //1. Whir_Dev_Model中添加数据
            //2. 创建表
            //3. 在表中创建初始的列
            ServiceFactory.ModelService.AddModel(model);
        }
        else
        {
            ServiceFactory.ModelService.Update(model);
        }


        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
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
        int modelId = RequestUtil.Instance.GetFormString("ModelId").ToInt();

        //删除
        Model model = ServiceFactory.ModelService.SingleOrDefault<Model>(modelId);
        if (model != null)
        {
            //1. 删除Whir_Dev_Field表中记录的字段信息
            //2. 删除表
            //3. 删除Whir_Dev_Model表中保存的模型信息
            try
            {
                ServiceFactory.ModelService.DeleteModel(model);
            }
            catch (Exception ex)
            {
                return new HandlerResult { Status = false, Message = "操作失败：" + ex.Message };

            }

        }

        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
    }

    /// <summary>
    /// 生成实体
    /// </summary>
    /// <returns></returns>
    public HandlerResult Entity()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int modelId = RequestUtil.Instance.GetFormString("ModelId").ToInt();

        //创建实体
        bool result = ServiceFactory.ModelService.CreateModelEntity(modelId);
        if (!result)
            return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
        else
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }
}