using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk;
using McTools.Xrm.Connection;
using System.ComponentModel;
using System.Drawing;

namespace CoreySutton.XrmToolBox.BulkUpdateMailboxes
{
    public partial class MyPluginControl : PluginControlBase
    {
        private Settings mySettings;

        public MyPluginControl()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification(
                "To begin select an action type fro the dropdown then click \"Load Mailboxes\", or read the documentation", 
                new Uri("https://github.com/CoreySutton"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }

        private void TsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void TsbLoadMailboxes_Click(object sender, EventArgs e)
        {
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            ExecuteMethod(() =>
            {
                string actionType = TscbActionType.ComboBox.SelectedItem?.ToString();
                if (actionType == "Bulk Approve")
                {
                    GetMailboxes(
                        SetMailboxDataGridView,
                        ApprovalStatus.PendingApproval,
                        ApprovalStatus.Rejected,
                        ApprovalStatus.Empty);
                }
                else if (actionType == "Bulk Reject")
                {
                    GetMailboxes(
                        SetMailboxDataGridView,
                        ApprovalStatus.Approved,
                        ApprovalStatus.PendingApproval);
                }
                else
                {
                    // Shouldn't be able to click load mailboxes if a action isn't selected
                    return;
                }
            });
        }

        private void TsbUpdateMailboxes_Click(object sender, EventArgs e)
        {
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            ExecuteMethod(() => {
                string actionType = TscbActionType.ComboBox.SelectedItem?.ToString();
                if (actionType == "Bulk Approve")
                {
                    ModifyMailboxApproval(ApprovalStatus.Approved);
                }
                else if (actionType == "Bulk Reject")
                {
                    ModifyMailboxApproval(ApprovalStatus.Rejected);
                }
                else
                {
                    // Shouldn't be able to click load mailboxes if a action isn't selected
                    return;
                }
            });
        }

        private void TsbSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in mailboxDataGridView.Rows)
            {
                row.Cells[MailboxRowCheckBox.Name].Value = true;
            }
        }

