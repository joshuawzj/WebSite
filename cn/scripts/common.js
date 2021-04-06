$(function(){
	
	$("a").focus(function(){this.blur();});
	
	$(".MainNav li").each(function (i, n) {
     if ($(this).find(".NavPull dd").length == 0) {
         $(this).find(".arr").hide();
		 $(this).find(".NavPull").addClass("null");
     }
    });
	//搜索s
	$(".Header .Searchico").click(function() {
	  $(".Header .searchbox").slideToggle();
	  $(this).toggleClass("on");
	  $(".MainNav").removeClass("on");
	  $(".closebtn").fadeOut("on");
	  $(".openbtn").removeClass("on");
    });
	//搜索end
	//
	$(".openbtn").click(function(){
	  $(this).toggleClass("on");
	  $(".MainNav").toggleClass("on");
	  $("body").toggleClass("bodyon");
	  $(".closebtn").slideToggle(0);
	}); 
	//	
	$(".closebtn").click(function(){
	  $(".openbtn").removeClass("on");
	  $(".MainNav").toggleClass("on");
	  $("body").removeClass("bodyon");
	  $(".closebtn").slideToggle(0);
	}); 
	//	
	$(window).scroll(function() {
	if($(window).scrollTop()>=100){
		$(".Header").addClass("fixed");
	}else{
		$(".Header").removeClass("fixed");
	} 
    });
	
	if($(window).width()>998){
	   		$(".MainNav li").hover(function(){
	   		   $(this).find(".NavPull").stop(true,true).delay(100).slideDown();
	   		},function(){
	   		$(this).find(".NavPull").stop(true,true).delay(100).slideUp();
	   		});
			//PC端下拉
			
	   }else{

	}
	
	if($(window).width()<998){
	   		$(".MainNav li").each(function(){
		      var Btn = $(this).find(".arr");
		      Btn.click(function(){
					       var statis = $(this).parents("li").find(".NavPull").css("display");
						   if(statis == "none"){
								  $(this).parents("li").siblings().removeClass("onnav");
								  $(this).parents("li").siblings().find(".NavPull").slideUp();
								  $(this).parents("li").addClass("onnav");
								  $(this).parents("li").find(".NavPull").slideDown();
					   }
						    else{
								   $(this).parents("li").find(".NavPull").slideUp();
								   $(this).parents("li").removeClass("onnav");
					   }
			  });
          }); 
		  //
		  $(".SinglePage img").removeAttr("width").removeAttr("height");

	      }else{

	     }
	//sj端下拉
	
	$(".Footer .share a").hover(function(){
	  $(this).stop(true,true).toggleClass("on");
	});
	$(".FastRight li").hover(function(){
      $(this).stop(true,true).toggleClass("on");
	});
	//
	
	
	$(".demobtn,.demo").click(function(){
      $("#popbox1").fadeIn();
	});
	$(".Popbox .close").click(function(){
      $("#popbox1").fadeOut();
	});
	//BANNER 上面的按钮点击弹窗
	
	$(".swhzbtn").click(function(){
      $("#popbox2").fadeIn();
	});
	$(".Popbox .close").click(function(){
      $("#popbox2").fadeOut();
	});
	//底部商务合作按钮点击弹窗
	
	
	$(".itemhover").hover(function(){
	  $(this).addClass("activehover");
	},function(){
	  $(this).removeClass("activehover");
	});
	$(".itemhover2").hover(function(){
	  $(this).addClass("activehover2");
	},function(){
	  $(this).removeClass("activehover2");
	});
	//滑过显示阴影效果
	
     $(".HomePro .nav li:first").addClass("on");
     $('.HomePro').each(function(){
		var _this=$(this);
		  _this.find(".slide:first").removeClass("nones");
			_this.find(".nav li").on("mouseover",function(){
				var num=$(this).index();
				$(this).addClass("on");
				$(this).siblings().removeClass("on");
				_this.find(".slide").addClass("nones");
				_this.find(".slide").eq(num).removeClass("nones");
			});
		
	});
	//   
    
	$(".LinkCol .nav li:first").addClass("on");
     $('.LinkCol').each(function(){
		var _this=$(this);
		  _this.find(".slide:first").removeClass("nones").addClass("animated fadeInLeft");
			_this.find(".nav li").on("mouseover",function(){
				var num=$(this).index();
				$(this).addClass("on");
				$(this).siblings().removeClass("on");
				_this.find(".slide").addClass("nones").removeClass("animated fadeIn");
				_this.find(".slide").eq(num).removeClass("nones").addClass("animated fadeIn");
			});
		
	});
	// 
	$(".Solutionbox.ry .slider .li:first").addClass("on");
     $('.Solutionbox.ry').each(function(){
		var _this=$(this);
		  _this.find(".imgbox span:first").removeClass("nones").addClass("animated fadeInLeft");
			_this.find(".slider .li").on("mouseover",function(){
				var num=$(this).index();
				$(this).addClass("on");
				$(this).siblings().removeClass("on");
				_this.find(".imgbox span").addClass("nones").removeClass("animated fadeIn");
				_this.find(".imgbox span").eq(num).removeClass("nones").addClass("animated fadeIn");
			});
		
	});
	// 
	$(".Dateline li:first").addClass("on");
     $('.history').each(function(){
		var _this=$(this);
		  _this.find(".slide:first").removeClass("nones").addClass("animated fadeInLeft");
			_this.find(".Dateline li").on("mouseover",function(){
				var num=$(this).index();
				$(this).addClass("on");
				$(this).siblings().removeClass("on");
				_this.find(".slide").addClass("nones").removeClass("animated fadeIn");
				_this.find(".slide").eq(num).removeClass("nones").addClass("animated fadeIn");
			});
		
	});
	// 
   
	
	$(".SubMenu .ColumnName .arr").click(function(){
	  $(this).parent().parent().find("ul").slideToggle();
	  $(this).parent().toggleClass("on");
	});
	//内页二级
	
	
	$(document).ready(function() {
	$('.gallery').each(function() { // the containers for all your galleries
		$(this).magnificPopup({
			delegate: 'a', // the selector for gallery item
			type: 'image',
			gallery: {
			  enabled:true
			}
		});
	});
  });  //弹出放大图层 
	
	$(".ItemList li").each(function(i,item){
	 if(Number(i+1)%2 == 0){
		 $(this).addClass("next");
	 }
	});
	//
	
	$(".Footer .item").each(function(){
		      var Btn = $(this).find(".arr");
		      Btn.click(function(){
					       var statis = $(this).parents(".item").find(".sub").css("display");
						   if(statis == "none"){
								  $(this).parents(".item").siblings().removeClass("on");
								  $(this).parents(".item").siblings().find(".sub").slideUp();
								  $(this).parents(".item").addClass("on");
								  $(this).parents(".item").find(".sub").slideDown();
					   }
						    else{
								   $(this).parents(".item").find(".sub").slideUp();
								   $(this).parents(".item").removeClass("on");
					   }
			  });
     }); 
   //底部栏目 
   
   $(".HrList li").each(function(){
		      var Btn = $(this).find(".jobtitle").find("table");
		      Btn.click(function(){
					       var statis = $(this).parents("li").find(".txtCont").css("display");
						   if(statis == "none"){
								  $(this).parents("li").siblings().removeClass("current");
								  $(this).parents("li").siblings().find(".txtCont").slideUp(500);
								  $(this).parents("li").addClass("current");
								  $(this).parents("li").find(".txtCont").slideDown(500);
					   }
						    else{
								   $(this).parents("li").find(".txtCont").slideUp(500);
								   $(this).parents("li").removeClass("current");
					   }
			  });
	  }); 
    //人才招聘
	
	$(".ProList li").hover(function(){
	  $(this).addClass("on");
	},function(){
	  $(this).removeClass("on");
	});
	//
	
	$(".PrevNextBox dl").each(function() {
     var myhref = $(this).find("a").attr("href");
     if (myhref == "#") {
          $(this).find("a").addClass("none");
          $(this).find("a").removeAttr("href");
     }
     });
	 //新闻详情
	$(".SinglePage img").parent("p span").css("text-indent","0em");
	$(".SinglePage img").parent("p").css("text-indent","0em");
	//
	
    var offset = 200,
		offset_opacity = 1200,
		scroll_top_duration = 1000,
		$back_to_top = $('.totop');

	$(window).scroll(function(){
		( $(this).scrollTop() > offset ) ? $back_to_top.addClass('cd-is-visible') : $back_to_top.removeClass('cd-is-visible');
	});
	$('.totop').click(function(){$('html,body').animate({scrollTop: '0px'}, 800);}); 
    //返回顶部
	

});


