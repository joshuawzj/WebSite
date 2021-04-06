
(function ($) {

    $.fn.WtlGenerate = function () {
        var thisObj = $(this);

        $(function () {
            whir.wtlInit.data.columnArray();

            $(thisObj).load(function () {
                var iframeObj = $($(this).prop("contentWindow").document);
                var height = iframeObj.find("body").height();
                $(this).css({ "height": height + "px" });
                $(".config-view,.cover").css({ "height": (height + 20) + "px" });

                whir.wtl.initWtl(iframeObj);
                whir.wtl.wtlEditClickEvent(iframeObj);
            });

            $("#select_coulumn").change(function () {
                var columnid = $(this).val();
                whir.wtlInit.columnFieldSelect(columnid);

                var wtlId = $("#generate_btn").attr("sel-data");
                whir.wtl.Service.updateWtl(wtlId, function (wtlObj) {
                    wtlObj.columnId = columnid;
                    return wtlObj;
                });
            });

            $("#cbNotBindColumn").click(function () {
                var wtlId = $("#generate_btn").attr("sel-data");
                whir.wtl.Service.updateWtl(wtlId, function (wtlObj) {
                    wtlObj.isNotBindColumn = $("#cbNotBindColumn").is(":checked");
                    return wtlObj;
                });
            });

            $(".generate_btn").click(function () {
                $("#wtl_generate_iframe").attr("src", "");
                $("#wtl_generate_iframe").attr("src", thisObj.attr("src"));
            });


            $("#wtl_generate_iframe").load(function () {
                if ($(this).attr("scr") != "") {
                    var wtlIframeDom = $($(this).prop("contentWindow").document);

                    var wtlcount = 0;
                    wtlIframeDom.find("[wtl-edit]").each(function () {
                        $(this).attr("wtl-id", wtlcount);

                        var codeCount = 0;
                        $(this).find("script[type^='html/']").each(function () {
                            var id = "code_" + wtlcount + "_" + codeCount;
                            $(this).attr("id", id);

                            codeCount++;
                        });
                        wtlcount++;
                    });


                    whir.wtl.generateHelp.generateWtl(wtlIframeDom, whir.wtl.Array.wtlArray);
                    console.log(wtlIframeDom.find("html").prop("outerHTML"));
                }
            });


            $(".save_btn").click(function () {
                if (confirm("保存之前，请先生成！是否已生成？")) {
                    var wtlIframeDom = $($("#wtl_generate_iframe").prop("contentWindow").document).find("html").clone();
                    if (confirm("是否给页面增加SEO")) {
                        wtlIframeDom.find("head").append("<wtl:Seo></wtl:Seo>");
                    }

                    if (confirm("是否给页面增加客服")) {
                        wtlIframeDom.find("body").append("<wtl:Service></wtl:Service>");
                    }

                    wtlIframeDom.find("body").append("<wtl:Statis></wtl:Statis>");

                    var html = encodeURIComponent(wtlIframeDom.html());

                    $.post("/whir_system/wtl/WtlSave.aspx", {
                        html: html,
                        fileName: whir.wtl.selectWtlFieldName
                    }, function (res) {
                        var result = JSON.parse(res);
                        if (result.IsSuccess) {
                            alert("保存成功");
                        }
                    });




                }
            });

            $(".form_relod").click(function () {
                whir.wtlInit.data.webFormArray();
            });

            $("#template_ul li").click(function () {
                whir.wtl.selectWtlFieldName = $(this).attr("data-url");
                $("#wtl_iframe").attr("src", globalObj.basePath + "whir_system/wtl/WtlHtmlRead.aspx?fileName=" + whir.wtl.selectWtlFieldName);
                closeConfig();
            });


        });
    }


})(jQuery);