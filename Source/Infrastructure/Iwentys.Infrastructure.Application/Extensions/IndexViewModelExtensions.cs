using System.Collections.Generic;
using System.Linq;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Infrastructure.Application.ViewModels;

namespace Iwentys.Infrastructure.Application.Extensions
{
    public static class IndexViewModelExtensions<T>
    {
        public static IndexViewModel<T> ToIndexViewModel(ICollection<T> items, PaginationFilter paginationFilter)
        {
            return new IndexViewModel<T>
            {
                PageViewModel = new (items.Count, paginationFilter.Page, paginationFilter.Take),
                Items = items.Skip((paginationFilter.Page - 1) * paginationFilter.Take)
                    .Take(paginationFilter.Take).ToList(),
            };
        }
    }
}