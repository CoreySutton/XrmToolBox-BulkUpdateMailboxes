using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
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

        public IDictionary<string, IList<Entity>> Get(
            BackgroundWorker worker,
            params ApprovalStatus[] approvalStatuses)
        {
            IDictionary<string, IList<Entity>> entities = new Dictionary<string, IList<Entity>>();
            
            IList<Entity> userMailboxes = GetUserMailboxes(worker, approvalStatuses);
            IList<Entity> queueMailboxes = GetQueueMailboxes(worker, approvalStatuses);
            
            if (userMailboxes != null) entities.Add("user", userMailboxes);
            if (queueMailboxes != null) entities.Add("queue", queueMailboxes);

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
                worker.ReportProgress(-1, $"Processing page {pageNumber} of mailboxes");
                results = _orgSvc.RetrieveMultiple(new QueryExpression("mailbox")
                {
                    ColumnSet = new ColumnSet(true),
                    Orders =
                    {
                        new OrderExpression("name", OrderType.Ascending)
                    },
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

        private List<Entity> GetQueueMailboxes(
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
    }
}
