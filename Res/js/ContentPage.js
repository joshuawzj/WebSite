
// -------------------------------------------------------------------
// 用于置标内容页分页使用，直接引用这个js
//注：virtualpaginate,分页内容必须包裹在elementType.innerHTML中，
// -------------------------------------------------------------------


var virtualpaginate = {
    init:function(className, chunksize, elementType) {
        elementType = (typeof elementType == "undefined") ? "div" : elementType; //The type of element used to divide up content into pieces. Defaults to "div"
        this.pieces = virtualpaginate.collectElementbyClass(className, elementType); //get total number of divs matching class name

        //Set this.chunksize: 1 if "chunksize" param is undefined, "chunksize" if it's less than total pieces available, or simply total pieces avail (show all)
        this.chunksize = (typeof chunksize == "undefined") ? 1 : (chunksize > 0 && chunksize < this.pieces.length) ? chunksize : this.pieces.length;
        this.pagecount = Math.ceil(this.pieces.length / this.chunksize); //calculate number of "pages" needed to show the divs


        this.showpage(-1); //show no pages (aka hide all)
        this.currentpage = 0; //Having hidden all pages, set currently visible page to 1st page
        this.showpage(this.currentpage); //Show first page
    },
    
// -------------------------------------------------------------------
// PRIVATE: collectElementbyClass(classname)- Returns an array containing DIVs with the specified classname
// -------------------------------------------------------------------
    collectElementbyClass: function(classname, element) { //Returns an array containing DIVs with specified classname
        var classnameRE = new RegExp("(^|\\s+)" + classname + "($|\\s+)", "i"); //regular expression to screen for classname within element
        var pieces = [];
        var alltags = document.getElementsByTagName(element);
        for (var i = 0; i < alltags.length; i++) {
            if (typeof alltags[i].className == "string" && alltags[i].className.search(classnameRE) != -1)
                pieces[pieces.length] = alltags[i];
        }
        return pieces;
    },
    // -------------------------------------------------------------------
// PUBLIC: showpage(pagenumber)- Shows a page based on parameter passed (0=page1, 1=page2 etc)
// -------------------------------------------------------------------
    showpage: function(pagenumber) {
        var totalitems = this.pieces.length; //total number of broken up divs
        var showstartindex = pagenumber * this.chunksize; //array index of div to start showing per pagenumber setting
        var showendindex = showstartindex + this.chunksize - 1; //array index of div to stop showing after per pagenumber setting
        for (var i = 0; i < totalitems; i++) {
            if (pagenumber == -2) { //查看全部
                this.pieces[i].style.display = "block";
                document.getElementById("whirshowpage").style.display = "none";
            } else {
                if (i >= showstartindex && i <= showendindex)
                    this.pieces[i].style.display = "block";
                else
                    this.pieces[i].style.display = "none";
            }
        }
        this.currentpage = parseInt(pagenumber);
        if (this.cpspan) //if <span class="paginateinfo> element is present, update it with the most current info (ie: Page 3/4)
            this.cpspan.innerHTML = 'Page ' + (this.currentpage + 1) + '/' + this.pagecount;
    },
    // -------------------------------------------------------------------
// PRIVATE: paginate_build_() methods- Various methods to create pagination interfaces
// paginate_build_selectmenu(paginatedropdown)- Accepts an empty SELECT element and turns it into pagination menu
// paginate_build_regularlinks(paginatelinks)- Accepts a collection of links and screens out/ creates pagination out of ones with specific "rel" attr
// paginate_build_flatview(flatviewcontainer)- Accepts <span class="flatview"> element and replaces it with sequential pagination links
// paginate_build_cpinfo(cpspan)- Accepts <span class="paginateinfo"> element and displays current page info (ie: Page 1/4)
    paginate_build_selectmenu: function(paginatedropdown) {
        var instanceOfBox = this;
        this.selectmenupresent = 1;
        for (var i = 0; i < this.pagecount; i++)
            paginatedropdown.options[i] = new Option("Page " + (i + 1) + " of " + this.pagecount, i);
        paginatedropdown.selectedIndex = this.currentpage;
        paginatedropdown.onchange = function() {
            instanceOfBox.showpage(this.selectedIndex);
        };
    },
    paginate_build_flatview: function(flatviewcontainer) {
        var instanceOfBox = this;
        var flatviewhtml = "";
        for (var i = 0; i < this.pagecount; i++)
            flatviewhtml += '<a href="#flatview" rel="' + i + '">' + (i + 1) + '</a> '; //build sequential pagination links
        flatviewcontainer.innerHTML = flatviewhtml;
        this.flatviewlinks = flatviewcontainer.getElementsByTagName("a");
        for (var i = 0; i < this.flatviewlinks.length; i++) {
            this.flatviewlinks[i].onclick = function() {
                try {
                    instanceOfBox.flatviewlinks[instanceOfBox.currentpage].className = "" //"Unhighlight" last flatview link clicked on...
                } catch(ex) {
                }
                this.className = "selected"; //while "highlighting" currently clicked on flatview link (setting its class name to "selected"
                instanceOfBox.showpage(this.getAttribute("rel"));
                return false;
            };
        }
        this.flatviewlinks[this.currentpage].className = "selected"; //"Highlight" current page
        this.flatviewpresent = true; //indicate flat view links are present
    },
    paginate_build_cpinfo: function(cpspan) {
        this.cpspan = cpspan;
        cpspan.innerHTML = 'Page ' + (this.currentpage + 1) + '/' + this.pagecount;
    },
    // -------------------------------------------------------------------
// PRIVATE: buildpagination()- Create pagination interface by calling one or more of the paginate_build_() functions
// -------------------------------------------------------------------

    buildpagination: function(divid) {
    var instanceOfBox = this;
    var paginatediv = document.getElementById(divid);
    if (this.chunksize == this.pieces.length) { //if user has set to display all pieces at once, no point in creating pagination div
        paginatediv.style.display = "none";
        return;
    }
    var paginationcode = paginatediv.innerHTML; //Get user defined, "unprocessed" HTML within paginate div
    if (paginatediv.getElementsByTagName("select").length > 0) //if there's a select menu in div
        this.paginate_build_selectmenu(paginatediv.getElementsByTagName("select")[0]);
    if (paginatediv.getElementsByTagName("a").length > 0) //if there are links defined in div
        this.paginate_build_regularlinks(paginatediv.getElementsByTagName("a"));
    var allspans = paginatediv.getElementsByTagName("span"); //Look for span tags within passed div
    for (var i = 0; i < allspans.length; i++) {
        if (allspans[i].className == "flatview")
            this.paginate_build_flatview(allspans[i]);
        else if (allspans[i].className == "paginateinfo")
            this.paginate_build_cpinfo(allspans[i]);
    }
    this.paginatediv = paginatediv;
},
    paginate_build_regularlinks:function(paginatelinks) {
    var instanceOfBox = this;
    for (var i = 0; i < paginatelinks.length; i++) {
        var currentpagerel = paginatelinks[i].getAttribute("rel");
        if (currentpagerel == "previous" || currentpagerel == "next" || currentpagerel == "first" || currentpagerel == "last" || currentpagerel == "all") //screen for these "rel" values
            paginatelinks[i].onclick = function() {
                instanceOfBox.navigate(this.getAttribute("rel"));
                return false;
            };
    }
},
    // -------------------------------------------------------------------
// PRIVATE: navigate(keyword)- Calls this.showpage() with the currentpage property preset based on entered keyword
// -------------------------------------------------------------------

    navigate : function(keyword) {

        if (this.flatviewpresent) {
            try {
                this.flatviewlinks[this.currentpage].className = ""; //"Unhighlight" previous page (before this.currentpage increments)
            } catch(ex) {
            }
        }
        if (keyword == "previous")
            this.currentpage = (this.currentpage > 0) ? this.currentpage - 1 : (this.currentpage == 0) ? this.pagecount - 1 : 0;
        else if (keyword == "next")
            this.currentpage = (this.currentpage < this.pagecount - 1) ? this.currentpage + 1 : 0;
        else if (keyword == "first")
            this.currentpage = 0;
        else if (keyword == "last")
            this.currentpage = this.pieces.length - 1;
        else if (keyword == "all") {

            this.currentpage = -2;
        }
        // try{
        this.showpage(this.currentpage);
        if (this.selectmenupresent)
            this.paginatediv.getElementsByTagName("select")[0].selectedIndex = this.currentpage;
        if (this.flatviewpresent) {
            try {
                this.flatviewlinks[this.currentpage].className = "selected"; //"Highlight" current page
            } catch(ex) {
            }
        }
    }
};

