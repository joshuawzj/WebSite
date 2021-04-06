<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Jurisdiction_column_top.ascx.cs" Inherits="whir_system_module_security_Jurisdiction_column_top" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<script>
    jQuery(document).ready(function () {
        //当前授权的栏目，标示颜色
        var selectColumnType = "<%=SiteId %><%=SelectType %>";
         $("#" + selectColumnType).css("color", "#FF5A00");

     });
</script>


<div class="panel panel-default">
    <div class="panel-heading"><%="选择站点/类型".ToLang() %></div>
    <div class="panel-body">
        <asp:DataList ID="rptList" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" Width="100%">
            <ItemTemplate>
                <div class="col-sm-12">
                        <b><%#Eval("SiteName").ToStr()%>：</b>
                    <a   href="Jurisdiction_column.aspx?siteid=<%#Eval("SiteId").ToStr()%>&type=0&roleid=<%=RoleID %>" id="<%#Eval("SiteId").ToStr()%>0"><%="站点栏目".ToLang() %></a>
                    <a   href="Jurisdiction_subject.aspx?siteid=<%#Eval("SiteId").ToStr()%>&type=1&roleid=<%=RoleID %>" id="<%#Eval("SiteId").ToStr()%>1"><%="子站栏目".ToLang() %></a>
                    <a   href="Jurisdiction_subject.aspx?siteid=<%#Eval("SiteId").ToStr()%>&type=2&roleid=<%=RoleID %>" id="<%#Eval("SiteId").ToStr()%>2"><%="专题栏目".ToLang() %></a>
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
</div>
<%if (SelectType > 0)
    { %>
<div class="panel panel-default">
    <div class="panel-heading">
        &nbsp;<%=("选择"+(SelectType==1?"子站":"专题")).ToLang() %>
    </div>
    <div class="panel-body">
        <div class="row">
            <div class="col-xs-4">
                <div class="input-group ">
                    <span class="input-group-addon"><%=(SelectType==1?"子站类型":"专题类型").ToLang()%></span>
                    <select id="Class" class="form-control" onchange="getSubject(this);">
                        <option value='' selected='selected'><%="==请选择==".ToLang() %></option>
                        <%foreach (var item in ListClass)
                            {%>
                        <option value="<%=item.SubjectClassId %>"><%=item.SubjectClassName %></option>
                        <% } %>
                    </select>
                </div>
            </div>
            <div class="col-xs-4">
                <div class="input-group ">
                    <span class="input-group-addon"><%=(SelectType==1?"子站":"专题").ToLang()%></span>
                    <select id="subjectId" class="form-control" onchange="goUrl(this);">
                    </select>

                </div>
            </div>
            <div class="col-xs-4">

                <input type="checkbox" id="isSame" name="isSame"/>
                <span style="padding-left: 2px;"><%="同时应用到其他同类型子站".ToLang() %></span>

            </div>
        </div>
    </div>
</div>
<select id="subjectIdhide" class="form-control" style="display: none;">
    <%foreach (var item in ListSubject)
        {%>
    <option value="<%=item.SubjectId %>" classid="<%=item.SubjectClassId %>"><%=item.SubjectName %></option>
    <% } %>
</select>
<script>
    var classId;
    function getSubject(o) {
        classId = $(o).val();
        $("#subjectId").empty();
        $("#subjectId").append("<option value='' selected='selected'><%="==请选择==".ToLang() %></option>");
        $("#subjectIdhide").find("option").each(function () {
            if ($(this).attr("classid") == classId)
                $("#subjectId").append($(this).clone());
        });
    }
    function goUrl(o) {
        if ($(o).val() == "")
            return;
        location.href = "Jurisdiction_subject.aspx?siteid=<%=SiteId%>&type=<%=SelectType%>&roleid=<%=RoleID %>&classid=" + classId + "&subjectId=" + $(o).val();
    }
    $("#Class").val("<%=RequestUtil.Instance.GetString("classid")%>").change();
    $("#subjectId").val("<%=RequestUtil.Instance.GetString("subjectId")%>");
</script>
<%} %>