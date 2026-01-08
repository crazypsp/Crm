using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Entities.Dispatch
{
    public enum DispatchStatus
    {
        Scheduled = 1,
        InProgress = 2,
        Completed = 3,
        PartiallyCompleted = 4,
        Failed = 99
    }
}
