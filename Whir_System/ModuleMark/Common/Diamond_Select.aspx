<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="diamond_select.aspx.cs" Inherits="whir_system_ModuleMark_common_diamond_select" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="<%=SysPath%>res/js/jquery.treeview/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="<%=SysPath%>res/js/jquery.treeview/jquery.ztree.core-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/jquery.treeview/jquery.ztree.excheck-3.3.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/Whir/whir.ztree.js" type="text/javascript"></script>

    <script>
       
        //加载树
         <%if (IsMultiple)
           {%>
        whir.ztree.area("areaTree", 
            "<%=SysPath%>ajax/common/getDiamondOption.aspx?formid=<%= FormId %>&subjectid=<%=SubjectId%>",
            { enable: true, chkboxType:{ "Y" : "s", "N" : "s" } });
        <%}
           else
           {
          %>
        whir.ztree.area("areaTree", "<%=SysPath%>ajax/common/getDiamondOption.aspx?formid=<%= FormId %>&subjectid=<%=SubjectId%>") ;
        <%}%>
        
        $(function () {
            $("#txtKeyWord").bind('keydown', function (e) {
                var key = e.which;
                if (key == 13) {
                    search();
                }});

            setTimeout(function(){
                whir.ztree.setCheck("areaTree","<%=FormOptionId%>");
            },500);

            $("[name=_dialog] .btn-primary", parent.document).click(function () {
                
                var text =whir.ztree.getSelectedText("areaTree");
                var value =whir.ztree.getSelected("areaTree");
                //if(text.lastIndexOf("> ")>=0)
                  //  text=text.substring(text.lastIndexOf(">")+2,text.length);
                window.parent.<%= CallBack %>(value,text);
                window.parent.whir.dialog.remove();
                
                return false;
            });
        });

        function search(){
            var keyword=$.trim($("#txtKeyWord").val());
            if(keyword!=''){
               <%if (IsMultiple)
               {%>
                whir.ztree.area("areaTree", 
                        "<%=SysPath%>ajax/common/getDiamondOption.aspx?formid=<%= FormId %>&subjectid=<%=SubjectId%>&keyword="+keyword,
                { enable: true, chkboxType:{ "Y" : "s", "N" : "s" } });
            <%}
               else
               {
              %>
                whir.ztree.area("areaTree", "<%=SysPath%>ajax/common/getDiamondOption.aspx?formid=<%= FormId %>&subjectid=<%=SubjectId%>&keyword="+keyword) ;
                <%}%>
               
            }
        }
       
    </script>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    
   <div class="mainbox  ">
        <div class="input-group panel-body"  >
                <input type="text" class="form-control" id="txtKeyWord" name="KeyWord" value="" maxlength="250"  />
                <span class="input-group-addon btn  pointer btn-primary" id="btnSearch"  onclick="search();" >搜索</span>
            </div>
        <ul id="areaTree" class="ztree"></ul>
    </div>

    <div class="divpages" style="text-align:center">
       <ul id="ulPages"></ul>
    </div>
 
</asp:content>
