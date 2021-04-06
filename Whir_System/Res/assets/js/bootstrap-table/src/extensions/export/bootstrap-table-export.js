/**
 * @author zhixin wen <wenzhixin2010@gmail.com>
 * extensions: https://github.com/kayalshri/tableExport.jquery.plugin
 */

(function ($) {
    'use strict';
    var sprintf = $.fn.bootstrapTable.utils.sprintf;

    var TYPE_NAME = {
        import: '&nbsp;导入',
        export: '&nbsp;导出'
    };

    $.extend($.fn.bootstrapTable.defaults, {
        showExport: false,
        showImport: false,
        exportDataType: 'basic', // basic, all, selected
        exportTypes: ['import', 'export'],
        exportOptions: {}
    });

    $.extend($.fn.bootstrapTable.defaults.icons, {
        export: 'glyphicon-export'
    });

    
    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales);

    var BootstrapTable = $.fn.bootstrapTable.Constructor,
        _initToolbar = BootstrapTable.prototype.initToolbar;

    BootstrapTable.prototype.initToolbar = function () {
        this.showToolbar = this.options.showExport;

        _initToolbar.apply(this, Array.prototype.slice.apply(arguments));
        if (this.options.showImport)
        {
            var that = this,
                $btnGroup = this.$toolbar.find('>.btn-group'),
                $import = $btnGroup.find('div.export');

            if (!$import.length) {
                $import = $([
                        '<button class="btn' +
                            sprintf(' btn-%s', this.options.buttonsClass) +
                            sprintf(' btn-%s', this.options.iconSize) +
                    '" id="import" title="' + this.options.formatImport() +'" type="button"><i class="glyphicon glyphicon glyphicon-log-in"></i>',
                       '</button>'].join('')).appendTo($btnGroup);

                $import.click(function () {
                    openImport();
                });
            }
        }

        if (this.options.showExport) {
            var that = this,
                $btnGroup = this.$toolbar.find('>.btn-group'),
                $export = $btnGroup.find('div.export');

            if (!$export.length) {
                $export = $([
                        '<button class="btn' +
                            sprintf(' btn-%s', this.options.buttonsClass) +
                            sprintf(' btn-%s', this.options.iconSize) +
                    '" id="export" title="' + this.options.formatExport() +'" type="button"><i class="glyphicon glyphicon glyphicon-log-out"></i>' ,
                       '</button>'].join('')).appendTo($btnGroup);

                $export.click(function () {
                    openSelectColumn();
                });
            }
        }
    };
})(jQuery);
