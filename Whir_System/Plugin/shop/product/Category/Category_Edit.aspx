<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="category_edit.aspx.cs" Inherits="whir_system_Plugin_shop_product_category_category_edit" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Register Src="~/whir_system/Plugin/shop/common/HeadContainer.ascx" TagName="HeadContainer" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="categorylist.aspx" aria-expanded="true"><%="商品类别管理".ToLang()%></a></li>
                    <li class="active"><a href="category_edit.aspx" data-toggle="tab" aria-expanded="true"><%="添加商品类别".ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div id="single" class="tab-pane active">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/shop/CategoryForm.aspx">
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="CourierName">
                                <%="上级类别：".ToLang()%>
                            </div>
                            <div class="col-md-8 ">
                                <select id="selecCategory" name="selecCategory" class="form-control">
                                    <option value=""><%="==请选择==".ToLang()%></option>
                                    <% foreach (System.Data.DataRow item in OptionTable.Rows)
                                        {
                                            var check = "";
                                    %>
                                    <% if (ShopCategory.ParentID == item["CategoryID"].ToInt())
                                        {
                                            check = "selected=\"selected\"";
                                        } %>
                                    <option <%=check %> value="<%=item["CategoryID"]%>">
                                        <%=item["CategoryName"]%></option>
                                    <%  }   %>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Com">
                                <%="类别名称：".ToLang()%>
                            </div>
                            <div class="col-md-8 ">
                                <input id="CategoryName" name="CategoryName" class="form-control" maxlength="30"
                                    value="<%=ShopCategory.CategoryName%>" required="true" />
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2 control-label" for="ShowImage">
                                <%="类别图标：".ToLang()%>
                            </div>
                            <div class="col-md-8 ">
                                <input id="txt_file" value="" name="txt_file" for="CategoryImages" type="file" class="file-loading" accept=".jpg,.png,.gif,.bmp" />
                                <input type="hidden" id="CategoryImages" value="<%=ShopCategory.CategoryImages %>" name="CategoryImages" />
                                <script type="text/javascript">
                                    var config = {
                                        uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages.aspx?FormId=0&savePath=<%=UploadFilePath %>&fileName=",
                                        previewFileType: "image",
                                        language: '<%=GetLoginUserLanguage()%>',
                                        allowedFileExtensions: ['jpg', 'png', 'gif'],
                                        initialCaption: '<%="支持格式：".ToLang()%>jpg,png,gif',
                                        previewClass: "bg-warning",
                                        initialPreviewAsData: true,
                                        initialPreviewFileType: 'image',
                                        pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=ProductPic&ControlId=txt_file',
                                            isPic: true
                                    };
                                    if ("<%=ShopCategory.CategoryImages %>") {
                                        config = {
                                            uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages.aspx?FormId=0&savePath=<%=UploadFilePath %>&fileName=",
                                            previewFileType: "image",
                                            language: '<%=GetLoginUserLanguage()%>',
                                            allowedFileExtensions: ['jpg', 'png', 'gif'],
                                            initialCaption: '<%="支持格式：".ToLang()%>jpg,png,gif',
                                            previewClass: "bg-warning",
                                            initialPreviewAsData: true,
                                            initialPreviewFileType: 'image',
                                            initialPreview: "<%=UploadFilePath+ShopCategory.CategoryImages %>",
                                            initialPreviewConfig: [{ caption: '', size: 0, name: '<%=ShopCategory.CategoryImages%>', key: 0 }],
                                            pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=ProductPic&ControlId=txt_file',
                                            isPic: true
                                        };
                                    };
                                    $(function () {
                                        $("#txt_file").fileinput(config).on("filebatchselected", function (event, files) {
                                            $(this).fileinput("upload");
                                        }).on("fileuploaded", function (event, data) {
                                            if (data.response && data.response.Result == true) {
                                                $("#CategoryImages").val(data.response.Msg);
                                            }
                                        }).on("fileclear", function (event, data) {
                                            $("#CategoryImages").val("");
                                        });
                                    });
                                </script>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2 control-label">
                                <%="SEO标题：".ToLang()%>
                            </div>
                            <div class="col-md-8 ">
                                <input id="MetaTitle" name="MetaTitle" class="form-control"
                                    value="<%=ShopCategory.MetaTitle%>" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label">
                                <%="SEO关键字：".ToLang()%>
                            </div>
                            <div class="col-md-8 ">
                                <input id="MetaKeyword" name="MetaKeyword" class="form-control"
                                    value="<%=ShopCategory.MetaKeyword%>" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label">
                                <%="SEO描述：".ToLang()%>
                            </div>
                            <div class="col-md-8 ">
                                <textarea id="MetaDescription" name="MetaDescription" class="form-control"
                                    value="<%=ShopCategory.MetaDescription%>" style="height: 112px;"></textarea>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-8 ">
                                <input type="hidden" name="_action" value="SaveCategory" />
                                <input type="hidden" name="CategoryID" value="<%=CategoryID %>" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                </div>
                                <a class="btn btn-white" href="categorylist.aspx"><%="返回".ToLang()%></a>
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
                                                        actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "categorylist.aspx?categoryid=<%= CategoryID %>");

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
