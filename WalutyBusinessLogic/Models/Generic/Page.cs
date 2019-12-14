namespace WalutyBusinessLogic.Models.Generic
{
    public class Page
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public Page(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public Page()
        {

        }
    }
}