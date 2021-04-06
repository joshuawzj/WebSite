//--------------------------------------------------------------------------------
// 文件描述：整站Ajax请求封装
// 文件作者：张清山
// 创建日期：2013-12-10 15:09:32
// 修改记录：
//--------------------------------------------------------------------------------

//调用代码示例：
//$.whir.ajax('UserHandler.AddMember', {
//    params: {
//        username: userName
//    },
//    success: function (response) {
//        var result = eval(response)[0];
//        if (Boolean(result.status)) {
//            alert(result.body);
//        }
//    },
//    err: function (msg) {
//        alert("异常：" + msg);
//    }
//});

//网站根目录
var rootpath = location.protocol + "//" + location.hostname + (location.port == 80 ? "" : ":" + location.port) + "/";
$.whir = $.whir || { version: '1.0' };
$.extend($.whir, {
    htmlEncode: function (text) {
        var span = $('<span>');
        span.html(text);
        return span.html();
    },
    ajax: function (cmdName, params) {
        if (typeof (cmdName) != 'string' || cmdName == 'undefind')
            return;
        var defaultParms = {
            url: rootpath + "ajax.aspx",
            data: '',
            type: 'POST',
            params: {}
        };
        var getXml = function (setting) {
            var xml = '';
            for (var arg in setting.params) {
                xml += ('<' + arg + '>' + $.whir.htmlEncode(setting.params[arg]) + '</' + arg + '>');
            }
            return xml;
        };
        var settings = $.extend(defaultParms, params);
        var data = '<root><_type>' + cmdName + '</_type>';
        data += getXml(settings);
        data += '</root>';
        settings.data = data;
        $.ajax(settings);
    } 
});

