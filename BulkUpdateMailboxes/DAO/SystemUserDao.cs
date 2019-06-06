﻿using Microsoft.Xrm.Sdk;
using System;

namespace CoreySutton.XrmToolBox.BulkUpdateMailboxes
{
    public class SystemUserDao
    {
        public Exception CaughtException = null;
        private readonly IOrganizationService _orgSvc;

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
