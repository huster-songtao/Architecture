using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Architecture.Domain;

/// <summary>
/// 分页信息
/// </summary>
/// <typeparam name="T">分页展示的信息</typeparam>
public class Pagination<T>
{
    /// <summary>
    /// 记录总数
    /// 参考 https://github.com/elastic/elasticsearch-net/blob/0b2a83b8f9647ae482c91e67fe9bf983d18c2947/src/Nest/Search/Search/SearchResponse.cs
    /// </summary>
    public long Total { get; set; } = 0;

    /// <summary>
    /// 页大小，每页显示多少条记录
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 页码，显示第几页
    /// </summary>
    public int PageIndex { get; set; } = 1;

    /// <summary>
    /// 总页数，共有多少页
    /// </summary>
    public int PageCount
    {
        get
        {
            if (PageSize == 0)
                return 1;

            // 共有多少页
            return (int)Math.Ceiling(Total / (double)PageSize);
        }
    }

    /// <summary>
    /// 记录
    /// </summary>
    public List<T> Entities { get; set; }
}
