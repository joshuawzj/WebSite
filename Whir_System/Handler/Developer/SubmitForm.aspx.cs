using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Whir.Language;

public partial class Whir_System_Handler_Developer_SubmitForm : SysHandlerPageBase
{
    public class DropDownList
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
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
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser || SysManagePageBase.IsSuperUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var submitId = RequestUtil.Instance.GetFormString("SubmitID").ToInt(0);
        var actionPage = RequestUtil.Instance.GetFormString("ActionPage");
        SubmitForm submitForm = ServiceFactory.SubmitFormService.SingleOrDefault<SubmitForm>(submitId) ?? ModelFactory<SubmitForm>.Insten();
        //反射获取表单字段数据
        var type = typeof(SubmitForm);
        try
        {
            submitForm = GetPostObject(type, submitForm) as SubmitForm;
            if (submitForm != null)
            {
                switch (submitForm.SuccessAction)
                {
                    //不刷新
                    case "0":
                        submitForm.SuccessAction = "false";
                        break;
                    //刷新
                    case "1":
                        submitForm.SuccessAction = "true";
                        break;
                    //跳转到指定页面
                    case "2":
                        submitForm.SuccessAction = actionPage;
                        break;
                }
                ServiceFactory.SubmitFormService.Save(submitForm);
            }
        }
        catch (Exception ex)
        {
            throw;
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
        int submitId = RequestUtil.Instance.GetFormString("submitId").ToInt();
        var submitForm = ServiceFactory.SubmitFormService.SingleOrDefault<SubmitForm>(submitId);
        if (submitForm == null)
            return new HandlerResult { Status = false, Message = "要删除的菜单数据不存在".ToLang() };

        ServiceFactory.SubmitFormService.Delete(submitForm);
        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult DelAll()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string ids = RequestUtil.Instance.GetFormString("selected").ToStr().Trim();
        foreach (var id in ids.Split(','))
        {
            ServiceFactory.SubmitFormService.Delete<SubmitForm>(id.ToInt());
        }

        return new HandlerResult {Status = true, Message = "删除成功".ToLang() };
    }


    /// <summary>
    /// 获取栏目数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult GetColumns()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        List<DropDownList> dropDownList = new List<DropDownList>();
        int columnid = RequestUtil.Instance.GetFormString("columnid").ToInt(0);
        if (columnid > 0)
        {
            var list = ServiceFactory.SubmitFormService.GetColumnsByMarkParentID(columnid);
            Column colNull = new Column();
            colNull.ColumnId = -1;
            colNull.ColumnName = "==请选择==".ToLang();
            list.Insert(0, colNull);
            foreach (var item in list)
            {
                DropDownList temp=new DropDownList()
                {
                    Value = item.ColumnId.ToStr(),
                    Text = item.ColumnName.ToStr()
                };
                dropDownList.Add(temp);
            }
            
        }
        else if (columnid < 0) //会员系统
        {
            dropDownList.Add(new DropDownList()
            {
                Value = "",
                Text = "==请选择==".ToLang()
            });
            dropDownList.Add(new DropDownList()
            {
                Value = "-1",
                Text = "会员登录".ToLang()
            });
            dropDownList.Add(new DropDownList()
            {
                Value = "-2",
                Text = "会员注册".ToLang()
            });
            dropDownList.Add(new DropDownList()
            {
                Value = "-3",
                Text = "更改密码".ToLang()
            });
            dropDownList.Add(new DropDownList()
            {
                Value = "-4",
                Text = "个人资料更改".ToLang()
            });
            dropDownList.Add(new DropDownList()
            {
                Value = "-5",
                Text = "是否登录".ToLang()
            });
            dropDownList.Add(new DropDownList()
            {
                Value = "-6",
                Text = "找回密码".ToLang()
            });
        }
        return new HandlerResult { Status = true, Message = dropDownList.ToJson() };
    }


    /// <summary>
    /// 获取示例代码
    /// </summary>
    /// <returns></returns>
    public HandlerResult GetDemoCode()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int columnId = RequestUtil.Instance.GetFormString("columnid").ToInt(0);
        int submitId = RequestUtil.Instance.GetFormString("submitId").ToInt(0);
        string demoCode = "";
        if (columnId < 0)//会员系统
        {
            switch (columnId)
            {
                case -1://会员登录
                    demoCode = GetTempDemoStr(SubmitFormMemType.Login);
                    break;
                case -2://会员注册
                    demoCode = ServiceFactory.SubmitFormService.GetDemoCodeByColumnID(1, submitId) + GetTempDemoStr(SubmitFormMemType.Regist);
                    break;
                case -3://更换密码
                    demoCode = GetTempDemoStr(SubmitFormMemType.ChangePassword);
                    break;
                case -4://个人资料更改
                    demoCode = ServiceFactory.SubmitFormService.GetDemoCodeByColumnID(1, submitId) + GetTempDemoStr(SubmitFormMemType.Personal);
                    break;
                case -5://是否登录
                    demoCode = GetTempDemoStr(SubmitFormMemType.IsLogin);
                    break;
                case -6://找回密码
                    demoCode = GetTempDemoStr(SubmitFormMemType.GetPassword);
                    break;
            }
        }
        else if (columnId > 0)
        {
            demoCode = ServiceFactory.SubmitFormService.GetDemoCodeByColumnID(columnId, submitId);
        }
        else
        {
            demoCode = "";
        }
        return new HandlerResult { Status = true, Message = demoCode };
    }

    /// <summary>
    /// 设置预览
    /// </summary>
    /// <returns></returns>
    public HandlerResult SetPreviewCode()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        Session["SubmitFormCode"] = RequestUtil.Instance.GetFormString("content");
        return new HandlerResult { Status = true, Message = "" };
    }

    /// <summary>
    /// 保存代码
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveCode()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int submitId = RequestUtil.Instance.GetFormString("SubmitID").ToInt(0);
        int columnId = RequestUtil.Instance.GetFormString("ChildColumn").ToInt(0);
        string invokeCode = RequestUtil.Instance.GetFormString("InvokeCode");
        SubmitForm model = ServiceFactory.SubmitFormService.SingleOrDefault<SubmitForm>(submitId);
        if (model != null)
        {
            model.ColumnId = columnId < 0 ? 1 : columnId;
            model.InvokeCode = invokeCode;
            if (columnId < 0)
            {
                model.MemberType = columnId;
            }
            ServiceFactory.SubmitFormService.Save(model);
            return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
        }
        else
        {
            return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
        }
        
    }

    
    /// <summary>
    /// 获取会员系统示例代码
    /// </summary>
    /// <param name="memberType"></param>
    /// <returns></returns>
    protected string GetTempDemoStr(SubmitFormMemType memberType)
    {
        string fileName = "login.config";
        switch (memberType)
        {
            case SubmitFormMemType.Login:
                fileName = "login.config";
                break;
            case SubmitFormMemType.ChangePassword:
                fileName = "changepassword.config";
                break;
            case SubmitFormMemType.IsLogin:
                fileName = "islogin.config";
                break;
            case SubmitFormMemType.Personal:
                fileName = "personal.config";
                break;
            case SubmitFormMemType.GetPassword:
                fileName = "getpassword.config";
                break;
            default:
                return "";
        }
        string fielPath = SysPath + "temp/labeltemp/" + fileName;
        return FileSystemHelper.ReadFile(Server.MapPath(fielPath));
    }


    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser || SysManagePageBase.IsSuperUser, true);
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int total = 0;
        var list = ServiceFactory.SubmitFormService.PageList(pageIndex, pageSize, out total);
        string data = list.ToJson();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

}