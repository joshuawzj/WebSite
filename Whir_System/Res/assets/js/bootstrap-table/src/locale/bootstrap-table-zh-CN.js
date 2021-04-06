/**
 * Bootstrap Table Chinese translation
 * Author: Zhixin Wen<wenzhixin2010@gmail.com>
 */
(function ($) {
    'use strict';

    $.fn.bootstrapTable.locales['zh-CN'] = {
        formatLoadingMessage: function () {
            return '正在努力地加载数据中，请稍候……';
        },
        formatRecordsPerPage: function (pageNumber) {
            return '每页显示 ' + pageNumber + ' 条记录';
        },
        formatShowingRows: function (pageFrom, pageTo, totalRows) {
            return '显示第 ' + pageFrom + ' 到第 ' + pageTo + ' 条记录，总共 ' + totalRows + ' 条记录';
        },
        formatSearch: function () {
            return '搜索';
        },
        formatNoMatches: function () {
            return '没有找到匹配的记录';
        },
        formatPaginationSwitch: function () {
            return '隐藏/显示分页';
        },
        formatRefresh: function () {
            return '刷新';
        },
        formatToggle: function () {
            return '切换';
        },
        formatColumns: function () {
            return '列';
        },
        formatAllRows: function () {
            return '所有';
        },
        formatExport: function () {
            return '导出数据';
        },
        formatImport: function () {
            return '导入数据';
        },
        formatPrint: function () {
            return '打印';
        },
        formatWtl: function () {
            return '置标';
        },
        formatSearchTxt: function () {
            return '输入内容查找';
        },
        formatSelectTxt: function () {
            return '请选择';
        },
        formatDateTxt: function () {
            return '选择时间段';
        },
        formatRegionTxt: function () {
            return '选择地区';
        },
        formatStartNumTxt: function () {
            return '开始数值';
        },
        formatEndNumTxt: function () {
            return '结束数值';
        },
        formatNumTxt: function () {
            return '输入数值范围';
        },
        formatCancelTxt: function () {
            return '取消';
        },
        formatConfirmTxt: function () {
            return '确定';
        },
        formatStartDateTxt: function () {
            return '开始日期';
        },
        formatEndDateTxt: function () {
            return '结束日期';
        },
        formatStartEndDateTxt: function () {
            return '开始日期不能大于结束日期';
        },
        formatNullDateTxt: function () {
            return '开始日期、结束日期必须同时为空或者同时不为空';
        },
        formatNullNumTxt: function () {
            return '开始数值和结束数值必须同时为空或者同时不为空';
        },
        formatStartEndNumTxt: function () {
            return '开始数值不能大于结束数值';
        },
        formatClearFilters: function () {
            return '清除筛选值';
        }
    };

    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales['zh-CN']);

})(jQuery);
