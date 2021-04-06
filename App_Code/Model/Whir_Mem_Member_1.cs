/*
* 栏目【会员列表[1]】实体文件，对应表-Whir_Mem_Member
* 文件生成日期: 2021-03-04 10:46:03
*/

using System;
using Whir.Repository;

[TableName("Whir_Mem_Member")]
[PrimaryKey("Whir_Mem_Member_PID", sequenceName = "seq_ezEIP")]
public class Whir_Mem_Member_1 : Whir.Domain.DomainBase
{

	/// <summary>
	/// 主键
	/// </summary>
	public int Whir_Mem_Member_PID { get; set; }

	/// <summary>
	/// 微信号
	/// </summary>
	public string Microscopy { get; set; }

	/// <summary>
	/// 用户名
	/// </summary>
	public string LoginName { get; set; }

	/// <summary>
	/// 密码
	/// </summary>
	public string Password { get; set; }

	/// <summary>
	/// 账户状态
	/// </summary>
	public int? AccountState { get; set; }

	/// <summary>
	/// 安全邮箱
	/// </summary>
	public string Email { get; set; }

	/// <summary>
	/// 剩余天数
	/// </summary>
	public string NumberDay { get; set; }

	/// <summary>
	/// 上次登录时间
	/// </summary>
	public DateTime? LastTime { get; set; }

	/// <summary>
	/// 本次登录时间
	/// </summary>
	public DateTime? ThisTime { get; set; }

	/// <summary>
	/// 所属子站ID
	/// </summary>
	public string SubjectID { get; set; }

	/// <summary>
	/// 栏目编号
	/// </summary>
	public string TypeID { get; set; }
	
	/// <summary>
	/// 到期时间
	/// </summary>
	public DateTime? EndTime { get; set; }
}
