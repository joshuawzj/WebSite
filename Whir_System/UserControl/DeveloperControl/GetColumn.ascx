<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GetColumn.ascx.cs" Inherits="Whir_System_UserControl_DeveloperControl_GetColumn" %>
<%@ Import Namespace="Whir.Language" %>
<div class="form-group">
    <div class="col-xs-4 control-label text-right" for="ddlSite">
        <%="站点：".ToLang()%>
    </div>
    <div class="col-xs-8">
        <select id="Site" name="Site" class="form-control">
            <option value="">==请选择==</option>
            <% foreach (var item in Sitelist)
               {
                   var check = "";
            %>
            <% if (item.SiteId == SiteId)
               {
                   check = "selected=\"selected\"";
               } %>
            <option <%=check %> value="<%=item.SiteId%>">
                <%=item.SiteName%></option>
            <%  }   %>
        </select>
    </div>
</div>
<div class="form-group">
    <div class="col-xs-4 control-label text-right" for="ddlColumn">
        <%="栏目：".ToLang()%>
    </div>
    <div class="col-xs-8">
        <select id="Column" name="Column" class="form-control">
            <option value="">==请选择==</option>
            <% foreach (var item in Columnlist)
               {
                   var check = "";
            %>
            <% if (item.ColumnId == ColumnId)
               {
                   check = "selected=\"selected\"";
               } %>
            <option <%=check %> value="<%=item.ColumnId%>">
                <%=item.ColumnName%></option>
            <%  }   %>
        </select>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        var column = $("#Column");
        $("#Site").change(function () {
            var siteid = $(this).val();
            //异步发送请求
            $.get("<%=SysPath %>ajax/common/getcolumn.aspx?time=" + new Date().getMilliseconds(),
                { siteid: siteid },
                function (data) {
                    column.html(data);
                });
        });
    });
</script>
