using System;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Service;
using System.Linq;
using Whir.Repository;
using System.Collections.Generic;

public partial class whir_system_Handler_Developer_Column : SysHandlerPageBase
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
        int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt();
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("栏目修改", columnId, -1));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }


        //int siteId = RequestUtil.Instance.GetFormString("SiteId").ToInt(1);
        //反射获取表单字段数据
        var type = typeof(Column);
        var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId) ?? ModelFactory<Column>.Insten();
        column = GetPostObject(type, column) as Column;
        if (column.Path.IsEmpty())
            return new HandlerResult { Status = false, Message = "英文目录不能为空".ToLang() };

        if (ServiceFactory.ColumnService.IsExsitPath(column.Path, CurrentSiteId, column.ColumnId))
            return new HandlerResult { Status = false, Message = "英文目录：".ToLang() + column.Path + " 重复，请更换其他目录".ToLang() };

        if (column.ModuleMark != null && (!(column.ModuleMark.ToLower() == "content_v0.0.01" || column.ModuleMark.ToLower() == "subsitecontent_v0.0.01")))
        {   //只有 简单信息 才有 启用转向链接，启用相关文章功能
            column.IsRedirect = false;
            column.IsRelated = false;
        }
        if (!ServiceFactory.ColumnService.IsCategoryParent(column.ColumnId))
            column.IsCategory = false;  //只有 有类别管理的栏目 才有单独类别功能

        column.SiteType = 0;

        if (column.ColumnId <= 0)
        {
            //默认栏目前台显示
            column.IsShow = true;
            column.IsCategoryShow = true;
            //默认生成动态
            column.CreateMode = 2;
            //添加栏目
            ServiceFactory.ColumnService.AddColumn(column);
        }
        else
        {
            //更新栏目
            ServiceFactory.ColumnService.UpdateColumn(column);
        }
        ContentHelper.SetColumnCookie("column_refresh_flag", "0", columnId.ToStr());
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveOutLink()
    {
        try
        {
            int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
            int siteId = RequestUtil.Instance.GetFormString("SiteId").ToInt(1);

            //反射获取表单字段数据
            var type = typeof(Column);
            var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId) ?? ModelFactory<Column>.Insten();
            column = GetPostObject(type, column) as Column;

            int count;
            if (columnId == 0)
            {
                if (column != null)
                {
                    column.SiteId = CurrentSiteId;
                    column.SiteType = 0;
                    column.IsCustomSubsite = false;

                    count = ServiceFactory.ColumnService.Insert(column).ToInt();
                    //操作日志记录
                    if (count > 0) ServiceFactory.ColumnService.SaveLog(column, "insert");

                    ////赋权限给客户后台
                    string txt = Whir.Security.ServiceFactory.RolesService.GetColumnAllFunction(columnId, siteId);
                    Whir.Security.ServiceFactory.RolesService.AddRoleJurisdiction(1, 2, txt);//默认给超级管理员加上权限

                }
                ContentHelper.SetColumnCookie("column_refresh_flag", "0", columnId.ToStr());
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
            count = ServiceFactory.ColumnService.Update(column).ToInt();
            //操作日志记录
            if (count > 0) ServiceFactory.ColumnService.SaveLog(column, "update");
            ContentHelper.SetColumnCookie("column_refresh_flag", "0", columnId.ToStr());
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }

    }

    /// <summary>
    /// 批量保存栏目
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveMulti()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int parentId = RequestUtil.Instance.GetFormString("ParentId").ToInt();
        string batchColumnName = RequestUtil.Instance.GetFormString("BatchColumnName");
        ServiceFactory.ColumnService.AddColumns(
                parentId,
                 CurrentSiteId,
                 batchColumnName);
        ContentHelper.SetColumnCookie("column_refresh_flag");
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult SaveArea()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("256"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int areaId = RequestUtil.Instance.GetFormString("id").ToInt();

        //反射获取表单字段数据
        var type = typeof(Area);
        var column = DbHelper.CurrentDb.Query<Area>("WHERE Id=@0", areaId).FirstOrDefault() ??
                     ModelFactory<Area>.Insten();
        column = GetPostObject(type, column) as Area;

        if (column.Id <= 0)
        {
            DbHelper.CurrentDb.Insert(column);
        }
        else
        {
            DbHelper.CurrentDb.Update(column);
        }
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 批量保存区域
    /// </summary>
    /// <returns></returns>
    public HandlerResult DeleteArea()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("256"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int id = RequestUtil.Instance.GetFormString("Id").ToInt();
        DbHelper.CurrentDb.Execute("delete dbo.Whir_Cmn_Area  WHERE Id=@0", id);
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 批量保存区域
    /// </summary>
    /// <returns></returns>
    public HandlerResult SortArea()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("256"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string strSort = RequestUtil.Instance.GetFormString("Sort").Trim(',');
        var arrSort = strSort.Split(',');
        foreach (string str in arrSort)
        {
            int id = str.Split('|')[0].ToInt();
            long sort = str.Split('|')[1].ToLong(0);
            DbHelper.CurrentDb.Execute("UPDATE Whir_Cmn_Area SET Sort=@0 WHERE Id=@1", sort, id);
        }
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 删除栏目
    /// </summary>
    /// <returns></returns>
    public HandlerResult DeleteColumn()
    {
        int id = RequestUtil.Instance.GetFormString("Id").ToInt();
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("栏目删除", id, -1));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //记录操作日志
        ServiceFactory.ColumnService.SaveLog(id, "delete");
        ServiceFactory.ColumnService.DeleteColumn(id);
        ContentHelper.SetColumnCookie("column_refresh_flag");
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 批量排序栏目
    /// </summary>
    /// <returns></returns>
    public HandlerResult SortColumn()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser||SysManagePageBase.IsSuperUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string strSort = RequestUtil.Instance.GetFormString("Sort").Trim(',');
        string[] arrSort = strSort.Split(',');
        foreach (string str in arrSort)
        {
            int columnId = str.Split('|')[0].ToInt();
            long sort = str.Split('|')[1].ToLong(0);
            ServiceFactory.ColumnService.ModifyColumnSort(columnId, sort);
        }
        ContentHelper.SetColumnCookie("column_refresh_flag");
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 移动栏目
    /// </summary>
    /// <returns></returns>
    public HandlerResult ColumnMove()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string origin = RequestUtil.Instance.GetFormString("Origin");
        string target = RequestUtil.Instance.GetFormString("Target");
        string selValue = RequestUtil.Instance.GetFormString("MoveType");
        return ColumnMove(origin, target, selValue);
    }

    /// <summary>
    /// 栏目移动
    /// </summary>
    /// <param name="origin">栏目源</param>
    /// <param name="target">目标源</param>
    /// <param name="selValue">移动方式</param>
    private HandlerResult ColumnMove(string origin, string target, string selValue)
    {
        string[] origins = origin.Substring(0, origin.Length - 1).Split(',');//去掉origin字符串最后一个,字符
        string[] targets = target.Substring(0, target.Length - 1).Split(',');//去掉target字符串最后一个,字符
        List<string> alOrigin = new List<string>();
        List<string> alTarget = new List<string>();

        //左侧的List集合
        foreach (string str in origins)//12,114
        {
            IList<Column> listOrigin = ServiceFactory.ColumnService.GetColumnByParentId(str.ToInt(), CurrentSiteId);
            if (!alOrigin.Contains(str))
            {
                alOrigin.AddRange(listOrigin.Select(cl => cl.ColumnId.ToStr()));
            }
        }
        //右侧的List集合
        foreach (string str in targets)
        {
            IList<Column> listTarget = ServiceFactory.ColumnService.GetColumnByParentId(str.ToInt(), CurrentSiteId);
            if (!alTarget.Contains(str))
            {
                alTarget.AddRange(listTarget.Select(cl => cl.ColumnId.ToStr()));
            }
        }
        //左右两侧比较，左侧多选，右侧单选，则取右侧与左侧进行对比
        //如果存在有相同的，则提示栏目源与目标栏目移动出错
        int targetId = targets[0].ToInt();
        //foreach (string strT in alTarget)
        //{
        if (alOrigin.Count == 1 && alOrigin[0].ToInt() == targetId)
        {
            return new HandlerResult { Status = false, Message = "目标栏目和源栏目相同，请正确勾选源栏目和目标栏目".ToLang() };
        }
        if (alOrigin.Contains(targetId.ToStr()))
        {
            return new HandlerResult { Status = false, Message = "目标栏目是源栏目的子栏目，请正确勾选源栏目和目标栏目".ToLang() };
        }

        //根据目标栏目的id查询出最大的SortID
        Column targetColumn = ServiceFactory.ColumnService.GetMaxSortByParentId(targetId, CurrentSiteId);
        //栏目
        long sort = targetColumn.Sort;
        int parentid = targetColumn.ParentId;
        int count = 0;
        switch (selValue)
        {
            case "Child":
                //遍历所有选中的栏目源
                //这样只能移动第一级
                foreach (string strO in alOrigin)
                {
                    string parentIDs = getAllParents(strO.ToInt());
                    Column thisColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(strO.ToInt());
                    if (thisColumn == null) continue;

                    for (int i = 0; i < parentIDs.Trim(',').Split(',').Length; i++)
                    {
                        string pids = parentIDs.Trim(',').Split(',')[i];

                        if (pids.IsEmpty()) continue;

                        if (alOrigin.Contains(pids))//已添加过
                        {
                            continue;
                        }

                        if (i == 0)
                        {
                            count += ServiceFactory.ColumnService.UpdateColumnMove(targetId, ++sort, strO.ToInt());
                        }
                        else
                        {
                            count += ServiceFactory.ColumnService.UpdateColumnMove(++sort, strO.ToInt());
                        }
                    }
                }
                if (count > 0)
                {
                    ContentHelper.SetColumnCookie("column_refresh_flag");
                    return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
                }
                break;
            case "Before":
                //遍历所有选中的栏目源
                //这样只能移动第一级
                foreach (string strO in alOrigin)
                {
                    string parentIDs = getAllParents(strO.ToInt());
                    Column thisColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(strO.ToInt());
                    if (thisColumn == null) continue;

                    for (int i = 0; i < parentIDs.Trim(',').Split(',').Length; i++)
                    {
                        string pids = parentIDs.Trim(',').Split(',')[i];

                        if (pids.IsEmpty()) continue;

                        if (alOrigin.Contains(pids))//已添加过
                        {
                            continue;
                        }
                        //栏目源换成目标栏目的sort
                        //查询栏目源和目标栏目之前的所有sort，并所有都改变
                        //父节点减一
                        if (i == 0)
                        {
                            count += ServiceFactory.ColumnService.UpdateColumnMove(parentid, --sort, strO.ToInt());
                        }
                        else
                        {
                            count += ServiceFactory.ColumnService.UpdateColumnMove(--sort, strO.ToInt());
                        }
                    }
                }
                if (count > 0)
                {
                    ContentHelper.SetColumnCookie("column_refresh_flag");
                    return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
                }

                break;
            case "After":
                //遍历所有选中的栏目源
                //这样只能移动第一级
                foreach (string strO in alOrigin)
                {
                    string parentIDs = getAllParents(strO.ToInt());
                    Column thisColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(strO.ToInt());
                    if (thisColumn == null) continue;

                    for (int i = 0; i < parentIDs.Trim(',').Split(',').Length; i++)
                    {
                        string pids = parentIDs.Trim(',').Split(',')[i];

                        if (pids.IsEmpty()) continue;

                        if (alOrigin.Contains(pids))//已添加过
                        {
                            continue;
                        }
                        if (i == 0)
                        {
                            count += ServiceFactory.ColumnService.UpdateColumnMove(parentid, ++sort, strO.ToInt());
                        }
                        else
                        {
                            count += ServiceFactory.ColumnService.UpdateColumnMove(++sort, strO.ToInt());
                        }
                    }
                }
                if (count > 0)
                {
                    ContentHelper.SetColumnCookie("column_refresh_flag");
                    return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
                }
                break;
        }
        return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
    }

    /// <summary>
    /// 根据栏目ID， 获取此栏目的父ID， 以及所有的上级父ID
    /// </summary>
    /// <param name="columnId"></param>
    /// <returns></returns>
    private string getAllParents(int columnId)
    {
        string parentIDs = "";
        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId);
        if (column != null)
        {
            if (column.ParentId == 0)
                return parentIDs + ",0";
            parentIDs += column.ParentId + "," + getAllParents(column.ParentId);
        }
        return parentIDs;
    }
}