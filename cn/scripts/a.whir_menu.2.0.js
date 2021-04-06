(function($,plugin){var data={},id=1,etid=plugin+'ETID';$.fn[plugin]=function(speed,group){id++;group=group||this.data(etid)||id;speed=speed||200;if(group===id)this.data(etid,group);this._hover=this.hover;this.hover=function(over,out){over=over||$.noop;out=out||$.noop;this._hover(function(event){var elem=this;clearTimeout(data[group]);data[group]=setTimeout(function(){over.call(elem,event)},speed)},function(event){var elem=this;clearTimeout(data[group]);data[group]=setTimeout(function(){out.call(elem,event)},speed)});return this};return this};$.fn[plugin+'Pause']=function(){clearTimeout(this.data(etid));return this};$[plugin]={get:function(){return id++},pause:function(group){clearTimeout(data[group])}}})(jQuery,'mouseDelay');
/*
/*by wanhu 20201020
*/
var whirmenu={
    main:function(type,lang,selectid){
        whirmenu.public(lang,selectid);
        whirmenu.menutype(type);
    },
    public:function(lang,selectid){
     $("#m"+selectid).addClass("aon");//头部导航选中当前栏目
         var _li=$("#menu li");
        if(lang=='cn'){
            var num=_li.length,
             w=100/num;
            _li.outerWidth(w+"%");
        }
    //判断是否有下拉
     _li.each(function(){
         var n=$(this).find("dt").length
         if(n>=1){
             $(this).addClass("has-sub");
         }
   //改变选中 鼠标经过去掉当前栏目选中
        $(this).hover(function(){
            $(this).addClass("aon").siblings().removeClass("aon");
        },function(){
            $(this).removeClass("aon");
            $("#m"+selectid).addClass("aon");
        });
     });
    },
    menutype:function(type){
     $(function ($) {
          $(window).on("resize", function () {
            if($(window).width()>1025){
                whirmenu.pc(type);
            }
            else{
                 whirmenu.wap();
            }
          }).trigger("resize");
      });
    },
    pc:function(type){
       //纵向
      if(type=="Vertical"){
          $(".has-sub").mouseDelay(false).hover(function(){
              $(this).find(".sub").slideDown(300);
              $(this).siblings().find(".sub").slideUp(300);
              //判断下拉框内容宽度超出屏幕
              var subw=$(this).find(".sub").outerWidth(),
               this_pleft=$(this).position().left+$(this).outerWidth(),
               ww=$(window).width(),
               right_w=ww-this_pleft
              //console.log(this_pleft,right_w)
              if(subw>=right_w){
                  var leftcss=subw-right_w
                  $(this).find(".sub").css("left",-leftcss);
              }else{
                  $(this).find(".sub").css("left","");
              }
              //判断下拉框内容宽度超出屏幕 END
          },function(){
              $(this).find(".sub").slideUp(300);
          });
          $("#menu").mouseleave(function(){
              $(this).find(".sub").slideUp(300);
          });
      }
        //横向
        
        //左侧竖向菜单 子菜单竖排
        if(type=='leftVertical'){
            $(".has-sub").mouseDelay(false).hover(function(){
                $(this).find(".sub").fadeIn(300);
                $(this).siblings().find(".sub").hide();
                var th=$(this).offset().top-$(document).scrollTop();
                var wh=$(window).height()
                var dlh=$(this).find(".sub dl").height()
                //判断越近底部高度减少 栏目高度超出 
                if(th+dlh>wh){
                    $(this).find(".sub dl").css("padding-top",th-(th+dlh-wh))
                }
                else{
                    $(this).find(".sub dl").css("padding-top",th)
                }
            },function(){
                $(this).find(".sub").fadeOut(300);
                $(this).find(".sub dl").removeAttr("style");
            });
        }
    },
    wap:function(){
          $("#menu li,#menu").unbind();
          $(".has-sub .op").remove();
          $(".has-sub").find("span").append("<i class='op'></i>");
          $('.op').click(function(){
              $(this).toggleClass("click");
              $(this).parent().next(".sub").slideToggle();
              $(this).parent().parent().siblings().find(".op").removeClass("click");
              $(this).parent().parent().siblings().find(".sub").slideUp();
          });
        //打开移动端导航
              $(".open-menu").unbind();
             whirOpen.one(".open-menu","body","menu-show","#menu");
        //end
    }
}
var whirOpen={
    one:function(a,b,bclass,c){
           $(a).click(function(e) {
                  $(this).toggleClass("on");
                  $(b).toggleClass(bclass);
                      $(document).on("click", function() {
                         $(b).removeClass(bclass);
                         $(a).removeClass("on");
                      });
                      e.stopPropagation();
                  });
              $(c).on("click", function(e) {
                 e.stopPropagation();
         });
    }
}
var whirsearch={
    open:function(a,b){  
	$(a).click(function(e) {
       $(this).toggleClass("on");
		$(b).stop(true, true).fadeToggle();
			$(document).on("click", function() {
			   $(b).stop(true, true).fadeOut();
			   $(a).removeClass("on");
			});
		    e.stopPropagation();
		});
	$(b).on("click", function(e) {
	   e.stopPropagation();
	});
    },
    search:function(a,b,c,d){
       $(a).jqSearch({
        TxtVal: c,
        KeyTxt1: "输入关键词搜索！",
        KeyTxt2: "输入的关键词字数不要过多！",
        KeyTxt3: "您输入的内容存在特殊字符！",
        KeyId: b, //输入框id
        KeyUrl: d, //跳转链接
        KeyHref: "key", //链接传值
        Static: false //是否静态站
    });
    }
}