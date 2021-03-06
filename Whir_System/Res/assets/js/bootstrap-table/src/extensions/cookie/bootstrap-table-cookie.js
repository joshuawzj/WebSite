/**
 * @author: Dennis Hernández
 * @webSite: http://djhvscf.github.io/Blog
 * @version: v1.2.2
 *
 * @update zhixin wen <wenzhixin2010@gmail.com>
 */

(function ($) {
    'use strict';

    var cookieIds = {
        sortOrder: 'bs.table.sortOrder',
        sortName: 'bs.table.sortName',
        pageNumber: 'bs.table.pageNumber',
        pageList: 'bs.table.pageList',
        columns: 'bs.table.columns',
        searchText: 'bs.table.searchText',
        filterControl: 'bs.table.filterControl',
        columnid: 'bs.table.columnid',
        subjectid: 'bs.table.subjectid'
    };

    var util = {
        queryStrings: function (url) {
            url = (url || window.location.search).toLowerCase();
            var arr = url.split('?'), params = arr.length > 1 ? arr[1] : null;
            if (params) {
                for (var i = 0, _form = {}, list = params.split('&'), l = list.length; i < l; i++) {
                    var p = list[i].split('='), n = p.length > 0 ? p[0] : 'i', v = p.length > 1 ? p[1] : '';
                    _form[n] = v;
                }
                return _form;
            }
            return {};
        },
        queryString: function (name, url) {
            var params = this.queryStrings((url || window.location.href));
            return params[name];
        }
    };

    //判断是否加载筛选
    var isloadfilter = function (that) {
        return that.options.cookie && util.queryString("filter") == "1" &&
            (JSON.parse(getCookie(that, that.options.cookieIdTable, cookieIds.filterControl))
            || getCookie(that, that.options.cookieIdTable, cookieIds.pageNumber)
            || getCookie(that, that.options.cookieIdTable, cookieIds.sortName)
            || getCookie(that, that.options.cookieIdTable, cookieIds.sortOrder)
            ) && (util.queryString("columnid") == getCookie(that, that.options.cookieIdTable, cookieIds.columnid)
            && util.queryString("subjectid") == getCookie(that, that.options.cookieIdTable, cookieIds.subjectid)
            )
    };

    //移除Cookie
    var clearCookie = function (that) {
        that.deleteCookie("columnid");
        that.deleteCookie("subjectid");
        that.deleteCookie("filterControl");
        that.deleteCookie("pageList");
        that.deleteCookie("pageNumber");
        that.deleteCookie("sortName");
        that.deleteCookie("sortOrder");
    };

    //根据字段名称字段类型
    var getColumnFieldType = function (that, field) {
        var result = "";
        $.each(that.columns, function (i, column) {
            if (column && column.filterControl && column.field == field) {
                result = column.filterControl;
                return false
            }
        });
        return result;
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

    var cookieEnabled = function () {
        return !!(navigator.cookieEnabled);
    };

    var inArrayCookiesEnabled = function (cookieName, cookiesEnabled) {
        var index = -1;

        for (var i = 0; i < cookiesEnabled.length; i++) {
            if (cookieName.toLowerCase() === cookiesEnabled[i].toLowerCase()) {
                index = i;
                break;
            }
        }

        return index;
    };

    var setCookie = function (that, cookieName, cookieValue) {
        if ((!that.options.cookie) || (!cookieEnabled()) || (that.options.cookieIdTable === '')) {
            return;
        }

        if (inArrayCookiesEnabled(cookieName, that.options.cookiesEnabled) === -1) {
            return;
        }

        cookieName = that.options.cookieIdTable + '.' + cookieName;

        switch(that.options.cookieStorage) {
            case 'cookieStorage':
                document.cookie = [
                        cookieName, '=', cookieValue,
                        '; expires=' + that.options.cookieExpire,
                        that.options.cookiePath ? '; path=' + that.options.cookiePath : '',
                        that.options.cookieDomain ? '; domain=' + that.options.cookieDomain : '',
                        that.options.cookieSecure ? '; secure' : ''
                    ].join('');
            break;
            case 'localStorage':
                localStorage.setItem(cookieName, cookieValue);
            break;
            case 'sessionStorage':
                sessionStorage.setItem(cookieName, cookieValue);
                break;
            default:
                return false;
        }

        return true;
    };

    var getCookie = function (that, tableName, cookieName) {
        if (!cookieName) {
            return null;
        }

        if (inArrayCookiesEnabled(cookieName, that.options.cookiesEnabled) === -1) {
            return null;
        }

        cookieName = tableName + '.' + cookieName;

        switch(that.options.cookieStorage) {
            case 'cookieStorage':
                return decodeURIComponent(document.cookie.replace(new RegExp('(?:(?:^|.*;)\\s*' + encodeURIComponent(cookieName).replace(/[\-\.\+\*]/g, '\\$&') + '\\s*\\=\\s*([^;]*).*$)|^.*$'), '$1')) || null;
            case 'localStorage':
                return localStorage.getItem(cookieName);
            case 'sessionStorage':
                return sessionStorage.getItem(cookieName);
            default:
                return null;
        }
    };

    var deleteCookie = function (that, tableName, cookieName) {
        cookieName = tableName + '.' + cookieName;
        
        switch(that.options.cookieStorage) {
            case 'cookieStorage':
                document.cookie = [
                        encodeURIComponent(cookieName), '=',
                        '; expires=Thu, 01 Jan 1970 00:00:00 GMT',
                        that.options.cookiePath ? '; path=' + that.options.cookiePath : '',
                        that.options.cookieDomain ? '; domain=' + that.options.cookieDomain : '',
                    ].join('');
                break;
            case 'localStorage':
                localStorage.removeItem(cookieName);
            break;
            case 'sessionStorage':
                sessionStorage.removeItem(cookieName);
            break;

        }
        return true;
    };

    var calculateExpiration = function(cookieExpire) {
        var time = cookieExpire.replace(/[0-9]*/, ''); //s,mi,h,d,m,y
        cookieExpire = cookieExpire.replace(/[A-Za-z]{1,2}}/, ''); //number

        switch (time.toLowerCase()) {
            case 's':
                cookieExpire = +cookieExpire;
                break;
            case 'mi':
                cookieExpire = cookieExpire * 60;
                break;
            case 'h':
                cookieExpire = cookieExpire * 60 * 60;
                break;
            case 'd':
                cookieExpire = cookieExpire * 24 * 60 * 60;
                break;
            case 'm':
                cookieExpire = cookieExpire * 30 * 24 * 60 * 60;
                break;
            case 'y':
                cookieExpire = cookieExpire * 365 * 24 * 60 * 60;
                break;
            default:
                cookieExpire = undefined;
                break;
        }

        return cookieExpire === undefined ? '' : '; max-age=' + cookieExpire;
    };

    var initCookieFilters = function (bootstrapTable) {
        if (isloadfilter(bootstrapTable)) {
            setTimeout(function () {
                var parsedCookieFilters = JSON.parse(getCookie(bootstrapTable, bootstrapTable.options.cookieIdTable, cookieIds.filterControl));
                var cachedFilters = {};

                if (!bootstrapTable.options.filterControlValuesLoaded && parsedCookieFilters) {
                    bootstrapTable.options.filterControlValuesLoaded = true;

                    var header = getCurrentHeader(bootstrapTable),
                        searchControls = getCurrentSearchControls(bootstrapTable),

                        applyCookieFilters = function (element, filteredCookies) {
                            $(filteredCookies).each(function (i, cookie) {
                                if (cookie.regin) {
                                    $(element).attr("areaid", cookie.text);
                                    $(element).val(cookie.regin);
                                } else
                                    $(element).val(cookie.text);
                                cachedFilters[cookie.field] = cookie.text;
                            });
                        };

                    header.find(searchControls).each(function () {
                        var field = $(this).closest('[data-field]').data('field'),
                            filteredCookies = $.grep(parsedCookieFilters, function (cookie) {
                                return cookie.field === field;
                            });

                        applyCookieFilters(this, filteredCookies);
                    });
                }
                bootstrapTable.initColumnSearch(cachedFilters);
            }, 250);
        }
    };

    $.extend($.fn.bootstrapTable.defaults, {
        cookie: true,
        cookieExpire: '1d',
        cookiePath: '/',
        cookieDomain: null,
        cookieSecure: null,
        cookieIdTable: 'bootstrapTableCookie',
        cookiesEnabled: [
            'bs.table.sortOrder', 'bs.table.sortName',
            'bs.table.pageNumber', 'bs.table.pageList',
            'bs.table.filterControl', 'bs.table.columnid',
            'bs.table.subjectid'
        ],
        cookieStorage: 'cookieStorage', //localStorage, sessionStorage
        //internal variable
        filterControls: [],
        filterControlValuesLoaded: true
    });

    $.fn.bootstrapTable.methods.push('getCookies');
    $.fn.bootstrapTable.methods.push('deleteCookie');

    $.extend($.fn.bootstrapTable.utils, {
        setCookie: setCookie,
        getCookie: getCookie
    });

    var BootstrapTable = $.fn.bootstrapTable.Constructor,
        _init = BootstrapTable.prototype.init,
        _initTable = BootstrapTable.prototype.initTable,
        _initServer = BootstrapTable.prototype.initServer,
        _onSort = BootstrapTable.prototype.onSort,
        _onPageNumber = BootstrapTable.prototype.onPageNumber,
        _onPageListChange = BootstrapTable.prototype.onPageListChange,
        _onPageFirst = BootstrapTable.prototype.onPageFirst,
        _onPagePre = BootstrapTable.prototype.onPagePre,
        _onPageNext = BootstrapTable.prototype.onPageNext,
        _onPageLast = BootstrapTable.prototype.onPageLast,
        _toggleColumn = BootstrapTable.prototype.toggleColumn,
        _selectPage = BootstrapTable.prototype.selectPage,
        _onSearch = BootstrapTable.prototype.onSearch;

    BootstrapTable.prototype.init = function () {
        var timeoutId = 0;
        this.options.filterControls = [];
        this.options.filterControlValuesLoaded = false;

        this.options.cookiesEnabled = typeof this.options.cookiesEnabled === 'string' ?
            this.options.cookiesEnabled.replace('[', '').replace(']', '')
                .replace(/ /g, '').toLowerCase().split(',') :
            this.options.cookiesEnabled;

        if (this.options.filterControl) {
            var that = this;
            this.$el.on('column-search.bs.table', function (e, field, text) {
                var isNewField = true;

                for (var i = 0; i < that.options.filterControls.length; i++) {
                    if (that.options.filterControls[i].field === field) {
                        if (typeof (text) == "undefined" || text == "") {
                            that.options.filterControls.splice(i, 1);
                        } else {
                            that.options.filterControls[i].text = text;
                            that.options.filterControls[i].regin = getColumnFieldType(that, field) == "regin" ? that.$header.find(".date-filter-control.bootstrap-table-filter-control-" + field).val() : "";
                            isNewField = false;
                        }
                        break;
                    }
                }
                if (isNewField && typeof (text) != "undefined" && text != "") {
                    that.options.filterControls.push({
                        field: field,
                        text: text,
                        regin: getColumnFieldType(that, field) == "regin" ? that.$header.find(".date-filter-control.bootstrap-table-filter-control-" + field).val() : ""
                    });
                }

                setCookie(that, cookieIds.filterControl, JSON.stringify(that.options.filterControls));
                //搜索设置页码为第一页
                setCookie(that, cookieIds.pageNumber, 1);

            }).on('post-body.bs.table', initCookieFilters(that));

            var columnid = util.queryString("columnid"), subjectid = util.queryString("subjectid");
            if (!util.queryString("filter")) {
                clearCookie(that);
                if (columnid) {
                    setCookie(that, cookieIds.columnid, columnid);
                }
                if (subjectid) {
                    setCookie(that, cookieIds.subjectid, subjectid);
                }
            }
            
        }
        _init.apply(this, Array.prototype.slice.apply(arguments));
    };

    BootstrapTable.prototype.initServer = function () {
        var bootstrapTable = this,
            selectsWithoutDefaults = [],

            columnHasSelectControl = function (column) {
                return column.filterControl && column.filterControl === 'select';
            },

            columnHasDefaultSelectValues = function (column) {
                return column.filterData && column.filterData !== 'column';
            },

            cookiesPresent = function () {
                var cookie = JSON.parse(getCookie(bootstrapTable, bootstrapTable.options.cookieIdTable, cookieIds.filterControl));
                return bootstrapTable.options.cookie && cookie;
            };

        selectsWithoutDefaults = $.grep(bootstrapTable.columns, function(column) {
            return columnHasSelectControl(column) && !columnHasDefaultSelectValues(column);
        });

        // reset variable to original initServer function, so that future calls to initServer
        // use the original function from this point on.
        BootstrapTable.prototype.initServer = _initServer;

        // early return if we don't need to populate any select values with cookie values
        if (isloadfilter(bootstrapTable)) {
            return;
        }

        // call BootstrapTable.prototype.initServer
        _initServer.apply(this, Array.prototype.slice.apply(arguments));
    };


    BootstrapTable.prototype.initTable = function () {
        _initTable.apply(this, Array.prototype.slice.apply(arguments));
        this.initCookie();
    };

    BootstrapTable.prototype.initCookie = function () {
        if (!isloadfilter(this)) {
            return;
        }

        if ((this.options.cookieIdTable === '') || (this.options.cookieExpire === '') || (!cookieEnabled())) {
            throw new Error("Configuration error. Please review the cookieIdTable, cookieExpire properties, if those properties are ok, then this browser does not support the cookies");
        }

        var sortOrderCookie = getCookie(this, this.options.cookieIdTable, cookieIds.sortOrder),
            sortOrderNameCookie = getCookie(this, this.options.cookieIdTable, cookieIds.sortName),
            pageNumberCookie = getCookie(this, this.options.cookieIdTable, cookieIds.pageNumber),
            pageListCookie = getCookie(this, this.options.cookieIdTable, cookieIds.pageList),
            columnsCookie = JSON.parse(getCookie(this, this.options.cookieIdTable, cookieIds.columns)),
            searchTextCookie = getCookie(this, this.options.cookieIdTable, cookieIds.searchText);

        //sortOrder
        this.options.sortOrder = sortOrderCookie ? sortOrderCookie : this.options.sortOrder;
        //sortName
        this.options.sortName = sortOrderNameCookie ? sortOrderNameCookie : this.options.sortName;
        //pageNumber
        this.options.pageNumber = pageNumberCookie ? +pageNumberCookie : this.options.pageNumber;
        //pageSize
        this.options.pageSize = pageListCookie ? pageListCookie === this.options.formatAllRows() ? pageListCookie : +pageListCookie : this.options.pageSize;
        //searchText
        this.options.searchText = searchTextCookie ? searchTextCookie : '';

        if (columnsCookie) {
            $.each(this.columns, function (i, column) {
                column.visible = $.inArray(column.field, columnsCookie) !== -1;
            });
        }
    };

    BootstrapTable.prototype.onSort = function () {
        _onSort.apply(this, Array.prototype.slice.apply(arguments));
        setCookie(this, cookieIds.sortOrder, this.options.sortOrder);
        setCookie(this, cookieIds.sortName, this.options.sortName);
    };

    BootstrapTable.prototype.onPageNumber = function () {
        _onPageNumber.apply(this, Array.prototype.slice.apply(arguments));
        setCookie(this, cookieIds.pageNumber, this.options.pageNumber);
    };

    BootstrapTable.prototype.onPageListChange = function () {
        _onPageListChange.apply(this, Array.prototype.slice.apply(arguments));
        setCookie(this, cookieIds.pageList, this.options.pageSize);
    };

    BootstrapTable.prototype.onPageFirst = function () {
        _onPageFirst.apply(this, Array.prototype.slice.apply(arguments));
        setCookie(this, cookieIds.pageNumber, this.options.pageNumber);
    };

    BootstrapTable.prototype.onPagePre = function () {
        _onPagePre.apply(this, Array.prototype.slice.apply(arguments));
        setCookie(this, cookieIds.pageNumber, this.options.pageNumber);
    };

    BootstrapTable.prototype.onPageNext = function () {
        _onPageNext.apply(this, Array.prototype.slice.apply(arguments));
        setCookie(this, cookieIds.pageNumber, this.options.pageNumber);
    };

    BootstrapTable.prototype.onPageLast = function () {
        _onPageLast.apply(this, Array.prototype.slice.apply(arguments));
        setCookie(this, cookieIds.pageNumber, this.options.pageNumber);
    };

    BootstrapTable.prototype.toggleColumn = function () {
        _toggleColumn.apply(this, Array.prototype.slice.apply(arguments));

        var visibleColumns = [];

        $.each(this.columns, function (i, column) {
            if (column.visible) {
                visibleColumns.push(column.field);
            }
        });

        setCookie(this, cookieIds.columns, JSON.stringify(visibleColumns));
    };

    BootstrapTable.prototype.selectPage = function (page) {
        _selectPage.apply(this, Array.prototype.slice.apply(arguments));
        setCookie(this, cookieIds.pageNumber, page);
    };

    BootstrapTable.prototype.onSearch = function () {
        var target = Array.prototype.slice.apply(arguments);
        _onSearch.apply(this, target);

        if ($(target[0].currentTarget).parent().hasClass('search')) {
          setCookie(this, cookieIds.searchText, this.searchText);
        }
    };

    BootstrapTable.prototype.getCookies = function () {
        var bootstrapTable = this;
        var cookies = {};
        $.each(cookieIds, function(key, value) {
            cookies[key] = getCookie(bootstrapTable, bootstrapTable.options.cookieIdTable, value);
            if (key === 'columns') {
                cookies[key] = JSON.parse(cookies[key]);
            }
        });
        return cookies;
    };

    BootstrapTable.prototype.deleteCookie = function (cookieName) {
        if ((cookieName === '') || (!cookieEnabled())) {
            return;
        }

        deleteCookie(this, this.options.cookieIdTable, cookieIds[cookieName]);
    };
})(jQuery);
