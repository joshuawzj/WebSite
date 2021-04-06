<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePwd.aspx.cs" Inherits="Whir_System_Module_Security_ChangePwd" %>


<%@ Import Namespace="Whir.Framework" %>

<!DOCTYPE html ">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
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
    <script>
        var _sysPath = "<%=SystemPath%>";
    </script>
</head>
<body>
    <div>
        <div id="login-wrapper">
            <div class="row">
                <div class="col-md-4 col-md-offset-4">
                    <div id="logo-login">
                        <h1>更换密码<span>每<%=AppSettingUtil.GetInt32("ChangePwdDays") %>天更换登录密码</span>
                        </h1>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 col-md-offset-4">
                    <div class="account-box">
                        <form id="formEdit" class="form-horizontal" form-url="<%= SystemPath %>Handler/Security/User.aspx">
                            <div class="form-group">
                                <div class="col-md-2 control-label">
                                    用户名
                                </div>
                                <div class="col-md-10">
                                    <input type="text" id="txtUserName" name="UserName" placeholder="用户名" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label">
                                   旧密码
                                </div>
                                <div class="col-md-10">
                                    <input type="password" id="txtOldPassword" name="txtOldPassword" placeholder="旧密码" class="form-control"
                                        required="true" minlength="6" maxlength="20" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label">
                                    新密码
                                </div>
                                <div class="col-md-10">
                                    <input type="password" onkeyup="pwStrength(this.value)" placeholder="新密码" onblur="pwStrength(this.value)"
                                        id="txtNewPassWord" name="txtNewPassWord" class="form-control" required="true"
                                        minlength="6" maxlength="20" />
                                    <table border="1" cellpadding="1" bordercolordark="#fdfeff" bordercolorlight="#99ccff"
                                        cellspacing="0" style="background-color: #e0f0ff; line-height: 16px; margin-top: 3px; border: solid 1px #99ccff;">
                                        <tr>
                                            <td id="strength_L" style="width: 48px;" align="center">弱
                                            </td>
                                            <td id="strength_M" style="width: 48px;" align="center">中
                                            </td>
                                            <td id="strength_H" style="width: 48px;" align="center">强
                                            </td>
                                        </tr>

                                    </table>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label">
                                   确认密码
                                </div>
                                <div class="col-md-10">
                                    <input type="password" id="txtNewPassWord2" name="txtNewPassWord2" placeholder="确认密码" class="form-control"
                                        required="true" minlength="6" maxlength="20" />
                                    <span class="note_gray">密码必须包含数字、字母、特殊符号，长度在6-20位</span>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-2 col-md-10">
                                    <input type="hidden" name="_action" value="ChangePwdFor30Day" />
                                    <button form-action="submit" form-success="refresh" class="btn btn-info ">
                                        提交</button>
                                       <a  href="ezEIP_Login.aspx" class="btn btn-white ">
                                        返回</a>
                                </div>
                            </div>
                        </form>
                        <div>
                            <div class="row"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="space15"></div>
        <div style="text-align: center; margin: 0 auto;">
            <h6 style="color: #fff;">Prower by WHIR
            </h6>
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
        
        //CharMode函数     
        //测试某个字符是属于哪一类.     
        function CharMode(iN) {
            if (iN >= 48 && iN <= 57) //数字     
                return 1;
            if (iN >= 65 && iN <= 90) //大写字母     
                return 2;
            if (iN >= 97 && iN <= 122) //小写     
                return 4;
            else
                return 8; //特殊字符     
        }
        //bitTotal函数     
        //计算出当前密码当中一共有多少种模式     
        function bitTotal(num) {
            modes = 0;
            for (i = 0; i < 4; i++) {
                if (num & 1) modes++;
                num >>>= 1;
            }
            return modes;
        }
        //checkStrong函数     
        //返回密码的强度级别     
        function checkStrong(sPW) {
            if (sPW.length <= 5)
                return 0; //密码太短     
            Modes = 0;
            for (i = 0; i < sPW.length; i++) {
                //测试每一个字符的类别并统计一共有多少种模式.     
                Modes |= CharMode(sPW.charCodeAt(i));
            }
            return bitTotal(Modes);
        }
        //pwStrength函数     
        //当用户放开键盘或密码输入框失去焦点时,根据不同的级别显示不同的颜色     
        function pwStrength(pwd) { 
            O_color = "#e0f0ff";
            L_color = "#FF0000";
            M_color = "#FF9900";
            H_color = "#33CC00";
            if (pwd == null || pwd == '') {
                Lcolor = Mcolor = Hcolor = O_color;
            }
            else {
                S_level = checkStrong(pwd);
                switch (S_level) {
                case 0:
                    Lcolor = Mcolor = Hcolor = O_color;
                case 1:
                    Lcolor = L_color;
                    Mcolor = Hcolor = O_color;
                    break;
                case 2:
                    Lcolor = Mcolor = M_color;
                    Hcolor = O_color;
                    break;
                default:
                    Lcolor = Mcolor = Hcolor = H_color;
                }
            }

            document.getElementById("strength_L").style.background = Lcolor;
            document.getElementById("strength_M").style.background = Mcolor;
            document.getElementById("strength_H").style.background = Hcolor;
            return;
        }

        var options = {
            fields: {
                UserName: {
                    validators: {
                        notEmpty: {
                            message: '用户名为必填项'
                        }
                    }
                },
                txtNewPassWord: {
                    validators: {
                        regexp: {
                            regexp: /^[^\s]*$/,
                            message: '6-20字符,除空格外任意字符组合'
                        },
                        different: {
                            field: 'txtOldPassword',
                            message: '新旧密码不能相同'
                        },
                        notEmpty: {
                            message: '密码为必填项'
                        }
                    }
                }, 
                txtNewPassWord2: {
                    validators: {
                        regexp: {
                            regexp: /^[^\s]*$/,
                            message: '6-20字符,除空格外任意字符组合'
                        }, identical: {
                            field: 'txtNewPassWord',
                            message: '两次密码不一致'
                        },
                        notEmpty: {
                            message: '确认密码为必填项'
                        }
                    }
                }
            }
        };
        $('#formEdit').Validator(options,
            function (el) {
                var actionSuccess = el.attr("form-success");
                var $form = $("#formEdit");
                var b = new Base64();  
                $("#txtOldPassword").val( b.encode($("#txtOldPassword").val()));
                $("#txtNewPassWord").val( b.encode($("#txtNewPassWord").val()));
                $("#txtNewPassWord2").val( b.encode($("#txtNewPassWord2").val()));
                $form.post({
                    success: function (response) {
                        if (response.Status == true) {
                            whir.toastr.success(response.Message, true, false, "ezEIP_login.aspx");
                        } else {
                            whir.toastr.error(response.Message);
                            document.getElementById("formEdit").reset(); 
                        }
                        whir.loading.remove();
                    },
                    error: function (response) {
                        whir.toastr.error(response.Message);
                        document.getElementById("formEdit").reset(); 
                        whir.loading.remove();
                    }
                });
                return false;
            });
               


    </script>

</body>
</html>
