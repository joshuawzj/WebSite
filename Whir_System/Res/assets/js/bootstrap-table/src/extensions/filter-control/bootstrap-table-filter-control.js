/**
 * @author: Dennis Hernández
 * @webSite: http://djhvscf.github.io/Blog
 * @version: v2.1.1
 */

(function ($) {

    'use strict';

    var global;

    var sprintf = $.fn.bootstrapTable.utils.sprintf,
        objectKeys = $.fn.bootstrapTable.utils.objectKeys;

    var showDatePicker = function (column, title, btntext, that) {
        if ($("#datepickerModal" + "_" + column.field).length == 0) {
            var vModal = sprintf("<div id=\"datepickerModal%s\"  class=\"modal fade\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"mySmallModalLabel\" aria-hidden=\"true\">", "_" + column.field);
            vModal += "<div class=\"modal-dialog modal-xs\">";
            vModal += " <div class=\"modal-content\">";
            vModal += "  <div class=\"modal-header\">";
            vModal += "   <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\" >&times;</button>";
            vModal += sprintf("   <h4 class=\"modal-title\">%s</h4>", title);
            vModal += "  </div>";
            vModal += "  <div class=\"modal-body modal-body-custom\">";
            vModal += sprintf("   <div class=\"container-fluid\" id=\"datepickerModalContent%s\" style=\"padding-right: 0px;padding-left: 0px;\" >", "_" + column.field);
            vModal += createFormAvd(column, btntext).join('');
            vModal += "   </div>";
            vModal += "  </div>";
            vModal += "  <div class=\"modal-footer\">";
            vModal += sprintf("<button type=\"button\" id=\"btnClose%s\" class=\"btn btn-default\" data-dismiss=\"modal\">" + global.options.formatCancelTxt() +"</button>", "_" + column.field);
            vModal += sprintf("<button type=\"button\"  id=\"btnSave%s\" class=\"btn btn-primary\">" + global.options.formatConfirmTxt() +"</button>", "_" + column.field); ;
            vModal += "  </div>";
            vModal += "  </div>";
            vModal += " </div>";
            vModal += "</div>";

            $("body").append($(vModal));

            //日期事件
         
            $("#divStartTimeFilter").datetimepicker(column.filterDatepickerOptions);
            $("#divEndTimeFilter").datetimepicker(column.filterDatepickerOptions);

            $("#btnClose" + "_" + column.field).click(function () {
                $("#datepickerModal" + "_" + column.field).modal('hide');
            });

            $("#btnSave" + "_" + column.field).click(function () {
                var start = $("[name=" + column.field + "Start]").val();
                var end = $("[name=" + column.field + "End]").val();
                if (start == ""&& end == "") {
                    // whir.toastr.warning("开始日期和结束日期不能为空。");
                    that.$header.find('.date-filter-control.bootstrap-table-filter-control-' + column.field)
                       .val("");
                    that.$header.find('.date-filter-control.bootstrap-table-filter-control-' + column.field).keyup();
                    $("#datepickerModal" + "_" + column.field).modal('hide');
                } else if ((start != "" && end != "") && CompareDate(start, end)) {
                    whir.toastr.warning(global.options.formatStartEndDateTxt());
                } else if (start != "" && end != "") {
                    that.$header.find('.date-filter-control.bootstrap-table-filter-control-' + column.field)
                        .val(start + "<&&<" + end);
                    that.$header.find('.date-filter-control.bootstrap-table-filter-control-' + column.field).keyup();
                    $("#datepickerModal" + "_" + column.field).modal('hide');
                }
                else {
                    whir.toastr.warning(global.options.formatNullDateTxt());
                }
            });

            $("#datepickerModal" + "_" + column.field).modal();
        } else {
            $("#datepickerModal" + "_" + column.field).modal();
        }
    };

    var showIntPicker = function (column, title, btntext, that) {
        if ($("#intpickerModal" + "_" + column.field).length == 0) {
            var vModal = sprintf("<div id=\"intpickerModal%s\"  class=\"modal fade\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"mySmallModalLabel\" aria-hidden=\"true\">", "_" + column.field);
            vModal += "<div class=\"modal-dialog modal-xs\">";
            vModal += " <div class=\"modal-content\">";
            vModal += "  <div class=\"modal-header\">";
            vModal += "   <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\" >&times;</button>";
            vModal += sprintf("   <h4 class=\"modal-title\">%s</h4>", title);
            vModal += "  </div>";
            vModal += "  <div class=\"modal-body modal-body-custom\">";
            vModal += sprintf("   <div class=\"container-fluid\" id=\"intpickerModalContent%s\" style=\"padding-right: 0px;padding-left: 0px;\" >", "_" + column.field);
            vModal += createFormInt(column, btntext).join('');
            vModal += "   </div>";
            vModal += "  </div>";
            vModal += "  <div class=\"modal-footer\">";
            vModal += sprintf("<button type=\"button\" id=\"btnClose%s\" class=\"btn btn-default\" data-dismiss=\"modal\">" + global.options.formatCancelTxt() + "</button>", "_" + column.field);
            vModal += sprintf("<button type=\"button\"  id=\"btnSave%s\" class=\"btn btn-primary\">" + global.options.formatConfirmTxt() + "</button>", "_" + column.field);;
            vModal += "  </div>";
            vModal += "  </div>";
            vModal += " </div>";
            vModal += "</div>";

            $("body").append($(vModal));

            
            $("#btnClose" + "_" + column.field).click(function () {
                $("#intpickerModal" + "_" + column.field).modal('hide');
            });

            $("#btnSave" + "_" + column.field).click(function () {
                var start = $("[name=" + column.field + "Start]").val();
                var end = $("[name=" + column.field + "End]").val();
                if (start == "" && end == "") {  
                    that.$header.find('.date-filter-control.bootstrap-table-filter-control-' + column.field)
                        .val("");
                    that.$header.find('.date-filter-control.bootstrap-table-filter-control-' + column.field).keyup();
                    $("#intpickerModal" + "_" + column.field).modal('hide');
                }
                else if ((start != "" && end == "") || (start == "" && end != "")) {
                    whir.toastr.warning(global.options.formatNullNumTxt());
                } else if (parseFloat(start)>parseFloat(end)) {
                    whir.toastr.warning(global.options.formatStartEndNumTxt());
                } else {
                    that.$header.find('.date-filter-control.bootstrap-table-filter-control-' + column.field)
                        .val(start + "<&&<" + end);
                    that.$header.find('.date-filter-control.bootstrap-table-filter-control-' + column.field).keyup();
                    $("#intpickerModal" + "_" + column.field).modal('hide');
                }
            });

            $("#intpickerModal" + "_" + column.field).modal();
        } else {
            $("#intpickerModal" + "_" + column.field).modal();
        }
    };

    //字符串转日期比较
    var CompareDate = function (d1, d2) {
        return ((new Date(d1.replace(/-/g, "\/"))) > (new Date(d2.replace(/-/g, "\/"))));
    }

    var createFormAvd = function (column) {
         
        var htmlForm = [];
        htmlForm.push('<div class="form-group">');
        htmlForm.push(sprintf('<label class="col-sm-4 control-label">%s(' + global.options.formatStartDateTxt() +')</label>', column.title));
        htmlForm.push('<div class=" input-group date form_datetime col-sm-6" id="divStartTimeFilter">');
        htmlForm.push(sprintf('<input type="text" readonly="readonly" class="form-control input-md" name="%s" placeholder="%s" id="%s">', column.field + "Start", global.options.formatStartDateTxt(), column.field));
        htmlForm.push('<span class=\"input-group-addon\"><span class=\"glyphicon glyphicon-remove\"></span></span><span class=\"input-group-addon\"><span class=\"glyphicon glyphicon-time\"></span></span>');
        htmlForm.push('</div>');
        htmlForm.push(sprintf('<label class="col-sm-4 control-label">%s(' + global.options.formatEndDateTxt() +')</label>', column.title));
        htmlForm.push('<div class=" input-group date form_datetime col-sm-6" id="divEndTimeFilter">');
        htmlForm.push(sprintf('<input type="text" readonly="readonly" class="form-control input-md" name="%s" placeholder="%s" id="%s">', column.field + "End", global.options.formatEndDateTxt(), column.field));
        htmlForm.push('<span class=\"input-group-addon\"><span class=\"glyphicon glyphicon-remove\"></span></span><span class=\"input-group-addon\"><span class=\"glyphicon glyphicon-time\"></span></span>');
        htmlForm.push('</div>');
        htmlForm.push('</div>');

        return htmlForm;
    };

    var createFormInt = function (column) {
        var htmlForm = [];
        htmlForm.push('<div class="form-group">');
        htmlForm.push(sprintf('<label class="col-sm-4 control-label">%s(' + global.options.formatStartNumTxt() +'	)</label>', column.title));
        htmlForm.push('<div class="col-sm-6">');
        htmlForm.push(sprintf('<input type="number"   class="form-control input-md" name="%s" placeholder="%s" id="%s">', column.field + "Start", global.options.formatStartNumTxt() , column.field));
        htmlForm.push('</div>');
        htmlForm.push(sprintf('<label class="col-sm-4 control-label">%s(' + global.options.formatEndNumTxt() +'	)</label>', column.title));
        htmlForm.push('<div class="col-sm-6">');
        htmlForm.push(sprintf('<input type="number"   class="form-control input-md" name="%s" placeholder="%s" id="%s">', column.field + "End", global.options.formatEndNumTxt(), column.field));
        htmlForm.push('</div>');
        htmlForm.push('</div>');

        return htmlForm;
    };

    //区域选择
    var showReginPicker = function (level, field) {
        var url = _sysPath + 'ModuleMark/Common/Area_Select.aspx?arealevel=' + level + '&callback=setAreaPickerValue&field=' + field;
        _dialog = whir.dialog.frame('' + global.options.formatRegionTxt() +'	', url, null, 400, 350);
    };
    var getOptionsFromSelectControl = function (selectControl) {
        return selectControl.get(selectControl.length - 1).options;
    };

    var hideUnusedSelectOptions = function (selectControl, uniqueValues) {
        var options = getOptionsFromSelectControl(selectControl);

        for (var i = 0; i < options.length; i++) {
            if (options[i].value !== "") {
                if (!uniqueValues.hasOwnProperty(options[i].value)) {
                    selectControl.find(sprintf("option[value='%s']", options[i].value)).hide();
                } else {
                    selectControl.find(sprintf("option[value='%s']", options[i].value)).show();
                }
            }
        }
    };

    var addOptionToSelectControl = function (selectControl, value, text) {
        value = $.trim(value);
        selectControl = $(selectControl.get(selectControl.length - 1));
        if (!existOptionInSelectControl(selectControl, value)) {
            selectControl.append($("<option></option>")
                .attr("value", value)
                .text($('<div />').html(text).text()));
        }
    };

    var sortSelectControl = function (selectControl) {
        var $opts = selectControl.find('option:gt(0)');
        $opts.sort(function (a, b) {
            a = $(a).text().toLowerCase();
            b = $(b).text().toLowerCase();
            if ($.isNumeric(a) && $.isNumeric(b)) {
                // Convert numerical values from string to float.
                a = parseFloat(a);
                b = parseFloat(b);
            }
            return a > b ? 1 : a < b ? -1 : 0;
        });

        selectControl.find('option:gt(0)').remove();
        selectControl.append($opts);
    };

    var existOptionInSelectControl = function (selectControl, value) {
        var options = getOptionsFromSelectControl(selectControl);
        if (options) {
            for (var i = 0; i < options.length; i++) {
                if (options[i].value === value.toString()) {
                    //The value is not valid to add
                    return true;
                }
            }
        }

        //If we get here, the value is valid to add
        return false;
    };

    var fixHeaderCSS = function (that) {
        that.$tableHeader.css('height', '77px');
    };

    var getCurrentHeader = function (that) {
        var header = that.$header;
        if (that.options.height) {
            header = that.$tableHeader;
        }

        return header;
    };

    var getCurrentSearchControls = function (that) {
        var searchControls = 'select, input';
        if (that.options.height) {
            searchControls = 'table select, table input';
        }

        return searchControls;
    };

    var getCursorPosition = function (el) {
        if ($.fn.bootstrapTable.utils.isIEBrowser()) {
            return -1;
            if ($(el).is('input')) {
                var pos = 0;
                if ('selectionStart' in el) {
                    pos = el.selectionStart;
                } else if ('selection' in document) {
                    el.focus();
                    var Sel = document.selection.createRange();
                    var SelLength = document.selection.createRange().text.length;
                    Sel.moveStart('character', -el.value.length);
                    pos = Sel.text.length - SelLength;
                }
                return pos;
            } else {
                return -1;
            }
        } else {
            return -1;
        }
    };

    var setCursorPosition = function (el, index) {
        return false;
        if ($.fn.bootstrapTable.utils.isIEBrowser()) {
            if (el.setSelectionRange !== undefined) {
                el.setSelectionRange(index, index);
            } else {
                $(el).val(el.value);
            }
        }
    };

    var copyValues = function (that) {
        var header = getCurrentHeader(that),
            searchControls = getCurrentSearchControls(that);

        that.options.valuesFilterControl = [];

        header.find(searchControls).each(function () {
            that.options.valuesFilterControl.push(
            {
                field: $(this).closest('[data-field]').data('field'),
                value: $(this).attr("regin") == "true" ? $(this).attr("areaid") + "," + $(this).val() : $(this).val(),
                position: getCursorPosition($(this).get(0))
            });
        });
    };

    var setValues = function (that) {
        var field = null,
            result = [],
            header = getCurrentHeader(that),
            searchControls = getCurrentSearchControls(that);

        if (that.options.valuesFilterControl.length > 0) {
            header.find(searchControls).each(function (index, ele) {
                field = $(this).closest('[data-field]').data('field');
                result = $.grep(that.options.valuesFilterControl, function (valueObj) {
                    return valueObj.field === field;
                });

                if (result.length > 0) {
                    if ($(this).attr("regin") == "true") {
                        var va = result[0].value.split(',');
                        if (va.length == 2) {
                            $(this).val(va[1]);
                            $(this).attr("areaid", va[0]);
                        }

                    } else {
                        $(this).val(result[0].value);
                    }
                    setCursorPosition($(this).get(0), result[0].position);
                }
            });
        }
    };

    var collectBootstrapCookies = function cookiesRegex() {
        var cookies = [],
            foundCookies = document.cookie.match(/(?:bs.table.)(\w*)/g);

        if (foundCookies) {
            $.each(foundCookies, function (i, cookie) {
                if (/./.test(cookie)) {
                    cookie = cookie.split(".").pop();
                }

                if ($.inArray(cookie, cookies) === -1) {
                    cookies.push(cookie);
                }
            });
            return cookies;
        }
    };

    var initFilterSelectControls = function (that) {
        var data = that.data,
            itemsPerPage = that.pageTo < that.options.data.length ? that.options.data.length : that.pageTo,

            isColumnSearchableViaSelect = function (column) {
                return column.filterControl && column.filterControl.toLowerCase() === 'select' && column.searchable;
            },

            isFilterDataNotGiven = function (column) {
                return column.filterData === undefined || column.filterData.toLowerCase() === 'column';
            },

            hasSelectControlElement = function (selectControl) {
                return selectControl && selectControl.length > 0;
            };

        var z = that.options.pagination ?
            (that.options.sidePagination === 'server' ? that.pageTo : that.options.totalRows) :
            that.pageTo;

        $.each(that.header.fields, function (j, field) {
            var column = that.columns[$.fn.bootstrapTable.utils.getFieldIndex(that.columns, field)],
                selectControl = $('.bootstrap-table-filter-control-' + escapeID(column.field));

            if (isColumnSearchableViaSelect(column) && isFilterDataNotGiven(column) && hasSelectControlElement(selectControl)) {
                if (selectControl.get(selectControl.length - 1).options.length === 0) {
                    //Added the default option
                    addOptionToSelectControl(selectControl, '', '');
                }

                var uniqueValues = {};
                for (var i = 0; i < z; i++) {
                    //Added a new value
                    var fieldValue = data[i][field],
                        formattedValue = $.fn.bootstrapTable.utils.calculateObjectValue(that.header, that.header.formatters[j], [fieldValue, data[i], i], fieldValue);

                    uniqueValues[formattedValue] = fieldValue;
                }

                for (var key in uniqueValues) {
                    addOptionToSelectControl(selectControl, uniqueValues[key], key);
                }

                sortSelectControl(selectControl);

                if (that.options.hideUnusedSelectOptions) {
                    hideUnusedSelectOptions(selectControl, uniqueValues);
                }
            }
        });
    };

    var escapeID = function (id) {
        return String(id).replace(/(:|\.|\[|\]|,)/g, "\\$1");
    };

    var createControls = function (that, header) {
        var addedFilterControl = false,
            isVisible,
            html,
            timeoutId = 0;

        $.each(that.columns, function (i, column) {
            isVisible = 'hidden';
            html = [];

            if (!column.visible) {
                return;
            }

            if (column.filterControl !== undefined) {
                column.filterControl = getFilterControl(column.filterControl);
            }

            if (!column.filterControl) {
                html.push('<div class="no-filter-control"></div>');
            } else {
                html.push('<div class="filter-control">');

                var nameControl = column.filterControl.toLowerCase();
                if (column.searchable && that.options.filterTemplate[nameControl]) {
                    addedFilterControl = true;
                    isVisible = 'visible';
                    html.push(that.options.filterTemplate[nameControl](that, column.field, isVisible, that.options.formatSearchTxt() ));
                }
            }

            $.each(header.children().children(), function (i, tr) {
                tr = $(tr);
                if (tr.data('field') === column.field) {
                    tr.find('.fht-cell').append(html.join(''));
                    return false;
                }
            });

            if (column.filterData !== undefined && column.filterData.toLowerCase() !== 'column') {
                var filterDataType = getFilterDataMethod(filterDataMethods, column.filterData.substring(0, column.filterData.indexOf(':')));
                var filterDataSource, selectControl;

                if (filterDataType !== null) {
                    filterDataSource = column.filterData.substring(column.filterData.indexOf(':') + 1, column.filterData.length);
                    selectControl = $('.bootstrap-table-filter-control-' + escapeID(column.field));

                    addOptionToSelectControl(selectControl, '', that.options.formatSelectTxt());
                    filterDataType(filterDataSource, selectControl);
                } else {
                    throw new SyntaxError('Error. You should use any of these allowed filter data methods: var, json, url.' + ' Use like this: var: {key: "value"}');
                }

                var variableValues, key;
                switch (filterDataType) {
                    case 'url':
                        $.ajax({
                            url: filterDataSource,
                            dataType: 'json',
                            success: function (data) {
                                for (var key in data) {
                                    addOptionToSelectControl(selectControl, key, data[key]);
                                }
                                //sortSelectControl(selectControl);
                            }
                        });
                        break;
                    case 'var':
                        variableValues = window[filterDataSource];
                        for (key in variableValues) {
                            addOptionToSelectControl(selectControl, key, variableValues[key]);
                        }
                        sortSelectControl(selectControl);
                        break;
                    case 'jso':
                        variableValues = JSON.parse(filterDataSource);
                        for (key in variableValues) {
                            addOptionToSelectControl(selectControl, key, variableValues[key]);
                        }
                        sortSelectControl(selectControl);
                        break;
                }
            }
        });

        if (addedFilterControl) {
            header.off('keyup', 'input').on('keyup', 'input', function (event) {
                var code = event.keyCode || event.which;
                if (code == 13) { //Enter keycode
                    that.onColumnSearch(event);
                }
                else {
                    clearTimeout(timeoutId);
                    timeoutId = setTimeout(function () {
                        that.onColumnSearch(event);
                    }, that.options.searchTimeOut);
                }
            });

            header.off('change', 'select').on('change', 'select', function (event) {
                clearTimeout(timeoutId);
                timeoutId = setTimeout(function () {
                    that.onColumnSearch(event);
                }, that.options.searchTimeOut);
            });

            header.off('mouseup', 'input').on('mouseup', 'input', function (event) {
                var $input = $(this),
                oldValue = $input.val();

                if (oldValue === "") {
                    return;
                }

                setTimeout(function () {
                    var newValue = $input.val();

                    if (newValue === "") {
                        clearTimeout(timeoutId);
                        timeoutId = setTimeout(function () {
                            that.onColumnSearch(event);
                        }, that.options.searchTimeOut);
                    }
                }, 1);
            });

            if (header.find('.date-filter-control').length > 0) {
                $.each(that.columns, function (i, column) {
                    if (column.filterControl !== undefined && column.filterControl.toLowerCase() === 'datepicker') {
                        if (column.format) {
                            column.filterDatepickerOptions.format = column.format;
                        }
                        if (column.filterDatepickerOptions.format == "yyyy-mm-dd") {
                            column.filterDatepickerOptions.minView = 2;
                        } else {
                            column.filterDatepickerOptions.minView = 0;
                        }
                        //column.filterDatepickerOptions.minView = "month";
                        header.find('.date-filter-control.bootstrap-table-filter-control-' + column.field)
                            .click(function () {
                                showDatePicker(column, that.options.formatDateTxt(), that.options.formatConfirmTxt(), that);
                            });
                    }
                    if (column.filterControl !== undefined && column.filterControl.toLowerCase() === 'regin') {
                        header.find('.date-filter-control.bootstrap-table-filter-control-' + column.field)
                            .click(function () {
                                showReginPicker(column.leve, column.field);
                            });
                    }
                    if (column.filterControl !== undefined && column.filterControl.toLowerCase() === 'int') {
                        header.find('.date-filter-control.bootstrap-table-filter-control-' + column.field)
                            .click(function () {
                                showIntPicker(column, that.options.formatNumTxt(), that.options.formatConfirmTxt() , that);
                            });
                    }
                });
            }
        } else {
            header.find('.filterControl').hide();
        }
    };

    var getDirectionOfSelectOptions = function (alignment) {
        alignment = alignment === undefined ? 'left' : alignment.toLowerCase();

        switch (alignment) {
            case 'left':
                return 'ltr';
            case 'right':
                return 'rtl';
            case 'auto':
                return 'auto';
            default:
                return 'ltr';
        }
    };

    var getFilterControl = function (fieldType) {
        if (!isNaN(fieldType)) {
            switch (fieldType) {
                case '7':
                    return 'datepicker';
                case '13':
                    return 'regin';
                case '4':
                case '9':
                    return 'select';
                case '0':
                case '1':
                case '2':
                case '3':
                case '5':
                case '8':
                case '10':
                case '11':
                case '12':
                case '14':
                case '15':
                case '16':
                    return 'input';
                case '6':
                    return 'int';
                default:
                    return 'input';
            }
        } else {
            return fieldType;
        }

    };

    var filterDataMethods =
        {
            'var': function (filterDataSource, selectControl) {
                var variableValues = window[filterDataSource];
                for (var key in variableValues) {
                    addOptionToSelectControl(selectControl, key, variableValues[key]);
                }
                sortSelectControl(selectControl);
            },
            'url': function (filterDataSource, selectControl) {
                $.ajax({
                    url: filterDataSource,
                    dataType: 'json',
                    success: function (data) {
                        for (var key in data) {
                            addOptionToSelectControl(selectControl, data[key].Value, data[key].Text);
                        }
                        //sortSelectControl(selectControl);
                    }
                });
            },
            'json': function (filterDataSource, selectControl) {
                var variableValues = JSON.parse(filterDataSource);
                for (var key in variableValues) {
                    addOptionToSelectControl(selectControl, key, variableValues[key]);
                }
                sortSelectControl(selectControl);
            }
        };

    var getFilterDataMethod = function (objFilterDataMethod, searchTerm) {
        var keys = Object.keys(objFilterDataMethod);
        for (var i = 0; i < keys.length; i++) {
            if (keys[i] === searchTerm) {
                return objFilterDataMethod[searchTerm];
            }
        }
        return null;
    };

    $.extend($.fn.bootstrapTable.defaults, {
        filterControl: true,
        onColumnSearch: function (field, text) {
            return false;
        },
        filterShowClear: true,
        alignmentSelectControlOptions: undefined,
        filterTemplate: {
            input: function (that, field, isVisible, placeholder) {
               
                return sprintf('<input type="text" class="form-control bootstrap-table-filter-control-%s" style="width: 100%; visibility: %s" placeholder="%s">', field, isVisible, placeholder);
            },
            select: function (that, field, isVisible) {
                return sprintf('<select class="form-control bootstrap-table-filter-control-%s" style="width: 100%; visibility: %s" dir="%s"></select>',
                    field, isVisible, getDirectionOfSelectOptions(that.options.alignmentSelectControlOptions));
            },
            datepicker: function (that, field, isVisible) {
                return sprintf('<input type="text" readonly="readonly" class="form-control date-filter-control bootstrap-table-filter-control-%s" placeholder="' + $.fn.bootstrapTable.defaults.formatDateTxt() +'" style="width: 100%;cursor: default; visibility: %s">', field, isVisible);
            },
            regin: function (that, field, isVisible) {
                return sprintf('<input type="text" readonly="readonly" regin="true" class="form-control date-filter-control bootstrap-table-filter-control-%s" placeholder="' + $.fn.bootstrapTable.defaults.formatRegionTxt() +'" style="width: 100%;cursor: default; visibility: %s">', field, isVisible);
            },
            int: function (that, field, isVisible) {
                return sprintf('<input type="text" readonly="readonly" class="form-control date-filter-control bootstrap-table-filter-control-%s" placeholder="' + $.fn.bootstrapTable.defaults.formatNumTxt() +'" style="width: 100%;cursor: default; visibility: %s">', field, isVisible);
            }
        },
        //internal variables
        valuesFilterControl: []
    });
    var _lang = ($("script[lang]").attr("lang") || "1");
    _lang = _lang == "2" ? "zh-TW" : _lang == "3"?"en":"zh-CN";

    $.extend($.fn.bootstrapTable.columnDefaults,
    {
        filterControl: undefined,
        filterData: undefined,
        format: '',
        leve: '',
        filterDatepickerOptions: {
            language: _lang,
            format: 'yyyy-mm-dd',
            todayBtn: 1,
            startView: 2,
            minView: 0,
            maxView: 2,
            autoclose: 1,
            todayHighlight: 1,
            forceParse: true
        },
        filterStrictSearch: false,
        filterStartsWithSearch: false,
        filterControlPlaceholder: ""
    });

    $.extend($.fn.bootstrapTable.Constructor.EVENTS, {
        'column-search.bs.table': 'onColumnSearch'
    });

    $.extend($.fn.bootstrapTable.defaults.icons, {
        clear: 'entypo-erase'
    });

   
    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales);

    var BootstrapTable = $.fn.bootstrapTable.Constructor,
        _init = BootstrapTable.prototype.init,
        _initToolbar = BootstrapTable.prototype.initToolbar,
        _initHeader = BootstrapTable.prototype.initHeader,
        _initBody = BootstrapTable.prototype.initBody,
        _initSearch = BootstrapTable.prototype.initSearch;

    BootstrapTable.prototype.init = function () {
        global = this;
        //Make sure that the filterControl option is set
        if (this.options.filterControl) {
            var that = this;

            // Compatibility: IE < 9 and old browsers
            if (!Object.keys) {
                objectKeys();
            }

            //Make sure that the internal variables are set correctly
            this.options.valuesFilterControl = [];

            this.$el.on('reset-view.bs.table', function () {
                //Create controls on $tableHeader if the height is set
                if (!that.options.height) {
                    return;
                }

                //Avoid recreate the controls
                if (that.$tableHeader.find('select').length > 0 || that.$tableHeader.find('input').length > 0) {
                    return;
                }

                createControls(that, that.$tableHeader);
            }).on('post-header.bs.table', function () {
                setValues(that);
            }).on('post-body.bs.table', function () {
                if (that.options.height) {
                    fixHeaderCSS(that);
                }
            }).on('column-switch.bs.table', function () {
                setValues(that);
            });
        }
        _init.apply(this, Array.prototype.slice.apply(arguments));
    };

    BootstrapTable.prototype.initToolbar = function () {
        this.showToolbar = this.options.filterControl && this.options.filterShowClear;

        _initToolbar.apply(this, Array.prototype.slice.apply(arguments));

        if (this.options.filterControl && this.options.filterShowClear) {
            var $btnGroup = this.$toolbar.find('>.btn-group'),
                $btnClear = $btnGroup.find('.filter-show-clear');

            if (!$btnClear.length) {
                $btnClear = $([
                    '<button class="btn btn-default filter-show-clear" ',
                    sprintf('type="button" title="%s">', this.options.formatClearFilters()),
                    sprintf('<i class="%s %s"></i> ', this.options.iconsPrefix, this.options.icons.clear),
                    '</button>'
                ].join('')).appendTo($btnGroup);

                $btnClear.off('click').on('click', $.proxy(this.clearFilterControl, this));
            }
        }
    };

    BootstrapTable.prototype.initHeader = function () {
        _initHeader.apply(this, Array.prototype.slice.apply(arguments));

        if (!this.options.filterControl) {
            return;
        }
        createControls(this, this.$header);
    };

    BootstrapTable.prototype.initBody = function () {
        _initBody.apply(this, Array.prototype.slice.apply(arguments));

        initFilterSelectControls(this);
    };

    BootstrapTable.prototype.initSearch = function () {
        _initSearch.apply(this, Array.prototype.slice.apply(arguments));

        if (this.options.sidePagination === 'server') {
            return;
        }

        var that = this;
        var fp = $.isEmptyObject(this.filterColumnsPartial) ? null : this.filterColumnsPartial;

        //Check partial column filter
        this.data = fp ? $.grep(this.data, function (item, i) {
            for (var key in fp) {
                var thisColumn = that.columns[$.fn.bootstrapTable.utils.getFieldIndex(that.columns, key)];
                var fval = fp[key].toLowerCase();
                var value = item[key];

                // Fix #142: search use formated data
                if (thisColumn && thisColumn.searchFormatter) {
                    value = $.fn.bootstrapTable.utils.calculateObjectValue(that.header,
                    that.header.formatters[$.inArray(key, that.header.fields)],
                    [value, item, i], value);
                }

                if (thisColumn.filterStrictSearch) {
                    if (!($.inArray(key, that.header.fields) !== -1 &&
                        (typeof value === 'string' || typeof value === 'number') &&
                        value.toString().toLowerCase() === fval.toString().toLowerCase())) {
                        return false;
                    }
                } else if (thisColumn.filterStartsWithSearch) {
                    if (!($.inArray(key, that.header.fields) !== -1 &&
                      (typeof value === 'string' || typeof value === 'number') &&
                      (value + '').toLowerCase().indexOf(fval) === 0)) {
                        return false;
                    }
                } else {
                    if (!($.inArray(key, that.header.fields) !== -1 &&
                        (typeof value === 'string' || typeof value === 'number') &&
                        (value + '').toLowerCase().indexOf(fval) !== -1)) {
                        return false;
                    }
                }
            }
            return true;
        }) : this.data;
    };

    BootstrapTable.prototype.initColumnSearch = function (filterColumnsDefaults) {
        copyValues(this);

        if (filterColumnsDefaults) {
            this.filterColumnsPartial = filterColumnsDefaults;
            this.updatePagination();

            for (var filter in filterColumnsDefaults) {
                this.trigger('column-search', filter, filterColumnsDefaults[filter]);
            }
        }
    };

    BootstrapTable.prototype.onColumnSearch = function (event) {
        if ($.inArray(event.keyCode, [37, 38, 39, 40]) > -1) {
            return;
        }

        copyValues(this);
        var text = $.trim($(event.currentTarget).val());
        if ($(event.currentTarget).attr("regin") == "true") {
            text = $(event.currentTarget).attr("areaid");
        }
        var $field = $(event.currentTarget).closest('[data-field]').data('field');

        if ($.isEmptyObject(this.filterColumnsPartial)) {
            this.filterColumnsPartial = {};
        }
        if (text) {
            this.filterColumnsPartial[$field] = text;
        } else {
            delete this.filterColumnsPartial[$field];
        }

        // if the btntext is the same as the previously selected column value,
        // bootstrapTable will not try searching again (even though the selected column
        // may be different from the previous search).  As a work around
        // we're manually appending some text to bootrap's btntext field
        // to guarantee that it will perform a search again when we call this.onSearch(event)
        this.btntext += "randomText";

        this.options.pageNumber = 1;
        this.onSearch(event);
        this.trigger('column-search', $field, text);
    };

    BootstrapTable.prototype.clearFilterControl = function () {
        if (this.options.filterControl && this.options.filterShowClear) {
            var that = this,
                cookies = collectBootstrapCookies(),
                header = getCurrentHeader(that),
                table = header.closest('table'),
                controls = header.find(getCurrentSearchControls(that)),
                search = that.$toolbar.find('.search input'),
                timeoutId = 0;

            $.each(that.options.valuesFilterControl, function (i, item) {
                item.value = '';
            });

            setValues(that);

            // Clear each type of filter if it exists.
            // Requires the body to reload each time a type of filter is found because we never know
            // which ones are going to be present.
            if (controls.length > 0) {
                this.filterColumnsPartial = {};
                $(controls[0]).trigger(controls[0].tagName === 'INPUT' ? 'keyup' : 'change');
            } else {
                return;
            }

            if (search.length > 0) {
                that.resetSearch();
            }

            // use the default sort order if it exists. do nothing if it does not
            if (that.options.sortName !== table.data('sortName') || that.options.sortOrder !== table.data('sortOrder')) {
                var sorter = header.find(sprintf('[data-field="%s"]', $(controls[0]).closest('table').data('sortName')));
                if (sorter.length > 0) {
                    that.onSort(table.data('sortName'), table.data('sortName'));
                    $(sorter).find('.sortable').trigger('click');
                }
            }

            // clear cookies once the filters are clean
            clearTimeout(timeoutId);
            timeoutId = setTimeout(function () {
                if (cookies && cookies.length > 0) {
                    $.each(cookies, function (i, item) {
                        if (that.deleteCookie !== undefined) {
                            that.deleteCookie(item);
                        }
                    });
                }
            }, that.options.searchTimeOut);
        }
    };
})(jQuery);
