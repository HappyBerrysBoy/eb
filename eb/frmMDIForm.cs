using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb
{
    public partial class frmMDIForm : Form
    {
        public frmMDIForm()
        {
            InitializeComponent();
        }

        private void btnManager_Click(object sender, EventArgs e)
        {
            frmInterList frm = new frmInterList();
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
