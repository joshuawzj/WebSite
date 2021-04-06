<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ezEIP_Login.aspx.cs" Inherits="whir_system_ezEIP_Login" %>

<%@ Import Namespace="Whir.Framework" %>

<!DOCTYPE html ">
<html lang="zh-cn">
<head>
<meta charset="utf-8" />
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title><%=AppSettingUtil.GetString("ProductName")%></title>
    <link href="<%=SystemPath %>Res/assets/css/style.css" rel="stylesheet" type="text/css" />
    <link href="<%=SystemPath %>res/assets/css/loader-style.css" rel="stylesheet" type="text/css" />
    <link href="<%=SystemPath %>res/assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="<%=SystemPath %>res/assets/css/signin.css" rel="stylesheet" type="text/css" />
    <link href="<%=SystemPath %>res/assets/js/iCheck/flat/all.css" rel="stylesheet" type="text/css" />
    <link href="<%=SystemPath %>Res/assets/js/toastr-master/toastr.css" rel="stylesheet" type="text/css" />
    <link href="<%=SystemPath %>res/css/css_whir_V450.css" rel="stylesheet" type="text/css" />
    <script src='<%=SystemPath %>res/js/jquery.min.js' type="text/javascript"></script>
    <script src='<%=SystemPath %>res/js/base64.js' type="text/javascript"></script>
    <script src="<%=SystemPath %>Res/assets/js/toastr-master/toastr.js" type="text/javascript"></script>
    <script src="<%=SystemPath %>res/js/Whir/whir.ajax.js" type="text/javascript"></script>
    <script src="<%=SystemPath %>res/js/Whir/whir.ui.js" type="text/javascript"></script>
    <script src="<%=SystemPath %>Res/assets/js/iCheck/jquery.icheck.js" type="text/javascript"></script>

