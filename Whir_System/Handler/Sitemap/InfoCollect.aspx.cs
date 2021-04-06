using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Security.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using System.Data;
using Whir.Domain;


public partial class Whir_System_Handler_Sitemap_InfoCollect : SysHandlerPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetFormString("_action");
        Exec(this, action);
    }


    #region 获取站点、管理员、角色信息统计

    //获取站点信息统计
    public HandlerResult GetSiteInfoCollect()
    {
        string begintime = Request.Form["begintime"];
        string endtime = Request.Form["endtime"];

        DataSet ds = ServiceFactory.ColumnService.GetSiteList();
        DataTable dt = ds.Tables[0];
        dt.Columns.Add(new DataColumn("Count", typeof(int)));
        DataRow dr = dt.NewRow();
        foreach (DataRow row in dt.Rows)
        {
            row["Count"] = BindTotalBySiteID(row["SiteID"], begintime, endtime);
        }
         return new HandlerResult { Status = true, Message = dt.ToJson() };
    }
    /// <summary>
    /// 获取管理员信息统计
    /// </summary>
    public HandlerResult GetUserInfoCoolect()
    {
        string begintime = Request.Form["begintime"];
        string endtime = Request.Form["endtime"];

        DataSet ds = ServiceFactory.ColumnService.GetUserInfoByGroup(true);
        DataTable dt = ds.Tables[0];
        dt.Columns.Add(new DataColumn("Count", typeof(int)));
        DataRow dr = dt.NewRow();
        foreach (DataRow row in dt.Rows)
        {
            row["Count"] = BindTotalByLoginName(row["LoginName"], begintime, endtime);
        }
        return new HandlerResult { Status = true, Message = dt.ToJson() };
    }
    /// <summary>
    /// 获取角色信息统计
    /// </summary>
    public HandlerResult GetRoleInfoCollect()
    {
        string begintime = Request.Form["begintime"];
        string endtime = Request.Form["endtime"];
        DataSet ds = ServiceFactory.ColumnService.GetRoleInfoByGroup(true);
        DataTable dt = ds.Tables[0];
        dt.Columns.Add(new DataColumn("Count", typeof(int)));
        DataRow dr = dt.NewRow();
        foreach (DataRow row in dt.Rows)
        {
            row["Count"] = BindTotalByRoleID(row["RoleID"], begintime, endtime);
        }
        return new HandlerResult { Status = true, Message = dt.ToJson() };
    }

    /// <summary>
    /// 根据站点ID获取栏目对应的表名列表
    /// </summary>
    /// <param name="siteid"></param>
    protected int BindTotalBySiteID(object siteid,string begintime, string endtime )
    {
        DataTable dt = ServiceFactory.ModelService.GetTableNameBySiteId(siteid.ToInt());

        int count = 0;
        
        foreach (DataRow row in dt.Rows)
        {
            DataSet ds = ServiceFactory.ColumnService.GetTotalByTableName(row["TableName"].ToStr(), begintime, endtime, siteid.ToInt());
            if (ds != null)
            {
                count += ds.Tables[0].Rows[0]["total"].ToInt();
            }
        }
        return count;
    }

    /// <summary>
    /// 根据管理员名称获取对应的表名列表
    /// </summary>
    /// <param name="loginname"></param>
    /// <param name="begintime"> </param>
    /// <param name="endtime"> </param>
    /// <returns></returns>
    protected int BindTotalByLoginName(object loginname, string begintime, string endtime)
    {
        IList<Model> modelList = ServiceFactory.ModelService.GetTableName();

        int count = 0;
      
        foreach (Model model in modelList)
        {
            DataSet ds = ServiceFactory.ColumnService.GetTotalByTableName(model.TableName, begintime, endtime, loginname.ToStr());
            if (ds != null)
            {
                count += ds.Tables[0].Rows[0]["total"].ToInt();
            }
        }
        return count;
    }
    /// <summary>
    /// 根据角色ID获取对应的表名列表
    /// </summary>
    /// <returns></returns>
    protected int BindTotalByRoleID(object roleid, string begintime, string endtime)
    {
        IList<Model> modelList = ServiceFactory.ModelService.GetTableName();
        int count = 0;
       
        Whir.Security.UsersService us = new Whir.Security.UsersService();
        IList<Users> userList = us.GetUserByRoleId(roleid.ToInt());

        if (userList.Count <= 0) return 0;//如果查询不出数据的话则返回0条数据

        string[] loginnames = userList.Select(p => p.LoginName).ToArray();

        foreach (Model model in modelList)
        {
            DataSet ds = ServiceFactory.ColumnService.GetTotalByTableName(model.TableName, begintime, endtime, loginnames);
            if (ds != null)
            {
                count += ds.Tables[0].Rows[0]["total"].ToInt();
            }
        }

        return count;
    }



    #endregion
}