(function () {
	var showMoreNChildren = function ($children, n) {
		//显示某jquery元素下的前n个隐藏的子元素
		var $hiddenChildren = $children.filter(":hidden");
		var cnt = $hiddenChildren.length;
		for ( var i = 0; i < n && i < cnt ; i++) {
			$hiddenChildren.eq(i).slideDown();
		}
		return cnt-n;//返回还剩余的隐藏子元素的数量
	}

	//对页中现有的class=showMorehandle的元素，在之后添加显示更多条，并绑定点击行为
	$.showMore = function (selector) {
		if (selector == undefined) { selector = ".showMoreNChildren" } 
		$(selector).each(function () {
			var pagesize = $(this).attr("pagesize") || 10;
			console.log(pagesize);
			$(this).find("li:lt("+pagesize+")").show();
			var $children = $(this).children();
			if ($children.length > pagesize) {
				for (var i = pagesize; i < $children.length; i++) {
					$children.eq(i).hide();
				}
				$("<div class='showMorehandle'>显示更多</div>").insertAfter($(this)).click(function () {
					if (showMoreNChildren($children, pagesize) <= 0) {
						//如果目标元素已经没有隐藏的子元素了，就隐藏“点击更多的按钮条”
						$(this).hide();
					};
				});
			}
		});
	}
})()
