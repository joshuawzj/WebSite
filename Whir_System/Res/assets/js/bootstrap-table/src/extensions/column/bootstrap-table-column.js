(function ($) {
    'use strict';

    var sprintf = $.fn.bootstrapTable.utils.sprintf;

    $.extend($.fn.bootstrapTable.defaults, {
        showSelectColumn: true,
        SelectColumnEvent: undefined
    });
    $.extend($.fn.bootstrapTable.defaults.icons, {
        column: 'glyphicon glyphicon-eye-open'
    });

    var BootstrapTable = $.fn.bootstrapTable.Constructor,
        _initToolbar = BootstrapTable.prototype.initToolbar;

    BootstrapTable.prototype.initToolbar = function () {
        this.showToolbar = this.options.showSelectColumn;

        _initToolbar.apply(this, Array.prototype.slice.apply(arguments));

        if (this.options.showSelectColumn) {
            var that = this,
                $btnGroup = this.$toolbar.find('>.btn-group'),
                $column = $btnGroup.find('button.bs-column');

            if (!$column.length) {
                $column = $([
                    '<button class="bs-column btn btn-default' + sprintf(' btn-%s"', this.options.iconSize) + '" name="column" title="' + this.options.formatColumns() +'" type="button">',
                    sprintf('<i class="%s %s"></i> ', this.options.iconsPrefix, this.options.icons.column),
                    '</button>'].join('')).appendTo($btnGroup);

                $column.click(function () {
                    that.options.SelectColumnEvent();
                });
            }
        }
    };
})(jQuery);
