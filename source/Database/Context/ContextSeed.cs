using SqlSugar;
using Architecture.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Architecture.Database;

public static class ContextSeed
{
    public static void Seed(this IServiceProvider services)
    {
        var unitOfWork = services.GetRequiredService<IUnitOfWork>();
        var client = unitOfWork.GetDbScope();

        // 数据库账号需要有比较高的权限

        // 如果不存在创建数据库
        client.DbMaintenance.CreateDatabase(); // 个别数据库不支持

        client.CodeFirst.InitTables(typeof(Auth)); // SqlSugar CodeFirst
        client.CodeFirst.InitTables(typeof(User)); // .SetStringDefaultLength(128)

        // 没有数据则新增默认数据
        if (!client.Queryable<Auth>().Where(a => a.Id > 0).Any())
        {
            // 新增管理员
            //var salt = Guid.NewGuid().ToString("N");
            //var password = "123456".HMACSHA256(salt);
            var auth = new Auth("lengyun.cn", "123456", Roles.User | Roles.Admin);
            client.Insertable(auth).ExecuteCommand();

            if (!client.Queryable<User>().Where(a => a.Id > 0).Any())
            {
                // 新增管理员
                //var status = Status.Active;
                //var authId = 1L;
                var user = new User(new Name("lengyun.cn", "lengyun.cn"), new Email("mp@lengyun.cn"), auth);
                client.Insertable(auth).ExecuteCommand();
            }
        }

        
    }
}
