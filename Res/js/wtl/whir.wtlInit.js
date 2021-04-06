var whir = window.whir || {};
whir.wtlInit = {
    data: {
        columnArray: function () {
            $("#select_coulumn option").each(function () {
                var columnId = $(this).val();
                if (columnId > 0) {
                    var column = new whir.wtl.columnHelp.Column();
                    column.name = $(this).text();

                    $.get(globalObj.basePath + "Whir_system/Wtl/ColumnAjax.aspx?columnid=" + columnId, function (res) {
                        column.id = columnId;
                        column.field = JSON.parse(res);
                        whir.wtl.columnHelp.columnArray.push(column);
                    });
                }
            });
        },
        webFormArray:function() {
            $.get(globalObj.basePath + "Whir_system/Wtl/WebFormAjax.aspx", function (res) {
                whir.wtl.Array.webFormArray = JSON.parse(res);
                whir.wtlInit.webFormSelect();
            });
        }
    },
    columnFieldSelect: function (id) {
        $(".field_select option").detach();
        $(".field_select").append($("<option>").html("选择表单字段"));
        if (id > 0) {
            var column = whir.wtl.columnHelp.getColumn(id);
            if (column != null) {
                var field = column.field;
                for (var i in field) {
                    $(".field_select").append($("<option>").html(field[i].FieldAlias).val(field[i].FieldName));
                }
            } else {
                alert("栏目数据不存在，请刷新页面重新加载");
            }
        }
    },
    columnSelect: function () {
        if (whir.wtl.columnHelp.columnArray instanceof Array) {
            $(".column_select select option").detach();
            $(".column_select select").append($("<option>").html("选择栏目").val(0));
            for (var i in whir.wtl.columnHelp.columnArray) {
                var column = whir.wtl.columnHelp.columnArray[i];
                $(".column_select select").append($("<option>").html(column.name).val(column.id));
            }
        }
    },
    sonColumnSelect: function (index) {
        if (whir.wtl.columnHelp.columnArray instanceof Array) {
            var sonColumnSelectDiv = $("#columnSon_" + index + "_select");

            sonColumnSelectDiv.find("select option").detach();
            sonColumnSelectDiv.find("select").append($("<option>").html("选择栏目").val(0));
            for (var i in whir.wtl.columnHelp.columnArray) {
                var column = whir.wtl.columnHelp.columnArray[i];
                sonColumnSelectDiv.find("select").append($("<option>").html(column.name).val(column.id));
            }
        }
    },
    wtlListSelect: function () {
        if (whir.wtl.Array.wtlArray != null) {
            $(".wtllist_select option").remove();
            $(".wtllist_select").append($("<option>").html("选择列表").val(0));
            for (var i in whir.wtl.Array.wtlArray) {
                for (var y in whir.wtl.Array.wtlArray[i].wtlCodeArray) {
                    var code = whir.wtl.Array.wtlArray[i].wtlCodeArray[y];
                    if (code.type == "list") {
                        $(".wtllist_select").append($("<option>").html("列表" + code.id).val(code.id)).attr("wtlid", whir.wtl.Array.wtlArray[i].id);
                    }
                }
            }
        }
    },
    menuTable: function (id) {
        var wtlObj = whir.wtl.Service.getWtlObj(id);
        $("#column_table tr").detach();
        var wtlColumn;
        var html;
        if (Number(wtlObj.menuMax) > 0) {

            for (var i = 1; i <= wtlObj.menuMax; i++) {
                wtlColumn = new whir.wtl.Model.WtlMenu();
                wtlColumn.index = i;
                wtlColumn.wtlId = wtlObj.id;
                whir.wtl.Array.wtlMenuArray.push(wtlColumn);
                html = template("columnTr", {
                    indexName: "栏目" + whir.wtl.NumberToChinese(i),
                    index: i,
                    isSon: wtlObj.isRequiredSon ? true : wtlObj.isSon
                });
                $("#column_table").append(html);
            }
        } else if (!wtlObj.isSon && wtlObj.wtlCodeArray != undefined && wtlObj.wtlCodeArray.length > 0) {

            var tempObj = {};
            for (var i in wtlObj.wtlCodeArray[0].fieldArray) {
                var field = wtlObj.wtlCodeArray[0].fieldArray[i];
                if (tempObj[field.name] == undefined) {
                    wtlColumn = new whir.wtl.Model.WtlMenu();
                    wtlColumn.index = i;
                    wtlColumn.wtlId = wtlObj.id;
                    whir.wtl.Array.wtlMenuArray.push(wtlColumn);

                    html = template("columnTr", {
                        indexName: field.name,
                        index: i,
                        isSon: false
                    });
                    $("#column_table").append(html);

                    tempObj[field.name] = "";
                }

                if (wtlColumn != undefined) {
                    wtlObj.wtlCodeArray[0].fieldArray[i].wtlColumnIndex = wtlColumn.index;
                }
            }
        }
    },
    webFormSelect:function() {
        if (whir.wtl.Array.webFormArray != null) {
            $(".form_select option").remove();
            $(".form_select").append($("<option>").val(0).html("选择表单"));
            for (var i in whir.wtl.Array.webFormArray) {
                var webForm = whir.wtl.Array.webFormArray[i];
                $(".form_select").append($("<option>").val(webForm.SubmitID).html(webForm.Name));
            }
        }
    },
    webFormHtmlCode: function (id) {
        var wtl = whir.wtl.Service.getWtlObj(id);
        if (wtl != null && wtl.wtlCodeArray != null && wtl.wtlCodeArray.length > 0) {
            var wtlCode = wtl.wtlCodeArray[0];
            var tempDom = $($.trim(wtlCode.htmlDom.parent().prop("outerHTML")));
            tempDom.find("script[type^='html/']").detach();
            $("#form_code").html($.trim(tempDom.html()));
        }
    }
    
}