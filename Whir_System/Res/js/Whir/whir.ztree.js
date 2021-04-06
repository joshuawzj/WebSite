var checkids;
(function (jQuery) {

    if (jQuery.browser) return;

    jQuery.browser = {};
    jQuery.browser.mozilla = false;
    jQuery.browser.webkit = false;
    jQuery.browser.opera = false;
    jQuery.browser.msie = false;

    var nAgt = navigator.userAgent;
    jQuery.browser.name = navigator.appName;
    jQuery.browser.fullVersion = '' + parseFloat(navigator.appVersion);
    jQuery.browser.majorVersion = parseInt(navigator.appVersion, 10);
    var nameOffset, verOffset, ix;

    // In Opera, the true version is after "Opera" or after "Version"  
    if ((verOffset = nAgt.indexOf("Opera")) != -1) {
        jQuery.browser.opera = true;
        jQuery.browser.name = "Opera";
        jQuery.browser.fullVersion = nAgt.substring(verOffset + 6);
        if ((verOffset = nAgt.indexOf("Version")) != -1)
            jQuery.browser.fullVersion = nAgt.substring(verOffset + 8);
    }
    // In MSIE, the true version is after "MSIE" in userAgent  
    else if ((verOffset = nAgt.indexOf("MSIE")) != -1) {
        jQuery.browser.msie = true;
        jQuery.browser.name = "Microsoft Internet Explorer";
        jQuery.browser.fullVersion = nAgt.substring(verOffset + 5);
    }
    // In Chrome, the true version is after "Chrome"  
    else if ((verOffset = nAgt.indexOf("Chrome")) != -1) {
        jQuery.browser.webkit = true;
        jQuery.browser.name = "Chrome";
        jQuery.browser.fullVersion = nAgt.substring(verOffset + 7);
    }
    // In Safari, the true version is after "Safari" or after "Version"  
    else if ((verOffset = nAgt.indexOf("Safari")) != -1) {
        jQuery.browser.webkit = true;
        jQuery.browser.name = "Safari";
        jQuery.browser.fullVersion = nAgt.substring(verOffset + 7);
        if ((verOffset = nAgt.indexOf("Version")) != -1)
            jQuery.browser.fullVersion = nAgt.substring(verOffset + 8);
    }
    // In Firefox, the true version is after "Firefox"  
    else if ((verOffset = nAgt.indexOf("Firefox")) != -1) {
        jQuery.browser.mozilla = true;
        jQuery.browser.name = "Firefox";
        jQuery.browser.fullVersion = nAgt.substring(verOffset + 8);
    }
    // In most other browsers, "name/version" is at the end of userAgent  
    else if ((nameOffset = nAgt.lastIndexOf(' ') + 1) <
        (verOffset = nAgt.lastIndexOf('/'))) {
        jQuery.browser.name = nAgt.substring(nameOffset, verOffset);
        jQuery.browser.fullVersion = nAgt.substring(verOffset + 1);
        if (jQuery.browser.name.toLowerCase() == jQuery.browser.name.toUpperCase()) {
            jQuery.browser.name = navigator.appName;
        }
    }
    // trim the fullVersion string at semicolon/space if present  
    if ((ix = jQuery.browser.fullVersion.indexOf(";")) != -1)
        jQuery.browser.fullVersion = jQuery.browser.fullVersion.substring(0, ix);
    if ((ix = jQuery.browser.fullVersion.indexOf(" ")) != -1)
        jQuery.browser.fullVersion = jQuery.browser.fullVersion.substring(0, ix);

    jQuery.browser.majorVersion = parseInt('' + jQuery.browser.fullVersion, 10);
    if (isNaN(jQuery.browser.majorVersion)) {
        jQuery.browser.fullVersion = '' + parseFloat(navigator.appVersion);
        jQuery.browser.majorVersion = parseInt(navigator.appVersion, 10);
    }
    jQuery.browser.version = jQuery.browser.majorVersion;
})(jQuery);

if (!window.whir) window.whir = {};