// -------------------------------------------------------------------
//kindEditor内容分页
// 用于置标内容页分页使用，解析kindEditor的分页符，直接引用这个js
//注：init的参数con需要分页的内容的容器
// 
var ContentPage = {
    contentArry: new Array(),
    currPage: 1,
    container: null,
    pageContent: "",
    init: function (con,urlpage) {
        if (con)
            this.container = con;
        if (urlpage)
            this.currPage = this.getQueryString(urlpage,1);
        this.pageContent = jQuery(this.container).html();
        var regstr = new RegExp('<hr style="page-break-after:always;" class="ke-pagebreak">', 'i');
        this.contentArry = this.pageContent.split(regstr);

        if (this.contentArry.length > 1) {
            var pageText = "<div class='paginationstyle'>";
            for (var i = 0; i < this.contentArry.length; i++) {
                if (urlpage) {
                    var tempUrl = this.addUrlParam(urlpage, (i + 1));
                    pageText += "<a href='" + tempUrl + "' class=''>" + (i + 1) + "</a>";
                }else
                    pageText += "<a href='#' onclick='return ContentPage.showpage(" + i + ")' class=''>" + (i + 1) + "</a>";
               
            }
            jQuery(this.container).html(this.contentArry[this.currPage-1]);
            jQuery(this.container).after(pageText + "</div>");
            jQuery(this.container).next(".paginationstyle").find("a").eq(this.currPage - 1).addClass("selected");
        }
        jQuery(this.container).find(".ke-pagebreak").hide();
    },
    showpage: function (pageno) {
        jQuery(this.container).html(this.contentArry[pageno]);
        jQuery(this.container).next(".paginationstyle").find("a").removeClass("selected");
        jQuery(this.container).next(".paginationstyle").find("a").eq(pageno).addClass("selected");
        return false;
    },
    addUrlParam: function(key,value) {
       
        //获取当前文档的URL,为后面分析它做准备
        var sURL = window.document.URL;
        
        //即arrayParams[1]的值为【first=1&second=2】
        var arrayParams = sURL.split("?");
        var params = "";
        var result = arrayParams[0] + "?";

        //URL中是否包含查询字符串
        if (sURL.indexOf("?") > 0) {
            
            //分解URL,第二的元素为完整的查询字符串
            //分解查询字符串
            //arrayURLParams[0]的值为【first=1 】
            //arrayURLParams[2]的值为【second=2】
            var arrayURLParams = arrayParams[1].split("&");
            //遍历分解后的键值对
            for (var i = 0; i < arrayURLParams.length; i++) {
                //分解一个键值对
                var sParam = arrayURLParams[i].split("=");
                if (sParam[0] != key) {
                    //组装不是要添加的参数
                    params += sParam[0] + "=" + sParam[1] + "&";
                    break;
                }
            }
                result += params;
        }

        result +=key +"=" + value;
        return result;
    },
    getQueryString: function (key,defaultVal) {
        var value = "";
        //获取当前文档的URL,为后面分析它做准备

        var sURL = window.document.URL;
        //URL中是否包含查询字符串

        if (sURL.indexOf("?") > 0) {

            //分解URL,第二的元素为完整的查询字符串

            //即arrayParams[1]的值为【first=1&second=2】

            var arrayParams = sURL.split("?");



            //分解查询字符串

            //arrayURLParams[0]的值为【first=1 】

            //arrayURLParams[2]的值为【second=2】

            var arrayURLParams = arrayParams[1].split("&");
            //遍历分解后的键值对
            for (var i = 0; i < arrayURLParams.length; i++) {
                //分解一个键值对
                var sParam = arrayURLParams[i].split("=");
                if ((sParam[0] == key) && (sParam[1] != "")) {
                    //找到匹配的的键,且值不为空
                    value = sParam[1];
                    break;
                }
            }
        }
        if (value==""&&defaultVal)
            value = defaultVal;
        return value;

    }
};

