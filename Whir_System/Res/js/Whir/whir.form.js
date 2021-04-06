(function ($) {

    $.fn.Validator = function (options, submitfn, isOnlyValid) {
        var form = this;
        var op = {
            container: 'popover', //验证提示方式 popover tooltip 或空
           // group: '.ValidatorCss',// 用于验证不通过时，定位提示不通过原因的位置
            submitButtons: '[form-action="submit"]',
            excluded: '',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            }
        };
        op = $.extend(true, op, options);

        //点击事件
        form.find(op.submitButtons)
            .click(function(e) {
                e.preventDefault();
                form.bootstrapValidator('validate');
                var btn = $(this);
                var times = 500;
            if (!isOnlyValid)
                times = 0;
                setTimeout(function() {
                    if (form.data('bootstrapValidator').isValid()) {
                        try {
                            $(document).click(); //在提交表单之前，触发页面的点击事件，来获取每个编辑器的值到隐藏控件
                        } catch (e) {
                        }
                        submitfn(btn);
                        $(op.submitButtons).attr("disabled", false);
                    }
                }, times);  //为了等待异步验证
                return false;
            });
        //表单验证配置
        this.bootstrapValidator(op);
    }

    $.fn.post = function (options) {
        var requestUrl = this.attr("form-url");
        var requestData;
        if (options.data) {
            requestData = options.data;
        } else {
            requestData = this.whirSerialize();
            //添加操作命令
            if (requestData && this.attr("commandname")) {
                requestData += "&commandname=" + this.attr("commandname");
            }
        }
        var requestMethod = this.attr("form-method");
        if (options && options.url) {
            options.url = options.url + "?time=" + new Date().getMilliseconds();
        }
        whir.loading.show();

        options = $.extend({
            url: requestUrl + "?time=" + new Date().getMilliseconds(),
            data: requestData,
            dataType: 'json',
            type: requestMethod || 'POST',
            success: options.success ||
                    function (response) {
                        alert(eval(response));
                        whir.loading.remove();
                    },
            error: options.error ||
                    function (response) {
                        alert(eval(response));
                        whir.loading.remove();
                    }
        },
            options || {});
        $.ajax(options);

    };
})(jQuery);

//校验必填项的方法
function formRequiredValidator(fieldId, pass,formId) {
    try {
        if (formId == undefined) {
            formId = $('#' + fieldId).parents("form").attr("Id");
            if (formId == undefined)
                formId = "formEdit";
        }
        var bootstrapValidator = $('#' + formId).data('bootstrapValidator');
        if (bootstrapValidator == undefined)
            bootstrapValidator = window.parent.$('#' + formId).data('bootstrapValidator');
        if (pass)
            bootstrapValidator.updateStatus(fieldId, 'VALID');
        else
            bootstrapValidator.updateStatus(fieldId, 'NOT_VALIDATED').validateField(fieldId);

    }
    catch (e)
    { }
}

//为jquery.serializeArray()解决radio,checkbox未选中时没有序列化的问题
$.fn.whirSerialize = function () {
    var a = this.serializeArray();
    var $radio = $('input[type=radio],input[type=checkbox]', this);
    var temp = {};
    $.each($radio, function () {
        if (!temp.hasOwnProperty(this.name)) {
            if ($("input[name='" + this.name + "']:checked").length == 0) {
                temp[this.name] = "";
                a.push({ name: this.name, value: "" });
            }
        }
    });
    return jQuery.param(a);
};

 