//--------------------------------------------------------------------------------
// 文件描述：二维码服务类
// 文件作者：张清山
// 创建日期：2015-01-21 10:45:32
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Security;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Plu.Model;
using ThoughtWorks.QRCode.Codec;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

namespace Plu.Service
{

    public class QRConfig
    {
        /// <summary>
        ///     是否包含图标
        /// </summary>
        public bool IsContainIco { get; set; }

        /// <summary>
        ///     图标文件路径
        /// </summary>
        public string IcoPath { get; set; }
    }
    /// <summary>
    ///二维码服务类
    /// </summary>
    public class QRService : DbBase<QR>
    {
        private static QRService _instance;
        private static readonly object SynObject = new object();

        private QRService()
        {
        }

        /// <summary>
        ///     单例实例
        /// </summary>
        public static QRService Instance
        {
            get
            {
                //线程安全
                lock (SynObject)
                {
                    return _instance ?? (_instance = new QRService());
                }
            }
        }

        /// <summary>
        /// 添加二维码
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Add(QR instance)
        {
            if (Insert(instance).ToInt() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     获取分页数据
        /// </summary>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="conditions">搜索条件</param>
        /// <param name="orderby">排序方式</param>
        /// <returns></returns>
        public Page<QR> Page(int pageIndex, int pageSize, Dictionary<string, string> conditions, string orderby)
        {
            string sql = " WHERE IsDel=0 ";
            var parms = new List<object>();
            if (conditions.Count > 0)
            {
                var i = 0;
                foreach (var condition in conditions)
                {
                    switch (condition.Key.ToLower())
                    {
                        default:
                            if (!condition.Value.IsEmpty())
                            {
                                sql += " AND {0} like '%'+@{1}+'%' ".FormatWith(condition.Key, i);
                                i++;
                                parms.Add(condition.Value);
                            }
                            break;
                    }
                }
            }
            sql += orderby;
            return DbHelper.CurrentDb.Page<QR>(pageIndex, pageSize, sql, parms.ToArray());
        }


        /// <summary>
        /// 修改二维码
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Modify(QR instance)
        {
            if (Update(instance).ToInt() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     删除二维码记录
        /// </summary>
        /// <param name="id">根据ID删除二维码记录</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(int id)
        {
            const string sql = "DELETE FROM Whir_Plu_QR WHERE Id=@0";
            DbHelper.CurrentDb.Execute(sql, id);
            return true;

        }

        /// <summary>
        ///     批量删除二维码实体
        /// </summary>
        /// <param name="ids">根据ID批量删除二维码实体</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(string ids)
        {
            var idArr = ids.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in idArr)
            {
                Delete(id.ToInt());
            }
            return true;

        }


        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GenerateImage(string url)
        {
            var qrCodeEncoder = new QRCodeEncoder();
            const int size = 4;
            const int version = 7;
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            int scale = Convert.ToInt16(size);
            qrCodeEncoder.QRCodeScale = scale;
            qrCodeEncoder.QRCodeVersion = version;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

            String data = url;
            Image image = qrCodeEncoder.Encode(data);
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            string filepath = HttpContext.Current.Server.MapPath("~/UploadFiles/QR/" + fileName);
            string dirPath = Path.GetDirectoryName(filepath);
            if (dirPath != null && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            #region 拼合ICO图标

            var config = GetConfig<QRConfig>();
            if (config.IsContainIco && !string.IsNullOrEmpty(config.IcoPath))
            {
                if (File.Exists(HttpContext.Current.Server.MapPath(config.IcoPath)))
                {
                    var img = Image.FromFile(HttpContext.Current.Server.MapPath(config.IcoPath));
                    CombinImage(image, img);
                }
            }

            #endregion

            image.Save(filepath);
            return (BasePath + "UploadFiles/QR/" + fileName).Replace("//", "/");
        }
        public string BasePath
        {
            get { return VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Request.ApplicationPath); }
        }

        public QR GetByFullUrl(string url)
        {
            var sql = "SELECT * FROM Whir_Plu_QR w WHERE LOWER(w.[FullUrl])=@0";
            return DbHelper.CurrentDb.FirstOrDefault<QR>(sql, url.ToLower());
        }
        public QR GetByKey(string key)
        {
            var sql = "SELECT * FROM Whir_Plu_QR w WHERE w.[Key]=@0";
            return DbHelper.CurrentDb.FirstOrDefault<QR>(sql, key);
        }

        #region 辅助方法

        /// <summary>
        ///     获取配置文件的服务器物理文件路径
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <returns>配置文件路径</returns>
        public static string GetConfigPath<T>()
        {
            return Thread.GetDomain().BaseDirectory + typeof(T).Name + ".config";
        }

        /// <summary>
        ///     更新配置信息，将配置信息对象序列化至相应的配置文件中，文件格式为带签名的UTF-8
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <param name="config">配置信息</param>
        public static void UpdateConfig<T>(T config)
        {
            if (config == null)
            {
                return;
            }
            Type configClassType = typeof(T);
            string configFilePath = GetConfigPath<T>(); //根据配置文件名读取配置文件 
            try
            {
                var xmlSerializer = new XmlSerializer(configClassType);
                using (var xmlTextWriter = new XmlTextWriter(configFilePath, Encoding.UTF8))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    var xmlNamespace = new XmlSerializerNamespaces();
                    xmlNamespace.Add(string.Empty, string.Empty);
                    xmlSerializer.Serialize(xmlTextWriter, config, xmlNamespace);
                }
            }
            catch (SecurityException ex)
            {
                throw new SecurityException(ex.Message, ex.DenySetInstance, ex.PermitOnlySetInstance, ex.Method,
                                            ex.Demanded, ex.FirstPermissionThatFailed);
            }
        }

        /// <summary>
        ///     获取配置信息
        /// </summary>
        /// <typeparam name="T">配置信息类</typeparam>
        /// <returns>配置信息</returns>
        public static T GetConfig<T>() where T : class, new()
        {
            Type configClassType = typeof(T);
            var configObject = new object();
            string configFilePath = GetConfigPath<T>(); //根据配置文件名读取配置文件 
            if (File.Exists(configFilePath))
            {
                using (var xmlTextReader = new XmlTextReader(configFilePath))
                {
                    var xmlSerializer = new XmlSerializer(configClassType);
                    configObject = xmlSerializer.Deserialize(xmlTextReader);
                }
            }
            var config = configObject as T;
            if (config == null)
            {
                return new T();
            }
            return config;
        }

        #endregion

        #region 生成带图片的二维码

        /// <summary>
        ///     合并图片
        /// </summary>
        /// <param name="imgBack"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Image CombinImage(Image imgBack, Image img)
        {
            if (img.Height != 50 || img.Width != 50)
            {
                img = ResizeImage(img, 50, 50, 0);
            }
            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height); //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);   

            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框  

            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);  

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }

        /// <summary>
        ///     调用此函数后使此两种图片合并，类似相册，有个
        ///     背景图，中间贴自己的目标图片
        /// </summary>
        /// <param name="imgBack">粘贴的源图片</param>
        /// <param name="destImg">粘贴的目标图片</param>
        public static Image CombinImage(Image imgBack, string destImg)
        {
            Image img = Image.FromFile(destImg);
            return CombinImage(imgBack, img);
        }

        /// <summary>
        ///     Resize图片
        /// </summary>
        /// <param name="bmp">原始Bitmap</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <param name="mode">保留着，暂时未用</param>
        /// <returns>处理以后的图片</returns>
        public static Image ResizeImage(Image bmp, int newW, int newH, int mode)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量  
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height),
                            GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}


