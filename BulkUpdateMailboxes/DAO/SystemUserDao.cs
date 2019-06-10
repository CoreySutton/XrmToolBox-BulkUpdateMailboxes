using Microsoft.Xrm.Sdk;
using System;

namespace CoreySutton.XrmToolBox.BulkUpdateMailboxes
{
    public class SystemUserDao
    {
        private readonly IOrganizationService _orgSvc;

        public SystemUserDao(IOrganizationService orgSvc)
        {
            _orgSvc = orgSvc;
        }

        public Exception SetMailboxApproval(Guid systemUserId, ApprovalStatus approvalStatus)
        {
            try
            {
                _orgSvc.Update(new Entity("systemuser")
                {
                    Id = systemUserId,
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
