var whir = window.whir || {};
whir.ajax = {
    get: function (url, data, callback) {
        $.get(url, data, function (result) {
            callback(result);
        });
    },
    getXML: function (url, data, callback) {
        $.get(url, data, function (result) {
            callback(result);
        });
    },
    htmlEncode: function (text) {
        var value = text;
        try {
            value = value.replace(/&emsp;/g, "&nbsp;");
            value = value.replace(/&/, "&amp;");
            value = value.replace(/</g, "&lt;");
            value = value.replace(/>/g, "&gt;");
            value = value.replace(/'/g, "&apos;");
            value = value.replace(/"/g, "&quot;");
        } catch (e) {
            var span = $('<span>');
            span.html(value);
            value = span.html();
            value = value.replace(/&/, "&amp;");
            value = value.replace(/</g, "&lt;");
            value = value.replace(/>/g, "&gt;");
            value = value.replace(/'/g, "&apos;");
            value = value.replace(/"/g, "&quot;");
        }
        return value;
    },
    post: function (url, params) {
        var defaultParms = {
            url: url,
            data: {
            },
            dataType: 'json',
            type: 'POST',
            success: function (response) {
            }
        };
        var settings = $.extend(defaultParms, params);
        settings.data['__RequestVerificationToken'] = $('[name="__RequestVerificationToken"]').val() || '';
        whir.loading.show();
        $.ajax(settings);
    },
    fixJsonDate: function (jsonDate, format) {// 发现实体转换的日期会多了8个小时，使用该方法会修正这8个小时
        if (!jsonDate) {
            return "";
        }
        jsonDate = jsonDate.replace("星期一", "").replace("星期二", "").replace("星期三", "").replace("星期四", "").replace("星期五", "").replace("星期六", "").replace("星期日", "");
        var date = null;

        if (jsonDate.toLowerCase().lastIndexOf("/date(")>=0) {
            if (jsonDate) {
                var strDate = jsonDate.toLowerCase().replace("/date(", "").replace(")/", "");
                try {
                    date = new Date(parseInt(strDate) - (8 * 3600 * 1000));
                } catch (ex) {
                }
            }
        }
        else {
            if (jsonDate.lastIndexOf("-") >= 0)
                jsonDate = jsonDate.replace(/-/g, "/");  //为了兼容苹果浏览器只支持/
            date = new Date(jsonDate);
        }
        if (!date) {
            return "";
        }
        date = { year: date.getFullYear(), month: date.getMonth() + 1, day: date.getDate(), hour: date.getHours(), minutes: date.getMinutes(),seconds:date.getSeconds() };
        switch (format) {
            case '-':
                return date.year + '-' + fixTime(date.month) + '-' + fixTime(date.day);
            case 'zh':
                return date.year + '年' + fixTime(date.month) + '月' + fixTime(date.day) + '日';
            default:
                return date.year + '-' + fixTime(date.month) + '-' + fixTime(date.day) + ' ' + fixTime(date.hour) + ':' + fixTime(date.minutes) + ':' + fixTime(date.seconds);
        }
        function fixTime(value) {
            return value.toString().length > 1 ? value : "0" + value;
        }
    },
    subString: function (data, subLenth) {
        if (data != "" && data != null) {
            data = data.replace(/<[^>]+>/g, "");
            if (data.length > subLenth) {
                return data.substring(0, subLenth) + ".....";
            }
        }
        return data;
    }
};
