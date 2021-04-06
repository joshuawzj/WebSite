using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Language;
using System.Collections.Generic;
using Whir.Repository;


public partial class Whir_System_Handler_Common_Sort : SysHandlerPageBase
{
    /// <summary>
    /// 栏目ID
    /// </summary>
    public int ColumnId { get; set; }


    /// <summary>
    /// 显示对应的子站/专题记录。若不是子站或专题则值为0
    /// </summary>
    public int SubjectId { get; set; }



    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetString("ColumnId").ToInt(0);

        SubjectId = RequestUtil.Instance.GetString("SubjectId").ToInt(0); 

        var action = RequestUtil.Instance.GetString("_action");

        Exec(this, action);


    }
     
    //内容管理列表 拖拽排序
    public HandlerResult SortDrag()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsRoleHaveColumnRes("排序", ColumnId, SubjectId == 0 ? -1 : SubjectId));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var ids = RequestUtil.Instance.GetFormString("Ids").TrimEnd(',');
        List<int> list = Array.ConvertAll(ids.Split(','), int.Parse).ToList();

        //赋值主栏目实体
        var mainColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
        var mainModel = new Model();
        //赋值主模型实体
        if (mainColumn != null)
            mainModel = ServiceFactory.ModelService.SingleOrDefault<Model>(mainColumn.ModelId);
        //1. 表名 & 主键
        string queryTableName = mainModel.TableName;
        string queryPrimaryKey = queryTableName + "_PID";
        string sql = "select top 1 sort from {0} where TypeId = @0 AND Isdel=0 AND {1} in (@1) order by Sort ".FormatWith(queryTableName, queryPrimaryKey);
         
        var minSort = DbHelper.CurrentDb.SingleOrDefault<decimal>(sql, ColumnId, list.ToArray()); //int 会超出数组大小的 sort在数据库存放时bigint
        foreach (var id in list)
        {
            sql = "Update {0} Set Sort=@1 WHERE {1}=@0".FormatWith(queryTableName, queryPrimaryKey);
            DbHelper.CurrentDb.Execute(sql, id, minSort - 1 + (list.Count - list.IndexOf(id)));
        }

        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
         
    }

    //会员组列表 拖拽排序
    public HandlerResult SortMemberGroup()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("302"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var ids = RequestUtil.Instance.GetFormString("Ids").TrimEnd(',');
        List<int> list = Array.ConvertAll(ids.Split(','), int.Parse).ToList();

        string sql = "select top 1 sort from Whir_Mem_MemberGroup where Isdel=0 AND GroupId in (@1) order by Sort ";

        var minSort = DbHelper.CurrentDb.SingleOrDefault<decimal>(sql, ColumnId, list.ToArray()); //int 会超出数组大小的 sort在数据库存放时bigint
        foreach (var id in list)
        {
            sql = "Update Whir_Mem_MemberGroup Set Sort=@1 WHERE GroupId=@0";
            DbHelper.CurrentDb.Execute(sql, id, minSort - 1 + (list.Count - list.IndexOf(id)));
        }

        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
 
    }

    //管理员列表 拖拽排序
    public HandlerResult SortUser()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("380"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var ids = RequestUtil.Instance.GetFormString("Ids").TrimEnd(',');
        List<int> list = Array.ConvertAll(ids.Split(','), int.Parse).ToList();

        string sql = "select top 1 sort from Whir_Sec_Users where Isdel=0 AND UserId in (@1) order by Sort ";

        var minSort = DbHelper.CurrentDb.SingleOrDefault<decimal>(sql, ColumnId, list.ToArray()); //int 会超出数组大小的 sort在数据库存放时bigint
        foreach (var id in list)
        {
            sql = "Update Whir_Sec_Users Set Sort=@1 WHERE UserId=@0";
            DbHelper.CurrentDb.Execute(sql, id, minSort - 1 + (list.Count - list.IndexOf(id)));
        }

        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
 
    }
}