<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    CodeFile="shopattrvalueselect.aspx.cs" Inherits="whir_system_Plugin_shop_common_shopattrvalueselect" %>

<%@ Register Src="~/whir_system/Plugin/shop/common/HeadContainer.ascx" TagName="HeadContainer"
    TagPrefix="uc1" %>
<%@ Import Namespace="Shop.Domain" %>
<%@ Import Namespace="Shop.Service" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
    <style type="text/css">
        .tdBodyLeft {
            width: 120px;
            height: 35px;
            text-align: right;
        }

        .tdBodyRight {
            width: 500px;
            text-align: left;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var selected = '';

            $("input[name='attrCheckbox']").each(function () {
                $(this).next().click(function () {
                    getAttr();
                });
            });


            function getAttr() {
                selected = "";
                $("#proAttrEdit .form-group").each(function () {
                    $(this).find("input[name='attrCheckbox']").each(function () {
                        if ($(this).next().prev().is(":checked")) {
                            selected += $(this).attr("rel") + "*" + $(this).val() + ",";
                        }
                    });
                    if (selected != "") {
                        selected = selected.substring(0, selected.length - 1) + ";";
                    }
                });
                if (selected != "") {
                    selected = selected.substring(0, selected.length - 1);
                }
            }


            $("[name=_dialog] .btn-primary", parent.document).click(function () {
                if (selected != "") {
                    window.parent.<%= CallBack %>(selected);
                    window.parent.whir.dialog.remove();
                } else {
                    whir.toastr.error("<%="请选择规格".ToLang()%>");
                }
                return false;
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
        <div class="panel-body" style=" padding-left:25px;">
            <form id="proAttrEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx">
                <%if (searchlist.Count > 0)
                    { %>
                <%foreach (var item in searchlist)
                    { %>
                <div class="form-group">
                    <div class="col-md-3 control-label" for="divIsAllowBuy">
                        <%=item.SearchName%>：
                    </div>
                    <div class="col-md-9 ">
                        <ul class="list">
                            <%  IList<ShopAttrValue> attrValuelist = ShopAttrValueService.Instance.GetAttrValueByAttrID(item.AttrID);%>
                            <%if (attrValuelist.Count > 0)
                                { %>
                            <%foreach (var itemValue in attrValuelist)
                                {%>
                            <li>
                                <input type="checkbox" rel="<%=itemValue.AttrValueName %>" name="attrCheckbox" value="<%=itemValue.AttrValueID %>" />
                                <label>
                                    <%=itemValue.AttrValueName %></label></li>
                            <%} %>
                            <%} %>
                        </ul>
                    </div>
                </div>
                <%} %>
                <%} %>
            </form>
        </div>
   
</asp:Content>
