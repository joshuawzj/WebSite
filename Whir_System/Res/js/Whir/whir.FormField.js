$(document)
    .ready(function () {
        //字段类型下拉事件
        $("#FieldType")
            .change(function () {
                ShowPlaceHolderByFieldType($(this).val());
            });
        //验证类型改变事件
        $("#ValidateType")
        .change(function () {
            ValidateTypeChange($(this).val());
        });
        //根据绑定类型, 显示不同的绑定选项输入项
        $("input[name='BindType']")
             .next()
             .click(function () {
                 ShowPlaceHolderByBindType();

             });
        $("input[name='BindType']")
        .parent()
        .next()
        .click(function () {
            ShowPlaceHolderByBindType();

        });
        //显示选项改变事件
        $("input[name='SelectedType']")
        .next()
        .click(function () {
            ShowPlaceHolderBySelectedType();

        });
        $("input[name='SelectedType']")
        .parent()
        .next()
        .click(function () {
            ShowPlaceHolderBySelectedType();

        });

        //显示选项改变事件
        $("input[name='DateDefaultValue']")
        .next()
        .click(function () {
            ShowDateDefaultValue();

        });
        $("input[name='DateDefaultValue']")
        .parent()
        .next()
        .click(function () {
            ShowDateDefaultValue();

        });



        $("#ValidateType")
        .change(function () {
            ValidateTypeChange($(this).val());
        });
    });

//赋值
function SetFieldValue(field) {
    if (field) {
        ShowPlaceHolderByFieldType(field.FieldType);
        $("#FieldType").val(field.FieldType);
        $("#FieldType").attr("disabled", true);
        $("#FieldName").val(field.FieldName);
        $("#FieldName").attr("readonly", true);

        $("#FieldAlias").val(field.FieldAlias);

        $("#txtDefaultValue").val(field.DefaultValue);
        $("#ValidateType").val(field.ValidateType);
        $("#ValidateExpression").val(field.ValidateExpression);

        //系统字段除了字段名称之外的信息都不可修改
        $("#IsAllowNull").attr("readonly", !field.IsSystemField);
        $("#txtDefaultValue").attr("readonly", !field.IsSystemField);
        $("#IsColor").attr("readonly", !field.IsSystemField);
        $("#IsBold").attr("readonly", !field.IsSystemField);
        $("#IsLengthCalc").attr("readonly", !field.IsSystemField);
        if (field.FormId == 0||field.FormId==undefined)
            $("#FieldId").val(field.FieldId);
        $("#divLinkAttr").removeClass("disabled");
        
    }
}

//时间格式按钮组事件
function ShowDateDefaultValue() {
    var formatStr = $("[name=DateFormat]:checked").val();
    var strl = "1900-01-01 00:00:00", strr = "1900-01-01";
    if ($("[name=DateDefaultValue]:checked").val() == "3") {
        $("#txtDateDefaultValue").show();
        $("#txtDateDefaultValue").val(formatStr == "yyyy-MM-dd" ? strr : strl);
    }
    else {
        $("#txtDateDefaultValue").hide();
    }
}
function ShowPlaceHolderBySelectedType() {
    var selectedType = $("input[name=SelectedType]:checked").val();

    if (selectedType == "1" || selectedType == "5" || selectedType == "6")
        $("[name=divRepeatColumn]").hide();
    else
        $("[name=divRepeatColumn]").show();
}

