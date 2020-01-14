using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengePad.ViewModels
{
    public class CreateObjectiveModal
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public long OperationId { get; }
        public long Points { get; set; }

        public CreateObjectiveModal(long operationId)
        {
            OperationId = operationId;
        }
    }
}
