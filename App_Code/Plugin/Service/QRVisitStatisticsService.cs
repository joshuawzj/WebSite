//--------------------------------------------------------------------------------
// 文件描述：访问统计服务类
// 文件作者：张清山
// 创建日期：2015-01-21 11:47:27
// 修改记录： 
//--------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using Plu.Model;
using Whir.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;
using NPOI.HSSF.UserModel;

namespace Plu.Service
{
    /// <summary>
    ///     访问统计服务类
    /// </summary>
    public class QRVisitStatisticsService : DbBase<QRVisitStatistics>
    {
        private static QRVisitStatisticsService _instance;
        private static readonly object SynObject = new object();

        private QRVisitStatisticsService()
        {
        }

        /// <summary>
        ///     单例实例
        /// </summary>
        public static QRVisitStatisticsService Instance
        {
            get
            {
                //线程安全
                lock (SynObject)
                {
                    return _instance ?? (_instance = new QRVisitStatisticsService());
                }
            }
        }

        /// <summary>
        ///     添加访问统计
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Add(QRVisitStatistics instance)
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
        public Page<QRVisitStatistics> Page(int pageIndex, int pageSize, Dictionary<string, string> conditions,
                                            string orderby)
        {
            string sql = " WHERE IsDel=0 ";
            var parms = new List<object>();
            if (conditions.Count > 0)
            {
                int i = 0;
                foreach (var condition in conditions)
                {
                    switch (condition.Key.ToLower())
                    {
                        case "date":
                            sql += " AND CreateDate {0} ".FormatWith(condition.Value);
                            break;
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
            return DbHelper.CurrentDb.Page<QRVisitStatistics>(pageIndex, pageSize, sql, parms.ToArray());
        }


        /// <summary>
        ///     修改访问统计
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool Modify(QRVisitStatistics instance)
        {
            if (Update(instance).ToInt() > 0)
            {
                return true;
            }
            return false;
        }

        public QRVisitStatistics GetByUrl(string url)
        {
            string sql = "SELECT * FROM Whir_Plu_QRVisitStatistics w WHERE LOWER(w.[Url])=@0";
            return DbHelper.CurrentDb.SingleOrDefault<QRVisitStatistics>(sql, url.ToLower());
        }

        /// <summary>
        ///     删除访问统计记录
        /// </summary>
        /// <param name="id">根据ID删除访问统计记录</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(int id)
        {
            try
            {
                const string sql = "DELETE FROM Whir_Plu_QRVisitStatistics WHERE Id=@0";
                DbHelper.CurrentDb.Execute(sql, id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///     批量删除访问统计实体
        /// </summary>
        /// <param name="ids">根据ID批量删除访问统计实体</param>
        /// <returns>返回true表示删除成功，返回false表示删除失败</returns>
        public static bool Delete(string ids)
        {
            try
            {
                string[] idArr = ids.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string id in idArr)
                {
                    Delete(id.ToInt());
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///     记录访问日志
        /// </summary>
        /// <param name="url"></param>
        public string Log(string url)
        {
            url = url.TrimEnd('/');
            var regex = new Regex(
                "(?i)(\\?|&)_ref=(?<key>\\d+)",
                RegexOptions.IgnoreCase
                | RegexOptions.CultureInvariant
                | RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                );
            string key = regex.Match(url).Groups["key"].Value;
            if (!string.IsNullOrEmpty(key))
            {
                QR qr = QRService.Instance.GetByKey(key);
                if (qr != null)
                {
                    string userAgent = HttpContext.Current.Request.UserAgent ?? "无";
                    QRVisitStatistics log = ModelFactory<QRVisitStatistics>.Insten();
                    log.Title = qr.Name;
                    log.Url = url;
                    log.IP = GetClientIP();
                    IpInfo ipInfo = GetClientInfo(log.IP);
                    log.ISP = ipInfo.isp;
                    log.City = ipInfo.city;
                    log.Broswer = HttpContext.Current.Request.Browser.Browser + "(" +
                                  HttpContext.Current.Request.Browser.Version + ")";
                    log.ClientType = GetClientType();
                    log.System = GetOSNameByUserAgent(userAgent);
                    Instance.Add(log);
                    return "ok";
                }
                return "key not found:" + key;
            }
            return "key is null";
        }

        /// <summary>
        ///     根据 User Agent 获取操作系统名称
        /// </summary>
        private string GetOSNameByUserAgent(string userAgent)
        {
            if (userAgent.Contains("NT 10.0"))
            {
                return "Windows 10";
            }
            if (userAgent.Contains("NT 8.1"))
            {
                return "Windows 8.1";
            }
            if (userAgent.Contains("NT 8.0"))
            {
                return "Windows 8";
            }
            if (userAgent.Contains("NT 6.1"))
            {
                return "Windows 7";
            }
            if (userAgent.Contains("NT 6.0"))
            {
                return "Windows Vista/Server 2008";
            }
            if (userAgent.Contains("NT 5.2"))
            {
                return "Windows Server 2003";
            }
            if (userAgent.Contains("NT 5.1"))
            {
                return "Windows XP";
            }
            if (userAgent.Contains("NT 5"))
            {
                return "Windows 2000";
            }
            if (userAgent.Contains("NT 4"))
            {
                return "Windows NT4";
            }
            if (userAgent.Contains("Me"))
            {
                return "Windows Me";
            }
            if (userAgent.Contains("98"))
            {
                return "Windows 98";
            }
            if (userAgent.Contains("95"))
            {
                return "Windows 95";
            }
            if (userAgent.Contains("Mac"))
            {
                return "Mac";
            }
            if (userAgent.Contains("Unix"))
            {
                return "UNIX";
            }
            if (userAgent.Contains("Linux"))
            {
                return "Linux";
            }
            if (userAgent.Contains("SunOS"))
            {
                return "SunOS";
            }
            return "未知";
        }

        public string GetClientType()
        {
            var variable = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (variable != null)
            {
                var b =
                    new Regex(
                        @"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino",
                        RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var v =
                    new Regex(
                        @"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-",
                        RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if (!(b.IsMatch(variable) || v.IsMatch(variable.Substring(0, 4))))
                {
                    //PC访问   
                    return "电脑";
                }
                if (variable.IndexOf("android", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    return "android";
                }
                if (variable.IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    return "iPhone";
                }
                if (variable.IndexOf("iPad", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    return "iPad";
                }
                if (variable.IndexOf("mac", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    return "mac";
                }
                if (variable.IndexOf("linux", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    return "linux";
                }
                return "手机";
            }
            return string.Empty;
        }

        /// <summary>
        ///     取得客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(ip))
            {
                string strHostName = Dns.GetHostName();
                ip = Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }
            return ip;
        }

        public IpInfo GetClientInfo(string ip)
        {
            try
            {
                string url = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js&ip=" + ip;
                WebRequest request = WebRequest.Create(new Uri(url));
                WebResponse response = request.GetResponse();
                string s = "";
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    s = HttpUtility.HtmlDecode(reader.ReadToEnd());
                    reader.Dispose();
                }
                if ((s != null) && !s.IsEmpty())
                {
                    s = s.Substring(s.IndexOf("=", StringComparison.Ordinal) + 2);
                    s = s.Substring(0, s.Length - 1);
                }
                var serializer = new JavaScriptSerializer();
                var info = serializer.Deserialize<IpInfo>(s);
                return info;
            }
            catch (Exception ex)
            {
                return new IpInfo();
            }
        }

        /// <summary>
        ///     导出
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public string Export(Dictionary<string, string> conditions)
        {
            try
            {
                string sql = @"SELECT  Id,
       Title AS '标题',
       URL AS '页面地址',
       IP AS 'IP地址',
       ISP AS 'ISP运营商',
       city AS '城市',
       Broswer AS '浏览器',
       ClientType AS '设备',
       SYSTEM AS '操作系统' FROM Whir_Plu_QRVisitStatistics  WHERE IsDel=0 ";
                var parms = new List<object>();
                if (conditions.Count > 0)
                {
                    int i = 0;
                    foreach (var condition in conditions)
                    {
                        switch (condition.Key.ToLower())
                        {
                            case "date":
                                sql += " AND CreateDate {0} ".FormatWith(condition.Value);
                                break;
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
                sql += "Order by CreateDate DESC";
                DataTable dt = DbHelper.CurrentDb.Query(sql, parms.ToArray()).Tables[0];
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                return "true|" + ExportDataTableToExcel(dt, fileName);
            }
            catch (Exception ex)
            {
                return "err|" + ex.Message;
            }
        }

        /// <summary>
        ///     将DataTable导出到Excel
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string ExportDataTableToExcel(DataTable dt, string fileName)
        {
            #region 表头

            var hssfworkbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            var hssfSheet = hssfworkbook.CreateSheet(fileName);
            hssfSheet.DefaultColumnWidth = 13;
            hssfSheet.SetColumnWidth(0, 25 * 256);
            hssfSheet.SetColumnWidth(3, 20 * 256);
            // 表头
            var tagRow = hssfSheet.CreateRow(0);
            tagRow.Height = 22 * 20;

            int colIndex;
            for (colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
            {
                tagRow.CreateCell(colIndex).SetCellValue(dt.Columns[colIndex].ColumnName);
            }

            #endregion

            #region 表数据

            // 表数据  
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                DataRow dr = dt.Rows[k];
                var row = hssfSheet.CreateRow(k + 1);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(dr[i].ToString());
                }
            }

            #endregion

            string path = string.Format("{0}Uploadfiles/qr/{1}.xls", HttpContext.Current.Request.PhysicalApplicationPath,
                                        fileName);
            string dirPath = Path.GetDirectoryName(path);
            if (dirPath != null)
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
            var file = new FileStream(path, FileMode.Create);

            hssfworkbook.Write(file);
            file.Close();
            string basePath = VirtualPathUtility.AppendTrailingSlash(HttpContext.Current.Request.ApplicationPath);

            return (basePath + "Uploadfiles/qr/" + fileName + ".xls");
        }
    }

    public class IpInfo
    {
        public string ret { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string isp { get; set; }
        public string type { get; set; }
        public string desc { get; set; }
    }
}