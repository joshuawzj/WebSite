//--------------------------------------------------------------------------------
// 文件描述：访问统计实体类
// 文件作者：张清山
// 创建日期：2015-01-21 11:44:29
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using Whir.Domain;
using Whir.Repository;

namespace Plu.Model
{
    /// <summary> 
    /// 访问统计实体类
    /// </summary>
    [TableName("Whir_Plu_QRVisitStatistics")]
    [PrimaryKey("Id")]
    [Serializable]
    public class QRVisitStatistics : DomainBase
    {


        /// <summary> 
        /// Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary> 
        /// 标题 
        /// </summary>
        public string Title { get; set; }

        /// <summary> 
        /// 页面地址 
        /// </summary>
        public string Url { get; set; }

        /// <summary> 
        /// IP地址 
        /// </summary>
        public string IP { get; set; }

        /// <summary> 
        /// 网络供应商 
        /// </summary>
        public string ISP { get; set; }

        /// <summary> 
        /// IP定位城市 
        /// </summary>
        public string City { get; set; }

        /// <summary> 
        /// 浏览器 
        /// </summary>
        public string Broswer { get; set; }

        /// <summary> 
        /// 客户端设备类型 
        /// </summary>
        public string ClientType { get; set; }

        /// <summary> 
        /// 操作系统 
        /// </summary>
        public string System { get; set; }


    }
}


