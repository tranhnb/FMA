using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activity.Publishers
{
    public class ActivityPublisher : Publisher<ActivityPublisher>
    {
        /// <summary>
        /// Publish information to subcribles
        /// </summary>
        public void Publish() {
            this.Notify(null);
        }
    }
}
