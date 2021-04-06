window.whir = window.whir || {};
whir.bdStatistics = (function () {
    'use strict';

    if (typeof Object.assign != 'function') {
        // Must be writable: true, enumerable: false, configurable: true
        Object.defineProperty(Object, "assign", {
            value: function assign(target, varArgs) { // .length of function is 2
                'use strict';
                if (target == null) { // TypeError if undefined or null
                    throw new TypeError('Cannot convert undefined or null to object');
                }

                var to = Object(target);

                for (var index = 1; index < arguments.length; index++) {
                    var nextSource = arguments[index];

                    if (nextSource != null) { // Skip over if undefined or null
                        for (var nextKey in nextSource) {
                            // Avoid bugs when hasOwnProperty is shadowed
                            if (Object.prototype.hasOwnProperty.call(nextSource, nextKey)) {
                                to[nextKey] = nextSource[nextKey];
                            }
                        }
                    }
                }
                return to;
            },
            writable: true,
            configurable: true
        });
    }
     

    function ajax(action, param, callback) {
        $.ajax({
            type: "POST",
            url: window.BDStatisticsUrl,
            data: Object.assign(param || {},
                {
                    _action: action
                }),
            success: function (response) {
                if (typeof callback === 'function')
                    callback(response && JSON.parse(response));
            }
        });
    }


    var api = {
        helper: {
            zeroFill: function (number) {
                if (number < 10)
                    return '0' + number;

                return number.toString();
            },
            getTrendTimeYText: function (yTextItem) {


                if (yTextItem.length === 1) {
                    if (typeof yTextItem[0] === 'number')
                        return api.helper.zeroFill(yTextItem[0]) + ':' + api.helper.zeroFill(yTextItem[0]);
                    else if (typeof yTextItem[0] === 'string')
                        return yTextItem[0];
                }

                return '';
            },
            getMetricsText: function (m) {

                if (m instanceof Array) {
                    var texts = [];
                    for (var i = 0; i < m.length; i++) {
                        if (metrics[m[i]]) {
                            texts.push(metrics[m[i]]);
                        }
                    }
                    return texts;
                } else if (typeof m === 'string') {
                    return metrics[m];
                }

                return null;
            },
            /**
             * 数值千分号
             * @param {} num 
             * @returns {} 
             */
            getCommafy: function (num) {
                num = num + "";
                var re = /(-?\d+)(\d{3})/;
                while (re.test(num)) {
                    num = num.replace(re, "$1,$2");
                }
                return num;
            },
            /**
             * 秒数转时间
             * @param {} value 
             * @returns {} 
             */
            secondToDate: function (value) {
                var minute = 60;
                var hour = 60 * minute;


                var totalHour = parseInt(value / hour);
                var totalMinute = parseInt((value - (totalHour * hour)) / minute);
                var totalSecond = parseInt((value - (totalMinute * minute)));

                return api.helper.zeroFill(totalHour) + ':' + api.helper.zeroFill(totalMinute) + ':' + api.helper.zeroFill(totalSecond);
            },
            getMetricsValue: function (metrics, value) {
                if (isNaN(value)) return value;

                switch (metrics) {
                    case 'bounce_ratio':
                    case 'trans_ratio':
                    case 'new_visitor_ratio':
                    case 'roi':
                    case 'exit_ratio':
                    case 'pv_ratio':
                        value += '%';
                        break;
                    case 'avg_visit_time':
                    case 'average_stay_time':
                        value = api.helper.secondToDate(value);
                        break;
                    default:
                        value = api.helper.getCommafy(value);
                        break;
                }

                return value;
            }
        },
        GetTimeTrendRpt: function (start, end) {
            ajax('GetTimeTrendRpt', {
                start: start,
                end: end
            }, function (res) {
                if (res.Status && res.Message) {
                    $(JSON.parse(res.Message)).each(function (idx, item) {
                        if (item.header.desc === "system failure") {
                            whir.toastr.error(item.header.failures[0].code + "&nbsp;" + item.header.failures[0].message);
                            return true;
                        }

                        console.log(item);
                    });
                }
            });
        },
        GetTrendTimeA: function (start, end, callback) {
            ajax('GetTrendTimeA', {
                start: start,
                end: end
            }, function (res) {
                if (res.Status && res.Message) {
                    var result = JSON.parse(res.Message);
                    if (typeof callback === 'function')
                        callback(result.body.data[0].result);
                }
            });
        },
        GetVisitToppageA: function (start, end, startIndex, callback) {
            ajax('GetVisitToppageA', {
                start: start,
                end: end,
                start_index: startIndex || 0
            }, function (res) {
                if (res.Status && res.Message) {
                    var result = JSON.parse(res.Message);
                    if (typeof callback === 'function')
                        callback(result.body.data[0].result);
                }
            });
        },
        GetVisitDistrictA: function (start, end, isWorld, callback) {
            ajax('GetVisitDistrictA', {
                start: start,
                end: end,
                isWorld: isWorld ? 1 : 0
            }, function (res) {
                if (res.Status && res.Message) {
                    var result = JSON.parse(res.Message);
                    if (typeof callback === 'function')
                        callback(result.body.data[0].result);
                }
            });
        }
    }


    return api;
})();