using System.Collections.Generic;
using System.Linq;

namespace CSD.Contracts;

public class PageResult<TData>
{
    public int Page { get; set; }

    public int Count { get; set; }

    public int TotalCount { get; set; }

    public IEnumerable<TData> Data { get; set; } = Enumerable.Empty<TData>();
}
