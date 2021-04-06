<%@ Page Language="C#" ValidateRequest="false" Inherits="FrontBasePage" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Label" %>
<script type="text/C#" runat="server">
/*

 *描述： 本文件由ezEIP模板发布引擎生成，请勿直接更改此文件！！ 

 *模板路径：C:\wwwroot\WebSite\cn\template\登录.shtml

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
    <wtl:resource runat="server"     ></wtl:resource>
</head>
<body>
<!--top--> 
    <wtl:include runat="server"      filename="top.html"></wtl:include>
<!--top End--> 
<div class="registered registered1" style="background: url(<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>uploadfiles/image/zc_02.jpg) no-repeat center center;background-size: cover;">
<div class="zc_box zc_box1">
    <form id='whir_2' res='whirform'>
  <div class="zc_list">
    <h1>登录</h1>
    <div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_08.png"/>
      <input type="text" name="loginname" id="txtUsername" placeholder="登录名"/>
    </div>
    <div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_12.png"/>
      <input type="password" name="password" id="txtPassword"  placeholder="密码（不少于8位）"/>
    </div>
    <div class="zc_radio">
      <input type="checkbox" id="mima"/><label for="mima">记住密码</label>
	  <a href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>forgetpassword.aspx">忘记密码？</a>
    </div>
    <div class="zc_btn">
      <a commandbutton="login" >立即登录</a>
    </div>
  </div>
    </form>
</div>
</div>


<!--top--> 
        <wtl:include runat="server"      filename="bottom.html"></wtl:include>

    <script type="text/javascript">
        $(function () {
            //会员登录

            $("[commandbutton='login']").bind('click', function () {
                var loginName = $("#txtUsername").val();
                var password = $("#txtPassword").val();
                var autologin = false;
                var errmsg = '';
                var index = 1;
                if (loginName == '') {
                    errmsg = index + ".登录名称不能为空\n";
                    index++;
                }
                if (password == '') {
                    errmsg += index + ".密码不能为空\n";
                    index++;
                }
                if (errmsg.length > 0) {
                    alert(errmsg);
                    return;
                }
                $.ajax({
                    //url: whir.site.getAppPath() + "label/member/login.aspx", //路径需更改
                    url: "<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>label/member/login.aspx", //没有用到 wtl:resource 置标时用此路径
                    type: "POST",
                    cache: false,
                    data: { loginname: loginName, password: password, autologin: autologin },
                    success: function (state) {
                        switch (state) { //返回0：用户名或密码错误；1:帐号未审核；2：登录成功；3:回收站
                            case "0":
                                alert("用户名或密码错误");
                                $("#txtUsername").val("");
                                $("#txtPassword").val("");
                                break;
                            case "1":
                                alert("帐号未审核");
                                $("#txtUsername").val("");
                                $("#txtPassword").val("");
                                break;
                            case "2":
                                alert("登录成功");
                                //location.href = whir.site.getAppPath() + "member/index.aspx";
                                location.href ='<%#Whir.Service.ServiceFactory.ColumnService.GetColumnListLink(6,true,0)%>'; //登录个人
                                break;
                            case "3":
                                alert("帐号已被冻结");
                                $("#txtUsername").val("");
                                $("#txtPassword").val("");
                                break;
                            default:
                                alert("登录失败");
                                $("#txtUsername").val("");
                                $("#txtPassword").val("");
                                break;
                        }
                    },
                    error: function () {
                        alert("登录失败");
                        $("#txtUsername").val("");
                        $("#txtPassword").val("");
                    }
                });
            });
        });
    </script>

<!--top End--> 
</body>

</html>
