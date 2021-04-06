

if (!window.whir) window.whir = {};
// 选择框操作.
whir.checkbox = {
    //销毁事件
    destroy: function () {
        $('input').iCheck('destroy');
    },
    //注册全选事件
    checkboxOnload: function (removeId, hidChoose, cbxtopId,cbxId) {
        var elTop,el;
        if (cbxtopId) {
            elTop = "input[name$='" + cbxtopId + "']";
        } else {
            elTop = "input[name=btSelectAll]";
        }
        if (cbxId) {
            el = "input[name$='" + cbxId + "']";
        } else {
            el = "input[name=btSelectItem]";
        }
        $(elTop)
            .next()
            .click(function () {
                var checked = $(elTop).prop('checked');
                if (checked) {
                    whir.checkbox.selectAll(removeId, hidChoose, cbxId);
                } else {
                    whir.checkbox.cancelSelectAll(removeId, hidChoose, cbxId);
                }
                if ($table) {
                    $table.find("tbody tr")
                        .each(function() {
                            $table
                                .bootstrapTable('getData')[
                                    parseInt($(this).attr("data-index"))][$table
                                .bootstrapTable('getOptions')
                                .idField] = checked;
                        });
                }
            });
        $(el)
            .each(function() {
                $(this).
                next()
                    .click(function() {
                        var checked = $(el + ":checked").length == $(el).length;
                        if (checked) {
                            $(elTop).iCheck('check');
                        } else {
                            $(elTop).iCheck('uncheck');
                        }
                        $("#" + hidChoose).val(whir.checkbox.getSelect(cbxId));
                        if ($("#" + hidChoose).val() !== "") {
                            $(removeId).prop('disabled', false);
                        } else {
                            $(removeId).prop('disabled', true);
                        }
                        if ($table) {
                            $table
                                .bootstrapTable('getData')[
                                    parseInt($(this).parent().parent().parent().attr("data-index"))][$table
                                .bootstrapTable('getOptions')
                                .idField] = $(this).prev().prop('checked');
                        }
                    });
            });

    },
    // 全选.
    selectAll: function (removeId, hidId, cbxId, midChar) {
        var el;
        if (cbxId) {
            el = "input[name$='" + cbxId + "']";
        } else {
            el = "input[name=btSelectItem]";
        }
        $(el).iCheck('check');
        $("#" + hidId).val(whir.checkbox.getSelect(cbxId, midChar));
        if ($("#" + hidId).val() !== "") {
            $(removeId).prop('disabled', false);
        } else {
            $(removeId).prop('disabled', true);
        }
    },
    cancelSelectAll: function (removeId, hidId, cbxId) {
        var el;
        if (cbxId) {
            el = "input[name$='" + cbxId + "']";
        } else {
            el = "input[name=btSelectItem]";
        }
        $(el).iCheck('uncheck');
        $("#" + hidId).val("");
        $(removeId).prop('disabled', true);
    },
    // 判断是否有选择.
    isSelect: function (buttonName) {
        var elements = document.getElementsByTagName("input");
        var selectedCount = 0;

        for (var i = 0; i < elements.length; i++) {
            if (elements[i].name.indexOf(buttonName) >= 0) {
                if (elements[i].checked == true) selectedCount += 1;
            }
        }
        if (selectedCount > 0) {
            return true;
        } else {
            return false;
        }
    },
    getSelect: function (cbxId, midChar) {
        var el;
        if (cbxId) {
            el = "input[name$='" + cbxId + "']";
        } else {
            el = "input[name=btSelectItem]";
        }
        var selected = "";
        $(el)
            .each(function () {
                if ($(this).prop('checked')) {
                    if (selected == "") {
                        selected = $(this).val();
                    } else {
                        if (midChar=="*")
                            selected = $(this).val() + midChar + selected;
                        else
                            selected = $(this).val() + "," + selected;
                    }
                }
            });
        return selected;
    },
    getSelectTotalCount: function (buttonName) {
        var selected = 0;
        var elements = document.getElementsByTagName("input");

        for (var i = 0; i < elements.length; i++) {
            if (elements[i].name.indexOf(buttonName) >= 0 && elements[i].name != "cbx_All" && elements[i].name != "btSelectAll") {
                if (elements[i].checked == true) {
                    selected++;
                }
            }
        }
        return selected;
    }
};
//列表表头全选checkbox
function checkTop(buttonName) {
    var elements = document.getElementsByTagName("input");
    for (var i = 0; i < elements.length; i++) {
        var el = elements[i];
        if (el.name.indexOf(buttonName) >= 0 && el.disabled == false) {
            el.checked = "checked";
        }
    }
}
//列表表头全选checkbox
function unCheckTop(buttonName) {
    var elements = document.getElementsByTagName("input");
    for (var i = 0; i < elements.length; i++) {
        var el = elements[i];
        if (el.name.indexOf(buttonName) >= 0 && el.disabled == false) {
            el.checked = "";
        }
    }
}

