using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;

using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;
using Whir.Repository;
public partial class whir_system_module_area_arealist : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 是否为开发者角色
    /// </summary>
    public bool IsDevRole = IsDevUser;

    public List<Area> Areaslist = new List<Area>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("256"));
            BindColumnList();
        }
    }

    //绑定栏目列表
    private void BindColumnList()
    {
        Areaslist = DbHelper.CurrentDb.Query<Area>("SELECT Id,Pid,ParentPath,Name,Sort FROM  dbo.Whir_Cmn_Area WHERE IsDel=0  ORDER BY Sort desc,UpdateDate DESC").ToList();

        Areaslist = GetNodeColumns(0, 1);
        //Areaslist = Areaslist.Where(p => p.Pid == 0 || p.ParentPath.Split(',').Count()<=4).ToList();
    }


    /// <summary>
    /// 获取下级栏目
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private List<Area> GetNodeColumns(int parentId, int level)
    {
        var columns = new List<Area>();
        foreach (var column in Areaslist.Where(p => p.Pid == parentId))
        {
            column.LevelNum = level;
            columns.Add(column);
            columns.AddRange(GetNodeColumns(column.Id, level + 1));
        }
        return columns;
    }

    /// <summary>
    /// 递归组装数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="parentId">父级类别ID号</param>
    /// <returns></returns>
    private void BindData(List<Area> data, int parentId)
    {

        //List<Area> newlist = data.Where(c => c.Pid == parentId).ToList();
        //foreach (Area item in newlist)
        //{
        //    list.Add(item);
        //    BindData(data, item.Id);
        //}
    }
    /// <summary>
    /// 展示数据显示方式和内容
    /// </summary>
    /// <param name="list"></param>
    private void ShowData(List<Area> list)
    {
        //Hashtable hash = new Hashtable();
        //string map = string.Empty;
        //if (list.Count > 0)
        //{
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        if (list[i].Pid == 0)
        //        {
        //            map += 0 + ",";
        //        }
        //        else
        //        {
        //            if (!hash.ContainsValue(list[i].Pid))//如果不存在则添加 2条
        //            {
        //                map += i + ",";
        //                hash.Add(i, list[i].Pid);
        //            }
        //            else
        //            {
        //                foreach (int ikey in hash.Keys)
        //                {
        //                    if (list[i].Pid == hash[ikey].ToInt())
        //                    {
        //                        map += ikey + ",";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    if (map.Length > 0)
        //    {
        //        hfStrMap.Value = map.TrimEnd(',');
        //    }
        //}
        //else
        //{
        //    ltNoRecord.Text = "无数据".ToLang();
        //}
    }

    /// <summary>
    /// 栏目列表绑定行为
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ColumnList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            Area Area = e.Item.DataItem as Area;
            if (Area == null) return;
            HyperLink hlkEdit = e.Item.FindControl("hlkEdit") as HyperLink;         //编辑
            LinkButton lbnDelete = e.Item.FindControl("lbnDelete") as LinkButton;   //删除
            PlaceHolder phCopy = e.Item.FindControl("phCopy") as PlaceHolder; //复制
            //编辑按钮链接地址
            if (hlkEdit != null)
            {

                hlkEdit.NavigateUrl = "area_edit.aspx?id=" + Area.Id + "&pid=" + Area.Pid;
            }

            //删除按钮
            if (lbnDelete != null)
            {
            }
        }
    }

    /// <summary>
    /// 列表删除事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ColumnList_Command(object sender, CommandEventArgs e)
    {
        string commandArgs = e.CommandArgument.ToStr();
        string commandName = e.CommandName.ToStr();

        if (commandName == "Delete")
        {
            //记录操作日志
            DbHelper.CurrentDb.Execute("delete dbo.Whir_Cmn_Area  WHERE Id=@0", commandArgs);
            Alert("操作成功".ToLang(), true);
        }

    }

    /// <summary>
    /// 底部批量操作点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Batch_Command(object sender, CommandEventArgs e)
    {
        try
        {
            string commandArgs = e.CommandArgument.ToStr();
            string commandName = e.CommandName.ToStr();

            switch (commandName)
            {
                case "Sort":
                    BatchSort();
                    Alert("操作成功".ToLang(), true);
                    break;
            }
        }
        catch (Exception ex)
        {
            ErrorAlert("操作失败：".ToLang() + ex.Message);
            return;
        }
    }

    //批量排序
    private void BatchSort()
    {
        //string strSort = hidSort.Value.Trim(',');
        //string[] arrSort = strSort.Split(',');
        //foreach (string str in arrSort)
        //{
        //    int id = str.Split('|')[0].ToInt();
        //    long sort = str.Split('|')[1].ToLong(0);
        //    //ServiceFactory.ColumnService.ModifyColumnSort(columnID, sort);
        //    DbHelper.CurrentDb.Execute("UPDATE Whir_Cmn_Area SET Sort=@0 WHERE Id=@1", sort, id);
        //}
    }
}