<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShopCart.ascx.cs" Inherits="Shop_UserControl_ShopCart" %>
<%@ Import Namespace="Whir.Framework" %>
<div class="ShopContain">
   
  <div class="MainBox">
  
      
      <!--Start-->
      <div class="mycar"><b></b></div>
   
    <!--Content Start-->
    <asp:MultiView runat="server" ID="CartView" ActiveViewIndex="0">
    <asp:View runat="server" ID="V_Empty">
    <div class="empty">
      <h5>您的购物车还是空的，赶紧行动吧！您可以：</h5>
      <p>如果您还未登录，可能导致购物车为空，请<a href="<%=AppName %>Shop/member/login.aspx">马上登录</a><br />
        如果您已经是会员，马上去<a href="<%=AppName %>Shop/productlist.aspx">挑选产品</a></p>
    </div>
    </asp:View>
    <asp:View runat="server" ID="V_CartShow">
    

    <!--购物车商品-->
    <div class="CartProduct">
    <table id="CartTable" width="100%" border="0" cellspacing="0" cellpadding="0" class="mytable">
      <tr class="item">
        <td width="10"></td>
        <td width="120"></td>
        <td>订单商品</td>
        <td width="150">金额</td>
        <td width="100">数量</td>
        <td width="100">小计</td>
        <td width="100">操作</td>
      </tr>

          <asp:Repeater ID="rptShopCart" runat="server">
        <ItemTemplate>
             <tr>
        <td><input type="checkbox" name="checkbox" id="checkbox_<%#Eval("CartID") %>" /></td>
        <td><dt class="picture"><a href="<%=AppName %>Shop/productinfo.aspx?proid=<%# Eval("ProID") %>&aids=<%# Eval("AttrValueIDs") %>">
         <img onerror="this.src='<%=AppName %>res/images/nofile.jpg'"
                    alt='<%#Eval("ProName") %>' src='<%=UploadFilePath %><%# Eval("ProImg") %>' />
        </a></dt></td>
        <td><dt class="name"><%# Eval("IsBuy").ToString()=="0"?"<span style='color:red' class='undercarriage'>[商品已下架]</span><br/>":"" %><a href="<%=AppName %>Shop/productinfo.aspx?proid=<%# Eval("ProID") %>&aids=<%# Eval("AttrValueIDs") %>" title='<%# Eval("ProName") %>'>
                        <%# Eval("ProName") %> <%# Eval("AttrValueNames") %>
                        <br /><%# Eval("ProNO") %>
                        </a>
            </dt></td>
        <td>
        <font><%#string.Format("{0:C2}", Eval("CostAmount").ToDecimal(0))%></font>
        </td>
        <td>
        <span class="subNum">-</span>
        <input type="text" name="txtNums"  style="text-align:center" id='txtNum_<%#Eval("CartID") %>' price='<%#Eval("CostAmount") %>'  oldvalue='<%# Convert.ToInt32(Eval("Qutity")) %>' class="text" value='<%# Convert.ToInt32(Eval("Qutity")) %>' />
        <span class="addNum">+</span>
        </td>
        <td><font id="txtTotal_<%#Eval("CartID") %>" value="<%# Eval("CostAmount").ToDecimal(0) * Eval("Qutity").ToDecimal(0) %>">￥<%#string.Format("{0:f2}", Eval("CostAmount").ToDecimal(0) *Eval("Qutity").ToDecimal(0))%></font></td>
        <td><input type="submit" name="btnRemovePros" id="btnRemovePro" runat="server" onserverclick="btnRemovePro_Click" value="删除" class="delete" /></td>
      </tr>
        </ItemTemplate>
    </asp:Repeater>
   
    </table>
    <div class="Total"><span style="float:left"><a  name="linkRemoveSelPro" id="linkRemoveSelPro" href="#" class="delSelPro">删除选中商品</a></span> <font  id="CartCount"><%=Count %></font> 件商品，总计：<font id="CartTotal">￥<%= string.Format("{0:f2}",Total) %></font><span>（不含运费）</span></div>
    </div>
    <!--购物车商品End-->
    
    <div class="btnDiv">
    <input type="hidden" id="pronum" name="pronum" />
   <input type="hidden" id="dels" name="dels" />
    <input type="button" name="btnContinueShopping" id="btnContinueShopping" value="继续购物" class="btn1" onfocus="this.blur()"/>
    <input type="submit" runat="server" onserverclick="btnUpdateShopCart_Click" name="btnUpdateShopCart" id="btnUpdateShopCart" style="display:none;" value="更新购物车" class="btn2" onfocus="this.blur()"/>
    <input type="submit" runat="server" onserverclick="btnClearShopCart_Click" name="btnClearShopCart" id="btnClearShopCart" value="清空购物车" class="btn2" onfocus="this.blur()"/>
     <input type="submit" style="display:none" runat="server" onserverclick="btnRemoveSelPro_Click" name="btnRemoveSelPro" id="btnRemoveSelPro" value="删除所选商品" class="btn2" onfocus="this.blur()"/>
    
    <input type="button" name="btnSettleAccounts" id="btnSettleAccounts" value="前往结算" class="btn1"  onfocus="this.blur()"/>
    </div>
    
    
    <!--Content End-->     
      <script type="text/javascript" src="<%=AppName %>Shop/Scripts/shopcart.js"></script>
  <script type="text/javascript">
      carurl = '<%=AppName %>Shop/';

      jQuery(":text[id][name='txtNums']").blur(function () {
          var obj = $(this);
          var value = obj.val();
          if (value == "")
              obj.val("1");
          if (parseInt(value, 10).toString() == "NaN") {
              obj.val("1");
          }
         
          jQuery.get(carurl + "ajax/AddCart.ashx", { cartid: obj.attr("id").replace("txtNum_", ""), qutity: parseInt(obj.val(), 10) }, function (data) {

              if (data != "1") {
                  obj.val(parseInt(obj.attr("oldvalue"), 10));
              } else {
                  var old = obj.attr("oldvalue");
                  obj.attr("oldvalue", parseInt(obj.val(), 10));
                  JSTotal(obj.attr("id").replace("txtNum_", ""), obj.attr("price"), obj.val(), old);
              }

          });

      });


      //价格小计
      function JSTotal(cartId,price,num,old) {

   
          var result = jQuery("#CartTotal").text().replace("￥", "");
          
          var cartItemTotal=jQuery("#txtTotal_" + cartId).text().replace("￥","");
          var currCartItemTotal = parseFloat(price,10) * parseFloat(num,10);
          result = parseFloat(result,10) - parseFloat(cartItemTotal,10) + parseFloat(currCartItemTotal,10);

          jQuery("#txtTotal_" + cartId).text("￥" + currCartItemTotal.toFixed(2));
          jQuery("#CartTotal").text("￥" + result.toFixed(2));


          
          var count = 0;
          result = jQuery("#CartCount").text();
          result = parseInt(result, 10) - parseInt(old, 10) + parseInt(num, 10);
          jQuery("#CartCount").text(result);
          
      }

      jQuery(":text[id][name='txtNums']").keypress(function (evt) {
          evt = (evt) ? evt : ((window.event) ? window.event : "") //兼容IE和Firefox获得keyBoardEvent对象
          var key = evt.keyCode ? evt.keyCode : evt.which; //兼容IE和Firefox获得keyBoardEvent对象的键值
         
          if (key < 48 || key > 59 || key==48 && jQuery(this).val() == "0" && jQuery(this).val() == "")
            return false;
      });

      jQuery(".addNum").click(function () {
          var numInput = jQuery(this).prev("input");
         

          jQuery(numInput).val(parseInt(jQuery(numInput).val(), 10) + 1);


          jQuery.get(carurl + "ajax/AddCart.ashx", { cartid: jQuery(numInput).attr("id").replace("txtNum_", ""), qutity: parseInt(jQuery(numInput).val(), 10) }, function (data) {

              if (data != "1") {
                  jQuery(numInput).val(parseInt(jQuery(numInput).attr("oldvalue"), 10));
              } else {

                  var old = jQuery(numInput).attr("oldvalue");
                  jQuery(numInput).attr("oldvalue", parseInt(jQuery(numInput).val(), 10));
                  JSTotal(jQuery(numInput).attr("id").replace("txtNum_", ""), jQuery(numInput).attr("price"), jQuery(numInput).val(), old);
              }

          });

      });
      jQuery(".subNum").click(function () {

          var numInput = jQuery(this).next("input");

          if (parseInt(jQuery(numInput).val()) > 1)
              jQuery(numInput).val(parseInt(jQuery(numInput).val(), 10) - 1);
          else
              jQuery(numInput).val('1');

        

          jQuery.get(carurl + "ajax/AddCart.ashx", { cartid: jQuery(numInput).attr("id").replace("txtNum_", ""), qutity: parseInt(jQuery(numInput).val(), 10) }, function (data) {

              if (data != "1") {
                  jQuery(numInput).val(parseInt(jQuery(numInput).attr("oldvalue"), 10));
              } else {
                  var old = jQuery(numInput).attr("oldvalue");
                  jQuery(numInput).attr("oldvalue", parseInt(jQuery(numInput).val(), 10));
                  JSTotal(jQuery(numInput).attr("id").replace("txtNum_", ""), jQuery(numInput).attr("price"), jQuery(numInput).val(), old);
              }

          });

      });
      jQuery("#btnContinueShopping").click(function () {
          window.location.href = '<%=AppName %>Shop/productlist.aspx';
      });
      jQuery("#btnSettleAccounts").click(function () {
          if (jQuery(".undercarriage").length > 0) {
              alert("您的购物车中有下架商品，请先移除再进行结算。");
          }
          else {
              window.location.href = '<%=AppName %>Shop/shopcart2.aspx';
          }
      });

      jQuery("#<%=btnUpdateShopCart.ClientID %>").click(function () {
          var values = '';
          jQuery(":text[id][name='txtNums']").each(function (i, item) {
              if (values != '') {
                  values += ',';
              }
              values += jQuery(item).attr("id").replace('txtNum', jQuery(item).val());
          });

          jQuery("#pronum").val(values);
          return true;
      });

      //清空购物车

      jQuery("#<%=btnClearShopCart.ClientID %>").click(function () {

          return window.confirm("确定清空购物车？");
      })



      //删除选中商品
      jQuery("#linkRemoveSelPro").click(function () {

          var delCartIds = "";


          jQuery("#CartTable :checked").each(function (i, item) {
              if (delCartIds != "")
                  delCartIds += ",";
              delCartIds += jQuery(this).attr("id").split('_')[1];
          });

          if (delCartIds == "") {
              alert("请选择需要移出购物车的商品");
              return false;
          }
          jQuery("#dels").val(delCartIds);

          if (window.confirm("是否确定移除选中商品！")) {
              jQuery("#<%=btnRemoveSelPro.ClientID %>").click();
          }
          return false;
      });


      jQuery("#CartTable .delete").click(function () {
          if (window.confirm("是否确定移除选中商品！")) {
              var delCartId = jQuery(this).parent().parent().find(":checkbox").attr("id").split('_')[1];
              jQuery("#dels").val(delCartId);
              return true;
          } else
              return false;
      })

      jQuery("#btnContinueShopping").click(function () {
          window.location.href = '<%=AppName %>Shop/productlist.aspx';
      });

      

  </script>

    </asp:View>
    </asp:MultiView>    
    </div>
      <!--End-->
      
  </div>

