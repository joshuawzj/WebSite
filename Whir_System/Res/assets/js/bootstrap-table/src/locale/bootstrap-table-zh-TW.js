/**
 * Bootstrap Table Chinese translation
 * Author: Zhixin Wen<wenzhixin2010@gmail.com>
 */
(function ($) {
    'use strict';

    $.fn.bootstrapTable.locales['zh-TW'] = {
        formatLoadingMessage: function () {
            return '正在努力地載入資料，請稍候……';
        },
        formatRecordsPerPage: function (pageNumber) {
            return '每頁顯示 ' + pageNumber + ' 項記錄';
        },
        formatShowingRows: function (pageFrom, pageTo, totalRows) {
            return '顯示第 ' + pageFrom + ' 到第 ' + pageTo + ' 項記錄，總共 ' + totalRows + ' 項記錄';
        },
        formatSearch: function () {
            return '搜尋';
        },
        formatNoMatches: function () {
            return '沒有找到符合的結果';
        },
        formatPaginationSwitch: function () {
            return '隱藏/顯示分頁';
        },
        formatRefresh: function () {
            return '重新整理';
        },
        formatToggle: function () {
            return '切換';
        },
        formatColumns: function () {
            return '選擇列';
        },
        formatAllRows: function () {
            return '所有';
        },
        formatExport: function () {
            return '導出數據';
        },
        formatImport: function () {
            return '導入數據';
        },
        formatPrint: function () {
            return '打印';
        },
        formatWtl: function () {
            return '置標';
        },
        formatSearchTxt: function () {
            return '輸入內容查找';
        },
        formatSelectTxt: function () {
            return '請選擇';
        },
        formatDateTxt: function () {
            return '選擇時間段';
        },
        formatRegionTxt: function () {
            return '選擇地區';
        },
        formatStartNumTxt: function () {
            return '開始數值';
        },
        formatEndNumTxt: function () {
            return '結束數值';
        },
        formatNumTxt: function () {
            return '輸入數值範圍';
        },
        formatCancelTxt: function () {
            return '取消';
        },
        formatConfirmTxt: function () {
            return '確定';
        },
        formatStartDateTxt: function () {
            return '開始日期';
        },
        formatEndDateTxt: function () {
            return '結束日期';
        },
        formatStartEndDateTxt: function () {
            return '開始日期不能大於結束日期';
        },
        formatNullDateTxt: function () {
            return '開始日期、結束日期必須同時為空或者同時不為空';
        },
        formatNullNumTxt: function () {
            return '開始數值和結束數值必須同時為空或者同時不為空';
        },
        formatStartEndNumTxt: function () {
            return '開始數值不能大於結束數值';
        },
        formatClearFilters: function () {
            return '清除篩選值';
        }
    };

    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales['zh-TW']);

})(jQuery);
