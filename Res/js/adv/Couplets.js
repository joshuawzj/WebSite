function CoupletsMove(o)
{
	var delta=0.08;
	var followObj= document.getElementById("CoupletAd_"+o.Setting.adId);
	var followObj_x	= o.Setting.adX;
	var followObj_y	=o.Setting.adY;
	
	if(o.Setting.adPosition=="leftTop" || o.Setting.adPosition=="rightTop")
	{
    if(followObj.offsetTop!=(document.documentElement.scrollTop+followObj_y)) {
					var dy=(document.documentElement.scrollTop+followObj_y-followObj.offsetTop)*delta;
					dy=(dy>0?1:-1)*Math.ceil(Math.abs(dy));
					followObj.style.top=followObj.offsetTop+dy+ "px";
					}
	}
	else
	{
		if(followObj.offsetTop!=(document.documentElement.clientHeight-followObj_y-o.Setting.adImgH+document.documentElement.scrollTop)) {
			
				followObj.style.top=(document.documentElement.clientHeight-followObj_y-o.Setting.adImgH+document.documentElement.scrollTop)+ "px";
					}
		}
		
			
	}
function CoupletsAd(options)
{
    this.Setting={
	adId:Math.ceil(Math.random()*100000),
	adX:0,     //广告X坐标
	adY:0,     //广告Y坐标
	adImg:'',  //广告路径
	adImgW:100,//广告宽度
	adImgH:100,  //广告高度
	adUrl:'#', //广告链接地址
	adIsClose:1,//是否允许关闭
	adTitle:'',
	adType:'img', //广告类型 img:图片 video：视频
	adPosition:'leftTop', //起始位置 leftTop\leftBottom\rightTop\rightBottom
	adPoster:'',//如果是视频类型时，设置的视频封面图
	adControls:1,//如果是视频类型时，是否显示按制条
	adAutoPlay:0//如果是视频类型时，是否自动播放，默认为否
	
	};
	$.extend(this.Setting,options);
	this.html='<DIV id=CoupletAd_'+this.Setting.adId+' style="Z-INDEX: 9999; POSITION: absolute;';
	this.html+=' width:'+this.Setting.adImgW+'px; height:'+this.Setting.adImgH+'px;';
	switch(this.Setting.adPosition)
	{
		case "leftBottom":
		this.html+=' left:'+this.Setting.adX+'px; bottom:'+this.Setting.adY+'px; ';
		break;
		case "rightTop":
		this.html+=' right:'+this.Setting.adX+'px; top:'+this.Setting.adY+'px; ';
		break;
		case "rightBottom":
		this.html+=' right:'+this.Setting.adX+'px; bottom:'+this.Setting.adY+'px; ';
		break;
		default:
		this.html+=' left:'+this.Setting.adX+'px; top:'+this.Setting.adY+'px; ';
		break;
		}
		this.html+=' ">';
		if(this.Setting.adIsClose==1)
		{this.html+='<div id="CoupletAd_btn_close_'+this.Setting.adId+'" title="关闭" style="position: absolute;top:0px;right:0px;width:20px;height:20px;background-color:#fff;Z-INDEX:10000; text-align:center"><a href="javascript:;" style="text-decoration:none; color:#000;font-size:14px;">X</a></div>';}
		if(this.Setting.adType=="video")
		{
			this.html+='<div><video ';
			if(this.Setting.adPoster!="")
			{this.html+=' poster="'+this.Setting.adPoster+'"';}
			this.html+=' src="'+this.Setting.adImg+'" width="'+this.Setting.adImgW+'" height="'+this.Setting.adImgH+'" ';
			if(this.Setting.adControls==1)
			{this.html+=' controls="controls"';}
			if(this.Setting.adAutoPlay==1)
			{this.html+=' autoplay ';}
			this.html+=' ></video><div>'+this.Setting.adTitle+'</div></div>';
			}
		else
		{
			this.html+='<div><a href="'+this.Setting.adUrl+'" target="_blank"><img src="'+this.Setting.adImg+'" border="0" alt="'+this.Setting.adTitle+'"';
			if(this.Setting.adImgW>0)
			{this.html+=' width="'+this.Setting.adImgW+'px"';}
			if(this.Setting.adImgH>0)
			{this.html+=' height="'+this.Setting.adImgH+'px"';}
			this.html+=' /></a></div>';
			}
		this.html+='</div>';
		this.time=null;
		var that=this;
		
		this.init=function(){
			document.write(that.html);
			if(that.Setting.adIsClose!=1)
		{$("#CoupletAd_btn_close_"+that.Setting.adId).hide();}
	    $("#CoupletAd_btn_close_"+that.Setting.adId).click(function(){
		$("#CoupletAd_"+that.Setting.adId).remove();
		clearInterval(that.time);
		//that.time=null;
		});
		that.time=setInterval(function(){CoupletsMove(that);},10);
		
	
			};
}
	
	

	