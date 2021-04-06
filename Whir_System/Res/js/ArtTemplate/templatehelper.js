template.helper('SplitIndex', function (data, splitStr, index) {

    if (data != "") {
        var str = data.split(splitStr);
        if (index < str.length) {
            return str[index];
        }
    }
    return "";
});

template.helper('ToDate', function (date, format) {
    if (date != "") {

        var dates;
        var dateStr;
        if (date != "" && date.indexOf("/Date(") > -1) {
            dateStr = date.replace("/Date(", "").replace(")/", "");
            try {
                date = new Date(parseInt(dateStr) - (8 * 3600 * 1000));
            } catch (ex) { }
        } else {
             dates = date.split(" ");
             dateStr = dates[0] + " " + dates[2];
             date = new Date(dateStr);
        }
        date = { year: date.getFullYear(), month: date.getMonth() + 1, day: date.getDate(), hour: date.getHours(), minutes: date.getMinutes() };
        switch (format) {
            case '-':
                return date.year + '-' + fixTime(date.month) + '-' + fixTime(date.day);
            case '.':
                return date.year + '.' + fixTime(date.month) + '.' + fixTime(date.day);
            case 'zh':
                return date.year + '年' + fixTime(date.month) + '月' + fixTime(date.day) + '日';
            case 'yyMM':
                return date.year + '-' + fixTime(date.month);
            case 'MM.dd':
                return fixTime(date.month) + '.' + fixTime(date.day);
            case 'dd':
                return fixTime(date.day);
            default:
                return date.year + '-' + fixTime(date.month) + '-' + fixTime(date.day) + ' ' + fixTime(date.hour) + ':' + fixTime(date.minutes);
        }
        function fixTime(value) {
            return value.toString().length > 1 ? value : "0" + value;
        }
    }
    return "";
});

template.helper('SubString', function (data, subLenth) {
    if (data != "") {
        data = data.replace(/<[^>]+>/g, "");
        return data.substring(0, subLenth);
    }
    return "";
});

template.helper('GetCityName', function (data) {
    return GetRegionNameByCode(data);
});

template.helper('ToInt', function (data) {
    return parseInt(data);
});

template.helper('GetTimePoor', function (data) {
    var date1;
    var dateStr;
    if (data != "" && data.indexOf("/Date(") > -1) {
        dateStr = data.replace("/Date(", "").replace(")/", "");
        try {
            date1 = new Date(parseInt(dateStr) - (8 * 3600 * 1000));
        } catch (ex) { }
    } else {
        data = date.split(" ");
        dateStr = data[0] + " " + data[2];
        date1 = new Date(dateStr);
    }

    var date2 = new Date();

    var date3 = date2.getTime() - date1.getTime(); //时间差的毫秒数
    var days = Math.floor(date3 / (24 * 3600 * 1000));

    //计算出小时数
    var leave1 = date3 % (24 * 3600 * 1000);    //计算天数后剩余的毫秒数
    var hours = Math.floor(leave1 / (3600 * 1000));
    //计算相差分钟数
    var leave2 = leave1 % (3600 * 1000);       //计算小时数后剩余的毫秒数
    var minutes = Math.floor(leave2 / (60 * 1000));
    //计算相差秒数
    var leave3 = leave2 % (60 * 1000);      //计算分钟数后剩余的毫秒数
    var seconds = Math.round(leave3 / 1000);

    if (seconds <= 60) {
        return seconds + "秒前";
    } else if (minutes <= 60) {
        return seconds + "分钟前";
    } else if (hours <= 24) {
        return hours + "小时前";
    } else {
        return days + "天前";
    }

});