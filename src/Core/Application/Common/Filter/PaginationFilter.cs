namespace Application.Common.Filter
{
    public class PaginationFilter
    {
        public int CurrentPageNumber { get; set; }
        public int CountPerPage { get; set; }
        public int ASC { get; set; }
        public string SortOrder { get; set; }
        public PaginationFilter()
        {
            CurrentPageNumber = 1;
            CountPerPage = 50;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            CurrentPageNumber = pageNumber < 1 ? 1 : pageNumber;
            CountPerPage = pageSize > 50 ? 50 : pageSize;
        }
    }
}