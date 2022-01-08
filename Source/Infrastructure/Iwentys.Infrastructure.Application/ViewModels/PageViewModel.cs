using System;

namespace Iwentys.Infrastructure.Application.ViewModels
{
    public record PageViewModel
    {
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalItemsCount { get; }

        public PageViewModel(int itemsCount, int pageNumber, int takeAmount)
        {
            PageNumber = pageNumber;
            TotalPages = (int) Math.Ceiling(itemsCount / (double) takeAmount);
            TotalItemsCount = itemsCount;
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}