//根据绑定类型, 显示不同的绑定选项输入项
function ShowPlaceHolderByBindType() {
    var bindType = $("input[name=BindType]:checked").val();
    if (bindType == "1") {
        $("[name=divBindText]").show();
        $("[name=divBindSQL]").hide();
        $("[name=divField]").hide();
        $("[name=divBindLevel]").hide();
    }
    else if (bindType == "2") {
        $("[name=divBindSQL]").show();
        $("[name=divField]").show();
        $("[name=divBindText]").hide();
        $("[name=divBindLevel]").hide();
    } else if (bindType == "3") {
        $("[name=divBindLevel]").show();
        $("[name=divField]").show();
        $("[name=divBindText]").hide();
        $("[name=divBindSQL]").hide();
    } else {
        $("[name=divBindLevel]").hide();
        $("[name=divField]").hide();
        $("[name=divBindText]").hide();
        $("[name=divBindSQL]").hide();
    }
    if (bindType == "3" || bindType == "4") {
        if (($("[name=SelectedType]:checked").val() == "2" || $("[name=SelectedType]:checked").val() == "3")) {
            $("[name=SelectedType][value=1]").iCheck('check');
        }
        $("[name=SelectedType][value=2]").iCheck('disable');
        $("[name=SelectedType][value=3]").iCheck('disable');

    } else {
        $("[name=SelectedType][value=2]").iCheck('enable');
        $("[name=SelectedType][value=3]").iCheck('enable');

    }
    ShowPlaceHolderBySelectedType();
}

//验证类型改变事件
function ValidateTypeChange(validateType) {
    var validateExpression = "";
    switch (validateType) {
        case "电子邮箱":
            validateExpression = "^\\w+([-+.]\\w+)*@\\w+([-.]\\\\w+)*\\.\\w+([-.]\\w+)*$";
            break;
        case "手机号码":
            validateExpression = "^0?(13[0-9]|14[5-9]|15[012356789]|166|17[0-8]|18[0-9]|19[8-9])[0-9]{8}$";
            break;
        case "电话号码":
            validateExpression = "^([0-9]{4}-[0-9]{8})|([0-9]{3}-[0-9]{8})|([0-9]{4}-[0-9]{7})$";
            break;
        case "手机号码/电话号码":
            validateExpression = "(^([0-9]{4}-[0-9]{8})|([0-9]{3}-[0-9]{8})|([0-9]{4}-[0-9]{7})$)|(^(1[3|4|5|7|8])(\\d){9}$)";
            break;
        case "QQ":
            validateExpression = "^\\d{5,10}$";
            break;
        case "身份证号码":
            validateExpression = "(\\d{6})(\\d{4})(\\d{2})(\\d{2})(\\d{3})([0-9]|X)";
            break;
        case "金额":
            validateExpression = "^\\d+(\\.\\d+)?$";
            break;
        case "数字":
            validateExpression = "^\\+?[0-9][0-9]{0,9}$";
            break;
        case "整数":
            validateExpression = "^[-\\+]?\\d+$";
            break;
        case "正整数":
            validateExpression = "^[1-9]\\d*$";
            break;
        case "负整数":
            validateExpression = "^-[1-9]\\d*$";
            break;
        case "非负整数":
            validateExpression = "^[\\+]?\\d+$";
            break;
        case "日期":
            validateExpression =
                "^(?:(?!0000)[0-9]{4}([-/.]?)(?:(?:0?[1-9]|1[0-2])([-/.]?)(?:0?[1-9]|1[0-9]|2[0-8])|(?:0?[13-9]|1[0-2])([-/.]?)(?:29|30)|(?:0?[13578]|1[02])([-/.]?)31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)([-/.]?)0?2([-/.]?)29)$";
            break;
        case "日期时间":
            validateExpression =
                "^(?:(?!0000)[0-9]{4}-(?:(?:0[1-9]|1[0-2])-(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])-(?:29|30)|(?:0[13578]|1[02])-31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)-02-29)\\s+([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$";
            break;
        case "中文":
            validateExpression = "^[\\u4E00-\\u9FA5]+$";
            break;
        case "英文":
            validateExpression = "^[a-zA-Z]+$";
            break;
        case "Url地址":
            validateExpression = "^((https|http|ftp)?://)";
            break;
        case "安全字符":
            validateExpression =
                '([A-Za-z0-9]|(-|_|\\~|\\!|\\@|\\#|\\$|\\%|\\^|\\&|\\*|\\.|\\(|\\)|\\[|\\]|\\\\|\\{|\\}|\\<|\\>|\\?|\\/|\\&quot;&quot;|\\.))*';
            break;
        case "长度":
            validateExpression = ".{0,250}";
            break;
        case "邮编":
            validateExpression = "[1-9]\\d{5}(?!\\d)";
            break;
        case "IP地址":
            validateExpression = "((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)";
            break;
        case "自定义表达式":
            validateExpression = "";
            break;
    }
    $("#ValidateExpression").val(validateExpression);
}

