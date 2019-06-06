using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk;
using McTools.Xrm.Connection;
using System.ComponentModel;

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
                "To begin click \"Load Mailboxes\", or read the documentation", 
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
                        ApprovalStatus.Rejected,
                        ApprovalStatus.Empty);
                }
                else if (actionType == "Bulk Reject")
                {
                    GetMailboxes(
                        SetMailboxDataGridView,
                        ApprovalStatus.Approved,
                        ApprovalStatus.PendingApproval,
                        ApprovalStatus.Empty);
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
            else if (args.Result is IList<Entity> results)
            {
                MessageBox.Show($"Found {results.Count} mailboxes");

                mailboxDataGridView.DataSource = new BindingSource()
                {
                    DataSource = results
                    .Where(e => e.GetAttributeValue<AliasedValue>("User.emailrouteraccessapproval") != null)
                    .Select(e => new MailboxRow()
                    {
                        MailboxId = e.Id,
                        MailboxName = e.GetAttributeValue<string>("name"),
                        Approval = (e.GetAttributeValue<AliasedValue>("User.emailrouteraccessapproval").Value as OptionSetValue).Value,
                        RegardingUserId = e.GetAttributeValue<EntityReference>("regardingobjectid").Id
                    })
                    .ToList()
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
                        Guid systemUserId = Guid.Parse(row.Cells[4].Value.ToString());

                        worker.ReportProgress(-1, $"Modifying mailbox {mailboxName} to {approvalStatus}");

                        SystemUserDao systemUserDao = new SystemUserDao(Service);
                        bool success = systemUserDao.SetMailboxApproval(systemUserId, approvalStatus);
                        if (!success && systemUserDao.CaughtException != null)
                        {
                            LogError($"Failed to approve mailbox");
                            LogError(systemUserDao.CaughtException.Message);
                            LogError(systemUserDao.CaughtException.StackTrace);
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
                }
            });
        }
    }

    public class MailboxRow
    {
        public Guid MailboxId { get; set; }
        public string MailboxName { get; set; }
        public int Approval { get; set; }
        public Guid RegardingUserId { get; set; }
    }
}