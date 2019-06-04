using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CoreySutton.XrmToolBox.BulkUpdateMailboxes
{
    public class SystemUserDao
    {
        public Exception CaughtException = null;
        private readonly IOrganizationService _orgSvc;
        private const int PAGE_SIZE = 5000;

        public SystemUserDao(IOrganizationService orgSvc)
        {
            _orgSvc = orgSvc;
        }

        public bool SetMailboxApproval(Guid systemUserId, ApprovalStatus approvalStatus)
        {
            try
            {
                _orgSvc.Update(new Entity("systemuser")
                {
                    Id = systemUserId,
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
