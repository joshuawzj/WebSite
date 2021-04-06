var whir = window.whir || {};
//不需要给checkbox美化皮肤，默认是需要美化的
var noSkinForCheckbox = false;
var noSkinForRadio = false;
var _lang ="lang"+ ($("script[lang]").attr("lang")||"1");
var languageOpt = {
    lang1: {
        success: "成功消息",
        info: "提示消息",
        warning: "警告消息",
        error: "错误消息",
        loadingMsg: "正在加载中",
        sysTip: "系统提示",
        confirm: "确定",
        cancel: "取消",
        close: "关闭",
        title: "标题",
        noLogin: "您还未登录，或登录超时，请先登录",
        logout: "您的账号在其他地方登录，已被强制下线"
    },
   lang2: {
       success: "成功消息",
       info: "提示消息",
       warning: "警告消息",
       error: "錯誤消息",
       loadingMsg: "正在加載中",
       sysTip: "系統提示",
       confirm: "確定",
       cancel: "取消",
       close: "關閉",
       title: "標題",
       noLogin: "您還未登錄，或登入超時，請先登錄。",
       logout: "您的帳號在其他地方登入，已被強制下線"
    },
    lang3: {
        success: "Success message",
        info: "Tip message",
        warning: "Warning message",
        error: "Error message",
        loadingMsg: "Loading",
        sysTip: "System tips",
        confirm: "Confirm",
        cancel: "Cancel",
        close: "Close",
        title: "Title",
        noLogin: "You are not logged in, or the login timed out, please login first",
        logout: "Your account has been logged in elsewhere and has been forced offline"
    }
}


/**
 * 消息提示
 */
whir.toastr = {
    init: function (ops) {
        toastr.options = {
            closeButton: true,
            debug: false,
            progressBar: true,
            positionClass: "toast-top-right",
            onclick: null,
            showDuration: "300",
            hideDuration: "1000",
            onShown: null,
            timeOut: "10000",
            extendedTimeOut: "1000",
            showEasing: "swing",
            hideEasing: "linear",
            showMethod: "slideDown",
            hideMethod: "fadeOut"
        };
        toastr.options = $.extend(toastr.options, ops);
    },
    success: function (message, isrefresh, isparent, url, ops) {
        this.init(ops);
        if (isrefresh && !isparent && !toastr.options.onShown) {
            if (url) {
                toastr.options.onShown = function () { setTimeout("window.location.href='" + url + "'", 300); };
            } else {
                toastr.options.onShown = function () { setTimeout("window.location.href=window.location.href", 300); };
            }
        }
        if (isrefresh && isparent && !toastr.options.onShown) {
            toastr.options.onShown = function () { setTimeout("window.parent.location.href=window.parent.location.href", 300); };
        }
        toastr["success"](message, languageOpt[_lang]['success']);
    },
    info: function (message, ops) {
        this.init(ops);
        toastr["info"](message, languageOpt[_lang]['info']);
    },
    warning: function (message, ops) {
        this.init(ops);
        toastr["warning"](message, languageOpt[_lang]['warning']);
    },
    error: function (message, ops) {
        this.init(ops);
        var url = _sysPath + "Login.aspx?PageUrl=" + escape(window.location.href);
        if (message == -1) {
            message = languageOpt[_lang]['noLogin'];
            toastr.options.onShown = function () { setTimeout("window.location.href='" + url + "'", 1000); };
        } else if (message == -2) {
            message = languageOpt[_lang]['logout'];
            toastr.options.onShown = function () { setTimeout("window.location.href='" + url + "'", 1000); };
        }
        toastr["error"](message, languageOpt[_lang]['error']);
    }
};

/**
 * 加载中...
 */
whir.loading = {
    show: function () {
        if ($("#whir_loading_mask").length <= 0) {
            var $mask = $("<div/>")
                .attr({ "id": "whir_loading_mask" })
                .addClass("loading-modal")
                .appendTo($("body"));
            var $loading = $("<div/>")
                .attr({ "id": "whir_loading" })
                .addClass("loading")
                .appendTo($("body"));
            var $loadIcon = $("<i/>")
                .addClass("fontawesome-refresh")
                .appendTo($loading);
            var $loadText = $("<span/>")
                .html(languageOpt[_lang]['loadingMsg']+"<br/>...")
                .appendTo($loading);
            $("#whir_loading_mask").click(function () {
                $("#whir_loading_mask").animate({ "opacity": "0" }, 400, function () { $(this).remove(); });
                $("#whir_loading").animate({ "opacity": "0" }, 400, function () { $(this).remove(); });
            });
        }
    },
    remove: function () {

        $("#whir_loading_mask").animate({ "opacity": "0" }, 400, function () { $(this).remove(); });
        $("#whir_loading").animate({ "opacity": "0" }, 400, function () { $(this).remove(); });
    }
};

