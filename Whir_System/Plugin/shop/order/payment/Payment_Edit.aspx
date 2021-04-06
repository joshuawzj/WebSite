<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="payment_edit.aspx.cs" Inherits="whir_system_Plugin_shop_order_payment_payment_edit" %>
<%@ Import Namespace="Whir.Language" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="PaymentList.aspx" aria-expanded="true"><%="支付方式管理".ToLang()%></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%="编辑支付方式".ToLang() %></a></li>
                </ul>
                <div class="space15"></div>
                <form id="formEdit" form-url="<%=SysPath%>Handler/Plugin/shop/PaymentForm.aspx">
                    <div class="form_center form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 control-label">
                                <%="支付方式名称：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" name="Name" value="<%=Name %>" data-toggle="tooltip" title="<%="前台用户支付时显示的名称，例如支付宝，线下支付。".ToLang()%>"
                                    class="form-control" required="true" minlength="1" maxlength="50" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 text-right">
                                <%="是否启用：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <ul class="list">
                                    <li>
                                        <input type="checkbox" id="IsStart" name="IsStart" value="1" /><span data-toggle="tooltip" title="<%="支付方式至少要有一项处于启动状态".ToLang()%>">&nbsp;<%="开启".ToLang()%> </span>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <%--<div class="form-group">
                            <div class="col-md-2 control-label">
                                <%="合作伙伴号：
                            </div>
                            <div class="col-md-10 ">
                                   <input type="text"  name="AccountNumber" value="" data-toggle="tooltip"  class="form-control" title="<%="接口公司给您的商户号，有些叫客户号。"   maxlength="128" />
                                 
                            </div>
                        </div>--%>
                        <div class="form-group" runat="server" id="tr1">
                            <div class="col-md-2 control-label" runat="server" id="trAccount">
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" name="Account" value="<%=Account %>" required="true"
                                    data-toggle="tooltip" title="<%="有些支付方式需要商户填写卖家账户才能支付,例如支付宝。".ToLang()%>" class="form-control" maxlength="64" />
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="tr2">
                            <div class="col-md-2 control-label" runat="server" id="trMd5Key">
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" name="Md5Key" value="<%=Md5Key %>" required="true" class="form-control"
                                    data-toggle="tooltip" title="<%="接口公司给您的私钥，有些叫MD5私钥。".ToLang()%>" maxlength="128" />
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="trAlipay" visible="false">
                            <div class="col-md-2 control-label">
                                <%="支付宝账户：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" name="AlipayAccount" value="<%=AlipayAccount %>" required="true"
                                    data-toggle="tooltip" title="<%="支付宝账号或卖家支付宝账户".ToLang()%>"
                                    class="form-control" maxlength="64" />
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="trurl1" visible="false">
                            <div class="col-md-2 control-label">
                                <%="返回地址：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" name="ReturnUrl" value="<%=ReturnUrl %>" required="true"
                                    data-toggle="tooltip" title="<%="支付完成后的返回地址，如http://www.***.com".ToLang()%>" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="trurl2" visible="false">
                            <div class="col-md-2 control-label">
                                <%="异步通知地址：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" name="NotifyUrl" value="<%=NotifyUrl %>" required="true"
                                    data-toggle="tooltip" title="<%="支付完成的异步通知，如http://www.***.com".ToLang()%>" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label">
                                <%="接口类型：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <select id="ddlPaymentMode" name="PaymentMode" class="form-control" disabled>
                                    <option value="99bill">快钱支付</option>
                                    <option value="alipay">支付宝</option>
                                    <option value="alipayinstant">支付宝实时到账</option>
                                    <option value="alipaywap">支付宝手机H5支付</option>
                                    <option value="chinabank">网银在线</option>
                                    <option value="chinapay">银联在线</option>
                                    <option value="cncard">云网支付</option>
                                    <option value="ipay">中国在线支付网</option>
                                    <option value="ips">上海环迅</option>
                                    <option value="tenpay">财付通</option>
                                    <option value="xpay">易付通</option>
                                    <option value="cod">货到付款</option>
                                    <option value="wechatpay">微信扫码支付</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label">
                                <%="描述：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <textarea id="txtDescn" name="Descn" rows="4" class="form-control"
                                    data-toggle="tooltip" title="<%="长度限制在255个字符以内".ToLang()%>" maxlength="255"><%=Descn%></textarea>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="id" value="<%=ItemId %>" />
                                <input type="hidden" name="_action" value="Save" />
                                <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                <a class="btn btn-white" href="paymentlist.aspx"><%="返回".ToLang()%></a>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        var options = {
            fields: {
                Name: {
                    validators: {
                                               regexp: {
                            regexp: /^[a-zA-Z0-9\u4e00-\u9fbb]{1,50}/,
                            message: '<%="支付方式名称长度限制在50个字符以内，可输入中文、英文及数字".ToLang()%>'
                        }
                    }
                },
                Account: {
                    validators: {
                        notEmpty: {
                            message: '必填项'
                        }
                    }
                },
                Md5Key: {
                    validators: {
                        notEmpty: {
                            message: '必填项'
                        }
                    }
                },
                AlipayAccount: {
                    validators: {
                        notEmpty: {
                            message: '必填项'
                        }
                    }
                },
                ReturnUrl: {
                    validators: {
                        notEmpty: {
                            message: '必填项'
                        }
                    }
                },
                NotifyUrl: {
                    validators: {
                        notEmpty: {
                            message: '必填项'
                        }
                    }
                },
                Descn: {
                    validators: {
                        notEmpty: {
                            message: '必填项'
                        }
                    },
                    regexp: {
                        regexp: /[^<|>]/,
                        message: '<%="不能出现“<”、“>”符号".ToLang()%>'
                    }
                }
            }
        };

        $(function () {
            var IsStart = "<%=IsStart%>";
                var PaymentMode = "<%=PaymentMode%>";
                if (IsStart == "1")
                    $("#IsStart").iCheck("check");
                $("#ddlPaymentMode").val(PaymentMode);
            });

            $('#formEdit').Validator(options,
                 function (el) {
                     var actionSuccess = el.attr("form-success");
                     var $form = $("#formEdit");

                     $form.post({
                         success: function (response) {
                             if (response.Status == true) {
                                 if (actionSuccess == "back")
                                     whir.toastr.success(response.Message, true, false, "paymentlist.aspx");
                                 else
                                     whir.toastr.success(response.Message, true);
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
    </script>

</asp:Content>
