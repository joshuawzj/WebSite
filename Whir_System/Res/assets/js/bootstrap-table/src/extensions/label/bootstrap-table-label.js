(function ($) {
    'use strict';

    var sprintf = $.fn.bootstrapTable.utils.sprintf;

    $.extend($.fn.bootstrapTable.defaults, {
    	showLabel: false
    });
    $.extend($.fn.bootstrapTable.defaults.icons, {
    	label: 'glyphicon glyphicon-tags'
    });

    var BootstrapTable = $.fn.bootstrapTable.Constructor,
        _initToolbar = BootstrapTable.prototype.initToolbar;

    BootstrapTable.prototype.initToolbar = function () {
        _initToolbar.apply(this, Array.prototype.slice.apply(arguments));

        if (this.options.showLabel) {
            var that = this,
                $btnGroup = this.$toolbar.find('>.btn-group'),
                label = $btnGroup.find('button.bs-label');

            if (!label.length) {
            	label = $([
                    '<button class="bs-label btn btn-default' + sprintf(' btn-%s"', this.options.iconSize) + '" name="label" title="' + this.options.formatWtl() +'" type="button">',
                    sprintf('<i class="%s %s"></i> ', this.options.iconsPrefix, this.options.icons.label),
                    '</button>'].join('')).appendTo($btnGroup);

            	label.click(function () {
            		whir.label.dialog(getQueryString("columnid"), getQueryString("subjectid"));
                });
            }
        }
    };
    var getQueryString = function (name) {
    	var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    	var r = window.location.search.substr(1).match(reg);
    	if (r != null) return unescape(r[2]); return null;
    };
})(jQuery);