/**
 * 弹窗
 */
whir.dialog = {
    /**
    * 返回整个弹窗的jQuery对象
    * 可调用返回对象的setContent动态设置自定义内容
    * 可调用返回对象的setIframe方法动态设置iframe信息
    * 可调用返回对象的close方法关闭弹窗
    **/
    show: function (options) {
        var opts = $.extend({
            title: languageOpt[_lang]['title'],
            content: '', // 可配置content或者iframe以确定是使用自定义内容还是一个页面来填充弹窗
            ok: function (dialog) { },
            cancel: function (dialog) { dialog.close(); },
            okText: languageOpt[_lang]['confirm'],
            cancelText: languageOpt[_lang]['cancel'],
            showOk: true,
            showCancel: true,
            iframe: {
                url: '',
                width: 400,
                height: 300,
                scroll: false
            },
            zIndex: 1003
        }, options);
        var dialog = $('<section class="fade" name="_dialog" style="position:fixed;width:100%;height:100%;top:0;left:0;background-color:rgba(0, 0, 0, 0.5);z-index:' + opts.zIndex + ';">' +
            '<div id="movePanel" class="panel" style="margin:0;position:absolute;">' +
            '   <header id="moveHeader" class="panel-heading" style="cursor:move" >' + opts.title +
            '       <span class="tools pull-right">' +
            '           <a href="javascript:;" class="icon-remove"></a>' +
            '       </span>' +
            '   </header>' +
            '   <div class="panel-body" style="padding:0;"></div>' +
            '   <div class="panel-bottom" style="text-align:right;padding:8px;border-top:1px solid #eff2f7;">' +
            '       <button class="btn btn-default" type="button" style="padding:3px 12px;margin-right:8px;">' + opts.cancelText + '</button>' +
            '       <button class="btn btn-primary" type="button" style="padding:3px 12px;">' + opts.okText + '</button>' +
            '   </div>' +
            '</div></section>').appendTo('body');
        dialog.setContent = function (content) {
            dialog.find('div.panel-body').html(content);
            dialog.setCenter();
        }
        dialog.setCenter = function () {
            dialog.find('div.panel').css({
                top: ($(window).height() - dialog.find('div.panel').height()) / 2 + 'px',
                left: (($(window).width() - dialog.find('div.panel').width()) / 2) + 'px'
            });
        }
        dialog.setIframe = function (iframe) {
            opts.iframe = $.extend(opts.iframe, iframe);
            opts.iframe.width = opts.iframe.width > $(window).width() - 50 ? $(window).width() - 50 : opts.iframe.width;
            opts.iframe.height = opts.iframe.height > $(window).height() - 90 ? $(window).height() - 90 : opts.iframe.height;
            dialog.setContent('<iframe allowtransparency="true" id="dialogiframe" frameborder="0" style="width:' +
                opts.iframe.width + 'px;height:' + opts.iframe.height + 'px;" scrolling="' + (opts.iframe.scroll ? 'yes' : 'no') + '"></iframe>');
            dialog.find('iframe').attr('src', opts.iframe.url);
        }
        if (!opts.showOk) {
            dialog.find('button.btn-primary').remove();
        }
        if (!opts.showCancel) {
            dialog.find('button.btn-default').remove();
        }
        if (!opts.showOk && !opts.showCancel) {
            dialog.find('div.panel-bottom').remove();
        }
        if (opts.content) {
            dialog.setContent(opts.content);
        } else {
            //dialog.find('div.panel-bottom').remove();
            //dialog.setContent('<iframe allowtransparency="true" frameborder="0" style="width:' + opts.iframe.width + 'px;height:' + opts.iframe.height + 'px;" scrolling="' + (opts.iframe.scroll ? 'yes' : 'no') + '"></iframe>');
            //dialog.find('iframe').attr('src', opts.iframe.url);
            dialog.setIframe();
        }

        dialog.close = function () {
            dialog.find('a.icon-remove').click();
        }
        dialog.find('a.icon-remove').click(function () {
            dialog.removeClass('in');
            window.setTimeout(function () {
                dialog.remove();
            }, 150);
        });

        dialog.find('button.btn-default').click(function () {
            opts.cancel.call(this, dialog);
        });
        dialog.find('button.btn-primary').click(function () {
            if (opts.ok) {
                opts.ok.call(this, dialog);
            }
        });
        window.setTimeout(function () {
            dialog.addClass('in');
        }, 150);

        $('#moveHeader').mousedown(
            function (event) {
                var isMove = true;
                document.body.className += '  disable-select';
                var absX = event.clientX - $('#movePanel').offset().left;
                $(document).mousemove(function (event) {
                    if (isMove) {
                        var obj = $('#movePanel');
                        var left = (event.clientX - absX) < 0 ? 0 : (event.clientX - absX);
                        left = left > $(window).width() - $('#movePanel').width() ? $(window).width() - $('#movePanel').width() : left;
                        var top = event.clientY < 0 ? 0 : event.clientY;
                        top = top > $(window).height() - $('#movePanel').height() ? $(window).height() - $('#movePanel').height() : top;
                        obj.css({ 'left': left, 'top': top });
                    }
                }).mouseup(function () {
                    isMove = false;
                    document.body.className = document.body.className.replace('  disable-select', '');
                });
            });



        return dialog;
    },
    confirm: function (message, okCallback) {
        var dialog = this.show({
            title: languageOpt[_lang]['confirm'],
            content: "<div class='text-center' style='padding:20px;'>" + message + "</div>",
            ok: okCallback
        });
        return dialog;
    },
    alert: function (message) {
        var dialog = this.show({
            title: languageOpt[_lang]['sysTip'],
            showOk: false,
            content: "<div class='text-center' style='padding:20px;'>" + message + "</div>",
            cancelText: languageOpt[_lang]['confirm']
        });
        return dialog;
    },
    frame: function (title, url, okCallback, width, height, showOk) {
        var dialog = this.show({
            title: title,
            iframe: {
                url: url,
                width: width || 300,
                height: height || 500,
                scroll: true
            },
            showOk: showOk || typeof (showOk) == "undefined" ? true : false,
            showCancel: true,
            ok: okCallback
        });
        return dialog;
    },
    remove: function () {
        $("[name=_dialog]").find('button.btn-default').click();
    }
}


