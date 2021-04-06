<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="PictureConfig.aspx.cs" Inherits="Whir_System_Module_Config_Picture" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 
    <!--颜色控件-->
    <link href="<%=SysPath%>res/assets/js/pickcolor/css/pick-a-color-1.2.3.min.css" rel="stylesheet" type="text/css" />
    <script src="<%=SysPath%>res/assets/js/pickcolor/js/pick-a-color-1.2.3.min.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/assets/js/pickcolor/js/tinycolor-0.9.15.min.js" type="text/javascript"></script>
    <!--滑动控件-->
    <link href="<%=SysPath%>res/assets/js/slider/css/bootstrap-slider.min.css" rel="stylesheet" type="text/css" />
    <script src="<%=SysPath%>res/assets/js/slider/js/bootstrap-slider.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="水印设置".ToLang()%></div>
            <div class="panel-body">
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Config/PictureConfig.aspx">
                        <div class="form-group">
                            <div class="col-md-2 text-right"
                                for="IsAutoMakeWatermark">
                                <%="自动水印：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <ul class="list" id="text1">
                                    <li>
                                        <input type="radio" id="IsAutoMakeWatermark_True" name="IsAutoMakeWatermark" value="1" />
                                        <label for="IsAutoMakeWatermark_True"><%="是".ToLang()%></label>
                                    </li>
                                    <li>
                                        <input type="radio" id="IsAutoMakeWatermark_False" name="IsAutoMakeWatermark" value="0" />
                                        <label for="IsAutoMakeWatermark_False"><%="否".ToLang()%></label>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 text-right" for="WaterMarkStyle">
                                <%="水印方式：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <ul class="list">
                                    <li>
                                        <input type="radio" id="WaterMarkStyle_True" name="WaterMarkStyle" value="0" />
                                        <label for="WaterMarkStyle_True"><%="文字".ToLang()%></label>
                                    </li>
                                    <li>
                                        <input type="radio" id="WaterMarkStyle_False" name="WaterMarkStyle" value="1" />
                                        <label for="WaterMarkStyle_False"><%="图片".ToLang()%></label>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Proportion">
                                <%="水印比例：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <select id="Proportion" name="Proportion" class="form-control">
                                    <option value="0.1">10%</option>
                                    <option value="0.2">20%</option>
                                    <option value="0.3">30%</option>
                                    <option value="0.4">40%</option>
                                    <option value="0.5">50%</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group" name="DivFont">
                            <div class="col-md-2 control-label" for="WaterMarkFontText">
                                <%="水印文字：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="WaterMarkFontText" name="WaterMarkFontText" value="<%=PictureConfig.WaterMarkFontText %>" class="form-control"
                                    required="true" maxlength="64" />
                            </div>
                        </div>
                        <div class="form-group" name="DivFont">
                            <div class="col-md-2 control-label" for="WaterMarkFont">
                                <%="字体：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <select id="WaterMarkFont" name="WaterMarkFont" class="form-control">
                                    <%=FontOption%>
                                </select>
                            </div>
                        </div>
                        <div class="form-group" name="DivFont">
                            <div class="col-md-2 control-label" for="WaterMarkFontColor">
                                <%="字体颜色：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" value="<%=PictureConfig.WaterMarkFontColor %>" id="WaterMarkFontColor" name="WaterMarkFontColor"
                                    class="pick-a-color form-control" />
                            </div>
                            <script type="text/javascript">
                                $(document).ready(function () {
                                    $("#WaterMarkFontColor").pickAColor({
                                        showSpectrum: true,
                                        showSavedColors: true,
                                        saveColorsPerElement: true,
                                        fadeMenuToggle: true,
                                        showAdvanced: true,
                                        showBasicColors: true,
                                        showHexInput: true,
                                        allowBlank: true,
                                        inlineDropdown: true
                                    });
                                });
                            </script>
                        </div>
                        <div class="form-group" name="DivPic">
                            <div class="col-md-2 control-label" for="WaterMarkPic">
                                <%="水印图片：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input id="txt_file" value="" name="txt_file" type="file" class="file-loading" for="WaterMarkPic" />
                                <input type="hidden" id="WaterMarkPic" value="<%=PictureConfig.WaterMarkPic %>" name="WaterMarkPic" />
                                <script type="text/javascript">
                                    var config = {
                                        uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages.aspx?FormId=0&savePath=<%=UploadFilePath %>logo/&fileName=",
                                        previewFileType: "any",
                                        language: '<%=GetLoginUserLanguage()%>',
                                        allowedFileExtensions: ['jpg', 'png'],
                                        initialCaption: '<%="支持格式：".ToLang()%>jpg,png',
                                        previewClass: "bg-warning",
                                        initialPreviewAsData: true,
                                        initialPreviewFileType: 'image',
                                        pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=WaterMarkPic&ControlId=txt_file',
                                        isPic: true
                                    };
                                    if ("<%=PictureConfig.WaterMarkPic %>") {
                                        config = {
                                            uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages.aspx?FormId=0&savePath=<%=UploadFilePath %>logo/&fileName=",
                                            previewFileType: "any",
                                            language: '<%=GetLoginUserLanguage()%>',
                                            allowedFileExtensions: ['jpg', 'png'],
                                            initialCaption: '<%="支持格式：".ToLang()%>jpg,png',
                                            previewClass: "bg-warning",
                                            initialPreviewAsData: true,
                                            initialPreviewFileType:'image',
                                            initialPreview: "<%=UploadFilePath+PictureConfig.WaterMarkPic %>",
                                            initialPreviewConfig:  [{caption: '', size: 0,name:'<%=PictureConfig.WaterMarkPic%>', key: 0}],
                                            pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=WaterMarkPic&ControlId=txt_file',
                                            isPic: true
                                        };
                                    };
                                    $(function () { 
                                        $("#txt_file").fileinput(config).on("filebatchselected",
                                            function(event, files) {
                                                $(this).fileinput("upload");
                                            }).on("fileuploaded", function(event, data) {
                                            if (data.response && data.response.Result == true) {
                                                $("#WaterMarkPic").val(data.response.Msg);
                                            }
                                        }).on("fileclear", function(event, data) {
                                            $("#WaterMarkPic").val("");
                                        });
                                    });
                                </script>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 text-right" for="WaterMarkFont">
                                <%="透明度：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <span class="note_gray"><%="透明".ToLang() %>&nbsp;</span>
                                <input id="ex1" data-slider-id='ex1Slider' type="text" width="88%" data-slider-min="1"
                                    data-slider-max="255" data-slider-step="1" data-slider-value="14" />
                                <input type="hidden" id="WaterMarkTransparent" name="WaterMarkTransparent" />
                                <script type="text/javascript">
                                    $('#ex1').slider({
                                        value: <%=PictureConfig.WaterMarkTransparent.IsEmpty()?"200": PictureConfig.WaterMarkTransparent%>,
                                        width: "1000px",
                                        formatter: function(value) {
                                            $("#WaterMarkTransparent").val(value);
                                            return value;
                                        }
                                    });
                                </script>
                                <span class="note_gray">&nbsp;<%="不透明".ToLang() %></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="WaterMarkWhere">
                                <%="水印位置：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <table width="256" height="207" border="0" background="../../res/images/flower.jpg">
                                    <tr>
                                        <td width="33%" align="center" style="vertical-align: middle;">
                                            <input type="radio" id="WaterMarkWhere1" value="1" checked="true" name="WaterMarkWhere" /><b>#1</b>
                                        </td>
                                        <td width="33%" align="center" style="vertical-align: middle;">
                                            <input type="radio" id="WaterMarkWhere2" value="2" name="WaterMarkWhere" /><b>#2</b>
                                        </td>
                                        <td width="33%" align="center" style="vertical-align: middle;">
                                            <input type="radio" id="WaterMarkWhere3" value="3" name="WaterMarkWhere" /><b>#3</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="center" style="vertical-align: middle;">
                                            <input type="radio" id="WaterMarkWhere4" value="4" name="WaterMarkWhere" /><b>#4</b>
                                        </td>
                                        <td width="33%" align="center" style="vertical-align: middle;">
                                            <input type="radio" id="WaterMarkWhere5" value="5" name="WaterMarkWhere" /><b>#5</b>
                                        </td>
                                        <td width="33%" align="center" style="vertical-align: middle;">
                                            <input type="radio" id="WaterMarkWhere6" value="6" name="WaterMarkWhere" /><b>#6</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="33%" align="center" style="vertical-align: middle;">
                                            <input type="radio" id="WaterMarkWhere7" value="7" name="WaterMarkWhere" /><b>#7</b>
                                        </td>
                                        <td width="33%" align="center" style="vertical-align: middle;">
                                            <input type="radio" id="WaterMarkWhere8" value="8" name="WaterMarkWhere" /><b>#8</b>
                                        </td>
                                        <td width="33%" align="center" style="vertical-align: middle;">
                                            <input type="radio" id="WaterMarkWhere9" value="9" name="WaterMarkWhere" /><b>#9</b>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-6 ">
                                <input type="hidden" name="_action" value="Save" />
                                <%if (IsCurrentRoleMenuRes("338"))
                                  { %>
                                <button type="button" onclick="save();" form-success="refresh" class="btn btn-info btn-block"><%="保存".ToLang()%></button>
                                <%} %>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <script type="text/javascript">

            //选择事件
            $(document).ready(function () {
                $("input[name='WaterMarkStyle']").next().click(function () {
                    Change();
                });
                $("input[name='WaterMarkStyle']").parent().next().click(function () {
                    Change();
                });
            });
         
            function Change() {
                if ($("input[name=WaterMarkStyle]:checked").val() === "0") {
                    $("[name=DivFont]").show();
                    $("[name=DivPic]").hide();
                
                } else {
                    $("[name=DivFont]").hide();
                    $("[name=DivPic]").show();
               
                }
            }

            //绑定值
            var isAutoMakeWatermark = "<%=PictureConfig.IsAutoMakeWatermark%>";
            if (isAutoMakeWatermark == "True") {
                isAutoMakeWatermark = "1";
            } else {
                isAutoMakeWatermark = "0";
            }
            $("[name='IsAutoMakeWatermark'][value='" + isAutoMakeWatermark + "']").prop("checked", "checked");
            $("[name='WaterMarkStyle'][value='<%=PictureConfig.WaterMarkStyle%>']").prop("checked", "checked");
         if ("<%=PictureConfig.WaterMarkStyle%>" === "1") {
                $("[name=DivFont]").hide();
                $("[name=DivPic]").show();
            } else {
                $("[name=DivFont]").show();
                $("[name=DivPic]").hide();
            }

            $("[name='Proportion']").val("<%=PictureConfig.Proportion%>");
            $("[name='WaterMarkFont']").val("<%=PictureConfig.WaterMarkFont%>");
            $("#WaterMarkWhere<%=PictureConfig.WaterMarkWhere%>").prop("checked", "checked");

            //保存按钮
            function save() {

                if ($("input[name=WaterMarkStyle]:checked").val() === "0" && $('input[name="WaterMarkFontText"]').val() == "") {
                    whir.toastr.error('<%="水印文字为必填项".ToLang() %>');
                    return false;
                }
                if ($("input[name=WaterMarkStyle]:checked").val() === "1" && $('input[name="WaterMarkPic"]').val() == "") {
                    whir.toastr.error('<%="水印图片为必填项".ToLang() %>');
                 return false;
             }

             var $form = $("#formEdit");
             $form.post({
                 success: function (response) {
                     if (response.Status == true) {
                         whir.toastr.success(response.Message);
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
         }
       
        </script>
    </div>
</asp:Content>
