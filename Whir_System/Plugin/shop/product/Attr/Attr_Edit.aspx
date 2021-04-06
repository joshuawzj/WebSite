<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="attr_edit.aspx.cs" Inherits="whir_system_Plugin_shop_product_attr_attr_edit" %>

<%@ Register Src="../../common/HeadContainer.ascx" TagName="HeadContainer" TagPrefix="uc1" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="AttrList.aspx" aria-expanded="true"> <%="规格管理".ToLang()%></a></li>
                    <li class="active"><a href="Attr_Edit.aspx" data-toggle="tab" aria-expanded="true"><%=ProcessStr %></a></li>
                </ul>
                <div class="space15"></div>
                <div id="single" class="tab-pane active">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/shop/AttrForm.aspx">
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Com">
                                <%="规格组名称：".ToLang()%>
                            </div>
                            <div class="col-md-8 ">
                                <input id="SearchName" name="SearchName" class="form-control" maxlength="128"
                                    value="<%=ShopAttr.SearchName %>" required="true" />
                            </div>
                        </div>
                        <div class="form-group attr-edit-text" for="ShowImage">
                            <div class="col-md-2 text-right" >
                                <%="是否开启图标显示：".ToLang()%>
                            </div>
                            <div class="col-md-8 ">
                                <ul class="list">
                                    
                                    <li>
                                        <input type="radio" <%=ShopAttr.IsShowImage?"checked=\"checked\"":"" %>  name="ShowImage" value="1" /> <%="开启".ToLang()%>
                                    </li>
                                    <li>
                                        <input type="radio" <%=ShopAttr.IsShowImage?"":"checked=\"checked\"" %>  name="ShowImage" value="0" /> <%="关闭".ToLang()%>
                                    </li>
                                    
                                </ul>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-8 ">
                                <input type="hidden" name="_action" value="SaveAttrEdit" />
                                <input type="hidden" name="AttrID" value="<%=AttrID %>" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                </div>
                                <a class="btn btn-white" href="AttrList.aspx"><%="返回".ToLang()%></a>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $('#formEdit').Validator(null,
              function (el) {
                  var actionSuccess = el.attr("form-success");
                  var $form = $("#formEdit");
                  $form.post({
                      success: function (response) {
                          if (response.Status == true) {
                              actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "attrlist.aspx?attrvalueid=<%=AttrID %>");

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
                  return false
              });
    </script>
</asp:Content>

