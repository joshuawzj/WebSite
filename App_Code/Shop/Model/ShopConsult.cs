
using System;

using Whir.Repository;
using Whir.Domain;

namespace Shop.Domain
{
    [TableName("Whir_Shop_Consult")]
    [PrimaryKey("ConsultID", sequenceName = "seq_ezEIP")]
    public class ShopConsult  : DomainBase
    {
        

		/// <summary>
		///主键ID
		/// <summary>
		public int ConsultID { get; set;}

		/// <summary>
		///商品主表主键ID
		/// <summary>
		public int ProID { get; set;}

        /// <summary>
        /// 商品会员表主键ID
        /// </summary>
        public int MemberID { get; set; }

		/// <summary>
		///咨询时售价
		/// <summary>
		public decimal ConsultSaleAmount { get; set;}

		/// <summary>
		///咨询内容
		/// <summary>
		public string Consult { get; set;}

		/// <summary>
		///回复内容
		/// <summary>
		public string Reply { get; set;}

		/// <summary>
		///回复时间
		/// <summary>
		public DateTime? ReplyDate { get; set;}

        /// <summary>
        /// 回复人
        /// </summary>
        public string ReplyUser { get; set; }

        /// <summary>
        /// 咨询商品会员名称
        /// </summary>
        [ResultColumn]
        public string LoginName { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [ResultColumn]
        public string ProName { get; set; }

        /// <summary>
        /// 商品主图
        /// </summary>
        [ResultColumn]
        public string ProImg { get; set; }

        /// <summary>
        /// 销售价格
        /// </summary>
        [ResultColumn]
        public decimal CostAmount { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        [ResultColumn]
        public string ProNO { get; set; }
    }
}
