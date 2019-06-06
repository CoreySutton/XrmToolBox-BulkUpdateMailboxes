using Microsoft.Xrm.Sdk;
using System;

namespace CoreySutton.XrmToolBox.BulkUpdateMailboxes
{
    public class QueueDao
    {
        public Exception CaughtException = null;
        private readonly IOrganizationService _orgSvc;

        public QueueDao(IOrganizationService orgSvc)
        {
            _orgSvc = orgSvc;
        }

        public bool SetMailboxApproval(Guid queueId, ApprovalStatus approvalStatus)
        {
            try
            {
                _orgSvc.Update(new Entity("queue")
                {
                    Id = queueId,
                    ["emailrouteraccessapproval"] = new OptionSetValue((int)approvalStatus)
                });

                return true;
            }
            catch (Exception ex)
            {
                CaughtException = ex;
                return false;
            }
        }
    }
}
