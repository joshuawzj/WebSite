;
(function() {
	let fixedScroll = function($fixedEl, opts) {
		this.defaults = {
			navEls: '',
			hookEls: '',
			hookOffset: 0,
			offset: 0,
			stickEndEl: '',
			callback: ''
		};
		$.extend(this, this.defaults, opts);
		this.flag = true;
		this.stickTop = 0;
		this.init_stickTop = 0;
		this.stickBottom = 9999999;
		this.fixedEl = $fixedEl;
		this.fixedElH = $fixedEl.height();
		this.fixedElW = $fixedEl.width();
		this.fixedElL = $fixedEl.offset().left;
		this.winEl = $(window);
		this.offset = parseInt(this.offset || 0);
		this.hookArea = [];
		this.isClickSwitch = false;
	}
	fixedScroll.prototype = {
		init: function() {
			if(this.fixedEl.length > 0) {
				this.stickTop = this.fixedEl.offset().top;
				this.init_stickTop = this.stickTop;
			}
			if(this.stickEndEl.length > 0) {
				this.stickBottom = this.stickEndEl.offset().top;
			}
			this.distance = this.stickBottom - this.stickTop - this.fixedElH - this.offset / 2;
			this.calcArea();
			this.flag && this.events();
			this.flag = false;
		},
		stickHandle: function() {
			if(this.winEl.scrollTop() > this.stickTop - this.offset) {
				if(this.winEl.scrollTop() < this.stickBottom - this.fixedElH - this.offset) {
					/*this.fixedEl.css({
						"position": "fixed",
						"top": this.offset,
						"left": this.fixedElL,
						"width": this.fixedElW,
						"height": this.fixedElH,
						"transform": "translateY(0)"
					});*/
					this.fixedEl.addClass('fix')
					
					typeof this.callback == 'function' && this.callback(1);
				} else {
					/*this.fixedEl.css({
						"top": "auto",
						"left": "auto",
						"position": "static",
						"transform": "translateY(" + this.distance + "px)"
					});*/
					this.fixedEl.removeClass('fix')
					typeof this.callback == 'function' && this.callback(0);
				}
			} else {
				/*this.fixedEl.css({
					"top": "auto",
					"position": "static"
				});*/
				
				this.fixedEl.removeClass('fix')
				typeof this.callback == 'function' && this.callback(0);
			}
		},
		resizeHeight: function(hasNewTop) {
			if(this.fixedEl.length > 0) {
				this.stickTop = hasNewTop ? this.fixedEl.offset().top : this.init_stickTop;
			}
			if(this.stickEndEl.length > 0) {
				this.stickBottom = this.stickEndEl.offset().top;
			}
			this.distance = this.stickBottom - this.stickTop - this.fixedElH - this.offset / 2;
			this.calcArea();
		},
		calcArea: function() {
			if(this.hookEls.length <= 0) return;
			let areas = [];
			this.hookEls.each(function(i, ele) {
				var start = $(this).offset().top;
				var end = start + $(this).height();
				areas.push([start, end]);
			})
			this.hookArea = areas;
		},
		hookScroll: function() {
			var t = this.winEl.scrollTop();
			for(var i in this.hookArea) {
				if((t > this.hookArea[i][0] - this.hookOffset) && t < this.hookArea[i][1]) {
					this.navStatus(i)
				} else {
					this.navStatus()
				}
			}
		},
		navStatus: function(i) {
			if(i || +i === 0) {
				this.navEls.eq(i).addClass('onli').siblings().removeClass('onli');
			} else {
				this.navEls.eq(i).removeClass('onli');
			}
		},
		refresh: function(i) {
			let top = this.hookArea[i][0] - this.hookOffset;
			this.calcArea();
			this.scrollTop(top, 120);
		},
		scrollTop: function(scrollTo, time) {
			var scrollTop = document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop;
			var scrollFrom = parseInt(scrollTop),
				i = 0,
				step = 5;
			scrollTo = parseInt(scrollTo);
			time /= step;
			var interval = setInterval(function() {
				i++;
				let top = (scrollTo - scrollFrom) / time * i + scrollFrom;
				document.body.scrollTop = top;
				document.documentElement.scrollTop = top;
				if(i >= time) {
					clearInterval(interval);
				}
			}, step)
		},
		events: function() {
			let _this = this;
			if(_this.navEls.length > 0) {
				this.navEls.on('click', function() {
					let i = $(this).index();
					_this.isClickSwitch = true;
					_this.refresh(i);
					_this.navStatus(i);
					setTimeout(function() {
						_this.isClickSwitch = false;
					}, 300);
				})
			}
			this.winEl.on("scroll", function() {
				(_this.fixedEl.length > 0) && _this.stickHandle();
				(_this.hookEls.length > 0 && !_this.isClickSwitch) && _this.hookScroll();
			});
		}
	}
	$.fn.fixedScroll = function(opts) {
		let drag = new fixedScroll(this, opts);
		drag.init();
		return drag;
	}
	if(typeof module !== 'undefined' && typeof exports === 'object') {
		module.exports = fixedScroll;
	} else if(typeof define === 'function' && (define.amd || define.cmd)) {
		define(function() {
			return fixedScroll;
		})
	} else {
		window.fixedScroll = fixedScroll;
	}
}).call(function() {
	return(typeof window !== 'undefined' ? window : global)
}, $)