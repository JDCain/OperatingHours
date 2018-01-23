using System;

namespace OperatingHours
{
    public interface IOperatingDay
    {
        TimeSpan CloseTime { get; }
        DayOfWeek DayOfWeek { get; }
        TimeSpan OpenTime { get; }
    }
}