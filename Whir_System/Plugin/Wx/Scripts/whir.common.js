
(function () {
    var whir = window.whir || {};
    whir.wx = {
        _basePath: '/',
        _loadTags: function (tags, buttonStyle, showAll, callback) {
            var views = [], className = buttonStyle == 'checkbox' ? 'icheckbox_flat-red' : 'iradio_flat-red';
            views.push('<ul class="wx-dialog-tags" style="width:400px;overflow:hidden;">');
            if (showAll) {
                views.push('<li style="float:left;width:33%;position:relative;height:20px;margin:8px 0;">');
                views.push('    <div data-val="0" class="' + className + '" style="position: relative;"><input style="position: absolute; opacity: 0;" type="checkbox"><ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins></div>');
                views.push('    <label style="position:absolute;left:24px;top:0px;">所有用户</label>');
                views.push('</li>');
            }
            for (var i = 0; i < tags.length; i++) {
                views.push('<li style="float:left;width:33%;position:relative;height:20px;margin:8px 0;">');
                views.push('    <div data-val="' + tags[i]['id'] + '" class="' + className + '" style="position: relative;"><input style="position: absolute; opacity: 0;" type="checkbox"><ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins></div>');
                views.push('    <label style="position:absolute;left:24px;top:0px;">' + tags[i]['name'] + '</label>');
                views.push('</li>');
            }
            views.push('</ul>');
            whir.dialog.confirm(views.join(''), function () {
                var data = null;
                if (buttonStyle == 'checkbox') {
                    data = [];
                    $('ul.wx-dialog-tags').find('div.checked').each(function () {
                        data.push({
                            id: $(this).attr('data-val'),
                            name: $(this).find('label').text()
                        });
                    });

                } else {
                    $('ul.wx-dialog-tags').find('div.checked').each(function () {
                        data = {
                            id: $(this).attr('data-val'),
                            name: $(this).find('label').text()
                        };
                    });
                }
                if (data == null || data.length == 0) {
                    whir.toastr.error('请至少选择一个标签');
                    return false;
                }
                whir.dialog.remove();
                if (typeof callback == 'function') {
                    callback(data);
                }
            });
            $('ul.wx-dialog-tags').find('div.icheckbox_flat-red').click(function () {
                if ($(this).hasClass('checked')) {
                    $(this).removeClass('checked');
                } else {
                    $(this).addClass('checked');
                }
            });
            $('ul.wx-dialog-tags').find('div.iradio_flat-red').click(function () {
                $(this).addClass('checked').parent().siblings().find('div.iradio_flat-red').removeClass('checked');
            });
            //$('#moveHeader').text('选择接收信息用户');
        },
        setBasePath: function (basePath) {
            this._basePath = basePath;
        },
        loadMedias: function (mediaType, callback) {
            whir.loading.show();

            var views = [], currPage = -1, pageSize = 10, pages = 0;
            views.push('<div class="data-images">');
            views.push('    <ul>');

            views.push('    </ul>');
            views.push('    <div class="btn-group">');
            views.push('        <button data-action="prev" type="button" class="btn btn-info">上一页</button>');
            views.push('        <button data-action="next" type="button" class="btn btn-success">下一页</button>');
            views.push('    </div>');
            views.push('</div>');
            whir.dialog.confirm(views.join(''), function () {
                if ($('div.data-images').find('li.selected').length == 0) {
                    whir.toastr.error('请选择图片！');
                    return false;
                }
                var selectNode = $('div.data-images').find('li.selected').first(), mediaId = selectNode.attr('data-media'), url = selectNode.attr('data-src'), name = selectNode.find('p').text(), webURL = selectNode.attr('data-websrc'), set = [];
                if (mediaType == 'news') {
                    var chirdren = decodeURIComponent(selectNode.attr('data-chirdren') || '').split('_xuan_');
                    for (var i = 0; i < chirdren.length; i++) {
                        var article = chirdren[i].split('_wen_');
                        if (article.length == 2) {
                            set.push({
                                title: article[0],
                                image: article[1]
                            });
                        }
                    }
                    set.unshift({ title: name, image: url });
                }
                callback({
                    mediaId: mediaId,
                    name: name,
                    url: url,
                    webURL: webURL,
                    set: set
                });
                whir.dialog.remove();
            });
            $('div.data-images').find('button').click(function () {
                var action = $(this).attr('data-action');
                if (action == 'prev') {
                    if (currPage > 0) {
                        currPage = currPage - 1;
                    } else {
                        currPage = 0;
                    }
                }
                else {
                    if (currPage >= pages) {
                        currPage = pages;
                    } else {
                        currPage = currPage + 1;
                    }
                }
                whir.ajax.post(whir.wx._basePath + 'Plugin/Wx/Ajax/Ajax.aspx?_action=GetMedias&media=' + mediaType + '&offset=' + (currPage * pageSize) + '&limit=' + pageSize, {
                    data: {},
                    success: function (response) {
                        whir.loading.remove();
                        for (var i = 0, rows = response['rows'], views = []; i < rows.length; i++) {
                            if (mediaType == 'news') {
                                for (var x = 0, children = rows[i]['Children'], stores = []; x < children.length; x++) {
                                    stores.push(children[x]['Title'] + '_wen_' + children[x]['ThumbMediaURL']);
                                }
                                views.push('<li data-media="' + rows[i]['MediaId'] + '" data-src="' + rows[i]['ThumbMediaURL'] + '" data-chirdren="' + encodeURIComponent(stores.join('_xuan_')) + '">');
                            } else {
                                views.push('<li data-media="' + rows[i]['MediaId'] + '" data-src="' + (rows[i]['LocalURL'] || rows[i]['WebURL']) + '" data-websrc="' + rows[i]['WebURL'] + '">');
                            }

                            if (mediaType == 'image') {
                                views.push(' <span style="background-image:url(' + (rows[i]['LocalURL'] || rows[i]['WebURL']) + ')"></span>');
                            }
                            if (mediaType == 'news') {
                                views.push(' <span style="background-image:url(' + rows[i]['ThumbMediaURL'] + ')"></span>');
                            }
                            if (mediaType == 'voice') {
                                views.push(' <span class="entypo-bell"></span>');
                            }
                            if (mediaType == 'video') {
                                views.push(' <span class="entypo-video"></span>');
                            }
                            views.push(' <p title="' + rows[i]['Title'] + '">' + rows[i]['Title'] + '</p>');
                            views.push('</li>');
                        }
                        $('div.data-images').find('ul').html(views.join('')).find('li').click(function () {
                            $(this).addClass('selected').siblings().removeClass('selected');
                        });
                        pages = Math.ceil(response['total'] / pageSize);
                    }
                });
                return false;
            }).filter('[data-action="next"]').click();
        },
        loadTags: function (options) {
            var context = this, opts = $.extend({
                showAll: false,
                buttonStyle: 'checkbox',
                callback: null
            }, options);
            if (this.tags && this.tags.length > 0) {
                this._loadTags(this.tags, opts.buttonStyle, opts.showAll, opts.callback);
            } else {
                whir.loading.show();
                whir.ajax.post(this._basePath + "Plugin/Wx/Ajax/Ajax.aspx", {
                    data: { _action: 'GetTags' },
                    success: function (response) {
                        whir.loading.remove();
                        context._loadTags(response['rows'], opts.buttonStyle, opts.showAll, opts.callback);
                        context.tags = response['rows'];
                    }
                });
            }
        }
    };
    window.whir = whir;
})(window);