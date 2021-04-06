/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：OrderList.aspx.cs
 * 文件描述：订单列表
 * 
 * 创建标识: liuyong 2013-02-04
 * 
 * 修改标识：
 */
using System;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Linq;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Shop.Domain;
using Shop.Service;
using Whir.Repository;
using Newtonsoft.Json;

//非系统的引用

public partial class whir_system_Plugin_shop_order_OrderList : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 订单类型，0:全部订单，1：未支付，2：已支付，3：未完成，4：交易完成，5：取消
    /// </summary>
    public int OrderType { get; set; }
    /// <summary>
    /// 支付方式字典
    /// </summary>
    public Dictionary<int, string> PaymentList = new Dictionary<int, string>();

    /// <summary>
    /// 配送方式
    /// </summary>
    public List<ShopCourier> CourierList { get; set; }
    /// <summary>
    /// 支付列表
    /// </summary>
    public DataTable PatList { get; set; }

    /// <summary>
    /// 会员级别列表
    /// </summary>
    public List<MemberGroup> memberGroupList { get; set; }

    /// <summary>
    /// 付款方式json
    /// </summary>
    public string PaymentListjson { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("407"));
        OrderType = RequestUtil.Instance.GetQueryInt("ordertype", 0);
       

        DataTable dt = PayInterface.Common.Tools.GetPayTypeList();//支付列表
        if (!IsPostBack)
        {
            #region 绑定下拉列表值
            memberGroupList = ServiceFactory.MemberGroupService.Query<MemberGroup>("Where IsDel=0 Order By Sort Desc, CreateDate Desc").ToList();//会员级别
            CourierList = ShopCourierService.Instance.Query<ShopCourier>("Where IsDel=0 Order By Sort Desc, CreateDate Desc").ToList();//配送方式
            PatList = dt;
            PaymentListjson = dt.ToBootsTrapTableFilterJson("id", "name");
            #endregion 绑定下拉列表值
        }
    }
   
    /// <summary>
    /// 获取订单流程状态
    /// </summary>
    /// <param name="status">0.交易完成；1.未完成</param>
    /// <param name="isCancel">是否已取消</param>
    /// <returns></returns>
    public string GetOrderStatus(int status, bool isCancel)
    {
        if (isCancel)
        {
            return "已取消";
        }
        switch (status)
        {
            case 0:
                return "交易完成";
            default:
                return "未完成";
        }
    }
    /// <summary>
    /// 拼接数据源sql语句
    /// </summary>
    /// <returns></returns>
    private string GetSQL()
    {
       // msg = "";
        string SQL = "SELECT o.*,m.LoginName FROM Whir_Shop_OrderInfo o LEFT JOIN Whir_Mem_Member m ON o.MemberID=m.Whir_Mem_Member_PID WHERE ";
        StringBuilder sb = new StringBuilder();
        #region 拼接搜索SQL语句
        //if (!OrderNo.IsEmpty())//订单编号
        //{
        //    sb.AppendFormat("o.OrderNo LIKE '%{0}%' AND ", OrderNo);
        //}
        //if (!MemberName.IsEmpty())//购买者
        //{
        //    sb.AppendFormat("m.LoginName LIKE '%{0}%'  AND ", MemberName);
        //}
        //if (ProAmountMin != 0M)//总金额-下限
        //{
        //    sb.AppendFormat("o.ProductAmount >= '{0}'  AND ", ProAmountMin);
        //}
        //if (ProAmountMax != 0M)//总金额-上限
        //{
        //    sb.AppendFormat("o.ProductAmount <= '{0}'  AND ", ProAmountMax);
        //}
        //if (ProAmountMin > ProAmountMax)
        //{
        //    msg = "总金额开始金额不能比结束金额大，否则找不到数据！";
        //}
        //if (CourierID != 0)//配送方式
        //{
        //    sb.AppendFormat("o.CourierID = {0}  AND ", CourierID);
        //}
        //if (!TakeName.IsEmpty())//收货人
        //{
        //    sb.AppendFormat("o.TakeName LIKE '{0}'  AND ", TakeName);
        //}
        //if (PayAmountMin != 0M)//应付金额-下限
        //{
        //    sb.AppendFormat("o.PayAmount >= '{0}'  AND ", PayAmountMin);
        //}
        //if (PayAmountMax != 0M)//应付金额-上限
        //{
        //    sb.AppendFormat("o.PayAmount <= '{0}'  AND ", PayAmountMax);
        //}
        //if (PayAmountMin > PayAmountMax)
        //{
        //    if (!string.IsNullOrEmpty(msg))
        //    {
        //        msg += "\n\r";
        //    }
        //    msg += "应付金额开始金额不能比结束金额大，否则找不到数据！";
        //}
        //if (PaymentID != -1)//支付方式
        //{
        //    sb.AppendFormat("o.PaymentID = {0}  AND ", PaymentID);
        //}
        //if (MemberType != 0)
        //{
        //    if (DbHelper.CurrentDb.ExecuteScalar<int>("SELECT COUNT(*) FROM WHIR_DEV_FORM frm INNER JOIN WHIR_DEV_FIELD fid ON frm.FIELDID=fid.FIELDID WHERE frm.ColumnID=@0 AND FIELDNAME=@1", 1, "MemberType") > 0)
        //    {
        //        sb.AppendFormat("m.MemberType ={0}  AND ", MemberType);
        //    }
        //}
        //if (StartDate != new DateTime(1900, 1, 1))
        //{
        //    string temp = ServiceFactory.DbService.GetDateCompareCondition("Createdate", StartDate.ToString("yyyy-MM-dd HH:mm:ss"), CompareType.Ge);
        //    sb.Append("o." + temp + " AND ");

        //}
        //if (EndDate != new DateTime(1900, 1, 1))
        //{
        //    EndDate = EndDate.AddDays(1);
        //    string temp = ServiceFactory.DbService.GetDateCompareCondition("Createdate", EndDate.ToString("yyyy-MM-dd HH:mm:ss"), CompareType.Le);
        //    sb.Append("o." + temp + " AND ");
        //}
        #endregion 拼接搜索SQL语句

        //#region 拼接订单类型SQL语句
        //switch (OrderType)
        //{
        //    case 0:
        //        break;
        //    case 1:
        //        sb.Append(" IsCancel<>1 AND IsPaid=0 AND ");
        //        break;
        //    case 2:
        //        sb.Append(" IsCancel<>1 AND IsPaid=1 AND");//订单流程状态：0.交易完成；1.新建；2.待发货；3.待收货
        //        break;
        //    case 3:
        //        sb.AppendFormat("  IsCancel<>1 AND Status <> 0 AND ");
        //        break;
        //    case 4:
        //        sb.Append(" IsCancel<>1 AND Status = 0 AND ");
        //        break;
        //    case 5:
        //        sb.Append(" IsCancel=1 AND ");
        //        break;
        //}
        //#endregion

        SQL = SQL + " {0} o.IsDel=0 Order By o.Sort Desc,o.Createdate Desc".FormatWith(sb.ToString());

        return SQL;
    }
    /// <summary>
    /// 获取支付方式名称
    /// </summary>
    /// <param name="paymentId"></param>
    /// <returns></returns>
    public string GetPayment(int paymentId)
    {
        if (PaymentList.Count == 0)
        {
            DataTable dt = PayInterface.Common.Tools.GetPayTypeList();//支付列表
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PaymentList.Add(dt.Rows[i]["id"].ToInt(), dt.Rows[i]["name"].ToStr());
            }
        }

        string payment = string.Empty;
        PaymentList.TryGetValue(paymentId, out payment);
        return payment;
    }
    protected void lbExport_Click(object sender, EventArgs e)
    {
        string SQL = GetSQL();
        DataTable table = DbHelper.CurrentDb.Query(SQL, null).Tables[0];
        if (table.Rows.Count == 0)
        {
            Alert("无数据，不可以导出");
            return;
        }
        DataTable tableExport = new DataTable();
        tableExport.Columns.Add(CreateCol("订单编号"));
        tableExport.Columns.Add(CreateCol("交易日期"));
        tableExport.Columns.Add(CreateCol("商品总额"));
        tableExport.Columns.Add(CreateCol("应付金额"));
        tableExport.Columns.Add(CreateCol("支付状态"));
        tableExport.Columns.Add(CreateCol("支付方式"));
        tableExport.Columns.Add(CreateCol("订单状态"));
        tableExport.Columns.Add(CreateCol("购买会员"));
        tableExport.Columns.Add(CreateCol("收货人"));
        tableExport.Columns.Add(CreateCol("电话号码"));
        tableExport.Columns.Add(CreateCol("邮政编码"));
        tableExport.Columns.Add(CreateCol("收货地址"));
        tableExport.Columns.Add(CreateCol("购买商品信息"));

        for (int i = 0; i < table.Rows.Count; i++)
        {
            DataRow r = tableExport.NewRow();
            r["订单编号"] = table.Rows[i]["OrderNo"].ToStr();
            r["交易日期"] = table.Rows[i]["Createdate"].ToStr();
            r["商品总额"] = table.Rows[i]["ProductAmount"].ToDecimal().ToString("f2");
            r["应付金额"] = table.Rows[i]["PayAmount"].ToDecimal().ToString("f2");
            r["支付状态"] = table.Rows[i]["IsPaid"].ToBoolean() ? "已支付" : "未支付";
            r["支付方式"] = GetPayment(table.Rows[i]["PaymentID"].ToInt());
            r["订单状态"] = GetOrderStatus(table.Rows[i]["Status"].ToInt(), table.Rows[i]["IsCancel"].ToBoolean());
            r["购买会员"] = table.Rows[i]["LoginName"].ToStr();
            r["收货人"] = table.Rows[i]["TakeName"].ToStr();
            r["电话号码"] = table.Rows[i]["TakeTel"].ToStr();
            r["邮政编码"] = table.Rows[i]["TakePostcode"].ToStr();
            r["收货地址"] = table.Rows[i]["TakeAddress"].ToStr();

            StringBuilder sb = new StringBuilder();
            IList<ShopOrderProduct> list = ShopOrderInfoService.Instance.Query<ShopOrderProduct>(
                "SELECT * FROM Whir_Shop_OrderProduct WHERE OrderID=@0 AND IsDel=0 Order By Sort Desc,Createdate Desc"
                , table.Rows[i]["OrderID"].ToInt()
                ).ToList();
            foreach (var p in list)
            {
                sb.AppendFormat("订单商品编号：{0}\n", p.ProNO);
                sb.AppendFormat("订单商品名称：{0}\n", p.ProName);
                sb.AppendFormat("商品金额：{0}\n", p.SaleAmount.ToString("f2"));
                sb.AppendFormat("购买数量：{0}\n", p.Count);
                sb.AppendFormat("小计：{0}\n", (p.SaleAmount * p.Count).ToString("f2"));
            }

            r["购买商品信息"] = sb.ToString();
            tableExport.Rows.Add(r);
        }

        ExcelUtil.CreateExcel(tableExport, DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
    }
    private DataColumn CreateCol(string colName)
    {
        DataColumn col = new DataColumn(colName);
        return col;
    }
}