whir.ztree = {

    area: function (wrapId, treeUrl, checkType, onCheck) {

        //选择类型  
        //默认为单选{ enable: true, chkStyle: 'radio', radioType: "all"  }
        //多选传入{ enable: true, chkboxType:{ "Y" : "s", "N" : "s" } }
        //不需要选择框, 传入{ enable: false }
        checkType = checkType || {
            enable: true,
            chkStyle: 'radio',
            radioType: "all"
        };

        if (treeUrl.indexOf('?') > 0)
            treeUrl += "&" + '?id=0';
        else
            treeUrl += "?" + '?id=0';

        //开始异步请求
        $.ajax({
            url: treeUrl,
            cache: false,
            success: function (rootNodes) {
                if (rootNodes != "[]") {
                    var setting = {
                        async: {
                            enable: true,
                            url: getUrl
                        },
                        check: checkType,
                        data: {
                            simpleData: {
                                enable: true
                            }
                        },
                        view: {
                            expandSpeed: ""
                        },
                        callback: {
                            beforeExpand: beforeExpand,
                            onAsyncSuccess: onAsyncSuccess,
                            onAsyncError: onAsyncError,
                            onCheck: onCheck || function () { },
                            onClick: onTreeTextClick
                        }
                    };

                    var zNodes = eval("(" + rootNodes + ")");

                    $(document).ready(function () {
                        $.fn.zTree.init($("#" + wrapId), setting, zNodes);
                    });
                }
                else
                    $("#" + wrapId).empty();
            },
            error: function () {

            }
        });

        function getUrl(treeId, treeNode) {
            var curCount = (treeNode.children) ? treeNode.children.length : 0;
            var param = "id=" + treeNode.id;
            if (treeUrl.indexOf('?') > 0)
                return treeUrl + "&" + param;
            else
                return treeUrl + "?" + param;
        }

        function ajaxGetNodes(treeNode, reloadType) {
            var zTree = $.fn.zTree.getZTreeObj(wrapId);
            if (reloadType == "refresh") {
                //treeNode.icon = "../../css/zTreeStyle/img/loading.gif";
                zTree.updateNode(treeNode);
            }
            zTree.reAsyncChildNodes(treeNode, reloadType, true);
        }

        //展开前执行
        function beforeExpand(treeId, treeNode) {
            if (!treeNode.isAjaxing) {
                startTime = new Date();
                treeNode.times = 1;
                ajaxGetNodes(treeNode, "refresh");
                return true;
            } else {
                alert("zTree 正在下载数据中，请稍后展开节点。。。");
                return false;
            }
        }
        //异步请求成功执行
        function onAsyncSuccess(event, treeId, treeNode, msg) {
            if (!msg || msg.length == 0) {
                return;
            }
            var zTree = $.fn.zTree.getZTreeObj(wrapId);
            var totalCount = treeNode.count;
            if (treeNode.children.length < totalCount) {
                setTimeout(function () { ajaxGetNodes(treeNode); }, 100);
            } else {
                treeNode.icon = "";
                zTree.updateNode(treeNode);
                zTree.selectNode(treeNode.children[0]);
                var endTime = new Date();
                var usedTime = (endTime.getTime() - startTime.getTime()) / 1000;
            }
            whir.ztree.setCheck(treeId, checkids||"");
        }
        //异步请求失败执行
        function onAsyncError(event, treeId, treeNode, XMLHttpRequest, textStatus, errorThrown) {
            var zTree = $.fn.zTree.getZTreeObj(wrapId);
            alert("异步获取数据出现异常。");
            treeNode.icon = "";
            zTree.updateNode(treeNode);
        }
        //点击树节点文字，默认选择此选项
        function onTreeTextClick(event, treeId, treeNode) {
            var zTree = $.fn.zTree.getZTreeObj(wrapId);
            zTree.checkNode(treeNode, true, true);
        }
    },

    //根据固定节点JSON绑定树
    column: function (wrapId, nodes, onCheck, checkType) {

        var transition = {
            setHidden: function (obj) {
                for (var j = 0; j < obj.length; j++) {
                    if (obj[j].children) {
                        this.setHidden(obj[j].children);
                        this.setParentHidden(obj[j]);
                    }
                    else if (obj[j].nocheck) {
                        obj[j].isHidden = true;
                    }
                }
            },
            setParentHidden: function (obj) {
                var i = 0;
                for (var j = 0; j < obj.children.length; j++) {
                    if (obj.children[j].isHidden) {
                        i++;
                    }
                }
                if (i == obj.children.length) {
                    obj.isHidden = true;
                }
            }
        };

        transition.setHidden(nodes);
        var setting = {
            //选择类型  
            //单选传入{ enable: true, chkStyle: 'radio', radioType: "all"  }
            //(默认为)多选传入{ enable: true, chkboxType:{ "Y" : "s", "N" : "s" } }
            //不需要选择框, 传入{ enable: false }
            check: checkType || {
                enable: true,
                chkboxType: { "Y": "s", "N": "s" }
            },
            callback: {
                onCheck: onCheck || function () { },
                onClick: function (event, treeId, treeNode) { $.fn.zTree.getZTreeObj(wrapId).expandNode(treeNode); }
            },
            view: {
                nameIsHTML: true,
                dblClickExpand: false
            },
            data: {
                key:
                    {
                        title: "fullName"
                    }
            }
        };
        $(document).ready(function () {
            $.fn.zTree.init($("#" + wrapId), setting, nodes);
        });
    },

    //根据固定节点JSON绑定树
    clickNode: function (wrapId, nodes, onClick) {

        var setting = {

            callback: {
                onClick: onClick || function () { },
            },
            view: {
                nameIsHTML: true,
                dblClickExpand: false
            },
            data: {
                key:
                    {
                        title: "fullName"
                    }
            }
        };
        $(document).ready(function () {
            $.fn.zTree.init($("#" + wrapId), setting, nodes);
        });
    },

    //设置选中的ID值
    setCheck: function (treeId, id) {
        //获取树对象
        var treeObj = $.fn.zTree.getZTreeObj(treeId);
        if (treeObj == null || treeObj == undefined) {
            return false;
        }
        checkids = id;
        var ids = id.split(',');
        for (var i = 0; i < ids.length; i++) {
            var id = ids[i];
            if (id == "") {
                continue;
            }
            var node = treeObj.getNodeByParam("id", id, null);
            if (node == null || node.checked)
                continue;
            treeObj.checkNode(node, true, true);
        }
        return true;
    },

    //获取选中的ID值, 多个ID用英文逗号隔开, 用于地区选择控件
    getSelected: function (treeId) {
        //获取树对象
        var treeObj = $.fn.zTree.getZTreeObj(treeId);
        if (treeObj == null || treeObj == undefined) {
            return "";
        }
        //以选中的节点集合
        var selectedNodes = treeObj.getCheckedNodes(true);

        var result = "";
        $(selectedNodes).each(function (idx, item) {
            result += treeObj.getNodeByTId(item.tId).id;
            result += ",";
        });

        if (result.length > 0)
            return result.substring(0, result.length - 1);
        else
            return result;
    },

    //获取选中的ID值, 多个ID用英文逗号隔开, 用于地区选择控件
    getSelectedText: function (treeId) {

        //获取树对象
        var treeObj = $.fn.zTree.getZTreeObj(treeId);
        if (treeObj == null || treeObj == undefined) {
            return "";
        }
        //以选中的节点集合
        var selectedNodes = treeObj.getCheckedNodes(true);

        var result = "";
        $(selectedNodes).each(function (idx, item) {
            result += treeObj.getNodeByTId(item.tId).name;
            result += "|";
        });

        if (result.length > 0)
            return result.substring(0, result.length - 1);
        else
            return result;
    },
    //获取节点
    getNode: function (treeId, nodeId) {
        //获取树对象
        var treeObj = $.fn.zTree.getZTreeObj(treeId);
        if (treeObj == null || treeObj == undefined) {
            return "";
        }
        var result = treeObj.getNodeByTId(nodeId);
        return result;
    },

    //获取单选树中, 处于选中状态的节点级别, 根节点为0, 依次累加
    getRadioSelectedLevel: function (treeId) {
        //获取树对象
        var treeObj = $.fn.zTree.getZTreeObj(treeId);
        //以选中的节点集合
        var selectedNodes = treeObj.getCheckedNodes(true);

        if (selectedNodes.length > 0) {
            var selectedNode = selectedNodes[0];
            return selectedNode.level;
        }

        return -1;
    },

    //获取选中的JSON, 用于添加信息时, 选择其它栏目
    getSelectedJson: function (treeId) {
        //获取树对象
        var treeObj = $.fn.zTree.getZTreeObj(treeId);
        //以选中的节点集合
        var selectedNodes = treeObj.getCheckedNodes(true);

        var json = "[";
        $(selectedNodes).each(function (idx, item) {
            var id = treeObj.getNodeByTId(item.tId).id;
            var name = treeObj.getNodeByTId(item.tId).name;
            var subjectId = treeObj.getNodeByTId(item.tId).subjectID;
            json += "{ id : '" + id + "', tid : '" + item.tId + "' , name : '" + name + "', subjectID : '" + subjectId + "' },";
        });

        if (json.length > 1)
            json = json.substring(0, json.length - 1);
        else
            json = json;
        json += "]";

        return json;
    },

    unCheck: function (treeId, tid) {

    }

};


