<%@ Page Language="C#" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Domain" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<script type="text/C#" runat="server">

    /// <summary>
    /// 页面加载, 处理异步请求, 根据栏目名称返回一串json字符串
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了
        int nodeid = WebUtil.Instance.GetQueryInt("nodeid", 0);
        string nodes = ServiceFactory.ColumnService.GetParentsById(nodeid);
        int siteid = WebUtil.Instance.GetQueryInt("siteid", 0);

        IList<Column> list = ServiceFactory.ColumnService.GetList(0, siteid);
        List<ColumnTree> ctList = new List<ColumnTree>();

        ColumnTree ct_nocheck = new ColumnTree();
        ct_nocheck.id = 0;
        ct_nocheck.name = "请选择要生成的栏目".ToLang();
        ct_nocheck.pId = -1;
        ct_nocheck.open = true;
        ct_nocheck.nocheck = true;
        ctList.Add(ct_nocheck);

        foreach (Column cl in list)
        {
            ColumnTree ct = new ColumnTree();
            ct.id = cl.ColumnId;
            string columnname = Regex.Replace(cl.ColumnName, "[\\W]", "");
            ct.name = columnname;//cl.ColumnName.Replace("┝", "");
            ct.pId = cl.ParentId;
            ct.open = false;//默认为false是因为还有另外两个是true的更上一级的父节点 // cl.ParentId == 0 ? true : false;
            string[] str_node = nodes.Split(',');
            foreach (string str in str_node)
            {
                if (ct.id == str.ToInt() && nodeid != str.ToInt())
                {
                    ct.open = true;
                }
            }
            ctList.Add(ct);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();

        Response.Write(jss.Serialize(ctList));
        Response.End();
    }


    /// <summary>
    /// 创建一个树形类
    /// </summary>
    class ColumnTree
    {
        public int id { get; set; }//本身Id
        public int pId { get; set; }//子Id
        public string name { get; set; }//显示名称
        public bool open { get; set; }//开放闭合显示
        public bool nocheck { get; set; }//是否显示选中框
    }

</script>
