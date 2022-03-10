namespace Architecture.Domain;

[Flags]
public enum Roles
{
    /// <summary>
    /// 无 -> 匿名用户
    /// </summary>
    None = 0,
    /// <summary>
    /// 用户 分免费用户和付费用户
    /// </summary>
    User = 1,
    /// <summary>
    /// 管理员
    /// </summary>
    Admin = 2
}
