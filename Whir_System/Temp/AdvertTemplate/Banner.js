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
    this.IsHitCount = "N";
    this.UploadFilePath = "";
    this.URL = "";
    this.SiteId = 0;
    this.ShowAD = showADContent;
}}

function showADContent() {{
    var content = this.ADContent;
    var str = "<div id='ZCMSAD_" + this.ADID + "' style='width:" + this.Width + "px; height:" + this.Height + "px; text-align:center; padding-bottom: 6px;'>";
    var AD = eval('(' + content + ')');
    if (this.ADType == "image") {{
        str += "<a href='" + this.URL + "?SiteId=" + this.SiteId + "&ADID=" + this.ADID + "&URL=" + AD.Images[0].ImgADLinkUrl + "' target='" + ((AD.ImgADLinkTarget == "Old") ? "_self" : "_blank") + "'>";
        str += "<img title='" + AD.Images[0].ImgADAlt + "' src='" + this.UploadFilePath + AD.Images[0].ImgPath + "' width='" + this.Width + "' height='" + this.Height + "' onerror=\"this.src='{9}res/images/nofile.jpg'\" style='border:0px;'>";
        str += "</a>";
    }} else if (this.ADType == "flash") {{
    document.write("&nbsp;");//至少要输出一个字，不然IE不显示
        str += "<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' width='"+this.Width+"' height='"+this.Height+"' id='FlashAD_"+this.ADID+"' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0'>";
	  str += "<param name='movie' value='"+this.UploadFilePath+AD.Images[0].ImgPath+"' />"; 
      str += "<param name='quality' value='autohigh' />";
      str += "<param name='wmode' value='opaque'/>";
      str += "<param name='swfversion' value='8.0.35.0' />";
	  str += "<embed src='"+this.UploadFilePath+AD.Images[0].ImgPath+"' quality='autohigh' wmode='opaque' name='flashad' swliveconnect='TRUE' pluginspage='http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash' type='application/x-shockwave-flash' width='"+this.Width+"' height='"+this.Height+"'></embed>";
      str += "</object>";
    }}
    str += "</div>";
    document.write(str);
}}

var ezshopAD_{0} = new EzShopAD('ezshopAD_{0}');
ezshopAD_{0}.ADID = {0};
ezshopAD_{0}.ADType = "{1}";
ezshopAD_{0}.ADName = "{2}";
ezshopAD_{0}.ADContent = "{{'Images':[{3}],'ImgADLinkTarget':'{4}','Count':'{5}','showAlt':'Y'}}";
ezshopAD_{0}.URL = "ADClick.aspx";
ezshopAD_{0}.SiteId = 206;
ezshopAD_{0}.Width = {6};
ezshopAD_{0}.Height = {7};
ezshopAD_{0}.UploadFilePath = "{8}";
ezshopAD_{0}.ShowAD();