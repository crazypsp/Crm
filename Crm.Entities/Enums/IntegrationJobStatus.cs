using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Entities.Enums
{
    public enum IntegrationJobStatus
    {
        Queued = 1,
        Running = 2,
        Succeeded = 3,
        InProgress,
        Failed = 99
    }
}
