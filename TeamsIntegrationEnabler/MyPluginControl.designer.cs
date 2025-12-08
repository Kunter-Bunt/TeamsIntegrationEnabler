namespace TeamsIntegrationEnabler
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lbTables = new System.Windows.Forms.ListBox();
            this.lblResultStatus = new System.Windows.Forms.Label();
            this.lblPassedMsg = new System.Windows.Forms.Label();
            this.lblFailedMsg = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.tsbLoadTables = new System.Windows.Forms.ToolStripButton();
            this.tsbEnable = new System.Windows.Forms.ToolStripButton();
            this.tsbDisable = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLoadTables,
            this.tssSeparator1,
            this.tsbEnable,
            this.toolStripSeparator1,
            this.tsbDisable});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(559, 31);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // lbTables
            // 
            this.lbTables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbTables.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbTables.FormattingEnabled = true;
            this.lbTables.Location = new System.Drawing.Point(3, 58);
            this.lbTables.Name = "lbTables";
            this.lbTables.Size = new System.Drawing.Size(178, 264);
            this.lbTables.TabIndex = 6;
            // 
            // lblResultStatus
            // 
            this.lblResultStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResultStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultStatus.Location = new System.Drawing.Point(190, 34);
            this.lblResultStatus.Name = "lblResultStatus";
            this.lblResultStatus.Size = new System.Drawing.Size(360, 30);
            this.lblResultStatus.TabIndex = 7;
            this.lblResultStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPassedMsg
            // 
            this.lblPassedMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPassedMsg.Location = new System.Drawing.Point(190, 70);
            this.lblPassedMsg.Name = "lblPassedMsg";
            this.lblPassedMsg.Size = new System.Drawing.Size(360, 40);
            this.lblPassedMsg.TabIndex = 8;
            // 
            // lblFailedMsg
            // 
            this.lblFailedMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFailedMsg.Location = new System.Drawing.Point(190, 115);
            this.lblFailedMsg.Name = "lblFailedMsg";
            this.lblFailedMsg.Size = new System.Drawing.Size(360, 40);
            this.lblFailedMsg.TabIndex = 9;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(3, 34);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(178, 20);
            this.txtSearch.TabIndex = 5;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // tsbLoadTables
            // 
            this.tsbLoadTables.Image = global::TeamsIntegrationEnabler.Properties.Resources.arrow_clockwise;
            this.tsbLoadTables.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoadTables.Name = "tsbLoadTables";
            this.tsbLoadTables.Size = new System.Drawing.Size(97, 28);
            this.tsbLoadTables.Text = "Load Tables";
            this.tsbLoadTables.Click += new System.EventHandler(this.tsbLoadTables_Click);
            // 
            // tsbEnable
            // 
            this.tsbEnable.AccessibleName = "Enable";
            this.tsbEnable.Image = global::TeamsIntegrationEnabler.Properties.Resources.checkmark;
            this.tsbEnable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEnable.Name = "tsbEnable";
            this.tsbEnable.Size = new System.Drawing.Size(70, 28);
            this.tsbEnable.Text = "Enable";
            this.tsbEnable.Click += new System.EventHandler(this.tsbEnable_Click);
            // 
            // tsbDisable
            // 
            this.tsbDisable.Image = global::TeamsIntegrationEnabler.Properties.Resources.dismiss;
            this.tsbDisable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDisable.Name = "tsbDisable";
            this.tsbDisable.Size = new System.Drawing.Size(73, 28);
            this.tsbDisable.Text = "Disable";
            this.tsbDisable.Click += new System.EventHandler(this.tsbDisable_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblFailedMsg);
            this.Controls.Add(this.lblPassedMsg);
            this.Controls.Add(this.lblResultStatus);
            this.Controls.Add(this.lbTables);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(559, 300);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.ToolStripButton tsbEnable;
        private System.Windows.Forms.ToolStripButton tsbDisable;
        private System.Windows.Forms.ListBox lbTables;
        private System.Windows.Forms.ToolStripButton tsbLoadTables;
        private System.Windows.Forms.Label lblResultStatus;
        private System.Windows.Forms.Label lblPassedMsg;
        private System.Windows.Forms.Label lblFailedMsg;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
