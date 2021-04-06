var whir = window.whir || {};
whir.cookie =
{
    get: function(name) {
        if (document.cookie.length > 0) {
            var start = document.cookie.indexOf(name + "=");
            if (start != -1) {
                start = start + name.length + 1;
                var end = document.cookie.indexOf(";", start);
                if (end == -1) end = document.cookie.length;
                return decodeURIComponent(document.cookie.substring(start, end));
            }
        }
        return "";
    },
    set: function(name, value, expiredays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + expiredays);
        document.cookie =
            name +
            "=" +
            escape(value) +
            ((expiredays == null) ? ";path=/" : ";expires=" + exdate.toGMTString() + ";path=/");
    }
};
 