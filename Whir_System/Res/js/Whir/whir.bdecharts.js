window.whir = window.whir || {};
whir.bdecharts = (function () {
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


    return function (el) {
        'use strict';
        var myChart = echarts.init(document.getElementById(el));
        var api = {
            lineStack: function (opt) {

                var tempOpt = Object.assign({
                    title: '折线图堆叠',
                    xAxisData: [],
                    yAxisData: [],
                    yAxisTextData: []
                }, opt || {});


                api.setOption({
                    title: {
                        text: tempOpt.title
                    },
                    tooltip: {
                        trigger: 'axis'
                    },
                    legend: {
                        data: tempOpt.yAxisTextData
                    },
                    grid: {
                        left: '3%',
                        right: '4%',
                        bottom: '3%',
                        containLabel: true
                    },
                    toolbox: {
                        feature: {
                            saveAsImage: {}
                        }
                    },
                    xAxis: {
                        type: 'category',
                        boundaryGap: false,
                        data: tempOpt.xAxisData
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: tempOpt.yAxisData
                });

                return api;
            },
            showLoading: function() {
                myChart.showLoading();
            },
            hideLoading: function() {
                myChart.hideLoading();
            },
            setOption: function (option) {
                myChart.setOption(option);
                return api;
            }
        };
        return api;
    }
})();