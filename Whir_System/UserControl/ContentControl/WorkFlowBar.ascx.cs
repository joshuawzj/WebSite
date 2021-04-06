/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：WorkFlowBar.aspx.cs
* 文件描述：工作流公共控件
*/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;
using Whir.ezEIP.Web;


public partial class Whir_System_UserControl_ContentControl_WorkFlowBar : Whir.ezEIP.Web.SysControlBase
{

    #region 对外属性

    /// <summary>
    ///是否显示退审节点
    /// </summary>
    public bool IsShowReturn = true;

    /// <summary>
    /// 栏目ID.通过URL参数接收
    /// </summary>
    public int ColumnId { get; set; }

    /// <summary>
    /// 根据传入的columnid，返回用于过滤列表的SQL的条件参数
    /// </summary>
    public string WhereSql
    {
        get
        {
            return ViewState["WhereSQL"].ToStr();
        }
        set
        {
            ViewState["WhereSQL"] = value;
        }
    }

    #endregion

    #region 属性
    /// <summary>
    /// 工作流主键
    /// </summary>
    private int WorkFlowId
    {
        get
        {
            return ViewState["WorkFlowId"].ToInt();
        }
        set
        {
            ViewState["WorkFlowId"] = value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ShowControls();
    }
    /// <summary>
    /// 
    /// </summary>
    private void ShowControls()
    {
        if (ColumnId == 0)
        {
            ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        }


        int activityId = RequestUtil.Instance.GetQueryInt("flowid", 0);//当前页面的工作流节点

        if (!IsPostBack)
        {
            Column model = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
            WorkFlowId = model != null ? model.WorkFlow : 0;

            if (WorkFlowId <= 0) { return; }//没有使用流程节点的，忽略下面操作

            //用于记录该工作流所有流程节点，格式"1,2,3"
            string ids = "";
            int firstActivityId = 0;//流程的第一个节点

            #region 显示链接
            #region 获取地址
            string url = Request.RawUrl.ToStr().ToLower();
            Regex urlRegex = new Regex(@"(^|\?|&)flowid=-?\d+(&|$)");//folwid是对应Whir_Ext_AuditActivity的ActivityId
            Match m = urlRegex.Match(url);
            if (m.Success)
            {
                url = url.Replace(m.Value.Replace("&", "").Replace("?", ""), "flowid=！");//！用于下面替换节点流程ID值使用，暂找不到特殊字符，先用
            }
            else
            {
                if (Request.QueryString.Count > 0)//已存在其它url参数
                {
                    url += "&flowid=！";
                }
                else
                {
                    url += "?flowid=！";
                }
            }

            urlRegex = new Regex(@"(^|\?|&)page=-?\d+(&|$)");//分页的页灵敏
            m = urlRegex.Match(url);
            if (m.Success)
            {
                url = url.Replace(m.Value.Replace("&", "").Replace("?", ""), "page=1");//默认切换时回到第一页
            }
            urlRegex = new Regex(@"(^|\?|&)filter=-?\d+(&|$)");
            m = urlRegex.Match(url);
            if (m.Success) {
                url = url.Replace(m.Value, "");//去除加载列表页筛选状态
            }
            
            #endregion

            string str = string.Format("<a href=\"{0}\" class=\"btn btn-white {3}\" id='workflow{1}'>{2}</a>", url, "-1", "审核通过".ToLang(), (activityId == 0 || activityId == -1) ? "active" : "").Replace("！", "-1");//-1代表是通过了的

            //其它节点
            List<AuditActivity> list = ServiceFactory.AuditActivityService.GetListBySort(WorkFlowId);
            for (int i = 0; i < list.Count; i++)
            {

                AuditActivity a = list[i];
                if (i == 0) { firstActivityId = a.ActivityId; }
               
                    str += "<a href=\"{0}\" class=\"btn btn-white {3}\" id='workflow{1}'>{2}</a>"
                                .FormatWith( url,a.ActivityId,a.ActivityName,(activityId == a.ActivityId) ? "active" : "").Replace("！", a.ActivityId.ToStr());
                    ids += a.ActivityId + ",";
              
            }
            ids = ids.Trim(',');

            if (IsShowReturn)
            {
                //退审
                str += string.Format("<a href=\"{0}\" class=\"btn btn-white {3}\" id='workflow{1}' >{2}</a>", url, "-2", "退审".ToLang(), (activityId == -2) ? "active" : "").Replace("！", "-2");
            }

            ltContent.Text = str;
            #endregion

            #region 对外提供过滤条件
            int currentActivityId = activityId == 0 ? -1 : activityId;
            if (currentActivityId == firstActivityId)
            {
                //把所有不是该流程节点的值都归于第一个节点，因为之前可能是A工作流，加记录后又改成B工作流后造成的
                ids = "," + ids + ",";
                ids = ids.Replace(string.Format(",{0},", firstActivityId), "").Trim(',');//不包括本身
                string noIds = (ids + ",-1,-2").Trim(',');
                WhereSql += string.Format(" AND (state NOT IN({0}) OR State IS NULL)", noIds);
            }
            else
            {
                WhereSql += string.Format(" AND state={0}", currentActivityId);
            }
            #endregion
        }
    }
    /// <summary>
    /// 提供外部调用搜索条件语句
    /// </summary>
    /// <returns></returns>
    public string GetWhereSql()
    {
        ShowControls();//必须还要绑定一次，否则取不到值GetWhereSQL()
        return WhereSql;
    }
}