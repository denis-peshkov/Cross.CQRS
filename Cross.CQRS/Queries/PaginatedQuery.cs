namespace Cross.CQRS.Queries;

public class PaginatedQuery<TResult> : Query<TResult>
    where TResult : class
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }
    
    protected PaginatedQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}
