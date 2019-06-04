﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CoreySutton.XrmToolBox.BulkUpdateMailboxes
{
    public class MailboxDao
    {
        private readonly IOrganizationService _orgSvc;
        private const int PAGE_SIZE = 5000;

        public MailboxDao(IOrganizationService orgSvc)
        {
            _orgSvc = orgSvc;
        }

        public List<Entity> Get(
            BackgroundWorker worker,
            params ApprovalStatus[] approvalStatuses)
        {
            List<Entity> entities = new List<Entity>();
            EntityCollection results = null;
            int pageNumber = 1;
            do
            {
                worker.ReportProgress(-1, $"Processing page {pageNumber} of mailboxes");
                results = _orgSvc.RetrieveMultiple(new QueryExpression("mailbox")
                {
                    ColumnSet = new ColumnSet(true),
                    PageInfo = new PagingInfo()
                    {
                        Count = PAGE_SIZE,
                        PageNumber = pageNumber,
                        PagingCookie = results?.PagingCookie ?? null,
                    },
                    LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = "mailbox",
                                LinkFromAttributeName = "regardingobjectid",
                                LinkToEntityName = "systemuser",
                                LinkToAttributeName = "systemuserid",
                                Columns = new ColumnSet("emailrouteraccessapproval"),
                                EntityAlias = "User",
                                LinkCriteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(
                                            "emailrouteraccessapproval",
                                            ConditionOperator.In,
                                            approvalStatuses.Select(a => (int) a).ToArray())
                                    }
                                }
                            }
                        }
                });
                entities.AddRange(results.Entities);
                pageNumber++;
            } while (results.MoreRecords);

            return entities;
        }
    }
}