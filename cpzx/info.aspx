<%@ Page Language="C#" ValidateRequest="false" Inherits="FrontBasePage" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Label" %>
<script type="text/C#" runat="server">
/*

 *描述： 本文件由ezEIP模板发布引擎生成，请勿直接更改此文件！！ 

 *模板路径：C:\wwwroot\WebSite\cn\template\支付页面.shtml

 *生成时间：2021-04-06 09:46:26

 */

public Whir.Domain.Column PageColumn ; 
public Whir.Domain.SiteInfo PageSiteInfo ; 
/*url变量*/ 
protected void Page_Load(object sender, EventArgs e){
    /*url变量赋值*/ 
    PageColumn = ServiceFactory.ColumnService.SingleOrDefault<Whir.Domain.Column>(4);
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
    <link href="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>res/js/layer/skin/default/layer.css" rel="stylesheet" />
    <script src="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>res/js/layer/layer.js"></script>
</head>
<body>
    <!--top-->
    <wtl:include runat="server"      filename="top.html"></wtl:include>
    <!--top End-->
    <div class="ban">
        <img src='<wtl:banner runat="server"     ></wtl:banner>' />
    </div>
    <div class="zf">
        <div class="auto auto_1200">
            <div class="zf_box">
                <wtl:list runat="server"  columnid="4"><ItemTemplate>
                    <div class="zf_li">
                        <div class="zf_li_box">
                            <h3><%# Container.DataRow["title"].ToReplace() %></h3>
                            <h1><%# Container.DataRow["price"].ToReplace() %><span>元</span></h1>
                            <p><%# Container.DataRow["content"].ToReplace() %></p>
                            <div class="zf_bot">
                                <ul style="margin-top:0px;">
                                   <wtl:list runat="server"  sql0='<%# Container.DataRow["whir_U_COntent_pid"].ToReplace() %>' sql="select Whir_U_Category_PId,CategoryName from Whir_U_Category  where isdel = 0 and Whir_U_Category_PId in (select value = substring(a.lable , b.number , charindex(',' , a.lable + ',' , b.number) - b.number)from (select lable from Whir_U_Content where typeid = 4 and Whir_U_Content_PId = {0} ) a join master..spt_values  b  on b.type='p' and b.number between 1 and len(a.lable) where substring(',' + a.lable , b.number , 1) = ',' )" ><ItemTemplate> 
										  <li><%# Container.DataRow["CategoryName"].ToReplace() %></li> 
									</ItemTemplate></wtl:list> 
                                </ul>

                                <div class="zf_btn" >
                                    <a href="javascript:dy(<%# Container.DataRow["whir_U_Content_pid"].ToReplace() %>);">立即订阅</a>
							
                                </div>
   <a style="display:block;margin-top:30px;"href="javascript:dy2(<%# Container.DataRow["whir_U_Content_pid"].ToReplace() %>);">PayPal支付</a>

                            </div>
                        </div>
                    </div>
                </ItemTemplate></wtl:list>
            </div>
        </div>
    </div>

    <div class="popbox" id="popbox1" style="display:none;">
        <div class="content">
            <a href="javascript:$('#popbox1').hide();" class="close"></a>
            <div class="tithead">请用微信、支付宝扫码支付</div>
            <dl>
                <dd>
                    <img src="" id="ewm" />
                </dd> 
				<dd>
					<p>手机访问请长按识别二维码</p>
				</dd>
				<dd class="btn">
					<button class="send" type="button" onclick="send()">支付完成</button>
				</dd>
            </dl> 
        </div>
    </div>
	<div class="popbox" id="popbox2" style="display:none;">
        <div class="content">
            <a href="javascript:$('#popbox2').hide();" class="close"></a>
            <dl id="paypal">
               
            </dl> 
        </div>
    </div>



    <script>
        function dy(e) {
            var member = "<%# GetUserId()%>";
            if (member == "0") {
                alert("请先登录！");
                location.href = "<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>denglu.aspx";
            } else {
                $.ajax({
                    type: "GET",
                    url: "<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>ajax/addOrder.aspx",
                    data: { id: e },
					dataType: "json",
                    success: function (data) {
					console.log(data);
                        if (data.status == "y") {
							$("#ewm").attr("src", data.info);
                            $("#popbox1").show();  
                        } else {
                            layer.alert(data.info);
                        }
                    }
                });
            }
        }
		  function dy2(e) {
            var member = "<%# GetUserId()%>";
            if (member == "0") {
                alert("请先登录！");
                location.href = "<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>denglu.aspx";
            } else {
			window.location.href="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>ajax/addOrder2.aspx?id="+e;
              
            }
        }

        function send(){
			$("#popbox1").hide();  
		}
    </script>

    <!--bottom-->
    <wtl:include runat="server"      filename="bottom.html"></wtl:include>
    <!--bottom End-->
</body>

</html>

