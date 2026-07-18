namespace DVLD1.People.Controls
{
    partial class ctrlPersonCardWithFilter
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
            this.components = new System.ComponentModel.Container();
            this.ctrlPersonCard1 = new DVLD1.People.Controls.ctrlPersonCard();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbFindPerson = new System.Windows.Forms.ComboBox();
            this.txtFindPerson = new System.Windows.Forms.TextBox();
            this.btnSearchOnPerson = new System.Windows.Forms.Button();
            this.btnAddPerson = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // ctrlPersonCard1
            // 
            this.ctrlPersonCard1.BackColor = System.Drawing.Color.White;
            this.ctrlPersonCard1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlPersonCard1.Location = new System.Drawing.Point(0, 107);
            this.ctrlPersonCard1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.ctrlPersonCard1.Name = "ctrlPersonCard1";
            this.ctrlPersonCard1.Size = new System.Drawing.Size(1094, 329);
            this.ctrlPersonCard1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAddPerson);
            this.groupBox1.Controls.Add(this.btnSearchOnPerson);
            this.groupBox1.Controls.Add(this.txtFindPerson);
            this.groupBox1.Controls.Add(this.cbFindPerson);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1081, 95);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 29);
            this.label5.TabIndex = 43;
            this.label5.Text = "Find By:";
            // 
            // cbFindPerson
            // 
            this.cbFindPerson.FormattingEnabled = true;
            this.cbFindPerson.Items.AddRange(new object[] {
            "National No.",
            "Person ID."});
            this.cbFindPerson.Location = new System.Drawing.Point(121, 42);
            this.cbFindPerson.Name = "cbFindPerson";
            this.cbFindPerson.Size = new System.Drawing.Size(249, 33);
            this.cbFindPerson.TabIndex = 44;
            this.cbFindPerson.SelectedIndexChanged += new System.EventHandler(this.cbFindPerson_SelectedIndexChanged);
            // 
            // txtFindPerson
            // 
            this.txtFindPerson.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFindPerson.Location = new System.Drawing.Point(395, 45);
            this.txtFindPerson.Name = "txtFindPerson";
            this.txtFindPerson.Size = new System.Drawing.Size(331, 30);
            this.txtFindPerson.TabIndex = 45;
            this.txtFindPerson.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFindPerson_KeyPress);
            this.txtFindPerson.Validating += new System.ComponentModel.CancelEventHandler(this.txtFindPerson_Validating);
            // 
            // btnSearchOnPerson
            // 
            this.btnSearchOnPerson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchOnPerson.Image = global::DVLD1.Properties.Resources.SearchPerson;
            this.btnSearchOnPerson.Location = new System.Drawing.Point(751, 35);
            this.btnSearchOnPerson.Name = "btnSearchOnPerson";
            this.btnSearchOnPerson.Size = new System.Drawing.Size(75, 44);
            this.btnSearchOnPerson.TabIndex = 46;
            this.btnSearchOnPerson.UseVisualStyleBackColor = true;
            this.btnSearchOnPerson.Click += new System.EventHandler(this.btnSearchOnPerson_Click);
            // 
            // btnAddPerson
            // 
            this.btnAddPerson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddPerson.Image = global::DVLD1.Properties.Resources.AddPerson_32;
            this.btnAddPerson.Location = new System.Drawing.Point(847, 35);
            this.btnAddPerson.Name = "btnAddPerson";
            this.btnAddPerson.Size = new System.Drawing.Size(75, 44);
            this.btnAddPerson.TabIndex = 47;
            this.btnAddPerson.UseVisualStyleBackColor = true;
            this.btnAddPerson.Click += new System.EventHandler(this.btnAddPerson_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ctrlPersonCardWithFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ctrlPersonCard1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ctrlPersonCardWithFilter";
            this.Size = new System.Drawing.Size(1136, 434);
            this.Load += new System.EventHandler(this.ctrlPersonCardWithFilter_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlPersonCard ctrlPersonCard1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAddPerson;
        private System.Windows.Forms.Button btnSearchOnPerson;
        private System.Windows.Forms.TextBox txtFindPerson;
        private System.Windows.Forms.ComboBox cbFindPerson;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
