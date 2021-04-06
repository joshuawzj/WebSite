<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="adminchangepwd.aspx.cs" Inherits="whir_system_module_security_adminchangepwd" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Module/Security/Admin.aspx">
                <div class="form-group">
                    <div class="col-md-2 control-label">
                        <%="旧密码：".ToLang()%>
                    </div>
                    <div class="col-md-8">
                        <input type="password" id="txtOldPassword" name="txtOldPassword" class="form-control"
                            required="true" minlength="6" maxlength="20" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 control-label">
                        <%="新密码：".ToLang()%>
                    </div>
                    <div class="col-md-8">
                        <input type="password" onkeyup="pwStrength(this.value)" onblur="pwStrength(this.value)"
                            id="txtNewPassWord" name="txtNewPassWord" class="form-control" required="true"
                            minlength="6" maxlength="20" />
                        <table border="1" cellpadding="1" bordercolordark="#fdfeff" bordercolorlight="#99ccff"
                            cellspacing="0" style="background-color: #e0f0ff; line-height: 16px; margin-top: 3px;
                            border: solid 1px #99ccff;">
                            <tr>
                                <td id="strength_L" style="width: 50px;" align="center">
                                    <%="弱".ToLang()%>
                                </td>
                                <td id="strength_M" style="width: 50px;" align="center">
                                    <%="中".ToLang()%>
                                </td>
                                <td id="strength_H" style="width: 50px;" align="center">
                                    <%="强".ToLang()%>
                                </td>
                            </tr>
                           
                        </table>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 control-label">
                        <%="确认密码：".ToLang()%>
                    </div>
                    <div class="col-md-8">
                        <input type="password" id="txtNewPassWord2" name="txtNewPassWord2" class="form-control"
                            required="true" minlength="6" maxlength="20" />
                        <span class="note_gray"> <%=(AppSettingUtil.GetString("IsStrongPassword").ToBoolean(true)?"密码必须包含数字、字母、特殊符号，长度在6-20位":"密码长度在6-20位").ToLang() %></span>
                           
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-8">
                        <input type="hidden" name="UserId" value="<%=CurrenUsers.UserId %>" />
                        <input type="hidden" name="_action" value="ChangePwd" />
                        <button form-action="submit" form-success="refresh" class="btn btn-info btn-block">
                            <%="提交".ToLang()%></button>
                    </div>
                </div>
                </form>
            </div>
        </div>
    </div>
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

        var regexp = /^[^\s]*$/, message = "<%="6-20字符,除空格外任意字符组合".ToLang()%>";
        <%if (AppSettingUtil.GetString("IsStrongPassword").ToBoolean(true)) {%>
            regexp = /^(?=.*[A-Za-z])(?=.*[0-9])(?=.*[^\sA-Za-z0-9])\S{6,20}$/;
            message = "<%="密码必须包含数字、字母、特殊符号，长度在6-20位".ToLang()%>";
        <%}%>
        

        var options = {
            fields: {
             txtNewPassWord: {
                    validators: {
                       regexp: {
                            regexp: regexp,
                            message: message
                       },
                       different: {
                           field: 'txtOldPassword',
                           message: '<%="新旧密码不能相同".ToLang()%>'
                       }
                    }
                }, 
                txtNewPassWord2: {
                    validators: {
                        regexp: {
                            regexp: regexp,
                            message: message
                        }, identical: {
                            field: 'txtNewPassWord',
                            message: '<%="两次密码输入不一致".ToLang()%>'
                        }
                    }
                }
            }
        };
        $('#formEdit').Validator(options,
             function (el) {
                 var actionSuccess = el.attr("form-success");
                 var $form = $("#formEdit");
                 $form.post({
                     success: function (response) {
                         if (response.Status == true) {
                             actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "AdminList.aspx");
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

</asp:Content>
