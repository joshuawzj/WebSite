using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Security.Domain;
using Whir.Service;
using Whir.Language;

public partial class whir_system_Handler_Developer_Menu : SysHandlerPageBase
{
    SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected List<SubjectColumn> SubjectColumnList = new List<SubjectColumn>();
    protected List<Column> ColumnList = new List<Column>();
    protected Dictionary<string, List<string>> RoleAllRes = new Dictionary<string, List<string>>();

    protected List<Menu> AllMenu = new List<Menu>();

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
        int menuId = RequestUtil.Instance.GetFormString("MenuId").ToInt();

        //反射获取表单字段数据
        var type = typeof(Menu);
        var menu = ServiceFactory.MenuService.SingleOrDefault<Menu>(menuId) ?? ModelFactory<Menu>.Insten();
        menu = GetPostObject(type, menu) as Menu;

        ServiceFactory.MenuService.Save(menu);
        ClearMenuCookies();
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
        int menuId = RequestUtil.Instance.GetFormString("MenuId").ToInt();
        var menu = ServiceFactory.MenuService.SingleOrDefault<Menu>(menuId);
        if (menu == null)
            return new HandlerResult { Status = false, Message = "要删除的菜单数据不存在".ToLang() };

        var subMenus = ServiceFactory.MenuService.Query<Menu>("WHERE ParentId=@0", menuId).ToList();
        if (subMenus.Any())
            return new HandlerResult { Status = false, Message = "不可删除含有下级的菜单".ToLang() };

        ServiceFactory.MenuService.Delete(menu);

