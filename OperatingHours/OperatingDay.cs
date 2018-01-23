using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingHours
{
    public class OperatingDay: IOperatingDay
    {
        public DayOfWeek DayOfWeek { get; protected set; }
        public TimeSpan OpenTime { get; protected set; }
        public TimeSpan CloseTime { get; protected set; }
        public OperatingDay(DayOfWeek theDay, TimeSpan openTime, TimeSpan closeTime)
        {
            DayOfWeek = theDay;
            OpenTime = openTime;
            CloseTime = closeTime;
            if (OpenTime > CloseTime)
            {
                throw new IndexOutOfRangeException();
            }
        }
    }
}
