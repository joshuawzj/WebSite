$(document)
    .ready(function () {
    	//字段类型下拉事件
    	$("#ShowType")
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



    	$("#ValidateType")
        .change(function () {
        	ValidateTypeChange($(this).val());
        });
    });
function ShowPlaceHolderBySelectedType() {
	var selectedType = $("select[name=ShowType]").find("option:selected").val();
	if (selectedType == "2" || selectedType == "3")
		$("[name=divRepeatColumn]").show();
	else
		$("[name=divRepeatColumn]").hide();
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
	}
	if (bindType == "3") {
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
		case "1":
			validateExpression = "^\\w+([-+.]\\w+)*@\\w+([-.]\\\\w+)*\\.\\w+([-.]\\w+)*$";
			break;
		case "2":
            validateExpression = "^0?(13[0-9]|14[5-9]|15[012356789]|166|17[0-8]|18[0-9]|19[8-9])[0-9]{8}$";
			break;
		case "3":
			validateExpression = "^([0-9]{4}-[0-9]{8})|([0-9]{3}-[0-9]{8})|([0-9]{4}-[0-9]{7})$";
			break;
		case "4":
			validateExpression = "(\\d{6})(\\d{4})(\\d{2})(\\d{2})(\\d{3})([0-9]|X)";
			break;
		case "5":
			validateExpression = "^\\d+(\\.\\d+)?$";
			break;
		case "6":
			validateExpression = "^\\+?[0-9][0-9]{0,9}$";
			break;
		case "7":
			validateExpression = "^[-\\+]?\\d+$";
			break;
		case "8":
			validateExpression = "^[\\+]?\\d+$";
			break;
		case "9":
			validateExpression =
                "^(?:(?!0000)[0-9]{4}([-/.]?)(?:(?:0?[1-9]|1[0-2])([-/.]?)(?:0?[1-9]|1[0-9]|2[0-8])|(?:0?[13-9]|1[0-2])([-/.]?)(?:29|30)|(?:0?[13578]|1[02])([-/.]?)31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)([-/.]?)0?2([-/.]?)29)$";
			break;
		case "10":
			validateExpression =
                "^(?:(?!0000)[0-9]{4}-(?:(?:0[1-9]|1[0-2])-(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])-(?:29|30)|(?:0[13578]|1[02])-31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)-02-29)\\s+([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$";
			break;
		case "11":
			validateExpression = "^[\\u4E00-\\u9FA5]+$";
			break;
		case "12":
			validateExpression = "^[a-zA-Z]+$";
			break;
		case "13":
			validateExpression = "^((https|http|ftp)?://)";
			break;
		case "14":
			validateExpression =
                '([A-Za-z0-9]|(-|_|\~|\!|\@|\#|\$|\%|\^|\&|\*|\.|\(|\)|\[|\]|\\|\{|\}|\<|\>|\?|\/|\""|\.))*';
			break;
		case "15":
			validateExpression = "";
			break;
	}
	$("#ValidateExpression").val(validateExpression);
}
//根据表单输入项的表现形式, 显示不同的输入框
function ShowPlaceHolderByFieldType(fieldType) {
	switch (fieldType.toString()) {
		case "1":
			//单行文本
			$("[name=divBindText]").hide();
			$("[name=divBindSQL]").hide();
			$("[name=divBindLevel]").hide();
			$("[name=divField]").hide();
			$("[name=divRepeatColumn]").hide();
			$("[name='divBindType']").hide();
			break;
		case "2":
			// 单选
			$("[name='divBindType']").show();
			$("[name=divBindText]").show();
			$("[name=divRepeatColumn]").show();
			break;
		case "3":
			// 多选
			$("[name='divBindType']").show();
			$("[name=divBindText]").show();
			$("[name=divRepeatColumn]").show();
			break;
		case "4":
			// 多行文本
			$("[name=divBindText]").hide();
			$("[name=divBindSQL]").hide();
			$("[name=divBindLevel]").hide();
			$("[name=divField]").hide();
			$("[name=divRepeatColumn]").hide();
			$("[name='divBindType']").hide();
			break;
		case "5":
			// 下拉框
			$("[name='divBindType']").show();
			$("[name=divBindText]").show();
			break;
		case "6":
			// HTML编辑器
			$("[name=divBindText]").hide();
			$("[name=divBindSQL]").hide();
			$("[name=divBindLevel]").hide();
			$("[name=divField]").hide();
			$("[name=divRepeatColumn]").hide()
			$("[name='divBindType']").hide();
			break;
		case "7":
			//图片上传
			$("[name=divBindText]").hide();
			$("[name=divBindSQL]").hide();
			$("[name=divBindLevel]").hide();
			$("[name=divField]").hide();
			$("[name=divRepeatColumn]").hide()
			$("[name='divBindType']").hide();
			break;
	}
}
