var whir = window.whir || { wtl: {} };
whir.wtl = whir.wtl || {};
whir.wtl.tabHelp = {
    //添加选项卡
    addTab: function (name, fieldArray) {
        $("#tab_div h5 a:last").before("<a><span><b>" + name + "</b></span></a>");
        $("#default_ops .All_table table:last").after(template("opsTable", {
            list: fieldArray
        }));
    },
    //添加子栏目配置选项卡
    addColumnTab: function (name, index) {
        $("#tabs_column h5 a:last").before("<a data-index='" + index + "'><span><b>" + name + "</b></span></a>");
        $("#column_ops .All_table table:last").after(template("columnSon", {
            index: index
        }));
    },
    //清空选项卡
    clearTab: function () {
        $("#tab_div a:gt(0):not(.close-config)").detach();
        $("#default_ops .All_table table:not(:eq(0))").detach();
    },
    clearColumnTab: function () {
        $("#tabs_column h5 a:gt(0):not(.close-config)").remove();
        $("#column_ops .All_table table:gt(0)").remove();
    },
    delColumnTab: function (index) {
        $("#tabs_column h5 a[data-index='" + index + "']").detach();
        $("#column_ops .All_table table[data-index='" + index + "']").detach();
    },
    //选项卡事件
    tabFunction: function () {
        $("#tab_div a").unbind().bind("click", function () {
            if (!$(this).find("span").hasClass("show")) {
                var temp = $("#tab_div span.show");
                $(this).find("span").addClass("show");
                temp.removeClass("show");

                console.log($(this).index());

                $("#default_ops .All_table table").hide();
                $("#default_ops .All_table table:eq(" + $(this).index() + ")").show();
            }
        });
    },
    //栏目选项卡事件
    columnTabFcuntion: function () {
        $("#tabs_column a").unbind().bind("click", function () {
            if (!$(this).find("span").hasClass("show")) {
                var temp = $("#tabs_column span.show");
                $(this).find("span").addClass("show");
                temp.removeClass("show");

                console.log($(this).index());

                $("#column_ops .All_table table").hide();
                $("#column_ops .All_table table:eq(" + $(this).index() + ")").show();
            }
        });
    },
    addWtlTab: function (wtlId) {
        var wtlOps = whir.wtl.Service.getWtlObj(wtlId);
        if (wtlOps != null) {
            whir.wtl.tabHelp.clearTab();
            if (wtlOps.wtlCodeArray != undefined) {
                for (var i in wtlOps.wtlCodeArray) {
                    var wtlCode = wtlOps.wtlCodeArray[i];
                    whir.wtl.tabHelp.addTab(wtlCode.name + "字段配置", wtlCode.fieldArray);
                }
            }
        }
    }
}