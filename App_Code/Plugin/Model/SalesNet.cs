/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：SalesNet.cs
 * 文件描述：销售网络实体
 */
using System;
using Whir.Repository;

namespace Plu.Domain
{
    [TableName("Whir_Plu_SalesNet")]
    [PrimaryKey("PID", sequenceName = "seq_ezEIP")]

    public class SalesNet : Whir.Domain.DomainBase
    {

        /// <summary>
        /// 主键
        /// </summary>
        public int PID { get; set; }

        /// <summary>
        /// 所属地区
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactInfo { get; set; }

    }
}