//根据表单输入项的表现形式, 显示不同的输入框
function ShowPlaceHolderByFieldType(fieldType) {
    if (fieldType.toString() == "1") {
        $("[name=IsLengthCalc][value=1]").iCheck('check');
    } else {
        $("[name=IsLengthCalc][value=0]").iCheck('check');
    }
     $("[name=divIsOpenCss]").hide();
     $("[name=divCssPath]").hide();
    switch (fieldType.toString()) {
        case "1":
            //单行文本
            $("[name=divIsAllowNull]").show();
            $("[name=divIsOnly]").show();
            $("[name=divIsReadOnly]").show();
            $("[name=divDefaultValue]").show();
            $("[name=divMaxLength]").show();
            $("[name=divIsColor]").show();
            $("[name=divIsBold]").show();
            $("[name=divLengthCalc]").show();
            $("[name=divValidateText]").show();
            $("[name=divValidateType]").show();
            $("[name=divValidateExpression]").show();
            $("[name=divWidth]").show();
            $("[name=divHeight]").hide();
            $("[name=divBindType]").hide();
            $("[name=divBindText]").hide();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").hide();
            $("[name=divDateFormat]").hide();
            $("[name=divBoolDefaultValue]").hide();
            $("[name=divImageUploadMode]").hide();
            $("[name=divFileUploadMode]").hide();
            $("[name=divArea]").hide();
            $("[name=divFileExts]").hide();
            $("[name=divContentPagingParam]").hide();
            break;
        case "2":
            // 多行文本
            $("[name=divIsAllowNull]").show();
            $("[name=divIsOnly]").show();
            $("[name=divIsReadOnly]").show();
            $("[name=divDefaultValue]").show();
            $("[name=divMaxLength]").show();
            $("[name=divIsColor]").hide();
            $("[name=divIsBold]").hide();
            $("[name=divLengthCalc]").show();
            $("[name=divValidateText]").hide();
            $("[name=divValidateType]").hide();
            $("[name=divValidateExpression]").hide();
            $("[name=divWidth]").show();
            $("[name=divHeight]").show();
            $("[name=divBindType]").hide();
            $("[name=divBindText]").hide();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").hide();
            $("[name=divDateFormat]").hide();
            $("[name=divBoolDefaultValue]").hide();
            $("[name=divImageUploadMode]").hide();
            $("[name=divFileUploadMode]").hide();
            $("[name=divArea]").hide();
            $("[name=divFileExts]").hide();
            $("[name=divContentPagingParam]").hide();
            break;
        case "3":
            // HTML
            $("[name=divIsAllowNull]").hide();
            $("[name=divIsOnly]").hide();
            $("[name=divIsReadOnly]").hide();
            $("[name=divDefaultValue]").show();
            $("[name=divMaxLength]").hide();
            $("[name=divIsColor]").hide();
            $("[name=divIsBold]").hide();
            $("[name=divLengthCalc]").hide();
            $("[name=divTipText]").hide();
            $("[name=divValidateText]").hide();
            $("[name=divValidateType]").hide();
            $("[name=divValidateExpression]").hide();
            $("[name=divWidth]").val(700).show();
            $("[name=divHeight]").val(350).show();
            $("[name=divBindType]").hide();
            $("[name=divBindText]").hide();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").hide();
            $("[name=divDateFormat]").hide();
            $("[name=divBoolDefaultValue]").hide();
            $("[name=divImageUploadMode]").hide();
            $("[name=divFileUploadMode]").hide();
            $("[name=divArea]").hide();
            $("[name=divFileExts]").hide();
            $("[name=divContentPagingParam]").show();
            $("[name=divIsOpenCss]").show();
            $("[name=divCssPath]").show();
            break;
        case "4":
            // 选项
            $("[name=divIsAllowNull]").show();
            $("[name=divIsOnly]").hide();
            $("[name=divIsReadOnly]").show();
            $("[name=divDefaultValue]").show();
            $("[name=divMaxLength]").hide();
            $("[name=divIsColor]").hide();
            $("[name=divIsBold]").hide();
            $("[name=divLengthCalc]").hide();
            $("[name=divValidateText]").hide();
            $("[name=divValidateType]").hide();
            $("[name=divValidateExpression]").hide();
            $("[name=divWidth]").hide();
            $("[name=divHeight]").hide();
            $("[name=divBindType]").show();
            $("[name=divBindText]").show();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").show();
            $("[name=divDateFormat]").hide();
            $("[name=divBoolDefaultValue]").hide();
            $("[name=divImageUploadMode]").hide();
            $("[name=divFileUploadMode]").hide();
            $("[name=divArea]").hide();
            $("[name=divFileExts]").hide();
            $("[name=divContentPagingParam]").hide();
            break;
        case "5":
            // 数字
            $("[name=divIsAllowNull]").show();
            $("[name=divIsOnly]").hide();
            $("[name=divIsReadOnly]").show();
            $("[name=divDefaultValue]").show();
            $("[name=divMaxLength]").show();
            $("[name=divIsColor]").hide();
            $("[name=divIsBold]").hide();
            $("[name=divLengthCalc]").hide();
            $("[name=divValidateText]").show();
            $("[name=divValidateType]").show();
            $("[name=divValidateExpression]").show();
            $("[name=divWidth]").show();
            $("[name=divHeight]").hide();
            $("[name=divBindType]").hide();
            $("[name=divBindText]").hide();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").hide();
            $("[name=divDateFormat]").hide();
            $("[name=divBoolDefaultValue]").hide();
            $("[name=divImageUploadMode]").hide();
            $("[name=divFileUploadMode]").hide();
            $("[name=divArea]").hide();
            $("[name=divFileExts]").hide();
            $("[name=divContentPagingParam]").hide();
            break;
        case "6":
            // 货币
            $("[name=divIsAllowNull]").show();
            $("[name=divIsOnly]").hide();
            $("[name=divIsReadOnly]").show();
            $("[name=divDefaultValue]").show();
            $("[name=divMaxLength]").show();
            $("[name=divIsColor]").hide();
            $("[name=divIsBold]").hide();
            $("[name=divLengthCalc]").hide();
            $("[name=divValidateText]").show();
            $("[name=divValidateType]").show();
            $("[name=divValidateExpression]").show();
            $("[name=divWidth]").show();
            $("[name=divHeight]").hide();
            $("[name=divBindType]").hide();
            $("[name=divBindText]").hide();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").hide();
            $("[name=divDateFormat]").hide();
            $("[name=divBoolDefaultValue]").hide();
            $("[name=divImageUploadMode]").hide();
            $("[name=divFileUploadMode]").hide();
            $("[name=divArea]").hide();
            $("[name=divFileExts]").hide();
            $("[name=divContentPagingParam]").hide();
            break;
        case "7":
            //日期和时间
            $("[name=divIsAllowNull]").show();
            $("[name=divIsOnly]").hide();
            $("[name=divIsReadOnly]").show();
            $("[name=divDefaultValue]").hide();
            $("[name=divMaxLength]").hide();
            $("[name=divIsColor]").hide();
            $("[name=divIsBold]").hide();
            $("[name=divLengthCalc]").hide();
            $("[name=divValidateText]").hide();
            $("[name=divValidateType]").hide();
            $("[name=divValidateExpression]").hide();
            $("[name=divWidth]").hide();
            $("[name=divHeight]").hide();
            $("[name=divBindType]").hide();
            $("[name=divBindText]").hide();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").hide();
            $("[name=divDateFormat]").show();
            $("[name=divBoolDefaultValue]").hide();
            $("[name=divImageUploadMode]").hide();
            $("[name=divFileUploadMode]").hide();
            $("[name=divArea]").hide();
            $("[name=divFileExts]").hide();
            $("[name=divContentPagingParam]").hide();
            break;
        case "8":
            // 超链接
            $("[name=divIsAllowNull]").show();
            $("[name=divIsOnly]").hide();
            $("[name=divIsReadOnly]").hide();
            $("[name=divDefaultValue]").show();
            $("[name=divMaxLength]").show();
            $("[name=divIsColor]").hide();
            $("[name=divIsBold]").hide();
            $("[name=divLengthCalc]").hide();
            $("[name=divValidateText]").hide();
            $("[name=divValidateType]").hide();
            $("[name=divValidateExpression]").hide();
            $("[name=divWidth]").show();
            $("[name=divHeight]").hide();
            $("[name=divBindType]").hide();
            $("[name=divBindText]").hide();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").hide();
            $("[name=divDateFormat]").hide();
            $("[name=divBoolDefaultValue]").hide();
            $("[name=divImageUploadMode]").hide();
            $("[name=divFileUploadMode]").hide();
            $("[name=divArea]").hide();
            $("[name=divFileExts]").hide();
            $("[name=divContentPagingParam]").hide();
            break;
        case "9":
            // 是/否
            $("[name=divIsAllowNull]").hide();
            $("[name=divIsOnly]").hide();
            $("[name=divIsReadOnly]").hide();
            $("[name=divDefaultValue]").hide();
            $("[name=divMaxLength]").hide();
            $("[name=divIsColor]").hide();
            $("[name=divIsBold]").hide();
            $("[name=divLengthCalc]").hide();
            $("[name=divValidateText]").hide();
            $("[name=divValidateType]").hide();
            $("[name=divValidateExpression]").hide();
            $("[name=divWidth]").hide();
            $("[name=divHeight]").hide();
            $("[name=divBindType]").hide();
            $("[name=divBindText]").hide();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").hide();
            $("[name=divDateFormat]").hide();
            $("[name=divBoolDefaultValue]").show();
            $("[name=divImageUploadMode]").hide();
            $("[name=divFileUploadMode]").hide();
            $("[name=divArea]").hide();
            $("[name=divFileExts]").hide();
            $("[name=divContentPagingParam]").hide();
            break;
        case "10":
            // 图片
            $("[name=divIsAllowNull]").show();
            $("[name=divIsOnly]").hide();
            $("[name=divIsReadOnly]").hide();
            $("[name=divDefaultValue]").hide();
            $("[name=divMaxLength]").hide();
            $("[name=divIsColor]").hide();
            $("[name=divIsBold]").hide();
            $("[name=divLengthCalc]").hide();
            $("[name=divValidateText]").hide();
            $("[name=divValidateType]").hide();
            $("[name=divValidateExpression]").hide();
            $("[name=divWidth]").show();
            $("[name=divHeight]").show();
            $("[name=divBindType]").hide();
            $("[name=divBindText]").hide();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").hide();
            $("[name=divDateFormat]").hide();
            $("[name=divBoolDefaultValue]").hide();
            $("[name=divImageUploadMode]").show();
            $("[name=divFileUploadMode]").hide();
            $("[name=divArea]").hide();
            $("[name=divFileExts]").show();
            $("#FileExts").val(_AllowPicType);
            $("[name=divContentPagingParam]").hide();
            break;
        case "11":
            // 文件
            $("[name=divIsAllowNull]").show();
            $("[name=divIsOnly]").hide();
            $("[name=divIsReadOnly]").hide();
            $("[name=divDefaultValue]").hide();
            $("[name=divMaxLength]").hide();
            $("[name=divIsColor]").hide();
            $("[name=divIsBold]").hide();
            $("[name=divLengthCalc]").hide();
            $("[name=divValidateText]").hide();
            $("[name=divValidateType]").hide();
            $("[name=divValidateExpression]").hide();
            $("[name=divWidth]").hide();
            $("[name=divHeight]").hide();
            $("[name=divBindType]").hide();
            $("[name=divBindText]").hide();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").hide();
            $("[name=divDateFormat]").hide();
            $("[name=divBoolDefaultValue]").hide();
            $("[name=divImageUploadMode]").hide();
            $("[name=divFileUploadMode]").show();
            $("[name=divFileExts]").show();
            $("[name=divArea]").hide();
            $("#FileExts").val(_AllowFileType);
            $("[name=divContentPagingParam]").hide();
            break;
        case "13":
            // 地区
            $("[name=divIsAllowNull]").show();
            $("[name=divIsOnly]").hide();
            $("[name=divIsReadOnly]").show();
            $("[name=divDefaultValue]").hide();
            $("[name=divMaxLength]").hide();
            $("[name=divIsColor]").hide();
            $("[name=divIsBold]").hide();
            $("[name=divLengthCalc]").hide();
            $("[name=divValidateText]").hide();
            $("[name=divValidateType]").hide();
            $("[name=divValidateExpression]").hide();
            $("[name=divWidth]").hide();
            $("[name=divHeight]").hide();
            $("[name=divBindType]").hide();
            $("[name=divBindText]").hide();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").hide();
            $("[name=divDateFormat]").hide();
            $("[name=divBoolDefaultValue]").hide();
            $("[name=divImageUploadMode]").hide();
            $("[name=divFileUploadMode]").hide();
            $("[name=divArea]").show();
            $("[name=divFileExts]").hide();
            $("[name=divContentPagingParam]").hide();
            break;
        case "14":
            // 密码型文本
            $("[name=divIsAllowNull]").show();
            $("[name=divIsOnly]").hide();
            $("[name=divIsReadOnly]").show();
            $("[name=divDefaultValue]").hide();
            $("[name=divMaxLength]").show();
            $("[name=divIsColor]").hide();
            $("[name=divIsBold]").hide();
            $("[name=divLengthCalc]").hide();
            $("[name=divValidateText]").show();
            $("[name=divValidateType]").show();
            $("[name=divValidateExpression]").show();
            $("[name=divWidth]").show();
            $("[name=divHeight]").hide();
            $("[name=divBindType]").hide();
            $("[name=divBindText]").hide();
            $("[name=divBindSQL]").hide();
            $("[name=divBindLevel]").hide();
            $("[name=divBindSelectedType]").hide();
            $("[name=divDateFormat]").hide();
            $("[name=divBoolDefaultValue]").hide();
            $("[name=divImageUploadMode]").hide();
            $("[name=divFileUploadMode]").hide();
            $("[name=divArea]").hide();
            $("[name=divFileExts]").hide();
            $("[name=divContentPagingParam]").hide();
            break;
    }
    var bootstrapValidator = $("#formEdit").data("bootstrapValidator");
    if (bootstrapValidator) {
        if (fieldType.toString() == "10" || fieldType.toString() == "11") {
            //必填
            bootstrapValidator.addField("TipText", { validators: { notEmpty: {} } });
            if (fieldType.toString() == "10") {
                bootstrapValidator.addField("Width", { validators: { notEmpty: {} } });
                bootstrapValidator.addField("Height", { validators: { notEmpty: {} } });
            } else if (fieldType.toString() == "11") {
                bootstrapValidator.enableFieldValidators("Width", false, "notEmpty");
                bootstrapValidator.enableFieldValidators("Height", false, "notEmpty");
                }
        }
        else {
            bootstrapValidator.enableFieldValidators("TipText", false, "notEmpty");
            bootstrapValidator.enableFieldValidators("Width", false, "notEmpty");
            bootstrapValidator.enableFieldValidators("Height", false, "notEmpty");
        }
        bootstrapValidator.resetForm(false);
    }
}