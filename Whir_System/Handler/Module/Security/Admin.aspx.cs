using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;

using Whir.Security.Domain;


public partial class Whir_System_Handler_Security_Security : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        var handlerResult = new HandlerResult();
        int userId = RequestUtil.Instance.GetFormString("UserId").ToInt(0);

        if (userId > 0)
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("322"));
        }
        else
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("320"));
        }
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //反射获取表单字段数据
        var type = typeof(Users);
        var user = Whir.Security.ServiceFactory.UsersService.SingleOrDefault<Users>(userId) ?? ModelFactory<Users>.Insten();

        var password = user.Password;
        user = GetPostObject(type, user) as Users;
        if (userId <= 0)
        {
            try
            {
                if (user != null && Whir.Security.ServiceFactory.UsersService.IsUserExists(user.LoginName))
                {
                    return new HandlerResult { Status = false, Message = "该用户名已存在".ToLang() };
                }
                if (user != null)
                {
                    if (!user.Password.IsGoodPwd() && AppSettingUtil.GetString("IsStrongPassword").ToBoolean(true))
                        return new HandlerResult { Status = false, Message = "密码必须包含数字、字母、特殊符号，长度在6-20位".ToLang() };

                    var listRole = Whir.Security.ServiceFactory.RolesService.GetList(CurrentUser.RolesId).ToList();
                    listRole.Add(CurrentUser.RoleInfo);  //包括自身
                    if (!SysManagePageBase.IsDevUser && !listRole.Exists(p => p.RoleId == user.RolesId))
                    {
                        Whir.Service.ServiceFactory.OperateLogService.Save(string.Format("管理员【{0}】修改【{1}】的信息时候，疑似越权操作，被阻止操作", CurrentUser.LoginName, user.LoginName));
                        return new HandlerResult { Status = false, Message = "操作失败，疑似越权操作".ToLang() };
                    }

                    user.LastLoginTime = DateTimeExt.MinDateTime;
                    user.SystemLanguage = 1;
                    user.SystemSkin = "Res/Images/Bg/bg6.jpg";
                    user.Password = StrExt.GetSHA1Str(user.Password);
                    user.LastPasswordDate = DateTime.Now;
                    Whir.Security.ServiceFactory.UsersService.Save(user);
                    Whir.Service.ServiceFactory.OperateLogService.Save(string.Format("添加管理员【{0}】", user.LoginName));
                }
            }
            catch (Exception)
            {
                return new HandlerResult { Status = false, Message = "添加失败".ToLang() };
            }
        }
        else
        {
            try
            {
                if (user != null)
                {
                    var listRole = Whir.Security.ServiceFactory.RolesService.GetList(CurrentUser.RolesId).ToList();
                    listRole.Add(CurrentUser.RoleInfo);  //包括自身
                    if (SysManagePageBase.IsDevUser || listRole.Exists(p => p.RoleId == user.RolesId))
                    {
                        if (!user.Password.IsEmpty() && !user.Password.IsGoodPwd() && AppSettingUtil.GetString("IsStrongPassword").ToBoolean(true))
                            return new HandlerResult { Status = false, Message = "密码必须包含数字、字母、特殊符号，长度在6-20位".ToLang() };

                        user.Password = user.Password != "" ? StrExt.GetSHA1Str(user.Password) : password;
                        user.LastPasswordDate = DateTime.Now;
                        Whir.Security.ServiceFactory.UsersService.Update(user);
                        Whir.Service.ServiceFactory.OperateLogService.Save(string.Format("修改管理员【{0}】信息", user.LoginName));
                    }
                    else
                    {
                        Whir.Service.ServiceFactory.OperateLogService.Save(string.Format("管理员【{0}】修改【{1}】的信息时候，疑似越权操作，被阻止操作", CurrentUser.LoginName, user.LoginName));
                        return new HandlerResult { Status = false, Message = "操作失败，疑似越权操作".ToLang() };
                    }
                }
                else
                {
                    return new HandlerResult { Status = false, Message = "找不到记录".ToLang() };
                }
            }
            catch (Exception)
            {
                return new HandlerResult { Status = false, Message = "修改失败".ToLang() };
            }
        }

        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };

    }

    /// <summary>
    /// 禁用
    /// </summary>
    /// <returns></returns>
    public HandlerResult Disabled()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("321"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int userId = RequestUtil.Instance.GetFormString("userid").ToInt(0);
        if (userId.Equals(1))
        {
            return new HandlerResult { Status = false, Message = "该账号不能被禁用".ToLang() };
        }
        var result = Whir.Security.ServiceFactory.UsersService.SetUserState(userId, 1);
        if (result == -1)
        {
            return new HandlerResult { Status = false, Message = "禁用失败,至少需要保留一个非禁用状态的超级管理员账号".ToLang() };
        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    /// <summary>
    /// 启用
    /// </summary>
    /// <returns></returns>
    public HandlerResult Enabled()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("321"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int userId = RequestUtil.Instance.GetFormString("userid").ToInt(0);
        if (SysManagePageBase.IsDevUser || SysManagePageBase.IsSuperUser)
        {
            var user = Whir.Security.ServiceFactory.UsersService.SingleOrDefault<Users>(userId) ?? ModelFactory<Users>.Insten();
            if (user != null && user.ErrorCount >= AppSettingUtil.GetInt32("ErrorCount"))
            {
                user.ErrorCount = 0;
                user.ErrorLastTime = new DateTime(1990, 1, 1);
                user.UpdateUser = CurrentUser.LoginName;
                user.UpdateDate = DateTime.Now;
                Whir.Security.ServiceFactory.UsersService.Update(user);
                Whir.Service.ServiceFactory.OperateLogService.Save(string.Format("管理员{0}解除【{1}】登录限制", CurrentUser.LoginName, user.LoginName));
            }
        }
        Whir.Security.ServiceFactory.UsersService.SetUserState(userId, 0);
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult Delete()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("323"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int userId = RequestUtil.Instance.GetFormString("userid").ToInt(0);
        int result = Whir.Security.ServiceFactory.UsersService.DeleteUser(userId);
        if (result == -1)
        {
            return new HandlerResult { Status = false, Message = "删除失败,至少需要保留一个非禁用状态的超级管理员账号".ToLang() };
        }
        if (result == 0)
        {
            return new HandlerResult { Status = false, Message = "删除失败".ToLang() };
        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    /// <summary>
    /// 个人资料
    /// </summary>
    /// <returns></returns>
    public HandlerResult UpdateUserInfo()
    {
        int userId = RequestUtil.Instance.GetFormString("UserId").ToInt(0);

        //反射获取表单字段数据
        var type = typeof(Users);
        var user = Whir.Security.ServiceFactory.UsersService.SingleOrDefault<Users>(userId) ?? ModelFactory<Users>.Insten();
        user = GetPostObject(type, user) as Users;
        if (userId <= 0)
        {
            return new HandlerResult { Status = false, Message = "当前用户不存在".ToLang() };
        }
        else
        {
            Whir.Security.ServiceFactory.UsersService.Update(user);
            if (user != null)
                Whir.Service.ServiceFactory.OperateLogService.Save(string.Format("修改管理员【{0}】个人信息", user.LoginName));
        }

        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 密码设置
    /// </summary>
    /// <returns></returns>
    public HandlerResult ChangePwd()
    {
        int userId = RequestUtil.Instance.GetFormString("UserId").ToInt(0);
        string oldPassword = RequestUtil.Instance.GetFormString("txtOldPassword");
        string newPassword = RequestUtil.Instance.GetFormString("txtNewPassWord");

        if (userId > 0)
        {
            if (newPassword != "")
            {
                if (!newPassword.IsGoodPwd() && AppSettingUtil.GetString("IsStrongPassword").ToBoolean(true))
                    return new HandlerResult { Status = false, Message = "密码必须包含数字、字母、特殊符号，长度在6-20位".ToLang() };


                oldPassword = StrExt.GetSHA1Str(oldPassword);
                newPassword = StrExt.GetSHA1Str(newPassword);
                Users model = Whir.Repository.DbHelper.CurrentDb.SingleOrDefault<Users>(userId);
                if (model != null && model.Password == oldPassword)
                {
                    if (oldPassword == newPassword)
                        return new HandlerResult { Status = false, Message = "新密码不能与旧密码一样".ToLang() };

                    model.Password = newPassword;
                    model.LastPasswordDate = DateTime.Now;
                    Whir.Repository.DbHelper.CurrentDb.Update(model);
                    //重新登录一次
                    //LoginService ls = new LoginService();
                    //ls.Login(Model.LoginName, txtNewPassWord.Text.Trim(), "");

                    Whir.Service.ServiceFactory.OperateLogService.Save(string.Format("【{0}】管理员修改登录密码成功",
                        model.LoginName));

                    return new HandlerResult { Status = true, Message = "修改密码成功".ToLang() };
                }
                return new HandlerResult { Status = false, Message = "旧密码输入错误".ToLang() };
            }
            return new HandlerResult { Status = false, Message = "请输入新密码".ToLang() };
        }
        return new HandlerResult { Status = false, Message = "找不到记录".ToLang() };
    }

    public void GetList()
    {
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int currentRolesId = Whir.ezEIP.BasePage.RequestInt32("rolesid", 0);
        string rolesIds = string.Empty;
        if (currentRolesId == 0)
        {
            SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("29"), true);
        }
        else
        {
            SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("325"), true);
        }
        #region 取得当前角色所有下级角色Id串
        if (currentRolesId != 0)
        {
            rolesIds = currentRolesId.ToStr();
        }
        else if (!SysManagePageBase.IsDevUser)
        {
            var roles = Whir.Security.ServiceFactory.RolesService.GetList(SysManagePageBase.CurrentUserRolesId);

            rolesIds += string.Join(",", roles.Select(p => p.RoleId));

            //如果当前角色有下级角色，则显示当前角色下的管理员
            if (rolesIds != "")
            {
                rolesIds = "," + SysManagePageBase.CurrentUserRolesId + "," + rolesIds.TrimEnd(',');
            }
            else
            {
                rolesIds = "," + SysManagePageBase.CurrentUserRolesId.ToStr();//如果当前角色无下级角色，则显示当前角色管理员
            }
        }
        else
        {
            //开发者账户按角色查看管理员
            var roleId = RequestUtil.Instance.GetQueryString("rolesid").ToInt();
            rolesIds = roleId > 0 ? roleId.ToStr() : string.Empty;
        }

        #endregion

        string sql = @"SELECT SU.*,SR.RoleName RolesName
                            FROM   Whir_Sec_Users SU
                                   {0} JOIN Whir_Sec_Roles SR ON  SU.RolesId = SR.RoleId
                            WHERE  SU.IsDel = 0 ".FormatWith(SysManagePageBase.IsSuperUser || SysManagePageBase.IsDevUser ? "left" : "");
        if (!SysManagePageBase.IsDevUser)
        {
            if (!string.IsNullOrEmpty(rolesIds))
            {
                sql += " AND SU.RolesId IN (@0)";
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(rolesIds))
            {
                sql += " AND SU.RolesId =@0";
            }
        }
        sql += " ORDER BY SU.Sort desc, SU.CreateDate DESC ";
        var list = Whir.Security.ServiceFactory.UsersService.Page(pageIndex, pageSize, sql, rolesIds.ToInts(','));

        long total = list.TotalItems;

        string data = list.Items.ToJson();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }
}