namespace TestNode
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            client.Abort();
            client.Join();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxId = new System.Windows.Forms.TextBox();
            this.uxServerPortUdp = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonDump = new System.Windows.Forms.Button();
            this.textBoxStoreKey = new System.Windows.Forms.TextBox();
            this.textBoxStoreVal = new System.Windows.Forms.TextBox();
            this.buttonStore = new System.Windows.Forms.Button();
            this.buttonFind = new System.Windows.Forms.Button();
            this.textBoxFindVal = new System.Windows.Forms.TextBox();
            this.textBoxFindKey = new System.Windows.Forms.TextBox();
            this.textBoxNodeHexId = new System.Windows.Forms.TextBox();
            this.textBoxStoreKeyHex = new System.Windows.Forms.TextBox();
            this.textBoxFindKeyHex = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBootstrap = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.uxCbUpnp = new System.Windows.Forms.CheckBox();
            this.txtIPEndPoint = new System.Windows.Forms.TextBox();
            this.txtBootstrapIP = new System.Windows.Forms.TextBox();
            this.textBoxDump = new System.Windows.Forms.RichTextBox();
            this.tbxPing = new System.Windows.Forms.TextBox();
            this.btnPing = new System.Windows.Forms.Button();
            this.lblPing = new System.Windows.Forms.Label();
            this.tbxPort = new System.Windows.Forms.TextBox();
            this.dgvPeers = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeers)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxId
            // 
            this.textBoxId.Location = new System.Drawing.Point(7, 16);
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.Size = new System.Drawing.Size(51, 20);
            this.textBoxId.TabIndex = 0;
            this.textBoxId.Text = "hola";
            // 
            // uxServerPortUdp
            // 
            this.uxServerPortUdp.Location = new System.Drawing.Point(176, 16);
            this.uxServerPortUdp.Name = "uxServerPortUdp";
            this.uxServerPortUdp.Size = new System.Drawing.Size(43, 20);
            this.uxServerPortUdp.TabIndex = 1;
            this.uxServerPortUdp.Text = "4401";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(226, 14);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(71, 23);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonDump
            // 
            this.buttonDump.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDump.Location = new System.Drawing.Point(203, 292);
            this.buttonDump.Name = "buttonDump";
            this.buttonDump.Size = new System.Drawing.Size(382, 23);
            this.buttonDump.TabIndex = 4;
            this.buttonDump.Text = "Dump";
            this.buttonDump.UseVisualStyleBackColor = true;
            this.buttonDump.Click += new System.EventHandler(this.buttonDump_Click);
            // 
            // textBoxStoreKey
            // 
            this.textBoxStoreKey.Location = new System.Drawing.Point(13, 161);
            this.textBoxStoreKey.Name = "textBoxStoreKey";
            this.textBoxStoreKey.Size = new System.Drawing.Size(105, 20);
            this.textBoxStoreKey.TabIndex = 5;
            // 
            // textBoxStoreVal
            // 
            this.textBoxStoreVal.Location = new System.Drawing.Point(126, 161);
            this.textBoxStoreVal.Name = "textBoxStoreVal";
            this.textBoxStoreVal.Size = new System.Drawing.Size(105, 20);
            this.textBoxStoreVal.TabIndex = 6;
            // 
            // buttonStore
            // 
            this.buttonStore.Location = new System.Drawing.Point(239, 161);
            this.buttonStore.Name = "buttonStore";
            this.buttonStore.Size = new System.Drawing.Size(75, 23);
            this.buttonStore.TabIndex = 7;
            this.buttonStore.Text = "Store";
            this.buttonStore.UseVisualStyleBackColor = true;
            this.buttonStore.Click += new System.EventHandler(this.buttonStore_Click);
            // 
            // buttonFind
            // 
            this.buttonFind.Location = new System.Drawing.Point(238, 213);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(75, 23);
            this.buttonFind.TabIndex = 10;
            this.buttonFind.Text = "Find";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // textBoxFindVal
            // 
            this.textBoxFindVal.Location = new System.Drawing.Point(124, 213);
            this.textBoxFindVal.Name = "textBoxFindVal";
            this.textBoxFindVal.ReadOnly = true;
            this.textBoxFindVal.Size = new System.Drawing.Size(105, 20);
            this.textBoxFindVal.TabIndex = 9;
            // 
            // textBoxFindKey
            // 
            this.textBoxFindKey.Location = new System.Drawing.Point(13, 213);
            this.textBoxFindKey.Name = "textBoxFindKey";
            this.textBoxFindKey.Size = new System.Drawing.Size(105, 20);
            this.textBoxFindKey.TabIndex = 8;
            // 
            // textBoxNodeHexId
            // 
            this.textBoxNodeHexId.Location = new System.Drawing.Point(7, 43);
            this.textBoxNodeHexId.Name = "textBoxNodeHexId";
            this.textBoxNodeHexId.ReadOnly = true;
            this.textBoxNodeHexId.Size = new System.Drawing.Size(290, 20);
            this.textBoxNodeHexId.TabIndex = 11;
            // 
            // textBoxStoreKeyHex
            // 
            this.textBoxStoreKeyHex.Location = new System.Drawing.Point(13, 187);
            this.textBoxStoreKeyHex.Name = "textBoxStoreKeyHex";
            this.textBoxStoreKeyHex.ReadOnly = true;
            this.textBoxStoreKeyHex.Size = new System.Drawing.Size(298, 20);
            this.textBoxStoreKeyHex.TabIndex = 12;
            // 
            // textBoxFindKeyHex
            // 
            this.textBoxFindKeyHex.Location = new System.Drawing.Point(13, 239);
            this.textBoxFindKeyHex.Name = "textBoxFindKeyHex";
            this.textBoxFindKeyHex.ReadOnly = true;
            this.textBoxFindKeyHex.Size = new System.Drawing.Size(298, 20);
            this.textBoxFindKeyHex.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Bootstrap:";
            // 
            // btnBootstrap
            // 
            this.btnBootstrap.Location = new System.Drawing.Point(236, 124);
            this.btnBootstrap.Name = "btnBootstrap";
            this.btnBootstrap.Size = new System.Drawing.Size(75, 23);
            this.btnBootstrap.TabIndex = 16;
            this.btnBootstrap.Text = "Bootstrap";
            this.btnBootstrap.UseVisualStyleBackColor = true;
            this.btnBootstrap.Click += new System.EventHandler(this.btnBootstrap_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(179, 126);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(51, 20);
            this.txtPort.TabIndex = 17;
            this.txtPort.Text = "4401";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.uxCbUpnp);
            this.groupBox1.Controls.Add(this.txtIPEndPoint);
            this.groupBox1.Controls.Add(this.textBoxNodeHexId);
            this.groupBox1.Controls.Add(this.textBoxId);
            this.groupBox1.Controls.Add(this.uxServerPortUdp);
            this.groupBox1.Controls.Add(this.buttonStart);
            this.groupBox1.Location = new System.Drawing.Point(10, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(425, 114);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IP EndPoint";
            // 
            // uxCbUpnp
            // 
            this.uxCbUpnp.AutoSize = true;
            this.uxCbUpnp.Location = new System.Drawing.Point(304, 18);
            this.uxCbUpnp.Name = "uxCbUpnp";
            this.uxCbUpnp.Size = new System.Drawing.Size(74, 17);
            this.uxCbUpnp.TabIndex = 13;
            this.uxCbUpnp.Text = "Open Port";
            this.uxCbUpnp.UseVisualStyleBackColor = true;
            // 
            // txtIPEndPoint
            // 
            this.txtIPEndPoint.Location = new System.Drawing.Point(62, 16);
            this.txtIPEndPoint.Name = "txtIPEndPoint";
            this.txtIPEndPoint.Size = new System.Drawing.Size(106, 20);
            this.txtIPEndPoint.TabIndex = 12;
            this.txtIPEndPoint.Text = "127.0.0.1";
            // 
            // txtBootstrapIP
            // 
            this.txtBootstrapIP.Location = new System.Drawing.Point(73, 126);
            this.txtBootstrapIP.Name = "txtBootstrapIP";
            this.txtBootstrapIP.Size = new System.Drawing.Size(100, 20);
            this.txtBootstrapIP.TabIndex = 19;
            // 
            // textBoxDump
            // 
            this.textBoxDump.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDump.Location = new System.Drawing.Point(10, 321);
            this.textBoxDump.Name = "textBoxDump";
            this.textBoxDump.Size = new System.Drawing.Size(1094, 382);
            this.textBoxDump.TabIndex = 20;
            this.textBoxDump.Text = "";
            // 
            // tbxPing
            // 
            this.tbxPing.Location = new System.Drawing.Point(63, 265);
            this.tbxPing.Name = "tbxPing";
            this.tbxPing.Size = new System.Drawing.Size(114, 20);
            this.tbxPing.TabIndex = 21;
            // 
            // btnPing
            // 
            this.btnPing.Location = new System.Drawing.Point(264, 263);
            this.btnPing.Name = "btnPing";
            this.btnPing.Size = new System.Drawing.Size(47, 23);
            this.btnPing.TabIndex = 22;
            this.btnPing.Text = "Ping";
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // lblPing
            // 
            this.lblPing.AutoSize = true;
            this.lblPing.Location = new System.Drawing.Point(15, 268);
            this.lblPing.Name = "lblPing";
            this.lblPing.Size = new System.Drawing.Size(49, 13);
            this.lblPing.TabIndex = 23;
            this.lblPing.Text = "Dest EP:";
            // 
            // tbxPort
            // 
            this.tbxPort.Location = new System.Drawing.Point(188, 265);
            this.tbxPort.Name = "tbxPort";
            this.tbxPort.Size = new System.Drawing.Size(70, 20);
            this.tbxPort.TabIndex = 24;
            // 
            // dgvPeers
            // 
            this.dgvPeers.AllowUserToAddRows = false;
            this.dgvPeers.AllowUserToDeleteRows = false;
            this.dgvPeers.AllowUserToResizeRows = false;
            this.dgvPeers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPeers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            this.dgvPeers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvPeers.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvPeers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPeers.Location = new System.Drawing.Point(441, 12);
            this.dgvPeers.Name = "dgvPeers";
            this.dgvPeers.ReadOnly = true;
            this.dgvPeers.Size = new System.Drawing.Size(669, 209);
            this.dgvPeers.TabIndex = 25;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 715);
            this.Controls.Add(this.dgvPeers);
            this.Controls.Add(this.tbxPort);
            this.Controls.Add(this.lblPing);
            this.Controls.Add(this.btnPing);
            this.Controls.Add(this.tbxPing);
            this.Controls.Add(this.textBoxDump);
            this.Controls.Add(this.txtBootstrapIP);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.btnBootstrap);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFindKeyHex);
            this.Controls.Add(this.textBoxStoreKeyHex);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.textBoxFindVal);
            this.Controls.Add(this.textBoxFindKey);
            this.Controls.Add(this.buttonStore);
            this.Controls.Add(this.textBoxStoreVal);
            this.Controls.Add(this.textBoxStoreKey);
            this.Controls.Add(this.buttonDump);
            this.Name = "frmMain";
            this.Text = "EvolutionDHT - DevConsole";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxId;
        private System.Windows.Forms.TextBox uxServerPortUdp;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonDump;
        private System.Windows.Forms.TextBox textBoxStoreKey;
        private System.Windows.Forms.TextBox textBoxStoreVal;
        private System.Windows.Forms.Button buttonStore;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.TextBox textBoxFindVal;
        private System.Windows.Forms.TextBox textBoxFindKey;
        private System.Windows.Forms.TextBox textBoxNodeHexId;
        private System.Windows.Forms.TextBox textBoxStoreKeyHex;
        private System.Windows.Forms.TextBox textBoxFindKeyHex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBootstrap;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtIPEndPoint;
        private System.Windows.Forms.TextBox txtBootstrapIP;
        private System.Windows.Forms.RichTextBox textBoxDump;
        private System.Windows.Forms.TextBox tbxPing;
        private System.Windows.Forms.Button btnPing;
        private System.Windows.Forms.Label lblPing;
        private System.Windows.Forms.TextBox tbxPort;
        private System.Windows.Forms.DataGridView dgvPeers;
        private System.Windows.Forms.CheckBox uxCbUpnp;
    }
}