//去除数组重复项 
function unique(data) {
    data = data || [];
    var a = {};
    for (var i = 0; i < data.length; i++) {
        var v = data[i];
        if (typeof (a[v]) == 'undefined') {
            a[v] = 1;
        }
    };
    data.length = 0;
    for (var i in a) {
        data[data.length] = i;
    }
    return data;
}

// 进度条.
whir.progressbar = {
    // 设置进度条(百分比)
    setProgressbar: function (appPath, obj) {
        var boxImage = appPath + "res/js/progressbar/images/progressbar.gif";
        var barImage = appPath + "res/js/progressbar/images/progressbg_blue.gif";
        obj.progressBar({ boxImage: boxImage, barImage: barImage });
    },

    // 设置进度条(分数比)
    setProgressbarByFraction: function (appPath, obj, max) {
        var m = 0;
        try {
            m = parseInt(max);
        } catch (e) { }
        var boxImage = appPath + "res/js/progressbar/images/progressbar.gif";
        var barImage = appPath + "res/js/progressbar/images/progressbg_blue.gif";
        obj.progressBar({ max: m, textFormat: 'fraction', boxImage: boxImage, barImage: barImage });
    }
};
whir.control = {
    //隐藏所有的控件 ,tagName 标签名称，attributeName 标签的属性名称，attributeValue 标签的属性值
    getAllControlByNameToDisabled: function (tagName, attributeName, attributeValue) {
        var controls = $(tagName + "[" + attributeName + "=" + attributeValue + "]");
        for (var i = 0; i < controls.length; i++) {
            controls[i].style.display = "none";
        }
    },

    //显示所有的控件
    getAllControlByNameToEnabled: function (tagName, attributeName, attributeValue) {
    	var controls = $(tagName + "[" + attributeName + "=" + attributeValue + "]");
    	for (var i = 0; i < controls.length; i++) {
    		controls[i].style.display = "";
    	}
    }
};
whir.label = {
	dialog: function (title,columnId, subjectId, itemId) {//'置标预览'
	    whir.dialog.frame(title, _sysPath + "ModuleMark/Common/Label.aspx?columnId=" + columnId + "&subjectId=" + subjectId + "&itemId=" + itemId, null, $(window).width() - $("#tree-wrap").width() * 2, $(window).height() - 234, false);
		return false;
    },
    shareDialog: function (title,columnId, subjectId, itemId) {//'分享'
        whir.dialog.frame(title, _sysPath + "ModuleMark/Common/Share.aspx?columnId=" + columnId + "&subjectId=" + subjectId + "&itemId=" + itemId, null, 230, 65, false);
        return false;
    }
};
function HTMLDecode(encodeHtml) {
    var div = document.createElement("div");
    div.innerHTML = encodeHtml;
    return div.innerText || div.textContent;
}