using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Config;
using Whir.Language;
using System.Text;
using Whir.Security.Service;
using Whir.Security.Domain;


public partial class Whir_System_Handler_Security_Roles : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
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
        int roleId = RequestUtil.Instance.GetFormString("RoleId").ToInt(0);
        string roleName = RequestUtil.Instance.GetFormString("RoleName");
        string remarks = RequestUtil.Instance.GetFormString("Remarks");
        int parentId = RequestUtil.Instance.GetFormString("ParentId").ToInt(0);
        var handlerResult = new HandlerResult();
        if (roleId > 0)
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("326"));
        }
        else
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("324"));
        }
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        if (roleId == 1) //角色为root，parentId强制为0
            parentId = 0;

        var listRole = Whir.Security.ServiceFactory.RolesService.GetList(CurrentUser.RolesId).ToList();
        listRole.Add(CurrentUser.RoleInfo);  //包括自身
        if (!SysManagePageBase.IsDevUser && !listRole.Exists(p => p.RoleId == parentId))
        {
            Whir.Service.ServiceFactory.OperateLogService.Save(string.Format("管理员【{0}】修改【{1}】的信息时候，疑似越权操作，被阻止操作", CurrentUser.LoginName, roleName));
            return new HandlerResult { Status = false, Message = "操作失败，疑似越权操作".ToLang() };
        }

        if (roleId <= 0)
        {
            #region 添加
            try
            {
                if (Whir.Security.ServiceFactory.RolesService.IsRoleExists(roleName))
                {
                    return new HandlerResult { Status = false, Message = "该角色名称已存在".ToLang() };
                }
                else
                {
                    var model = ModelFactory<Roles>.Insten();
                    model.Remarks = remarks;
                    model.RoleName = roleName;
                    model.ParentId = parentId == 0 ? model.ParentId : parentId;//parentId为0，则不改变原来的parentId
                    if (model.ParentId > 0)
                    {
                        var parent = Whir.Security.ServiceFactory.RolesService.SingleOrDefault<Roles>(model.ParentId);
                        if (parent != null)
                        {
                            model.Depth = parent.Depth + 1;
                        }
                    }
                    else
                    {
                        model.Depth = 0;
                    }
                    //var parentModel = Whir.Security.ServiceFactory.RolesService.SingleOrDefault<Roles>(model.ParentId) ?? new Roles();
                    //if (parentModel.RoleId > 0)
                    //{
                    //    model.MenuJurisdiction = parentModel.MenuJurisdiction;
                    //    model.ColumnJurisdiction = parentModel.ColumnJurisdiction;
                    //    model.SubjectColumnJurisdiction = parentModel.SubjectColumnJurisdiction;
                    //    model.SubSiteColumnJurisdiction = parentModel.SubSiteColumnJurisdiction;
                    //    model.WorkFlowJurisdiction = parentModel.WorkFlowJurisdiction;
                    //    model.CategoryJurisdiction = parentModel.CategoryJurisdiction;
                    //}
                    Whir.Security.ServiceFactory.RolesService.Save(model);
                    Whir.Service.ServiceFactory.OperateLogService.Save(string.Format("添加角色{0}", roleName));
                    return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
                }
            }
            catch (Exception ex)
            {
                return new HandlerResult { Status = false, Message = "添加失败".ToLang() };
            }
            #endregion
        }
        else
        {
            #region 修改
            try
            {
                if (Whir.Security.ServiceFactory.RolesService.IsRoleExists(roleName, roleId))
                {

                    return new HandlerResult { Status = false, Message = "该角色名称已存在".ToLang() };
                }
                else
                {
                    var model = Whir.Security.ServiceFactory.RolesService.SingleOrDefault<Roles>(roleId);
                    if (model != null)
                    {
                        model.Remarks = remarks;
                        model.RoleName = roleName;
                        model.ParentId = parentId == 0 ? model.ParentId : parentId; //parentId为0，则不改变原来的parentId
                        if (model.ParentId > 0)
                        {
                            var parent = Whir.Security.ServiceFactory.RolesService.SingleOrDefault<Roles>(model.ParentId);
                            if (parent != null)
                            {
                                model.Depth = parent.Depth + 1;
                            }
                        }
                        else
                        {
                            model.Depth = 0;
                        }

                        Whir.Security.ServiceFactory.RolesService.Update(model);
                        ServiceFactory.OperateLogService.Save(string.Format("修改角色名称{0}", roleName));
                        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

                    }
                    else
                    {
                        return new HandlerResult { Status = false, Message = "找不到记录".ToLang() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new HandlerResult { Status = false, Message = "修改失败".ToLang() };

            }
            #endregion
        }
    }


    /// <summary>
    /// 删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult Delete()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("327"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int roleId = RequestUtil.Instance.GetFormString("RoleId").ToInt(0);
        if (Whir.Security.ServiceFactory.RolesService.GetListByParentId(roleId).Count > 0)
            return new HandlerResult { Status = false, Message = "该角色下面存在子角色，不能删除".ToLang() };
        else
        {

            int result = Whir.Security.ServiceFactory.RolesService.Delete<Roles>(roleId);
            if (result == 1)
            {
                Whir.Security.ServiceFactory.UsersService.ClearRole(roleId);

                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
            else
            {
                return new HandlerResult { Status = false, Message = "删除失败".ToLang() };
            }
        }
    }



}