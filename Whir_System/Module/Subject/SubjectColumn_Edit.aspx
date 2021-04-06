<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="subjectcolumn_edit.aspx.cs" Inherits="whir_system_module_subject_subjectcolumn_edit" %>

<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">

            <div class="panel-body">

                <section class="panel">
                        <% if (CurrentColumn.ColumnId == 0)
                            { %>
                            <ul class="nav nav-tabs">
                                <li><a  aria-expanded="true" href="<%=IsDevUser? "SubjectColumnList":"SubjectList" %>.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>&subjectId=<%= SubjectId %>">
                                    <%=SubjectTypeId == 1 ? "子站栏目结构".ToLang() : "专题栏目结构".ToLang()%></a></li> 
                                  <li class="active"><a data-toggle="tab" href="#single" aria-expanded="true"><%="单条添加".ToLang()%></a></li>
                                  <li class=""><a data-toggle="tab" href="#multi" aria-expanded="false"><%="批量添加".ToLang()%></a></li>
                            </ul>
                        
                        <% } %>
                        <% else if (CurrentColumn.ColumnId != 0)
                            { %>
                            <ul class="nav nav-tabs">
                                 <%if (IsDevUser || IsSuperUser)
                                     { %>
                               <li><a  aria-expanded="true" href="<%=IsDevUser ? "SubjectColumnList" : "SubjectList" %>.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>&subjectId=<%= SubjectId %>">
                                    <%=SubjectTypeId == 1 ? "子站栏目结构".ToLang() : "专题栏目结构".ToLang()%></a></li> 
                                <%} %>
                                <%if (IsRoleHaveColumnRes("栏目修改", CurrentColumn.ColumnId, SubjectId))
                                    {%>
                                <li class="active"><a href="ColumnList.aspx"><%="栏目设置".ToLang() %></a></li>
                                <%} %>

                            </ul>
                         
                        <% } %>
                       <div class="panel-body">
                            <div class="tab-content">
                                <% if (CurrentColumn.ColumnId == 0)
                                    { %>
                                <div id="single" class="tab-pane <%=CurrentColumn.ColumnId==0?"active":"" %>">
                                    <form id="formSingle" class="form-horizontal" form-url="<%=SysPath%>Handler/Common/Subject.aspx">
                                        <div class="tab-content">
                                             <%if (IsRoleHaveColumnRes("栏目修改", CurrentColumn.ColumnId, SubjectId))
                                                 {%>
                                             <div id="baseTab" class="tab-pane <%=CurrentColumn.ColumnId ==0?"active":"" %>">
                                                <div class="form-group">
                                                    <div class="col-md-2 control-label" for="ParentId"><%="上级栏目：".ToLang()%></div>
                                                    <div class="col-md-10 ">
                                                        <select id="ParentId" name="ParentId" class="form-control">
                                                            <option value="0"><%="请选择".ToLang() %></option>
                                                            <% foreach (var column in ColumnTreeList)
                                                                {
                                                                    %>
                                                            <option value="<%=column.ColumnId%>"><%=column.ColumnName%></option>
                                                            <%  }   %>
                                                        </select>
                                                    </div>
                                                </div> 
                                                <%if (IsDevUser)
                                                    { %>
                                                <div class="form-group">
                                                    <div class="col-md-2 control-label" for="ModelId"><%="功能模块：".ToLang()%></div>
                                                    <div class="col-md-10 ">
                                                        <select id="ModelId" name="ModelId" class="form-control">
                                                            <option value="0"><%="请选择".ToLang() %></option>
                                                            <% foreach (var model in AllModel.Where(p => p.ParentId == 0 && !p.IsDel))
                                                                {
                                                                    %>
                                                            <option value="<%=model.ModelId%>"><%=model.ModelName%></option>
                                                            <%  }   %>
                                                        </select>
                                                    </div>
                                                </div>
                                                <%} %>
                                                <div class="form-group">
                                                    <div class="col-md-2 control-label" for="ColumnName"><%="栏目名称：".ToLang()%></div>
                                                    <div class="col-md-10 ">
                                                        <input id="ColumnName" name="ColumnName" class="form-control"
                                                             value="<%=SubColumn.ColumnName.IsEmpty()?CurrentColumn.ColumnName:SubColumn.ColumnName%>"
                                                               required="true"/>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-2 control-label" for="ColumnNameStage"><%="栏目别名：".ToLang()%></div>
                                                    <div class="col-md-10 ">
                                                        <input id="ColumnNameStage" name="ColumnNameStage" class="form-control" value="<%=SubColumn.ColumnNameStage.IsEmpty()?CurrentColumn.ColumnNameStage:SubColumn.ColumnNameStage %>"/>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-2 control-label" for="Path"><%="英文目录：".ToLang()%></div>
                                                    <div class="col-md-10 ">
                                                        <input id="Path" name="Path" class="form-control" value="<%=CurrentColumn.Path%>" required="true" maxlength="128"/>
                                                    </div>
                                                </div>
                                            </div>
                                                <%} %>
                                          <div class="form-group">
                                                <div class="col-md-offset-2 col-md-10 ">
                                                    <input type="hidden" name="ColumnId" value="<%=CurrentColumn.ColumnId%>" />
                                                    <input type="hidden" name="SubjectClassId" value="<%=SubjectClassId%>" />
                                                    <input type="hidden" name="SubjectId" value="<%=SubjectId%>" />
                                                    <input type="hidden" name="SiteId" value="<%=CurrentSiteId%>" />
                                                    <input type="hidden" name="CreateDate" value="<%=CurrentColumn.CreateDate.Value(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")%>"/> 
                                                    <input type="hidden" name="CreateUser" value="<%=CurrentColumn.CreateUser??CurrentUserName%>"/> 
                                                    <input type="hidden" name="UpdateDate" value="<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>"/> 
                                                    <input type="hidden" name="UpdateUser" value="<%=CurrentUserName%>"/> 
                                                    <input type="hidden" name="SiteType" value="<%=SubjectClassId%>"/> 
                                                    <input type="hidden" name="SubjectTypeId" value="<%=SubjectTypeId%>"/> 
                                                    <input type="hidden" name="_action" value="Save"/>
                                                    <div class="btn-group">
                                                        <button form-action="submitSingle" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                                        <% if (ColumnId == 0)
                                                            { %>
                                                        <button form-action="submitSingle" form-success="refresh" class="btn btn-info"><%= "提交并继续".ToLang() %></button>
                                                        <% } %>
                                                    </div>
                                                    <a class="btn btn-white" href="SubjectColumnList.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>&subjectId=<%= SubjectId %>"><%="返回".ToLang()%></a>
                                                </div>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                                <div id="multi" class="tab-pane">
                                    <form id="formMulti" class="form-horizontal" form-url="<%=SysPath%>Handler/Common/Subject.aspx">
                                        <div class="form-group">
                                            <div class="col-md-2 control-label" for="ParentId"><%="上级栏目：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <select   name="ParentId" class="form-control">
                                                    <option value="0"><%="请选择".ToLang() %></option>
                                                    <% foreach (var column in ColumnTreeList)
                                                        {
                                                            %>
                                                    <option value="<%=column.ColumnId%>"><%=column.ColumnName%></option>
                                                    <%  }   %>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2 control-label" for="BatchColumnName"><%="栏目名称：".ToLang()%></div>
                                            <div class="col-md-5">
                                                <textarea   name="BatchColumnName" class="form-control" rows="16" required="true" ></textarea>
                                            </div>
                                            <div class="col-md-5 alert alert-info">
                                                  <strong><%="注意：".ToLang()%></strong><br />
                                                <%="1．每行填写一个栏目".ToLang()%><br />
                                                <%="2．子栏目相对父栏目使用“-”缩进".ToLang()%><br />
                                                <%="3．设置栏目功能模块在栏目名称后“#”隔开".ToLang()%><br />
                                                <strong><%="示例：".ToLang()%></strong><br />
                                                <%="栏目1".ToLang()%><br />
                                                <%="-下级栏目2".ToLang()%><br />
                                                <%="--下下级栏3#简单信息".ToLang()%><br />
                                                <%="-下级栏目4".ToLang()%><br />
                                                <strong><%="功能模块：".ToLang()%></strong><br />
                                              <%="产品展示、单页图文、电子期刊、简单信息、类别管理、人才招聘、上传下载、提交表单、网上留言、网上投票、问卷调查、销售网络、友情链接、子站产品展示、子站单页图文、子站简单信息、子站提交表单".ToLang()%> 
                                                   </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-offset-2 col-md-10 ">
                                                <input type="hidden" name="SubjectClassId" value="<%=SubjectClassId%>" />
                                                <input type="hidden" name="SubjectId" value="<%=SubjectId%>" />
                                                <input type="hidden" name="_action" value="SaveMulti"/>
                                                <div class="btn-group">
                                                <button form-action="submitMulti" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                                <% if (ColumnId == 0)
                                                    {%>
                                                    <button form-action="submitMulti" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                                 <%  } %>
                                                </div>
                                                <a class="btn btn-white" href="SubjectColumnList.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>&subjectId=<%= SubjectId %>"><%="返回".ToLang()%></a>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                                <% } %>
                        <% else if (CurrentColumn.ColumnId != 0)
                            { %>
                                <%if (IsRoleHaveColumnRes("栏目修改", CurrentColumn.ColumnId, SubjectId))
                                    {%>
                                <div id="set" class="tab-pane <%=CurrentColumn.ColumnId !=0?"active":"" %>">
                              <form id="formSet" class="form-horizontal" form-url="<%=SysPath%>Handler/Common/Subject.aspx">
                              <div class="panel panel-default panels">
                                  <div class="panel-heading"><%="基本信息".ToLang()%></div>
                                  <div class="panel-body">
                                        <div class="form-group">
                                                    <div class="col-md-2 control-label" for="ColumnName"><%="栏目名称：".ToLang()%></div>
                                                    <div class="col-md-10 ">
                                                        <input id="ColumnName" name="ColumnName" class="form-control"
                                                             value="<%=SubColumn.ColumnName.IsEmpty()?CurrentColumn.ColumnName:SubColumn.ColumnName%>"
                                                               required="true"/>
                                                    </div>
                                                </div>
                                        <div class="form-group">
                                                    <div class="col-md-2 control-label" for="ColumnNameStage"><%="栏目别名：".ToLang()%></div>
                                                    <div class="col-md-10 ">
                                                        <input id="ColumnNameStage" name="ColumnNameStage" class="form-control" value="<%=SubColumn.ColumnNameStage.IsEmpty()?CurrentColumn.ColumnNameStage:SubColumn.ColumnNameStage %>"/>
                                                    </div>
                                        </div>

                                      <%if (IsRoleHaveColumnRes("SEO设置", CurrentColumn.ColumnId, SubjectId))
                                          {%>
                                        <div class="form-group">
                                            <div class="col-md-2 control-label" for="MetaTitle"><%="SEO标题：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <input id="MetaTitle" name="MetaTitle" class="form-control" maxlength="200" value="<%=SubColumn.MetaTitle%>"/>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2 control-label" for="MetaKeyword"><%="SEO关键字：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                               <textarea id="MetaKeyword" name="MetaKeyword" class="form-control" style="overflow-x: hidden;" maxlength="200"><%=SubColumn.MetaKeyword%></textarea>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2 control-label" for="MetaDesc"><%="SEO描述：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <textarea id="MetaDesc" name="MetaDesc" class="form-control" style="overflow-x: hidden;" maxlength="200"><%=SubColumn.MetaDesc%></textarea>
                                            </div>
                                        </div>
                                        <%} %>
                                      <div class="form-group">
                                                <div class="col-md-2 control-label" for="BannerUrl"><%="Banner链接：".ToLang()%></div>
                                                <div class="col-md-10 ">
                                                    <input id="BannerUrl" name="BannerUrl" class="form-control" value="<%=CurrentColumn.BannerUrl%>"
                                                            ValidationExpression="http[s]?:\/\/([\w-]+\.)+[\w-]+([\w-./?%&=]*)|([\w-./?%&=]*)"/>
                                                    <span class="note_gray"><%="以“http://”开始代表外网地址，以“/”开始代表本站根目录，最多可输入1024个字符".ToLang()%></span>
                                                </div>
                                            </div>
                                        <div class="form-group">
                                            <div class="col-md-2 control-label" for="txt_file"><%="Banner图：".ToLang()%></div>
                                             <div class="col-md-10">
                                                  <input id="txt_file" value="" name="txt_file" type="file" for="ImageUrl" class="file-loading" accept="<%=AcceptPicType %>" />
                                                  <input type="hidden" id="ImageUrl" value="<%=CurrentColumn.ImageUrl %>" name="ImageUrl" />
                                             </div>
                                        </div>
                                      <div class="form-group">
                                            <div class="col-md-2 control-label" for="txt_small"><%="移动端Banner图：".ToLang()%></div>
                                             <div class="col-md-10">
                                                  <input id="txt_small" value="" name="txt_small" type="file" for="SmallImageUrl" class="file-loading" accept="<%=AcceptPicType %>" />
                                                  <input type="hidden" id="SmallImageUrl" value="<%=CurrentColumn.SmallImageUrl %>" name="SmallImageUrl" />
                                             </div>
                                        </div>
                                       <div class="form-group">
                                                    <div class="col-md-2 text-right" for="IsShow"><%="是否前台显示：".ToLang()%></div>
                                                    <div class="col-md-10 ">
                                                        <ul class="list">
                                                            <li>
                                                                <input type="radio" id="IsShow_True" name="IsShow" value="1" />
                                                                <label for="IsShow_True"><%="是".ToLang()%></label>
                                                            </li>
                                                            <li>
                                                                <input type="radio" id="IsShow_False" name="IsShow" value="0" />
                                                                <label for="IsShow_False"><%="否".ToLang()%></label>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                       <div class="form-group">
                                                    <div class="col-md-2 text-right" for="IsCategoryShow"><%="是否前台显示类别：".ToLang()%></div>
                                                    <div class="col-md-10 ">
                                                        <ul class="list">
                                                            <li>
                                                                <input type="radio" id="IsCategoryShow_True" name="IsCategoryShow" value="1" />
                                                                <label for="IsCategoryShow_True"><%="是".ToLang()%></label>
                                                            </li>
                                                            <li>
                                                                <input type="radio" id="IsCategoryShow_False" name="IsCategoryShow" value="0" />
                                                                <label for="IsCategoryShow_False"><%="否".ToLang()%></label>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                  </div>
                                  
                                  <%if (IsDevUser)
                                      { %>
                                  <div class="panel-heading"><%="高级设置".ToLang()%></div>
                                  <div class="panel-body">
                                            <div class="form-group">
                                                    <div class="col-md-2 control-label" for="ParentId"><%="上级栏目：".ToLang()%></div>
                                                    <div class="col-md-10 ">
                                                <select id="ParentId" name="ParentId" class="form-control" <%=CurrentUserRolesId==1?"":"disabled='disabled'" %> >
                                                            <option value="0"><%="请选择".ToLang() %></option>
                                                            <% foreach (var column in ColumnTreeList)
                                                                {
                                                                    %>
                                                            <option value="<%=column.ColumnId%>"><%=column.ColumnName%></option>
                                                            <%  }   %>
                                                        </select>
                                                    </div>
                                                </div>
                                            <div class="form-group">
                                                    <div class="col-md-2 control-label" for="ModelId"><%="功能模块：".ToLang()%></div>
                                                    <div class="col-md-10 ">
                                                        <select id="ModelId" name="ModelId" class="form-control">
                                                            <option value="0"><%="请选择".ToLang() %></option>
                                                            <% foreach (var model in AllModel.Where(p => p.ParentId == 0 && !p.IsDel))
                                                                {
                                                                    %>
                                                            <option value="<%=model.ModelId%>"><%=model.ModelName%></option>
                                                            <%  }   %>
                                                        </select>
                                                    </div>
                                                </div>
                                            <div class="form-group">
                                                    <div class="col-md-2 control-label" for="Path"><%="英文目录：".ToLang()%></div>
                                                    <div class="col-md-10 ">
                                                <input id="Path" name="Path" class="form-control" value="<%=CurrentColumn.Path%>"  required="true" maxlength="128" />
                                                    </div>
                                                </div>
                                            <div class="form-group">
                                            <div class="col-md-2 control-label" for="DefaultTemp"><%="栏目首页模板：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <div class="input-group ">
                                                    <input id="DefaultTemp" name="DefaultTemp" class="form-control" readonly="readonly" value="<%=CurrentColumn.DefaultTemp%>" />
                                                    <span class="input-group-addon pointer" for="DefaultTemp" dialog-url="TemplateSelector.aspx"><%="选择".ToLang()%></span>
                                                    <span class="input-group-addon pointer" onclick="$('#DefaultTemp').val('');"><%="清空".ToLang()%></span>
                                                </div>
                                            </div>
                                        </div>
                                            <div class="form-group">
                                            <div class="col-md-2 control-label" for="ListTemp"><%="列表页模板：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <div class="input-group ">
                                                    <input id="ListTemp" name="ListTemp" class="form-control" readonly="readonly" value="<%=CurrentColumn.ListTemp%>" />
                                                    <span class="input-group-addon pointer" for="ListTemp" dialog-url="TemplateSelector.aspx"><%="选择".ToLang()%></span>
                                                    <span class="input-group-addon pointer" onclick="$('#ListTemp').val('');"><%="清空".ToLang()%></span>
                                                </div>
                                            </div>
                                        </div>
                                            <div class="form-group">
                                            <div class="col-md-2 control-label" for="ContentTemp"><%="详细页模板：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <div class="input-group ">
                                                    <input id="ContentTemp" name="ContentTemp" class="form-control" readonly="readonly" value="<%=CurrentColumn.ContentTemp%>" />
                                                    <span class="input-group-addon pointer" for="ContentTemp" dialog-url="TemplateSelector.aspx"><%="选择".ToLang()%></span>
                                                    <span class="input-group-addon pointer" onclick="$('#ContentTemp').val('');"><%="清空".ToLang()%></span>
                                                </div>
                                            </div>
                                        </div>
                                            <div class="form-group">
                                            <div class="col-md-2 control-label" for="PreviewTemp"><%="预览页模板：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <div class="input-group ">
                                                    <input id="PreviewTemp" name="PreviewTemp" class="form-control" readonly="readonly" value="<%=CurrentColumn.PreviewTemp%>" />
                                                    <span class="input-group-addon pointer" for="PreviewTemp" dialog-url="TemplateSelector.aspx"><%="选择".ToLang()%></span>
                                                    <span class="input-group-addon pointer" onclick="$('#PreviewTemp').val('');"><%="清空".ToLang()%></span>
                                                </div>
                                            </div>
                                        </div>
                                  </div>

                                  <div class="panel-heading"><%="开发设置".ToLang()%></div>
                                  <div class="panel-body">
                                            <div class="form-group">
                                            <div class="col-md-2 text-right" for="PreviewTemp"><%="生成模式：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <ul class="list">
                                                    <li>
                                                        <input type="radio" id="CreateMode_No" name="CreateMode" value="0" />
                                                        <label for="CreateMode_No"><%="不生成".ToLang()%></label>
                                                    </li>
                                                    <li>
                                                        <input type="radio" id="CreateMode_Static" name="CreateMode" value="1" />
                                                        <label for="CreateMode_Static"><%="生成静态".ToLang()%></label>
                                                    </li>
                                                    <li>
                                                        <input type="radio" id="CreateMode_Dynamic" name="CreateMode" value="2" />
                                                        <label for="CreateMode_Dynamic"><%="生成动态".ToLang()%></label>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                            <div class="form-group">
                                            <div class="col-md-2 control-label" for="PreviewTemp"><%="导航链接地址：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <input id="LocationUrl" name="LocationUrl" class="form-control" value="<%=CurrentColumn.LocationUrl%>"
                                                        ValidationExpression="http[s]?:\/\/([\w-]+\.)+[\w-]+([\w-./?%&=]*)|([\w-./?%&=]*)"
                                                     />
                                                <span class="note_gray"><%="以“http://”开始代表外网地址，以“/”开始代表本站根目录，最多可输入1024个字符".ToLang()%></span>
                                            </div>
                                        </div>
                                            <div class="form-group">
                                            <div class="col-md-2 control-label" for="WorkFlow"><%="工作流定义：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <select id="WorkFlow" name="WorkFlow" class="form-control">
                                                    <option value="0"><%="请选择".ToLang() %></option>
                                                    <% foreach (var flow in AllWorkFlow)
                                                        {
                                                            %>
                                                    <option value="<%=flow.WorkFlowId%>"><%=flow.WorkFlowName%></option>
                                                    <%  }   %>
                                                </select>
                                                <span class="note_gray"><%="修改工作流会导致该栏目信息所有审核中的状态重置".ToLang()%></span>
                                            </div>
                                        </div>
                                            <div class="form-group">
                                            <div class="col-md-2 control-label" for="CategoryLevel"><%="单独类别级数：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <input id="CategoryLevel" name="CategoryLevel" class="form-control" value="<%=CurrentColumn.CategoryLevel%>"
                                                        ValidationExpression="[0-9]*[1-9][0-9]*"  <%=CurrentColumn.ColumnId !=0&&CurrentColumn.IsCategory?" min='1'":""%> max="9" />
                                                <span class="note_gray"><%="必须输入正整数".ToLang()%></span>
                                            </div>
                                        </div>
                                            <div class="form-group">
                                            <div class="col-md-2 text-right" ><%="其它设置：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <ul class="list">
                                                        <%if (CurrentColumn.ModuleMark != null && (CurrentColumn.ModuleMark.ToLower() == "content_v0.0.01" ||
                                                                                                 CurrentColumn.ModuleMark.ToLower() == "subsitecontent_v0.0.01"))
                                                            { %>
                                                    <li>
                                                        <input type="checkbox" id="IsRelated" name="IsRelated" value="1" />
                                                        <label for="IsRelated"><%="启用相关文章".ToLang()%></label>
                                                    </li>
                                                    <li>
                                                        <input type="checkbox" id="IsRedirect" name="IsRedirect" value="1" />
                                                        <label for="IsRedirect"><%="启用转向链接".ToLang()%></label>
                                                    </li>
                                                        <%} %>
                                                        <%if (Whir.Service.ServiceFactory.ColumnService.IsCategoryParent(ColumnId))
                                                            { %>
                                                    <li>
                                                        <input type="checkbox" id="IsCategory" name="IsCategory" value="1" />
                                                        <label for="IsCategory"><%="启用单独类别".ToLang()%></label>
                                                    </li>
                                                        <%} %>
                                                </ul>
                                            </div>
                                        </div>
                                            <div class="form-group" id="divIsCategoryPower">
                                            <div class="col-md-2 text-right" for="IsCategoryPower"><%="启用类别权限：".ToLang()%></div>
                                            <div class="col-md-10 ">
                                                <ul class="list">
                                                    <li>
                                                        <input type="radio" id="IsCategoryPower_True" name="IsCategoryPower" value="1" />
                                                        <label for="IsCategoryPower_True"><%="启用".ToLang()%></label>
                                                    </li>
                                                    <li>
                                                        <input type="radio" id="IsCategoryPower_False" name="IsCategoryPower" value="0" />
                                                        <label for="IsCategoryPower_False"><%="不启用".ToLang()%></label>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                          
                                  </div>
                                  <%} %>
                               </div>
                              
                                            <div class="form-group">
                                                <div class="col-md-offset-2 col-md-10 ">
                                                    <input type="hidden" name="ColumnId" value="<%=CurrentColumn.ColumnId%>" />
                                                    <input type="hidden" name="SubjectClassId" value="<%=SubjectClassId%>" />
                                                    <input type="hidden" name="SubjectId" value="<%=SubjectId%>" />
                                                    <input type="hidden" name="SiteId" value="<%=CurrentSiteId%>" />
                                                    <input type="hidden" name="CreateDate" value="<%=CurrentColumn.CreateDate.Value(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")%>"/> 
                                                    <input type="hidden" name="CreateUser" value="<%=CurrentColumn.CreateUser??CurrentUserName%>"/> 
                                                    <input type="hidden" name="UpdateDate" value="<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>"/> 
                                                    <input type="hidden" name="UpdateUser" value="<%=CurrentUserName%>"/> 
                                                    <input type="hidden" name="SiteType" value="<%=SubjectClassId%>"/> 
                                                    <input type="hidden" name="SubjectTypeId" value="<%=SubjectTypeId%>"/> 
                                                    <input type="hidden" name="_action" value="Save"/>
                                                    <div class="btn-group">
                                                       <%if (IsDevUser || IsSuperUser)
                                                           { %>
                                                        <button form-action="submitSet" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                                        <%}
                                                            else
                                                            { %>  
                                                        <button form-action="submitSet" form-success="refresh" class="btn btn-info"><%="提交".ToLang()%></button>

                                                        <%} %>
                                                        <% if (ColumnId == 0)
                                                            { %>
                                                        <button form-action="submitSet" form-success="refresh" class="btn btn-info"><%= "提交并继续".ToLang() %></button>
                                                        <% } %> 
                                                    </div>
                                                    <%if (IsDevUser || IsSuperUser)
                                                        { %>
                                                    <a class="btn btn-white" href="<%=IsDevUser? "SubjectColumnList":"SubjectList" %>.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>&subjectId=<%= SubjectId %>"><%="返回".ToLang()%></a>
                                                 <%} %>

                                            </div>
                                            </div>
                              </form>
                               </div>
                                <% } %>
                           <% } %>
                            </div>
                        </div>
                    </section>

            </div>
        </div>
    </div>
    <script type="text/javascript">

        //弹出层
        $("[dialog-url]").click(function () {
            var dialogUrl = $(this).attr("dialog-url");
            var targetControl = $(this).attr("for");

            var dialog = whir.dialog.frame("<%="选择模板".ToLang()%>", dialogUrl, function () {
                var frameDocument = dialog.find("iframe").contents();
                var checked = frameDocument.find("input[type='radio'][name='template']:checked");
                if (checked.length != 1) {
                    whir.toastr.warning("<%="请选择一个模板".ToLang()%>");
                    return;
                }
                $(document.getElementById(targetControl)).val(checked.val());
                dialog.remove();
            }, 800);
        });

        //选择事件
        $(document).ready(function () {
            $("input[name='IsCategory']")
                .next()
                .click(function () {
                    Change();

                });
            $("input[name='IsCategory']")
                .parent()
                .next()
                .click(function () {
                    Change();

                });

            //栏目名称自动翻译
            $("#ColumnName").blur(function () {
                var columnNameStage = $("#ColumnNameStage");
                var path = $("#Path");
                if ($(this).val() != "" & (columnNameStage.val() == "" || path.val() == "")) {
                    $.ajax({
                        type: "GET",
                        url: "<%=SysPath %>ajax/common/translater.aspx",
                        data: "source=" + encodeURI($(this).val()) + "&from=zh-cn&to=en&camelCase=1",
                        success: function (msg) {
                            columnNameStage.val() == "" ? columnNameStage.val(msg).change() : "";
                            path.val() == "" ? path.val(msg).change() : "";
                            formRequiredValidator('Path', true, 'formSingle'); //校验必填项的方法 
                            formRequiredValidator('Path', true, 'formSet'); //校验必填项的方法
                        },
                        error: function (response) {
                            whir.toastr.warning("<%="翻译失败".ToLang()%>");
                        }
                    });

                    }
            });


        });
            function Change() {
                if ($("input[name=IsCategory]").prop("checked")) {
                    $("#divIsCategoryPower").show();
                } else {
                    $("#divIsCategoryPower").hide();
                }
            }
            //绑定值
            $("[name='ParentId']").val("<%=CurrentColumn.ParentId%>");
        $("[name='ModelId']").val("<%=CurrentColumn.ModelId%>");
        $("[name='WorkFlow']").val("<%=CurrentColumn.WorkFlow%>");
        $("[name='CreateMode'][value='<%=CurrentColumn.CreateMode%>']").prop("checked", "checked");
        $("[name='IsRelated'][value='<%=CurrentColumn.IsRelated.ToInt()%>']").prop("checked", "checked");
        $("[name='IsRedirect'][value='<%=CurrentColumn.IsRedirect.ToInt()%>']").prop("checked", "checked");
        $("[name='IsCategory'][value='<%=CurrentColumn.IsCategory.ToInt()%>']").prop("checked", "checked");
        $("[name='IsCategoryPower'][value='<%=CurrentColumn.IsCategoryPower.ToInt()%>']").prop("checked", "checked");
        $("[name='IsShow'][value='<%=CurrentColumn.IsShow.ToInt()%>']").prop("checked", "checked");
        $("[name='IsCategoryShow'][value='<%=CurrentColumn.IsCategoryShow.ToInt()%>']").prop("checked", "checked");
        Change();
        if ("<%=ParentId %>" != "0") {
            $("[name=ParentId]").val("<%=ParentId %>");
        }
        //提交内容
        var editOptions = {
            fields: {
                ColumnName: {
                    validators: {
                        notEmpty: {
                            message: '<%="栏目名称为必填项".ToLang() %>'
                        }
                    }
                }, CategoryLevel: {
                    validators: {
                        integer: {
                            message: '<%="单独类别级数只能是数字".ToLang() %>'
                        }
                    }
                },
                Path: {
                    validators: {
                        regexp: {
                            regexp: /^[a-zA-Z0-9]{1,99}$/i,
                            message: '<%="英文目录只能是1-99个英文或数字".ToLang() %>'
                        }
                    }
                },
                LocationUrl: {
                    validators: {
                        regexp: {
                            regexp: /(^\/.*)|(^(http|https):\/\/.*)/,   //还要修改
                            message: '<%="请输入正确网址".ToLang() %>'
                        }
                    }
                },
                BannerUrl: {
                    validators: {
                        regexp: {
                            regexp: /(^\/.*)|(^(http|https):\/\/.*)/,   //还要修改
                            message: '<%="请输入正确网址".ToLang() %>'
                        }
                    }
                }
            },
            submitButtons: '[form-action="submitSingle"]'
        };
        $('#formSingle').Validator(editOptions,
            function (el) {
                var actionSuccess = el.attr("form-success");

                var $form = $("#formSingle");
                $form.post({
                    success: function (response) {
                        if (response.Status == true) {
                            actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) :
                            whir.toastr.success(response.Message, true, false, "<%=IsDevUser? "SubjectColumnList":"SubjectList" %>.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>&subjectId=<%= SubjectId %>");
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    },
                    error: function (response) {
                        whir.toastr.error(response.Message);
                        whir.loading.remove();
                    }
                });
                return false;
            });
            //提交内容
            var options = {
                submitButtons: '[form-action="submitMulti"]'
            };
            $('#formMulti').Validator(options,
                function (el) {
                    var actionSuccess = el.attr("form-success");

                    var $form = $("#formMulti");
                    $form.post({
                        success: function (response) {
                            if (response.Status == true) {
                                actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) :
                                hir.toastr.success(response.Message, true, false, "<%=IsDevUser? "SubjectColumnList":"SubjectList" %>.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>&subjectId=<%= SubjectId %>");
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    },
                    error: function (response) {
                        whir.toastr.error(response.Message);
                        whir.loading.remove();
                    }
                });
                return false;
            });
    </script>

    <script type="text/javascript">
        //上传文件
        var currentpath = "<%= Server.UrlEncode(UploadFilePath) %>";

        //注册上传控件
        function regeditUploadFile() {
            $("#txt_file")
                .fileinput({
                    language: '<%=GetLoginUserLanguage()%>',
                    uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages.aspx?FormId=0&savePath=<%=UploadFilePath %>banner/&fileName=",
                    previewFileType: "image",
                    allowedFileExtensions: [<%=AllowPicType %>],
                    initialCaption: '<%=SystemConfig.BannerTip %>',
                    previewClass: "bg-warning",
                    initialPreviewAsData: true,
                    initialPreviewFileType: 'image',
                    <% if (!CurrentColumn.ImageUrl.IsEmpty())
        {%>
                    initialPreview: "<%=UploadFilePath+CurrentColumn.ImageUrl %>",
                    initialPreviewConfig: [{ caption: '', size: 0, name: '<%=CurrentColumn.ImageUrl%>', key: 0 }],
                    <%}%>
                    pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=ImageUrl&ControlId=txt_file',
                    isPic: true
                })
                .on("filebatchselected",
                function (event, files) {
                    $(this).fileinput("upload");
                })
                .on("fileuploaded",
                function (event, data) {
                    if (data.response && data.response.Result == true) {
                        $("#ImageUrl").val(data.response.Msg);
                    } else {
                        whir.toastr.error(data.response.Msg);
                    }
                }).on("fileclear", function (event, data) {
                    $("#ImageUrl").val("");
                });
            $("#txt_small")
                .fileinput({
                    language: '<%=GetLoginUserLanguage()%>',
                    uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages.aspx?FormId=0&savePath=<%=UploadFilePath %>banner/&fileName=",
                    previewFileType: "image",
                    allowedFileExtensions: [<%=AllowPicType %>],
                    initialCaption: '<%=SystemConfig.SmallBannerTip %>',
                    previewClass: "bg-warning",
                    initialPreviewAsData: true,
                    initialPreviewFileType: 'image',
                    <% if (!CurrentColumn.SmallImageUrl.IsEmpty())
        {%>
                    initialPreview: "<%=UploadFilePath+CurrentColumn.SmallImageUrl %>",
                    initialPreviewConfig: [{ caption: '', size: 0, name: '<%=CurrentColumn.SmallImageUrl%>', key: 0 }],
                    <%}%>
                    pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=SmallImageUrl&ControlId=txt_small',
                    isPic: true
                })
                .on("filebatchselected",
                function (event, files) {
                    $(this).fileinput("upload");
                })
                .on("fileuploaded",
                function (event, data) {
                    if (data.response && data.response.Result == true) {
                        $("#SmallImageUrl").val(data.response.Msg);
                    } else {
                        whir.toastr.error(data.response.Msg);
                    }
                }).on("fileclear", function (event, data) {
                    $("#SmallImageUrl").val("");
                });
        }

        $(document).ready(function () {
            regeditUploadFile();
            editOptions.submitButtons = '[form-action="submitSet"]';
            var options = editOptions;
            $('#formSet').Validator(options,
                function (el) {
                    var actionSuccess = el.attr("form-success");
                    var $form = $("#formSet");
                    $form.post({
                        success: function (response) {
                            if (response.Status == true) {
                                actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) :
                                whir.toastr.success(response.Message, true, false, "<%=IsDevUser? "SubjectColumnList":"SubjectList" %>.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>&subjectId=<%= SubjectId %>");
                            } else {
                                whir.toastr.error(response.Message);
                            }
                            whir.loading.remove();
                        },
                        error: function (response) {
                            whir.toastr.error(response.Message);
                            whir.loading.remove();
                        }
                    });
                    return false;
                });
        });

    </script>
</asp:Content>
