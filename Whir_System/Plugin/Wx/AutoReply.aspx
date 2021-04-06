
<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="AutoReply.aspx.cs" Inherits="Whir_System_Plugin_Wx_AutoReply" %>

<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link rel="stylesheet" href="Styles/jquery.steps.css" />
    <link rel="stylesheet" href="Styles/style.css" />
    <style type="text/css">
        .file-live-thumbs {
            height: 300px;
            overflow-y: auto;
        }

        .bg-warning {
            display: none;
        }

        .btn.btn-primary.btn-file {
            height: 100%;
            left: 0;
            position: absolute;
            top: 0;
            width: 100%;
            opacity: 0;
        }

        .tabcontrol > .steps > ul > li {
            margin-left: 10px;
        }

        div.data-item {
            margin-top: 0;
        }

        #wizard-tab {
            position: relative;
        }
        .tabcontrol > .content {
            height: auto;
        }

        .tabcontrol > .content > .body {
            position: relative;
            height: auto;
        }
    </style>
   

    <script src="<%=SysPath %>res/js/jquery_sorttable/core.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/widget.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/mouse.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/sortable.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/whir.common.js"></script>
 
    <script type="text/javascript">
        whir.wx.setBasePath('<%=SysPath%>');
        //批量选中
        function selectAction() {
            if (!whir.checkbox.isSelect('cb_Position')) {
                TipMessage('<%="请选择".ToLang()%>');
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <textarea id="data-json" style="display:none;"><%=this.DataJSON %></textarea>
    <div class="space15"></div>
    <div class="content-wrap" style="padding:0 14px;">
    <div class="row">
        <div class="col-lg-12">
            <div class="panel">
                    <div class="panel-body">
                        <ul class="nav nav-tabs">
                            <li><a href="Reply.aspx"><%="关注自动回复".ToLang()%></a></li>
                            <li class="active"><a href="AutoReply.aspx"><%="消息自动回复".ToLang()%></a></li>
                            <li><a href="KeywordReply.aspx"><%="关键词自动回复".ToLang()%></a></li>
                            <label style="float: right;"><%="当前公众号".ToLang() %>：<%=this.CurrentCredence!=null?this.CurrentCredence.AppName+"（"+this.CurrentCredence.AppId+"）":("<a class=\"btn-empty-wx\" href=\"WxCredence.aspx\">"+"未设置当前公众号，点击进行设置".ToLang()+"</a>")%></label>
                        </ul>
                        <div class="space15"></div>
                        <div class="form_center">
                                <form id="formEdit" enctype="multipart/form-data" runat="server" class="form-horizontal" form-url="">
                                <div class="form-group data-menu-type">
                                    <div class="col-md-2 control-label" for="Content">
                                        <%="回复类型：".ToLang()%></div>
                                    <div class="col-md-10 ">
                                        <div class="btn-radio">
                                            <div class="iradio_flat-red checked" data-val="article" style="position: relative;">
                                                <ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins>
                                            </div>
                                            <label for="square-radio-1"><%="图文".ToLang()%></label>
                                        </div>
                                        <div class="btn-radio">
                                            <div class="iradio_flat-red" data-val="text" style="position: relative;">
                                                <ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins>
                                            </div>
                                            <label for="square-radio-1"><%="文字".ToLang()%></label>
                                        </div>
                                        <div class="btn-radio">
                                            <div class="iradio_flat-red" data-val="image" style="position: relative;">
                                                <ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins>
                                            </div>
                                            <label for="square-radio-1"><%="图片".ToLang()%></label>
                                        </div>
                                        <div class="btn-radio">
                                            <div class="iradio_flat-red" data-val="voice" style="position: relative;">
                                                <ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins>
                                            </div>
                                            <label for="square-radio-1"><%="语音".ToLang()%></label>
                                        </div>
                                        <div class="btn-radio">
                                            <div class="iradio_flat-red" data-val="video" style="position: relative;">
                                                <ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins>
                                            </div>
                                            <label for="square-radio-1"><%="视频".ToLang()%></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <div class="data-item">
                                            <div data-type="article" class="sample">
                                                <div id="btn-choice-article" class="ibtn">
                                                    <span class="entypo-newspaper"></span>
                                                    <label><%="选择图文素材".ToLang()%></label>
                                                </div>
                                                <a class="ibtn" href="EditArticle.aspx" target="_blank">
                                                    <span class="entypo-publish"></span>
                                                    <label><%="添加图文素材".ToLang()%></label>
                                                </a>
                                                <div class="ibtn-media">
                                                    <div class="article">
                                                        <img src="/WebSite/uploadfiles/ban.jpg" alt="" />
                                                        <p><%="文章标题".ToLang()%></p>
                                                    </div>
                                                    <div class="article">
                                                        <img src="/WebSite/uploadfiles/ban.jpg" alt="" />
                                                        <p><%="文章标题".ToLang()%></p>
                                                    </div>
                                                    <div class="article">
                                                        <img src="/WebSite/uploadfiles/ban.jpg" alt="" />
                                                        <p><%="文章标题".ToLang()%></p>
                                                    </div>
                                                    <a id="article-remove" class="entypo-cancel btn-remove"></a>
                                                    <input type="hidden" name="article-mediaid" />
                                                </div>
                                            </div>
                                            <div data-type="video" class="sample">
                                                <div id="btn-choice-video" class="ibtn">
                                                    <span class="entypo-video"></span>
                                                    <label><%="选择视频素材".ToLang()%></label>
                                                </div>
                                                <div class="ibtn">
                                                    <span class="entypo-publish"></span>
                                                    <label><%="上传视频素材".ToLang()%></label>
                                                    <input id="video-data-file" name="video-data-file" type="file" />
                                                </div>
                                                <div class="ibtn-media">
                                                    <p class="word"></p>
                                                    <a id="video-remove" class="entypo-cancel btn-remove"></a>
                                                    <input type="hidden" name="video-mediaid" />
                                                </div>
                                            </div>
                                            <div data-type="voice" class="sample">
                                                <div id="btn-choice-voice" class="ibtn">
                                                    <span class="entypo-bell"></span>
                                                    <label><%="选择语音素材".ToLang()%></label>
                                                </div>
                                                <div class="ibtn">
                                                    <span class="entypo-publish"></span>
                                                    <label><%="上传语音素材".ToLang()%></label>
                                                    <input id="voice-data-file" name="voice-data-file" type="file" />
                                                </div>
                                                <div class="ibtn-media">
                                                    <p class="word"></p>
                                                    <a id="voice-remove" class="entypo-cancel btn-remove"></a>
                                                    <input type="hidden" name="voice-mediaid" />
                                                </div>
                                            </div>
                                            <div data-type="image" class="sample">
                                                <div id="btn-choice-image" class="ibtn">
                                                    <span class="entypo-picture"></span>
                                                    <label><%="选择图片素材".ToLang()%></label>
                                                </div>
                                                <div class="ibtn">
                                                    <span class="entypo-publish"></span>
                                                    <label><%="上传图片素材".ToLang()%></label>
                                                    <input id="image-data-file" name="image-data-file" type="file" />
                                                </div>
                                                <div class="ibtn-media">
                                                    <img src="/WebSite/uploadfiles/ban.jpg" alt="" />
                                                    <a id="image-remove" class="entypo-cancel btn-remove"></a>
                                                    <input type="hidden" name="image-mediaid" />
                                                </div>
                                            </div>
                                            <div data-type="text" class="sample">
                                                <textarea placeholder="<%="请填写内容".ToLang()%>" name="text-value" class="form-control" style="height: 100px;" maxlength="600"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div >
                                    <button id="btn-save" class="btn btn-info btn-block"><%="保存".ToLang()%></button>
                                </div>

                            </form>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

     $(document).ready(function () {

         var id = '<%=this.ViewData.Whir_Wx_ReplyId%>', uploaderConfig = {
             image: {
                 extensions: ['jpg', 'jpeg', 'png', 'gif', 'bmp'],
                 caption: '<%="支持格式：".ToLang()%>bmp/png/jpeg/jpg/gif'
             },
             voice: {
                 extensions: ['mp3', 'wma', 'wav', 'amr'],
                 caption: '<%="支持格式：".ToLang()%>mp3/wma/wav/amr'
             },
             video: {
                 extensions: ['rm', 'rmvb', 'wmv', 'avi', 'mpg', 'mpeg', 'mp4'],
                 caption: '<%="支持格式：".ToLang()%>rm/rmvb/wmv/avi/mpg/mpeg/mp4'
             }
         }, dataStore = $.parseJSON($('#data-json').val() || '{}');

         var fnFills = {
             fillImage: function (url, mediaId) {
                 $('input[name="image-mediaid"]').val(mediaId);
                 $('#image-remove').prev().attr('src', url).parents('div.ibtn-media:first').show().siblings().hide();
             },
             fillVoice: function (name, mediaId) {
                 $('input[name="voice-mediaid"]').val(mediaId);
                 $('#voice-remove').prev().text(name).parents('div.ibtn-media:first').show().siblings().hide();
             },
             fillVideo: function (name, mediaId) {
                 $('input[name="video-mediaid"]').val(mediaId);
                 $('#video-remove').prev().text(name).parents('div.ibtn-media:first').show().siblings().hide();
             },
             fillArticle: function (mediaId, rows) {
                 $('input[name="article-mediaid"]').val(mediaId);
                 $('#article-remove').parents('div.ibtn-media:first').show().siblings().hide();
                 for (var i = 0, views = []; i < rows.length; i++) {
                     views.push('<div class="article">');
                     views.push('   <img src="' + rows[i]['image'] + '" alt="">');
                     views.push('   <p>' + rows[i]['title'] + '</p>');
                     views.push('</div>');
                 }
                 $('#article-remove').prevAll('.article').remove();
                 $('#article-remove').before(views.join(''));
             },
             fillURL: function (url) {
                 $('input[name="url-value"]').val(url);
             },
             fillText: function (text) {
                 $('textarea[name="text-value"]').val(text);
             }
         };

         // 保存
         $('#btn-save').click(function () {
             var dataType = $('div.btn-radio').find('> div.checked').attr('data-val'), data = { _action: 'SaveReply', id: id, replyType: '2' };
             switch (dataType) {
                 case 'text':
                     if ($.trim($('textarea[name="text-value"]').val()) == '') {
                         $('textarea[name="text-value"]').focus();
                         whir.toastr.error('<%="请填写内容".ToLang()%>');
                         return false;
                     }
                     data['dataType'] = 4;
                     data['message'] = $.trim($('textarea[name="text-value"]').val());
                     break;
                 case 'article':
                     if ($.trim($('input[name="article-mediaid"]').val()) == '') {
                         whir.toastr.error('<%="请选择图文信息".ToLang()%>');
                         return false;
                     }
                     data['dataType'] = 5;
                     data['message'] = $.trim($('input[name="article-mediaid"]').val());
                     break;
                 case 'image':
                     if ($.trim($('input[name="image-mediaid"]').val()) == '') {
                         whir.toastr.error('<%="请选择图片信息".ToLang()%>');
                         return false;
                     }
                     data['dataType'] = 1;
                     data['message'] = $.trim($('input[name="image-mediaid"]').val());
                     break;
                 case 'voice':
                     if ($.trim($('input[name="voice-mediaid"]').val()) == '') {
                         whir.toastr.error('<%="请选择语音信息".ToLang()%>');
                         return false;
                     }
                     data['dataType'] = 2;
                     data['message'] = $.trim($('input[name="voice-mediaid"]').val());
                     break;
                 case 'video':
                     if ($.trim($('input[name="video-mediaid"]').val()) == '') {
                         whir.toastr.error('<%="请选择视频信息".ToLang()%>');
                         return false;
                     }
                     data['dataType'] = 3;
                     data['message'] = $.trim($('input[name="video-mediaid"]').val());
                     break;
             }

             whir.loading.show();
             whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                 data: data,
                 success: function (response) {
                     whir.loading.remove();
                     if (response.Status == true) {
                         whir.toastr.success('<%="保存成功".ToLang()%>');
                         id = response.Message;
                     } else {
                         whir.toastr.error(response.Message);
                     }
                 }
             });

         });

         // 类型切换
         $('div.btn-radio').find('div.iradio_flat-red').click(function () {
             var dataVal = $(this).attr('data-val'), name = $('div.data-phone').find('span.selected,p.selected').attr('data-name');
             $(this).addClass('checked').parent().siblings().find('div.iradio_flat-red').removeClass('checked');
             $('div.data-item').find('> div.sample').hide().filter('[data-type="' + dataVal + '"]').show();
         }).filter('[data-val="' + (id != '0' ? '<%=this.ViewData.ReplyDataType%>' : 'article') + '"]').click();

         //------------- 图片相关----------------//

         // 选择图片
         $('#btn-choice-image').click(function () {
             whir.wx.loadMedias('image', function (data) {
                 fnFills.fillImage(data['url'], data['mediaId']);
             });
         });

         // 上传图片
         renderUploader('image-data-file', 'image', function (data) {
             var result = data.split('|');
             fnFills.fillImage((result[2] || result[1]), result[0]);
         });

         // 删除图片
         $('#image-remove').click(function () {
             $(this).parents('div.ibtn-media:first').hide().siblings().show();
             $('input[name="image-mediaid"]').val('');
         });

         //------------- 语音相关----------------//

         // 选择语音
         $('#btn-choice-voice').click(function () {
             whir.wx.loadMedias('voice', function (data) {
                 fnFills.fillVoice(data['name'] || '<%="标题".ToLang()%>', data['mediaId']);
             });
         });

         // 上传语音
         renderUploader('voice-data-file', 'voice', function (data) {
             var result = data.split('|');
             fnFills.fillVoice(result[3] || '<%="标题".ToLang()%>', result[0]);
         });

         // 删除语音
         $('#voice-remove').click(function () {
             $(this).parents('div.ibtn-media:first').hide().siblings().show();
             $('input[name="voice-mediaid"]').val('');
         });

         //------------- 视频相关----------------//

         // 选择视频
         $('#btn-choice-video').click(function () {
             whir.wx.loadMedias('video', function (data) {
                 fnFills.fillVideo(data['name'] || '<%="标题".ToLang()%>', data['mediaId']);
             });
         });

         // 上传视频
         renderUploader('video-data-file', 'video', function (data) {
             var result = data.split('|');
             fnFills.fillVideo(result[3] || '<%="标题".ToLang()%>', result[0]);
         });

         // 删除视频
         $('#video-remove').click(function () {
             $(this).parents('div.ibtn-media:first').hide().siblings().show();
             $('input[name="video-mediaid"]').val('');
         });

         //------------- 图文相关----------------//

         // 选择图文
         $('#btn-choice-article').click(function () {
             whir.wx.loadMedias('news', function (data) {
                 fnFills.fillArticle(data['mediaId'], data['set']);
             });
         });

         // 删除图文
         $('#article-remove').click(function () {
             $(this).parents('div.ibtn-media:first').hide().siblings().show();
             $('input[name="article-mediaid"]').val('');
         });

         // 实例化上传控件
         function renderUploader(id, mediaType, callback) {
             $('#' + id).fileinput({
                 language: '<%=GetLoginUserLanguage()%>',
                 uploadUrl: "<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx?",
                 allowedFileExtensions: uploaderConfig[mediaType]['extensions'],
                 initialCaption: uploaderConfig[mediaType]['caption'],
                 previewClass: "bg-warning",
                 initialPreviewAsData: true,
                 initialPreviewFileType: 'image',
                 noPicker: true,
                 showCancel: false,
                 showRemove: false,
                 showUpload: false,
                 showCaption: false,
                 showUploadedThumbs: false,
                 uploadExtraData: { _action: 'UploadMedia', media: mediaType }
             }).on("filebatchselected", function (event, files) {
                 $(this).fileinput("upload");
             }).on("fileuploaded", function (event, data) {
                 if (data.response.Status && data.response.Message) {
                     if (typeof callback == 'function') {
                         callback(data.response.Message);
                     }
                 } else {
                     whir.toastr.error(data.response.Message);
                 }
                 $('div.kv-upload-progress').hide();
             });
         }

         // 使用菜单信息填充表单
         function fillForm(media) {
             
             switch (media['ReplyDataType']) {
                 case 'article': // 图文
                     fnFills.fillArticle(media['MessageDetail'] || '', media['Data'] || []);
                     if (!media['MessageDetail']) {
                         $('#article-remove').click();
                     }
                     break;
                 case 'image': // 图片
                     fnFills.fillImage(media['Data'] || '', media['MessageDetail'] || '');
                     if (!media['MessageDetail']) {
                         $('#image-remove').click();
                     }
                     break;
                 case 'voice': // 语音
                     fnFills.fillVoice(media['Data'] || '', media['MessageDetail'] || '');
                     if (!media['MessageDetail']) {
                         $('#voice-remove').click();
                     }
                     break;
                 case 'video': // 视频
                     fnFills.fillVideo(media['Data'] || '', media['MessageDetail'] || '');
                     if (!media['MessageDetail']) {
                         $('#video-remove').click();
                     }
                     break;
                 case 'text': // 文本
                     fnFills.fillText(media['MessageDetail'] || '');
                     break;
             }
             $('div.btn-radio').find('div.iradio_flat-red').filter('[data-val="' + media['ReplyDataType'] + '"]').click();

         }

         // 计算文本长度
         function computeTextLength(text) {
             for (var i = 0, length = 0; i < text.length; i++) {
                 var code = text.charCodeAt(i);
                 if (code >= 0 && code <= 128) {
                     length += 1;
                 } else {
                     length += 2;
                 }
             }
             return length;
         }

         // 获取唯一名号
         function getUniqueName() {
             var l = this.___length || 0;
             this.___length = l + 1;
             return Math.random().toString().replace('.', '') + l;
         }

         $('div.wrap-fluid > div.container-fluid').css({
             minHeight: ($('#skin-select').height() - 50) + 'px'
         });

         // 填充表单
         fillForm(dataStore);
     });
 </script>

</asp:content>

