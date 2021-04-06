var whir = window.whir || { wtl: {} };
whir.wtl = whir.wtl || {};
//置标生成
whir.wtl.generateHelp = {
    menu: {
        getSonFieldReplaceCode: function (wtlField, wtlColumnSon) {
            var replaceVal = "";
            if (wtlField != null) {
                switch (wtlField.type) {
                    case "栏目名称":
                        switch (wtlColumnSon.type) {
                            case "column":
                                replaceVal = "{$ColumnName}";
                                break;
                        }
                        break;
                    case "A链接":
                        switch (wtlColumnSon.type) {
                            case "column":
                                replaceVal = "{$url,{$ColumnId}}";
                                break;
                        }
                        break;
                }
            }
            return replaceVal;
        },
        getFieldReplaceCode: function (wtlField, wtlColumn) {
            var replaceVal = "";
            var columnIdHtml = "";
            if (wtlColumn.columnId > 0) {
                columnIdHtml = "ColumnId='" + wtlColumn.columnId + "'";
            }

            if (wtlField != null) {
                switch (wtlField.type) {
                    case "index":
                        replaceVal = wtlColumn.index;
                        break;
                    case "栏目ID":
                        replaceVal = wtlColumn.columnId;
                        break;
                    case "栏目名称":
                        switch (wtlColumn.type) {
                            case "column":
                                replaceVal = "<wtl:column " + columnIdHtml + "></wtl:column>";
                                break;
                            case "other":
                                replaceVal = wtlColumn.elseText;
                                break;
                            default:
                                replaceVal = "首页";
                                break;
                        }
                        break;
                    case "A链接":
                        switch (wtlColumn.type) {
                            case "column":
                                replaceVal = "{$url," + (wtlColumn.columnId > 0 ? wtlColumn.columnId.toString() : "{@columnid}") + "}";
                                break;
                            case "other":
                                replaceVal = wtlColumn.elseUrl;
                                break;
                            default:
                                replaceVal = "{$indexurl}";
                                break;
                        }
                        break;
                    case "栏目别名":
                        switch (wtlColumn.type) {
                            case "column":
                                replaceVal = "<wtl:column " + columnIdHtml + " Field='ColumnNameStage'></wtl:column>";
                                break;
                        }
                        break;
                }
            }
            return replaceVal;
        },
        getSonListHtml: function (wtlColumn, wtlCode, sonListColumnHtml) {
            var sonColumnWtlHtml = "";

            var sonColumn = wtlColumn.sonColumn;
            if (sonColumn.type == "column") {
                for (var y in wtlCode.fieldArray) {
                    var field = wtlCode.fieldArray[y];
                    sonListColumnHtml = whir.wtl.generateHelp.getReplaceWtlCodeHtml(sonListColumnHtml, whir.wtl.generateHelp.getFieldReplaceHtml(field.matchStr), whir.wtl.generateHelp.menu.getSonFieldReplaceCode(field, sonColumn));
                }


                sonColumnWtlHtml = "<wtl:list sql=\"select * from Whir_Dev_Column Where parentid=" + sonColumn.columnId + " \"> ";
                sonColumnWtlHtml += sonListColumnHtml;
                sonColumnWtlHtml += "</wtl:list>";


            } else if (sonColumn.elseArray != null && sonColumn.elseArray.length > 0) {


                for (var y in sonColumn.elseArray) {

                    var tempElseColumnWtlHtml = sonListColumnHtml;
                    for (var z in wtlCode.fieldArray) {
                        field = wtlCode.fieldArray[z];
                        var replaceVal = "";

                        switch (field.type) {
                            case "A链接":
                                replaceVal = sonColumn.elseArray[y].url;
                                break;
                            case "栏目名称":
                                replaceVal = sonColumn.elseArray[y].text;
                                break;
                        }
                        tempElseColumnWtlHtml = whir.wtl.generateHelp.getReplaceWtlCodeHtml(tempElseColumnWtlHtml, whir.wtl.generateHelp.getFieldReplaceHtml(field.matchStr), replaceVal);
                    }


                    sonColumnWtlHtml += tempElseColumnWtlHtml;
                }
            }

            return sonColumnWtlHtml;
        },
        getListHtml: function (wtl, wtlCode, wtlCodeHtmlDom) {
            var columnWtlHtml = "";
            for (var i in whir.wtl.Array.wtlMenuArray) {
                var columnTempCodeHtml = wtlCodeHtmlDom.html();
                var wtlColumn = whir.wtl.Array.wtlMenuArray[i];
                if (wtlColumn.wtlId == wtl.id) {
                    var codeDom = $($.trim(columnTempCodeHtml));
                    var oneColumnHtml = codeDom.prop("outerHTML");
                    var sonListColumnHtml = codeDom.find("son list").html();

                    var field;
                    for (var y in wtlCode.fieldArray) {
                        field = wtlCode.fieldArray[y];
                        oneColumnHtml = whir.wtl.generateHelp.getReplaceWtlCodeHtml(oneColumnHtml, whir.wtl.generateHelp.getFieldReplaceHtml(field.matchStr), whir.wtl.generateHelp.menu.getFieldReplaceCode(field, wtlColumn));
                    }
                    columnTempCodeHtml = oneColumnHtml;

                    var sonColumnWtlHtml = "";
                    codeDom = $($.trim(columnTempCodeHtml));
                    if (wtlColumn.isSon && wtlColumn.sonColumn != null) {
                        whir.wtl.generateHelp.menu.getSonListHtml(wtlColumn, wtlCode, sonListColumnHtml);
                    } else {
                        codeDom.find("son").detach();
                    }


                    codeDom.find("son list").after(sonColumnWtlHtml);
                    codeDom.find("son list").remove();


                    codeDom.find("son").after(codeDom.find("son").html());
                    codeDom.find("son").remove();

                    columnWtlHtml += "\n" + codeDom.prop("outerHTML");

                }
            }

            return columnWtlHtml;
        },
        getSingleHtml: function (wtl, wtlCode, wtlCodeHtmlDom) {
            var columnTempCodeHtml = wtlCodeHtmlDom.html();
            var field;
            for (var y in wtlCode.fieldArray) {
                field = wtlCode.fieldArray[y];
                columnTempCodeHtml = whir.wtl.generateHelp.getReplaceWtlCodeHtml(columnTempCodeHtml, whir.wtl.generateHelp.getFieldReplaceHtml(field.matchStr), whir.wtl.generateHelp.menu.getFieldReplaceCode(field, whir.wtl.Service.getWtlMenu(field.wtlColumnIndex)));
            }

            return columnTempCodeHtml;
        }
    },
    list: {
        getWtlHtml: function (wtl, wtlCodeHtml, wtlCode) {
            var tempCodeHtml = "";
            var columnIdHtml = "";
            if (whir.wtl.columnHelp.getIsBindColumn()) {
                columnIdHtml = " columnid=\"" + wtl.columnId + "\" ";
            }


            tempCodeHtml += "<wtl:list " +
                "ID=\"" + wtlCode.id + "\" " +
                columnIdHtml + " " +
                "count='" + wtlCode.listTopCount + "' " +
                "needpage='" + wtlCode.isListNeedpage + "' " +
                "> ";
            tempCodeHtml += wtlCodeHtml;
            tempCodeHtml += "</wtl:list>";


            return tempCodeHtml;
        },
        getCodeHtml: function (wtl, wtlCode, wtlCodeHtml) {
            var codeHtml = "";
            if (wtlCodeHtml != "") {
                codeHtml = wtlCodeHtml;
                for (var f in wtlCode.fieldArray) {
                    var wtlField = wtlCode.fieldArray[f];
                    if (wtlField != null) {
                        codeHtml = whir.wtl.generateHelp.getReplaceWtlCodeHtml(codeHtml, whir.wtl.generateHelp.getFieldReplaceHtml(wtlField.matchStr), whir.wtl.generateHelp.getFieldReplaceCode(wtlField, wtl, wtlCode));
                    }
                }
            }
            return codeHtml;
        }
    },
    content: {
        getWtlHtml: function (wtl, wtlCodeHtml, wtlCode, isOnlyFixedText) {
            var tempCodeHtml = wtlCodeHtml;
            if (wtl.columnId > 0 && !isOnlyFixedText) {
                tempCodeHtml = "<wtl:inforarea " + (whir.wtl.columnHelp.getIsBindColumn() ? " ColumnId='" + wtl.columnId + "' " : "") + ">" + wtlCodeHtml + "</wtl:inforarea>";
            }
            return tempCodeHtml;
        }
    },
    //获取默认字段替换代码
    getFieldReplaceCode: function (wtlField, wtl, wtlCode) {
        var replaceVal = "";
        if (wtlField != null) {
            switch (wtlField.type) {
                case "固定文本":
                    replaceVal = wtlField.fixedTextVal;
                    if (replaceVal == undefined || replaceVal == "") {
                        replaceVal = wtlField.defaultVal;
                    }
                    break;
                case "图片链接":
                    replaceVal = "{$uploadpath}{$" + wtlField.columnFieldName + "}";
                    break;
                case "文本字段":
                    replaceVal = "{$" + wtlField.columnFieldName;
                    if (Number(wtlField.substringNumber) > 0) {
                        replaceVal += "," + wtlField.substringNumber;
                    }
                    replaceVal += "}";
                    break;
                case "日期时间":
                    replaceVal = "{$" + wtlField.columnFieldName + "}";
                    break;
                case "A链接":
                    switch (wtlField.atype) {
                        case "系统生成":
                            switch (wtlCode.type) {
                                case "list":
                                    replaceVal = "{$PageUrl" + (wtlField.elseparm != "" ? "," + wtlField.elseparm : "") + "}";
                                    break;
                                case "content":
                                    replaceVal = "{$url," + wtl.columnId + (wtlField.elseparm != "" ? "," + wtlField.elseparm : "") + "}";
                                    break;
                            }
                            break;
                        case "选择字段":
                            replaceVal = "{$" + wtlField.columnFieldName + "}";
                            break;
                        case "固定地址":
                            replaceVal = wtlField.fixedUrl;
                            break;
                        case "首页链接":
                            replaceVal = "{$indexurl}";
                            break;

                    }
                    break;
                case "版权":
                    replaceVal = "<wtl:system type='copyright' sitepath='cn'></wtl:system>";
                    break;
                case "备案":
                    replaceVal = "<wtl:system type='record' sitepath='cn'></wtl:system>";
                    break;
                case "点击率":
                    replaceVal = "<wtl:Hits " + (wtl.columnId > 0 ? "ColumnId='" + wtl.columnId + "'" : "") + " Field='" + (wtlField.columnFieldName == "" ? "Hits" : wtlField.columnFieldName) + "'></wtl:Hits>";
                    break;
            }
        }

        return replaceVal;
    },
    //获取字段要替换的占位符
    getFieldReplaceHtml: function (matchStr) {
        return matchStr.replace(/\$/g, "\\$").replace(/\{/g, "\\{").replace(/\}/g, "\\}");
    },
    //获取替换后的值
    getReplaceWtlCodeHtml: function (codeHtml, replaceHtml, replaceVal) {
        return codeHtml.replace(new RegExp(replaceHtml, "g"), replaceVal);
    },
    //是否可以生成
    isCanGenrate: function (wtl, wtlCode) {
        return wtl != undefined && wtlCode != undefined && wtlCode.fieldArray != undefined;
    },
    //生成置标
    generateWtl: function (wtlIframeDom, wtlArray) {
        if (wtlIframeDom != undefined) {
            wtlIframeDom.find("view").detach();

            if (wtlArray != undefined) {
                for (var i in wtlArray) {
                    if (wtlArray.hasOwnProperty(i)) {

                        var wtl = wtlArray[i];
                        var wtlHtmlDom = wtlIframeDom.find("[wtl-edit][wtl-id='" + wtl.id + "']");

                        if (wtl.wtlCodeArray != undefined && wtlHtmlDom != undefined) {

                            for (var y in wtl.wtlCodeArray.sort(whir.wtl.wtlCodeSort)) {
                                if (wtl.wtlCodeArray.hasOwnProperty(y)) {
                                    var wtlCode = wtl.wtlCodeArray[y];
                                    var wtlCodeHtmlDom = wtlHtmlDom.find("script[type^='html/'][id='" + wtlCode.id + "']");

                                    whir.wtlGlobal.getByType(wtlCode.type).generate(wtl, wtlCode, wtlCodeHtmlDom);

                                    var tempHtml = wtlCodeHtmlDom.html();
                                    var tempParent = wtlCodeHtmlDom.parent();
                                    $(tempParent).html(tempHtml);
                                }
                            }
                        }
                    }
                }

            }
        }
    }
}