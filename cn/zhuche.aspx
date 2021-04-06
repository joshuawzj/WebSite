<%@ Page Language="C#" ValidateRequest="false" Inherits="FrontBasePage" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Label" %>
<script type="text/C#" runat="server">
/*

 *描述： 本文件由ezEIP模板发布引擎生成，请勿直接更改此文件！！ 

 *模板路径：C:\wwwroot\WebSite\cn\template\注册.shtml

 *生成时间：2021-03-19 09:59:03

 */

public Whir.Domain.Column PageColumn ; 
public Whir.Domain.SiteInfo PageSiteInfo ; 
/*url变量*/ 
protected void Page_Load(object sender, EventArgs e){
    /*url变量赋值*/ 
    PageColumn = ServiceFactory.ColumnService.SingleOrDefault<Whir.Domain.Column>(0);
    ServiceFactory.ColumnService.CheckCanBrowseAspx();
    PageSiteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<Whir.Domain.SiteInfo>(1);
    Whir.Label.LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn); 
    DataBind();
}
</script><!DOCTYPE html>
<html lang="zh-cn">
<head>
<wtl:RemoveColor  runat="server"></wtl:RemoveColor>
<meta charset="utf-8">
        <wtl:include runat="server"      filename="head.html"></wtl:include>
</head>
<body>
<!--top--> 
       <wtl:include runat="server"      filename="top.html"></wtl:include>
<!--top End--> 
<div class="registered" style="background: url(<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>uploadfiles/image/zc_02.jpg) no-repeat center center;background-size: cover;">
<div class="zc_box">
    
  <div class="zc_list">
    <h1>注册</h1>
    <div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_03.png"/>
      <input type="text" id="18251" name="18251"  placeholder="微信号"/>
           <span name="whirValidator" validatorfor="18251" required="true" requiredmsg="微信号为必填" regexp="" errmsg="微信号验证不通过"></span>
    </div>
    <div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_06.png"/>
      <input type="text"  id="11238" name="11238" placeholder="登录名"/>
        <span name="whirValidator" validatorfor="11238" required="true" requiredmsg="呢称为必填" regexp="" errmsg="呢称验证不通过"></span>
    </div>
    <div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_08.png"/>
      <input type="text" id="11243" name="11243"  placeholder="邮箱"/>
        <span name="whirValidator" validatorfor="11243" required="true" requiredmsg="安全邮箱为必填" onlymsg="安全邮箱已经被使用" regexp="^\w+([-+.]\w+)*@\w+([-.]\\w+)*\.\w+([-.]\w+)*$" errmsg="安全邮箱验证不通过"></span>
    </div>
    <div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_10.png"/>
      <input type="text" placeholder="验证码" id="code"/>
      <button onclick="send()">发送验证码</button>
    </div>
    <div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_12.png"/>
      <input id="11239" name="11239" type="password"  placeholder="密码（不少于8位）"/>
        <span name="whirValidator" validatorfor="11239" required="true" requiredmsg="密码为必填" regexp="" errmsg="密码验证不通过"></span>
    </div>
    <div class="zc_radio">
      <input type="checkbox" id="11241" name="11241" value="1" checked="checked"/>同意注册，并已阅读<a href="#">服务协议</a>
        <span name="whirValidator" validatorfor="11241" required="true" requiredmsg="请勾选会员注册协议!"></span>
    </div>
    <div class="zc_btn">
      <a href="#" id="ljzc" class="ljzc" onclick="sava()">立即注册</a>
    </div>
  </div>

</div>
</div>

<script>
    function send() {
        var email = $("#11243").val();
        var reg = /^\w+([-+.]\w+)*@\w+([-.]\\w+)*\.\w+([-.]\w+)*$/;
        if (!reg.test(email)) { alert("邮箱格式不正确！"); return; }

        $.post("<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>ajax/AjaxData.aspx", { action: "sendcode", email }, function (ref) {
            if (ref == "1") {
                alert("已发送验证码至您的邮箱，请进入邮箱查看！");
            } else {
                alert(ref);
            }
        });
    }
    function sava() {
        var weixinnumber = $("#18251").val();
        if (weixinnumber == "") { alert("请输入微信号！"); return; }

        var loginname = $("#11238").val();
        if (loginname == "") { alert("请输入登录名！"); return; }

        var email = $("#11243").val();
        var reg = /^\w+([-+.]\w+)*@\w+([-.]\\w+)*\.\w+([-.]\w+)*$/;
        if (!reg.test(email)) { alert("邮箱格式不正确！"); return; }

        var code = $("#code").val();
        if (code == "") { alert("请输入验证码！"); return; }

        var pwd = $("#11239").val();
        if (pwd == "") { alert("请输入密码！"); return; }
        if (pwd.length < 8) {
            alert("密码长度不少于8位！"); return;
        }

        $.post("<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>ajax/AjaxData.aspx", { action:"zhuce", weixinnumber, loginname, email, code, pwd }, function (ref) {
            if (ref == "1") {
                $(".main_tc").show();
				 setInterval("myInterval()",2000);//1000为1秒钟
            } else {
                alert(ref);
                location.href = location.href;
            }
        });
        
    }
	
	     function myInterval()
       {
           location.href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>denglu.aspx";
        }
</script>

<!-- 弹窗 -->
<div class="main_tc">
  <div class="mainbox">
    <div class="tc">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/logo.png"/>
      <h2>注册成功，欢迎加入光盐财经!</h2> 
    </div>
  </div>
</div>
<!--top--> 
     <wtl:include runat="server"      filename="bottom.html"></wtl:include>
<!--top End--> 
</body>

</html>
