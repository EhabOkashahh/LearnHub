
namespace Shared.DTOS
{
    public class PaginatedResponse<TEntity>
    {
         public PaginatedResponse(int pageIndex, int pageSize, int totalCount,IEnumerable<TEntity> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }    
        
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public IEnumerable<TEntity> Data { get; set; }

       
    }
}