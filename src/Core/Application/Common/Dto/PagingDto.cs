namespace Application.Common.Dto
{
    public class PagingDto : PagingDto<object>
    {

        public PagingDto(object data, int? totalcount = null)
            : base(data, totalcount)
        {

        }
    }

    public class PagingDto<T>
    {
        public int? TotalCount { get; set; }
        public T Data { get; set; }
        public PagingDto(T data, int? totalcount = null)
        {
            TotalCount = totalcount;
            Data = data;
        }
    }
}