using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CoreySutton.XrmToolBox.BulkUpdateMailboxes
{
    public class MailboxDao
    {
        public int PageSize { get; set; }

        private readonly IOrganizationService _orgSvc;

        public MailboxDao(IOrganizationService orgSvc, int pageSize = 5000)
        {
            _orgSvc = orgSvc;
            PageSize = pageSize;
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
                        Count = PageSize,
                        PageNumber = pageNumber,
                        PagingCookie = results?.PagingCookie ?? null,
                    },
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(
                                "emailrouteraccessapproval",
                                ConditionOperator.In,
                                approvalStatuses.Select(a => (int) a).ToArray())
                        }
                    }
                });
                entities.AddRange(results.Entities);
                pageNumber++;
            } while (results.MoreRecords);

            return entities;
        }

        private List<Entity> GetUserMailboxes(
            BackgroundWorker worker,
            params ApprovalStatus[] approvalStatuses)
        {
            List<Entity> entities = new List<Entity>();
            EntityCollection results = null;
            int pageNumber = 1;
            do
            {
                worker.ReportProgress(-1, $"Processing page {pageNumber} of user mailboxes");
                results = _orgSvc.RetrieveMultiple(new QueryExpression("mailbox")
                {
                    ColumnSet = new ColumnSet(true),
                    Orders =
                    {
                        new OrderExpression("name", OrderType.Ascending)
                    },
                    PageInfo = new PagingInfo()
                    {
                        Count = PageSize,
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

        private List<Entity> GetQueueMailboxes(
            BackgroundWorker worker,
            params ApprovalStatus[] approvalStatuses)
        {
            List<Entity> entities = new List<Entity>();
            EntityCollection results = null;
            int pageNumber = 1;
            do
            {
                worker.ReportProgress(-1, $"Processing page {pageNumber} of queue mailboxes");
                results = _orgSvc.RetrieveMultiple(new QueryExpression("mailbox")
                {
                    ColumnSet = new ColumnSet(true),
                    PageInfo = new PagingInfo()
                    {
                        Count = PageSize,
                        PageNumber = pageNumber,
                        PagingCookie = results?.PagingCookie ?? null,
                    },
                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            LinkFromEntityName = "mailbox",
                            LinkFromAttributeName = "regardingobjectid",
                            LinkToEntityName = "queue",
                            LinkToAttributeName = "queueid",
                            EntityAlias = "Queue",
                            Columns = new ColumnSet("emailrouteraccessapproval"),
                            LinkCriteria = {
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

        public Exception SetApproval(Guid mailboxId, ApprovalStatus approvalStatus)
        {
            try
            {
                _orgSvc.Update(new Entity("mailbox")
                {
                    Id = mailboxId,
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
