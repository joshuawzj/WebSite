<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="attrvalue_edit.aspx.cs" Inherits="whir_system_Plugin_shop_product_attr_attrvalue_edit" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Register Src="../../common/HeadContainer.ascx" TagName="HeadContainer" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
    <script type="text/javascript">
        $(function () {
            $(".text_common").addClass("text_width");
        });
    </script>
      
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="attrlist.aspx" aria-expanded="true"><%="规格管理".ToLang()%></a></li>
                    <li class="active"><a href="attr_edit.aspx" data-toggle="tab" aria-expanded="true"><%=ProcessStr %></a></li>
                </ul>

                <div class="space15"></div>
                <div id="single" class="tab-pane active">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/shop/AttrForm.aspx">
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="AttrValueName">
                                <%="规格值：".ToLang()%>
                            </div>
                            <div class="col-md-8 ">
                                <input id="AttrValueName" name="AttrValueName" class="form-control" maxlength="30"
                                    value="<%=ShopAttrVal.AttrValueName %>" required="true" />
                            </div>
                        </div>
                        <%if (ShopAttr.IsShowImage)
                            { %>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="ShowImage">
                                <%="规格图标：".ToLang()%>
                            </div>
                            <div class="col-md-8 ">
                                <input id="txt_file" value="" name="txt_file" for="ShowImage" type="file" class="file-loading" />
                                <input type="hidden" id="ShowImage" value="<%=ShopAttrVal.ShowImage %>" name="ShowImage" />
                                <script type="text/javascript">
                                    var config = {
                                        uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages.aspx?FormId=0&savePath=<%=UploadFilePath %>&fileName=",
                                            previewFileType: "any",
                                            language: '<%=GetLoginUserLanguage()%>',
                                            allowedFileExtensions: ['jpg', 'png', 'gif'],
                                            initialCaption: '<%="支持格式：".ToLang()%>jpg,png,gif',
                                            previewClass: "bg-warning",
                                            initialPreviewAsData: true,
                                            initialPreviewFileType: 'image',
                                            pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=ProductPic&ControlId=txt_file',
                                            isPic: true
                                    };
                                    if ("<%=ShopAttrVal.ShowImage %>") {
                                        config = {
                                            uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages.aspx?FormId=0&savePath=<%=UploadFilePath %>&fileName=",
                                            previewFileType: "any",
                                            language: '<%=GetLoginUserLanguage()%>',
                                            allowedFileExtensions: ['jpg', 'png', 'gif'],
                                            initialCaption: '<%="支持格式：".ToLang()%>jpg,png,gif',
                                            previewClass: "bg-warning",
                                            initialPreviewAsData: true,
                                            initialPreviewFileType: 'image',
                                            initialPreview: "<%=UploadFilePath+ShopAttrVal.ShowImage %>",
                                            initialPreviewConfig: [{ caption: '', size: 0, name: '<%=ShopAttrVal.ShowImage%>', key: 0 }],
                                            pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=ProductPic&ControlId=txt_file',
                                            isPic: true
                                            };
                                        };
                                        $(function () {
                                            $("#txt_file").fileinput(config).on("filebatchselected", function (event, files) {
                                                $(this).fileinput("upload");
                                            }).on("fileuploaded", function (event, data) {
                                                if (data.response && data.response.Result == true) {
                                                    $("#ProductPic").val(data.response.Msg);
                                                }
                                            }).on("fileclear", function (event, data) {
                                                $("#ProductPic").val("");
                                            });
                                        });
                                </script>
                            </div>
                        </div>
                        <% }%>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-8 ">
                                <input type="hidden" name="_action" value="SaveAttrValueEdit" />
                                <input type="hidden" name="AttrID" value="<%=AttrID %>" />
                                <input type="hidden" name="AttrValueID" value="<%=AttrValueID %>" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                </div>
                                <a class="btn btn-white" href="attrlist.aspx"><%="返回".ToLang()%></a>
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
