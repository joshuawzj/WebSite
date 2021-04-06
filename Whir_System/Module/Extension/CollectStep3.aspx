<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="CollectStep3.aspx.cs" Inherits="Whir_System_Module_Extension_CollectStep3" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        h3
        {
            text-align: center;
            font-size: 12px;
        }

            h3 span
            {
                margin: 0px 10px 0px 10px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
             <div class="panel-heading" align='center'>
                <span><%="第一步：基本设置".ToLang()%></span>&nbsp;
                <i class="fa fa-lg fa-angle-right"></i>&nbsp;
               <span><%="第二步：列表页规则设置".ToLang()%></span>&nbsp;
                <i class="fa fa-lg fa-angle-right"></i>&nbsp;
                 <span  class="text-danger"><b><%="第三步：内容页规则设置".ToLang()%></b></span>&nbsp;
                <i class="fa fa-lg fa-angle-right"></i>&nbsp;<span><%="完成".ToLang()%></span>
            </div>
            <div class="panel-body">
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Extension/Collect.aspx">
                        <div class="form-group">
                            <div class="col-md-2 text-right"><%="项目名称".ToLang()%></div>
                            <div class="col-md-10 "><b>
                                <asp:Label ID="lblItemName" runat="server"></asp:Label></b></div>
                        </div>
                        <asp:Repeater ID="rptList" runat="server">
                            <ItemTemplate>
                                <div class="form-group">
                                    <div class="col-md-2 control-label" for="<%#Eval("FormId")%>">
                                        <%#Eval("FieldAlias")%>
                                    </div>
                                    <div class="col-md-5">
                                        <div class="input-group collectType">
                                            <span class="input-group-addon" style="width: 50%;">
                                                <input type="radio" class="defaultradio" id="radDefault<%#Eval("FormId")%>" name="<%#Eval("FormId")%>" checked='checked'   value="1" />
                                                <label class="radio-label" for="radDefault<%#Eval("FormId")%>"><%="指定默认值".ToLang()%></label>
                                            </span>
                                            <input type="text" class="form-control defaultvalue" style="width: 50%; height:36px" value="<%#Eval("DefaultValue")%>" formid="<%#Eval("FormId")%>" />
                                        </div>
                                    </div>
                                    <div class="col-md-5">
                                        <div class="input-group collectType">
                                            <span class="input-group-addon">
                                                <input type="radio" class="ruleradio" id="radRule<%#Eval("FormId")%>" name="<%#Eval("FormId")%>" value="2" />
                                                <label class="radio-label" for="radRule<%#Eval("FormId")%>"><%="使用采集规则".ToLang()%></label>
                                            </span>
                                            <input type="hidden" class="form-control defaultvalue" value />
                                            <a class="btn btn-white input-group-addon" onclick="openSetRule(<%#Eval("FormId") %>,<%=CollectId %>)" disabled="disabled"><%="设置采集规则".ToLang()%></a>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div class="button_submit_div text-center">
                            <input type="hidden" name="_action" value="Finished" />
                            <input type="hidden" name="hidValue" value="" />
                            <input type="hidden" name="CollectId" value="<%=CollectId %>" />
                            <div class="btn-group">
                                <a class="btn btn-info" href="CollectStep2.aspx?collectid=<%=CollectId %>"><%="上一步".ToLang()%></a>
                                <button form-action="submit" form-success="refresh" class="btn btn-info"><%="完成".ToLang()%></button>
                            </div>
                            <a class="btn text-danger border-danger" href="CollectList.aspx"><%="返回采集管理".ToLang()%></a>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            var data = <%=GetEditInfo() %>;
            for (var i = 0; i < data.length; i++) {
                if (data[i].CollectType == 2) {//选择值类型
                    $("input[name='" + data[i].FormId + "'][value='2']").iCheck("check");
                }
                //默认值
                $(" .defaultvalue[formid='" + data[i].FormId + "']").val(data[i].DefaultValue);
            }
         
            //指定默认值点击
            $(".defaultradio").next().click(function(){
                if($(this).prev().prop("checked")){
                    var name=$(this).prev().attr("name");
                    var ruleradio=  $("input[name='"+name+"'].ruleradio");
                    $(ruleradio).parent().parent().parent().find("a").attr("disabled","disabled");
                    $(this).parent().parent().parent().find(".defaultvalue").removeAttr("disabled");
                }
            });
            //设置规则点击
            $(".ruleradio").next().click(function(){
                if($(this).prev().prop("checked")){
                    var name=$(this).prev().attr("name");
                    var defaultradio=  $("input[name='"+name+"'].defaultradio");
                    $(defaultradio).parent().parent().parent().find(".defaultvalue").attr("disabled","disabled");
                    $(this).parent().parent().parent().find("a").removeAttr("disabled");
                }
            });
            $(".ruleradio:checked").next().click();

        });
        
        ///获取保存数据
        function GetDataStr() {
            var str = "";
            $(".collectType").each(function () {
                if( $(this).find("input[type='radio']").prop("checked"))
                str += $(this).find("input[type='radio']").attr("name") + "§" + $(this).find("input[type='radio']").val() + "§" +  $(this).find(".defaultvalue").val() + "□";
            });
            $("[name=hidValue]").val(str);
        }

        function openSetRule(formid, collectid) {

            var opts = {
                title: '<%="设置采集规则".ToLang()%>',
                content: '',
                ok: function (dialog) {
                },
                cancel: function (dialog) { dialog.close(); },
                okText: '<%="确定".ToLang()%>',
                cancelText: '<%="关闭".ToLang()%>',
                showOk: true,
                showCancel: true,
                iframe: {
                    url: "CollectFieldSetRule.aspx?formid=" + formid + "&collectid=" + collectid,
                    width: 1000,
                    height: 550,
                    scroll: true
                },
                zIndex: 1003
            };
            whir.dialog.show(opts);


           
        }

        //提交内容
        $("[form-action='submit']").click(function() {
            GetDataStr();
            var actionSuccess = $(this).attr("form-success");
            var $form = $("#formEdit");
            $form.post({
                success: function(response) {
                    if (response.Status == true) {
                        whir.toastr.success(response.Message);
                    } else {
                        whir.dialog.alert(response.Message);
                    }
                    whir.loading.remove();
                },
                error: function(response) {
                    whir.toastr.error(response.Message);
                    whir.loading.remove();
                }
            });
            return false;
           
        });
         
    </script>
</asp:Content>
