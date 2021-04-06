<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegKey.aspx.cs" Inherits="whir_system_module_security_regkey" %>

<%@ Import Namespace="Whir.Framework" %>

<html lang="zh-cn">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title><%=AppSettingUtil.GetString("ProductName")%></title>
    <link href="<%=SystemPath %>res/css/blue.css" rel="stylesheet" type="text/css" />
    <link href="<%=SystemPath %>Res/assets/css/style.css" rel="stylesheet" type="text/css" />
    <link href="<%=SystemPath %>res/assets/css/loader-style.css" rel="stylesheet" type="text/css" />
    <link href="<%=SystemPath %>res/assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="<%=SystemPath %>res/assets/css/signin.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="<%= SystemPath %>Res/assets/js/bootstrapvalidator/bootstrapValidator.css">
    <link href="<%=SystemPath %>Res/assets/js/toastr-master/toastr.css" rel="stylesheet" type="text/css" />
    <link href="<%=SystemPath %>res/css/css_whir_V450.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="<%= SystemPath %>Res/assets/js/jquery.js"></script>
    <script type="text/javascript" src="<%= SystemPath %>res/js/whir/whir.form.js"></script>
    <script type="text/javascript" src="<%= SystemPath %>Res/assets/js/bootstrapvalidator/bootstrapValidator.js"></script>
    <script type="text/javascript" src="<%= SystemPath %>Res/assets/js/bootstrapvalidator/language/zh_CN.js"></script>
    <script src='<%=SystemPath %>res/js/base64.js' type="text/javascript"></script>
    <script src="<%=SystemPath %>Res/assets/js/toastr-master/toastr.js" type="text/javascript"></script>
    <script src="<%=SystemPath %>res/js/Whir/whir.ajax.js" type="text/javascript"></script>
    <script src="<%=SystemPath %>res/js/Whir/whir.ui.js" type="text/javascript"></script>

    <script type="text/javascript" src="<%= SystemPath %>Res/assets/js/bootstrap.js"></script>
    <link href="<%= SystemPath %>Res/assets/js/bootstrap-dialog/src/css/bootstrap-dialog.css" rel="stylesheet" type="text/css" />
    <script src="<%= SystemPath %>Res/assets/js/bootstrap-dialog/src/js/bootstrap-dialog.js" type="text/javascript"></script>

</head>
<body>
    <form runat="server">
        <div class="wap-login-wrapper">
            <div id="login-wrapper">
                <div class="row">
                    <div class="col-md-4 col-md-offset-4">
                        <div id="logo-login">
                            <h1>授权码
                            </h1>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4 col-md-offset-4">
                        <div class="account-box">
                            <div class="form-group">
                                <asp:TextBox ID="txtCode" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
                                <div class="text-danger text-center">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ControlToValidate="txtCode" Display="Dynamic" ErrorMessage="请输入授权码"
                                        Font-Bold="True"></asp:RequiredFieldValidator>
                                    <asp:Label ID="lbTips" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <div style="display:none"><%=HttpContext.Current.Request.Url.Host.ToStr().ToLower().EncodeBase64() %></div>
                                </div>
                            </div>
                            <div class="text-center">
                                <div class="btn-group">
                                    <asp:Button ID="btnSubmit" runat="server" Text="提交" OnClick="btnSubmit_Click" CssClass="btn btn-info" />
                                    <a href="ezEIP_Login.aspx" class="btn btn-white ">返回</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="space15"></div>
            <div style="text-align: center; margin: 0 auto;">
                <h6 style="color: #fff;">Powered by WHIR</h6>
            </div>
        </div>
    </form>
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
        var centerPoint = new BMap.Point(locationPoint.lng, locationPoint.lat + 0.005);//偏离量0.005，避免登录窗口挡住中心点
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
</body>
</html>
