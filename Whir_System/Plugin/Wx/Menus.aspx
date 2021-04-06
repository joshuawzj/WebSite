<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="Menus.aspx.cs" Inherits="Whir_System_Plugin_Wx_Menus" %>

<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"> 
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading">
                <%="自定义菜单".ToLang()%>
                <label style="float: right;"><%="当前公众号".ToLang() %>：<%=this.CurrentCredence!=null?this.CurrentCredence.AppName+"（"+this.CurrentCredence.AppId+"）":("<a class=\"btn-empty-wx\" href=\"WxCredence.aspx\">"+"未设置当前公众号，点击进行设置".ToLang()+"</a>")%></label>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-lg-8">
                        <div class="panel panel-default">
                            <div class="panel-heading"><%="编辑菜单".ToLang()%></div>
                            <div class="panel-body">
                                <form id="formEdit" enctype="multipart/form-data" runat="server" class="form-horizontal" form-url="">
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="Title">
                                            <%="菜单名称：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <input type="text" id="Title" name="Title" class="form-control" required="true" maxlength="64" />
                                        </div>
                                    </div>
                                    <div class="form-group data-menu-type">
                                        <div class="col-md-2 control-label" for="Content">
                                            <%="内容类型：".ToLang()%>
                                        </div>
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
                                            <div class="btn-radio">
                                                <div class="iradio_flat-red" data-val="url" style="position: relative;">
                                                    <ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins>
                                                </div>
                                                <label for="square-radio-1"><%="地址".ToLang()%></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="Author">
                                            <%="内容信息：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
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
                                                <div data-type="url" class="sample">
                                                    <input type="text" placeholder="<%="请填写地址".ToLang()%>" name="url-value" class="form-control" maxlength="500" />
                                                </div>
                                                <div data-type="empty" class="sample">
                                                    <div class="ibtn-media" style="display: block;">
                                                        <p class="word" style="text-align: center;"><%="该菜单有子菜单，无法设置其内容".ToLang()%></p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-offset-2 col-md-10 text-center">
                                            <button form-action="submit" form-success="back" class="btn btn-info"><%="保存".ToLang()%></button>
                                            <a id="btn-remove" class="btn text-danger border-danger"><%="删除".ToLang()%></a>
                                        </div>
                                    </div>
                                </form>
                                <div class="space10"></div>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="panel panel-default">
                            <div class="panel-heading"><%="菜单管理".ToLang()%></div>
                            <div class="panel-body">
                                <div class="data-phone">
                                    <div class="header">
                                        <p><%=this.CurrentCredence!=null?this.CurrentCredence.AppName:"公众号名称".ToLang() %></p>
                                    </div>
                                    <div class="bottom">
                                        <ul>
                                             
                                        </ul>
                                    </div>
                                </div>
                                <div class="space10"></div>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            var menuStores = {

            };
            var uploaderConfig = {
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
            };



            var options = {
                fields: {
                    Title: {
                        validators: {
                            notEmpty: {
                                message: '<%="请填写菜单名称".ToLang() %>'
                            }
                        }
                    }
                }
            };

            // 加载菜单
            whir.loading.show();
            whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                data: {
                    _action: 'GetMenus'
                },
                success: function (response) {
                    whir.loading.remove();
                    if (response.Status == true) {
                        for (var i = 0, data = $.parseJSON(response['Message']), buttons = (data['menu'] || { button: [] })['button'], views = []; i < buttons.length; i++) {
                            var name = getUniqueName();
                            views.push('<li>');
                            views.push('   <span data-name="' + name + '">' + buttons[i]['name'] + '</span>');

                            var children = buttons[i]['sub_button'] || [];
                            views.push('   <div class="children">');
                            for (var x = 0; x < children.length; x++) {
                                var childrenName = name + '_' + x;
                                views.push('<p data-name="' + childrenName + '" df title="' + children[x]['name'] + '">' + children[x]['name'] + '</p>');
                                menuStores[childrenName] = children[x];
                            }
                            views.push('       <a class="entypo-plus"></a>');
                            views.push('       <span></span>');
                            views.push('       <span class="s1"></span>');
                            views.push('   </div>');

                            views.push('   <span class="entypo-plus"></span>');
                            views.push('</li>');
                            menuStores[name] = buttons[i];
                        }
                        if (buttons.length < 3) {
                            for (var x = 0, l = 3 - buttons.length; x < l; x++) {
                                var n = 'empty_' + getUniqueName();
                                views.push('<li>');
                                views.push('   <span data-name="' + n + '" style="display:none;"><%="菜单名称".ToLang()%></span>');
                                views.push('   <div class="children">');
                                views.push('       <a class="entypo-plus"></a>');
                                views.push('       <span></span>');
                                views.push('       <span class="s1"></span>');
                                views.push('   </div>');
                                views.push('   <span style="display:block;" class="entypo-plus"></span>');
                                views.push('</li>');
                            }
                        }
                        $('div.data-phone').find('ul').html(views.join('')).find('span[data-name],p[data-name]').each(function () {
                            bindMenuEvent(this);
                        });

                        $('div.data-phone').find('a.entypo-plus,span.entypo-plus').each(function () {
                            bindPlusEvent(this);
                        });
                        $('div.data-phone').find('ul').find('span[data-name]:visible,span.entypo-plus:visible').first().click();

                    } else {
                        //whir.toastr.error(response.Message);
                    }
                }
            });

            // 类型切换
            $('div.btn-radio').find('div.iradio_flat-red').click(function () {
                var dataVal = $(this).attr('data-val'), name = $('div.data-phone').find('span.selected,p.selected').attr('data-name'), data = menuStores[name];
                $(this).addClass('checked').parent().siblings().find('div.iradio_flat-red').removeClass('checked');
                $('div.data-item').find('> div.sample').hide().filter('[data-type="' + dataVal + '"]').show();
                fillCurrentMenuData();
                if (data['user_type'] != dataVal) {
                    $('div.data-item').find('> div.sample').filter('[data-type="' + dataVal + '"]').find('a.btn-remove').click();
                }
            });

            // 表单验证
            $("#formEdit").Validator(options, function (el) {

                fillCurrentMenuData();
                var dataName = validateStores();
                if (dataName) {
                    $('div.data-phone').find('span[data-name="' + dataName + '"],p[data-name="' + dataName + '"]').click();
                    whir.toastr.error('<%="请完善菜单信息".ToLang()%>');
                    return false;
                }
                var menus = [];
                $('div.data-phone').find('span[data-name]').each(function () {
                    var name = $(this).attr('data-name'), data = $.extend({}, menuStores[name]), children = [];
                    if (data['name']) {
                        $(this).siblings().filter('div.children').find('p').each(function () {
                            var childName = $(this).attr('data-name'), childData = $.extend({}, menuStores[childName]);
                            children.push(childData);
                        });
                        data['sub_button'] = children;
                        menus.push(data);
                    }
                });
                menus = repairPostMenus(menus);
                whir.loading.show();
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                 data: {
                     _action: "SaveMenus",
                     menus: JSON.stringify(menus)
                 },
                 success: function (response) {
                     whir.loading.remove();
                     if (response.Status == true) {
                         whir.toastr.success('<%="保存成功！".ToLang()%>');

                     } else {
                         whir.toastr.error(response.Message);
                     }
                 }
             });
             return false;
         });

         // 监控标题
         $('input[name="Title"]').keyup(function () {
             var dataType = $(this).attr('data-type'), title = $.trim($(this).val()), max = dataType == 'root' ? 8 : 16;
             var currLength = computeTextLength(title);
             if (currLength > max) {
                 whir.toastr.error('<%="长度不超过".ToLang()%>' + (max / 2) + '<%="个汉字或".ToLang()%>' + max + '<%="个字母".ToLang()%>');
                 $(this).val(title.substring(0, (max / 2)));
             }
         });

         // 删除菜单
         $('#btn-remove').click(function () {
             var node = $('div.data-phone').find('span.selected,p.selected'), name = node.attr('data-name') || '';
             if (name) {
                 var tagName = node[0].tagName.toLowerCase();
                 node.siblings().filter('.entypo-plus').css({ display: 'block' });
                 if (tagName == 'p') {
                     var nextNode = (node.siblings().filter('p').length > 0
                         ? node.siblings().filter('p').first()
                         : node.parent().siblings().filter('span:visible'));
                     node.remove();
                     nextNode.click();
                 } else {
                     node.parent().find('div.children').find('p').each(function () {
                         var name = $(this).attr('data-name');
                         $(this).remove();
                         delete menuStores[name];
                     });
                     node.parent().siblings().first().find('span:visible').click();
                     node.hide();
                 }
                 delete menuStores[name];
                 repairAddButtonBorder();
             }
             return false;
         });

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

            // 绑定菜单事件
            function bindMenuEvent(o) {
                $(o).click(function () {
                    fillCurrentMenuData();
                    $('div.data-phone').find('.selected').removeClass('selected');
                    $(this).addClass('selected');
                    var name = $(this).attr('data-name'), menu = menuStores[name];
                    if (this.tagName.toLowerCase() == 'span') {
                        $(this).parent().each(function () {
                            $(this).find('div.children').show();
                        }).siblings().find('div.children').hide();
                    }
                    fillForm(name, menu);
                });
            }

            // 绑定添加菜单事件
            function bindPlusEvent(o) {
                $(o).click(function () {
                    var name = getUniqueName(), menu = {
                        user_type: 'article',
                        type: 'view_limited',
                        name: '<%="菜单名称".ToLang()%>'
                    };
                    menuStores[name] = menu;
                    if (this.tagName.toLowerCase() == 'span') { // 第一菜单
                        $(this).hide().siblings().filter('span').attr('data-name', name).text(menu.name).show().click();
                    } else { // 二级菜单
                        var l = $(this).siblings().filter('p').length;
                        if (l == 4) {
                            $(this).hide();
                        }
                        $(this).before('<p data-name="' + name + '" style="margin: 2px;"><%="菜单名称".ToLang()%></p>').prev().each(function () {
                            bindMenuEvent(this);
                        }).click();
                    }
                    repairAddButtonBorder();
                });
            }

            // 调整添加子菜单按钮上边框
            function repairAddButtonBorder() {
                $('div.data-phone').find('a.entypo-plus').each(function () {
                    if ($(this).siblings().filter('p').length > 0) {
                        $(this).css({ borderTopStyle: 'dashed' });
                    } else {
                        $(this).css({ borderTopStyle: 'none' });
                    }
                });
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

            // 将表单信息映射到对象
            function fillCurrentMenuData() {
                if ($('div.data-phone').find('span.selected,p.selected').length > 0) {
                    var node = $('div.data-phone').find('span.selected,p.selected').first(), name = node.attr('data-name'), dataType = $('div.btn-radio').find('> div.checked').attr('data-val'), data = menuStores[name];
                    // 重置除菜单名称外的其他信息
                    //for (var item in data) {
                    //    if (item != 'name') {
                    //        data[item] = '';
                    //    }
                    //}
                    data['name'] = $.trim($('input[name="Title"]').val());
                    switch (dataType) {
                        case 'article':
                            data['type'] = 'view_limited';
                            data['media_id'] = $('input[name="article-mediaid"]').val();
                            if (data['media_id']) {
                                var rows = [];
                                $('div.data-item').find('div.sample[data-type="article"]').find('div.article').each(function () {
                                    rows.push({
                                        title: $(this).find('p').text(),
                                        image: $(this).find('img').attr('src')
                                    });
                                });
                                data['data'] = rows;
                            }
                            break;
                        case 'text':
                            data['type'] = 'click';
                            data['key'] = data['name'];
                            data['data'] = $('textarea[name="text-value"]').val();
                            break;
                        case 'image':
                            data['type'] = 'media_id';
                            data['media_id'] = $('input[name="image-mediaid"]').val();
                            if (data['media_id']) {
                                data['data'] = $('div.data-item').find('div.sample[data-type="image"]').find('div.ibtn-media').find('img').attr('src');
                            }
                            break;
                        case 'voice':
                            data['type'] = 'media_id';
                            data['media_id'] = $('input[name="voice-mediaid"]').val();
                            if (data['media_id']) {
                                data['data'] = $('div.data-item').find('div.sample[data-type="voice"]').find('div.ibtn-media').find('p').text();
                            }
                            break;
                        case 'video':
                            data['type'] = 'media_id';
                            data['media_id'] = $('input[name="video-mediaid"]').val();
                            if (data['media_id']) {
                                data['data'] = $('div.data-item').find('div.sample[data-type="video"]').find('div.ibtn-media').find('p').text();
                            }
                            break;
                        case 'url':
                            data['type'] = 'view';
                            data['url'] = $('input[name="url-value"]').val();
                            break;
                    }
                    data['user_type'] = dataType;
                    node.text(data['name']);
                    menuStores[name] = data;
                }
            }

            // 使用菜单信息填充表单
            function fillForm(name, menu) {

                var dataType = 'children', node = $('div.data-phone').find('span[data-name="' + name + '"],p[data-name="' + name + '"]'), hasChildren = node.siblings().filter('div.children').find('p').length > 0;
                $('input[name="Title"]').val(menu['name']).attr('data-type', dataType);
                if (hasChildren) {
                    // 有子菜单
                    $('div.data-menu-type').hide();
                    $('div.btn-radio').find('div.iradio_flat-red').filter('[data-val="' + menu['user_type'] + '"]').click();
                    $('div.data-item').find('> div.sample').hide().filter('[data-type="empty"]').show();
                    dataType = 'root';
                } else {
                    $('div.data-menu-type').show();
                    switch (menu['user_type']) {
                        case 'url': // 链接
                            fnFills.fillURL(menu['url'] || '');
                            break;
                        case 'article': // 图文
                            fnFills.fillArticle(menu['media_id'] || '', menu['data'] || []);
                            if (!menu['media_id']) {
                                $('#article-remove').click();
                            }
                            break;
                        case 'image': // 图片
                            fnFills.fillImage(menu['data'] || '', menu['media_id'] || '');
                            if (!menu['media_id']) {
                                $('#image-remove').click();
                            }
                            break;
                        case 'voice': // 语音
                            fnFills.fillVoice(menu['data'] || '', menu['media_id'] || '');
                            if (!menu['media_id']) {
                                $('#voice-remove').click();
                            }
                            break;
                        case 'video': // 视频
                            fnFills.fillVideo(menu['data'] || '', menu['media_id'] || '');
                            if (!menu['media_id']) {
                                $('#video-remove').click();
                            }
                            break;
                        case 'text': // 文本
                            fnFills.fillText(menu['data'] || '');
                            break;
                    }
                    $('div.btn-radio').find('div.iradio_flat-red').filter('[data-val="' + menu['user_type'] + '"]').click();
                }

            }

            // 调整提交的菜单信息
            function repairPostMenus(menus) {
                for (var i = 0; i < menus.length; i++) {
                    menus[i] = repairPostMenu(menus[i]);
                    if (menus[i]['sub_button'].length > 0) {
                        for (var x = 0, children = menus[i]['sub_button']; x < children.length; x++) {
                            children[x] = repairPostMenu(children[x]);
                        }
                    }
                }
                return menus;
            }

            // 调整单个提交的菜单信息
            function repairPostMenu(menu) {
                if (menu['sub_button'] && menu['sub_button'].length > 0) {
                    menu['type'] = null;
                    menu['key'] = null;
                    menu['url'] = null;
                    menu['media_id'] = null;
                    menu['user_type'] = null;
                } else {
                    switch (menu['user_type']) {
                        case 'article':
                        case 'image':
                        case 'voice':
                        case 'video':
                            menu['url'] = null;
                            menu['key'] = null;
                            menu['data'] = null;
                            break;
                        case 'text':
                            menu['url'] = null;
                            menu['media_id'] = null;
                            break;
                        case 'url':
                            menu['key'] = null;
                            menu['media_id'] = null;
                            menu['data'] = null;
                            break;
                    }
                }
                return menu;
            }

            // 验证菜单数据
            function validateStores() {
                var dataName = '';
                for (var name in menuStores) {
                    var data = menuStores[name], validateFields = ['name'], node = $('div.data-phone').find('span[data-name="' + name + '"],p[data-name="' + name + '"]'), hasChildren = node.siblings().filter('div.children').find('p').length > 0;
                    if (!hasChildren) {
                        switch (data['type']) {
                            case 'view':
                                validateFields.push('url');
                                break;
                            case 'view_limited':
                            case 'media_id':
                                validateFields.push('media_id');
                                break;
                            case 'click':
                                validateFields.push('key');
                                validateFields.push('data');
                                break;
                        }
                    }
                    for (var i = 0; i < validateFields.length; i++) {
                        if ($.trim(data[validateFields[i]]) == '') {
                            dataName = name;
                        }
                    }
                }

                return dataName;
            }

            // 获取唯一名号
            function getUniqueName() {
                var l = this.___length || 0;
                this.___length = l + 1;
                return Math.random().toString().replace('.', '') + l;
            }
        });
    </script>
</asp:Content>

