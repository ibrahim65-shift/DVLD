using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Applications.Application_Types
{
    public partial class frmListApplicationTypes : Form
    {
        public frmListApplicationTypes()
        {
            InitializeComponent();
        }

        private void frmListApplicationTypes_Load(object sender, EventArgs e)
        {
            dgvListApplicationTypes.DataSource = clsApplicationType.GetAllApplicationTypes();
            lblRecord.Text = dgvListApplicationTypes.Rows.Count.ToString();

            if(dgvListApplicationTypes.Rows.Count > 0 )
            {
                dgvListApplicationTypes.Columns[0].HeaderText = "ID";
                dgvListApplicationTypes.Columns[1].HeaderText = "Title";
                dgvListApplicationTypes.Columns[2].HeaderText = "Fees";
                dgvListApplicationTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditApplicationType frm = new frmEditApplicationType((int)dgvListApplicationTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListApplicationTypes_Load(null, null);
        }
    }
}
