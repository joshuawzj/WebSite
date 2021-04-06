using System;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Language;
using System.Collections.Generic;
using Whir.Repository;
using System.Linq;

public partial class Whir_System_Handler_Common_Subject : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }

    //删除
    public HandlerResult DeleteSubject()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser || SysManagePageBase.IsSuperUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            ServiceFactory.SubjectService.Delete<Subject>(RequestUtil.Instance.GetString("id").ToInt());
            ContentHelper.SetColumnCookie("subject_refresh_flag");
            ContentHelper.SetColumnCookie("subsite_refresh_flag");

            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }
    //删除
    public HandlerResult DeleteSubjectClass()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            ServiceFactory.SubjectClassService.Delete<SubjectClass>(RequestUtil.Instance.GetString("id").ToInt());
            ContentHelper.SetColumnCookie("subject_refresh_flag");
            ContentHelper.SetColumnCookie("subsite_refresh_flag");
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }
    //排序
    public HandlerResult SortSubject()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser || SysManagePageBase.IsSuperUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            string apidSort = RequestUtil.Instance.GetString("apidsort").Trim(','); //主键ID与Sort键值对字符串
            var idSorts = apidSort.Split(','); //主键ID与Sort主键对
            foreach (var s in idSorts)
            {
                //ID与Sort
                var idSort = s.Split('|');
                //更新排序
                long sort = 0;
                if (idSort.Length < 3 || !long.TryParse(idSort[2], out sort))
                {
                    continue;
                }
                switch (idSort[0])
                {
                    case "0":
                        ServiceFactory.SubjectClassService.ModifySort(idSort[1].ToInt(), sort);
                        break;
                    case "1":
                        ServiceFactory.SubjectService.ModifySort(idSort[1].ToInt(), sort);
                        break;
                        //case "2"://关闭此排序
                        //    ServiceFactory.ColumnService.ModifyColumnSort(idSort[1].ToInt(), sort);
                        //    break;
                }

            }
            ContentHelper.SetColumnCookie("subsite_refresh_flag");
            ContentHelper.SetColumnCookie("subject_refresh_flag");
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }
    //保存子站类型
    public HandlerResult SaveSubjectClass()
    {
        int subjectClassId = RequestUtil.Instance.GetFormString("SubjectClassId").ToInt(0);
        var path = RequestUtil.Instance.GetFormString("Path");

        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveSubjectRes("subjectclass", "添加子站", subjectClassId, CurrentSiteId, 0));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }

        SubjectClass subjectclass = ServiceFactory.SubjectClassService.SingleOrDefault<SubjectClass>(subjectClassId) ??
                                    ModelFactory<SubjectClass>.Insten();
        var type = typeof(SubjectClass);
        subjectclass = GetPostObject(type, subjectclass) as SubjectClass;

        var column = ServiceFactory.ColumnService.GetSubjectIndexColumn(subjectclass.SubjectClassId) ??
                         ModelFactory<Column>.Insten();

        if (ServiceFactory.ColumnService.IsExsitPath(path, CurrentSiteId, column.ColumnId))
            return new HandlerResult { Status = false, Message = "英文目录：".ToLang() + path + " 重复，请更换其他目录".ToLang() };

        try
        {
            if (subjectClassId == 0)
            {
                subjectclass.SiteId = CurrentSiteId;
                int classId = ServiceFactory.SubjectClassService.Insert(subjectclass).ToInt();
                if (classId > 0)
                {
                    //加入类型首页
                    Column indexColumn = ModelFactory<Column>.Insten();
                    indexColumn.ColumnName = "首页";
                    indexColumn.ParentId = -10;
                    indexColumn.ColumnNameStage = subjectclass.SubjectClassName + " - 首页";
                    indexColumn.SubId = classId;
                    indexColumn.Path = path;
                    indexColumn.SiteId = CurrentSiteId;
                    ServiceFactory.ColumnService.Save(indexColumn);
                }
                //操作日志
                ServiceFactory.OperateLogService.Save("添加【{0}】".FormatWith(subjectclass.SubjectClassName));
                ContentHelper.SetColumnCookie("subject_refresh_flag");
                ContentHelper.SetColumnCookie("subsite_refresh_flag");
                string txt = Whir.Security.ServiceFactory.RolesService.GetSubjectAllFunction("subjectclass", subjectclass.SubjectClassId, subjectclass.SiteId, 0);
                if (subjectclass.SubjectTypeId == 2)
                    Whir.Security.ServiceFactory.RolesService.AddRoleJurisdiction(3, 2, txt);//默认给超级管理员加上权限
                else
                    Whir.Security.ServiceFactory.RolesService.AddRoleJurisdiction(2, 2, txt);//默认给超级管理员加上权限

                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
            else
            {
                subjectclass.SubjectClassName = subjectclass.SubjectClassName.Trim();
                subjectclass.SubjectTypeId = subjectclass.SubjectTypeId;

                column.Path = path;
                ServiceFactory.ColumnService.Update(column);

                ServiceFactory.SubjectClassService.Update(subjectclass);
                //操作日志
                ServiceFactory.OperateLogService.Save("修改【{0}】".FormatWith(subjectclass.SubjectClassName));
                ContentHelper.SetColumnCookie("subject_refresh_flag");
                ContentHelper.SetColumnCookie("subsite_refresh_flag");

                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }

        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }

    //保存子站
    public HandlerResult SaveSubject()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser || SysManagePageBase.IsSuperUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int subjectId = RequestUtil.Instance.GetFormString("SubjectId").ToInt(0);
        int subjectTypeId = RequestUtil.Instance.GetFormString("SubjectTypeId").ToInt(0);
        Subject subject = ServiceFactory.SubjectService.SingleOrDefault<Subject>(subjectId) ??
                          ModelFactory<Subject>.Insten();
        var type = typeof(Subject);
        subject = GetPostObject(type, subject) as Subject;

        try
        {
            switch (RequestUtil.Instance.GetFormString("SubjectTypeId").ToInt(0))
            {
                case 1:
                    ContentHelper.SetColumnCookie("subsite_refresh_flag");
                    break;
                case 2:
                    ContentHelper.SetColumnCookie("subject_refresh_flag");
                    break;
            }

            if (subjectId == 0)
            {
                subject.SiteId = CurrentSiteId;
                ServiceFactory.SubjectService.Save(subject);

                //加子站权限
                string txt = Whir.Security.ServiceFactory.RolesService.GetSubjectAllFunction("subject", subject.SubjectId, subject.SiteId, subject.SubjectClassId);
                //加子站栏目权限
                var columnList = Whir.Service.ServiceFactory.ColumnService.Query<Column>("Where SiteType=@0 and IsDel=0", subject.SubjectClassId).ToList();

                foreach (var column in columnList)
                {
                    txt += "," + Whir.Security.ServiceFactory.RolesService.GetSubjectAllFunction("subjectcolumn", column.ColumnId, subject.SiteId, subject.SubjectClassId);
                }

                //默认给超级管理员加上权限
                if (subjectTypeId == 2)
                    Whir.Security.ServiceFactory.RolesService.AddRoleJurisdiction(3, 2, txt);
                else
                    Whir.Security.ServiceFactory.RolesService.AddRoleJurisdiction(2, 2, txt);

                //操作日志
                ServiceFactory.OperateLogService.Save("添加【{0}】".FormatWith(subject.SubjectName));

                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }
            else
            {
                ServiceFactory.SubjectService.Update(subject);
                //操作日志
                ServiceFactory.OperateLogService.Save("修改【{0}】".FormatWith(subject.SubjectName));

                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            }

        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }

    //删除
    public HandlerResult DeleteColumn()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            ServiceFactory.ColumnService.DeleteColumn(RequestUtil.Instance.GetString("id").ToInt());
            ContentHelper.SetColumnCookie("subject_refresh_flag");
            ContentHelper.SetColumnCookie("subsite_refresh_flag");
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }
    //排序
    public HandlerResult SortColumn()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string strSort = RequestUtil.Instance.GetString("apidsort").Trim(',');
        string[] arrSort = strSort.Split(',');
        foreach (string str in arrSort)
        {
            int columnId = str.Split('|')[0].ToInt();
            long sort = str.Split('|')[1].ToLong(0);
            ServiceFactory.ColumnService.ModifyColumnSort(columnId, sort);
        }
        ContentHelper.SetColumnCookie("subsite_refresh_flag");
        ContentHelper.SetColumnCookie("subject_refresh_flag");
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    //保存栏目
    public HandlerResult Save()
    {
        int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt();
        int subjectId = RequestUtil.Instance.GetFormString("SubjectId").ToInt();
        int subjectTypeId = RequestUtil.Instance.GetFormString("SubjectTypeId").ToInt();
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("栏目修改", columnId, subjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }

        //反射获取表单字段数据
        var type = typeof(Column);
        var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId) ?? ModelFactory<Column>.Insten();
        column = GetPostObject(type, column) as Column;

        if (column.Path.IsEmpty())
            return new HandlerResult { Status = false, Message = "英文目录不能为空".ToLang() };

        if (ServiceFactory.ColumnService.IsExsitPath(column.Path, CurrentSiteId, column.ColumnId))
            return new HandlerResult { Status = false, Message = "英文目录：".ToLang() + column.Path + " 重复，请更换其他目录".ToLang() };

        if (column.ColumnId <= 0)
        {
            //产品类型栏目开启一级类别
            if (column.ModelId > 0)
            {
                var model = ServiceFactory.ModelService.SingleOrDefault<Model>(column.ModelId);
                if (model != null && model.ModuleMark.IndexOf("Product_v0.0.01", StringComparison.Ordinal) != -1)
                {
                    column.IsCategory = true;
                    column.CategoryLevel = 1;
                }
            }
            //默认栏目前台显示
            column.IsShow = true;
            column.IsCategoryShow = true;
            //默认生成动态
            column.CreateMode = 2;
            //添加栏目
            columnId = ServiceFactory.ColumnService.AddColumn(column);
        }
        else
        {
            //更新栏目
            ServiceFactory.ColumnService.UpdateColumn(column);
        }
        if (subjectId != 0)
        {
            #region 添加seo信息 到SubjectColumn表
            SubjectColumn subColumn = ServiceFactory.ColumnService.SingleOrDefault<SubjectColumn>("where ColumnId=@0 and SubjectId=@1", column.ColumnId, subjectId)
                ?? ModelFactory<SubjectColumn>.Insten();
            subColumn.MetaDesc = column.MetaDesc;
            subColumn.MetaKeyword = column.MetaKeyword;
            subColumn.MetaTitle = column.MetaTitle;
            subColumn.ColumnId = column.ColumnId;
            subColumn.SubjectId = subjectId;
            subColumn.ColumnName = column.ColumnName;
            subColumn.ColumnNameStage = column.ColumnNameStage;
            subColumn.ImageUrl = column.ImageUrl;
            subColumn.SmallImageUrl = column.SmallImageUrl;
            if (subColumn.SubjectColumnId > 0)
                ServiceFactory.SubjectColumnService.Update(subColumn);
            else
                ServiceFactory.SubjectColumnService.Insert(subColumn);
            #endregion
        }
        switch (subjectTypeId)
        {
            case 1:
                ContentHelper.SetColumnCookie("subsite_refresh_flag", "0", columnId.ToStr());
                break;
            case 2:
                ContentHelper.SetColumnCookie("subject_refresh_flag", "0", columnId.ToStr());
                break;
        }

        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    //批量保存栏目
    public HandlerResult SaveMulti()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int parentId = RequestUtil.Instance.GetFormString("ParentId").ToInt();
        int subjectClassId = RequestUtil.Instance.GetFormString("SubjectClassId").ToInt();
        string batchColumnName = RequestUtil.Instance.GetFormString("BatchColumnName");
        ServiceFactory.ColumnService.AddColumns(
            parentId,
            CurrentSiteId,
            subjectClassId,
            batchColumnName);

        ContentHelper.SetColumnCookie("subsite_refresh_flag");
        ContentHelper.SetColumnCookie("subject_refresh_flag");

        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    //移动栏目
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
        int subjectClassId = RequestUtil.Instance.GetFormString("SubjectClassId").ToInt();
        return ColumnMove(origin, target, selValue, subjectClassId);
    }

    /// <summary>
    /// 栏目移动
    /// </summary>
    /// <param name="origin">栏目源</param>
    /// <param name="target">目标源</param>
    /// <param name="selValue">移动方式</param>
    /// <param name="subjectClassId"></param>
    private HandlerResult ColumnMove(string origin, string target, string selValue, int subjectClassId)
    {
        string childNode = string.Empty;//获取子节点,这是需要移动的栏目ID字符串
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
                foreach (Column cl in listOrigin)
                {
                    alOrigin.Add(cl.ColumnId.ToStr());
                }
            }
        }
        //右侧的List集合
        foreach (string str in targets)
        {
            IList<Column> listTarget = ServiceFactory.ColumnService.GetColumnByParentId(str.ToInt(), CurrentSiteId);

            if (!alTarget.Contains(str))
            {
                foreach (Column cl in listTarget)
                {
                    alTarget.Add(cl.ColumnId.ToStr());
                }
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
        string SQL = "select max(Sort) Sort,max(ParentId) ParentId from Whir_Dev_Column WHERE IsDel=0 AND ColumnId=@0 AND SiteId=@1 AND SiteType=@2 AND (MarkType='' OR MarkType IS NULL)";
        Column targetColumn = DbHelper.CurrentDb.SingleOrDefault<Column>(SQL, targetId, CurrentSiteId, subjectClassId);
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

                    bool isMoved = false;

                    if (isMoved) continue;

                    for (int i = 0; i < parentIDs.Trim(',').Split(',').Length; i++)
                    {
                        string pids = parentIDs.Trim(',').Split(',')[i];

                        if (pids.IsEmpty()) continue;

                        if (alOrigin.Contains(pids))//已添加过
                        {
                            isMoved = true;
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
                    long beforeSortID = thisColumn.Sort;
                    if (thisColumn == null) continue;

                    bool isMoved = false;

                    if (isMoved) continue;

                    for (int i = 0; i < parentIDs.Trim(',').Split(',').Length; i++)
                    {
                        string pids = parentIDs.Trim(',').Split(',')[i];

                        if (pids.IsEmpty()) continue;

                        if (alOrigin.Contains(pids))//已添加过
                        {
                            isMoved = true;
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
                    long afterSortId = thisColumn.Sort;
                    if (thisColumn == null) continue;

                    bool isMoved = false;

                    if (isMoved) continue;

                    for (int i = 0; i < parentIDs.Trim(',').Split(',').Length; i++)
                    {
                        string pids = parentIDs.Trim(',').Split(',')[i];

                        if (pids.IsEmpty()) continue;

                        if (alOrigin.Contains(pids))//已添加过
                        {
                            isMoved = true;
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
            else
                parentIDs += column.ParentId + "," + getAllParents(column.ParentId);
        }
        return parentIDs;
    }

    /// <summary>
    /// 更改排序
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="originSort"></param>
    /// <param name="targetSort"></param>
    /// <param name="columnId"></param>
    /// <returns></returns>
    private void ModifySort(int parentId, long originSort, long targetSort, int columnId)
    {
        if (originSort > targetSort)
        {
            ServiceFactory.ColumnService.UpdateColumnSort(parentId, targetSort, columnId);
        }
        else
        {
            ServiceFactory.ColumnService.UpdateColumnSort(parentId, originSort, columnId);
        }
    }

    /// <summary>
    /// 当一个节点存在父节点则调用该方法可以修改它们的sort
    /// </summary>
    /// <param name="originId"></param>
    /// <param name="targetId"></param>
    /// <param name="columnId"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private int ModifyNoParentSort(long originId, long targetId, int columnId, ref int count)
    {
        if (originId > targetId)
        {
            count += ServiceFactory.ColumnService.UpdateColumnSort(targetId, originId, columnId);
        }
        else
        {
            count += ServiceFactory.ColumnService.UpdateColumnSort(originId, targetId, columnId);
        }
        return count;
    }
}