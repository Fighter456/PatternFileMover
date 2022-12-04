namespace PatternFileMover
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.konfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quellverzeichnisAuswählenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zuordnungenBearbeitenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.konfigurationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // konfigurationToolStripMenuItem
            // 
            this.konfigurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quellverzeichnisAuswählenToolStripMenuItem,
            this.zuordnungenBearbeitenToolStripMenuItem});
            this.konfigurationToolStripMenuItem.Name = "konfigurationToolStripMenuItem";
            this.konfigurationToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.konfigurationToolStripMenuItem.Text = "Konfiguration";
            // 
            // quellverzeichnisAuswählenToolStripMenuItem
            // 
            this.quellverzeichnisAuswählenToolStripMenuItem.Name = "quellverzeichnisAuswählenToolStripMenuItem";
            this.quellverzeichnisAuswählenToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.quellverzeichnisAuswählenToolStripMenuItem.Text = "Quellverzeichnis auswählen";
            this.quellverzeichnisAuswählenToolStripMenuItem.Click += new System.EventHandler(this.quellverzeichnisAuswählenToolStripMenuItem_Click);
            // 
            // zuordnungenBearbeitenToolStripMenuItem
            // 
            this.zuordnungenBearbeitenToolStripMenuItem.Name = "zuordnungenBearbeitenToolStripMenuItem";
            this.zuordnungenBearbeitenToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.zuordnungenBearbeitenToolStripMenuItem.Text = "Zuordnungen bearbeiten";
            this.zuordnungenBearbeitenToolStripMenuItem.Click += new System.EventHandler(this.zuordnungenBearbeitenToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(800, 426);
            this.dataGridView1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem konfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quellverzeichnisAuswählenToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem zuordnungenBearbeitenToolStripMenuItem;
    }
}

