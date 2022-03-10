//using DotNetCore.Domain;

using SqlSugar;

namespace Architecture.Domain;

public class User : Entity<long>
{
    /// <summary>
    /// 无参构造
    /// </summary>
    public User() { }

    public User
    (
        Name name,
        Email email,
        Auth auth
    )
    {
        Name = name;
        Email = email;
        Auth = auth;
        Activate();
    }

    [SugarColumn(IsPrimaryKey = true, IsNullable = false, IsIdentity = true)]
    public override long Id { get; protected set; } // bigint

    public User(long id) => Id = id;

    /// <summary>
    /// 自定格式的情况 length不要设置
    /// </summary>
    [SugarColumn(ColumnDataType = "Nvarchar(64)", ColumnDescription = "姓名", IsNullable = true)]
    public Name Name { get; private set; }

    /// <summary>
    /// 可以为空
    /// </summary>
    [SugarColumn(Length = 128, ColumnDescription = "电子邮箱", IsNullable = true)]
    public Email Email { get; private set; }

    [SugarColumn(Length = 128, ColumnDescription = "用户状态", IsNullable = false, DefaultValue = "1")]
    public Status Status { get; private set; } // int

    //public long AuthId { get; private set; } // bigint
    public Auth Auth { get; private set; }

    public void Activate()
    {
        Status = Status.Active;
    }

    public void Inactivate()
    {
        Status = Status.Inactive;
    }

    public void Update(string firstName, string lastName, string email)
    {
        Name = new Name(firstName, lastName);
        Email = new Email(email);
    }
}
