using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Architecture.Domain;

/// <summary>
/// IsDisabledDelete禁止删除列
/// IsDisabledUpdateAll=true 那么表存在就不会在执行任何更新操作
/// </summary>
[SugarTable("leng_auth", TableDescription = "身份验证信息", IsDisabledDelete = true, IsDisabledUpdateAll = true)]
public class Auth : Entity<long>
{
    /// <summary>
    /// 具有公共的无参数构造
    /// </summary>
    public Auth() { }

    public Auth
    (
        string login,
        string password,
        Roles roles
    )
    {
        Salt = Guid.NewGuid().ToString("N"); // 79005744e69a4b09996b08fe0b70cbb9 去NM的短横杠
        Login = login;
        Password = password.HMACSHA256(Salt);
        Roles = roles;
    }

    [SugarColumn(ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false, IsIdentity = true)]
    public override long Id { get; protected set; } // bigint

    [Required]
    [SugarColumn(ColumnDataType = "nvarchar(128)", ColumnDescription = "用户名", IsNullable = false, UniqueGroupNameList = new string[] { "unique_auth_username" }, IndexGroupNameList = new string[] { "index_auth_username_password" })]
    public string Login { get; private set; }// userName

    [Required]
    [SugarColumn(ColumnDataType = "nvarchar(1024)", ColumnDescription = "密码", IsNullable = false, IndexGroupNameList = new string[] { "index_auth_username_password" })]
    public string Password { get; private set; }

    [Required]
    [SugarColumn(Length = 64, ColumnDescription = "密码盐", IsNullable = false)]
    public string Salt { get; private set; }

    [Required]
    [SugarColumn(ColumnDataType = "int", ColumnDescription = "角色", IsNullable = false)]
    public Roles Roles { get; private set; } // int

    public void UpdatePassword(string password)
    {
        Password = password;
    }
}
