<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="ConsultDetail.aspx.cs" Inherits="Whir_System_Plugin_shop_product_Consult_ConsultDetail" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap" id="ConsultDetail-wrap">
        <div class="panel-body">
            <div class="row">
                <div class="col-md-6">
                    <div class="panel panel-default ">
                        <div class="panel-heading">
                            <%="商品信息".ToLang()%>
                        </div>
                        <div class="panel-body ">
                            <div class="form-group">
                                <div class="col-md-4 control-label">
                                    <%="商品名称：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <label><%=ShopProductInfo.ProName %></label>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4 control-label">
                                    <%="商品图片：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <label>
                                        <img src="<%=UploadFilePath+ ShopProductInfo.ProImg %>" style="max-width: 160px; max-height: 160px" /></label>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4 control-label">
                                    <%="咨询时销售价格：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <label><%=ShopConsultInfo.ConsultSaleAmount %></label>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4 control-label">
                                    <%="销售价格：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <label><%=ShopProductInfo.CostAmount%></label>
                                </div>
                                <div class="clear"></div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="panel panel-default ">
                        <div class="panel-heading">
                            <%="咨询信息".ToLang()%>
                        </div>
                        <div class="panel-body ">
                            <div class="form-group">
                                <div class="col-md-3 control-label">
                                    <%="咨询人：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <label><%=RequestString("ConsultUser") %></label>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 control-label">
                                    <%="咨询内容：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <label><%=ShopConsultInfo.Consult %></label>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 control-label">
                                    <%="回复内容：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <label>
                                        <textarea name="replyContent" id="replyContent" class="form-control" rows="6" cols="500" maxlength="250"><%=ShopConsultInfo.Reply %></textarea></label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 control-label">
                                    <%="回复时间：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <label><%=ShopConsultInfo.ReplyDate %></label>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 control-label">
                                    <%="回复人：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <label><%=ShopConsultInfo.ReplyUser %></label>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 control-label">
                                    <%="是否审核：".ToLang()%>
                                </div>
                                <div class="col-md-9 ">
                                    <label>
                                        <input type="checkbox" name="checkAudit" id="checkAudit" /></label>
                                </div>
                                <div class="clear"></div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        $(function () {
            var ischeck = <%=ShopConsultInfo.State%>;
            if (ischeck == 1)
                $("#checkAudit").iCheck("check");
            else
                $("#checkAudit").iCheck("false");
            $("[name=_dialog] .btn-primary", parent.document).click(function () {
                var consultId = '<%=ShopConsultInfo.ConsultID%>';
                var check = $("#checkAudit").is(':checked');
                if (check) {
                    check = 1;
                }
                var content = $("#replyContent").val();
                if (content == "" || content == null || content == undefined) {
                    whir.toastr.error("<%="回复内容不能为空".ToLang()%>");
                    return;
                }
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/ConsultForm.aspx",
                    {
                        data: {
                            _action: "Reply",
                            consultId: consultId,
                            reptyContent: content,
                            repty: check
                        },
                        success: function (response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                window.parent.whir.toastr.success(response.Message);
                                window.parent.whir.dialog.remove();
                            } else {
                                window.parent.whir.toastr.error(response.Message);
                            }
                        }
                    }
                );

            });
        });


    </script>
</asp:Content>

