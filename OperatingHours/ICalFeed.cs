
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OperatingHours
{
    public interface ICalFeed
    {
        Task<bool> IsOpenAsync();
        Task<bool> IsOpenAsync(DateTimeOffset datetime);
    }
}