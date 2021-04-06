<%@ Page Language="C#" ValidateRequest="false" Inherits="FrontBasePage" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Label" %>
<script type="text/C#" runat="server">
/*

 *描述： 本文件由ezEIP模板发布引擎生成，请勿直接更改此文件！！ 

 *模板路径：C:\wwwroot\WebSite\cn\template\用户中心.shtml

 *生成时间：2021-04-02 17:26:02

 */

public Whir.Domain.Column PageColumn ; 
public Whir.Domain.SiteInfo PageSiteInfo ; 
/*url变量*/ 
protected void Page_Load(object sender, EventArgs e){
    /*url变量赋值*/ 
    PageColumn = ServiceFactory.ColumnService.SingleOrDefault<Whir.Domain.Column>(6);
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
    <script>
        var member ="<%# GetUserId()%>";
        if(member=="0")
        {
            alert("请先登录！");
            location.href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>denglu.aspx";
        }
    </script>
</head>
<body>
    <!--top-->
    <wtl:include runat="server"      filename="top.html"></wtl:include>
    <!--top End-->
    <div class="ban user">
        <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>uploadfiles/image/yh_02.jpg" />
        <div class="user_box">
            <div class="auto auto_1200">
                <h1>用户中心</h1>
                <div class="tx">
                    <div class="tx_box">
                        <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/name.png" />
                    </div>
                    <p><%# GetUserName()%> / <a style="color:#fff" href="javascript:loginout();">退出</a></p>
                </div>
            </div>
        </div>
    </div>

    <div class="user_cen">
        <div class="auto auto_1200">
            <div class="user_top">
                <div class="user_list">
                    <div class="user_list1">
                        <h2>专享文章</h2>
                        <ul>
                            <wtl:list runat="server"  columnid="9" count="7"><ItemTemplate>
                                <wtl:if runat="server"      testtype='<%# Container.DataRow["files"].ToReplace() %>' TestOperate="NotEmpty">
                                    <successTemplate>
									
									<!--文件-->	
									
										<li>
											<a <wtl:if runat="server"       testtype="<%# CheckTime()%>" TestOperate="Equals"   testvalue="0"><successTemplate>href='javascript:tip();'</successTemplate><failureTemplate>href='<%=Whir.ezEIP.Web.SysManagePageBase.Insten.UploadFilePath %><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent.Parent.Parent).DataRow["files"].ToReplace() %>' target="_blank"</failureTemplate></wtl:if>>
												<p><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["title"].ToReplace() %></p>
												<span><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["createdate"].ToReplace().ToDateTime().ToString("yyyy.MM") %></span>
											</a>
										</li>
																				
										
                                    </successTemplate>
                                    <failureTemplate>
									
									<!--文章-->
										<li>
											<a <wtl:if runat="server"       testtype="<%# CheckTime()%>" TestOperate="Equals"   testvalue="0"><successTemplate>href='javascript:tip();'</successTemplate><failureTemplate>href='<%#Whir.Service.ServiceFactory.ColumnService.GetColumnListLink(9,true,3)%>?itemid=<%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent.Parent.Parent).DataRow["whir_U_content_PID"].ToReplace() %>' target="_blank"</failureTemplate></wtl:if>>
												<p><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["title"].ToReplace() %></p>
												<span><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["createdate"].ToReplace().ToDateTime().ToString("yyyy.MM") %></span>
											</a>
										</li>
										
										
										
                                    </failureTemplate>
                                </wtl:if>
                            </ItemTemplate></wtl:list>
                        </ul>
                    </div>
                </div>
                <Wtl:list runat="server"  sql="select * from whir_mem_member where typeid=1 and isdel=0 and whir_mem_member_pid={0}" sql0="<%# GetUserId()%>"><ItemTemplate>
                    <div class="user_list">
                        <div class="user_list1">
                            <div class="U_top">
                                <div class="U_right">
                                    <h3><%# Container.DataRow["Email"].ToReplace() %></h3> <a style="float:right" href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>changeemail.aspx">修改邮箱</a>
                                    <p>到期时间：<b style="color:red"><%# Container.DataRow["EndTime"].ToReplace().ToDateTime().ToString("yyyy-MM-dd") %></b>，到期后无法查看文章</p>
									
                                </div>
                                <div class="U_bot">
                                    <div class="U_box">
                                        <h3>上次登录时间：</h3>
                                        <span><%# Container.DataRow["LastTime"].ToReplace().ToDateTime().ToString("yyyy-MM-dd hh:mm:ss") %></span>
                                    </div>
                                    <div class="U_btn">
                                        <a href='<%#Whir.Service.ServiceFactory.ColumnService.GetColumnListLink(4,true,3)%>'>立即续费</a>
										
                                    </div> 
									<a href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>changepassword.aspx">修改密码</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate></Wtl:list>

            </div>

            <div class="user_bottom">
                <div class="user_xw">
                    <h2>推荐阅读</h2>
                    <div class="user_bottom1">
                        <div class="user_li">
                            <ul>
                                <wtl:list runat="server"   columnid="11" count="7"><ItemTemplate>
                                    <wtl:if runat="server"       testtype='<%# Container.DataRow["files"].ToReplace() %>' TestOperate="NotEmpty">
                                        <successTemplate>
                                            
										<!--文件-->	
											<li>
												<a <wtl:if runat="server"         testtype="<%# CheckTime()%>" TestOperate="Equals"   testvalue="0"><successTemplate>href='javascript:tip();'</successTemplate><failureTemplate>href='<%=Whir.ezEIP.Web.SysManagePageBase.Insten.UploadFilePath %><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent.Parent.Parent).DataRow["files"].ToReplace() %>' target="_blank"</failureTemplate></wtl:if>>
													<p><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["title"].ToReplace() %></p>
													<span><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["createdate"].ToReplace().ToDateTime().ToString("yyyy.MM") %></span>
												</a>
											</li>
											
                                        </successTemplate>
                                        <failureTemplate>
											
										<!--文章-->	
											<li>
												<a <wtl:if runat="server"         testtype="<%# CheckTime()%>" TestOperate="Equals"   testvalue="0"><successTemplate>href='javascript:tip();'</successTemplate><failureTemplate>href='<%#Whir.Service.ServiceFactory.ColumnService.GetColumnListLink(11,true,3)%>?itemid=<%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent.Parent.Parent).DataRow["whir_U_content_PID"].ToReplace() %>' target="_blank"</failureTemplate></wtl:if>>
													<p><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["title"].ToReplace() %></p>
													<span><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["createdate"].ToReplace().ToDateTime().ToString("yyyy.MM") %></span>
												</a>
											</li>
											
                                        </failureTemplate>
                                    </wtl:if>
                                </ItemTemplate></wtl:list>
                            </ul>
                        </div>
                        <div class="user_li">
                            <ul>
                                <wtl:list runat="server"   columnid="11" where="whir_U_content_PID not in (select top 7 whir_U_content_PID from whir_U_content where typeid=11 and isDel=0 order by sort desc,createdate desc)" count="7"><ItemTemplate>
                                    <wtl:if runat="server"       testtype='<%# Container.DataRow["files"].ToReplace() %>' TestOperate="NotEmpty">
                                        <successTemplate>
										
										<!--文件-->
											  
											<li>
												<a <wtl:if runat="server"         testtype="<%# CheckTime()%>" TestOperate="Equals"   testvalue="0"><successTemplate>href='javascript:tip();'</successTemplate><failureTemplate>href='<%=Whir.ezEIP.Web.SysManagePageBase.Insten.UploadFilePath %><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent.Parent.Parent).DataRow["files"].ToReplace() %>' target="_blank"</failureTemplate></wtl:if>>
													<p><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["title"].ToReplace() %></p>
													<span><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["createdate"].ToReplace().ToDateTime().ToString("yyyy.MM") %></span>
												</a>
											</li>
										 
                                           
                                        </successTemplate>
                                        <failureTemplate>
										
										<!--文章-->
											
											  
											  
											<li>
												<a <wtl:if runat="server"         testtype="<%# CheckTime()%>" TestOperate="Equals"   testvalue="0"><successTemplate>href='javascript:tip();'</successTemplate><failureTemplate>href='<%#Whir.Service.ServiceFactory.ColumnService.GetColumnListLink(11,true,3)%>?itemid=<%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent.Parent.Parent).DataRow["whir_U_content_PID"].ToReplace() %>' target="_blank"</failureTemplate></wtl:if>>
													<p><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["title"].ToReplace() %></p>
													<span><%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["createdate"].ToReplace().ToDateTime().ToString("yyyy.MM") %></span>
												</a>
											</li>
										 
                                            
                                        </failureTemplate>
                                    </wtl:if>
                                </ItemTemplate></wtl:list>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
	
	<script>
	  function loginout() { 

        $.ajax({ 

			url: "<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>label/member/loginout.aspx?t=" + new Date().getMilliseconds(), 

            success: function (data) { 

                location.href ='<%=Whir.Service.ServiceFactory.SiteInfoService.GetSiteUrlLinkAspx("indexurl",PageSiteInfo)%>';  
 

            } 

        }); 

    }


		function tip(){
			layer.confirm('您尚未开通服务，或者身份已过期，请重新续费。', {
			  btn: ['确定','取消'] //按钮
			}, function(){
			  location.href='<%#Whir.Service.ServiceFactory.ColumnService.GetColumnListLink(4,true,3)%>';
			}, function(){
			  
			});
		}

	</script>


    <!--bottom-->
    <wtl:include runat="server"      filename="bottom.html"></wtl:include>
    <!--bottom End-->
</body>

</html>
