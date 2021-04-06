<%@ Page Language="C#" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Whir.ezEIP.Web" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Domain" %>
<%@ Import Namespace="Whir.Language" %>
<script type="text/C#" runat="server">

    private int _nodeid = 0;
    private string _nodes = "";
    private int _subjectTypeId = 0;
    private int _parentId = 0;
    private int _siteid;
    /// <summary>
    /// 页面加载, 处理异步请求, 根据栏目名称返回一串json字符串
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        new Whir.ezEIP.Web.SysManagePageBase().IsHadLogin(); //判断是否登录了

        _nodeid = WebUtil.Instance.GetQueryInt("nodeid", 0);
        _nodes = ServiceFactory.ColumnService.GetParentsById(_nodeid);
        _subjectTypeId = WebUtil.Instance.GetQueryInt("subjecttypeid", 0);//子站为1，专题为2，否则为0，默认为栏目内容管理
        _siteid = WebUtil.Instance.GetQueryInt("siteid", 0);//当前站点Id
        int parentId = WebUtil.Instance.GetQueryInt("parentid", 0);

        switch (_subjectTypeId)
        {
            case 0://站点群栏目
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string json = jss.Serialize(BindColumnInfo());
                    Response.Write(json);
                    CookieUtil.Instance.RemoveCookie("column_refresh_flag");//清除刷新值，更改后的数据只刷新一次
                    Response.End();
                }
                break;
            case 1:
                {
                    var listSubjectClass = ServiceFactory.SubjectClassService.GetSubsiteClassList(_siteid).ToList();
                    var listSubjectTypeTree = BindSubjectInfo(listSubjectClass, _siteid).ToList();

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    List<SubjectTypeTree> tree = listSubjectTypeTree;   //模板子站
                    List<SubjectTypeTree> subjectTree = BindSubsiteInfo();
                    if (subjectTree != null)
                        tree.AddRange(subjectTree);   //自定义子站

                    string json = jss.Serialize(tree);
                    Response.Write(json);
                    CookieUtil.Instance.RemoveCookie("subsite_refresh_flag");
                    Response.End();
                }
                break;
            case 2://专题
                {
                    var listSubjectClass = ServiceFactory.SubjectClassService.GetSubjectClassList(_siteid).ToList();
                    var listSubjectTypeTree = BindSubjectInfo(listSubjectClass, _siteid).ToList();
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string json = jss.Serialize(listSubjectTypeTree);
                    Response.Write(json);
                    CookieUtil.Instance.RemoveCookie("subject_refresh_flag");
                    Response.End();
                }
                break;
            default:
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string json = jss.Serialize(BindColumnInfo());
                    Response.Write(json);
                    CookieUtil.Instance.RemoveCookie("column_refresh_flag");
                    Response.End();
                }
                break;
        }
    }

    /// <summary>
    /// 绑定默认栏目内容信息
    /// </summary>
    private List<ColumnTree> BindColumnInfo()
    {
        IList<Column> list = ServiceFactory.ColumnService.GetList(0, _siteid);

        List<ColumnTree> ctList = new List<ColumnTree>();

        ColumnTree ctManage = new ColumnTree();
        ctManage.id = 0;
        ctManage.name = "内容管理".ToLang();
        ctManage.pId = -2;
        ctManage.open = true;
        ctManage.modelid = 0;
        ctList.Add(ctManage);

        foreach (Column cl in list)
        {
            if (!Whir.Security.ServiceFactory.RolesService.IsRoleHaveColumnJurisdiction("查看", cl.ColumnId, Whir.Security.Service.AuthenticateHelper.User.RolesId, cl.SiteId))
                continue;
            
            ColumnTree ct = new ColumnTree();
            ct.id = cl.ColumnId;
            ct.modelid = cl.ModelId;

            string columnname = cl.ColumnName.Replace("├─", "").Replace("│", "").Replace("└─", "").Replace("　", "");

            if (SysManagePageBase.IsDevUser)
            {
                ct.name = columnname + "[" + cl.ColumnId + "]";
            }
            else
            {
                ct.name = columnname;
            }

            ct.pId = cl.ParentId;
            ct.open = false;//默认为false是因为还有另外两个是true的更上一级的父节点 // cl.ParentId == 0 ? true : false;
            ct.outLinkUrl = cl.OutUrl.ToStr();
            string[] strNode = _nodes.Split(',');
            foreach (string str in strNode)
            {
                if (ct.id == str.ToInt() && _nodeid != str.ToInt())
                {
                    ct.open = true;
                }
            }
            ctList.Add(ct);
        }

        return ctList;
    }

    /// <summary>
    /// 绑定子站根据子站、专题返回的列表返回对应的数据
    /// </summary>
    /// <param name="subjectClassList"></param>
    private List<SubjectTypeTree> BindSubjectInfo(IList<SubjectClass> subjectClassList, int siteID)
    {
        List<SubjectTypeTree> stList = new List<SubjectTypeTree>();

        foreach (SubjectClass sc in subjectClassList)
        {
            int idSubjectclass = Rand.Instance.Number(4, true).ToInt();
            int pidSubjectclass = Rand.Instance.Number(4, true).ToInt();
            SubjectTypeTree st = new SubjectTypeTree();
            st.id = sc.SubjectClassId.ToStr();
            st.name = sc.SubjectClassName;
            st.classid = sc.SubjectClassId;
            st.pId = "0";
            st.open = true;//默认为false是因为还有另外两个是true的更上一级的父节点 // cl.ParentId == 0 ? true : false;
            stList.Add(st);

            IList<Subject> subjectList = ServiceFactory.SubjectService.GetListBySubjectClassId(sc.SubjectClassId);

            ///遍历二级 Whir.Security.ServiceFactory.RolesService.IsRoleHaveMenuRes(
            foreach (Subject sj in subjectList)
            {
                if (!Whir.Security.ServiceFactory.RolesService.IsRoleHaveSubjectJurisdiction("subject", "查看", sj.SubjectId, Whir.Security.Service.AuthenticateHelper.User.RolesId, _siteid, 0))
                { continue; }

                //string idSubject = "-" + Rand.Instance.Number(4, true);//负数表明是 站点/专题 文件夹
                string idSubject = "-" + sj.SubjectId;//负数表明是 站点/专题 文件夹

                SubjectTypeTree stSecond = new SubjectTypeTree();
                stSecond.id = idSubject;
                stSecond.name = sj.SubjectName;
                stSecond.classid = sc.SubjectClassId;
                stSecond.pId = sj.SubjectClassId.ToStr();
                stSecond.open = false;
                stList.Add(stSecond);

                var listColumn = ServiceFactory.ColumnService.Query<Column>("WHERE IsDel=0 AND ParentId=@0 AND SiteId=@1 AND SiteType=@2 AND (MarkType='' OR MarkType IS NULL) ORDER BY Sort ASC", _parentId, siteID, sc.SubjectClassId).ToList();

                foreach (Column column in listColumn)
                {

                    stList.AddRange(GetChildColumn(column, idSubject, sj, false));
                }
            }
        }

        return stList;
    }
    /// <summary>
    /// 递归获取子栏目
    /// </summary>
    /// <param name="column"></param>
    /// <param name="id_subject"></param>
    /// <param name="sj"></param>
    /// <param name="issubsite"></param>
    /// <returns></returns>
    private List<SubjectTypeTree> GetChildColumn(Column column, string id_subject, Subject sj, bool issubsite)
    {
        List<SubjectTypeTree> stList = new List<SubjectTypeTree>();
        if (column.LinkSubjectClassId > 0)
        {
            SubjectClass subjectClass =
                        ServiceFactory.SubjectClassService.SingleOrDefault<SubjectClass>(column.LinkSubjectClassId);
            if (subjectClass == null)
            {
                return stList;
            }
            List<SubjectTypeTree> listSubjectTypeTree = BindColumnSubjectInfo(subjectClass, column, sj.SubjectId, id_subject, issubsite).ToList();
            if (listSubjectTypeTree.Count > 0)
            {
                var stThird = listSubjectTypeTree[0];

                stThird.id = column.ColumnId.ToStr();
                stThird.subjectid = sj.SubjectId;

                stThird.pId = id_subject.ToStr();
                stThird.classid = column.SiteType;

                stThird.modelid = column.ModelId;
                stThird.open = false;
                stThird.issubsite = issubsite;
            }
            return listSubjectTypeTree;
        }
        if (Whir.Security.ServiceFactory.RolesService.IsRoleHaveSubjectJurisdiction("subjectcolumn", "查看", column.ColumnId, Whir.Security.Service.AuthenticateHelper.User.RolesId, column.SiteId, sj.SubjectId))
        {
            IList<Column> listChildColumn = ServiceFactory.ColumnService.GetListByParentId(column.ColumnId, column.SiteId);//查询是否存在子栏目


            //模板子站,则需要读取别名，若不存在别名就读本身名
            string columnAlias = ServiceFactory.SubjectColumnService.GetColumnName(column.ColumnId, sj.SubjectId).ToStr();
            column.ColumnName = columnAlias == "" ? column.ColumnName : columnAlias;//c.ColumnName;

            int pidSubject = Rand.Instance.Number(4, true).ToInt();//定义这个变量为了节点无重复，避免节点数据出错
            SubjectTypeTree st_third = new SubjectTypeTree();

            //判断如果存在子栏目,则加一个字符分割，标识该栏目属于哪个父级站点的
            //这是因为JZree一个pid只能对应一个id的原因，不能同时存在两个相同的id
            if (listChildColumn.Count > 0)
            {
                st_third.id = column.ColumnId + "-" + pidSubject;
            }
            else
            {
                st_third.id = column.ColumnId.ToStr();
                st_third.subjectid = sj.SubjectId;
            }
            st_third.pId = id_subject.ToStr();
            st_third.classid = column.SiteType;

            if (SysManagePageBase.IsDevUser)
            {
                //开发者才显示栏目id
                st_third.name = column.ColumnName + "[" + st_third.id + "]";
            }
            else
            {
                st_third.name = column.ColumnName;// +"[" + st_third.id + "]";
            }

            st_third.modelid = column.ModelId;
            st_third.open = false;
            st_third.issubsite = issubsite;
            stList.Add(st_third);

            foreach (Column cl in listChildColumn)
            {
                if (!Whir.Security.ServiceFactory.RolesService.IsRoleHaveSubjectJurisdiction("subjectcolumn", "查看", cl.ColumnId, Whir.Security.Service.AuthenticateHelper.User.RolesId, cl.SiteId, sj.SubjectId)) 
                { continue; }
                List<SubjectTypeTree> clList = GetChildColumn(cl, cl.ParentId + "-" + pidSubject, sj, issubsite);
                if (clList.Count == 1)//以此判断没有子栏目了
                {
                    SubjectTypeTree stFourth = new SubjectTypeTree();
                    stFourth.id = cl.ColumnId.ToStr();
                    stFourth.pId = cl.ParentId + "-" + pidSubject;
                    stFourth.subjectid = sj.SubjectId;
                    if (SysManagePageBase.IsDevUser)
                    {
                        //开发者才显示栏目id
                        stFourth.name = cl.ColumnName + "[" + cl.ColumnId + "]";
                    }
                    else
                    {
                        stFourth.name = cl.ColumnName;
                    }

                    stFourth.modelid = cl.ModelId;
                    stFourth.classid = sj.SubjectClassId;
                    stFourth.open = false;
                    stFourth.issubsite = issubsite;
                    stList.Add(stFourth);
                }
                else
                {
                    stList.AddRange(clList);
                }
            }
        }
        return stList;
    }

    /// <summary>
    /// 自定义子站
    /// </summary>
    private List<SubjectTypeTree> BindSubsiteInfo()
    {
        var listSubject = ServiceFactory.SubjectService.GetListBySubjectClassId(0).Where(p => p.SiteId == _siteid);

        //如果自定义子站没有栏目，不显示相关信息
        if (listSubject.Count() == 0)
        {
            return null;
        }
        List<SubjectTypeTree> stList = new List<SubjectTypeTree>();

        SubjectTypeTree ct_manage = new SubjectTypeTree();
        ct_manage.id = "0";
        ct_manage.name = "自定义子站".ToLang();
        ct_manage.pId = "-2";
        ct_manage.open = true;
        ct_manage.issubsite = true;
        stList.Add(ct_manage);

        foreach (Subject sj in listSubject)
        {
            string idSubject = "-" + Rand.Instance.Number(4, true);

            SubjectTypeTree stSecond = new SubjectTypeTree();
            stSecond.id = idSubject;//负数表名是文件夹
            stSecond.name = sj.SubjectName;
            stSecond.pId = "0";
            stSecond.classid = sj.SubjectId;
            stSecond.open = false;
            stSecond.issubsite = true;
            stList.Add(stSecond);

            var listColumn = ServiceFactory.ColumnService.Query<Column>("WHERE IsDel=0 AND ParentId=@1 AND SiteType=@0 AND IsCustomSubsite=1 AND (MarkType='' OR MarkType IS NULL) ORDER BY Sort ASC", sj.SubjectId, _parentId).ToList();


            //检查栏目是否有查看功能，若没有则飞过
            foreach (Column column in listColumn)
            {
                stList.AddRange(GetChildColumn(column, idSubject, sj, true));


            }

        }

        return stList;
    }

    /// <summary>
    /// 绑定子站根据子站、专题返回的列表返回对应的数据
    /// </summary>
    private List<SubjectTypeTree> BindColumnSubjectInfo(SubjectClass subjectClass, Column column, int subjectId, string id_subject, bool issubsite)
    {
        var stList = new List<SubjectTypeTree>();

        int idSubjectclass = Rand.Instance.Number(4, true).ToInt();
        int pidSubjectclass = Rand.Instance.Number(4, true).ToInt();
        var st = new SubjectTypeTree();
        st.id = column.ColumnId.ToStr() + "-" + pidSubjectclass.ToStr();
        st.name = subjectClass.SubjectClassName;
        st.classid = subjectClass.SubjectClassId;
        st.pId = idSubjectclass.ToStr();
        st.open = false; //默认为false是因为还有另外两个是true的更上一级的父节点 // cl.ParentId == 0 ? true : false;
        stList.Add(st);

        IList<Subject> subjectList = ServiceFactory.SubjectService.GetListBySubjectClassId(subjectClass.SubjectClassId);

        //遍历二级
        foreach (Subject sj in subjectList)
        {
            if (!Whir.Security.ServiceFactory.RolesService.IsRoleHaveSubjectJurisdiction("subject", "查看", sj.SubjectId, Whir.Security.Service.AuthenticateHelper.User.RolesId, _siteid, 0)) { continue; }
             
            int idSubject = Rand.Instance.Number(4, true).ToInt();

            var stSecond = new SubjectTypeTree();
            stSecond.id = idSubject.ToStr();
            stSecond.name = sj.SubjectName;
            stSecond.classid = subjectClass.SubjectClassId;
            stSecond.pId = column.ColumnId.ToStr();
            stSecond.open = false;
            stSecond.issubsite = issubsite;
            stList.Add(stSecond);

            List<Column> listColumn =
                ServiceFactory.ColumnService.Query<Column>(
                    "WHERE IsDel=0 AND ParentId=@0 AND SiteId=@1 AND SiteType=@2 AND (MarkType='' OR MarkType IS NULL) ORDER BY Sort ASC",
                    _parentId, column.SiteId, subjectClass.SubjectClassId).ToList();

            foreach (Column col in listColumn)
            {
                stList.AddRange(GetChildColumn(col, idSubject.ToStr(), sj, false));
            }
        }

        return stList;
    }

    /// <summary>
    /// 创建一个栏目树形类
    /// </summary>
    private class ColumnTree
    {
        public int id { get; set; } //本身Id
        public int pId { get; set; } //子Id
        public string name { get; set; } //显示名称
        public bool open { get; set; } //开放闭合显示
        public int modelid { get; set; } //判断如果modelid=0，单击不跳转，右击没显示
        public int isparent { get; set; }//是否有子栏目
        public bool nocheck { get; set; }//是否显示复选框
        /// <summary>
        /// 外部链接地址，以有值判断为是外部链接栏目
        /// </summary>
        public string outLinkUrl { get; set; }
    }

    /// <summary>
    /// 创建一个专题子站的树形类
    /// </summary>
    private class SubjectTypeTree
    {
        public string id { get; set; } //本身Id
        public string pId { get; set; } //子Id
        public string name { get; set; } //显示名称
        public bool open { get; set; } //开放闭合显示
        public int subjectid { get; set; } //自定义一个subjectid，用来判断权限
        public int modelid { get; set; } //判断如果modelid=0，单击不跳转，右击没显示
        public int classid { get; set; } //专题子站分类
        public bool issubsite { get; set; } //是否专题栏目
    }

  
    
</script>