/**
 * 换肤
 */
whir.skin = {
    load: function (imgName) {
        var imgPath = "Res/Images/Bg/" + imgName;
        whir.ajax.post(_sysPath + "Handler/Security/User.aspx",
            {
                data: {
                    _action: "ModifySkin",
                    SystemSkin: imgPath
                },
                success: function (response) {
                    if (response.Status == true) {
                        whir.loading.remove();
                        $("body")
                            .css({
                                "background-image": "url('" + _sysPath + imgPath + "')",
                                "background-repeat": "repeat",
                                "background-size": "cover",
                                "background-position": "center"
                            });

                    } else {
                        whir.toastr.error(response.Message);
                    }
                    whir.loading.remove();
                },
                error: function () {
                    whir.loading.remove();
                }
            });

    },
    radio: function (tag) {
        if (tag == false) { //取消美化radio
            noSkinForRadio = true;
            return;
        }
        if (noSkinForRadio) //取消美化radio
            return;
        //单选按钮美化
        $("input[type='radio']")
            .iCheck({
                checkboxClass: 'icheckbox_flat-red',
                radioClass: 'iradio_flat-red'
            });

    },

    checkbox: function (tag) {
        if (tag == false) { //取消美化checkbox
            noSkinForCheckbox = true;
            return;
        }
        if (noSkinForCheckbox) //取消美化checkbox
            return;

        //多选按钮美化
        $("input[type='checkbox']")
            .iCheck({
                checkboxClass: 'icheckbox_flat-red',
                radioClass: 'iradio_flat-red'
            });

    }
}

function imgSize() {
    var imgObj = new Image();
    imgObj.src = $('#pViewPic').attr("src");
    var imgWidth = imgObj.width;
    var imgHeight = imgObj.height;
    if (imgWidth > 800) {
        imgHeight = 800 / imgWidth * imgHeight;
        imgWidth = 800;
    }
    if (imgHeight > ($(window).height() - 90)) {
        imgWidth = ($(window).height() - 90) / imgHeight * imgWidth;
        imgHeight = ($(window).height() - 90);

    }
    $("#movePanel").css({
        top: ($(window).height() - imgHeight - 90) / 2 + 'px',
        left: ($(window).width() - imgWidth) / 2 + 'px'
    });

}
