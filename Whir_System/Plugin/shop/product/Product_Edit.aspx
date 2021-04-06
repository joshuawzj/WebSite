<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Product_Edit.aspx.cs" Inherits="whir_system_Plugin_shop_product_product_edit" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Shop.Service" %>
<%@ Import Namespace="Shop.Domain" %>
<%@ Register Src="~/whir_system/Plugin/shop/common/HeadContainer.ascx" TagName="HeadContainer"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
    <script src="<%=SysPath%>res/js/jquery_sorttable/core.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/jquery_sorttable/widget.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/jquery_sorttable/mouse.js" type="text/javascript"></script>
    <script src="<%=SysPath%>res/js/jquery_sorttable/sortable.js" type="text/javascript"></script>
    
    <style type="text/css">
        .tdHeaderLeft {
            background-color: #F4F7F8;
            width: 120px;
            padding-left: 30px;
            text-align: left;
            border-left: 1px solid #D6DFE5;
            border-top: 1px solid #D6DFE5;
            border-bottom: 1px solid #D6DFE5;
        }

        .tdHeaderRight {
            background-color: #F4F7F8;
            padding-right: 30px;
            text-align: right;
            border-right: 1px solid #D6DFE5;
            border-top: 1px solid #D6DFE5;
            border-bottom: 1px solid #D6DFE5;
        }

        .tdBody {
            text-align: center;
            border-left: 1px solid #D6DFE5;
            border-right: 1px solid #D6DFE5;
            border-bottom: 1px solid #D6DFE5;
            padding-top: 15px;
        }

        .button_submit_div {
            padding-bottom: 3px;
            padding-top: 3px;
        }

        .tdBodyLeft {
            border-left: 1px solid #D6DFE5;
            width: 130px;
            height: 35px;
            text-align: right;
        }

        .tdBodyRight {
            border-right: 1px solid #D6DFE5;
            width: 500px;
            text-align: left;
        }

        .img_upload {
            height: 90px;
            width: 90px;
        }

        .sortable_img_li {
            width: 120px;
            cursor: auto;
            height: 130px;
        }

        .tdHeaderLeft1 {
            width: 120px;
            text-align: right;
            border-left: 1px solid #D6DFE5;
            border-top: 1px solid #D6DFE5;
        }

        .tdHeaderRight1 {
            padding-right: 30px;
            text-align: left;
            border-right: 1px solid #D6DFE5;
            border-top: 1px solid #D6DFE5;
        }

        .tdBodyLeft1 {
            border-left: 1px solid #D6DFE5;
            width: 120px;
            height: 30px;
            text-align: right;
        }

        .tdBodyRight1 {
            border-right: 1px solid #D6DFE5;
            width: 500px;
            text-align: left;
        }

        .tdBottomLeft1 {
            width: 120px;
            text-align: right;
            border-left: 1px solid #D6DFE5;
            border-bottom: 1px solid #D6DFE5;
        }

        .tdBottomRight1 {
            padding-right: 30px;
            text-align: right;
            border-right: 1px solid #D6DFE5;
            border-bottom: 1px solid #D6DFE5;
        }

        .td2 {
            border-left: 1px solid #D6DFE5;
            border-right: 1px solid #D6DFE5;
        }

        .TrLast {
            line-height: 8px;
            border-top: 1px solid #D6DFE5;
        }

        .btn_bold.bold {
            font-weight: bold;
            background: #f0f4f7;
        }

        .btn_bold {
            width: 24px;
            height: 24px;
            display: inline-block;
            white-space: nowrap;
            text-align: center;
            border: 1px solid #d6dfe5;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body ">
                <ul class="nav nav-tabs">
                    <li><a href="ProductList.aspx" aria-expanded="true"><%="商品列表".ToLang()%></a></li>
                    <li class="active"><a data-toggle="tab" href="#baseInfo" aria-expanded="true"><%=ProcessStr %> - <%="基本信息".ToLang()%></a></li>
                    <li><a data-toggle="tab" href="#attachInfo" aria-expanded="true"><%="附加信息".ToLang()%></a></li>
                    <li><a data-toggle="tab" href="#searchOption" aria-expanded="true"><%="搜选项".ToLang()%></a></li>
                    <li><a data-toggle="tab" href="#specificationIno" aria-expanded="true"><%="规格信息".ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div class="panel-body">
                    <div class="tab-content">
                        <div id="baseInfo" class="tab-pane active">
                            <div class="panel-body">
                                <form id="proBasicEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/Shop/ProductListForm.aspx">
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="divProName">
                                            <%="商品名称：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <input id="ProName" value="<%=ShopProductInfo.ProName %>" name="ProName" class="form-control" required="true"
                                                maxlength="200" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="divProNO">
                                            <%="商品编号：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <input id="ProNO" value="<%=ShopProductInfo.ProNO %>" name="ProNO" class="form-control" required="true"
                                                maxlength="20" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="divCategoryID">
                                            <%="商品分类：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <select id="CategoryID" style="display: inline-block;" name="CategoryID" class="form-control">
                                                <option value=""><%="==请选择==".ToLang()%></option>
                                                <% foreach (DataRow item in OptionTable.Rows)
                                                    {
                                                        var check = "";
                                                %>
                                                <% if (ShopProductInfo.CategoryID == item["CategoryID"].ToInt())
                                                    {
                                                        check = "selected=\"selected\"";
                                                    } %>
                                                <option <%=check %> value="<%=item["CategoryID"]%>">
                                                    <%=item["CategoryName"]%></option>
                                                <%  }   %>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 text-right" for="divIsAllowBuy">
                                            <%="可购买状态：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <ul class="list">
                                                <li>
                                                    <input type="radio" id="IsAllowBuy_True" name="IsAllowBuy" value="1" />
                                                    <label for="IsAllowBuy_True"><%="可购买".ToLang()%></label></li>
                                                <li>
                                                    <input type="radio" id="IsAllowBuy_False" name="IsAllowBuy" value="0" />
                                                    <label for="IsAllowBuy_False"><%="不可购买".ToLang()%></label></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="divCostAmount">
                                            <%="价格：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <div class="input-group">
                                                <input id="CostAmount" value="<%=ShopProductInfo.CostAmount %>" name="CostAmount" class="form-control" required="true"
                                                    maxlength="20" />
                                                <span class="input-group-addon"><%="元".ToLang()%></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="divImages">
                                            <%="商品图片：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <%=ProductMultiPicHtml %>
                                            <span class="note_gray"><%="第一张图片为商品主图片".ToLang()%></span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 text-right" for="divIsAttrPro">
                                            <%="是否拆分规格：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <ul class="list">
                                                <li>
                                                    <input type="checkbox" id="IsAttrPro" name="IsAttrPro" value="1" />&nbsp; <%="是".ToLang()%></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="form-group" id="attrList" style="display: none;">
                                        <div class="col-md-2 control-label" for="divIsAttrPro">
                                        </div>
                                        <div class="col-md-10 ">
                                            <ul class="list">
                                                <%if (SaList.Count > 0)
                                                    { %>
                                                <%for (int i = 0; i < SaList.Count; i++)
                                                    { %>
                                                <%if (ShopProductInfo.AttrIDs.ToStr().Contains(SaList[i].AttrID.ToStr()))
                                                    { %>
                                                <li>
                                                    <input type="checkbox" checked="checked" name="attrCheckbox" value="<%=SaList[i].AttrID %>" />&nbsp;<%=SaList[i].SearchName %></li>
                                                <%} %>
                                                <%else
                                                    { %>
                                                <li>
                                                    <input type="checkbox" name="attrCheckbox" value="<%=SaList[i].AttrID %>" />&nbsp;<%=SaList[i].SearchName %></li>
                                                <%} %>
                                                <%} %>
                                                <%} %>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-offset-2 col-md-10">
                                            <input type="hidden" name="_action" value="SaveProduct" />
                                            <input type="hidden" name="ProID" value="<%=ProId %>" />
                                            <input type="hidden" name="attrIDs" value="<%=ShopProductInfo.AttrIDs %>" />
                                            <button form-action="submit" form-success="refresh" class="btn btn-info">
                                                <%="保存并继续添加".ToLang()%></button>
                                            <button form-action="submit" form-success="edit" class="btn btn-info">
                                                <%="保存并继续编辑".ToLang()%></button>
                                            <a class="btn btn-white" href="productlist.aspx?filter=1"><%="返回".ToLang()%></a>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                        <div id="attachInfo" class="tab-pane ">
                            <div class="panel-body" id="proExtra" style="display: none;">
                                <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx">
                                    <%=GetHtml( new ShopField() { ShowType=6,  FieldName="ProDesc", FieldAlias="详情描述".ToLang() },ShopProductInfo.ProDesc) %>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="divMetaTitle">
                                            <%="SEO标题：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <input id="MetaTitle" value="<%=ShopProductInfo.MetaTitle %>" name="MetaTitle" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="divMetaKeyword">
                                            <%="SEO关键字：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <input id="MetaKeyword" value="<%=ShopProductInfo.MetaKeyword %>" name="MetaKeyword" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="divMetaDescription">
                                            <%="SEO描述：".ToLang()%>
                                        </div>
                                        <div class="col-md-10 ">
                                            <textarea name="MetaDescription" class="form-control" rows="5"><%=ShopProductInfo.MetaDescription %></textarea>
                                        </div>
                                    </div>
                                    <%=FieldHtml %>
                                    <div class="form-group">
                                        <div class="col-md-offset-2 col-md-10 ">
                                            <input type="hidden" name="_action" value="SaveExtraProduct" />
                                            <input type="hidden" name="ProID" value="<%=ProId %>" />
                                            <button form-action="submit" form-success="refresh" class="btn btn-info">
                                                <%="保存".ToLang()%></button>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                        <div id="searchOption" class="tab-pane ">
                            <div class="panel-body" id="searchOptionDiv" style="display: none;">
                                <form id="proSearchEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx">
                                    <%if (Searchlist.Count > 0)
                                        { %>
                                    <%foreach (var item in Searchlist)
                                        { %>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="divIsAllowBuy">
                                            <%=item.SearchName%>：
                                        </div>
                                        <div class="col-md-10 ">
                                            <ul class="list">
                                                <%  IList<ShopSearchValue> searchValuelist = ShopSearchValueService.Instance.GetSearchValueBySearchID(item.SearchID);%>
                                                <%if (searchValuelist.Count > 0)
                                                    { %>
                                                <%foreach (var itemValue in searchValuelist)
                                                    {%>
                                                <%if (ShopProductInfo.SearchValueIDs != null && ShopProductInfo.SearchValueIDs.Split(',').Contains(itemValue.SearchValueID.ToStr()))
                                                    { %>
                                                <li>
                                                    <input type="checkbox" name="SearchValueIDs" checked="checked" value="<%=itemValue.SearchValueID %>" />
                                                    <label><%=itemValue.SearchValueName %></label>
                                                </li>
                                                <%} %>
                                                <%else
                                                    { %>
                                                <li>
                                                    <input type="checkbox" name="SearchValueIDs" value="<%=itemValue.SearchValueID %>" />
                                                    <label><%=itemValue.SearchValueName %></label></li>
                                                <%} %>
                                                <%} %>
                                                <%} %>
                                            </ul>
                                        </div>
                                    </div>
                                    <%} %>
                                    <%} %>
                                    <div class="form-group">
                                        <div class="col-md-offset-2 col-md-10 ">
                                            <input type="hidden" name="_action" value="SaveProductSearchValue" />
                                            <input type="hidden" name="ProID" value="<%=ProId %>" />
                                            <button form-action="submit" form-success="refresh" class="btn btn-info">
                                                <%="保存".ToLang()%></button>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                        <div id="specificationIno" class="tab-pane ">
                            <div class="panel-body">
                                <form id="proSpecEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx">
                                    <div class="form-group">
                                        <div class="col-md-offset-2 col-md-10 ">
                                            <input type="hidden" name="_action" value="SaveSpecProduct" />
                                            <input type="hidden" name="ProID" value="<%=ProId %>" />
                                            <input type="hidden" name="AttrProList" value="" id="AttrProList" />
                                            <input type="hidden" name="ProAttrXml" value="" id="ProAttrXml" />
                                            <button form-action="submit" form-success="refresh" class="btn btn-info">
                                                <%="保存".ToLang()%></button>
                                            <a id="btnAddProSepc" href="javascript:;" class="btn btn-info">
                                                <%="添加规格商品".ToLang()%></a>
                                        </div>
                                    </div>
                                    <div class="row" id="divAttrProList">
                                    </div>

                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        //保存基本信息
        $('#proBasicEdit').Validator(null,
            function (el) {
                var actionSuccess = el.attr("form-success");
                var $form = $("#proBasicEdit");
                $form.post({
                    success: function (response) {
                        if (response.Status == true) {
                            actionSuccess == "refresh" ? whir.toastr.success("<%="操作成功".ToLang()%>", true, false, "product_edit.aspx") : whir.toastr.success("<%="操作成功".ToLang()%>", true, false, "product_edit.aspx?proid=" + response.Message);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    },
                    error: function (response) {
                        whir.toastr.error(response.Message);
                        whir.loading.remove();
                    }
                });
                return false;
            });
        //保存自定字段信息
        $('#formEdit').Validator(null,
            function (el) {
                var actionSuccess = el.attr("form-success");
                var $form = $("#formEdit");
                $form.post({
                    success: function (response) {
                        if (response.Status == true) {
                            actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "product_edit.aspx?proid=<%=ProId%>");
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    },
                    error: function (response) {
                        whir.toastr.error(response.Message);
                        whir.loading.remove();
                    }
                });
                return false;
            });
        //保存搜选项
        $('#proSearchEdit').Validator(null,
            function (el) {
                var actionSuccess = el.attr("form-success");
                var $form = $("#proSearchEdit");
                $form.post({
                    success: function (response) {
                        if (response.Status == true) {
                            actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "product_edit.aspx?proid=<%=ProId%>");
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    },
                    error: function (response) {
                        whir.toastr.error(response.Message);
                        whir.loading.remove();
                    }
                });
                return false;
            });
        //保存规格商品
        $('#proSpecEdit').Validator(null,
            function (el) {
                var actionSuccess = el.attr("form-success");
                var $form = $("#proSpecEdit");
                SubmitAttrProList();//规格商品xml
                $form.post({
                    success: function (response) {
                        if (response.Status == true) {
                            actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "product_edit.aspx?proid=<%=ProId%>");
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    },
                    error: function (response) {
                        whir.toastr.error(response.Message);
                        whir.loading.remove();
                    }
                });
                return false;
            });

        //规格
        var attrIDs = '';
        //搜选项值
        var searchValueIds = '';
        $(document).ready(function () {
            var isAllowBuy = '<%=ShopProductInfo.IsAllowBuy?"1":"0"%>';
               if (isAllowBuy == "1")
                   $("#IsAllowBuy_True").iCheck("check");
               else
                   $("#IsAllowBuy_False").iCheck("check");

               if ('<%=ShopProductInfo.AttrIDs%>' != '') {
                $("#attrList").show();

                $("input[name='IsAttrPro']").each(function () {
                    $(this).parent().attr("class", "icheckbox_flat-red checked");
                });
            } else {
                $("#attrList").hide();
                //$("#proExtra").hide();
            }
            if ('<%=ProId%>' != '0') {
                $("#proExtra").css("display", "block");
                $("#searchOptionDiv").css("display", "block");
            }
            $("input[name='IsAttrPro']").each(function () {
                $(this).next().click(function () {
                    if ($("#attrList").attr("style") == "display: none;") {
                        $("#attrList").show();
                    } else {
                        $("#attrList").hide();
                    }
                });
            });
            //规格click事件
            $("input[name='attrCheckbox']").each(function () {
                $(this).next().click(function () {
                    attrIDs = "";
                    $("input[name='attrCheckbox']").each(function () {
                        if ($(this).next().prev().is(":checked")) {
                            attrIDs += $(this).val() + ',';
                        }
                    });

                    $("input[name='attrIDs']").val(attrIDs);
                });
            });
            //显示附加信息
            if ('<%=ShopProductInfo.ProID%>' != '0') {
                $("#proSpec").show();
            }
            //添加规格商品
            $("#btnAddProSepc").click(function () {
                var url = '<%=SysPath%>Plugin/shop/common/shopattrvalueselect.aspx?callback=getAttrValueAndShow&proid=<%=ProId %>';
                whir.dialog.frame('<%="请选择".ToLang()%>', url, 'id', 600, 300);
            });
           });
           //获取选中规格值
           function getAttrValueAndShow(attrValue) {
               var assemble = printCombination(attrValue);
               for (var i = 0; i < assemble.length; i++) {
                   var item = assemble[i].split(',');
                   if (item.length > 0) {
                       var names = '';
                       var ids = '';
                       for (var j = 0; j < item.length; j++) {
                           if (item[j] != "" && item[j].split('*').length > 1) {
                               names += item[j].split('*')[0] + " ";
                               ids += item[j].split('*')[1] + ",";
                           }
                       }
                       if (ids.length > 0) {
                           ids = ids.substring(0, ids.length - 1);
                       }
                       addAttrPro(names, ids, '', '', 0);
                   }
               }
           }
           //解析数组，返回数组元素组合
           function printCombination(temp) {
               var arrLen;   //versions数组长度
               var comCollction = new Array(); //versions数组产生的所有组合值
               arrLen = temp.split(';').length;
               var versions = new Array(arrLen);
               for (var i = 0; i < arrLen; i++) {
                   versions[i] = temp.split(';')[i].split(',');
               }
               if (arrLen < 2) {
                   return temp.split(',');
               }
               var k;
               var curCount = 0;
               var len0 = versions[0].length;
               //初始化第一个数组，即用versions第一行值生成数组arr0i
               for (var i = 0; i < len0; i++) {
                   eval("var arr" + 0 + i + "= new Array()");
                   eval("arr" + 0 + i)[0] = versions[0][i] + ",";

               }
               //循环versions的剩余行 （1至arrLen）
               for (var m = 1; m < arrLen; m++) {
                   //循环arr0i
                   for (var n = 0; n < len0; n++) {
                       k = 0;
                       curCount = 0;
                       eval("var arr" + m + n + "= new Array()");
                       while (true) {
                           eval("var temp=arr" + (m - 1) + n + "[k]");
                           //数组arr(m-1)n[k]的值未定义时，跳出
                           if (eval("temp==undefined")) {
                               break;
                           }
                           //versions数组的第m行的每个值与arr(m-1)n[k]连接，保存到新数组arrmn[cur_count]
                           for (var j = 0; j < versions[m].length; j++) {
                               eval("arr" + m + n)[curCount++] = eval("arr" + (m - 1) + n)[k] + versions[m][j] + ","; //此处可添加分隔符
                           }

                           k++;
                       }
                   }
                   if (m == arrLen - 1) { //最后一轮循环结束时，将产生的所有组合赋值给全局数组comCollction
                       var cnt = 0;
                       for (var w = 0; w < len0; w++) {
                           for (var t = 0; t < eval("arr" + m + w).length; t++) {
                               //alert(eval("arr"+m+w)[t]);
                               comCollction[cnt++] = eval("arr" + m + w)[t];
                           }
                       }

                   }
               }
               return comCollction;
           }
           //添加规格
           function addAttrPro(names, ids, price, isusemainimage, attrproid, images, initialPreview, initialPreviewConfig) {
               var tempid = ids.replace(/,/g, "");
               var idsAsc = ids.split(',').sort(function (a, b) {
                   return a < b ? 1 : -1;
               });
               if ($("#divAttrProList :hidden").is("input[value='" + ids + "']") || $("#divAttrProList :hidden").is("input[value='" + idsAsc.toString() + "']"))
                   return;
               var tableHtml = '';
               tableHtml += '<div class="col-md-12" id="div' + tempid + '">';
               tableHtml += '<ul class="list-group ">';
               tableHtml += '<li class="list-group-item"><a href="javascript:removeAttrPro(\'div' + tempid + '\');" class="fontawesome-trash text-danger border-danger">&nbsp;<%="移除此规格".ToLang()%></a></li>';
               tableHtml += '<li class="list-group-item padding-left30 ">';

               tableHtml += ' <div class="form-group">';
               tableHtml += ' <label class="col-sm-3 control-label"><%="商品后缀：".ToLang()%></label>';
               tableHtml += ' <div class="col-sm-9" style="line-height: 34px;">';
               tableHtml += '<input type="hidden" value="' + ids + '" id="hidAttrId' + tempid + '" />';
               tableHtml += '<span style="color: red;"  id="spanAttrId' + tempid + '">' + names + '</span><span style="color: Gray;"><%="（根据所选规格自动生成）".ToLang()%></span>';
               tableHtml += '</div></div>';

               tableHtml += '<div class="form-group">';
               tableHtml += ' <label class="col-sm-3 control-label"><%="价格：".ToLang()%></label>';
               tableHtml += '<div class="col-sm-9">';
               tableHtml += ' <div class="input-group">';
               tableHtml += '<input type="text" id="hidAmount' + tempid + '"  class="form-control" maxlength="12" value="' + (price == "" ? "0.00" : price) + '" />';
               tableHtml += '<span class="input-group-addon"> <%="元".ToLang()%></span>';
               tableHtml += '</div></div></div>';

               tableHtml += '<div class="form-group">';
               tableHtml += '<label class="col-sm-3 control-label"><%="规格图片：".ToLang()%></label>';
               tableHtml += '<div class="col-sm-9">';
               tableHtml += '<input id="txt_' + tempid + '" for="hidImage' + tempid + '"  name="txt_' + tempid + '" type="file" class="file-loading" multiple accept="<%=AcceptPicType %>" />';
               tableHtml += '<ul class="list" style="padding-top:10px;"><li><input type="checkbox" ' + (isusemainimage == "" ? "" : "checked=\"checked\"") + '  id="chk' + tempid + '"/>';
               tableHtml += '<span style="padding-left:15px;" ><%="同时延用左侧基本信息的图片".ToLang()%></span > </li></ul>';
               tableHtml += '<input type="hidden" id="hidImage' + tempid + '" name="hidImage' + tempid + '" value="' + (images || "") + '" />';
               tableHtml += '<input type="hidden" id="hidAttrProId' + tempid + '" value="' + attrproid + '" />&nbsp;';
               tableHtml += '</div></div>';

               tableHtml += '</li></ul></div>  <div class="space10"></div>';

               $("#divAttrProList").append(tableHtml);
               whir.skin.checkbox();
               regeditUploadFile(tempid, initialPreview, initialPreviewConfig);

           }
                 //移除规格
                 function removeAttrPro(id) {
                     $("#" + id).remove();
                 }
                 //提交规格商品时将数据以xml填充入隐藏文本
                 function SubmitAttrProList() {
                     var list = "<attrprolist>";
                     $("#divAttrProList .col-md-12").each(function () {
                         list += "<attrpro>";
                         //规格名称
                         list += "<attrvaluenames>" + $(this).find("[id^='spanAttrId']").html() + "</attrvaluenames>";
                         //规格值
                         list += "<attrvalueids>" + $(this).find("[id^='hidAttrId']").val() + "</attrvalueids>";
                         //价格
                         list += "<costamount>" + $(this).find("[id^='hidAmount']").val() + "</costamount>";
                         //延用左侧基本信息的图片 $("#cbxSubColumns:checked").length;
                         list += "<isusemainimage>" + $(this).find("[id^='chk']").prop('checked') + "</isusemainimage>";
                         //图片信息
                         list += "<images>" + $(this).find("[id^='hidImage']").val() + "</images>";
                         //主图 默认第一张
                         list += "<proimg></proimg>";

                         //属性商品ID
                         list += "<attrproid>" + $(this).find("[id^='hidAttrProId']").val() + "</attrproid>";
                         list += "</attrpro>";
                     });
                     list += "</attrprolist>";
                     $("#ProAttrXml").val(list);

                     return true;
                 }

        <%=ScriptAttrPro %>

        //注册上传控件
                 function regeditUploadFile(id, initialPreview, initialPreviewConfig) {
                     $("#txt_" + id)
                         .fileinput({
                             language: '<%=GetLoginUserLanguage()%>',
                             uploadUrl: '<%=SysPath%>Ajax/Extension/UploadImages_Form.aspx?formid=0&controlID=txt_' + id + '&image=',
                             previewFileType: "image",
                             allowedFileExtensions: [<%=AllowPicType %>],
                             initialCaption: '<%="第一张图片为主图".ToLang()%>',
                             previewClass: "bg-warning",
                             initialPreviewAsData: true,
                             overwriteInitial: false,
                             initialPreview: eval(initialPreview),
                             initialPreviewConfig: eval(initialPreviewConfig != undefined ? initialPreviewConfig.replace(/&1/g, "{").replace(/&2/g, "}") : ""),
                             initialPreviewFileType: 'image',
                             pickerUrl: '<%=SysPath%>/ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=checkbox&HidChooseId=hidImage' + id + '+&ControlId=txt_' + id,
                             isPic: true,
                         }).on("filebatchselected",
                         function (event, files) {
                             $(this).fileinput("upload");
                         })
                         .on("fileuploaded",
                         function (event, data) {
                             if (data.response && data.response.Result == true) {
                                 var tempVal = $("#hidImage" + id).val();
                                 if (tempVal != '' && tempVal != '*')
                                     $("#hidImage" + id).val($("#hidImage" + id).val() + '*' + data.response.Msg);
                                 else
                                     $("#hidImage" + id).val(data.response.Msg);
                             } else {
                                 whir.toastr.error(data.response.Msg);
                             }
                         });
                 }
    </script>
</asp:Content>
