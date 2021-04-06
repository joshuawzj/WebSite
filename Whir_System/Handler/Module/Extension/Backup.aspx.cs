using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Config;
using Whir.Language;
using System.Configuration;
using Whir.Service;


public partial class Whir_System_Handler_Module_Extension_Backup : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetFormString("_action");
        Exec(this, action);
    }

    public EnumType.DbType GetCurrentUseDbType()
    {
        string curDbType = ConfigurationManager.AppSettings.Get("UseDbType");
        if (curDbType.Equals("oracle"))
            return EnumType.DbType.Oracle;
        if (curDbType.Equals("mysql"))
            return EnumType.DbType.MySql;

        return EnumType.DbType.SqlServer;
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("333"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //备份的备注
        string remark = RequestUtil.Instance.GetFormString("Remark");
        //MySQL数据库安装的bin目录
        string mySqlBinPath = RequestUtil.Instance.GetFormString("MySQLBinPath");
        Backup model = Whir.Domain.ModelFactory<Backup>.Insten();
        try
        {
            string filename = "";

            //如果当前使用额SqlServer 数据库
            if (GetCurrentUseDbType().Equals(EnumType.DbType.SqlServer))
            {
                ////数据库服务器主机
                //string dbServerHost = null;
                //string webServerHost = Request.UserHostAddress;

                ////首先判断当前Web服务器是否和SqlServer数据库服务器同一个主机
                //if (!ServiceFactory.BackupService.IsSameHost(out dbServerHost))
                //{
                //    //不同主机
                //    Alert(string.Format("很抱歉，当前使用的是MSSQLServer数据库，不支持远程数据库备份，请到服务器上进行备份<br/>Web服务器主机：{0}<br/>数据库服务器主机：{1}", webServerHost, dbServerHost), "backup.aspx");
                //    return;
                //}

                //filename = "~/dbbackup/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";
                //去掉 ~ / ,,因为HTML的 a href 不能使用服务器端的 ~ ，同时不能写死了，因为IIS上和 本地市不一样的，因为不可以把全部的路径存储到数据中，而是动态根据当前是部署在IIS上，还是在本地
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                filename = "dbbackup/" + time + ".bak";

                model.Remark = remark;
                model.BackupName = time + ".bak";
                model.BackupPath = filename;

                //SQLServer 有问题，执行为 -1 也成功 ，因为执行   cmdText= @"backup database Whir_ezEIP_DEV to disk= '" + backupPath + "'"; 
                EnumBackupMsg value = ServiceFactory.BackupService.ExecuteBackup(AppName + filename, mySqlBinPath);
                if (value == EnumBackupMsg.BackupSuccess)
                {
                    //执行保存
                    ServiceFactory.BackupService.Save(model);

                    //记录操作日志
                    ServiceFactory.BackupService.SaveLog(model, "insert");

                    return new HandlerResult {Status = true, Message = "操作成功".ToLang()};
                }
                else
                    return GetResult(value, model, 0);
            }

            //如果当前使用的是 Oracle数据库
            if (GetCurrentUseDbType().Equals(EnumType.DbType.Oracle))
            {
                return new HandlerResult {Status = false, Message = "很抱歉，当前使用的是Oracle数据库，请到服务器上进行备份".ToLang()};
            }

            //如果当前是 MySQL数据库
            if (GetCurrentUseDbType().Equals(EnumType.DbType.MySql))
            {
                return new HandlerResult { Status = false, Message = "很抱歉，当前使用的是MySQL数据库，请到服务器上进行备份".ToLang() };

                //数据库服务器主机
                string dbServerHost = null;
                string webServerHost = Request.UserHostAddress;

                //首先判断当前Web服务器是否和Oracle数据库服务器同一个主机
                if (!ServiceFactory.BackupService.IsSameHost(out dbServerHost))
                {
                    return new HandlerResult
                    {
                        Status = false,
                        Message =
                            "当前使用的是MySQL数据库，不支持远程数据库备份，请到服务器上进行备份<br/>Web服务器主机：{0}<br/>数据库服务器主机：{1}".ToLang()
                                .FormatWith(webServerHost, dbServerHost)
                    };
                }

                //filename = "~/dbbackup/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".sql";
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                filename = "dbbackup/" + time + ".sql";
                model.Remark = remark;
                model.BackupName = time + ".sql";
                model.BackupPath = filename;
            }

            EnumBackupMsg result = ServiceFactory.BackupService.ExecuteBackup(AppName + filename, mySqlBinPath);

            GetResult(result, model, 0);
        }
        catch (Exception ex)
        {
            return new HandlerResult {Status = false, Message = "备份失败".ToLang()};
        }
        return new HandlerResult {Status = true, Message = "操作成功".ToLang()};
    }

    /// <summary>
    /// 还原数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Restore()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("330"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int backupId = RequestUtil.Instance.GetFormString("backupId").ToInt(0);
        
            if (CurrentDbType.CurDbType == EnumType.DbType.Oracle)
            {
                return new HandlerResult { Status = false, Message = "很抱歉，当前使用的是Oracle数据库，不支持在线还原，请到数据库管理系统中进行还原".ToLang() };
            }

            //数据库服务器主机
            string dbServerHost = null;
            string webServerHost = Request.ServerVariables["LOCAL_ADDR"]; 

            //首先判断当前Web服务器是否和Oracle数据库服务器同一个主机
            if (!ServiceFactory.BackupService.IsSameHost(out dbServerHost))
            {
                //不同主机
                return new HandlerResult { Status = false, Message = "当前使用的是{2}数据库，不支持远程数据库还原，请到服务器上进行还原<br/>Web服务器主机：{0}，<br/>数据库服务器主机：{1}".ToLang().FormatWith(webServerHost, dbServerHost, CurrentDbType.CurDbType.ToStr()) };
            }

            Backup model = ServiceFactory.BackupService.SingleOrDefault<Backup>(backupId);

            if (model != null)
            {
                try
                {
                    //EnumBackupMsg result = ServiceFactory.BackupService.ExecuteRestore(model.BackupPath);
                    //解决IIS和本地路径的问题
                    EnumBackupMsg result = ServiceFactory.BackupService.ExecuteRestore(AppName + model.BackupPath);
                    if (result == EnumBackupMsg.RestoreSuccess)
                    {
                        return new HandlerResult { Status = true, Message = "还原数据库成功".ToLang() };

                    }
                    else
                    {
                      return GetResult(result);
                    }
                }
                catch (Exception ex)
                {
                    return new HandlerResult { Status = false, Message = "由于当前有用户在访问数据库，因此不能执行还原数据库,请您选择适当的时间".ToLang() };

                }
            }
            else
            {
                return new HandlerResult { Status = false, Message = "还原的数据不存在".ToLang() };
            }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult Delete()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("332"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int backupId = RequestUtil.Instance.GetFormString("backupId").ToInt(0);

        //根据Id找到实体对象
        Backup model = ServiceFactory.BackupService.SingleOrDefault<Backup>(backupId);

        if (model != null)
        {
            try
            {
                //删除备份
                EnumBackupMsg result = ServiceFactory.BackupService.ExecuteDelete(model.BackupPath);
                if (result == EnumBackupMsg.DeleteSuccess)
                {
                    bool isSuccess = ServiceFactory.BackupService.Delete<Backup>(backupId) > 0;

                    if (isSuccess)
                    {
                        //记录操作日志
                        ServiceFactory.BackupService.SaveLog(model, "delete");
                        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
                    }
                    else
                    {
                        return new HandlerResult { Status = false, Message = "删除失败".ToLang() };
                    }
                }
                else
                {
                  return GetResult(result);
                }
            }
            catch (Exception ex)
            {
                return new HandlerResult { Status = false, Message = "删除失败".ToLang() + ex.Message };
            }
        }
        else
        {
            return new HandlerResult { Status = false, Message = "找不到删除的文件".ToLang() };
        }
    }

    /// <summary>
    /// 根据备份数据库的信息给出相关的提示
    /// </summary>
    /// <param name="result">数据库备份操作信息枚举类型</param>
    /// <param name="model">实体</param>
    /// <param name="primaryKey">主键</param>
    protected HandlerResult GetResult(EnumBackupMsg result, Backup model, int primaryKey)
    {
        if (result == EnumBackupMsg.BackupDBZero)
        {
            return new HandlerResult { Status = false, Message = "当前备份的数据为空".ToLang() };
        }
        else if (result == EnumBackupMsg.CurrentOracle)
        {
            return new HandlerResult { Status = false, Message = "当前使用的是Oracle数据库，请到Oracle数据库的管理系统中进行备份".ToLang() };
        }
        else if (result == EnumBackupMsg.DifferentHost)
        {
            return new HandlerResult { Status = false, Message = "Web服务器的主机和数据库服务器的主机不同，不能支持远程备份".ToLang() };
        }
        else if (result == EnumBackupMsg.ErrorMySQLBin)
        {
            return new HandlerResult { Status = false, Message = "当前使用的是MySQL数据库，提供的MySQL数据库的bin安装目录路径不存在".ToLang() };
        }
        else if (result == EnumBackupMsg.Fail)
        {
            //由于只有SQLServer才允许远程备份

            string dbServerHost = null;
            string webServerHost = Request.UserHostAddress;

            if (CurrentDbType.CurDbType == EnumType.DbType.SqlServer)
            {
                //首先判断当前Web服务器是否和SqlServer数据库服务器同一个主机
                if (!ServiceFactory.BackupService.IsSameHost(out dbServerHost))
                {
                    return new HandlerResult { Status = false, Message = "当前使用的是SQLServer数据库，远程备份的权限不够！".ToLang() };
                }
            }

            else
            {
                return new HandlerResult { Status = false, Message = "备份失败".ToLang() };
            }
        }
        else if (result == EnumBackupMsg.NonBackup)
        {
            return new HandlerResult { Status = false, Message = "当前备份的数据不存在".ToLang() };
        }
        else if (result == EnumBackupMsg.NonBackupDbPath)
        {
            return new HandlerResult { Status = false, Message = "当前备份数据的路径不存在".ToLang() };
        }
        else if (result == EnumBackupMsg.NonDelDbPath)
        {
            return new HandlerResult { Status = false, Message = "当前需要删除的备份数据不存在".ToLang() };
        }
        else if (result == EnumBackupMsg.NonRestoreDbPath)
        {
            return new HandlerResult { Status = false, Message = "当前需要还原的备份数据不存在".ToLang() };
        }
        else if (result == EnumBackupMsg.Success)
        {
            //执行保存
            ServiceFactory.BackupService.Save(model);
            //记录操作日志
            ServiceFactory.BackupService.SaveLog(primaryKey, "insert");
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        else if (result == EnumBackupMsg.BackupSuccess)
        {
            //执行保存
            ServiceFactory.BackupService.Save(model);
            //记录操作日志
            ServiceFactory.BackupService.SaveLog(primaryKey, "insert");
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        else if (result == EnumBackupMsg.BackupFail)
        {
            return new HandlerResult { Status = false, Message = "备份失败".ToLang() };
        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }


    /// <summary>
    /// 根据备份数据库的信息给出相关的提示
    /// </summary>
    /// <param name="result"></param>
    protected HandlerResult GetResult(EnumBackupMsg result)
    {
        if (result == EnumBackupMsg.BackupDBZero)
        {
            return new HandlerResult { Status = false, Message = "当前备份的数据为空".ToLang() };
        }
        else if (result == EnumBackupMsg.CurrentOracle)
        {
            return new HandlerResult { Status = false, Message = "当前使用的是Oracle数据库，请到Oracle数据库的管理系统中进行备份".ToLang() };
        }
        else if (result == EnumBackupMsg.DifferentHost)
        {
            return new HandlerResult { Status = false, Message = "Web服务器的主机和数据库服务器的主机不同，不能支持远程备份".ToLang() };
        }
        else if (result == EnumBackupMsg.ErrorMySQLBin)
        {
            return new HandlerResult { Status = false, Message = "当前使用的是MySQL数据库，提供的MySQL数据库的bin安装目录路径不存在".ToLang() };
        }
        else if (result == EnumBackupMsg.Fail)
        {
            return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
        }
        else if (result == EnumBackupMsg.NonBackup)
        {
            return new HandlerResult { Status = false, Message = "当前备份的数据不存在".ToLang() };
        }
        else if (result == EnumBackupMsg.NonBackupDbPath)
        {
            return new HandlerResult { Status = false, Message = "当前备份数据的路径不存在".ToLang() };
        }
        else if (result == EnumBackupMsg.NonDelDbPath)
        {
            return new HandlerResult { Status = false, Message = "当前需要删除的备份数据不存在".ToLang() };
        }
        else if (result == EnumBackupMsg.NonRestoreDbPath)
        {
            return new HandlerResult { Status = false, Message = "当前需要还原的备份数据不存在".ToLang() };
        }
        else if (result == EnumBackupMsg.NonDownloadPath)
        {
            return new HandlerResult { Status = false, Message = "当前需要下载的备份数据不存在".ToLang() };
        }
        else if (result == EnumBackupMsg.OnlineUseSystem)
        {
            return new HandlerResult { Status = false, Message = "由于当前有用户在访问数据库，因此不能执行还原数据,请您选择适当的时间，或者请直接到数据库管理系统中还原数据".ToLang() };
        }
        else if (result == EnumBackupMsg.BackupFail)
        {
            return new HandlerResult { Status = false, Message = "备份失败".ToLang() };
        }
        else if (result == EnumBackupMsg.RestoreDbZero)
        {
            return new HandlerResult { Status = false, Message = "需要还原的数据为空".ToLang() };
        }
        else if (result == EnumBackupMsg.RestoreFail)
        {
            return new HandlerResult { Status = false, Message = "还原失败".ToLang() };
        }
        else if (result == EnumBackupMsg.DeleteFail)
        {
            return new HandlerResult { Status = false, Message = "删除失败".ToLang() };
        }
        else if (result == EnumBackupMsg.DownloadDbZero)
        {
            return new HandlerResult { Status = false, Message = "当前下载的数据为空".ToLang() };
        }

        else if (result == EnumBackupMsg.Success)
        {
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }
}