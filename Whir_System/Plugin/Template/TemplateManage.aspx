<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="TemplateManage.aspx.cs" Inherits="Whir_System_Plugin_Template_TemplateManage" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Controls.UI.Controls" %>
<%@ Import Namespace="Whir.Domain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function openHtml(cid, name, val) {
            $("#txtName").val(name);
            $("[name='txtHTML']").val($("#" + cid).val());
            editorSetContent($("#" + cid).val());
            $("[name='editor']").iCheck("uncheck");
            $("[name='editor'][value='" + val + "']").iCheck("check");
        }

        function editorSetContent(insertTxt) {
            if ($("#ifmEWebEditor-1").length > 0) {//ewebeditor
                var wd = document.getElementById("ifmEWebEditor-1").contentWindow;
                wd.focus();
                wd.setHTML(insertTxt);
            } else if (typeof (editortxtHTML) != "undefined") {//kindeditor
                editortxtHTML.focus();
                editortxtHTML.html(insertTxt);
            } else if (typeof (uetxtHTML) != "undefined") {//ueditor,wueditor
                uetxtHTML.focus();
                uetxtHTML.setContent(insertTxt);
                    //execCommand('inserthtml', insertTxt)
            }
        }

        function canDel() {
            var select = whir.checkbox.getSelect("editor");
            if (select == "") {
                whir.toastr.error("<%="请选择模板".ToLang()%>");
                return false;
            }
            if ($("#txtName").val() == "") {
                whir.toastr.error("<%="请选择模板".ToLang()%>");
                return false;
            }
            //whir.dialog.confirm("确定删除该记录吗？", function () {
            //    return true;
            //});
            return confirm("<%="确定删除该记录吗？".ToLang()%>");
        }

        function canSave() {
            var select = whir.checkbox.getSelect("editor");
            if (select == "") {
                whir.toastr.error("<%="请选择一种编辑器".ToLang()%>");
                return false;
            }
            if ($("#txtName").val() == "") {
                whir.toastr.error("<%="请输入模板名称".ToLang()%>");
                return false;
            }
            if (!/^[\u4e00-\u9fa5_a-zA-Z0-9]+$/.test($("#txtName").val())) {
                whir.toastr.error("<%="模板名称只能是字母、数字、中文".ToLang()%>");
                return false;
            }
            if ($("[name='txtHTML']").val().indexOf("<textarea") > -1 || $("[name='txtHTML']").val().indexOf("</textarea>") > -1) {
                whir.toastr.error("<%="模板HTML内容不能包含textarea标签".ToLang()%>");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <form runat ="server">
      <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
        <div class="panel-heading"><%="编辑器模板".ToLang()%></div>
            <div class="panel-body">
                    <div class="col-md-3">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <%="编辑器".ToLang()%>
                            </div>
                            <div class="panel-body">
                             <ul class="list-group">
                                <li class="list-group-item ">eWebEditor
                                </li>
                                <li class="list-group-item padding-left30 line-height25">
                                    <%foreach (var item in eWebEditorList)
                                      { %>
                                    <a href="javascript:openHtml('eWebEditor<%=eWebEditorList.IndexOf(item) %>','<%=item.Name %>',1)" ><%=item.Name %></a>
                                    <textarea style="display:none" id="eWebEditor<%=eWebEditorList.IndexOf(item) %>"><%=item.Html %></textarea>
                                    <br />
                                    <%} %>
                                     
                                </li>
                                <li class="list-group-item ">KindEditor
                                </li>
                                <li class="list-group-item padding-left30 line-height25">
                                    <%foreach (var item in kindEditorList)
                                      { %>
                                    <a href="javascript:openHtml('kindEditor<%=kindEditorList.IndexOf(item) %>','<%=item.Name %>',2)" ><%=item.Name %></a>
                                    <textarea style="display:none" id="kindEditor<%=kindEditorList.IndexOf(item) %>"><%=item.Html %></textarea>
                                    <br />
                                    <%} %>
                                </li>
                                <li class="list-group-item "><%="百度编辑器".ToLang()%>
                                </li>
                                <li class="list-group-item padding-left30 line-height25">
                                    <%foreach (var item in uEditorList)
                                      { %>
                                    <a href="javascript:openHtml('uEditor<%=uEditorList.IndexOf(item) %>','<%=item.Name %>',3)" ><%=item.Name %></a>
                                    <textarea style="display:none"  id="uEditor<%=uEditorList.IndexOf(item) %>"><%=item.Html %></textarea>
                                    <br />
                                    <%} %>
                                </li>
                            </ul>

                            </div>
                        </div>
                    </div>
                    <div class="col-md-9 ">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <%="模板文件".ToLang()%>
                            </div>
                            <div class="panel-body  form-horizontal">
                                  
                                 <div class="form-group">
                                    <div class="col-md-2 text-right" >
                                         <%="模板名称：".ToLang()%>
                                    </div>
                                    <div class="col-md-10 ">
                                        <input type="text" id="txtName" name="txtName"  class="form-control"  required="true" maxlength="10"    />
                                    </div>
                                </div>
                                 <div class="form-group">
                                    <div class="col-md-2 text-right" >
                                         <%="所属编辑器：".ToLang()%>
                                    </div>
                                    <div class="col-md-10 ">
                                        <ul class="list">
                                            <li>
                                                  <input type="checkbox" id="eWebEditor" name="editor" value="1"  />
                                                        <label for="IsRelated"><%="eWebEditor".ToLang()%></label>
                                             </li>
                                             <li>
                                                  <input type="checkbox" id="KindEditor" name="editor" value="2"  />
                                                        <label for="IsRelated"><%="KindEditor".ToLang()%></label>
                                             </li>
                                             <li>
                                                  <input type="checkbox" id="uEditor" name="editor" value="3"  />
                                                        <label for="IsRelated"><%="百度编辑器".ToLang()%></label>
                                             </li>
                                         </ul>
                                    </div>
                                </div>
                                 <div class="form-group">
                                    <div class="col-md-2 text-right" >
                                         <%="模板HTML：".ToLang()%>
                                    </div>
                                    <div class="col-md-10 ">
                                        <%=new Editer(new Column(), new Form() { DefaultValue = "", FormId = -1 }, new Field{FieldAlias = "模板HTML",FieldName = "txtHTML",}, RegularEnum.Never).Render("") %>                                    
                                    </div>
                                </div>
                                 <div class="form-group">
                                    <div class="col-md-offset-2 col-md-10 ">
                                        <%if (IsCurrentRoleMenuRes("382"))
                                          { %>
                                        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-info" OnClientClick="return canSave()"><%="保存".ToLang()%> </asp:LinkButton>
                                          <%} %>
                                         <%if (IsCurrentRoleMenuRes("383"))
                                           { %>
                                        <asp:LinkButton ID="btnDel" runat="server" OnClick="btnDel_Click" CssClass="btn text-danger border-danger"   OnClientClick="return canDel()"><%="删除".ToLang()%> </asp:LinkButton>
                                         <%} %>
                                    </div>
                                 </div>
                                      
                            </div>
                        </div>
                    </div>
            </div>
        </div>
    </div>
    
    </form>
  
</asp:content>

