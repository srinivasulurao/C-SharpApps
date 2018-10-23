namespace IncidentCopyAddin
{
    partial class CopyDetailsButton
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button_copy = new System.Windows.Forms.Button();
            this.textBox_details = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.button_copy, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox_details, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(264, 49);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // button_copy
            // 
            this.button_copy.AutoSize = true;
            this.button_copy.Location = new System.Drawing.Point(3, 3);
            this.button_copy.Name = "button_copy";
            this.button_copy.Size = new System.Drawing.Size(68, 23);
            this.button_copy.TabIndex = 0;
            this.button_copy.Text = "Copy...";
            this.button_copy.UseVisualStyleBackColor = true;
            this.button_copy.Click += new System.EventHandler(this.button_copy_Click);
            // 
            // textBox_details
            // 
            this.textBox_details.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_details.Location = new System.Drawing.Point(77, 3);
            this.textBox_details.Multiline = true;
            this.textBox_details.Name = "textBox_details";
            this.textBox_details.ReadOnly = true;
            this.textBox_details.Size = new System.Drawing.Size(184, 43);
            this.textBox_details.TabIndex = 1;
            // 
            // CopyDetailsButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CopyDetailsButton";
            this.Size = new System.Drawing.Size(264, 49);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button_copy;
        private System.Windows.Forms.TextBox textBox_details;
    }
}
