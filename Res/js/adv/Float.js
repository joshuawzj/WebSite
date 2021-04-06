function FloatMove(o){
	var el=$("#floatAd_"+o.defaultSetting.AdId);
	var left = el.offset().left;
    var top = el.offset().top;
    var L = T = 0; //左边界和顶部边界
    var R = $(window).width() - el.width(); // 右边界
    var B = $(window).height() - el.height(); //下边界
    if (left < L) {
        o.xin = true; // 水平向右移动
    }
    if (left > R) {
        o.xin = false;
    }
    if (top < T) {
        o.yin = true;
    }
    if (top > B) {
        o.yin = false;
    }
    left = left + o.step * (o.xin == true ? 1 : -1);
    top = top + o.step * (o.yin == true ? 1 : -1);
    // 给div 元素重新定位
    el.offset({
        top: top,
        left: left
    })
	}

/*
@param AdId:唯一值 ID
@param adX:图片的左边起始位置
@param adY:图片的顶部起始位置
@param adImg:图片路径
@param adImgW:图片的宽度
@param adImgH:图片的高度
@param adUrl:链接地址
@param adIsClose:是否允许关闭
@param adTitle:标题
*/
function FloatAd(options)
{
	//id,x,y,img,w,h,url,isclose
this.xin = true,
this.yin = true;
this.step = 1;
this.delay = 20;
this.defaultSetting={
	AdId:Math.ceil(Math.random()*100000),
	adX:0,
	adY:0,
	adImg:'',
	adImgW:0,
	adImgH:0,
	adUrl:'#',
	adIsClose:1,
	adTitle:''
	};
	$.extend(this.defaultSetting,options);

this.html='<div id="floatAd_'+this.defaultSetting.AdId+'" style="position:absolute;left:'+this.defaultSetting.adX+'px;top:'+this.defaultSetting.adY+'px;">';
this.html+='<div id="floatAd_btn_close_'+this.defaultSetting.AdId+'" style="width:20px;height:20px;position:absolute;right:0px;text-align: center;cursor:pointer;" title="关闭">X</div>';
this.html+='<div><a href="" target="_blank" title="'+this.defaultSetting.adTitle+'"><img border="0" alt="'+this.defaultSetting.adTitle+'" src="'+this.defaultSetting.adImg+'"';
if(this.defaultSetting.adImgW>0)
{this.html+=' width="'+this.defaultSetting.adImgW+'"';}
if(this.defaultSetting.adImgH>0)
{this.html+=' height="'+this.defaultSetting.adImgH+'"';}
this.html+=' /></a></div></div>';
var that=this;
this.init=function(){
	document.write(that.html);
	if(that.defaultSetting.adIsClose!=1)
	{$("#floatAd_btn_close_"+that.defaultSetting.AdId).hide();}
	var time = window.setInterval(function(){FloatMove(that);}, that.delay);
	$("#floatAd_btn_close_"+that.defaultSetting.AdId).click(function(){
		$("#floatAd_"+that.defaultSetting.AdId).remove();
		clearInterval(time);
		});
		var o=$("#floatAd_"+that.defaultSetting.AdId);
		
		
    o.mouseover(function() {
        clearInterval(time);
    });
   o.mouseout(function() {
        time = window.setInterval(function(){FloatMove(that);}, that.delay)
    });
	};


}

//调用式例
//new FloatAd(1,50,50,'http://localhost:8000/eip5.3/cn/images/historyhead.gif',0,0,'#',1).init();
//new FloatAd({AdId:1,adX:50,adY:50,adImg:'http://localhost:8000/eip5.3/cn/images/historyhead.gif',adImgW:0,adImgH:0,adUrl:'#',adIsClose:1,adTitle:'测试广告'}).init();