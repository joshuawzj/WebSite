//--------------------------------------------------------------------------------
// 文件描述：二维码实体类
// 文件作者：张清山
// 创建日期：2015-01-21 10:43:25
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using Whir.Domain;
using Whir.Repository;

namespace Plu.Model
{
    /// <summary>
    ///     二维码实体类
    /// </summary>
    [TableName("Whir_Plu_QR")]
    [PrimaryKey("Id")]
    [Serializable]
    public class QR : DomainBase
    {
        /// <summary>
        ///     Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  KEY
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     页面地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 完整Url
        /// </summary>
        public string FullUrl { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImagePath { get; set; }
    }
}