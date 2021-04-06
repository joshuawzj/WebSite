<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Setting.aspx.cs" Inherits="Whir_System_Module_Setting_Setting" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <script type="text/javascript">
        $(function () {

            showEditorCodeRow();
            showSystemLoginBg();

            $("[name='Editor']").next().click(function () {
                showEditorCodeRow();
            });

            $("[name='SystemLoginBgType']").next().click(function () {
                showSystemLoginBg();
            });
        });

        function showSystemLoginBg() {
            $("#SystemLoginBgForMap").hide();
            $("#SystemLoginBgForCustom").hide();
            $("#SystemLoginBgForRandom").hide();
            var value = $("[name='SystemLoginBgType']:checked").val();
            if (value == 0)
                $("#SystemLoginBgForMap").show();
            else if (value == 1)
                $("#SystemLoginBgForCustom").show();
            else if (value == 2)
                $("#SystemLoginBgForRandom").show();
        }

        function showEditorCodeRow() {
            var value = $("[name='Editor']:checked").val();
            if (value == "ewebeditor") {
                $("#trEditorCode").show();
            } else {
                $("#trEditorCode").hide();
            }
        }
    </script>
    <style type="text/css">
.tooltip-inner{  
    color:#113f6c;  
    font-size: 1.2em;  
    padding: 4px;  
    background-color: #fff;  
} 
.tooltip.bottom .tooltip-arrow{  
    border-bottom-color: #fff;  
}  
    </style>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
        <div class="panel-heading"><%="系统设置".ToLang()%></div>
            <div class="panel-body">
                <div class="form_center">
                    <div id="SystemConfig" class="tab-pane">
                        <form id="formSystemConfig" class="form-horizontal" form-url="<%=SysPath %>Handler/Setting/SeoSetting.aspx">
                            <div class="form-group">
                                <div class="col-md-3 text-right" for="SystemLoginBgForMap">
                                    <%="前台页面置灰：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                   <ul class="list">
                                        <li>
                                            <input type="radio" name="IsRemoveColor" value="1" <%=IsRemoveColor?"checked=\"checked\"":"" %>   />
                                            <%="开启".ToLang()%>
                                        </li>
                                        <li>
                                            <input type="radio" name="IsRemoveColor" value="0" <%=!IsRemoveColor?"checked=\"checked\"":"" %>   />
                                            <%="关闭".ToLang()%>
                                        </li>
                                       </ul>
                                </div>
                            </div>
                            <div class="form-group" name="DivFont">
                                <div class="col-md-3 text-right" for="SystemLoginCopyright">
                                    <%="登录页背景：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <ul class="list">
                                        <li>
                                            <input type="radio" name="SystemLoginBgType" value="0" /> <%="地图背景".ToLang()%>
                                        </li>
                                         <li>
                                             <input type="radio" name="SystemLoginBgType" value="1" /> <%="自定义背景图片".ToLang()%>
                                        </li>
                                         <li>
                                             <input type="radio" name="SystemLoginBgType" value="2" /> <%="随机幻灯片".ToLang()%>
                                        </li>
                                    </ul>
                                  
                                </div>
                            </div>

                             <div class="form-group"  id="SystemLoginBgForMap">
                                <div class="col-md-3 control-label" for="SystemLoginBgForMap">
                                   <%="地图坐标：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <input type="text" name="SystemLoginBgForMap" value="<%=SystemConfig.SystemLoginBgForMap %>"
                                    class="form-control" maxlength="200" />
                                    <span class="note_gray"><%="输入地图坐标，例如：113.379361, 23.128169".ToLang()%></span>
                                </div>
                            </div>

                            <div class="form-group" id="SystemLoginBgForCustom">
                                <div class="col-md-3 control-label" for="SystemLoginBgForCustom">
                                   <%="上传背景图片：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                     <input id="txt_file" value="" name="txt_file" type="file" class="file-loading" for="ImageUrl"/>
                                        <script>
                                            var config = {
                                                uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages.aspx?FormId=0&savePath=<%= UploadFilePath%>logo/&fileName=",
                                                previewFileType: "any",
                                                initialCaption: '<%="支持格式：".ToLang()%>jpg|png|gif',
                                                language: '<%=GetLoginUserLanguage()%>',
                                                allowedFileExtensions: ['jpg', 'png', 'gif'],
                                                previewClass: "bg-warning",
                                                initialPreviewAsData: true,
                                                initialPreviewFileType: 'image',
                                                pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=ImageUrl&ControlId=txt_file',
                                                isPic: true
                                            };

                                            if ("<%=SystemConfig.SystemLoginBgForCustom  %>") {
                                                config = {
                                                    uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages.aspx?FormId=0&savePath=<%= UploadFilePath%>logo/&fileName=",
                                                    previewFileType: "any",
                                                    initialCaption: '<%="支持格式：".ToLang()%>jpg|png|gif',
                                                    language: '<%=GetLoginUserLanguage()%>',
                                                    allowedFileExtensions: ['jpg', 'png', 'gif'],
                                                    previewClass: "bg-warning",
                                                    initialPreviewFileType: 'image',
                                                    initialPreviewAsData: true,
                                                    initialPreview: "<%=UploadFilePath+SystemConfig.SystemLoginBgForCustom %>",
                                                    initialPreviewConfig: [{ caption: '', size: 0, name: '<%=SystemConfig.SystemLoginBgForCustom%>', key: 0 }],
                                                    pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=ImageUrl&ControlId=txt_file',
                                                    isPic: true
                                                };
                                            };
                                            $(function () {
                                                $("#txt_file")
                                                    .fileinput(config)
                                                    .on("filebatchselected",
                                                    function (event, files) {
                                                        $(this).fileinput("upload");

                                                    })
                                                    .on("fileuploaded",
                                                    function (event, data) {
                                                        if (data.response && data.response.Result == true) {
                                                            $("#ImageUrl").val(data.response.Msg).change();

                                                        }
                                                    })
                                                    .on("fileclear",
                                                    function (event, data) {
                                                        $("#ImageUrl").val("");
                                                    });

                                            });
                                        </script>

                                      <input type="hidden" id="ImageUrl"  name="SystemLoginBgForCustom" value="<%=SystemConfig.SystemLoginBgForCustom %>"
                                    class="form-control" maxlength="200" />
                                </div>
                            </div>

                            <div class="form-group"  id="SystemLoginBgForRandom" >
                                <div class="col-md-3 control-label" for="SystemLoginBgForRandom">
                                   <%="幻灯片切换时间：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                      <input type="number" name="SystemLoginBgForRandom" value="<%=SystemConfig.SystemLoginBgForRandom %>"
                                    class="form-control" maxlength="12" />
                                     <span class="note_gray"><%="输入幻灯片切换时间，单位毫秒，例如：2000，即2秒".ToLang()%></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 control-label" for="HistoryNum">
                                    <%="历史记录数量：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <input type="text" id="HistoryNum" name="HistoryNum" value="<%=SystemConfig.HistoryNum %>"
                                        class="form-control" maxlength="4" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 control-label" for="PageSetting">
                                    <%="分页范围：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <div class="input-group">
                                        <input type="text" id="PageSetting" name="PageSetting" value="<%=SystemConfig.PageSetting%>" class="form-control" maxlength="100" />
                                        <span class="input-group-addon"><%="示例：".ToLang()%>15|20|30|50</span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 control-label" for="ListTextLength">
                                    <%="列表显示字符长度：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <input type="text" id="ListTextLength" name="ListTextLength" min="10" required="true" value="<%=SystemConfig.ListTextLength %>"
                                        class="form-control" />
                                </div>
                            </div>
                             <div class="form-group">
                                <div class="col-md-3 control-label" for="ListTextLength">
                                    <%="Banner图提示文字：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <input type="text" id="BannerTip" name="BannerTip"  value="<%=SystemConfig.BannerTip %>"
                                        class="form-control" />
                                </div>
                            </div>
                             <div class="form-group">
                                <div class="col-md-3 control-label" for="ListTextLength">
                                    <%="移动端Banner图提示文字：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <input type="text" id="SmallBannerTip" name="SmallBannerTip"  value="<%=SystemConfig.SmallBannerTip %>"
                                        class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 text-right" for="Editor">
                                    <%="编辑器类型：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <ul class="list">
                                        <li>
                                            <input type="radio" id="Editor_KindEditor" name="Editor" value="kindeditor" />
                                            <label data-toggle="tooltip" data-placement="top" title="<img src='<%=SysPath %>Res/images/editor/kindeditor.png' style='width:600px'>" data-html="true" for="Editor_KindEditor"><%="KindEditor".ToLang()%></label>
                                        </li>
                                        <li>
                                            <input type="radio" id="Editor_eWebEditor" name="Editor" value="ewebeditor" />
                                            <label data-toggle="tooltip" data-placement="top" title="<img src='<%=SysPath %>Res/images/editor/ewebeditor.png' style='width:600px'>" data-html="true" for="Editor_eWebEditor"><%="eWebEditor（收费）".ToLang()%></label>
                                        </li>
                                        <li>
                                            <input type="radio" id="Editor_UEditor" name="Editor" value="ueditor" />
                                            <label data-toggle="tooltip" data-placement="top" title="<img src='<%=SysPath %>Res/images/editor/ueditor.png' style='width:600px'>" data-html="true" for="Editor_UEditor"><%="百度编辑器".ToLang()%></label>
                                        </li>
                                        <li>
                                            <input type="radio" id="Editor_WuEditor" name="Editor" value="wueditor" />
                                            <label data-toggle="tooltip" data-placement="top" title="<img src='<%=SysPath %>Res/images/editor/wueditor.png' style='width:600px'>" data-html="true" for="Editor_WuEditor"><%="微信编辑器".ToLang()%></label>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="form-group" id="trEditorCode">
                                <div class="col-md-3 control-label" for="EditorCode">
                                    <%="eWebeditor授权码：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <div class="input-group">
                                        <input type="text" id="EditorCode" name="EditorCode" value="<%=EditorCode %>" class="form-control" />
                                        <a class="input-group-addon btn-success" href="http://www.ewebeditor.net/licensing.asp" target="_blank"><%="获取授权码".ToLang()%></a>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-3 col-md-9 ">
                                    <input type="hidden" name="_action" value="SaveSysSetting" />
                                     <%if (IsCurrentRoleMenuRes("371")) { %>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info btn-block"><%="保存".ToLang()%></button>
                                <%} %>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $("[name=Editor][value=<%=SystemConfig.Editor %>]").prop("checked", "checked");
        $("[name='SystemLoginBgType'][value='<%=SystemConfig.SystemLoginBgType%>']").prop("checked", "checked");

        //提交内容
        var options = {
            fields: {
                HistoryNum: {
                    validators: {
                        regexp: {
                            regexp: /^[1-9]\d*$/,
                            message: '<%="历史记录数量为正整数".ToLang() %>'
                        }
                    }
                }, PageSetting: {
                    validators: {
                        regexp: {
                            regexp: /(\+?[1-9][0-9]{0,8}\|){0,6}(\+?[1-9][0-9]{0,8})/,
                            message: '<%="格式不正确或内容太长，每页显示数字不要超过10位数，分页选项不要超过6组".ToLang() %>'
                        }
                    }
                }, ListTextLength: {
                    validators: {
                        regexp: {
                            regexp: /^((1[1-9])|([2-9]\d)|([1-9]\d{2,}))$/,
                            message: '<%="必须为大于10的整数".ToLang() %>'
                        }
                    }
                }
            }

        };


        $('#formSystemConfig').Validator(options,
          function () {
              var actionSuccess = $(this).attr("form-success");
              var $form = $("#formSystemConfig");
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
          });
    </script>
</asp:content>
