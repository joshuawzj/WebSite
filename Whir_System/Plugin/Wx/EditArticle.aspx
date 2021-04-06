<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="EditArticle.aspx.cs" Inherits="Whir_System_Plugin_Wx_EditArticle" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Service" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="Styles/style.css" />
    <style type="text/css">
        table td {
            border-bottom: 1px solid #EAEFF2;
        }

        table td {
            padding: 9px 4px;
        }

        .ke-tabs-li:first-child {
            display: none;
        }
    </style>
    <script type="text/javascript" src="Scripts/whir.common.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="Articles.aspx" aria-expanded="true"><%="图文素材".ToLang() %></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%="编辑素材".ToLang()%></a></li>
                    <label style="float: right;"><%="当前公众号".ToLang() %>：<%=this.CurrentCredence!=null?this.CurrentCredence.AppName+"（"+this.CurrentCredence.AppId+"）":("<a class=\"btn-empty-wx\" href=\"WxCredence.aspx\">"+"未设置当前公众号，点击进行设置".ToLang()+"</a>")%></label>
                </ul>
                <div class="space15"></div>

                <div class="row">
                    <div class="col-lg-9">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <%="图文编辑".ToLang()%>
                            </div>
                            <form id="formEdit" enctype="multipart/form-data" runat="server" class="form-horizontal" form-url="">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="Title">
                                            <%="标题：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <input type="text" id="Title" name="Title" class="form-control" required="true" maxlength="64" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="Author">
                                            <%="作者：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <input type="text" id="Author" name="Author" class="form-control" maxlength="50" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="OriginalDetail">
                                            <%="内容：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                            <textarea id="OriginalDetail" name="OriginalDetail" class="form-control" style="height: 300px;"></textarea>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="Summary">
                                            <%="摘要：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <textarea id="Summary" name="Summary" class="form-control" style="height: 80px;" maxlength="120"></textarea>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="ThumbMediaId">
                                            <%="封面：".ToLang()%>
                                        </div>
                                        <div class="col-md-10">
                                            <input type="hidden" id="ThumbMediaId" name="ThumbMediaId" class="form-control" />
                                            <input type="hidden" id="ThumbMediaURL" name="ThumbMediaURL" class="form-control" />
                                            <input type="hidden" id="ThumbMediaWebURL" name="ThumbMediaWebURL" class="form-control" />
                                            <div id="btn-choice-image" class="btn btn-white btn-file">
                                                <i class="glyphicon glyphicon-folder-open"></i>&nbsp;  
                                                <span class="hidden-xs"><%="选择图片".ToLang() %> …</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="Summary">
                                            <%="显示封面：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <div class="btn-radio">
                                                <div class="iradio_flat-red checked" data-val="1" style="position: relative;">
                                                    <ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins>
                                                </div>
                                                <label for="square-radio-1"><%="显示".ToLang()%></label>
                                            </div>
                                            <div class="btn-radio">
                                                <div class="iradio_flat-red" data-val="0" style="position: relative;">
                                                    <ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins>
                                                </div>
                                                <label for="square-radio-1"><%="不显示".ToLang()%></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="SourceURL">
                                            <%="原文链接：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <input type="text" id="SourceURL" name="SourceURL"
                                                class="form-control" maxlength="500" />
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-footer">
                                    <div class="col-md-offset-2 col-md-10 ">
                                        <div class="btn-group">
                                            <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                            <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                        </div>
                                        <a class="btn btn-white" href="Articles.aspx"><%="返回".ToLang()%></a>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                    <div class="col-lg-3">
                        <div class="panel panel-default">
                            <div class="panel-heading"><%="图文列表".ToLang()%></div>
                            <div class="panel-body">
                                <div class="data-articles">
                                </div>
                                <div id="data-plus" class="data-plus entypo-plus"></div>
                                <div class="space10"></div>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" charset="utf-8" src="<%=AppName%>Editor/KindEditor/kindeditor.js"></script>
    <script type="text/javascript" charset="utf-8" src="<%=AppName%>Editor/KindEditor/lang/zh_CN.js"></script>
    <script type="text/javascript" charset="utf-8" src="<%=AppName%>Editor/KindEditor/plugins/code/prettify.js"></script>
    
    <script type="text/javascript">
        var kindEditor = null;
        
        $(document).ready(function () {
            // $("#txtOriginalDetail").val(ueContent.getContent());
            //?_action=KindEditorImageUploader
            whir.wx.setBasePath('<%=SysPath%>');
            var dataStores = {

            }, currName = '', mediaId = '<%=this.MediaId%>';

        KindEditor.ready(function (K) {
            kindEditor = K.create('#OriginalDetail', {
                urlType: 'domain', cssPath: '<%=AppName%>Editor/kindEditor/plugins/code/prettify.css',
                uploadJson: '<%=AppName%>Whir_System/Plugin/Wx/Ajax/Ajax.aspx',
                //fileManagerJson: '<%=AppName%>Editor/kindEditor/asp.net/file_manager_json.ashx',
                items: [
                    'justifyleft', 'justifycenter', 'justifyright',
                    'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
                    'superscript', 'clearhtml', 'fullscreen',
                    'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
                    'italic', 'underline', 'strikethrough', 'removeformat', '|', 'image',
                    'table', 'hr'
                ],
                extraFileUploadParams: { _action: 'KindEditorImageUploader' },
                allowFileManager: false,
                filterMode: false,
                imageTabIndex: 1,
                afterChange: function () {
                    $('#OriginalDetail').val(this.html());
                }
            });
            });

            // 添加页面
            $('#data-plus').click(function () {
                fillCurrentPageData();
                addPage({
                    ArticleIndex: $('div.data-articles').find('div.article').length
                });
                $('div.data-articles').find('div.article:last').click();
            });

            // 显示切换
            $('div.btn-radio').find('div.iradio_flat-red').click(function () {
                $('div.btn-radio').find('div.iradio_flat-red').removeClass('checked');
                $(this).addClass('checked');
            });

            // 选择图片
            $('#btn-choice-image').click(function () {
                whir.wx.loadMedias('image', function (data) {
                    $('input[name="ThumbMediaId"]').val(data['mediaId']);
                    $('input[name="ThumbMediaURL"]').val(data['url']);
                    $('input[name="ThumbMediaWebURL"]').val(data['webURL']);
                    showAvatar(data['url']);
                    fillCurrentPageData();
                });
            });

            function showAvatar(url) {
                $('#btn-choice-image').parent().find('div.data-avatar').remove();
                $('#btn-choice-image').parent().append('<div class="data-avatar"><img src="' + url + '" /></div>');
            }

            // 表单验证
            $("#formEdit").Validator(options, function (el) {

                fillCurrentPageData();
                var dataName = validateStores();
                if (dataName) {
                    $('div.data-articles').find('> div[data-name="' + dataName + '"]').click();
                    whir.toastr.error('<%="请完善表单信息".ToLang()%>');
                return false;
            }
            var articles = [];
            $('div.data-articles').find('> div.article').each(function (i) {
                var name = $(this).attr('data-name'), data = $.extend(dataStores[name], { ArticleIndex: i });
                data['Detail'] = repariPostImage(data['OriginalDetail']);
                articles.push(data);
            });
            whir.loading.show();
            whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                data: {
                    _action: "SaveArticle",
                    media: mediaId,
                    articles: JSON.stringify(articles)
                },
                success: function (response) {
                    whir.loading.remove();
                    if (response.Status == true) {
                        whir.toastr.success('<%="保存成功".ToLang()%>');                         
                        el.attr("form-success") == "refresh" ? whir.toastr.success(response.Message, true, false) :
                        whir.toastr.success(response.Message, true, false, "Articles.aspx");
                       
                    } else {
                        whir.toastr.error(response.Message);
                    }
                }
            });
            return false;
        });

        if (mediaId) {
            whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                data: {
                    _action: "GetArticle",
                    media: mediaId
                },
                success: function (response) {
                    whir.loading.remove();
                    if (response.Status == true) {
                        for (var i = 0, rows = $.parseJSON(response['Message']); i < rows.length; i++) {
                            rows[i]['UpdateDate'] = whir.ajax.fixJsonDate(rows[i]['UpdateDate']);
                            addPage(rows[i]);
                        }
                        $('div.data-articles').find('div.article:first').click();
                    } else {
                        whir.toastr.error('<%="未获取到图文内容".ToLang()%>');
                    }
                }
            });
        } else {
            $('#data-plus').click();
        }

        function addPage(data) {
            var views = [], name = Math.random().toString().replace('.', '') + $('div.data-articles').find('div.article').length;
            views.push('<div data-name="' + name + '" class="article" data-index="' + (data['ArticleIndex']) + '">');
            if (data['ThumbMediaURL']) {
                views.push('    <span style="background-image:url(' + data['ThumbMediaURL'] + ');"></span>');
            } else {
                views.push('    <span class="fontawesome-picture"></span>');
            }
            views.push('    <p>' + (data['Title'] || '<%="页面".ToLang()%>') + '</p>');
            views.push('</div>');
            $('div.data-articles').append(views.join('')).find('div.article:last').click(function () {
                fillCurrentPageData();
                $(this).addClass('selected').siblings().removeClass('selected');
                var name = $(this).attr('data-name'), article = dataStores[name];
                fillForm(article);
                currName = name;
            });
            dataStores[name] = data;
        }

        function fillForm(data) {
            $('#formEdit').find('input,textarea').val('');
            for (var name in data) {
                $('#formEdit').find('input[name="' + name + '"],textarea[name="' + name + '"]').val(data[name]);
            }
            if (kindEditor) {
                kindEditor.html(data['OriginalDetail'] || '');
            }
            if (data['ThumbMediaURL']) {
                showAvatar(data['ThumbMediaURL']);
            } else {
                $('div.data-avatar').remove();
            }
            $('div.btn-radio').find('> div.iradio_flat-red').removeClass('checked').filter('[data-val="' + (data['IsShowCover'] == false ? 0 : 1) + '"]').addClass('checked');
        }

        function fillCurrentPageData() {
            if ($('div.data-articles').find('> div.selected').length > 0) {
                var currNode = $('div.data-articles').find('> div.selected'), name = currNode.attr('data-name') || '', data = dataStores[name];
                $('#formEdit').find('input,textarea').each(function () {
                    data[$(this).attr('name')] = $(this).val();
                });
                data['IsShowCover'] = $('div.btn-radio').find('> div.checked').attr('data-val') == 1 ? true : false;
                var thumbImage = data['ThumbMediaURL'] || '';
                if (thumbImage) {
                    currNode.find('span').removeClass('fontawesome-picture').css({ backgroundImage: 'url(' + thumbImage + ')' });
                }
                currNode.find('p').text(data['Title'] || '<%="标题".ToLang()%>');
                dataStores[name] = data;
            }
        }

        function repariPostImage(html) {
            var reg = new RegExp('src="(.+?)\\?m=(.+?)"', 'g');
            html = html.replace(reg, function (match, value1, value2, index, input) {
                return 'src="' + decodeURIComponent(value2) + '"';
            });
            return html;
        }

        function validateStores() {
            var dataName = '', validateFields = ['Title'];
            for (var name in dataStores) {
                var data = dataStores[name];
                for (var i = 0; i < validateFields.length; i++) {
                    if ($.trim(data[validateFields[i]]) == '') {
                        dataName = name;
                    }
                }
            }
            return dataName;
        }
        });
    </script>
</asp:Content>

