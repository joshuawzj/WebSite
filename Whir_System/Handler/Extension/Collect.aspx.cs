using System;
using Whir.Cache;
using Whir.Cache.Enum;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using System.Linq;
using Whir.Language;
using System.Collections.Generic;


public partial class Whir_System_Handler_Extension_Collect : SysHandlerPageBase
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
        int collectId = RequestUtil.Instance.GetFormString("CollectId").ToInt();
        var handlerResult = new HandlerResult();
        if (collectId > 0)
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("351"));
        }
        else
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("353"));
        }
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //反射获取表单字段数据
        var type = typeof(Collect);
        var collect = ServiceFactory.CollectService.SingleOrDefault<Collect>(collectId) ?? ModelFactory<Collect>.Insten();
        try
        {
            collect = GetPostObject(type, collect) as Collect;
        }
        catch (Exception ex)
        {
            throw;
        }

        if (collectId > 0) //编辑
        {
            ServiceFactory.CollectService.Update(collect);
            //清理缓存
            if (collect != null)
            {
                string ruleCacheKey = CacheKeys.CollectionRulesPrefix + collect.CollectId;
                SiteCache.Remove(ruleCacheKey);
            }
        }
        else
        {
            collectId = ServiceFactory.CollectService.Insert(collect).ToInt();

        }
        return new HandlerResult { Status = true, Message = SysPath + "module/extension/CollectStep2.aspx?collectid=" + collectId };
    }


    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Next()
    {
        int collectId = RequestUtil.Instance.GetFormString("CollectId").ToInt();
        var handlerResult = new HandlerResult();
        if (collectId > 0)
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("351"));
        }
        else
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("353"));
        }
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //反射获取表单字段数据
        var type = typeof(Collect);
        var collect = ServiceFactory.CollectService.SingleOrDefault<Collect>(collectId) ?? ModelFactory<Collect>.Insten();
        try
        {
            collect = GetPostObject(type, collect) as Collect;
        }
        catch (Exception ex)
        {
            throw;
        }

        ServiceFactory.CollectService.Update(collect);
        return new HandlerResult { Status = true, Message = SysPath + "module/extension/CollectStep2.aspx?collectid=" + collectId };
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Finish()
    {
        int formId = RequestUtil.Instance.GetFormString("formid").ToInt();
        int collectId = RequestUtil.Instance.GetFormString("CollectId").ToInt();
        var handlerResult = new HandlerResult();
        if (collectId > 0)
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("351"));
        }
        else
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("353"));
        }
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var model = ServiceFactory.CollectFieldService.Query<CollectField>("SELECT * FROM Whir_Ext_CollectField WHERE FormId=@0 AND CollectId=@1",
            formId, collectId).FirstOrDefault() ?? ModelFactory<CollectField>.Insten(); ;

        //反射获取表单字段数据
        var type = typeof(CollectField);
        try
        {
            model = GetPostObject(type, model) as CollectField;
        }
        catch (Exception ex)
        {
            throw;
        }

        ServiceFactory.CollectFieldService.Save(model);
        return new HandlerResult { Status = true, Message = "设置成功".ToLang() };
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Finished()
    {
        //var formId = RequestUtil.Instance.GetFormString("formid").ToInt();
        var collectId = RequestUtil.Instance.GetFormString("CollectId").ToInt();
        var hidValue = RequestUtil.Instance.GetFormString("hidValue").ToStr();

        var handlerResult = new HandlerResult();
        if (collectId > 0)
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("351"));
        }
        else
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("353"));
        }
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        ////反射获取表单字段数据
        //var type = typeof(CollectField);
        //try
        //{
        //    model = GetPostObject(type, model) as CollectField;
        //}
        //catch (Exception ex)
        //{
        //    throw;
        //}

        string data = hidValue.TrimEnd('□');
        string[] strArr = data.Split('□');
        IList<int> formidList = new List<int>();
        for (int i = 0; i < strArr.Length; i++)
        {
            string[] formidTypeValue = strArr[i].Split('§');
            if (formidTypeValue.Length < 3)
            {
                continue;
            }
            int formid = formidTypeValue[0].ToInt();
            formidList.Add(formid);
            int collectType = formidTypeValue[1].ToInt();
            string defaultValue = formidTypeValue[2].Trim();
            var model = ServiceFactory.CollectFieldService.Query<CollectField>(
                               "SELECT * FROM Whir_Ext_CollectField WHERE FormId=@0 AND CollectId=@1",
                               formid, collectId).FirstOrDefault() ?? ModelFactory<CollectField>.Insten();

            if (model != null)
            {
                if (model.Id > 0)
                {
                    model.CollectType = collectType;
                    model.DefaultValue = defaultValue;
                    ServiceFactory.CollectFieldService.Update(model);
                }
                else
                {
                    model.FormId = formid;
                    model.CollectType = collectType;
                    model.DefaultValue = defaultValue;
                    model.CollectId = collectId;
                    ServiceFactory.CollectFieldService.Insert(model);
                }
            }
        }
        //清除之前保存不存在的表单
        if (formidList.Count > 0)
            ServiceFactory.CollectFieldService.Execute(
                "DELETE FROM Whir_Ext_CollectField WHERE FormId NOT IN (@0) AND CollectId=@1", formidList.ToArray(),
                collectId);
        return new HandlerResult { Status = true, Message = "设置成功".ToLang() };
    }


    /// <summary>
    /// 获取分页数据
    /// </summary>
    /// <returns></returns>
    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("38"), true);
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;


        var data = ServiceFactory.CollectService.Page(pageIndex, pageSize,
            "SELECT coll.*,colu.ColumnName FROM Whir_Ext_Collect coll INNER JOIN Whir_Dev_Column colu ON coll.ColumnId=colu.ColumnId Where colu.IsDel=0 Order By coll.Sort Desc");
        long total = data.TotalItems.ToInt();
        var js = data.Items.ToJson();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, js);
        Response.Clear();
        Response.Write(json);
        //Response.End();
    }


    /// <summary>
    /// 排序
    /// </summary>
    /// <returns></returns>
    public HandlerResult Sort()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("355"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var apidSort = RequestUtil.Instance.GetString("SortIds");

        if (!apidSort.IsEmpty())
        {
            var idSorts = apidSort.Split(','); //主键ID与Sort主键对
            foreach (var s in idSorts)
            {
                //ID与Sort
                var idSort = s.Split('|');
                //更新排序
                long sort = 0;
                if (idSort.Length < 2 || !long.TryParse(idSort[1], out sort))
                {
                    continue;
                }
                ServiceFactory.CollectService.ModifySort(idSort[0].ToInt(), sort);
            }
        }
        return new HandlerResult { Status = true, Message = "排序成功".ToLang() };
    }


    /// <summary>
    /// 删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult Delete()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("352"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var ids = RequestUtil.Instance.GetString("ChooseIds");

        if (!ids.IsEmpty())
        {
            string[] idArray = ids.Split(new char[] { ',' });
            foreach (string str in idArray)
            {
                int pid = str.ToInt();

                #region 操作日志使用

                Collect model = ServiceFactory.CollectService.SingleOrDefault<Collect>(pid);

                #endregion 操作日志使用

                ServiceFactory.CollectService.Delete(pid);

                //添加操作记录日志
                ServiceFactory.OperateLogService.Save(string.Format("删除信息采集，项目名称【{0}】", model.ItemName));
            }
        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }
}