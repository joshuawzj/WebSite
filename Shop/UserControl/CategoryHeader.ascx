<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CategoryHeader.ascx.cs" Inherits="Shop_UserControl_CategoryHeader"  EnableViewState="false"  %>
<div class="CategoryCar">
  <div class="MyCategory">
    <h3><span class="a_arrow" onclick="javascript:location='<%=AppName %>Shop/productlist.aspx'">商品列表</span></h3>
      <div class="HideMenu">
         <%-- <asp:Literal ID="ltCategoryList" runat="server"></asp:Literal>--%>
        
      </div>
    </div>
    <div class="MyCar"><i>购物车中共有 <b>
        <asp:Literal ID="ltCartCount" runat="server"></asp:Literal></b> 件商品</i> <a href='<%=AppName %>Shop/ShopCart.aspx' class="a_pay"></a></div>
</div>
<%--<script type="text/javascript">
    jQuery(".MyCategory .item").bind("mouseover", function () {
        jQuery(this).find("dt").attr("class", "MouseOver");
        jQuery(this).find(".sub").show();

    })
    jQuery(".MyCategory .item").bind("mouseleave", function () {
        jQuery(this).find(".sub").hide();
        jQuery(this).find("dt").attr("class", "");
    })


    jQuery(".MyCategory .a_arrow").bind("mouseover", function () {
        jQuery(".HideMenu").show();
    })
    jQuery(".MyCategory").bind("mouseleave", function () {
        jQuery(".HideMenu").hide();
    })


 </script>--%>