</head>
<body>
    <div class="wap-login-wrapper">
        <div id="login-wrapper">
            <div class="row">
                <div class="col-md-4 col-md-offset-4">
                    <div id="logo-login">
                        <h1>光盐财经后台系统 <span><%=AppSettingUtil.GetString("Version")%></span>
                        </h1>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 col-md-offset-4">
                    <div class="account-box">
                        <form role="form" id="aspnetForm">
                            <div class="form-group">
                                <label>用户名</label>
                                <input type="text" id="txtUserName" name="UserName" class="form-control">
                            </div>
                            <div class="form-group">
                                <label>密码</label>
                                <input type="password" id="txtPassword" name="Password" onfocus="this.value=''" class="form-control">
                            </div>
                            <div class="form-group" style="display: none" id="divCode">
                                <label>验证码</label>
                                <div class="input-group">
                                    <input type="text" name="Code" id="txtCode" class="form-control" autocomplete="off" maxlength="4" />
                                    <span class="login-checkcode input-group-addon">
                                        <img id="imgCode" alt="看不清楚，请点击换一张." src="checkcode.ashx" onclick="this.src=this.src+'?'" />
                                    </span>
                                </div>
                            </div>
                            <div class=" pull-left">
                                <ul class="list">
                                    <li>
                                        <input type="checkbox" id="Remember" name="Remember" /><label> 记住用户名</label></li>
                                </ul>
                            </div>
                            <input type="button" class="btn btn-primary pull-right" id="btnLogin" onclick="submitLogin();" value="登录" />
                        </form>
                        <div>
                            <div class="row"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="space15"></div>
        <div class="text-center">
            <h6 style="color: #fff;">Powered by WHIR</h6>
        </div>
    </div>


    <%if (SystemConfig.SystemLoginBgType == 0)
      {%>
    <div class="login-bg"></div>
    <div class="map login-map">
        <div style="width: 100%; height: 100%;" id="allmap"></div>
    </div>
    <script type="text/javascript" src="http://api.map.baidu.com/api?key=&v=1.1&services=true"></script>
    <script type="text/javascript">

        var map = new BMap.Map("allmap");
        var locationPoint = new BMap.Point(<%=SystemConfig.SystemLoginBgForMap%>);
        var centerPoint =  new BMap.Point(locationPoint.lng, locationPoint.lat+0.005);//偏离量0.005，避免登录窗口挡住中心点
        map.centerAndZoom(centerPoint, 16);
        var mkr = new BMap.Marker(locationPoint); map.addOverlay(mkr);

    </script>
    <%}
      else if (SystemConfig.SystemLoginBgType == 1)
      {%>
    <div class="login-custom" style="background: url('<%=UploadFilePath+ SystemConfig.SystemLoginBgForCustom %>') no-repeat center center"></div>

    <%}
      else
      {%>
    <div class="login-banner">
        <div class="bd">
            <ul>
                <li style="background: url('http://area.sinaapp.com/bingImg') no-repeat center center"></li>
                <li style="background: url('http://area.sinaapp.com/bingImg?daysAgo=1') no-repeat center center"></li>
                <li style="background: url('http://area.sinaapp.com/bingImg?daysAgo=2') no-repeat center center"></li>
            </ul>
        </div>
    </div>
    
    <script type="text/javascript" src="<%=SystemPath %>/res/js/jquery.SuperSlide.2.1.1.js"></script>
    <script type="text/javascript">
        jQuery(".login-banner").slide({ effect: "fold", mainCell: ".bd ul", autoPlay: true, delayTime: <%=SystemConfig.SystemLoginBgForRandom%> });
    </script>

    <%} %>
    <script type="text/javascript">

        function submitLogin() {

            var userName = $("#txtUserName").val();
            var password = $("#txtPassword").val();
            var verifiyCode = $("#txtCode").val();
            var isRemember = '1';
            if ($("[name='Remember']").prop("checked"))
                isRemember = "1";
            else
                isRemember = "0";
            if (userName == "" || password == "") {
                whir.toastr.warning("请填写用户名、密码");
                return;
            }
            //加密后再验证
            var b = new Base64();  
            password = b.encode(password);  
            whir.ajax.post('<%=SystemPath%>Handler/Security/User.aspx', {
                data: {
                    _action: "Login",
                    userName: userName,
                    password: password,
                    verifiyCode: verifiyCode,
                    isRemember: isRemember
                },
                success: function (result) {
                    if (result.Status) {
                        //判断是否含有PageUrl
                        var PageUrl = '<%=Whir.Framework.RequestUtil.Instance.GetString("PageUrl")%>';
                        if (PageUrl == "")
                            location.href = '<%=AppPath%>' + 'Main.aspx';
                        else
                            location.href = PageUrl;
                    } else {
                        if (result.Message == "-1")
                            location.href = 'RegKey.aspx'; //跳转到授权页面
                        else if(result.Message == "-2")
                            location.href = 'ChangePwd.aspx'; //跳转到修改密码页面
                        else {
                            isShowCode();
                            $("#imgCode").click();
                            whir.toastr.warning(result.Message);
                        }
                    }
                    whir.loading.remove();
                },
                error:function(result){
                    whir.toastr.error(result.responseText);
                    whir.loading.remove();
                }
            });
        }

        function isShowCode() {
            whir.ajax.post('<%=SystemPath%>Handler/Security/User.aspx', {
                data: {
                    _action: "IsShowCode"
                },
                success: function (result) {
                    if (result.Status) {
                        $("#divCode").show();
                    }
                    whir.loading.remove();
                }
            });

        }

        function getLoinName() {
            whir.ajax.post('<%=SystemPath%>Handler/Security/User.aspx', {
                data: {
                    _action: "GetLoginName"  
                },
                success: function (result) {
                    if (result.Status) {
                        $("#txtUserName").val(result.Message);
                        $("[name='Remember']").iCheck("check");
                    }
                    whir.loading.remove();
                }
            });
        }


        $(document).ready(function () {
            whir.skin.checkbox(); //美化checkbox
            isShowCode();
            getLoinName();
            $("#txtUserName").focus();
            document.onkeydown = function (e) {
                var ev = document.all ? window.event : e;
                if (ev.keyCode == 13) {
                    submitLogin();
                }
            };
        });

      
    </script>
</body>

</html>
