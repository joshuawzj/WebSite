var chnNumChar = ["零", "一", "二", "三", "四", "五", "六", "七", "八", "九"];
var chnUnitSection = ["", "万", "亿", "万亿", "亿亿"];
var chnUnitChar = ["", "十", "百", "千"];
var whir = window.whir || {};
whir.wtl = {
    Array: {
        webFormArray: [],
        wtlMenuArray: [],
        wtlArray: []
    },
    Model: {
        //置标块
        Wtl: function () {
            this.id = "";
            this.columnId = 0;
            this.type = "";
            this.isNotBindColumn = false;
            this.wtlCodeArray = new Array();
            this.menuMax = 0;
            this.menuMix = 0;
            this.menuMaxLevel = 2;
            this.isSon = false;
            this.isRequiredSon = false;
        },
        //块代码段
        WtlCode: function () {
            this.id = "";
            this.type = "";
            this.name = "";
            this.htmlDom = null;
            this.fieldArray = new Array();
            this.fieldMatch = null;
            this.level = 0;   //所在层级，生成时从最低处生成
            this.formId = 0;
            this.pagperWtlCodeListId = 0;
            this.isListNeedpage = false;
            this.listTopCount = 0;
            this.locationOps = {
                columnId: 0,
                IsShowColumnNameStage: false,
                DefaultValue: "首页",
                IsCategory: false,
                CategoryParam: "lcid",
                Separator: ">"
            };
            this.surveyOps = {
                columnId: 0
            }

        },
        //块字段
        WtlField: function () {
            this.id = "";
            this.name = "";
            this.type = "";
            this.matchGroup = null;
            this.matchStr = "";

            this.wtlColumnIndex = 0;
            this.elseparm = "";            //其他参数
            this.atype = "系统生成";       //a标签类型
            this.substringNumber = 0;      //截取字符数
            this.fixedUrl = "";
            this.columnFieldName = "";
            this.fixedTextVal = "";
            this.defaultVal = "";
            this.defaultFormat = "";
        },
        //置标菜单
        WtlMenu: function () {
            this.wtlId = 0;
            this.index = 0;
            this.type = "column";
            this.columnId = "";
            this.elseUrl = "";
            this.elseText = "";
            this.isSon = false;
            this.sonColumn = new whir.wtl.Model.WtlMenuSon();
        },
        //置标子菜单
        WtlMenuSon: function () {
            this.type = "column";
            this.columnId = 0;
            this.elseArray = new Array();
        }
    },
    Service: {
        //获取置标菜单
        getWtlMenu: function (index) {
            if (whir.wtl.Array.wtlMenuArray != null) {
                for (var i in whir.wtl.Array.wtlMenuArray) {
                    if (whir.wtl.Array.wtlMenuArray[i].index == index) {
                        return whir.wtl.Array.wtlMenuArray[i];
                    }
                }
            }
            return null;
        },
        //修改置标菜单
        updateWtlMenu: function (index, callback) {
            if (whir.wtl.Array.wtlMenuArray != null) {
                for (var i in whir.wtl.Array.wtlMenuArray) {
                    var wtlColumn = whir.wtl.Array.wtlMenuArray[i];
                    if (wtlColumn.index == index) {
                        whir.wtl.Array.wtlMenuArray[i] = callback(wtlColumn);
                        break;
                    }
                }
            }
        },
        //修改置标子菜单
        updateWtlMenuSon: function (index, callback) {
            whir.wtl.Service.updateWtlMenu(index, function (column) {
                if (column.sonColumn != null) {
                    column.sonColumn = callback(column.sonColumn);
                }
                return column;
            });
        },
        //获取置标块配置实体
        getWtlObj: function (id) {
            if (whir.wtl.Array.wtlArray != null) {
                for (var i in whir.wtl.Array.wtlArray) {
                    if (whir.wtl.Array.wtlArray[i].id == id) {
                        return whir.wtl.Array.wtlArray[i];
                    }
                }
            }
            return null;
        },
        //修置标块配置实体（callback 函数里操作配置实体）
        updateWtl: function (id, callback) {
            if (whir.wtl.Array.wtlArray != null) {
                for (var i in whir.wtl.Array.wtlArray) {
                    if (whir.wtl.Array.wtlArray[i].id == id) {
                        if (callback != undefined) {
                            whir.wtl.Array.wtlArray[i] = callback(whir.wtl.Array.wtlArray[i]);
                            break;
                        }
                    }
                }
            }
        }
    },


    selectWtlId: 0,
    selectWtlFieldName: "",
    SectionToChinese: function (section) {
        var strIns = '', chnStr = '';
        var unitPos = 0;
        var zero = true;
        while (section > 0) {
            var v = section % 10;
            if (v === 0) {
                if (!zero) {
                    zero = true;
                    chnStr = chnNumChar[v] + chnStr;
                }
            } else {
                zero = false;
                strIns = chnNumChar[v];
                strIns += chnUnitChar[unitPos];
                chnStr = strIns + chnStr;
            }
            unitPos++;
            section = Math.floor(section / 10);
        }
        return chnStr;
    },
    NumberToChinese: function (num) {
        var unitPos = 0;
        var strIns = '', chnStr = '';
        var needZero = false;

        if (num === 0) {
            return chnNumChar[0];
        }

        while (num > 0) {
            var section = num % 10000;
            if (needZero) {
                chnStr = chnNumChar[0] + chnStr;
            }
            strIns = whir.wtl.SectionToChinese(section);
            strIns += (section !== 0) ? chnUnitSection[unitPos] : chnUnitSection[0];
            chnStr = strIns + chnStr;
            needZero = (section < 1000) && (section > 0);
            num = Math.floor(num / 10000);
            unitPos++;
        }

        return chnStr;
    },
    wtlFieldRegexStr: "(\\$)(\\{.*?\\})(\\$)",
    //块代码段排序
    wtlCodeSort: function (a, b) {
        return b.level - a.level;
    },
    //获取当前选中置标块Id
    getSelWtlId: function () {
        return whir.wtl.selectWtlId;
    },
    //设置当前选中置标块Id
    setSelWtlId: function (id) {
        whir.wtl.selectWtlId = id;
    },
    //绑定置标已设置值
    bindWtlVal: function (id) {
        var wtlObj = whir.wtl.Service.getWtlObj(id);
        if (wtlObj != null) {

            $("#select_coulumn").val(wtlObj.columnId);
            $("#select_coulumn").change();

            $("#cbNotBindColumn").prop("checked", wtlObj.isNotBindColumn);

            if (wtlObj.wtlCodeArray != undefined) {
                for (var i in wtlObj.wtlCodeArray) {
                    var wtlCode = wtlObj.wtlCodeArray[i];
                    var tableDom = $(".All_table table:eq(" + (i + 1) + ")");

                    for (var f in wtlCode.fieldArray) {
                        var field = wtlCode.fieldArray[f];

                        switch (field.type) {
                            case "固定文本":
                                tableDom.find(".fixed_text[name='" + field.name + "']").val(field.fixedTextVal);
                                break;
                            default:
                                tableDom.find(".field_select[name='" + field.name + "']").val(field.columnFieldName);
                                break;
                        }


                        tableDom.find(".substring_number[name='" + field.name + "']").val(field.substringNumber);
                        tableDom.find(".fixed_url[name='" + field.name + "']").val(field.fixedUrl);
                        tableDom.find(".parm_text[name='" + field.name + "']").val(field.elseparm);
                        tableDom.find(".radio_a_type[name='" + field.name + "'][value='" + field.atype + "']").prop("checked", true);
                    }

                }
            }
        }
    },
    //绑定置标块事件
    wtlEditClickEvent: function (iframeObj) {
        iframeObj.find("[wtl-edit]").click(function () {
            $("#select_coulumn").val("0");
            var id = $(this).attr("wtl-id");
            whir.wtl.setSelWtlId(id);

            var wtl = whir.wtl.Service.getWtlObj(id);
            whir.wtlGlobal.getByType(wtl.type).init(wtl.id);
        });

        iframeObj.find("[wtl-edit]").find("*").click(function (e) {
            e.preventDefault();
        });
    },
    //加载置标对象数据
    initWtl: function (iframeObj) {

        whir.wtl.Array.wtlArray = [];
        iframeObj.find("[wtl-edit]").each(function () {
            $(this).attr("wtl-id", whir.wtl.Array.wtlArray.length);

            var wtl = new whir.wtl.Model.Wtl();
            wtl.id = whir.wtl.Array.wtlArray.length;

            $(this).find("script[type^='html/']").each(function () {
                var codeType = wtl.type = $(this).attr("type").replace("html/", "");
                var level = $(this).parents().length;
                var menuMax = 0;
                var menuMin = 0;


                var wtlCode = new whir.wtl.Model.WtlCode();

                var codeHtml = $(this).html();
                var name = $(this).attr("name");
                if (name == undefined || name == "") {
                    switch (codeType) {
                        case "list":
                            name = "列表";
                            break;
                        case "content":
                            name = "内容";
                            break;
                        case "list-top":
                            name = "头条列表";
                            break;
                        case "menu":
                            name = "栏目菜单";
                            menuMax = $(this).attr("max");
                            menuMin = $(this).attr("min");

                            wtl.menuMax = (menuMax == undefined ? 0 : menuMax);
                            wtl.menuMix = (menuMin == undefined ? 0 : menuMin);
                            wtl.isSon = $(this).attr("son") == "true";
                            wtl.isRequiredSon = $(this).attr("sonreq") == "true";
                            wtl.menuMaxLevel = $(this).attr("maxlevel");
                            break;
                        case "pager":
                            name = "分页";
                            break;
                        case "form":
                            name = "提交表单";
                            break;
                        case "location":
                            name = "当前位置";
                            break;
                        case "survey":
                            name = "问卷调查";
                            break;
                    }
                }




                wtlCode.id = "code_" + whir.wtl.Array.wtlArray.length + "_" + wtl.wtlCodeArray.length;
                wtlCode.type = codeType;
                wtlCode.name = name;
                wtlCode.level = level;
                wtlCode.htmlDom = $(this);

                wtlCode.fieldMatch = whir.wtl.fieldHelp.getMatch(codeHtml);
                wtlCode.fieldArray = whir.wtl.fieldHelp.getArrayByMatch(wtlCode.fieldMatch);
                wtl.wtlCodeArray.push(wtlCode);

                $(this).attr("id", wtlCode.id);
            });
            whir.wtl.Array.wtlArray.push(wtl);
        });
        console.log(whir.wtl.Array.wtlArray);
    }
}


