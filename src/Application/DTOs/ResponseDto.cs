namespace Application.DTOs;

public class ResponseDto<T>
{
    public int TotalCount { get; set; }
    //public int PageSize { get; set; }
    //public int PageNumber { get; set; }
    //public bool HasNextPage { get; set; }
    //public bool HasPreviousPage { get; set; }
    public T Data { get; set; } = default!;
}
