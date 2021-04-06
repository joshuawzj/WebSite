using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using System.Collections.Generic;
using Whir.Language;
using Whir.Label;

public partial class whir_system_Handler_Developer_Site : SysHandlerPageBase
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
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int siteId = RequestUtil.Instance.GetFormString("SiteId").ToInt();

        //反射获取表单字段数据
        var type = typeof(SiteInfo);
        var site = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(siteId) ?? ModelFactory<SiteInfo>.Insten();

        site = GetPostObject(type, site) as SiteInfo;
        ServiceFactory.SiteInfoService.Save(site);

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
        int siteId = RequestUtil.Instance.GetFormString("SiteId").ToInt();
        var site = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(siteId);
        if (site == null)
            return new HandlerResult { Status = false, Message = "要删除的菜单数据不存在".ToLang() };

        ServiceFactory.SiteInfoService.Delete(site);
        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Delete()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int siteId = RequestUtil.Instance.GetFormString("SiteId").ToInt();
        ServiceFactory.SiteInfoService.Delete<SiteInfo>(siteId);
        ServiceFactory.SiteInfoService.SaveLog(siteId.ToInt(), "delete");
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    /// <summary>
    /// 栏目站点复制
    /// </summary>
    /// <returns></returns>
    public HandlerResult SiteColumnCopy()
    {
        int SiteId = RequestUtil.Instance.GetFormString("SiteId").ToInt();
        int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt();
        int quantity = RequestUtil.Instance.GetFormString("Quantity").ToInt();
        string name = RequestUtil.Instance.GetFormString("Name");
        string path = RequestUtil.Instance.GetFormString("Path");
        int ddlColumn = RequestUtil.Instance.GetFormString("ddlColumn").ToInt();
        int ddlSite = RequestUtil.Instance.GetFormString("ddlSite").ToInt();
        if (path.IsEmpty())
            return new HandlerResult { Status = false, Message = "英文目录不能为空".ToLang() };

        if (columnId != 0)
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser || SysManagePageBase.IsSuperUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            //新copy出来的栏目id集合
            int[] newColumnsIdArr = new int[quantity];
            //复制栏目
            try
            {

                string columnName = name.Trim();
                string columnPath = path.Trim();
                int parentId = 0;
                int siteId = 0;

                Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId.ToInt());

                if (ddlColumn != 0)
                {
                    Column targetColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ddlColumn.ToInt());
                    parentId = targetColumn.ColumnId;
                    siteId = targetColumn.SiteId;
                }
                else
                {
                    parentId = 0;
                    siteId = ddlSite.ToInt();
                }

                if (column != null)
                {
                    if (ServiceFactory.ColumnService.IsExsitPath(columnPath, siteId))
                        return new HandlerResult { Status = false, Message = "英文目录：".ToLang() + columnPath + " 重复，请更换其他目录".ToLang() };

                    string newColumnPath = columnPath;
                    for (int i = 0; i < quantity.ToInt(); i++)
                    {
                        if (i > 0)
                            columnPath = newColumnPath + i + Rand.Instance.Number(3, true);
                        int newColumnId = ServiceFactory.ColumnService.CopyColumn(columnId,
                            columnName,
                            columnPath,
                            parentId,
                            column.MarkParentId,
                            siteId,
                            column.SiteType,
                            column.IsCustomSubsite);
                        newColumnsIdArr[i] = newColumnId;
                    }

                    //清除所有角色的栏目资源缓存
                    ContentHelper.SetColumnCookie("column_refresh_flag");
                    return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
                }
            }
            catch (Exception ex)
            {
                return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
            }
            //发布新copy出来的栏目
            if (newColumnsIdArr.Length > 0)
            {
                List<Column> listColumns =
                    ServiceFactory.ColumnService.Query<Column>("WHERE ColumnId IN (@0)", newColumnsIdArr).ToList();
                SiteInfo site = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(ddlSite.ToInt());
                if (listColumns.Count > 0 && site != null)
                {
                    LabelHelper.Instance.BuildSiteColumn(listColumns, site, true, true, true, false);
                }
            }
        }
        else if (SiteId != 0)
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            //复制站点
            try
            {
                string siteName = name.Trim();
                string sitePath = path.Trim();
                ServiceFactory.SiteInfoService.CopySite(SiteId, siteName, sitePath);
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
            catch (Exception ex)
            {
                return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
            }
        }
        return new HandlerResult { Status = false, Message = "操作失败：".ToLang() };
    }
}