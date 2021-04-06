<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SiteMap.aspx.cs" Inherits="Whir_System_Module_Developer_SiteMap" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <style>
        .form_center
        {
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        
        $(function () {
           

            $("#rblType :radio").click(function () {
                location.href = "<%=SysPath %>module/developer/sitemap.aspx?type=" + $(this).val();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel panel-default">
            <div class="panel-body">
                <div class="form_center">
                    <div class="alert alert-danger no-opacity">
                        <button data-dismiss="alert" class="close" type="button">×</button>
                        <span class="entypo-attention"></span>
                        <span>
                            <%="1.网站地图根据后台栏目结构生成".ToLang() %><br />
                            <%="2.每个栏目的链接地址与绑定的模板生成关联，若没有绑定模板则需要手动更改其链接地址".ToLang() %><br />
                            <%="3.所得到的代码不一定适合前台页面的样式".ToLang() %>
                        </span>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading"><%="操作".ToLang()%></div>
                        <div class="panel-body">
                            <div class="form_center">
                                <form id="formEdit" class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-2 text-right" for="rblType">
                                        <%=" 呈现方式：".ToLang()%>
                                    </div>
                                    <div class="col-md-10 ">
                                        <input type="hidden" id="_action" name="_action" value="GetData"/>
                                        <ul class="list" id="text1">
                                            <li>
                                                <input type="radio" checked="checked" id="rblType_True" name="rblType" value="0" />
                                                <label for="rblType_True">栏目名称</label>
                                            </li>
                                            <li>
                                                <input type="radio" id="rblType_False" name="rblType" value="1" />
                                                <label for="rblType_False">栏目别名</label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading"><%="代码".ToLang()%></div>
                        <div class="panel-body">
                            <div class="form_center">
                                <div class="form-group row">
                                    <div class="col-md-2 text-right" for="rblType">
                                        <%=" 静态方式：".ToLang()%>
                                    </div>
                                    <div class="col-md-10 ">
                                        <textarea class="form-control" rows="10" id="txtContent" onchange="clip.setText(this.value)"></textarea>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-md-offset-4 col-md-6">
                                       
                                    </div>
                                    <div class="col-md-2">&nbsp;</div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-md-2 text-right" for="rblType">
                                        <%=" 动态方式：".ToLang()%>
                                    </div>
                                    <div class="col-md-10 ">
                                        <textarea id="txtDynamic" class="form-control" rows="10" onchange="clip2.setText(this.value)"></textarea>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-md-offset-4 col-md-6">
                                    
                                    </div>
                                    <div class="col-md-2">&nbsp;</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <%="效果预览".ToLang() %></div>
                        <div class="panel-body">
                            <div class="form_center">
                                <div class="form-group">
                                    <div class="col-md-10 " id="divContent">

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        //选择事件
        $(document).ready(function () {
            $("input[name='rblType']").next().click(function () {
                Change();

            });
            $("input[name='rblType']").parent().next().click(function () {
                Change();

            });
        });

        function Change() {
            $("#formEdit")
                .post({
                    url: "<%=SysPath%>Handler/Developer/SiteMap.aspx",
                    success:
                        function (response) {
                            $("#txtContent").val("");
                            $("#txtDynamic").val("");
                            $("#divContent").html("");
                            if (response.Status === true) {
                                var result = response.Message.split('|@|');
                                $("#txtContent").val(result[0] ? result[0] : "");
                                $("#txtDynamic").val(result[1] ? result[1] : "");
                                $("#divContent").html(result[0] ? result[0] : "");
                            } else {
                                whir.toastr.error(response.Message);
                            }
                            whir.loading.remove();
                            return false;
                        }
                });
            return false;
        }

        Change();
    </script>
</asp:Content>
