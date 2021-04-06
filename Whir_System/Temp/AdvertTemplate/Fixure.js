function EzShopAD(PositionID) {{
    this.ID = PositionID;
    this.ADID = 0;
    this.ADType = "";
    this.ADName = "";
    this.ADContent = "";
    this.PaddingLeft = 0;
    this.PaddingTop = 0;
    this.Width = 0;
    this.Height = 0;
    this.IsHitCount = "Y";
    this.Scroll = "N";
    this.Align = "N";
    this.UploadFilePath = "";
    this.URL = "";
    this.SiteId = 0;
    this.ShowAD = showADContent;
}}

var delta = 0.08

function showADContent() {{
    var content = this.ADContent;
    var str = "<div id='ZCMSAD_" + this.ADID + "' style='left:" + this.PaddingLeft + "px;top:" + this.PaddingTop + "px;width:" + this.Width + "px; height:" + this.Height + "px; position: absolute;visibility: visible;z-index:777777;'>";
    var AD = eval('(' + content + ')');
    if (this.ADType == "image") {{
        str += "<a href='" + this.URL + "?SiteId=" + this.SiteId + "&ADID=" + this.ADID + "&URL=" + AD.Images[0].ImgADLinkUrl + "' target='" + ((AD.ImgADLinkTarget == "Old") ? "_self" : "_blank") + "'>";
        str += "<img title='" + AD.Images[0].ImgADAlt + "' src='" + this.UploadFilePath + AD.Images[0].ImgPath + "' width='" + this.Width + "' height='" + this.Height + "' onerror=\"this.src='{11}res/images/nofile.jpg'\"  style='border:0px;'>";
        str += "</a>";
    }} else if (this.ADType == "flash") {{
     document.write("&nbsp;");//至少要输出一个字，不然IE不显示
       str += "<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' width='"+this.Width+"' height='"+this.Height+"' id='FlashAD_"+this.PosID+"' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0'>";
	  str += "<param name='movie' value='"+this.UploadFilePath+AD.Images[0].ImgPath+"' />"; 
      str += "<param name='quality' value='autohigh' />";
      str += "<param name='wmode' value='opaque'/>";
	  str += "<embed wmode='opaque' src='"+this.UploadFilePath+AD.Images[0].ImgPath+"' quality='autohigh' name='flashad' swliveconnect='TRUE' pluginspage='http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash' type='application/x-shockwave-flash' width='"+this.Width+"' height='"+this.Height+"'></embed>";
      str += "</object>";
    }}
    str += "<div style='text-align:right;'><a href='#;' class='aclose' onclick='javascript:document.getElementById(\"ZCMSAD_" + this.ADID + "\").style.display=\"none\"'>关闭</a></div>";
    str += "</div>";
    document.write(str);
    setInterval("playFixureAD(\"" + this.Align + "\",\"ZCMSAD_" + this.ADID + "\")", 10);
}}

function playFixureAD(Align, ADID) {{
    var followObj = document.getElementById(ADID);
    var followObj_x = 0;
    var followObj_y = 0;
    if (Align == "Y") {{
        followObj_x = parseInt((document.documentElement.clientWidth / 2) - (followObj.clientWidth / 2));
        followObj_y = parseInt((document.documentElement.clientHeight / 2) - (followObj.clientHeight / 2));
        if (followObj.offsetLeft != (document.documentElement.scrollLeft + followObj_x)) {{
            var dx = (document.documentElement.scrollLeft + followObj_x - followObj.offsetLeft) * delta;
            dx = (dx > 0 ? 1 : -1) * Math.ceil(Math.abs(dx));
            followObj.style.left = (followObj.offsetLeft + dx) + "px";
        }}
        if (followObj.offsetTop != (document.documentElement.scrollTop + followObj_y)) {{
            var dy = (document.documentElement.scrollTop + followObj_y - followObj.offsetTop) * delta;
            dy = (dy > 0 ? 1 : -1) * Math.ceil(Math.abs(dy));
            followObj.style.top = (followObj.offsetTop + dy) + "px";
        }}
    }}
}}

var ezshopAD_{0} = new EzShopAD('ezshopAD_{0}');
ezshopAD_{0}.ADID = {0};
ezshopAD_{0}.ADType = "{1}";
ezshopAD_{0}.ADName = "{2}";
ezshopAD_{0}.ADContent = "{{'Images':[{3}],'ImgADLinkTarget':'{4}','Count':'{5}','showAlt':'Y'}}";
ezshopAD_{0}.URL = "ADClick.aspx";
ezshopAD_{0}.SiteId = 206;
ezshopAD_{0}.PaddingLeft = {9};
ezshopAD_{0}.PaddingTop = {10};
ezshopAD_{0}.Scroll = 'Y';
ezshopAD_{0}.Align = 'Y';
ezshopAD_{0}.Width = {6};
ezshopAD_{0}.Height = {7};
ezshopAD_{0}.UploadFilePath = "{8}";
ezshopAD_{0}.ShowAD();

var isIE=!!window.ActiveXObject; 
if (isIE){{

	if (document.readyState=="complete"){{
		ezshopAD_{0}.Stat();
	}} else {{
		document.onreadystatechange=function(){{
			if(document.readyState=="complete") ezshopAD_{0};
		}}
	}}
}} else {{
	ezshopAD_{0}.Stat();
}}