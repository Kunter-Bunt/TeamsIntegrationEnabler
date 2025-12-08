using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace TeamsIntegrationEnabler
{
    public partial class MyPluginControl : PluginControlBase
    {
        private Settings mySettings;
        private readonly Dictionary<string, bool> teamsEnabledByLogicalName = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        private KeyValuePair<string, string>[] allListItems = new KeyValuePair<string, string>[0]; // logicalName -> display text

        public MyPluginControl()
        {
            InitializeComponent();
            this.Resize += MyPluginControl_Resize;
            lbTables.DrawItem += LbTables_DrawItem;
            lbTables.MeasureItem += LbTables_MeasureItem;
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }

            AdjustListBoxLayout();

            LoadTables();
        }

        private void tsbEnable_Click(object sender, EventArgs e)
        {
            ExecuteMethod(() => changeTeamsIntegrationStatus(true));
        }

        private void tsbDisable_Click(object sender, EventArgs e)
        {
            ExecuteMethod(() => changeTeamsIntegrationStatus(false));
        }

        private void tsbLoadTables_Click(object sender, EventArgs e)
        {
            ExecuteMethod(LoadTables);
        }

        private void changeTeamsIntegrationStatus(bool enable)
        {
            var selectedItems = lbTables.SelectedItems;
            var tableNames = "[";
            foreach (var item in selectedItems)
            {
                tableNames += "\"" + item.ToString().Split(' ')[0] + "\",";
            }
            tableNames = tableNames.TrimEnd(',') + "]";

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"{(enable ? "Enabling" : "Disabling")} tables: {tableNames}",
                Work = (worker, args) =>
                {
                    var request = new OrganizationRequest("msdyn_SetTeamsDocumentStatus")
                    {
                        ["LogicalEntityNames"] = tableNames,
                        ["Enable"] = enable
                    };
                    var response = Service.Execute(request);
                    args.Result = response;
                },
                PostWorkCallBack = (args) =>
                {
                    var response = args.Result as OrganizationResponse;
                    bool? operationResult = response?["OperationResult"] as bool?;
                    var passedLogicalEntityNames = response?["PassedLogicalEntityNames"] as string;
                    var failedLogicalEntityNames = response?["FailedLogicalEntityNames"] as string;

                    // Update status label
                    if (operationResult.GetValueOrDefault())
                    {
                        lblResultStatus.Text = "SUCCESS";
                        lblResultStatus.ForeColor = Color.ForestGreen;
                    }
                    else
                    {
                        lblResultStatus.Text = "FAILURE";
                        lblResultStatus.ForeColor = Color.Firebrick;
                    }

                    string verb = enable ? "enabled" : "disabled";

                    // Format messages: parse the stringified arrays like "[account,contact]" or "[]"
                    string FormatTablesMessage(string names)
                    {
                        if (string.IsNullOrWhiteSpace(names)) return string.Empty;
                        var trimmed = names.Trim();
                        if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                        {
                            trimmed = trimmed.Substring(1, trimmed.Length - 2); // remove [ ]
                        }
                        // Split by comma and trim items; handle empty string -> no tables
                        var parts = trimmed.Length == 0 ? new string[0] : trimmed.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToArray();
                        return string.Join(", ", parts);
                    }

                    var passedList = FormatTablesMessage(passedLogicalEntityNames);
                    var failedList = FormatTablesMessage(failedLogicalEntityNames);

                    lblPassedMsg.ForeColor = Color.ForestGreen;
                    lblFailedMsg.ForeColor = Color.Firebrick;

                    lblPassedMsg.Text = string.IsNullOrEmpty(passedList)
                        ? string.Empty
                        : $"Successfully {verb} Teams Integration for {passedList}";

                    lblFailedMsg.Text = string.IsNullOrEmpty(failedList)
                        ? string.Empty
                        : $"Failed to {verb} Teams Integration for {failedList}";

                    // Refresh UI
                    lblResultStatus.Invalidate();
                    lblPassedMsg.Invalidate();
                    lblFailedMsg.Invalidate();

                    var res = MessageBox.Show("Operation completed. Refresh Tables?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (res == DialogResult.Yes)
                        LoadTables();
                }
            });

        }

        private void LoadTables()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading table metadata",
                Work = (worker, args) =>
                {
                    RetrieveAllEntitiesRequest retrieveAllEntityRequest = new RetrieveAllEntitiesRequest
                    {
                        RetrieveAsIfPublished = true,
                        EntityFilters = EntityFilters.Entity
                    };
                    RetrieveAllEntitiesResponse retrieveAllEntityResponse = (RetrieveAllEntitiesResponse)Service.Execute(retrieveAllEntityRequest);

                    teamsEnabledByLogicalName.Clear();
                    foreach (var em in retrieveAllEntityResponse.EntityMetadata)
                    {
                        if (em?.IsMSTeamsIntegrationEnabled != null)
                        {
                            teamsEnabledByLogicalName[em.LogicalName] = em.IsMSTeamsIntegrationEnabled == true;
                        }
                    }

                    allListItems = retrieveAllEntityResponse.EntityMetadata
                        .Select(em =>
                        {
                            var display = em.DisplayName?.UserLocalizedLabel?.Label;
                            var text = $"{em.LogicalName} ({display})";
                            return new KeyValuePair<string, string>(em.LogicalName, text);
                        })
                        .ToArray();
                },
                PostWorkCallBack = (args) =>
                {
                    ApplySearchFilter();
                    AdjustListBoxLayout();
                    lbTables.Invalidate();
                }
            });
        }

        private void AdjustListBoxLayout()
        {
            if (lbTables == null || toolStripMenu == null) return;

            var clientWidth = this.ClientSize.Width;
            var clientHeight = this.ClientSize.Height;
            var topOffset = toolStripMenu.Height;

            if (txtSearch != null)
            {
                txtSearch.Location = new Point(3, topOffset + 3);
            }
            lbTables.Location = new Point(3, (txtSearch != null ? txtSearch.Bottom + 3 : topOffset + 3));

            var bottomPadding = 3;
            var newHeight = Math.Max(0, clientHeight - (lbTables.Location.Y + bottomPadding));
            lbTables.Height = newHeight;

            int maxAllowedWidth = (int)(clientWidth * 0.5);

            int newWidth;
            if (lbTables.Items.Count == 0)
            {
                newWidth = Math.Min(500, maxAllowedWidth);
                if (newWidth <= 0) newWidth = Math.Min(178, maxAllowedWidth);
            }
            else
            {
                int maxWidthNeeded = 0;
                using (var g = lbTables.CreateGraphics())
                {
                    foreach (var item in lbTables.Items)
                    {
                        var text = item?.ToString() ?? string.Empty;
                        var size = g.MeasureString(text, lbTables.Font);
                        maxWidthNeeded = Math.Max(maxWidthNeeded, (int)Math.Ceiling(size.Width));
                    }
                }
                int padding = 16; // typical listbox padding
                int scrollbarWidth = SystemInformation.VerticalScrollBarWidth;
                int desiredWidth = maxWidthNeeded + padding + scrollbarWidth;

                newWidth = Math.Min(Math.Max(desiredWidth, lbTables.MinimumSize.Width), maxAllowedWidth);
                if (newWidth <= 0) newWidth = Math.Min(178, maxAllowedWidth); // fallback
            }

            if (txtSearch != null)
            {
                txtSearch.Width = newWidth;
            }
            lbTables.Width = newWidth;

            if (txtSearch != null)
            {
                txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }
            lbTables.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            AdjustResultPanelLayout(clientWidth, topOffset);
        }

        private void AdjustResultPanelLayout(int clientWidth, int topOffset)
        {
            if (lblResultStatus == null || lblPassedMsg == null || lblFailedMsg == null) return;
            int startX = (int)(clientWidth * 0.5) + 8;
            int rightPadding = 8;
            int availableWidth = Math.Max(0, clientWidth - startX - rightPadding);

            try
            {
                lblResultStatus.Font = new Font(lblResultStatus.Font.FontFamily, 24f, FontStyle.Bold);
                lblPassedMsg.Font = new Font(lblPassedMsg.Font.FontFamily, 16f, FontStyle.Regular);
                lblFailedMsg.Font = new Font(lblFailedMsg.Font.FontFamily, 16f, FontStyle.Regular);
            }
            catch { }

            lblResultStatus.Location = new Point(startX, topOffset + 6);
            lblResultStatus.Size = new Size(availableWidth, 36);

            lblPassedMsg.Location = new Point(startX, lblResultStatus.Bottom + 8);
            lblPassedMsg.Size = new Size(availableWidth, 60);

            lblFailedMsg.Location = new Point(startX, lblPassedMsg.Bottom + 6);
            lblFailedMsg.Size = new Size(availableWidth, 60);

            lblResultStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPassedMsg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblFailedMsg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        private void LbTables_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            int baseHeight = lbTables.Font.Height + 6;
            e.ItemHeight = (int)Math.Ceiling(baseHeight * 1.2);
        }

        private void LbTables_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lbTables.Items.Count)
            {
                return;
            }

            var itemText = lbTables.Items[e.Index].ToString();
            var logicalName = itemText.Split(' ')[0];
            bool enabled;
            bool hasStatus = teamsEnabledByLogicalName.TryGetValue(logicalName, out enabled);

            Color backColor;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                backColor = SystemColors.Highlight;
            }
            else if (hasStatus)
            {
                backColor = enabled ? Color.FromArgb(210, 242, 215) : Color.FromArgb(255, 224, 224); // soft green/red
            }
            else
            {
                backColor = lbTables.BackColor;
            }

            using (var backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            Color foreColor = ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ? SystemColors.HighlightText : lbTables.ForeColor;
            using (var textBrush = new SolidBrush(foreColor))
            {
                var textRect = new Rectangle(e.Bounds.X + 4, e.Bounds.Y + 3, e.Bounds.Width - 8, e.Bounds.Height - 6);
                e.Graphics.DrawString(itemText, e.Font, textBrush, textRect);
            }

            e.DrawFocusRectangle();
        }

        private void MyPluginControl_Resize(object sender, EventArgs e)
        {
            AdjustListBoxLayout();
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
            AdjustListBoxLayout();
        }

        private void ApplySearchFilter()
        {
            if (allListItems == null) return;
            var query = (txtSearch.Text ?? string.Empty).Trim();
            IEnumerable<KeyValuePair<string, string>> filtered = allListItems;
            if (query.Length > 0)
            {
                var q = query.ToLowerInvariant();
                filtered = allListItems.Where(kv =>
                    kv.Key.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    kv.Value.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            lbTables.BeginUpdate();
            try
            {
                lbTables.Items.Clear();
                lbTables.Items.AddRange(filtered.Select(kv => kv.Value).ToArray());
            }
            finally
            {
                lbTables.EndUpdate();
            }
        }
    }
}