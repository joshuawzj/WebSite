/*
 * Copyright © 2009-2013 万户网络技术有限公司
 * 文 件 名：Shop_productinfo.aspx.cs
 * 文件描述：商品详细页面
 *          
 * 
 * 创建标识: 
 * 
 * 修改标识：
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

//非系统
using Whir.Domain;
using Whir.Framework;
using Shop.Domain;
using Shop.Service;
using System.Web.UI.HtmlControls;
using Whir.Repository;
using Whir.Service;

public partial class Shop_productinfo : Shop.Common.PageBase
{
    /// <summary>
    /// 当前编辑的商品ID
    /// </summary>
    protected int ProID
    {
        get
        {
            if (ViewState["ProID"] == null)
            {
                ViewState["ProID"] = 0;
            }
            return ViewState["ProID"].ToInt();
        }

        set
        {
            ViewState["ProID"] = value;
        }
    }
    /// <summary>
    /// 属性商品ID串
    /// </summary>
    protected string AttrValueIDs
    {
        get
        {
            return ViewState["AttrValueIDs"].ToStr();
        }

        set
        {
            ViewState["AttrValueIDs"] = value;
        }
    }
    /// <summary>
    /// 商品主图
    /// </summary>
    protected string ProImg
    {
        get
        {
            return ViewState["ProImg"].ToStr();
        }

        set
        {
            ViewState["ProImg"] = value;
        }
    }
    /// <summary>
    /// 是否为属性商品
    /// </summary>
    protected bool IsAttrPro
    {
        get
        {
            if (ViewState["IsAttrPro"] == null)
            {
                ViewState["IsAttrPro"] = false; 
            }
            return ViewState["IsAttrPro"].ToBoolean();
        }

        set
        {
            ViewState["IsAttrPro"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ProID = RequestUtil.Instance.GetQueryInt("proid", 0);
            AttrValueIDs = Server.UrlDecode(RequestUtil.Instance.GetString("aids"));
            if (ProID > 0)
            {
                BindCategoryLocation();// 绑定当前类别位置
                bindProInfo();// 获取商品信息
                bindProAttr();// 绑定商品规格信息
                bindConsult();// 绑定商品咨询
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('找不到该商品相关信息!');location='productlist.aspx';", true);
            }
        }
    }
    /// <summary>
    /// 绑定当前类别位置
    /// </summary>
    private void BindCategoryLocation()
    {
        ltCategory.Text = "";
        IList<ShopCategory> scList = ShopCategoryService.Instance.GetCategoryListByProID(ProID);
        if (scList.Count > 0)
        {
            for (int i = 0; i < scList.Count; i++)
            {
                ltCategory.Text += "<a href=\"productlist.aspx?categoryid=" + scList[i].CategoryID + "\">" + scList[i].CategoryName + "</a>";
                if (i != scList.Count - 1)
                {
                    ltCategory.Text += "&nbsp;>&nbsp;";
                }
            }
        }
    }

    /// <summary>
    /// 获取商品信息
    /// </summary>
    /// <param name="ProID"></param>
    private void bindProInfo()
    {
        IsAttrPro = false;
        DataTable dt = new DataTable();
        string sql = @"select p.*,a.AttrProID,a.AttrValueIDs,a.AttrValueNames,a.CostAmount as AttrCostAmount,a.ProImg as AttrProImg,a.Images as AttrImages,a.IsUseMainImage,"
        + (CurrentUseDbType == EnumType.DbType.SqlServer ? "ISNULL" : CurrentUseDbType == EnumType.DbType.MySql ? "IFNULL" : "nvl") +
                        "((select min(ap.costamount) from whir_shop_attrpro ap where ap.proid=p.ProID),0) as AttrMinPrice,"+  (CurrentUseDbType == EnumType.DbType.SqlServer ? "ISNULL" : CurrentUseDbType == EnumType.DbType.MySql ? "IFNULL" : "nvl") 
                        +"((select max(ap.costamount) from whir_shop_attrpro ap where ap.proid=p.ProID),0) as AttrMaxPrice,"
        + " (select count(atp.attrproid) from whir_shop_attrpro atp where atp.proid=p.ProID) as AttrCount" +
        @" from Whir_Shop_ProInfo p  left join Whir_Shop_AttrPro a
                        on p.ProID=a.ProID where p.ProID=" + ProID + " ";
        if (!string.IsNullOrEmpty(AttrValueIDs))
        {
            sql += " AND a.AttrValueIDs=@0";
            dt = DbHelper.CurrentDb.Query(sql, ShopAttrValueService.Instance.OrderByIDs(AttrValueIDs)).Tables[0];
        }
        else
        {
            dt = DbHelper.CurrentDb.Query(sql).Tables[0];
        }
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ltProName.Text = dr["ProName"].ToStr();
            ltProNO.Text = dr["ProNO"].ToStr();
            ltCostAmount.Text = Convert.ToDecimal(dr["CostAmount"]).ToString("C");
            if (Convert.ToInt32(dr["AttrCount"]) > 0)
            {
                IsAttrPro = true;
                ltCostAmount.Text = Convert.ToDecimal(dr["AttrMinPrice"]).ToString("C") + "—" + Convert.ToDecimal(dr["AttrMaxPrice"]).ToString("C");
            }
            ltProDesc.Text = dr["ProDesc"].ToStr();
            IsAllowBuy.Visible = dr["IsAllowBuy"].ToBoolean();
            #region 商品图片
            string uploadpath = Whir.Framework.WebUtil.Instance.AppPath() + AppSettingUtil.AppSettings["UploadFilePath"];
            string syspath = Whir.Framework.WebUtil.Instance.AppPath() + Whir.Framework.AppSettingUtil.AppSettings["SystemPath"];
            if (!string.IsNullOrEmpty(AttrValueIDs))//是否为规格商品
            {
                string[] attrimgs = dr["AttrImages"].ToStr() != "" ? dr["AttrImages"].ToStr().Trim('*').Split('*') : new string[] { };
                if (!string.IsNullOrEmpty(dr["AttrProImg"].ToStr()))//是否有设置主图
                {
                    ltProImg.Text = "<img class=\"Img\" jqimg=\"" + CheckImgFile(uploadpath + dr["AttrProImg"].ToStr()) + "\" onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + uploadpath + dr["AttrProImg"].ToStr() + "\" />";
                    ProImg = dr["AttrProImg"].ToStr();
                    // ltImgs.Text += "<li><img onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + uploadpath + dr["AttrProImg"].ToStr() + "\" /></li>";
                }
                else
                {
                    if (attrimgs.Length > 0)
                    {
                        ltProImg.Text = "<img class=\"Img\" jqimg=\"" + CheckImgFile(uploadpath + attrimgs[0]) + "\" onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + uploadpath + attrimgs[0] + "\" />";
                        ProImg = attrimgs[0];
                    }
                }
                if (attrimgs.Length > 0)
                {
                    for (int i = 0; i < attrimgs.Length; i++)
                    {
                        ltImgs.Text += "<li><img onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + uploadpath + attrimgs[i] + "\" /></li>";
                    }
                }
                if (dr["IsUseMainImage"].ToBoolean())
                {
                    string[] imgs = dr["Images"].ToStr() != "" ? dr["Images"].ToStr().Trim('*').Split('*') : new string[] { };
                    if (!string.IsNullOrEmpty(dr["ProImg"].ToStr()))//是否有设置主图
                    {
                        ltProImg.Text = "<img class=\"Img\" jqimg=\"" + CheckImgFile(uploadpath + dr["ProImg"].ToStr()) + "\" onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + uploadpath + dr["ProImg"].ToStr() + "\" />";
                        ProImg = dr["ProImg"].ToStr();
                        //   ltImgs.Text += "<li><img onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + uploadpath + dr["ProImg"].ToStr() + "\" /></li>";
                    }
                    else
                    {
                        if (imgs.Length > 0)
                        {
                            ltProImg.Text = "<img class=\"Img\" jqimg=\"" + CheckImgFile(uploadpath + imgs[0]) + "\" onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + uploadpath + imgs[0] + "\" />";
                            ProImg = imgs[0];
                        }
                    }
                    if (imgs.Length > 0)
                    {
                        for (int i = 0; i < imgs.Length; i++)
                        {
                            ltImgs.Text += "<li><img onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + uploadpath + imgs[i] + "\" /></li>";
                        }
                    }
                }

                //使用规格商品价格
                if (!string.IsNullOrEmpty(dr["AttrCostAmount"].ToStr()))
                {
                    ltCostAmount.Text = Convert.ToDecimal(dr["AttrCostAmount"]).ToString("C");
                }
            }
            else
            {
                string[] imgs = dr["Images"].ToStr().Trim('*').Split('*');
                if (!string.IsNullOrEmpty(dr["ProImg"].ToStr()))//是否有设置主图
                {
                    ltProImg.Text = "<img class=\"Img\" jqimg=\"" + CheckImgFile(uploadpath + dr["ProImg"].ToStr()) + "\" onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + uploadpath + dr["ProImg"].ToStr() + "\" />";
                    ProImg = dr["ProImg"].ToStr();
                    //  ltImgs.Text = "<li><img onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + uploadpath + dr["ProImg"].ToStr() + "\" /></li>";
                }
                else
                {
                    if (imgs.Length > 0)
                    {
                        ltProImg.Text = "<img class=\"Img\" jqimg=\"" + CheckImgFile(uploadpath + imgs[0]) + "\" onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + uploadpath + imgs[0] + "\" />";
                        ProImg = imgs[0];
                    }
                }
                if (imgs.Length > 0)
                {
                    for (int i = 0; i < imgs.Length; i++)
                    {
                        ltImgs.Text += "<li><img onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + uploadpath + imgs[i] + "\" /></li>";
                    }
                }


            }
            if (string.IsNullOrEmpty(ltImgs.Text.Trim()))
            {
                ltProImg.Text = "<img class=\"Img\" jqimg=\"" + syspath + "res/images/nopicture.jpg" + "\" onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + syspath + "res/images/nopicture.jpg" + "\" />";
                ltImgs.Text = "<li><img onerror=\"this.src='" + syspath + "res/images/nopicture.jpg'\" src=\"" + syspath + "res/images/nopicture.jpg"+"\" /></li>";
            }
            #endregion

        }
        else
        {
            if (string.IsNullOrEmpty(AttrValueIDs))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('找不到该商品相关信息!');location='productlist.aspx';", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('找不到该商品相关信息!');location='productinfo.aspx?proid=" + ProID + "&categoryid=" + RequestUtil.Instance.GetQueryInt("categoryid", 0) + "';", true);
            }
        }
    }
    /// <summary>
    /// 检查图片文件是否存在
    /// </summary>
    /// <param name="path">图片文件地址</param>
    /// <returns></returns>
    private string CheckImgFile(string path)
    {
        string filepath = Server.MapPath(path);
        if (System.IO.File.Exists(filepath))
        {
            return path;
        }
        else
        {
            return SysPath + "res/images/nopicture.jpg";
        }
    }
    /// <summary>
    /// 绑定商品规格信息
    /// </summary>
    private void bindProAttr()
    {
        ltProAttr.Text = "";
        IList<ShopAttrPro> sapList = ShopAttrProService.Instance.GetShopAttrProByProID(ProID);
        if (sapList.Count > 0)
        {
            divProAttr.Visible = true;
            string attrValues = "";//所有规格值ID集合
            for (int i = 0; i < sapList.Count; i++)
            {
                attrValues += sapList[i].AttrValueIDs + (i == sapList.Count - 1 ? "" : ",");
            }
            IList<ShopAttrValue> savList = ShopAttrValueService.Instance.GetAttrValueByAttrValueIDs(attrValues); //商品相关所有规格值列表
            IList<ShopAttr> saList = ShopAttrService.Instance.GetShopAttrListByAttrValueIDs(attrValues);//商品相关所有规格列表
            if (saList.Count > 0)
            {
                foreach (ShopAttr sa in saList)
                {
                    ltProAttr.Text += "<li>";
                    ltProAttr.Text += "<dd>" + sa.SearchName + "：</dd>";
                    List<ShopAttrValue> tempsaList = savList.Where(attr => attr.AttrID == sa.AttrID).ToList();
                    if (tempsaList.Count > 0)
                    {
                        ltProAttr.Text += "<dt>";
                        foreach (ShopAttrValue sav in tempsaList)
                        {
                            if (sa.IsShowImage)
                            {
                                ltProAttr.Text += "<a avid=\"" + sav.AttrValueID + "\"><img style=\"height:20px;width:20px;\" onerror=\"this.src='" + SysPath + "res/images/nopicture.jpg'\" src=\"" + UploadFilePath + sav.ShowImage + "\" /></a>";
                            }
                            else
                            {
                                ltProAttr.Text += "<a avid=\"" + sav.AttrValueID + "\">" + sav.AttrValueName + "</a>";
                            }
                        }
                        ltProAttr.Text += "</dt>";
                    }
                    ltProAttr.Text += "</li>";
                }
            }
        }
        else
        {
            divProAttr.Visible = false;
        }
    }
    /// <summary>
    /// 绑定商品咨询
    /// </summary>
    private void bindConsult()
    {
        string SQL = @"SELECT WHIR_SHOP_CONSULT.*,WHIR_MEM_MEMBER.LOGINNAME FROM WHIR_SHOP_CONSULT
                                                LEFT JOIN WHIR_MEM_MEMBER
                                                ON WHIR_SHOP_CONSULT.MEMBERID=WHIR_MEM_MEMBER.WHIR_MEM_MEMBER_PID
                                                WHERE WHIR_SHOP_CONSULT.ISDEL=0 AND WHIR_SHOP_CONSULT.STATE=-1 AND WHIR_SHOP_CONSULT.PROID=@0 ORDER BY WHIR_SHOP_CONSULT.CONSULTID DESC ";
        var list = ShopConsultService.Instance.Page(pager1.PageIndex, pager1.PageSize, SQL, ProID);
        pager1.RecordsTotal = list.TotalItems.ToInt();
        rptConsultList.DataSource = list.Items;
        rptConsultList.DataBind();

    }
    //提交咨询
    protected void Button1_Click(object sender, EventArgs e)
    {
        WebUser.IsLogin("shop/member/login.aspx");
        if (string.IsNullOrEmpty(txtConsult.Text.Trim()))
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('请先输入咨询内容再提交!');location='" + Request.Url.ToStr() + "'", true);
            return;
        }
        if (IsAttrPro && string.IsNullOrEmpty(AttrValueIDs))
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('请选择好规格属性再进行咨询!');location='" + Request.Url.ToStr() + "'", true);
            return;
        }
        ShopConsult sc = new ShopConsult();
        sc.MemberID = WebUser.GetUserValue("Whir_Mem_Member_PID").ToInt();
        sc.CreateUser = WebUser.GetUserValue("LoginName");
        sc.CreateDate = DateTime.Now;
        sc.Consult = txtConsult.Text.Trim();
        decimal costamount = 0m;
        string str = ltCostAmount.Text.Trim('￥').Trim('¥');
        decimal.TryParse(str, out costamount);
        sc.ConsultSaleAmount = costamount;
        sc.ProID = ProID;
        sc.ProName = ltProName.Text;
        sc.ProImg = ProImg;
        ShopConsultService.Instance.Insert(sc);
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('提交咨询成功!');location='" + Request.Url.ToStr() + "'", true);
    }

    /// <summary>
    /// 放入购物车
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddCart_Click(object sender, EventArgs e)
    {
        int result = 0;
        int attrProID = 0;
        int qutity = RequestUtil.Instance.GetFormString("qutity").ToInt(0);
        ShopAttrPro shopAttrPro = ShopAttrProService.Instance.SingleOrDefault<ShopAttrPro>("SELECT * from Whir_Shop_AttrPro WHERE ProID=@0 AND AttrValueIDs=@1", ProID, AttrValueIDs);
        if (shopAttrPro != null)
            attrProID = shopAttrPro.AttrProID;
        result = ShopCartService.Instance.AddCart(ProID, attrProID, qutity);



        if (result == 0)
        {

            ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('已成功放入购物车!');location='" + AppName + "shop/productinfo.aspx?proid=" + ProID + "&aids=" + AttrValueIDs + "'", true);

        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('放入失败!');", true);
        }
    }

}