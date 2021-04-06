using System;
using System.Collections.Generic;
using System.Web.UI;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Label;
using Whir.Label.Dynamic;


public partial class Whir_System_UserControl_LabelControl_Search : SysControlBase
{
    #region 对外属性

    /// <summary>
    /// 栏目ID
    /// </summary> 
    public string ColumnId = WebUtil.Instance.GetQueryInt("typeid", 0).ToStr();

    /// <summary>
    /// 子站Id
    /// </summary> 
    public int SubjectId = WebUtil.Instance.GetQueryInt("subjectId", 0);

    /// <summary>
    /// 其它栏目ID，用于读取同一个栏目类型其它栏目值,要求格式"栏目ID,栏目ID"
    /// </summary>
    public string OtherColumnId = "";

    /// <summary>
    /// 读取数量
    /// </summary>
    public string Count = "0";

    /// <summary>
    /// 排序语句
    /// </summary>
    public string Order { get; set; }

    /// <summary>
    /// SQL语句
    /// </summary>
    public string Sql { get; set; }

    /// <summary>
    /// 搜索关键词
    /// </summary>
    public string SearchParm = "key";

    /// <summary>
    /// 条件语句
    /// </summary>
    public string Where = "";

    /// <summary>
    /// 无数据提示文字
    /// </summary>
    public string NullTip = "";

    /// <summary>
    /// 是否需要分页
    /// </summary>
    public bool NeedPage = false;


    #endregion 对外属性

    /// <summary>
    /// 模板
    /// </summary>
    private ITemplate itemTemplate = null;
    [TemplateContainer(typeof(Search.DataContainer))]
    public ITemplate ItemTemplate
    {
        get { return itemTemplate; }
        set { itemTemplate = value; }
    }


    /// <summary>
    /// 当前页面所属栏目实体（仅为栏目的首页、列表页、内容页时有值）
    /// </summary>
    public Column PageColumn { get; set; }
    /// <summary>
    /// 当前页面所属站点实体
    /// </summary>
    public SiteInfo PageSiteInfo { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Bind();
        }
        catch (Exception ex)
        {
            Show_Error(ex, this.Controls, ID);
        }
    }

    private void Bind()
    {
        this.Controls.Clear();
        SearchParm = RequestUtil.Instance.GetString(SearchParm);
        Search queryList = new Search(ID, ColumnId.ToInt(), OtherColumnId, SubjectId, Count.ToInt(), SearchParm, Where, Order, Sql, NeedPage, this.Parent.Page, ItemTemplate, PageColumn, PageSiteInfo, this);
        List<Search.DataContainer> itemplateList;
        queryList.GetQueryList(out itemplateList);
        if (itemplateList.Count == 0)
        {
            LiteralControl lt = new LiteralControl();
            lt.Text = NullTip.IsEmpty() ? PageSiteInfo.NullTip : NullTip; //无信息时的提示

            this.Controls.Add(lt);
        }
        foreach (Search.DataContainer d in itemplateList)
        {
            this.Controls.Add(d);
        }
        DataBind();//必须绑定一次，否则前台无法调用数据
        //绑定后为子置标控件赋值PageColumn、PageSiteInfo
        LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn);
    }


}