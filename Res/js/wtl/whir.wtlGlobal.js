var whir = window.whir || {};
whir.wtlGlobal = {
    list: {
        generate: function (wtl, wtlCode, wtlCodeHtmlDom) {
            var wtlCodeHtml = wtlCodeHtmlDom.html();
            var tempCodeHtml = "";
            if (whir.wtl.generateHelp.isCanGenrate(wtl, wtlCode)) {

                wtlCode.listTopCount = wtlCodeHtmlDom.parent().children(":not(script)").length;
                tempCodeHtml = whir.wtl.generateHelp.list.getWtlHtml(wtl, whir.wtl.generateHelp.list.getCodeHtml(wtl, wtlCode, wtlCodeHtml), wtlCode);
            }

            wtlCodeHtmlDom.html(tempCodeHtml);
        },
        optionEvent: function () {
            whir.wtlGlobal["defaults"].optionEvent();
        },
        init: function (wtlId) {
            whir.wtlGlobal["defaults"].init(wtlId);
        }
    },
    content: {
        generate: function (wtl, wtlCode, wtlCodeHtmlDom) {
            var wtlCodeHtml = wtlCodeHtmlDom.html();

            if (whir.wtl.generateHelp.isCanGenrate(wtl, wtlCode)) {
                var isOnlyFixedText = true;
                for (var f in wtlCode.fieldArray) {
                    if (wtlCode.fieldArray.hasOwnProperty(f)) {
                        var wtlField = wtlCode.fieldArray[f];
                        if (wtlField != null) {
                            var replaceHtml = whir.wtl.generateHelp.getFieldReplaceHtml(wtlField.matchStr);
                            var replaceVal = whir.wtl.generateHelp.getFieldReplaceCode(wtlField, wtl, wtlCode);
                            switch (wtlField.type) {
                                case "图片链接":
                                case "A链接":
                                    isOnlyFixedText = false;
                                    break;
                            }

                            wtlCodeHtml = whir.wtl.generateHelp.getReplaceWtlCodeHtml(wtlCodeHtml, replaceHtml, replaceVal);
                        }
                    }
                }

                wtlCodeHtml = whir.wtl.generateHelp.content.getWtlHtml(wtl, wtlCodeHtml, wtlCode, isOnlyFixedText);
                wtlCodeHtmlDom.html(wtlCodeHtml);
            }
        },
        optionEvent: function () {
            whir.wtlGlobal["defaults"].optionEvent();
        },
        init: function (wtlId) {
            whir.wtlGlobal["defaults"].init(wtlId);
        }
    },
    listtop: {
        generate: function (wtl, wtlCode, wtlCodeHtmlDom) {
            var wtlCodeHtml = wtlCodeHtmlDom.html();
            if (whir.wtl.generateHelp.isCanGenrate(wtl, wtlCode)) {
                var tempHtml = whir.wtl.generateHelp.list.getWtlHtml(wtl, whir.wtl.generateHelp.list.getCodeHtml(wtl, wtlCode, wtlCodeHtml));
                wtlCodeHtmlDom.html(tempHtml);
            }
        },
        optionEvent: function () {
            whir.wtlGlobal["defaults"].optionEvent();
        },
        init: function (wtlId) {
            whir.wtlGlobal["defaults"].init(wtlId);
        }
    },
    menu: {
        generate: function (wtl, wtlCode, wtlCodeHtmlDom) {
            if (whir.wtl.generateHelp.isCanGenrate(wtl, wtlCode) && whir.wtl.Array.wtlMenuArray != null) {


                var columnWtlHtml = "";
                if (Number(wtl.menuMix) == isNaN || Number(wtl.menuMix) == 0) {
                    columnWtlHtml = whir.wtl.generateHelp.menu.getSingleHtml(wtl, wtlCode, wtlCodeHtmlDom);
                } else {
                    columnWtlHtml = whir.wtl.generateHelp.menu.getListHtml(wtl, wtlCode, wtlCodeHtmlDom);
                }

                wtlCodeHtmlDom.html(columnWtlHtml);
            }
        },
        optionEvent: function () {
            $(".columnType:radio").unbind().bind("click", function () {
                var type = $(this).val();
                var index = $(this).attr("data-index");
                var selectTd = $("#column_table td[data-index='" + index + "']");

                $("#column_" + index + "_select").hide();
                $("#other_" + index + "_input").hide();
                $("[ops_tr_index='" + index + "']").show();
                selectTd.prop("rowspan", Number(selectTd.attr("oldrowspan")));
                switch (type) {
                    case "column":
                        $("#column_" + index + "_select").show();
                        break;
                    case "other":
                        $("#other_" + index + "_input").show();
                        break;
                    default:

                        selectTd.prop("rowspan", Number(selectTd.attr("rowspan")) - 1);
                        $("[ops_tr_index='" + index + "']").hide();
                        break;
                }

                whir.wtl.Service.updateWtlMenu(index, function (column) {
                    column.type = type;
                    return column;
                });

            });

            $(".sonColumn:radio").unbind().bind("click", function () {
                var isSon = $(this).val();
                var index = $(this).attr("data-index");
                if (Number(isSon) > 0) {
                    whir.wtl.tabHelp.addColumnTab("栏目" + whir.wtl.NumberToChinese(index) + "子栏目", index);
                    whir.wtlInit.sonColumnSelect(index);
                    whir.wtlGlobal.menu.sonColumnOptionEvent();
                } else {
                    whir.wtl.tabHelp.delColumnTab(index);
                }

                whir.wtl.tabHelp.columnTabFcuntion();

                whir.wtl.Service.updateWtlMenu(index, function (column) {
                    column.isSon = Number(isSon) > 0;
                    return column;
                });

            });

            var otherInputTime;
            $(".other_input input").unbind().bind("keyup", function () {
                clearTimeout(otherInputTime);

                var index = $(this).attr("data-index");
                var name = $(this).attr("name");
                var val = $(this).val();
                otherInputTime = setTimeout(function () {

                    whir.wtl.Service.updateWtlMenu(index, function (column) {

                        switch (name) {
                            case "url":
                                column.elseUrl = val;
                                break;
                            case "text":
                                column.elseText = val;
                                break;
                        }

                        return column;
                    });


                }, 500);
            });

            $(".column_select select").unbind().bind("change", function () {
                var index = $(this).attr("data-index");
                var val = $(this).val();

                whir.wtl.Service.updateWtlMenu(index, function (column) {
                    column.columnId = val;
                    return column;
                });


            });
        },
        init: function (wtlId) {
            whir.wtl.tabHelp.clearColumnTab();
            whir.wtlInit.menuTable(wtlId);
            whir.wtlInit.columnSelect();
            whir.wtlGlobal.getByType("menu").optionEvent();
            whir.wtlGlobal.openConfig("column");
        },
        sonColumnOptionEvent: function () {
            $(".columnSonType").unbind().bind("click", function () {
                var type = $(this).val();
                var index = $(this).attr("data-index");


                $("#columnSon_" + index + "_select").hide();
                $("#otherSon_" + index + "_input").hide();
                switch (type) {
                    case "column":
                        $("#columnSon_" + index + "_select").show();
                        break;
                    case "other":
                        $("#otherSon_" + index + "_input").show();
                        break;
                }

                whir.wtl.Service.updateWtlMenuSon(index, function (columnSon) {
                    columnSon.type = type;
                    return columnSon;
                });

            });

            $(".columnSon_select select").unbind().bind("change", function () {
                var index = $(this).attr("data-index");
                var val = $(this).val();

                whir.wtl.Service.updateWtlMenuSon(index, function (columnSon) {
                    columnSon.columnId = val;
                    return columnSon;
                });

            });

            var otherSonInputTime;
            $(".otherSon_input input").unbind().bind("keyup", function () {
                clearTimeout(otherSonInputTime);

                var valData = {};
                var arraydex = $(this).parent().attr("array-index");
                var index = $(this).attr("data-index");
                var name = $(this).attr("name");
                var val = $(this).val();

                switch (name) {
                    case "url":
                        valData.url = val;
                        break;
                    case "text":
                        valData.text = val;
                        break;
                }


                otherSonInputTime = setTimeout(function () {

                    whir.wtl.Service.updateWtlMenuSon(index, function (columnSon) {

                        if (columnSon.elseArray != null && columnSon.elseArray.length > arraydex) {
                            var temp = columnSon.elseArray[arraydex];
                            columnSon.elseArray[arraydex] = $.extend(temp, valData);
                        } else {
                            columnSon.elseArray.push(valData);
                        }

                        return columnSon;

                    });


                }, 500);
            });


            $(".otherSon_add_a").unbind().bind("click", function () {
                var thisParent = $(this).parent();
                thisParent.parent().find("div:last").after("<div array-index='" + thisParent.parent().find("div[array-index]").length + "'>" + thisParent.html() + "</div>");
                thisParent.parent().find(".otherSon_del_a:last").show();
                whir.wtlGlobal.menu.sonColumnOptionEvent();
            });
        }
    },
    form: {
        generate: function (wtl, wtlCode, wtlCodeHtmlDom) {
            if (wtlCode != null) {
                wtlCodeHtmlDom.html("<wtl:webform formid='" + wtlCode.formId + "'></wtl:webform>");
            }
        },
        optionEvent: function () {
            $(".form_select").unbind().bind("change", function () {
                var wtlId = whir.wtl.getSelWtlId();
                var wtl = whir.wtl.Service.getWtlObj(wtlId);
                var selVal = $(this).val();
                if (wtl != null && wtl.wtlCodeArray != null && wtl.wtlCodeArray.length > 0) {
                    whir.wtl.Service.updateWtl(wtlId, function (wtlObj) {
                        wtlObj.wtlCodeArray[0].formId = selVal;
                        return wtlObj;
                    });
                }
            });
        },
        init: function (wtlId) {
            whir.wtlInit.data.webFormArray();
            whir.wtlInit.webFormHtmlCode(wtlId);

            whir.wtlGlobal.getByType("form").optionEvent();
            whir.wtlGlobal.openConfig("form");
        }
    },
    pager: {
        generate: function (wtl, wtlCode, wtlCodeHtmlDom) {
            if (wtlCode != null) {
                wtlCodeHtmlDom.html("<wtl:Pager TargetID='" + wtlCode.pagperWtlCodeListId + "'></wtl:Pager>");
            }
        },
        optionEvent: function () {
            $(".wtllist_select").unbind().bind("change", function () {
                var wtlId = whir.wtl.getSelWtlId();
                var wtl = whir.wtl.Service.getWtlObj(wtlId);
                var selListCodeId = $(this).val();
                var selListWtlId = $(this).attr("wtlid");
                if (wtl != null && wtl.wtlCodeArray != null && wtl.wtlCodeArray.length > 0) {
                    whir.wtl.Service.updateWtl(wtlId, function (wtlObj) {
                        wtlObj.wtlCodeArray[0].pagperWtlCodeListId = selListCodeId;
                        return wtlObj;
                    });


                    whir.wtl.Service.updateWtl(selListWtlId, function (wtlObj) {
                        for (var i in wtlObj.wtlCodeArray) {
                            if (wtlObj.wtlCodeArray[i].id == selListCodeId) {
                                wtlObj.wtlCodeArray[i].isListNeedpage = true;
                                break;
                            }
                        }
                        return wtlObj;
                    });
                }
            });
        },
        init: function (wtlId) {
            whir.wtlInit.wtlListSelect();
            whir.wtlGlobal.getByType("pager").optionEvent();
            whir.wtlGlobal.openConfig("pager");
        }
    },
    location: {
        generate: function (wtl, wtlCode, wtlCodeHtmlDom) {
            if (wtlCode != null && wtlCode.locationOps != null) {
                var codeHtml = wtlCodeHtmlDom.html();
                var locationOps = wtlCode.locationOps;
                var tempHtml = "<wtl:Location " +
                    " ColumnId='" + locationOps.columnId + "'" +
                    " Separator='" + locationOps.Separator + "'" +
                    " IsShowColumnNameStage='" + locationOps.IsShowColumnNameStage + "'" +
                    " DefaultValue='" + locationOps.DefaultValue + "'" +
                    " IsCategory='" + locationOps.IsCategory + "'" +
                    " CategoryParam='" + locationOps.CategoryParam + "'></wtl:Location>";


                if (codeHtml.indexOf("{code}") > -1) {
                    codeHtml = codeHtml.replace("{code}", tempHtml);
                } else {
                    codeHtml = tempHtml;
                }

                wtlCodeHtmlDom.html(codeHtml);
            }
        },
        optionEvent: function () {
            $("[name='location_column']").unbind().bind("change", function () {
                var wtlId = whir.wtl.getSelWtlId();
                var wtl = whir.wtl.Service.getWtlObj(wtlId);
                var selVal = $(this).val();
                if (wtl != null && wtl.wtlCodeArray != null && wtl.wtlCodeArray.length > 0) {
                    whir.wtl.Service.updateWtl(wtlId, function (wtlObj) {
                        wtlObj.wtlCodeArray[0].locationOps = $.extend(wtlObj.wtlCodeArray[0].locationOps, {
                            columnId: selVal
                        });
                        return wtlObj;
                    });
                }
            });

            $("[name='location_IsShowColumnNameStage']").unbind().bind("click", function () {
                var wtlId = whir.wtl.getSelWtlId();
                var wtl = whir.wtl.Service.getWtlObj(wtlId);
                var val = $(this).val() == "1";
                if (wtl != null && wtl.wtlCodeArray != null && wtl.wtlCodeArray.length > 0) {
                    whir.wtl.Service.updateWtl(wtlId, function (wtlObj) {
                        wtlObj.wtlCodeArray[0].locationOps = $.extend(wtlObj.wtlCodeArray[0].locationOps, {
                            IsShowColumnNameStage: val
                        });
                        return wtlObj;
                    });
                }
            });


            $("[name='location_IsCategory']").unbind().bind("click", function () {
                var wtlId = whir.wtl.getSelWtlId();
                var wtl = whir.wtl.Service.getWtlObj(wtlId);
                var val = $(this).val() == "1";
                if (wtl != null && wtl.wtlCodeArray != null && wtl.wtlCodeArray.length > 0) {
                    whir.wtl.Service.updateWtl(wtlId, function (wtlObj) {
                        wtlObj.wtlCodeArray[0].locationOps = $.extend(wtlObj.wtlCodeArray[0].locationOps, {
                            IsCategory: val
                        });
                        return wtlObj;
                    });
                }
            });

            var locationValTime;
            $("[name='location_CategoryParam']").unbind().bind("keyup", function () {
                var wtlId = whir.wtl.getSelWtlId();
                var wtl = whir.wtl.Service.getWtlObj(wtlId);
                var val = $(this).val();
                if (wtl != null && wtl.wtlCodeArray != null && wtl.wtlCodeArray.length > 0) {
                    clearTimeout(locationValTime);
                    locationValTime = setTimeout(function () {
                        whir.wtl.Service.updateWtl(wtlId, function (wtlObj) {
                            wtlObj.wtlCodeArray[0].locationOps = $.extend(wtlObj.wtlCodeArray[0].locationOps, {
                                CategoryParam: val
                            });
                            return wtlObj;
                        });
                    }, 500);

                }
            });

            $("[name='location_Separator']").unbind().bind("keyup", function () {
                var wtlId = whir.wtl.getSelWtlId();
                var wtl = whir.wtl.Service.getWtlObj(wtlId);
                var val = $(this).val();
                if (wtl != null && wtl.wtlCodeArray != null && wtl.wtlCodeArray.length > 0) {
                    clearTimeout(locationValTime);
                    locationValTime = setTimeout(function () {
                        whir.wtl.Service.updateWtl(wtlId, function (wtlObj) {
                            wtlObj.wtlCodeArray[0].locationOps = $.extend(wtlObj.wtlCodeArray[0].locationOps, {
                                Separator: val
                            });
                            return wtlObj;
                        });
                    }, 500);
                }
            });

            $("[name='location_DefaultValue']").unbind().bind("keyup", function () {
                var wtlId = whir.wtl.getSelWtlId();
                var wtl = whir.wtl.Service.getWtlObj(wtlId);
                var val = $(this).val();
                if (wtl != null && wtl.wtlCodeArray != null && wtl.wtlCodeArray.length > 0) {

                    clearTimeout(locationValTime);
                    locationValTime = setTimeout(function () {
                        whir.wtl.Service.updateWtl(wtlId, function (wtlObj) {
                            wtlObj.wtlCodeArray[0].locationOps = $.extend(wtlObj.wtlCodeArray[0].locationOps, {
                                DefaultValue: val
                            });
                            return wtlObj;
                        });
                    }, 500);
                }
            });
        },
        init: function (wtlId) {
            whir.wtlGlobal.getByType("location").optionEvent();
            whir.wtlGlobal.openConfig("location");
        }
    },
    survey: {
        generate: function (wtl, wtlCode, wtlCodeHtmlDom) {
            if (wtlCode != null && wtlCode.surveyOps != null) {
                var ops = wtlCode.surveyOps;
                var tempHtml = "<wtl:Survey ColumnId='" + ops.columnId + "' ></wtl:Survey>";
                wtlCodeHtmlDom.html(tempHtml);
            }
        },
        optionEvent: function () {
            $("[name='survey_column']").unbind().bind("change", function () {
                var wtlId = whir.wtl.getSelWtlId();
                var val = $(this).val();
                whir.wtl.Service.updateWtl(wtlId, function (wtlObj) {
                    if (wtlObj.wtlCodeArray != null && wtlObj.wtlCodeArray.length > 0) {
                        wtlObj.wtlCodeArray[0].surveyOps = $.extend(wtlObj.wtlCodeArray[0].surveyOps, {
                            columnId: val
                        });
                    }

                    return wtlObj;
                });
            });
        },
        init: function (wtlId) {
            whir.wtlGlobal.getByType("survey").optionEvent();
            whir.wtlGlobal.openConfig("survey");
        }
    },
    defaults: {
        optionEvent: function () {
            $(".field_select").unbind().bind("change", function () {

                var fieldId = $(this).attr("name");
                whir.wtl.fieldHelp.updateOption(whir.wtl.getSelWtlId(), fieldId, {
                    columnFieldName: $(this).val()
                });
            });


            var textKeyTime;
            $(".fixed_text").unbind().bind("keyup", function () {
                var thisObj = $(this);

                clearTimeout(textKeyTime);
                textKeyTime = setTimeout(function () {
                    whir.wtl.fieldHelp.updateOption(whir.wtl.getSelWtlId(), thisObj.attr("name"), {
                        fixedTextVal: thisObj.val()
                    });
                }, 500);
            });


            $(".substring_number").unbind().bind("keyup", function () {
                var thisObj = $(this);

                clearTimeout(textKeyTime);
                textKeyTime = setTimeout(function () {
                    whir.wtl.fieldHelp.updateOption(whir.wtl.getSelWtlId(), thisObj.attr("name"), {
                        substringNumber: thisObj.val()
                    });
                }, 500);
            });


            $(".parm_text").unbind().bind("keyup", function () {
                var thisObj = $(this);

                clearTimeout(textKeyTime);
                textKeyTime = setTimeout(function () {
                    whir.wtl.fieldHelp.updateOption(whir.wtl.getSelWtlId(), thisObj.attr("name"), {
                        elseparm: thisObj.val()
                    });
                }, 500);
            });

            $(".fixed_url").unbind().bind("keyup", function () {
                var thisObj = $(this);

                clearTimeout(textKeyTime);
                textKeyTime = setTimeout(function () {
                    whir.wtl.fieldHelp.updateOption(whir.wtl.getSelWtlId(), thisObj.attr("name"), {
                        fixedUrl: thisObj.val()
                    });
                }, 500);
            });


            $(".radio_a_type").unbind().bind("click", function () {
                var thisObj = $(this);
                var name = thisObj.attr("name");
                var type = thisObj.val();

                $("#a_" + name + "_type_field_tr").hide();
                $("#a_" + name + "_type_text_tr").hide();
                $("#a_" + name + "_type_parm_tr").hide();
                $("#radio_a_type_" + name + "_tr td:eq(0)").prop("rowspan", 2);
                switch (type) {
                    case "选择字段":
                        $("#a_" + name + "_type_field_tr").show();
                        break;
                    case "固定地址":
                        $("#a_" + name + "_type_text_tr").show();
                        break;
                    case "系统生成":
                        $("#a_" + name + "_type_parm_tr").show();
                        break;
                    case "首页链接":
                        $("#radio_a_type_" + name + "_tr td:eq(0)").prop("rowspan", 1);
                        break;
                }

                clearTimeout(textKeyTime);
                textKeyTime = setTimeout(function () {
                    whir.wtl.fieldHelp.updateOption(whir.wtl.getSelWtlId(), name, {
                        atype: type
                    });
                }, 500);
            });
        },
        init: function (wtlId) {
            whir.wtl.tabHelp.addWtlTab(wtlId);
            whir.wtlInit.columnFieldSelect($("#select_coulumn").val());
            whir.wtlInit.columnSelect();
            whir.wtl.tabHelp.tabFunction();

            whir.wtlGlobal.getByType("").optionEvent();

            whir.wtlGlobal.openConfig();
        }
    },
    getByType: function (type) {
        if (type == null || type == "") {
            type = "defaults";
        }
        return whir.wtlGlobal[type.replace("-", "")];
    },
    openConfig: function (type) {
        $("#default_ops").hide();
        $("#column_ops").hide();
        $("#pagper_ops").hide();
        $("#form_ops").hide();
        $("#location_ops").hide();
        $("#survey_ops").hide();

        switch (type) {
            case "column":
                $("#column_ops").show();
                $("#tabs_column a:eq(0)").click();
                break;
            case "pager":
                $("#pagper_ops").show();
                $("#tabs_pagper a:eq(0)").click();
                break;
            case "form":
                $("#form_ops").show();
                $("#tabs_form a:eq(0)").click();
                break;
            case "location":
                $("#location_ops").show();
                $("#tabs_location a:eq(0)").click();
                break;
            case "survey":
                $("#survey_ops").show();
                $("#tabs_survey a:eq(0)").click();
                break;
            default:
                $("#default_ops").show();
                $("#tab_div a:eq(0)").click();
                break;
        }

        $(".frame-view .cover").fadeIn();
        $(".config-view").animate({ right: "0px" });
    }
}