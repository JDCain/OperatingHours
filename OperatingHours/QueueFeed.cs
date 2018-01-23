using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingHours
{
    public class QueueFeed : IQueueFeed
    {
        public string Team { get; }
        public string Queue { get; }
        public ICalFeed CalFeed { get; }
        /// <summary>
        ///
        /// </summary>
        /// <param name="team"></param>
        /// <param name="queue"></param>
        /// <param name="calFeed"></param>
        public QueueFeed(string team, string queue, ICalFeed calFeed)
        {
            Team = team;
            Queue = queue;
            CalFeed = calFeed;
            Override = null;
        }

        public bool? Override { get; private set; }

        public void SetOverride(bool? overrideValue)
        {
            Override = overrideValue;
        }
    }
}
