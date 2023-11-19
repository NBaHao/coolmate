using Microsoft.AspNetCore.WebUtilities;
using CoolMate.Filter;

namespace CoolMate.Services
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