        ClearMenuCookies();

        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
    }
    /// <summary>
    /// 清除菜单缓存
    /// </summary>
    public void ClearMenuCookies()
    {
        string refreshName = "menu_refresh_flag{0}".FormatWith(0);
        CookieUtil.Instance.SetCookie(refreshName, "1", "1");
        refreshName = "menu_refresh_flag{0}".FormatWith(1);
        CookieUtil.Instance.SetCookie(refreshName, "1", "1");
        refreshName = "menu_refresh_flag{0}".FormatWith(406);
        CookieUtil.Instance.SetCookie(refreshName, "1", "1");
        refreshName = "menu_refresh_flag{0}".FormatWith(390);
        CookieUtil.Instance.SetCookie(refreshName, "1", "1");
        refreshName = "menu_refresh_flag{0}".FormatWith(6);
        CookieUtil.Instance.SetCookie(refreshName, "1", "1");
    }

    /// <summary>
    /// 获取栏目菜单
    /// </summary>
    /// <returns></returns>
    public HandlerResult GetColumnMenu()
    {
        var siteId = CurrentSiteId;
        var subjectTypeId = RequestUtil.Instance.GetFormString("subjecttypeid").ToInt(0);//子站为1，专题为2，否则为0，默认为栏目内容管理
        string html = "";
        string sessionName = "";
        string refreshName = "";

        try
        {
            switch (subjectTypeId)
            {
                case 0://内容管理
                    {
                        sessionName = "columnSessionHtml";
                        refreshName = "column_refresh_flag";
                        html = Session[sessionName] == null ? "" : Session[sessionName].ToStr();
                        if (html == "" || CookieUtil.Instance.GetCookieValue(refreshName, "0") != "")
                        {
                            GetRoleAllRes();
                            ColumnList = ServiceFactory.ColumnService.GetList().ToList();
                            html = "<ul  class=\"topnav menu-left-nest\">";
                            html += "<li><a style=\"border-left: 0px solid!important;\" class=\"title-menu-left\">";
                            html += "<span>" + "内容管理".ToLang() + "</span></a></li>";
                            html += GetColumnMenuHtml(0, siteId, 0, 0, 1);
                            html += " </ul>";
                        }
                    }
                    break;
                case 1://子站
                    {
                        sessionName = "subSiteSessionHtml";
                        refreshName = "subsite_refresh_flag";
                        html = Session[sessionName] == null ? "" : Session[sessionName].ToStr();
                        if (html == "" || CookieUtil.Instance.GetCookieValue(refreshName, "0") != "")
                        {
                            GetRoleAllRes();
                            ColumnList = ServiceFactory.ColumnService.GetList().ToList();
                            SubjectColumnList = ServiceFactory.SubjectColumnService.GetList().ToList();
                            html = "<ul  class=\"topnav menu-left-nest\">";
                            html += "<li><a  style=\"border-left: 0px solid!important;\" class=\"title-menu-left\">";
                            html += "<span>" + "子站管理".ToLang() + "</span></a></li>";
                            html += GetSubSiteMenuHtml(siteId);
                            html += " </ul>";
                        }
                    }
                    break;
                case 2://专题
                    {

                        sessionName = "subjectSessionHtml";
                        refreshName = "subject_refresh_flag";
                        html = Session[sessionName] == null ? "" : Session[sessionName].ToStr();
                        if (html == "" || CookieUtil.Instance.GetCookieValue(refreshName, "0") != "")
                        {
                            GetRoleAllRes();
                            ColumnList = ServiceFactory.ColumnService.GetList().ToList();
                            SubjectColumnList = ServiceFactory.SubjectColumnService.GetList().ToList();
                            html = "<ul  class=\"topnav menu-left-nest\">";
                            html += "<li><a  style=\"border-left: 0px solid!important;\" class=\"title-menu-left\">";
                            html += "<span>" + "专题管理".ToLang() + "</span></a></li>";
                            html += GetSubjectMenuHtml(siteId);
                            html += " </ul>";
                        }
                    }
                    break;
            }
            CookieUtil.Instance.RemoveCookie(refreshName);
            Session[sessionName] = html;
            return new HandlerResult
            {
                Status = true,
                Message = html
            };
        }
        catch (Exception ex)
        {
            LogHelper.Log(ex);
            return new HandlerResult
            {
                Status = false,
                Message = "获取数据失败！".ToLang()
            };
        }
    }

    /// <summary>
    /// 获取菜单
    /// </summary>
    /// <returns></returns>
    public HandlerResult GetMenu()
    {
        var type = RequestUtil.Instance.GetFormString("type").ToInt(0);// 0=功能模块、1=系统设置、id=开发菜单、id=商城菜单、id=公众号菜单 
        string sessionName = "menuSessionHtml{0}".FormatWith(type);
        string refreshName = "menu_refresh_flag{0}".FormatWith(type);
        int cutLength = 25;
        try
        {
            var html = "";
            switch (type)
            {
                case 0://功能模块 
                    {

                        html = Session[sessionName] == null ? "" : Session[sessionName].ToStr();
                        if (html == "" || CookieUtil.Instance.GetCookieValue(refreshName, "0") != "")
                        {
                            html = "";
                            GetRoleAllRes();
                            AllMenu = ServiceFactory.MenuService.GetList().ToList();
                            foreach (var rootMenu in AllMenu.Where(p => p.ParentId == 0 && p.MenuType == "left" && p.IsShow).OrderBy(p => p.Sort).ThenBy(p => p.CreateDate))
                            {
                                //一级菜单
                                if (rootMenu.IsShow && IsRoleHaveRes("menu", "", 0, 0, 0, rootMenu.MenuId))
                                {
                                    var menuName = rootMenu.MenuName.ToLang().Cut(cutLength, "...");
                                    html += "<ul  class=\"topnav menu-left-nest\">";
                                    html += "<li><a style=\"border-left: 0px solid!important;\" class=\"title-menu-left\">";
                                    html += "<span title='{1}'>{0}</span></a></li>".FormatWith(menuName, rootMenu.MenuName.ToLang());
                                    html += GetMenuHtml(rootMenu.MenuId);
                                    html += " </ul>";
                                }
                            }
                        }
                    }
                    break;
                case 1:// 系统设置 
                    {

                        html = Session[sessionName] == null ? "" : Session[sessionName].ToStr();
                        if (html == "" || CookieUtil.Instance.GetCookieValue(refreshName, "0") != "")
                        {
                            html = "";
                            GetRoleAllRes();
                            AllMenu = ServiceFactory.MenuService.GetList().ToList();
                            foreach (var rootMenu in AllMenu.Where(p => p.ParentId == 0 && p.MenuType == "right" && p.IsShow).OrderBy(p => p.Sort).ThenBy(p => p.CreateDate))
                            {
                                //一级菜单
                                if (rootMenu.IsShow && IsRoleHaveRes("menu", "", 0, 0, 0, rootMenu.MenuId))
                                {
                                    var menuName = rootMenu.MenuName.ToLang().Cut(cutLength, "...");
                                    html += "<ul  class=\"topnav menu-left-nest\">";
                                    html += "<li><a style=\"border-left: 0px solid!important;\" class=\"title-menu-left\">";
                                    html += "<span title='{1}'>{0}</span></a></li>".FormatWith(menuName, rootMenu.MenuName.ToLang());
                                    html += GetMenuHtml(rootMenu.MenuId);
                                    html += " </ul>";
                                }
                            }
                        }
                    }
                    break;
                default://商城、公众号
                    {
                        html = Session[sessionName] == null ? "" : Session[sessionName].ToStr();
                        if (html == "" || CookieUtil.Instance.GetCookieValue(refreshName, "0") != "")
                        {
                            html = "";
                            GetRoleAllRes();
                            AllMenu = ServiceFactory.MenuService.GetList().ToList();
                            foreach (var rootMenu in AllMenu.Where(p => p.ParentId == type && p.IsShow).OrderBy(p => p.Sort).ThenBy(p => p.CreateDate))
                            {
                                //一级菜单
                                if (rootMenu.IsShow && IsRoleHaveRes("menu", "", 0, 0, 0, rootMenu.MenuId))
                                {
                                    var menuName = rootMenu.MenuName.ToLang().Cut(cutLength, "...");
                                    html += "<ul  class=\"topnav menu-left-nest\">";
                                    html += "<li><a style=\"border-left: 0px solid!important;\" class=\"title-menu-left\">";
                                    html += "<span title='{1}'>{0}</span></a></li>".FormatWith(menuName, rootMenu.MenuName.ToLang());
                                    html += GetMenuHtml(rootMenu.MenuId);
                                    html += " </ul>";
                                }
                            }
                        }
                    }
                    break;
            }
            CookieUtil.Instance.RemoveCookie(refreshName);
            Session[sessionName] = html;
            return new HandlerResult
            {
                Status = true,
                Message = html
            };
        }
        catch (Exception ex)
        {
            LogHelper.Log(ex);
            return new HandlerResult
            {
                Status = false,
                Message = "获取数据失败！".ToLang()
            };
        }
    }

    /// <summary>
    ///  递归获取菜单 
    /// </summary>
    /// <param name="parentId">父节点</param>
    /// <returns></returns>
    protected string GetMenuHtml(int parentId)
    {
        var temp = "";

        var subMenuList = AllMenu.Where(p => p.ParentId == parentId).OrderBy(p => p.Sort).ToList();
        if (subMenuList.Count == 0)
            return temp;
        else
        {
            foreach (var item in subMenuList)
            {
                if (!item.IsShow || !IsRoleHaveRes("menu", "", 0, 0, 0, item.MenuId))
                    continue;
                var menuName = item.MenuName.ToLang().Cut(20, "...");
                string pyname = SplitWordUtil.Instance.ConvertAllSpell(item.MenuName.ToLang()).ToLower();
                string szmname = SplitWordUtil.Instance.GetChineseSpell(item.MenuName.ToLang()).ToLower();
                temp += "<li><a id=\"menu{0}\" pyname=\"{2}\" szmname=\"{3}\" class=\"tooltip-tip ajax-load tooltipster-disable\" href=\"{1}\"".FormatWith(item.MenuId, SysPath + item.Url, pyname, szmname);
                temp += "title=\"{0}\"><i class=\"{1}\"></i><span>{2}</span></a></li>".FormatWith(item.MenuName.ToLang(), item.MenuIcon, menuName);

            }
        }
        return temp;
    }

    /// <summary>
    ///  递归获取菜单 内容管理
    /// </summary>
    /// <param name="parentId">父节点</param>
    /// <param name="siteId">站点id</param>
    /// <param name="siteType">子站、专题</param>
    /// <param name="subjectId">子站id</param>
    /// <param name="depth">递归深度</param>
    /// <returns></returns>
    protected string GetColumnMenuHtml(int parentId, int siteId, int siteType, int subjectId, int depth)
    {
        int cutLength = 9;
        string html = "";
        string temp = "";
        //子菜单按钮：查看前台1、发布2、栏目设置3(基本信息3、高级设置4、seo设置5)、外链6、表单管理7
        string subMenuButton = "";
        var list = ColumnList.Where(p => !p.IsDel && p.ParentId == parentId && p.SiteId == siteId && p.SiteType == siteType && p.MarkParentId == 0).OrderBy(p => p.Sort).ToList();
        foreach (var column in list)
        {
            if (subjectId > 0)
            {
                if (!IsRoleHaveRes("subjectcolumn", "查看", column.ColumnId, siteId, subjectId, 0)) continue; //没有查看栏目权限
            }
            else
            {
                if (!IsRoleHaveRes("column", "查看", column.ColumnId, siteId, subjectId, 0)) continue; //没有查看栏目权限
            }
            string herf = (SysPath + "ModuleMark/Common/Redirect.aspx?columnid={0}{1}&time=" + Rand.Instance.Number(7))
                                        .FormatWith(column.ColumnId, subjectId == 0 ? "" : "&subjectid=" + subjectId);//判断是否子站下的栏目，是就加上子站id

            if (column.ModelId != 0)
                subMenuButton += "1,";
            if (IsRoleHaveRes("menu", "发布页面", column.ColumnId, siteId, subjectId, 363))
                subMenuButton += "2,";
            if (SysManagePageBase.IsDevUser)
            {
                if (column.ModelId != 0)
                    subMenuButton += "3,7,";
                else
                    subMenuButton += "3,";
            }
            else
            {
                if (IsRoleHaveRes(subjectId > 0 ? "subjectcolumn" : "column", "栏目修改", column.ColumnId, siteId, subjectId, 0))
                    subMenuButton += "3,";
            }
            if (!column.OutUrl.IsEmpty())
            {
                herf = column.OutUrl; //如果是外链
                subMenuButton = "6";
            }

            temp = GetColumnMenuHtml(column.ColumnId, siteId, siteType, subjectId, depth + 1);
            var subjectColumn = SubjectColumnList.FirstOrDefault(p => p.ColumnId == column.ColumnId && p.SubjectId == subjectId);
            string columnName = subjectColumn != null ? subjectColumn.ColumnName : column.ColumnName;
            columnName = columnName.IsEmpty() ? column.ColumnName : columnName;
            string name = columnName.Length > cutLength - depth ? columnName.Cut(cutLength - depth, "...") : columnName;
            string pyname = SplitWordUtil.Instance.ConvertAllSpell(columnName).ToLower();
            string szmname = SplitWordUtil.Instance.GetChineseSpell(columnName).ToLower();

            if (temp != "")  //有子栏目
            {
                subMenuButton += "2";
                html += "<li><a id=\"column{2}-{3}\" class=\"tooltip-tip{0} ajax-load tooltipster-disable\" href=\"{1}\"".FormatWith(depth == 1 ? "" : depth.ToStr(), "javascript:;", column.ColumnId, subjectId);
                html += "title=\"{2}\" pyname=\"{3}\" szmname=\"{4}\" path=\"{5}\" ><span>{0}{1}</span>".FormatWith(name, SysManagePageBase.IsDevUser ? "[" + column.ColumnId + "]" : "", columnName, pyname, szmname,column.Path.ToLower());
                html += "<span class=\"caret\" style=\"border-width:7px !important;\"  buttonIds=\"{0}\" columnId=\"{1}\" subjectId=\"{2}\" classId=\"{3}\" style=\"cursor:pointer;\"></span></a>"
                    .FormatWith(subMenuButton, column.ColumnId, subjectId, siteType);
                html += "<ul>";
                html += temp;
                html += "</ul></li>";
            }
            else
            {
                if (!column.OutUrl.IsEmpty())
                    html += "<li><a class=\"tooltip-tip{0} ajax-load tooltipster-disable\" href=\"javascript:;\" onclick=\"javascript:window.open('{1}')\"".FormatWith(depth == 1 ? "" : depth.ToStr(), herf);
                else
                    html += "<li><a class=\"tooltip-tip{0} ajax-load tooltipster-disable\" href=\"javascript:;\" onclick=\"javascript:window.location.href='{1}'\"".FormatWith(depth == 1 ? "" : depth.ToStr(), herf);

                html += "title=\"{2}\" pyname=\"{3}\" szmname=\"{4}\" path=\"{5}\"><span>{0}{1}</span>".FormatWith(name, SysManagePageBase.IsDevUser ? "[" + column.ColumnId + "]" : "", columnName, pyname, szmname,column.Path.ToLower());
                html += "<span class=\"caret\" style=\"border-width:7px !important;\"   buttonIds=\"{0}\" columnId=\"{1}\" subjectId=\"{2}\" classId=\"{3}\"  style=\"cursor:pointer;\"></span></a>"
                    .FormatWith(subMenuButton, column.ColumnId, subjectId, siteType);
                html += "</li>";
            }
            subMenuButton = "";
        }

        return html;
    }

    ///<summary>
    /// 递归获取菜单 子站
    /// </summary>
    /// <param name="columnId"></param>
    /// <param name="siteId"></param>
    /// <param name="siteType"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    protected string GetSubSiteMenuHtml(int siteId)
    {
        string html = "";
        var listSubjectClass = ServiceFactory.SubjectClassService.GetSubsiteClassList(siteId).ToList();

        foreach (SubjectClass sc in listSubjectClass)
        {
            var subjectList = ServiceFactory.SubjectService.GetListBySubjectClassId(sc.SubjectClassId);
            if (subjectList.Count > 0)
            {
                html += "<li><a id=\"class{2}\"  class=\"tooltip-tip{0} ajax-load tooltipster-disable\" href=\"{1}\"".FormatWith("", "javascript:;", sc.SubjectClassId);
                html += "title=\"{0}\"><span>{0}</span></a>".FormatWith(sc.SubjectClassName);
                html += "<ul>";

                foreach (var subject in subjectList)
                {
                    if (!IsRoleHaveRes("subject", "查看", subject.SubjectId, siteId, subject.SubjectId, 0)) continue; //没有查看栏目权限
                    var listColumn = ColumnList.Where(p => !p.IsDel && p.ParentId == 0 && p.SiteId == siteId && p.SiteType == sc.SubjectClassId).OrderBy(p => p.Sort).ToList();

                    string name = subject.SubjectName.Length > 10 ? subject.SubjectName.Substring(0, 7) + "..." : subject.SubjectName;

                    if (listColumn.Count > 0)
                    {

                        html += "<li><a  id=\"subsite{2}\"  class=\"tooltip-tip{0} ajax-load tooltipster-disable\" href=\"{1}\"".FormatWith(2, "javascript:;", subject.SubjectId); //三级菜单
                        html += "title=\"{2}\" ><span>{0}{1}</span>".FormatWith(name, SysManagePageBase.IsDevUser ? "[" + subject.SubjectId + "]" : "", subject.SubjectName);
                        html += "<span class=\"caret\" style=\"border-width:7px !important;\"  buttonIds=\"{0}\" columnId=\"{1}\" subjectId=\"{2}\" classId=\"{3}\" style=\"cursor:pointer;\"></span></a>"
                                        .FormatWith("1,2", subject.SubjectId * -1, subject.SubjectId, sc.SubjectClassId);
                        html += "<ul>";
                        html += GetColumnMenuHtml(0, siteId, sc.SubjectClassId, subject.SubjectId, 3); //递归子站栏目 下的子栏目
                        html += "</ul></li>";
                    }
                    else
                    {
                        name = sc.SubjectClassName.Length > 10 ? sc.SubjectClassName.Substring(0, 7) + "..." : sc.SubjectClassName;
                        html += "<li><a class=\"tooltip-tip{0} ajax-load tooltipster-disable\" href=\"{1}\"".FormatWith(2, "javascript:;");
                        html += "title=\"{0}\"  ><span>{1}</span></a>".FormatWith(sc.SubjectClassName, name);

                    }
                }
                html += "</ul></li>";
            }
            else
            {
                html += "<li><a class=\"tooltip-tip{0} ajax-load tooltipster-disable\" href=\"{1}\"".FormatWith("", "javascript:;");
                html += "title=\"{0}\"><span>{0}</span></a></li>".FormatWith(sc.SubjectClassName);

            }
        }
        return html;
    }

    ///<summary>
    /// 递归获取菜单 专题
    /// </summary>
    /// <param name="columnId"></param>
    /// <param name="siteId"></param>
    /// <param name="siteType"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    protected string GetSubjectMenuHtml(int siteId)
    {
        string html = "";
        var listSubjectClass = ServiceFactory.SubjectClassService.GetSubjectClassList(siteId).ToList();
        foreach (SubjectClass sc in listSubjectClass)
        {
            var subjectList = ServiceFactory.SubjectService.GetListBySubjectClassId(sc.SubjectClassId);
            if (subjectList.Count > 0)
            {
                html += "<li><a id=\"class{2}\" class=\"tooltip-tip{0} ajax-load tooltipster-disable\" href=\"{1}\"".FormatWith("", "javascript:;", sc.SubjectClassId);
                html += "title=\"{0}\"><span>{0}</span></a>".FormatWith(sc.SubjectClassName);
                html += "<ul>";

                foreach (var subject in subjectList)
                {
                    if (!IsRoleHaveRes("subject", "查看", subject.SubjectId, siteId, subject.SubjectId, 0)) continue; //没有查看栏目权限
                    var listColumn = ColumnList.Where(p => !p.IsDel && p.ParentId == 0 && p.SiteId == siteId && p.SiteType == sc.SubjectClassId).OrderBy(p => p.Sort).ToList();

                    string name = subject.SubjectName.Length > 10 ? subject.SubjectName.Substring(0, 7) + "..." : subject.SubjectName;

                    if (listColumn.Count > 0)
                    {

                        html += "<li><a   id=\"subject{2}\" class=\"tooltip-tip{0} ajax-load tooltipster-disable\" href=\"{1}\"".FormatWith(2, "javascript:;", subject.SubjectId); //三级菜单
                        html += "title=\"{2}\" ><span>{0}{1}</span>".FormatWith(name, SysManagePageBase.IsDevUser ? "[" + subject.SubjectId + "]" : "", subject.SubjectName);
                        html += "<span class=\"caret\" style=\"border-width:7px !important;\"  buttonIds=\"{0}\" columnId=\"{1}\" subjectId=\"{2}\" classId=\"{3}\" style=\"cursor:pointer;\"></span></a>"
                                    .FormatWith("1,2", subject.SubjectId * -1, subject.SubjectId, sc.SubjectClassId);
                        html += "<ul>";
                        html += GetColumnMenuHtml(0, siteId, sc.SubjectClassId, subject.SubjectId, 3); //递归子站栏目 下的子栏目
                        html += "</ul></li>";
                    }
                    else
                    {
                        name = sc.SubjectClassName.Length > 10 ? sc.SubjectClassName.Substring(0, 7) + "..." : sc.SubjectClassName;
                        html += "<li><a class=\"tooltip-tip{0} ajax-load tooltipster-disable\" href=\"{1}\"".FormatWith(2, "javascript:;");
                        html += "title=\"{0}\"  ><span>{1}</span></a>".FormatWith(sc.SubjectClassName, name);

                    }
                }
                html += "</ul></li>";
            }
            else
            {
                html += "<li><a class=\"tooltip-tip{0} ajax-load tooltipster-disable\" href=\"{1}\"".FormatWith("", "javascript:;");
                html += "title=\"{0}\"><span>{0}</span></a></li>".FormatWith(sc.SubjectClassName);

            }
        }
        return html;
    }

    /// <summary>
    /// 获取当前角色所有权限 缓存起来
    /// </summary>
    /// <returns></returns>
    protected void GetRoleAllRes()
    {
        try
        {
            if (Whir.Security.Service.AuthenticateHelper.User != null)
            {
                Whir.Security.RolesService rolesService = new Whir.Security.RolesService();
                var model = rolesService.SingleOrDefault<Roles>(Whir.Security.Service.AuthenticateHelper.User.RolesId) ?? new Roles();
                var subjectList = model.SubjectColumnJurisdiction.ToStr().Split(',').ToList();
                subjectList.AddRange(model.SubSiteColumnJurisdiction.ToStr().Split(',').ToList());
                RoleAllRes.Add("column", model.ColumnJurisdiction.ToStr().Split(',').ToList());
                RoleAllRes.Add("subject", subjectList);
                RoleAllRes.Add("menu", model.MenuJurisdiction.ToStr().Split(',').ToList());

            }

        }
        catch
        {

        }
    }

    /// <summary>
    /// 判断是否有权限
    /// </summary>
    /// <returns></returns>
    protected bool IsRoleHaveRes(string type, string functionName, int columnId, int siteId, int subjectId, int menuId)
    {
        if (Whir.Security.Service.AuthenticateHelper.User.RolesId == 1)
            return true;
        switch (type)
        {
            case "menu":
                return RoleAllRes[type].Contains(menuId.ToStr());
            case "column":
                return RoleAllRes[type].Contains(string.Format("{0}|siteId{1}|{2}", functionName, siteId, columnId));
            case "subject":
            case "subjectcolumn":
                //专题、子站
                return RoleAllRes["subject"].Contains(string.Format("{0}|{1}|{2}|siteId{3}|{4}", type, functionName, columnId, siteId, subjectId));
            default:
                return false;
        }
    }
}