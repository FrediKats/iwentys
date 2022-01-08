using System.Collections.Generic;

namespace Iwentys.Infrastructure.Application.ViewModels
{
    public record IndexViewModel<T>
    {
        public IEnumerable<T> Items { get; init; }
        public PageViewModel PageViewModel { get; init; }
    }
}