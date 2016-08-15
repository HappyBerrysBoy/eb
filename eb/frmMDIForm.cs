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
            frm.TopLevel = false;
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            frm.TopLevel = false;
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
