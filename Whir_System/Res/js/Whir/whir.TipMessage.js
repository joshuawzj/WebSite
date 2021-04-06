
function TipMessage(text, callback) {
    whir.toastr.info(text);
    if (callback) {
        callback();
    }
}


$(function () {
    confirmWarn();
});
//确认删除
function confirmWarn() {

    $("a[confirmId][confirmText][confirmText!=''][confirmId!=''][href!='javascript:;']").each(function (idx, item) {
        bindConfirmAction($(item));
    });

    function bindConfirmAction(jqObj) {
        var confirmId = jqObj.attr("confirmId") || jqObj.attr("confirmid");
        var confirmText = jqObj.attr("confirmText") || jqObj.attr("confirmtext");
        if (!(confirmId || confirmText)) { return false; }

        var href = jqObj.attr("href");

        if (!href) { return false; }
        jqObj.attr("href", "javascript:;");
        var realAction = href.replace(/"/g, "'");

        var onclickEvent = jqObj.attr("comfirmClick");

        jqObj.click(function () {
            var isPass = true;
            //是否有点击事件，有即优先走onclick事件
            if (onclickEvent != null) {
                if (eval(onclickEvent) == false) {
                    isPass = false;
                }
            }

            if (isPass) {
                whir.dialog.confirm(confirmText,
                    function() { eval(realAction) });
                return false;
            }
        });
    }
}