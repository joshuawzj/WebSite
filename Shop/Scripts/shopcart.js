/* JavaScript Document
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：shopcart.js
 * 文件描述：购物车操作
 * 
 * 创建标识: lurong 2013-2-18
 * 
 * 修改标识：
*/

var carurl = '';//购物车文件路径

//将商品添加到购物车，如商品在购物车中已存在，则数量以累加的方式处理
function AddCart(pid, aid, num) {

    jQuery.get(carurl + "ajax/AddCart.ashx", { proid: pid, attrproid: aid, qutity: num }, function (data) {
        if (data == "1")
            alert('添加成功！');
        else
            alert(data);
    });
}

function AddCart(cid, num) {

    jQuery.get(carurl + "ajax/AddCart.ashx", { cartid: cid, qutity: num }, function (data) {
 
        return data;

    });
}








