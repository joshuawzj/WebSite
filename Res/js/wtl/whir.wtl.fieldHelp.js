var whir = window.whir || { wtl: {} };
whir.wtl = whir.wtl || {};
whir.wtl.fieldHelp = {
    fieldTotalCount: 0,
    //修改字段配置
    updateOption: function (wtlid, fieldId, fieldOps) {
        whir.wtl.Service.updateWtl(wtlid, function (wtlObj) {
            if (wtlObj != null) {
                for (var i in wtlObj.wtlCodeArray) {
                    var wtlFieldArray = wtlObj.wtlCodeArray[i].fieldArray;

                    for (var y in wtlFieldArray) {
                        if (wtlFieldArray[y] != undefined && wtlFieldArray[y].id == Number(fieldId)) {
                            wtlObj.wtlCodeArray[i].fieldArray[y] = $.extend(wtlFieldArray[y], fieldOps);
                            return wtlObj;
                        }
                    }
                }
            }
        });

    },
    getMatchGroup: function (fieldMacthStr) {
        return new RegExp(whir.wtl.wtlFieldRegexStr, "i").exec(fieldMacthStr);
    },
    getMatch: function (scriptCodeHtml) {
        var fieldRegex = new RegExp(whir.wtl.wtlFieldRegexStr, "gi");
        return scriptCodeHtml.match(fieldRegex);
    },
    getArrayByMatch: function (codeFieldMatch) {
        var verifyObj = {};
        var tempFieldArray = new Array();

        if (codeFieldMatch != null) {
            for (var fieldMatchIndex in codeFieldMatch) {

                var tempGroup = whir.wtl.fieldHelp.getMatchGroup(codeFieldMatch[fieldMatchIndex]);

                if (tempGroup.length == 4 && tempGroup[2] != "") {
                    var tempFieldObj = JSON.parse(tempGroup[2].replace(/'/g, "\""));

                    var isAdd = true;

                    if (verifyObj[tempFieldObj.fileName] != undefined && verifyObj[tempFieldObj.fileName] == tempFieldObj.fileType) {
                        isAdd = false;
                    }

                    if (isAdd) {
                        var wtlField = new whir.wtl.Model.WtlField();
                        wtlField.id = whir.wtl.fieldHelp.fieldTotalCount;
                        wtlField.matchGroup = tempGroup;
                        wtlField.matchStr = codeFieldMatch[fieldMatchIndex];
                        wtlField.name = tempFieldObj.fileName;
                        wtlField.type = tempFieldObj.fileType;
                        wtlField.defaultVal = tempFieldObj.defaultVal;
                        wtlField.defaultFormat = tempFieldObj.defaultFormat;

                        tempFieldArray.push(wtlField);
                        verifyObj[tempFieldObj.fileName] = tempFieldObj.fileType;
                        whir.wtl.fieldHelp.fieldTotalCount++;
                    }
                }
            }
        }

        return tempFieldArray;
    }

}