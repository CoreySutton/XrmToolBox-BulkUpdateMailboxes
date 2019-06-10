using Microsoft.Xrm.Sdk;
using System;

namespace CoreySutton.XrmToolBox.BulkUpdateMailboxes
{
    public class QueueDao
    {
        private readonly IOrganizationService _orgSvc;

        public QueueDao(IOrganizationService orgSvc)
        {
            _orgSvc = orgSvc;
        }

        public Exception SetMailboxApproval(Guid queueId, ApprovalStatus approvalStatus)
        {
            try
            {
                _orgSvc.Update(new Entity("queue")
                {
                    Id = queueId,
                    ["emailrouteraccessapproval"] = new OptionSetValue((int)approvalStatus)
                });

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
