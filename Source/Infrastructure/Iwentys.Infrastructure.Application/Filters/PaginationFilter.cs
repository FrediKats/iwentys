using Iwentys.Common.Exceptions;

namespace Iwentys.Infrastructure.Application.Filters
{
    public record PaginationFilter
    {
        private const int DefaultTakeAmount = 10;
        private const int DefaultPageNumber = 1;

        public PaginationFilter(int take, int page)
        {
            if (take < 0) throw new InnerLogicException("Take amount must be positive");
            if (page < 0) throw new  InnerLogicException("Page number must be positive");

            Page = page == 0 ? DefaultPageNumber : page;
            Take = take == 0 ? DefaultTakeAmount : take;
        }

        public int Page { get; }
        public int Take { get; }
    }
}