        private void TsbSelectNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in mailboxDataGridView.Rows)
            {
                row.Cells[MailboxRowCheckBox.Name].Value = false;
            }
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

        private void TscbActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = TscbActionType.ComboBox.SelectedItem?.ToString();
            if (value == "Bulk Approve" || value == "Bulk Reject")
            {
                TsbLoadMailboxes.Visible = true;
            }
            else
            {
                TsbLoadMailboxes.Visible = false;                
            }

            TsbUpdateMailboxes.Visible = false;
            TsbSelectAll.Visible = false;
            TsbSelectNone.Visible = false;
            mailboxDataGridView.DataSource = null;
        }

        #endregion

        private void GetMailboxes(
            Action<RunWorkerCompletedEventArgs> postWorkCallBack,
            params ApprovalStatus[] approvalStatuses)
        {            
            MailboxDao mailboxDao = new MailboxDao(Service);

            bool parsed = int.TryParse(TstbPageSize.Text, out int pageSize);
            if (parsed) mailboxDao.PageSize = pageSize;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting mailboxes",
                Work = (worker, args) => {
                    args.Result = mailboxDao.Get(worker, approvalStatuses);
                },
                ProgressChanged = (e) =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = postWorkCallBack
            });
        }

        private void SetMailboxDataGridView(RunWorkerCompletedEventArgs args)
        {
            if (args.Error != null)
            {
                MessageBox.Show(
                    args.Error.ToString(),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (args.Result is List<Entity> results)
            {
                //StringBuilder message = new StringBuilder();
                //if (results.ContainsKey("user"))
                //    message.AppendLine($"Found {results["user"].Count()} user mailboxes");
                //if (results.ContainsKey("queue"))
                //    message.AppendLine($"Found {results["queue"].Count()} queue mailboxes");
                //MessageBox.Show(message.ToString());

                MessageBox.Show($"Found {results.Count()} mailboxes");

                //List<MailboxRow> rows = new List<MailboxRow>();

                //if (results.ContainsKey("user")) {
                //    rows.AddRange(results["user"]
                //        .Where(e => e.GetAttributeValue<AliasedValue>("User.emailrouteraccessapproval") != null)
                //        .Select(e => new MailboxRow()
                //        {
                //            MailboxId = e.Id,
                //            MailboxName = e.GetAttributeValue<string>("name"),
                //            Approval = (e.GetAttributeValue<AliasedValue>("User.emailrouteraccessapproval").Value as OptionSetValue).Value,
                //            RegardingUserId = e.GetAttributeValue<EntityReference>("regardingobjectid"),
                //            Regarding = "user"
                //        })
                //        .ToList());
                //}

                //if (results.ContainsKey("queue"))
                //{
                //    rows.AddRange(results["queue"]
                //        .Where(e => e.GetAttributeValue<AliasedValue>("Queue.emailrouteraccessapproval") != null)
                //        .Select(e => new MailboxRow()
                //        {
                //            MailboxId = e.Id,
                //            MailboxName = e.GetAttributeValue<string>("name"),
                //            Approval = (e.GetAttributeValue<AliasedValue>("Queue.emailrouteraccessapproval").Value as OptionSetValue).Value,
                //            RegardingUserId = e.GetAttributeValue<EntityReference>("regardingobjectid"),
                //            Regarding = "queue"
                //        })
                //        .ToList());
                //}

                List<MailboxRow> rows = results
                        .Select(e => new MailboxRow()
                        {
                            MailboxId = e.Id,
                            MailboxName = e.GetAttributeValue<string>("name"),
                            Approval = e.GetAttributeValue<OptionSetValue>("emailrouteraccessapproval").Value,
                            RegardingObjectLogicalName = e.GetAttributeValue<EntityReference>("regardingobjectid").LogicalName,
                            RegardingObjecId = e.GetAttributeValue<EntityReference>("regardingobjectid").Id
                        })
                        .ToList();

                mailboxDataGridView.DataSource = new BindingSource()
                {
                    DataSource = rows
                };

                TsbUpdateMailboxes.Visible = true;
                TsbSelectAll.Visible = true;
                TsbSelectNone.Visible = true;
            }
        }

        private void ModifyMailboxApproval(ApprovalStatus approvalStatus)
        {
            IList<DataGridViewRow> checkedRows = (from DataGridViewRow row in mailboxDataGridView.Rows
                                                  where Convert.ToBoolean(row.Cells[MailboxRowCheckBox.Name].Value) == true
                                                  select row).ToList();

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Modifying mailboxes",
                IsCancelable = true,
                AsyncArgument = checkedRows,
                Work = (worker, args) => {
                    var rows = args.Argument as IList<DataGridViewRow>;
                    for (var i = 0; i < rows.Count(); i++)
                    {
                        DataGridViewRow row = rows[i];
                        string mailboxName = row.Cells[2].Value as string;
                        worker.ReportProgress(-1, $"Modifying mailbox {mailboxName} to {approvalStatus}");

                        if (row.Cells[1].Value is Guid mailboxId)
                        {
                            MailboxDao mailboxDao = new MailboxDao(Service);
                            Exception approvalException = mailboxDao.SetApproval(
                                mailboxId,
                                approvalStatus);

                            if (approvalException != null)
                            {
                                LogError($"Failed to approve mailbox");
                                LogError(approvalException.Message);
                                LogError(approvalException.StackTrace);
                            }
                        }
                    }
                },
                ProgressChanged = e =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(
                            args.Error.ToString(),
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }

                    TsbLoadMailboxes.Visible = false;
                    TsbUpdateMailboxes.Visible = false;
                    TsbSelectAll.Visible = false;
                    TsbSelectNone.Visible = false;
                    mailboxDataGridView.DataSource = null;
                    TscbActionType.SelectedIndex = 0;

                    MessageBox.Show("Complete");
                }
            });
        }

        private void TstbPageSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TstbPageSize_TextChanged(object sender, EventArgs e)
        {
           HideNotification();

            bool parsed = int.TryParse(TstbPageSize.Text, out int pageSize);

            if (!parsed) { 
                TstbPageSize.ForeColor = Color.Red;
                ShowErrorNotification("Page Size is not a valid number", null);
            }
            else if (pageSize < 1 || pageSize > 5000)
            {
                TstbPageSize.ForeColor = Color.Red;
                ShowErrorNotification("Page Size must be between 0 and 5000", null);
            }
            else
            {
                TstbPageSize.ForeColor = Color.Black;
            }
        }
    }

    public class MailboxRow
    {
        public Guid MailboxId { get; set; }
        public string MailboxName { get; set; }
        public int Approval { get; set; }
        public string RegardingObjectLogicalName { get; set; }
        public Guid RegardingObjecId { get; set; }
    }
}