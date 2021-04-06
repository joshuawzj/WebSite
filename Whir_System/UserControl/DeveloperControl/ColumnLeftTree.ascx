<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ColumnLeftTree.ascx.cs"
    Inherits="whir_system_UserControl_DeveloperControl_ColumnLeftTree" %>

 <%@ Import Namespace="Whir.Language" %>


<link href="<%=SysPath %>res/js/jquery.treeview/zTreeStyle/zTreeStyle.css" rel="stylesheet"
    type="text/css" />
<script src="<%=SysPath %>res/js/jquery.treeview/jquery.ztree.core-3.3.js" type="text/javascript"></script>
<script src="<%=SysPath %>res/js/jquery.treeview/jquery.ztree.excheck-3.3.js"
    type="text/javascript"></script>
<script type="text/javascript">
		<!--
        var zNodes=<%=BindTree() %>;
 
        var setting = {
            check: {
                enable: false,  //不显示 checkbox 
                chkboxType:{ "Y" : "s", "N" : "s" }
            },
            data: {
                simpleData: {
                    enable: true,    
                    idKey:"id",
                    pIdKey: "pId",
                    rootPId:0
                }
            },
            callback: {
                onClick:zTreeOnClick
            }
        };
       
        $(function() {
            var showselectid="<%=ShowSelectID %>";
            var treeObj=$.fn.zTree.init($("#browser"), setting, zNodes);
            treeObj.selectNode(treeObj.getNodeByTId(showselectid));
        });

        function zTreeOnClick(event, treeId, treeNode) {
            var nodes = $.fn.zTree.getZTreeObj("browser").getSelectedNodes();
            var id=nodes[0].Id;
            var parentId=nodes[0].pId;
            
            //根据 whir_system/Config/GenerateLabel.config 中的配置，父节点，因此没有任何的链接,也就是 parentId 为-1的都是父节点。
            if(parentId === 0&&id !== 0)
            {
                //异步的方式获取当前需要调整的页面
                $.get("<%=SysPath %>ajax/developer/generatelabel.aspx",{id:id,showselectid:treeNode.tId},function(data){
                    if(data== "error"){
                        alert("当前Id为"+id+"的置标不存在");
                    }else{
                        window.location=data;
                    }
                });
           }
        };

		//-->
</script>
<script type="text/javascript">
    $(function () {

    /* 不支持多浏览器的 复制
        $("#A2").click(function () {
            var txt = $("#ctl00_ContentPlaceHolder1_txtContent");
            if (txt.val().trim() == "") {
                TipMessage("生成置标内容为空", function () { txt.focus(); });
                return;
            }

            if (document.all) {                                            //判断Ie
                window.clipboardData.setData('text', txt.val().trim());
                TipMessage("复制成功");
            } else {
                TipMessage("您的浏览器不支持剪贴板操作，请自行复制。");
            }
        });*/


           

        //设置左右两边高度统一
        if ($("#multiline").height() != null) {
            if ($("#browser").height() > $("#multiline").height()) {
                $("#multiline").height($("#browser").height() - 295);
            }
        } else if ($("#singleline").height() != null) {
            if ($("#browser").height() > $("#singleline").height()) {
                $("#singleline").height($("#browser").height() - 60);
            }
        }
    })
</script>


 <script type="text/javascript" src="<%=SysPath%>res/js/ZeroClipboard.js"></script>
 <script type="text/javascript">
 
    //支持多浏览器 的复制
     var clip = null;
     $(function () {
         //设置ZeroClipboard.swf的路径  
         ZeroClipboard.setMoviePath("<%=SysPath%>res/flash/ZeroClipboard.swf");

         $("#A2").each(function () {

             clip = new ZeroClipboard.Client();
             clip.setHandCursor(true);
             var obj = $(this);
             var id = $(this).attr("id");
             var content = $('#Content').val();
             clip.setText(content);
             clip.glue(id);
             //这个是复制成功后的提示    
             clip.addEventListener("complete", function () {
                 var txt = $('#Content').val();

                 if ($.trim(txt) != "") {
                     TipMessage('<%="已经复制到剪切板".ToLang() %>');
                 } else {
                     TipMessage('<%="无数据，不可复制".ToLang() %>');
                 }
             });
         });
     });
    </script>
<ul id="browser" class="ztree" style="overflow: auto;">
</ul>
