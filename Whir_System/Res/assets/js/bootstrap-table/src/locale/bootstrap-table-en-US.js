/**
 * Bootstrap Table English translation
 * Author: Zhixin Wen<wenzhixin2010@gmail.com>
 */
(function ($) {
    'use strict';

    $.fn.bootstrapTable.locales['en-US'] = {
        formatLoadingMessage: function () {
            return 'Loading, please wait...';
        },
        formatRecordsPerPage: function (pageNumber) {
            return pageNumber + ' rows per page';
        },
        formatShowingRows: function (pageFrom, pageTo, totalRows) {
            return 'Showing ' + pageFrom + ' to ' + pageTo + ' of ' + totalRows + ' rows';
        },
        formatSearch: function () {
            return 'Search';
        },
        formatNoMatches: function () {
            return 'No matching records found';
        },
        formatPaginationSwitch: function () {
            return 'Hide/Show pagination';
        },
        formatRefresh: function () {
            return 'Refresh';
        },
        formatToggle: function () {
            return 'Toggle';
        },
        formatColumns: function () {
            return 'Columns';
        },
        formatAllRows: function () {
            return 'All';
        },
        formatExport: function () {
            return 'Export data';
        },
        formatImport: function () {
            return 'Import data';
        },
        formatPrint: function () {
            return 'Print';
        },
        formatWtl: function () {
            return 'WTL';
        },
        formatSearchTxt: function () {
            return 'Input search';
        },
        formatSelectTxt: function () {
            return 'Please select';
        },
        formatDateTxt: function () {
            return 'Select time period';
        },
        formatRegionTxt: function () {
            return 'Select region';
        },
        formatStartNumTxt: function () {
            return 'Start value';
        },
        formatEndNumTxt: function () {
            return 'End value';
        },
        formatNumTxt: function () {
            return 'Input range of values';
        },
        formatCancelTxt: function () {
            return 'Cancel';
        },
        formatConfirmTxt: function () {
            return 'Confirm';
        },
        formatStartDateTxt: function () {
            return 'Start date';
        },
        formatEndDateTxt: function () {
            return 'End date';
        },
        formatStartEndDateTxt: function () {
            return 'Start date cannot be greater than the end date';
        },
        formatNullDateTxt: function () {
            return 'Start date, end date must be empty at the same time or not empty at the same time';
        },
        formatNullNumTxt: function () {
            return 'Start value and end value must be empty at the same time or not empty at the same time';
        },
        formatStartEndNumTxt: function () {
            return 'Start value cannot be greater than end value';
        },
        formatClearFilters: function () {
            return 'Clear filters';
        }
    };

    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales['en-US']);

})(jQuery);
