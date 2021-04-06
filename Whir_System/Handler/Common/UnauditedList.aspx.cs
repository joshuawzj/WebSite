using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Whir.ezEIP.Web;
using Whir.Repository;
using Whir.Framework;
using Whir.Service;
using System.Data;
using System.Text;
using Whir.Domain;
using Whir.Language;
using System.Web.Script.Serialization;


public partial class Whir_System_Handler_Common_UnauditedList : SysHandlerPageBase
{
    /// <summary>
    /// 工作流数据
    /// </summary>
    public class WorkFlowData
    {
        public int ColumnId { get; set; }
        public int SubjectId { get; set; }
        public int ActivityId { get; set; }
    }

    #region 属性

    /// <summary>
    /// 当前页码
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 每页显示数量
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 栏目字典
    /// </summary>
    protected Dictionary<int, string> DicColumns = new Dictionary<int, string>();

    #endregion

    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);

    }

    #region 列表。由于考虑到二次开发较多，这里代码大多不封装到service层

    /// <summary>
    /// 绑定列表数据
    /// </summary>
    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("54"), true);
        //url的过滤条件
        string title = Server.UrlDecode(RequestUtil.Instance.GetString("title"));
        int searchColumnId = RequestUtil.Instance.GetString("columnid").ToInt();
        string startTime = RequestUtil.Instance.GetString("startime");
        string endTime = RequestUtil.Instance.GetString("endtime");

        //页码
        PageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        PageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / PageSize + 1;

        if (startTime != "" && endTime != "")
        {
            startTime = ServiceFactory.DbService.GetColumnValue(startTime, FieldType.DateTime);
            endTime = ServiceFactory.DbService.GetColumnValue(endTime, FieldType.DateTime);
        }

        //第一节点的审核节点
        IList<int> firstActivityIdList = new List<int>();
        firstActivityIdList = DbHelper.CurrentDb.Query<int>("SELECT ActivityId FROM Whir_Ext_AuditActivity WHERE PreActivityId=0").ToList();
         
        //拼接SQL对象
        StringBuilder sbSql = new StringBuilder();

        if (CurrentUser.RolesId == 1 || CurrentUser.RolesId == 2)//开发者、超级管理员
        {
            DataTable table = DbHelper.CurrentDb.Query(@"SELECT col.ColumnId,col.ColumnName,col.WorkFlow,model.ModelID,model.TableName FROM Whir_Dev_Column col
                                INNER JOIN Whir_Dev_Model model ON col.ModelID=model.ModelID
                                WHERE col.SiteId=@0 AND col.WorkFlow>0 AND col.IsDel=0", CurrentSiteId).Tables[0];

            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    int columnId = row["ColumnId"].ToInt();
                    int workFlow = row["WorkFlow"].ToInt();
                    string columnName = row["ColumnName"].ToStr();
                    int modelId = row["ModelID"].ToInt();
                    string tableName = row["TableName"].ToStr();

                    if (!DicColumns.Keys.Contains(columnId))//报错栏目名称，绑定列表时，直接调用方法显示栏目名称，减少数据库的访问
                    {
                        DicColumns.Add(columnId, columnName);
                    }

                    if (sbSql.Length > 0)
                    {
                        sbSql.Append(" UNION ");
                    }
                    sbSql.AppendFormat(@"(SELECT {0}_PID  Id,typeid,'' ColumnName,state,'' WorkFlowName ,subjectid,{1} workflow,{2} commontitle,CreateUser,CreateDate FROM {0}  
                          WHERE typeid={3} AND State NOT IN(-1,-2) AND IsDel!=1)",
                          tableName,
                          workFlow,
                          GetShowField(modelId),
                          columnId
                          );
                    //state=0 为第一个审核节点
                }
            }
        }
        else//非开发者、超级管理员
        {
            Whir.Security.RolesService rs = new Whir.Security.RolesService();
            var list = rs.GetWorkFlowJurisdictionListByRoleId(CurrentUser.RolesId);
            List<WorkFlowData> wfdList = new List<WorkFlowData>();
            foreach (var item in list)
            {
                if (item.IsEmpty())
                    continue;
                string[] strs = item.Split('|');
                WorkFlowData data = new WorkFlowData();
                data.ColumnId = strs[3].ToInt();
                data.SubjectId = strs.Length < 5 ? 0 : strs[4].ToInt(0);
                data.ActivityId = strs[0].ToStr().Replace("workFlow", "").ToInt();
                wfdList.Add(data);
            }
            if (wfdList.Any())
            {
                DataTable table = DbHelper.CurrentDb.Query(@"SELECT col.ColumnId,col.ColumnName,col.WorkFlow,model.ModelID,model.TableName FROM Whir_Dev_Column col
                                INNER JOIN Whir_Dev_Model model ON col.ModelID=model.ModelID
                                WHERE col.SiteId=@0 AND col.WorkFlow>0 AND col.IsDel=0 and col.ColumnId in (@1)", CurrentSiteId, wfdList.Select(p => p.ColumnId)).Tables[0];
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        int columnId = row["ColumnId"].ToInt();
                        int subjectId = wfdList.Find(p => p.ColumnId == columnId).SubjectId;
                        int workFlow = row["WorkFlow"].ToInt();
                        int activityId = wfdList.Find(p => p.ColumnId == columnId).ActivityId;
                        string columnName = row["ColumnName"].ToStr();
                        int modelId = row["ModelID"].ToInt();
                        string tableName = row["TableName"].ToStr();

                        if (!DicColumns.Keys.Contains(columnId))//报错栏目名称，绑定列表时，直接调用方法显示栏目名称，减少数据库的访问
                        {
                            DicColumns.Add(columnId, columnName);
                        }

                        if (sbSql.Length > 0)
                        {
                            sbSql.Append(" UNION ");
                        }
                        sbSql.AppendFormat(@"(SELECT {0}_PID  Id,typeid,'' ColumnName,subjectid,{6} workflow,{1} state,'' WorkFlowName,{2} commontitle,CreateUser,CreateDate FROM {0}  
                          WHERE typeid={3} AND SubjectId={4}AND State={5} AND IsDel!=1)",
                              tableName,
                              activityId,
                              GetShowField(modelId),
                              columnId,
                              subjectId,
                              firstActivityIdList.Contains(activityId) ? 0 : activityId,
                              workFlow
                              );
                        //state=0 为第一个审核节点
                    }
                }
            }
        }


        if (sbSql.Length > 0)
        {
            #region sql拼接
            string newSql = "SELECT m.* FROM ({0}) m WHERE 1=1 ".FormatWith(sbSql.ToString());
            int i = 0;
            var parms = new List<object>();
            string filter = RequestUtil.Instance.GetQueryString("filter");
            Dictionary<string, string> searchDic = ToDictionary(filter);
            foreach (var kv in searchDic)
            {
                if (kv.Value.Contains("<&&<"))
                {
                    newSql += " and {0} between @{1} and @{2} ".FormatWith(kv.Key, i++, i++);
                    parms.Add(Regex.Split(kv.Value, "<&&<")[0]);
                    parms.Add(Regex.Split(kv.Value, "<&&<")[1]);
                }
                else if (kv.Key.ToLower().Contains("columnname"))
                {
                    string ids = "";
                    IList<Column> ChildList = ServiceFactory.ColumnService.GetListByColumnName(kv.Value, CurrentSiteId);
                    foreach (Column c in ChildList)
                    {
                        ids += c.ColumnId + ",";
                    }
                    ids = ids.Trim(',');
                    if (ids.Length > 0)
                    {
                        newSql += " AND typeid in(" + ids + ")";
                    }
                }
                else
                {
                    newSql += " and {0} like '%'+@{1}+'%' ".FormatWith(kv.Key, i++);
                    parms.Add(kv.Value);
                }
            }

            newSql += "  ORDER BY m.CreateDate DESC";//必须得加表别名，否则错误 
            #endregion

            var pds = DbHelper.CurrentDb.Query(PageIndex, PageSize, newSql, parms.ToArray());

            var dt = pds.Items.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                dr["WorkFlowName"] = GetAuditActivityName(dr["workflow"].ToStr(), dr["state"].ToStr());
                dr["ColumnName"] = GetColumnNameByColumnId(dr["typeid"]);
                dr["commontitle"] = dr["commontitle"].ToStr().RemoveHtml();
            }

            if (pds.TotalItems > 0)
            {
                if (pds.ItemsPerPage > 0)
                {
                    long total = pds.TotalItems;
                    string data = pds.Items.Tables[0].ToJson();
                    string json = "{{\"total\":{0},\"rows\":{1}}}".FormatWith(total, data);
                    Response.Clear();
                    Response.Write(json);
                    //Response.End();
                }
                else
                {
                    PageIndex = 1;
                    GetList();
                }
            }
            else
            {
                string data = new List<string>().ToJson();
                string json = "{{\"total\":{0},\"rows\":{1}}}".FormatWith(0, data);
                Response.Clear();
                Response.Write(json);
                //Response.End();

            }
        }
        else
        {
            string data = new List<string>().ToJson();
            string json = "{{\"total\":{0},\"rows\":{1}}}".FormatWith(0, data);
            Response.Clear();
            Response.Write(json);
            //Response.End();

        }
    }


    private Dictionary<string, string> ToDictionary(string str)
    {
        try
        {
            if (str.IsEmpty())
                return new Dictionary<string, string>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<Dictionary<string, string>>(str);
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }


    /// <summary>
    /// 返回节点名称
    /// </summary>
    /// <param name="workFolwId">工作流ID</param>
    /// <param name="activityId">节点ID</param>
    /// <returns></returns>
    public string GetAuditActivityName(string workFolwId, string activityId)
    {
        return ServiceFactory.WorkFlowService.GetAuditActivityName(workFolwId.ToInt(), activityId.ToInt());
    }

    /// <summary>
    /// 单个删除
    /// </summary>
    public HandlerResult SingleDelete()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("377"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string Key = RequestUtil.Instance.GetString("key");
        int SubjectId = RequestUtil.Instance.GetString("subjectid").ToInt(0);

        int Id = Key.Split('|')[0].ToInt();
        int ColumnId = Key.Split('|')[1].ToInt();
        if (ServiceFactory.WorkFlowService.IsCanDelete(ColumnId, SubjectId, Id, CurrentUser.RolesId, CurrentUser.LoginName))
        {
            //赋值主栏目实体
            Column c = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
            if (c != null)
            {
                Model MainModel = ServiceFactory.ModelService.SingleOrDefault<Model>(c.ModelId);
                if (MainModel != null)
                {
                    string SQL = "UPDATE {0} SET IsDel=1 WHERE {0}_PID=@0".FormatWith(MainModel.TableName);//放入回收站
                    int result = DbHelper.CurrentDb.Execute(SQL, Id);

                    return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
                }
                else
                {
                    return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
                }
            }
            else
            {
                return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
            }
        }
        else
        {
            return new HandlerResult { Status = false, Message = "操作失败，当前管理员没有此操作权限".ToLang() };
        }
    }

    /// <summary>
    ///批量事件(批量删除,批量审核,批量退审)
    /// </summary>
    public HandlerResult BatchEvent()
    {
        string cmd = RequestUtil.Instance.GetString("cmd").ToLower();
        string ids = RequestUtil.Instance.GetString("cbPosition");
        string Reason = Server.UrlDecode(RequestUtil.Instance.GetString("reason")); //退审理由


        //批量删除
        if (cmd == "del")
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("377"));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            foreach (var id in ids.Split(','))
            {
                int subjectId = id.Split('|')[4].ToInt();
                ServiceFactory.GridViewService.DeleteInfo(id.Split('|')[1].ToInt(), subjectId, false, id.Split('|')[0].ToStr(), CurrentUser.RolesId, CurrentUser.LoginName);
            }

            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

        }
        if (cmd == "passflow")
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("373"));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            if (ids.ToStr().Trim() == "") { return new HandlerResult { Status = false, Message = "参数错误".ToLang() }; }
            //批量审核通过
            foreach (var id in ids.Split(','))
            {

                IList<AuditActivity> aList = ServiceFactory.AuditActivityService.GetListBySort(id.Split('|')[2].ToInt());
                int CurrentActivityId = 0;
                if (aList.Count > 0)
                {
                    if (aList.Where(p => p.ActivityId == id.Split('|')[3].ToInt()).ToList().Count == 0)
                    {
                        CurrentActivityId = aList[0].ActivityId;//如果不属于任何节点，则使用第一个节点
                    }
                    else
                    {
                        CurrentActivityId = id.Split('|')[3].ToInt();
                    }
                }

                ServiceFactory.GridViewService.PassWorkFlow(id.Split('|')[1].ToInt(), CurrentActivityId, new int[] { id.Split('|')[0].ToInt() });
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        if (cmd == "returnflow")
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("374"));
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            if (ids.ToStr().Trim() == "") { return new HandlerResult { Status = false, Message = "参数错误".ToLang() }; }
            //批量退审 
            foreach (var id in ids.Split(','))
            {
                IList<AuditActivity> aList = ServiceFactory.AuditActivityService.GetListBySort(id.Split('|')[2].ToInt());
                int CurrentActivityId = 0;
                if (aList.Count > 0)
                {
                    if (aList.Where(p => p.ActivityId == id.Split('|')[3].ToInt()).ToList().Count == 0)
                    {
                        CurrentActivityId = aList[0].ActivityId;//如果不属于任何节点，则使用第一个节点
                    }
                    else
                    {
                        CurrentActivityId = id.Split('|')[3].ToInt();
                    }
                }

                ServiceFactory.GridViewService.ReturnWorkFlow(id.Split('|')[1].ToInt(), CurrentActivityId, new int[] { id.Split('|')[0].ToInt() }, Reason);
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }

        return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
    }

    /// <summary>
    /// 根据栏目ID获取栏目名称
    /// </summary>
    /// <param name="colId"></param>
    /// <returns></returns>
    protected string GetColumnNameByColumnId(object colId)
    {
        int columnId = colId.ToInt();
        if (DicColumns.Keys.Contains(columnId))
        {
            return DicColumns[columnId];
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 返回各种类型栏目要显示的字段
    /// </summary>
    /// <param name="modleMark"></param>
    private string GetShowField(int modelId)
    {
        string FieldName = "typeid";//用作标题字段
        FieldName = ServiceFactory.FieldService.GetWorkFlowTitle(modelId);
        return FieldName == "" ? "typeid" : FieldName;
    }


    /*
   
    /// <summary>
    /// 行命令事件
    /// </summary>
    protected void rpList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("del"))
        {
            int Id = e.CommandArgument.ToStr().Split('|')[0].ToInt();
            int ColumnId = e.CommandArgument.ToStr().Split('|')[1].ToInt();
            if (ServiceFactory.WorkFlowService.IsCanDelete(ColumnId, SubjectId, Id, CurrentUser.RolesId, CurrentUserName))
            {
                //赋值主栏目实体
                Column c = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
                if (c != null)
                {
                    Model MainModel = ServiceFactory.ModelService.SingleOrDefault<Model>(c.ModelId);
                    if (MainModel != null)
                    {
                        string SQL = "UPDATE {0} SET IsDel=1 WHERE {0}_PID=@0".FormatWith(MainModel.TableName);//放入回收站
                        int result = DbHelper.CurrentDb.Execute(SQL, Id);
                        Alert("操作成功".ToLang(), true);
                    }
                }
            }
            else
            {
                ErrorAlert("操作失败，当前管理员没有此操作权限".ToLang());
            }
        }
    }
    /// <summary>
    /// 行绑定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rpList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var lbDelete = e.Item.FindControl("lbDel") as LinkButton;
            if (null != lbDelete)
            {
                ConfirmDelete(lbDelete);
            }

            DataRowView drv = e.Item.DataItem as DataRowView;
            var ltTitle = e.Item.FindControl("ltTitle") as Literal;
            if (null != ltTitle)
            {
                int length = ConfigHelper.GetSystemConfig().ListTextLength;
                if (length <= 0) { length = 30; }
                ltTitle.Text = drv["commontitle"].ToStr().Cut(length, "...");
            }
        }
    }

    /// <summary>
    /// 批量操作
    /// </summary>
    protected void Batch_Command(object sender, CommandEventArgs e)
    {
        string cmd = e.CommandName;
        string ids = Request["cb_Position"].ToStr().Trim(',');
        //批量删除
        if (cmd == "del")
        {
            foreach (var id in ids.Split(','))
            {
                ServiceFactory.GridViewService.DeleteInfo(id.Split('|')[1].ToInt(), SubjectId, false, id.Split('|')[0].ToStr(), CurrentUser.RolesId, CurrentUserName);
            }
            Alert("操作成功".ToLang(), true);
        }
        if (cmd == "PassFlow")
        {
            if (ids.ToStr().Trim() == "") { return; }
            //批量审核通过
            foreach (var id in ids.Split(','))
            {

                IList<AuditActivity> aList = ServiceFactory.AuditActivityService.GetListBySort(id.Split('|')[2].ToInt());
                int CurrentActivityId = 0;
                if (aList.Count > 0)
                {
                    if (aList.Where(p => p.ActivityId == id.Split('|')[3].ToInt()).ToList().Count == 0)
                    {
                        CurrentActivityId = aList[0].ActivityId;//如果不属于任何节点，则使用第一个节点
                    }
                    else
                    {
                        CurrentActivityId = id.Split('|')[3].ToInt();
                    }
                }

                ServiceFactory.GridViewService.PassWorkFlow(id.Split('|')[1].ToInt(), CurrentActivityId, new int[] { id.Split('|')[0].ToInt() });
            }
            Alert("操作成功".ToLang(), true);
        }
        if (cmd == "ReturnFlow")
        {
            if (ids.ToStr().Trim() == "") { return; }
            //批量退审 
            foreach (var id in ids.Split(','))
            {
                IList<AuditActivity> aList = ServiceFactory.AuditActivityService.GetListBySort(id.Split('|')[2].ToInt());
                int CurrentActivityId = 0;
                if (aList.Count > 0)
                {
                    if (aList.Where(p => p.ActivityId == id.Split('|')[3].ToInt()).ToList().Count == 0)
                    {
                        CurrentActivityId = aList[0].ActivityId;//如果不属于任何节点，则使用第一个节点
                    }
                    else
                    {
                        CurrentActivityId = id.Split('|')[3].ToInt();
                    }
                }

                ServiceFactory.GridViewService.ReturnWorkFlow(id.Split('|')[1].ToInt(), CurrentActivityId, new int[] { id.Split('|')[0].ToInt() }, hidReturnReason.Value);
            }
            Alert("操作成功".ToLang(), true);
        }

    }
    
    
    */

    #endregion

}