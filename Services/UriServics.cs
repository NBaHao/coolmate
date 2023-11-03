using Microsoft.AspNetCore.WebUtilities;
using WebApplication1.Filter;

namespace WebApplication1.Services
{
    public class UriService
    {
        public Uri GetPageUri(PaginationFilter filter, string enpointUri)
        {
            var modifiedUri = QueryHelpers.AddQueryString(enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
        
    }
}
